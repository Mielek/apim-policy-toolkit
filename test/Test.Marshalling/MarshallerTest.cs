using Mielek.Builders;
using Mielek.Model.Policies;

namespace Mielek.Marshalling;

[TestClass]
public class MarshallerTest
{
    [TestMethod]
    public void PolicyDocument()
    {
        var document = PolicyDocumentBuilder
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
                            .Value(expression => expression.Constant("Test"));
                    })
                    .Base()
                    .SetHeader(policy =>
                    {
                        policy.Name("X-Test").ExistAction(ExistAction.Append)
                            .Value("Test")
                            .Value(expression => expression.Inlined("context.Deployment.Region"))
                            .Value(expression => expression.FromFile("../../../scripts/guid-time.csx"));
                    });
            })
            .Outbound(policies => policies.SetBody(policy => policy.Body(expression => expression.FromFile("../../../scripts/filter-body.csx"))))
            .Build();

        using (var marshaller = Marshaller.Create(Console.Out))
        {
            document.Accept(marshaller);
        }
        Console.Out.Flush();
    }

    [TestMethod]
    public void PolicyFragment()
    {
        var fragment = PolicyFragmentBuilder.Create()
            .Policies(policies =>
            {
                policies
                    .SetHeader(policy => policy.Name("X-Test")
                            .ExistAction(ExistAction.Append)
                            .Value(expression => expression.Constant("test")))
                    .SetMethod(policy => policy.Options())
                    .SetStatus(policy => policy.Code(222).Reason("My reason"))
                    .SetBody(policy => policy.Body(expression => expression.Constant("MyBody")).XsiNil(XsiNilType.Blank));
            }).Build();

        using (var marshaller = Marshaller.Create(Console.Out))
        {
            fragment.Accept(marshaller);
        }
        Console.Out.Flush();
    }
}