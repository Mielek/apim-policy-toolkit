#! /bin/bash
set -e

dotnet build
dotnet pack
cd ./example
dotnet tool restore
