# Policy documents repository and solution structure

The Azure API Management policy toolkit is an elastic solution which can accomodate almost any repository, solution or
project structure. It is up to the developer to decide how the structure of the repository will look like and how it will
be incorporated with CI/CD pipelines. In this guide we will cover two structures which are recommended by the toolkit
team.

* Flat structure
* Hierarchical structure

Both of the structures are valid and can be used in the project. The choice of the structure depends on a team
preference.

## Flat structure

```
.
├── policies.sln
├── .gitignore
├── .config
│   └── dotnet-tools.json
├── src
│   ├── src.csproj
│   ├── ApiEchoApiGetPolicy.cs
│   ├── ApiEchoApiPolicy.cs
│   ├── ApiEchoApiPostPolicy.cs
│   ├── GlobalPolicy.cs
│   ├── ProductStarterPolicy.cs
│   └── ProductUnlimitedPolicy.cs
└── test 
    ├── test.csproj
    ├── ApiEchoApiGetPolicyTests.cs
    ├── ApiEchoApiPolicyTests.cs
    ├── ApiEchoApiPostPolicyTests.cs
    ├── GlobalPolicyTests.cs
    ├── ProductStarterPolicyTests.cs
    └── ProductUnlimitedPolicyTests.cs
```

Flat structure is a simple and easy to understand solution structure. In this structure, root folder contains a solution
file for `src` and `test`projects. All policies are placed in the `src` folder for the `src` project and all tests are
placed in the `test` folder for the `test` project.

For this structure, executing the build, test and compilation commands is easy and compilation will produce the policy
documents as a flat structure. For example, the following command will compile the policies and place them in
the `target` folder:

```shell
dotnet policy-compiler --s .\src\ --o .\target\
```

Target folder after the compilation will look like this:

```
.
└── target
    ├── ApiEchoApiGetPolicy.xml
    ├── ApiEchoApiPolicy.xml
    ├── ApiEchoApiPostPolicy.xml
    ├── GlobalPolicy.xml
    ├── ProductStarterPolicy.xml
    └── ProductUnlimitedPolicy.xml
```

| :page_facing_up: We assumed that every file contains only one policy document class, names are the same as file names and `Document` attribute does not define its own name. |
|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

### Produce hierarchical target structure

The `Document` attribute allows naming the document. It as well allows to specify a directory tree in which documents
need to be placed. Because of that, the compiler can produce a hierarchical structure.

For example, to reproduce the hierarchical structure from the hierarchical example, the following `Document` attributes
can be used:

* For API policy:
    ```csharp
    [Document("apis/echo-api/policy.xml")]
    public class ApiEchoApiPolicy : IDocument 
    // ...
    ```
* For API operation policy:
    ```csharp
    [Document("apis/echo-api/operations/get/policy.xml")]
    public class ApiEchoApiGetPolicy : IDocument 
    // ...
    ```
* For product policy:
    ```csharp
    [Document("products/Starter/policy.xml")]
    public class ProductStarterPolicy : IDocument 
    // ...
    ```

## Hierarchical structure

```
.
├── policies.sln
├── .gitignore
├── .config
│   └── dotnet-tools.json
├── src
│   ├── src.csproj
│   ├── GlobalPolicy.cs
│   ├── apis
│   │   └── echo-api
│   │       ├── ApiEchoApiPolicy.cs
│   │       └── operations
│   │           ├── get
│   │           │   └── ApiEchoApiGetPolicy.cs
│   │           └── post
│   │               └── ApiEchoApiPostPolicy.cs
│   └── products
│       ├── Starter
│       │   └── ProductStarterPolicy.cs
│       └── Unlimited
│           └── ProductUnlimitedPolicy.cs
└── test
    ├── test.csproj
    ├── GlobalPolicyTests.cs
    ├── apis
    │   └── echo-api
    │       ├── ApiEchoApiPolicyTests.cs
    │       └── operations
    │           ├── get
    │           │   └── ApiEchoApiGetPolicyTests.cs
    │           └── post
    │               └── ApiEchoApiPostPolicyTests.cs
    └── products
        ├── Starter
        │   └── ProductStarterPolicyTests.cs
        └── Unlimited
            └── ProductUnlimitedPolicyTests.cs
```

Hierarchical structure leverages folders to represents the entities which contain policies. This structure corresponds
to ARM structure of resources in Azure API Management. In this structure, policies are placed in the folders which
represent the entities in the API Management instance.

For this structure, executing the build, test and compilation commands is easy. Compilation will produce the policy
documents in the same structure as is present under `src` folder. For example, the following command will compile the
policies and place them in the `target` folder:

```shell
dotnet policy-compiler --s .\src\ --o .\target\
```

```
.
├── GlobalPolicy.xml
├── apis
│   └── echo-api
│       ├── ApiEchoApiPolicy.xml
│       └── operations
│           ├── get
│           │   └── ApiEchoApiGetPolicy.xml
│           └── post
│               └── ApiEchoApiPostPolicy.xml
└── products
    ├── Starter
    │   └── ProductStarterPolicy.xml
    └── Unlimited
        └── ProductUnlimitedPolicy.xml
```

We recommend to only use a name functionality of the `Document` attribute with some common name, like `policy.xml` to
name the files the same in every folder. For example the `Document` attribute should look like this for every policy:

```csharp
[Document("policy.xml")]
```

Then the compiler will produce the files with the same name in every folder, like in below example.

```
.
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
    ├── Starter
    │   └── policy.xml
    └── Unlimited
        └── policy.xml
```

