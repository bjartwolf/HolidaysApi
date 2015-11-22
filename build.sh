#!/bin/bash
if test "$OS" = "Windows_NT"
then

  .nuget/nuget.exe restore
  exit_code=$?
  if [ $exit_code -ne 0 ]; then
    exit $exit_code
  fi
  .nuget/NuGet.exe install FAKE -OutputDirectory tools -ExcludeVersion -Prerelease
  mono tools/FAKE/tools/FAKE.exe build.fsx $@
else
  # use mono
  mono .paket/paket.bootstrapper.exe
  mono .paket/paket.exe restore
  exit_code=$?
  if [ $exit_code -ne 0 ]; then
    exit $exit_code
  fi
  
  mono packages/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
fi
