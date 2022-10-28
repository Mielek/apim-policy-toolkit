return PolicyFragmentBuilder.Create()
    .Policies(policies =>
    {
        policies
            .SetHeader(policy => policy.Name("X-Test").ExistAction(ExistAction.Append).Value("test"))
            .SetMethod(policy => policy.Options())
            .SetStatus(policy => policy.Code(404).Reason("My reason"))
            .SetBody(policy => policy.Body("MyBody").XsiNil(XsiNilType.Blank));
    }).Build();