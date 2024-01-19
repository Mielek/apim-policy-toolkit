param servicename string

resource service 'Microsoft.ApiManagement/service@2023-03-01-preview' existing = {
  name: servicename
  scope: resourceGroup()
}

resource echoApi 'Microsoft.ApiManagement/service/apis@2023-03-01-preview' existing = {
  parent: service
  name: 'echo-api'
}

resource retrieveResource 'Microsoft.ApiManagement/service/apis/operations@2023-03-01-preview' existing = {
  parent: echoApi
  name: 'retrieve-resource'
}

resource retrieveResourcePolicy 'Microsoft.ApiManagement/service/apis/operations/policies@2023-03-01-preview' = {
  parent: retrieveResource
  name: 'policy'
  properties: {
    format: 'rawxml'
    value: loadTextContent('./target/${echoApi.name}.${retrieveResource.name}.xml', 'utf-8')
  }
}
