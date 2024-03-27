# Azure Api Management policy toolkit

**Azure api management policy toolkit** is a set of libraries and tools for **Azure Api Management** which target **policy document**. The toolkit was design to help **create** and **test** policy documents with complex expressions.

## Getting started

### Prerequisites

* Check that you have latest version [dotnet](https://dotnet.microsoft.com/download) installed.
* Clone repository to your local machine.
* Build the nuget packages by running `dotnet build` and then `dotnet pack` in the root folder of the repository.

| :exclamation: Currently the libraries and compiler is not available on nuget. To be able to skip the project setup we recommend to use `example` folder with example project from repository. The steps for that you can find (here)[#Setup example project]. |
|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

### Setting up the policy project

* Create a folder for project in your local machine.
* Run `dotnet new classlib` to create new class library project.
* Run `dotnet new nugetconfig` to create local nuget config file
* Run `dotnet nuget add source <path-to-repository>\output` to add local repository with build toolkit packages.
* Run `dotnet add package Mielek.Azure.ApiManagement.PolicyToolkit` to add policy authoring library to the project.
* Run `dotnet tool install Mielek.Azure.ApiManagement.PolicyToolkit.Compiler` to install policy compiler tool for the project.
* Open the folder with the project in your IDE of choice.

### Writing policy document

One policy document is one C# class in one `cs` file. Lets create a new `ApiOperationPolicy.cs` file in the project.

The class should inherit from `ICodeContext` interface from `Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext` and have `CodeDocument` attribute from `Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext.Attributes` namespace.

```csharp
using Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext;
using Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext.Attributes;

namespace Contoso.Apis;

[CodeDocument]
public class ApiOperationPolicy : ICodeDocument
{
    
}
```

The `ICodeContext` interface contains methods `Inbound`, `Outbound`, `Backend` and `OnError` which are used to define policy sections.

| :exclamation: Currently only `Inbound` and `Outbound` methods are supported. |
|------------------------------------------------------------------------------|

Lets implement `Inbound` method which will set header `X-Hello` with value `World`.

```csharp
[CodeDocument]
public class ApiOperationPolicy : ICodeDocument
{
    public void Inbound(IInboundContext c)
    {
        c.SetHeader("X-Hello", "World");
    }
}
```

Lets unpack the code above:
* `Inbound` accepts one parameter `IInboundContext`.
* `IInboundContext` contains methods to set headers, query parameters, set body and more. They are directly mapped to policies.
* `SetHeader` maps to `set-header` policy
*  Parameters `X-Hello` and `World` in `SetHeader` method are mapped to policy attributes `name` and `value` element.

Now lets run the compiler to generate policy document.

Execute `dotnet policy-compiler --s .\ --o .\` in the project folder. The compiler will generate `ApiOperationPolicy.xml` file in the project folder.

```xml
<policies><inbound><set-header name="X-Hello" exists-action="override"><value>World</value></set-header></inbound></policies>
```

The generated file is a policy document which can be imported to Azure Api Management. The document by default minified. To have a formatted document use `--format true` parameter.

Lets run the modified command `dotnet policy-compiler --s .\ --o .\ --format true`

```xml
<policies>
    <inbound>
        <set-header name="X-Hello" exists-action="override">
            <value>World</value>
        </set-header>
    </inbound>
</policies>
```

Lets now use write more complex policy document. Lets assume that we want to add authorization header to the request.
If requests comes from Company IP addresses `10.0.0.0/24` is should use basic authorization with `username` and `password` from named values.
If request comes from other IP addresses it should use `Bearer` token received from https://graph.microsoft.com.

```csharp
using Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext;
using Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext.Attributes;
using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

namespace Contoso.Apis;

[CodeDocument]
public class ApiOperationPolicy : ICodeDocument
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
    
    public bool IsCompanyIP(IContext context)
        => context.Request.IpAddress.StartsWith("10.0.0.");
}
```

The code above contains more complex logic. Lets unpack it:
* `if` statement is an equivalent of `choose` policy with `when` element.
* `IsCompanyIP` is a method checks if request comes from company IP addresses and It is used as an policy expression
* Every method other then section method are treated as expressions. They need to accept one parameter of name context with `IContext` type from `Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context` namespace.
* `IContext` type contains the same properties as `context` object in policy expressions.
* `AuthenticationBasic` method is mapped to `authentication-basic` policy.
* `AuthenticationManagedIdentity` method is mapped to `authentication-managed-identity` policy.
 
Lets now run the compiler `dotnet policy-compiler --s .\ --o .\ --format true`

```xml
<policies>
    <inbound>
        <choose>
            <when condition="@(context.Request.IpAddress.StartsWith("10.0.0."))">
                <authentication-basic username="{{username}}" password="{{password}}" />
            </when>
            <otherwise>
                <authentication-managed-identity resource="https://graph.microsoft.com"/>
            </otherwise>
        </choose>
    </inbound>
</policies>
```

Cool right? But what if we want to create a bearer token by ourselves? We can save the result of the `AuthenticationManagedIdentity` method to a variable and use it in the `SetHeader` method.

```csharp
[CodeDocument]
public class ApiOperationPolicy : ICodeDocument
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
    
    public bool IsCompanyIP(IContext context)
        => context.Request.IpAddress.StartsWith("10.0.0.");
}
```

The `$"Bearer {token}"` concatenation is an equivalent of `@($"Bearer {context.Variables["token"]}")` in policy expressions.

Lets rerun the compiler `dotnet policy-compiler --s .\ --o .\ --format true` to verify that.

```xml
<policies>
    <inbound>
        <choose>
            <when condition="@(context.Request.IpAddress.StartsWith("10.0.0."))">
                <authentication-basic username="{{username}}" password="{{password}}" />
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

### Setup example project

* Open this repository forlder in terminal.
* Run `setup-example.cmd` to setup example project or check manual project build below.
* Open `examples` folder in your IDE of choice (tested: VS, VS code, Raider).

### Manual example project build

* Run `dotnet build` to build the project.
* Run `dotnet pack` to create nuget package. Package will be created in `output` folder.
* Open `examples` folder in terminal.
* Execute `dotnet run` to run the example.

### 




