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