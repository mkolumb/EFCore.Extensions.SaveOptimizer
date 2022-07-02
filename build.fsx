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

Target.create "Clean" (fun _ -> !! "**/bin" ++ "**/obj" |> Shell.cleanDirs)

Target.create "SolutionBuild" (fun _ -> !! "**/*.sln" |> Seq.iter (DotNet.build id))

Target.create "InternalTests" (fun _ ->
    !! "**/*Internal*.Tests.csproj"
    |> Seq.iter (DotNet.test id))

Target.create "DbTest" (fun _ ->
    (!! "**/*.Tests.csproj"
     -- "**/*Internal*.Tests.csproj"
     -- "**/*Shared*.Tests.csproj")
    |> Seq.iter (DotNet.test id))

Target.create "All" ignore

"Clean"
==> "SolutionBuild"
==> "InternalTests"
==> "DbTest"
==> "All"

Target.runOrDefault "All"
