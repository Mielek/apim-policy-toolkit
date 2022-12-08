return PolicyDocumentBuilder
    .Create()
    .Inbound(policies =>
    {
        policies
            .CheckHeader(policy => 
            {
                policy.Name("X-Checked")
                    .FailedCheckHttpCode(400)
                    .FailedCheckErrorMessage("Bad request")
                    .IgnoreCase(expression => expression.FromFunctionFile("./expressions/simple-expressions-library.csx", "IsVariableSet"))
                    .Value("Test")
                    .Value("Other-Test");
            })
            .Base()
            .SetHeader(policy =>
            {
                policy.Name("X-Test").ExistAction(ExistAction.Append)
                    .Value("Test")
                    .Value(expression => expression.Inlined(context => context.Deployment.Region))
                    .Value(expression => expression.FromFile("./expressions/guid-time.csx"))
                    .Value(expression => expression.FromFunctionFile("./expressions/simple-expressions-library.csx", "GetKnownGUIDOrGenerateNew"));
            });
    })
    .Outbound(policies => policies.Base().SetBody(policy => policy.Body(expression => expression.FromFile("./expressions/filter-body.csx"))))
    .Build();