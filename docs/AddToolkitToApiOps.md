# Add policy solution folder to repository containing data for APIOps

Azure API Management policy toolkit is very elastic and allow a developer to define how the structure of repository will
look. In this guide we will touch how toolkit project can produce policies which can be easily published
by [APIOps tool](https://azure.github.io/apiops) to Azure API Management instance.

Repository with Azure API Management data created by APIOps tool should have structure similar to below folder tree.
We included folders and files about policies in the repository.

```
.
└── api-ops-data
    ├── policy.xml
    ├── apis
    │   └── echo-api
    │       ├── policy.xml
    │       └── operations
    │           ├── get
    │           │   └── policy.xml
    │           └── post
    │               └── policy.xml
    └── products
        └── Unlimited
            └── policy.xml
```

The compiler is able to generate policies in the above structure. There are two ways to specify the path to the policy
file.

## By using Document attribute

Document attribute can be used to specify the path to the policy file. The path is relative to the output folder
provided to policy toolkit compiler. In below listing you can find example of the Document attribute with path.

```csharp
[Document("apis/echo-api/operations/get/policy.xml")]
public class ApiEchoApiGetPolicy : IDocument 
{
    // ...
}
```

Folder structure for the project should look like below:

```
.
├── api-ops-data
│    ├── policy.xml
│    ├── apis
│    │   └── echo-api
│    │       ├── policy.xml
│    │       └── operations
│    │           ├── get
│    │           │   └── policy.xml
│    │           └── post
│    │               └── policy.xml
│    └── products
│        └── Unlimited
│            └── policy.xml
└── policies
    ├── policies.sln
    ├── .gitignore
    ├── src
    │   ├── src.csproj
    │   ├── GlobalPolicy.cs
    │   ├── ApiEchoApiPolicy.cs
    │   ├── ApiEchoApiGetPolicy.cs
    │   ├── ApiEchoApiPostPolicy.cs
    │   └── ProductUnlimitedPolicy.cs
    └── test 
        ├── test.csproj
        ...
```

If you ran the compiler with the following command:

```shell
dotnet policy-compiler --s $PATH_TO_POLICY_PROJECT\src\ --o $PATH_TO_POLICY_PROJECT\api-ops-data\
```

Compiler with information from above will produce a file in the following location:
`$PATH_TO_POLICY_PROJECT\api-ops-data\apis\echo-api\operations\get\policy.xml`

## Mimic folder structure

In the policy project you can mimic the folder structure of the APIOps repository. This way you can easily find the
policy file in the policy project.

```
.
├── api-ops-data
│    ├── policy.xml
│    ├── apis
│    │   └── echo-api
│    │       ├── policy.xml
│    │       └── operations
│    │           ├── get
│    │           │   └── policy.xml
│    │           └── post
│    │               └── policy.xml
│    └── products
│        └── Unlimited
│            └── policy.xml
└── policies
    ├── policies.sln
    ├── .gitignore
    ├── src
    │   ├── src.csproj
    │   ├── apis
    │   │   └── echo-api
    │   │       ├── ApiEchoApiPolicy.cs
    │   │       └── operations
    │   │           ├── get
    │   │           │   └── ApiEchoApiGetPolicy.cs
    │   │           └── post
    │   │               └── ApiEchoApiPostPolicy.cs
    │   └── products
    │       └── Unlimited
    │           └── ProductUnlimitedPolicy.cs
    └── test 
        ├── test.csproj
        ...
```

Running the compiler with will replace the policy files in the `api-ops-data` folder.

```shell
dotnet policy-compiler --s $DIRECTORY_PATH\policies\src\ --o $DIRECTORY_PATH\api-ops-data\
```

Remember that compiler by default produces file names equal to the name of a class when name is not provided in Document attribute.
To make sure that `policy.xml` file is produce for each C# policy class make sure to set `policy.xml` as Document name.

```csharp
[Document("policy.xml")]
public class ApiEchoApiGetPolicy : IDocument 
{
    // ...
}
```
