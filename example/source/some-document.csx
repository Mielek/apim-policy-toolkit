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
                    .Value(expression => expression.FromFile("./expressions/guid-time.csx"));
            });
    })
    .Outbound(policies => policies.Base().SetBody(policy => policy.Body(expression => expression.FromFile("./expressions/filter-body.csx"))))
    .Build();