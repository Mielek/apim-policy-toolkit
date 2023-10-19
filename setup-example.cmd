dotnet nuget locals global-packages --clear
dotnet pack
cd ./example
./install-tool.cmd
