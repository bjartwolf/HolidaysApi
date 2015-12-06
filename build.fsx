#r @"packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Testing

open System.IO

let buildDir  = "./build/"
let testDir   = "./test/"

let appReferences  = !! "HolidaysApi.Server\*.fsproj"

let testReferences = !! "HolidaysApi.Tests\*.fsproj"

Target "Clean" (fun _ -> 
    CleanDirs [buildDir; testDir]
)

Target "BuildApp" (fun _ ->
    MSBuildRelease buildDir "Build" appReferences
        |> Log "AppBuild-Output: "
)

Target "BuildTest" (fun _ ->
    MSBuildDebug testDir "Build" testReferences
        |> Log "TestBuild-Output: "
)

Target "XUnitTest" (fun _ ->  
    !! (testDir + "/*Tests.dll")
        |> xUnit2 (fun p -> p))

Target "VersionHack" (fun _ ->
    CopyFile "./test/FSharp.Core.dll" buildDir)

Target "Docker" (fun _ ->  
        let errorcode = Shell.Exec("docker", "build .")
        trace (sprintf "Docker image %i" errorcode)
)

"Clean"
  ==> "BuildApp"
  ==> "BuildTest"
  ==> "XUnitTest"
  ==> "VersionHack"
  ==> "Docker"

RunTargetOrDefault "VersionHack"
