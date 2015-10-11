#r @"packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Testing
RestorePackages()

open System.IO

let buildDir  = "./build/"
let testDir   = "./test/"

let appReferences  = !! "HolidayApi\*.fsproj"

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

"Clean"
  ==> "BuildApp"
  ==> "BuildTest"
  ==> "XUnitTest"

RunTargetOrDefault "XUnitTest"
