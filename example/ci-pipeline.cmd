dotnet build || exit -1
dotnet test || exit -2
dotnet policy-transformer --dllFile  ./source/bin/Debug/.net7/Source.Example.dll --out ./target --format true || exit -3
az deployment group create --resource-group rmielowski-current-wus2 --template-file .\deployment.bicep --parameters servicename=rmielowski-current-premium --name deploy-1 || exit -4
