dotnet build || exit -1
dotnet test || exit -2
dotnet policy-transformer-v2 --s .\source\ --o .\target\ --format true || exit -3
az deployment group create --resource-group rmielowski-current-wus2 --template-file .\deployment-v2.bicep --parameters servicename=rmielowski-current-premium --name deploy-1 || exit -4
