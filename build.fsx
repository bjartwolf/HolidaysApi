#r @"packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Testing

open System.IO

let buildDir  = "./build/"
let testDir   = "./test/"
let iisDir = "./iisApp/"
let aspNetDir = "./aspNetApp/"

let appReferences  = !! "HolidaysApi.Server\*.fsproj"

let testReferences = !! "HolidaysApi.Tests\*.fsproj"

Target "Clean" (fun _ -> 
    CleanDirs [buildDir; testDir]
)

Target "BuildApp" (fun _ ->
    MSBuildRelease buildDir "Build" appReferences
        |> Log "AppBuild-Output: "
)

Target "BuildIis" (fun _ ->
    MSBuildRelease iisDir "Build" !! "HolidaysApi.IIS\*.fsproj"
        |> Log "AppBuild-Output: "
)

Target "BuildAspnet" (fun _ ->
    MSBuildRelease iisDir "Build" !! "HolidaysApi.AspNet\*.csproj"
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
    CopyFiles buildDir !!"./test/FSharp.Core.*"
    CopyFiles iisDir !!"./test/FSharp.Core.*"
    CopyFiles aspNetDir !!"./test/FSharp.Core.*")

Target "Docker" (fun _ ->  
        let errorcode = Shell.Exec("docker", "build .")
        trace (sprintf "Docker image %i" errorcode)
)

"Clean"
  ==> "BuildApp"
  ==> "BuildIis"
  ==> "BuildAspNet"
  ==> "BuildTest"
  ==> "XUnitTest"
  ==> "VersionHack"
  ==> "Docker"

RunTargetOrDefault "VersionHack"
