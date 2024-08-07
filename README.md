# Azure Api Management policy toolkit

**Azure api management policy toolkit** is a set of libraries and tools for **Azure Api Management** which target 
**policy document**. The toolkit was design to help **create** and **test** policy documents with complex expressions.

## Getting started

### Prerequisites

* Check that you have latest version [dotnet](https://dotnet.microsoft.com/download) installed.
* Clone repository to your local machine.
* Build the nuget packages by running commands below in the root folder of the repository. The commands will build and pack the libraries into the `output` folder.
    ```shell
    dotnet build
    dotnet pack
    ```
* Setup the example solution by doing steps from [here](#setup-the-solution)

| :exclamation: Instead of setting up the solution we recommend using example folder from repository for the testing purposes. You can find the automated steps for setting example solution up [here](#setup-the-example-solution). |
|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

* Open the solution in your IDE of choice. We tested [VS](https://visualstudio.microsoft.com), [Raider](https://www.jetbrains.com/rider/), [VS code](https://code.visualstudio.com/) with [C# devkit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit), but any IDE with C# support should work.

## Writing simple policy document

In this section we will describe how to create a simple policy document.

### Writing the document

One policy document is one C# class in one `cs` file. Lets create a new `ApiOperationPolicy.cs` file in the project
which will be our policy document.

The class in the file should inherit from `IDocument` interface and have `Document` attribute
from `Mielek.Azure.ApiManagement.PolicyToolkit.Authoring`.

```csharp
using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Contoso.Apis;

[Document]
public class ApiOperationPolicy : IDocument
{
    
}
```

The `IDocument` type contains methods `Inbound`, `Outbound`, `Backend` and `OnError` which are used to define policy
sections.

| :exclamation: Currently only `Inbound` and `Outbound` methods are supported. |
|------------------------------------------------------------------------------|

Lets implement `Inbound` method. In the method lets add the policy to set header `X-Hello` to value `World`.

```csharp
[Document]
public class ApiOperationPolicy : IDocument
{
    public void Inbound(IInboundContext c)
    {
        c.SetHeader("X-Hello", "World");
    }
}
```

Lets unpack the code above:

* `Inbound` method accepts one parameter `IInboundContext c`.
* `IInboundContext` contains methods which are directly mapped to policies.
* `SetHeader` maps to `set-header` policy
* Parameters `X-Hello` and `World` in `SetHeader` method are mapped to policy's `name` attribute and `value` element.

Now lets run the compiler to generate policy document.

Execute compiler command in the project folder.

```shell
dotnet policy-compiler --s .\source --o .
``` 

The compiler is a dotnet tool named `policy-compiler`. The `--s` parameter is a source folder with policy documents.
The `--o` parameter is an output folder for generated policy documents.

The compiler will generate `ApiOperationPolicy.xml` file in the project folder. Generated file should have the following
content:

```razor
<policies>
    <inbound>
        <set-header name="X-Hello" exists-action="override">
            <value>World</value>
        </set-header>
    </inbound>
</policies>
```

The generated file is a policy document which can be imported to Azure Api Management. The document by default is
minified. To have a formatted document use we can add `--format true` parameter to the command.

Lets run the modified command:

```shell
dotnet policy-compiler --s .\source --o . --format true
```

The generated file should now change to have the following content:

```razor
<policies>
    <inbound>
        <set-header name="X-Hello" exists-action="override">
            <value>World</value>
        </set-header>
    </inbound>
</policies>
```

## Writing more complex policy document

Lets now use write more complex policy document. Lets assume that we want to add authorization header to the request.
If requests comes from Company IP addresses `10.0.0.0/24` is should use basic authorization with `username`
and `password` from named values.
If request comes from other IP addresses it should use `Bearer` token received from https://graph.microsoft.com.

```csharp
using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;
using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Contoso.Apis;

[Document]
public class ApiOperationPolicy : IDocument
{
    public void Inbound(IInboundContext c)
    {
        if(IsCompanyIP(c.Context))
        {
            c.AuthenticationBasic("{{username}}", "{{password}}");
        }
        else
        {
            c.AuthenticationManagedIdentity("https://graph.microsoft.com");
        }
    }
    
    public bool IsCompanyIP(IExpressionContext context)
        => context.Request.IpAddress.StartsWith("10.0.0.");
}
```

Lets unpack the code above it:

* `if` statement is an equivalent of `choose` policy with `when` element.
* `IsCompanyIP` is a method which checks if request comes from company IP addresses and it is mapped to a policy
  expression
* Every method other then section method are treated as expressions. They need to accept one parameter of name context
  with `IExpressionContext` type from `Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context` namespace.
* `IExpressionContext` type contains the same properties as `context` object in policy expressions.
* `AuthenticationBasic` method is mapped to `authentication-basic` policy.
* `AuthenticationManagedIdentity` method is mapped to `authentication-managed-identity` policy.

Lets now run the compiler

```shell
dotnet policy-compiler --s .\source --o . --format true
```

Content of the generated file should be:

```razor
<policies>
    <inbound>
        <choose>
            <when condition="@(context.Request.IpAddress.StartsWith("10.0.0."))">
                <authentication-basic username="{{username}}" password="{{password}}"/>
            </when>
            <otherwise>
                <authentication-managed-identity resource="https://graph.microsoft.com"/>
            </otherwise>
        </choose>
    </inbound>
</policies>
```

Cool right? But what if we want to create a bearer token by ourselves?
We can save the result of the `AuthenticationManagedIdentity` method to a variable and use it in the `SetHeader` method.

```csharp
[Document]
public class ApiOperationPolicy : IDocument
{
    public void Inbound(IInboundContext c)
    {
        if(IsCompanyIP(c.Context))
        {
            c.AuthenticationBasic("{{username}}", "{{password}}");
        }
        else
        {
            var token = c.AuthenticationManagedIdentity("https://graph.microsoft.com");
            c.SetHeader("Authorization", $"Bearer {token}");
        }
    }
    
    public bool IsCompanyIP(IExpressionContext context)
        => context.Request.IpAddress.StartsWith("10.0.0.");
}
```

The `$"Bearer {token}"` concatenation is an equivalent of `@($"Bearer {context.Variables["token"]}")` in policy
expressions.

Lets rerun the compiler again to verify that.

```shell
dotnet policy-compiler --s .\ --o . --format true
```

Content of the generated file change to:

```razor
<policies>
    <inbound>
        <choose>
            <when condition="@(context.Request.IpAddress.StartsWith("10.0.0."))">
                <authentication-basic username="{{username}}" password="{{password}}"/>
            </when>
            <otherwise>
                <authentication-managed-identity resource="https://graph.microsoft.com" output-token-variable-name="token"/>
                <set-header name="Authorization">
                    <value>@($"Bearer {context.Variables["token"]}")</value>
                </set-header>
            </otherwise>
        </choose>
    </inbound>
</policies>
```

## Testing policy expressions

In this section we will write a simple test for the expression from the previous section.

### Writing the test

Lets create a new `ApiOperationPolicyTests.cs` file in the test project which will be our test class.
In the test class we need to add reference to the `Mielek.Azure.ApiManagement.PolicyToolkit.Emulator.Expressions`
namespace.
In this namespace the `MockExpressionContext` class is available which is a mock of the `IExpressionContext` interface.

Lets write a test for the `IsCompanyIP` method.

```csharp
using Contoso.Apis;
using Mielek.Azure.ApiManagement.PolicyToolkit.Emulator.Expressions;

namespace Contoso.Test.Apis;

[TestClass]
public class ApiOperationPolicyTest
{
    [TestMethod]
    public void TestIsCompanyIP()
    {
        var context = new MockExpressionContext();
        var document = new ApiOperationPolicy();

        context.Request.IpAddress = "10.0.0.1";
        Assert.IsTrue(document.IsCompanyIP(context));

        context.Request.IpAddress = "10.0.1.1";
        Assert.IsFalse(document.IsCompanyIP(context));
    }
}
```

The above code is standard mstest test. We can run test by executing the following command in the test project folder.

```shell
dotnet test
```


## Setup the solution

Before we start to look into the libraries and tools we need to setup the solution.

* Create a folder for solution in your local machine.
* Open your terminal in the created folder.
* Run command to create new solution.
    ```shell
    dotnet new sln
    ```
* Run command to create local nuget config file
    ```shell
    dotnet new nugetconfig
    ```
* Run command to add local repository with build toolkit packages.
    ```shell
    dotnet nuget add source <path-to-repository>\output
    ```
* Run command to install policy compiler tool for the solution.
    ```shell
    dotnet tool install Mielek.Azure.ApiManagement.PolicyToolkit.Compiler
    ```

### Setting up the policy authoring project

* Create a `source` folder for project in the solution folder.
* Open your terminal in the created folder.
* Run command to create new class library project.
    ```shell
    dotnet new classlib
    ```
* Run command to add policy authoring library to the project.
    ```shell
    dotnet add package Mielek.Azure.ApiManagement.PolicyToolkit
    ```

### Setting up the test project

* Create a `test` folder for project in the solution folder.
* Open your terminal in the created folder.
* Run command to create new mstest project.
    ```shell
    dotnet new mstest
    ```
* Run command to add policy testing library to the project.
    ```shell
    dotnet add package Mielek.Azure.ApiManagement.PolicyToolkit.Testing
    ```
* Run command to add reference to the project with policy document.
    ```shell
    dotnet add reference ../source
    ```

## Setup the example solution

* Open this repository folder in terminal.
* Run script to setup example solution or check [manual](#manual-example-project-build) project build.
    ```shell
    setup-example.cmd
    ```
* Open `examples` folder in your IDE of choice (tested: VS, VS code, Raider).

### Manual example project build

* Run command to build the project.
    ```shell
    dotnet build
    ```
* Run command to create nuget package. Package will be created in `output` folder.
    ```shell
    dotnet pack
    ```
* Open `examples` folder in terminal.
* Run command to create new solution.
    ```shell
    dotnet new sln
    ```
