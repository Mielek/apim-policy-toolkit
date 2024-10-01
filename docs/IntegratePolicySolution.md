# Steps for deploying policies created by the policy toolkit

The Azure API Management policy toolkit targets the advance user who leverages a CI/CD pipeline to check and deploy
changes made to Azure API Management instance. The solution with policy documents can be easily integrated with any
infrastructure as a code repository containing data about Azure API Management instance like ARM templates, Bicep,
Terraform or APIOps.

In this tutorial we will show you what steps need to be added to the CI/CD pipeline to integrate the policy documents
created by the policy toolkit. We will execute them locally to understand what is happening under the hood.

## Prerequisites

### Infrastructure as a code repository

We assume that you have a policy document solution created, and you use a hierarchical approach for the project
structure. You can read about the hierarchical structure in
the [Policy documents repository and solution structure](./SolutionStructureRecomendation.md). The repository which we
will be talking about is looking like the one in the following schema:

```
.
├── policies.sln
├── .gitignore
├── .config
│   └── dotnet-tools.json
├── enviroments
│   └── dev
│       └── deployment.bicep
├── src
│   ├── src.csproj
│   └── apis
│       └── echo-api
│           └── ApiEchoApiPolicy.cs
└── test
    ├── test.csproj
    └── apis
        └── echo-api
            └── ApiEchoApiPolicyTests.cs
```

### Azure API Management instance

We assume that you have an Azure API Management instance created and that you have access to it. We assume that you have
an API created in the Azure API Management instance. The API is named `echo-api`. We assume that you can test the policy
by invoking some of the operations on the API.

### Azure CLI

In this tutorial we will use a bicep file for the deployment description. To be able to use the bicep file a Azure CLI
tool need to be available in the environment. You can download the Azure CLI from
the [official site](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli).

## Building and testing

The policy documents solution is standard .NET project. Because of that building and testing the solution is very easy.
You need to execute standard .NET commands to build and test the solution in root of the solution folder. These steps
are essential to know that the policy documents are correct and that they are not breaking the build.

#### Building the solution

```shell
dotnet build
```

#### Testing the solution

```shell
dotnet test
```

In your CI/CD pipeline this two command should be split into separate steps and if any of them fails the pipeline should
fail as well.

## Compiling the policy documents

After standard C# compilation and testing is done, the policy documents can be compiled by the policy toolkit compiler.
Because, the bicep file for deployment is in the `.\enviroments\dev\` folder, the compilation target folder should be
`.\enviroments\dev\` as well. This will allow easy referencing of the policy documents in the bicep file.

```shell
dotnet policy-compiler --s .\src\ --o .\enviroment\dev\
```

The command will produce the policy documents and the folder structure will look like this:

```
.
├── policies.sln
├── enviroments
│   └── dev
│       ├── deployment.bicep
│       └── apis
│           └── echo-api
│               └── policy.xml
├── src
│   ├── src.csproj
│   └── apis
│       └── echo-api
│           └── ApiEchoApiPolicy.cs
└── ...
```

Your CI/CD pipeline should as well fail if the compilation of the policy documents fails.

## Deploying the policy documents

The bicep file for the deployment is in the `.\enviroments\dev\` folder. The bicep file is referencing the service, the
API and the policy document. The bicep file should look like this:

```bicep
param servicename string

resource service 'Microsoft.ApiManagement/service@2023-03-01-preview' existing = {
  name: servicename
  scope: resourceGroup()
}

resource echoApi 'Microsoft.ApiManagement/service/apis@2023-03-01-preview' existing = {
  parent: service
  name: 'echo-api'
}

resource echoApiPolicy 'Microsoft.ApiManagement/service/apis/policies@2023-03-01-preview' = {
  parent: echoApi
  name: 'policy'
  properties: {
    format: 'rawxml'
    value: loadTextContent('./apis/${echoApi.name}/ApiEchoApiPolicy.xml', 'utf-8')
  }
}
```

Please notice that the `loadTextContent` function is used to load the content of the policy document. The content is
loaded from the file under following path `./apis/echo-api/ApiEchoApiPolicy.xml`.

The deployment can be done by the Azure CLI. The following command will deploy the policy document to the Azure API:

```shell
cd .\enviroments\dev\
az deployment group create \
  --resource-group <<YOUR_RESOURCE_GROUP>> \
  --template-file .\deployment.bicep \
  --parameters servicename=<<YOUR_SERVICE_NAME>> \
  --name deploy-1
```

## Conclusion

Now you know what steps are required to deploy the policy documents. You can replicate this steps in the CI/CD pipeline.

We papered a short guides how to integrate the policy documents solution with APIOps. You can read about it in the
[APIOps integration](./IntegratePolicySolutionWithApiOps.md) document.

