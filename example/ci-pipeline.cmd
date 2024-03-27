dotnet build || exit -1
dotnet test || exit -2
dotnet policy-compiler --s .\source\ --o .\target\ --format true || exit -3
az deployment group create --resource-group YOUR_RESOURCE_GROUP --template-file .\deployment.bicep --parameters servicename=YOUR_SERVICE_NAME --name deploy-1 || exit -4
