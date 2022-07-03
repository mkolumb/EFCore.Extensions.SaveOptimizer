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

Target.initEnvironment ()

let scriptRun =
    fun path ->
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

        Command.RawCommand(
            "docker",
            Arguments.OfArgs [ "image"
                               "prune"
                               "--all"
                               "--force" ]
        )
        |> CreateProcess.fromCommand
        |> Proc.run
        |> ignore

let testRun =
    fun path ->
        Trace.log ("Running test: " + path)

        DotNet.test (fun o -> o) path

let buildRun =
    fun path ->
        Trace.log ("Running build: " + path)

        DotNet.build (fun o -> o) path

let packRun =
    fun path ->
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

Target.create "Clean" (fun _ -> !! "**/bin" ++ "**/obj" |> Shell.cleanDirs)

Target.create "RemoveContainersBefore" (fun _ ->
    let dir = Shell.pwd ()
    let path = Path.combine dir "remove_containers.ps1"
    scriptRun (path))

Target.create "RemoveContainersAfter" (fun _ ->
    let dir = Shell.pwd ()
    let path = Path.combine dir "remove_containers.ps1"
    scriptRun (path))

Target.create "SolutionBuild" (fun _ -> !! "**/*.sln" |> Seq.iter (buildRun))

Target.create "InternalTests" (fun _ ->
    !! "**/*Internal*.Tests.csproj"
    |> Seq.iter (testRun))

Target.create "DbTest" (fun _ ->
    (!! "**/*.Tests.csproj"
     -- "**/*Internal*.Tests.csproj"
     -- "**/*Shared*.Tests.csproj")
    |> Seq.iter (fun job ->
        let dir =
            (FileInfo.ofPath (job.Replace("Tests", "Benchmark")))
                .Directory
                .FullName

        scriptRun (Path.combine dir "start.ps1")

        testRun (job)

        scriptRun (Path.combine dir "stop.ps1")))

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

"Pack" ==> "All"

"Clean" ==> "Pack"

"Clean" ==> "SolutionBuild"

"DbTest" ?=> "Pack"

Target.runOrDefault "All"
