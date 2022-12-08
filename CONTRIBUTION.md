## Setup

1. Download and install [dotnet](https://dotnet.microsoft.com/en-us/download) according to instructions
2. Check that dotnet is available in CLI
3. Run `dotnet build` in main folder

## Example solution

How to use libraries and command line tool from the toolkit can be found in the [example project](example). It shows a working repository with policy documents created by csx scripts and policy expressions as well defined as csx scripts. The solution contains examples of policies with expressions and a test project.

Before running anything in the example solution please execute below steps.

1. Execute [setup steps](#setup)
2. (Optional) Clean nuget cache `dotnet nuget locals global-packages --clear`
3. Run `.\setup-example.cmd`

## Project structure

### Model

Model library contain classes representing structures in policy document, like fragments, policies and policy sections.
[Learn more](src/Model)

### Builders

Builders library contains classes which are responsible for creating object oriented definition of policy document.
[Learn more](src/Builders)

```csharp
PolicyDocumentBuilder
    .Create()
    .Inbound(policies =>
    {
        policies
            .CheckHeader(policy =>
            {
                policy.Name("X-Checked")
                    .FailedCheckCode(400)
                    .FailedCheckErrorMessage("Bad request")
                    .IgnoreCase(true)
                    .Value("Test")
                    .Value("Other-Test");
            })
            .Base()
            .SetHeader(policy =>
            {
                policy.Name("X-Test").ExistAction(ExistAction.Append)
                    .Value("Test")
                    .Value(expression => expression.Inlined("context.Deployment.Region"))
                    .Value(expression => expression.FromFile("./scripts/guid-time.csx"));
            });
    })
    .Outbound(policies => policies.Base().SetBody(policy => policy.Body(expression => expression.FromFile("./scripts/filter-body.csx"))))
    .Build();
```

### Marshalling

Marshalling library contains all the necessary classes to transform object model into XML policy.
[Learn more](src/Marshalling)

```csharp
var model = CreateSomePolicyDocument();
using(var marshaller = Marshaller.Create("output/path/to/file.xml"))
{
    model.Accept(marshaller);
}
```

### Expression.Context

Expression.Context project contains classes representing a context of policy expression.
[Learn more](src/Expressions/Context)

### Expression.Testing

Expression.Testing project is responsible for classes which help with testing policy expressions defining mock context. Id defines simple expression runner, mock context and set of assertions.
[Learn more](src/Expressions/Context)

```csharp
var expression = ExpressionProvider.LoadFromFile(ScriptPath("guid-time.csx"));
var context = new MockContext();
var result = await expression.Execute(context);
```

### Transformer

Transformer project is responsible for code of `policy-transformer` tool. Tool is responsible for creating XML representation of policy documents by running builder scripts.
[Learn more](src/Transformer)

```cmd
dotnet policy-transformer --s ./source --t ./target
```
