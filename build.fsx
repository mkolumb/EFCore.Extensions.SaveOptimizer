#r "paket:
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
nuget Fake.Core.Target //"
#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators
open System.Threading.Tasks

Target.initEnvironment ()

let scriptRun =
    fun (path: string) ->
        Trace.log ("Running script: " + path)

        Command.RawCommand("pwsh", Arguments.OfArgs [ "-File"; path ])
        |> CreateProcess.fromCommand
        |> Proc.run
        |> ignore

        Command.RawCommand(
            "docker",
            Arguments.OfArgs [ "volume"
                               "prune"
                               "--force" ]
        )
        |> CreateProcess.fromCommand
        |> Proc.run
        |> ignore

let testRun =
    fun (path: string) ->
        Trace.log ("Running test: " + path)

        let dir = Shell.pwd ()
        let settingsPath = Path.combine dir "github.runsettings"

        let testConfiguration (defaults: DotNet.TestOptions) =
            { defaults with Settings = Some(settingsPath) }

        DotNet.test testConfiguration path

let dbTestRun =
    fun (path: string) ->
        let dir =
            (FileInfo.ofPath (path.Replace("Tests", "Benchmark")))
                .Directory
                .FullName

        scriptRun (Path.combine dir "start.ps1")

        testRun (path)

        scriptRun (Path.combine dir "stop.ps1")

let buildRun =
    fun (path: string) ->
        Trace.log ("Running build: " + path)

        DotNet.build (fun o -> o) path

let packRun =
    fun (path: string) ->
        let version = Environment.environVar ("PACKAGE_VERSION")

        Trace.log ("Running pack: " + path + " - " + version)

        let setMsBuildParams (defaults: MSBuild.CliArguments) =
            { defaults with Properties = [ "PackageVersion", version ] }

        let packConfiguration (defaults: DotNet.PackOptions) =
            { defaults with
                Configuration = DotNet.Release
                OutputPath = Some "./packages"
                IncludeSymbols = false
                MSBuildParams = setMsBuildParams defaults.MSBuildParams }

        DotNet.pack packConfiguration path

let iterInParallel action (array: 'T []) =
    let options: ParallelOptions = new ParallelOptions()
    options.MaxDegreeOfParallelism <- 4

    Parallel.For(0, array.Length, options, (fun i -> action array.[i]))
    |> ignore

Target.create "Clean" (fun _ -> !! "**/bin" ++ "**/obj" |> Shell.cleanDirs)

Target.create "RemoveContainersBefore" (fun _ ->
    let dir = Shell.pwd ()
    let path = Path.combine dir "remove_containers.ps1"
    scriptRun (path))

Target.create "RemoveContainersAfter" (fun _ ->
    let dir = Shell.pwd ()
    let path = Path.combine dir "remove_containers.ps1"
    scriptRun (path))

Target.create "PullImages" (fun _ ->
    let dir = Shell.pwd ()
    let path = Path.combine dir "pull_images.ps1"
    scriptRun (path))

Target.create "SolutionBuild" (fun _ ->
    !! "**/*.sln"
    |> Seq.toArray
    |> iterInParallel (buildRun))

Target.create "InternalTests" (fun _ ->
    !! "**/*Internal*.Tests.csproj"
    |> Seq.toArray
    |> iterInParallel (testRun))

Target.create "DbTest" (fun _ ->
    (!! "**/*.Tests.csproj"
     -- "**/*Internal*.Tests.csproj"
     -- "**/*Shared*.Tests.csproj")
    |> Seq.toArray
    |> iterInParallel (dbTestRun))

Target.create "Pack" (fun _ ->
    (!! "**/EFCore.Extensions.SaveOptimizer.Internal.csproj"
     ++ "**/EFCore.Extensions.SaveOptimizer.Dapper.csproj"
     ++ "**/EFCore.Extensions.SaveOptimizer.csproj")
    |> Seq.iter (packRun))

Target.create "All" ignore

"RemoveContainersBefore"
==> "SolutionBuild"
==> "InternalTests"
==> "DbTest"
==> "RemoveContainersAfter"
==> "All"

"Clean" ==> "Pack"

"Clean" ==> "SolutionBuild"

"DbTest" ?=> "Pack"

Target.runOrDefault "All"
