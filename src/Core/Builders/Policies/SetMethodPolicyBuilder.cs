namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OnErrorSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class SetMethodPolicyBuilder
{
    private IExpression<string>? _method;

    public SetMethodPolicyBuilder Get()
    {
        return Method(HttpMethod.Get);
    }
    public SetMethodPolicyBuilder Post()
    {
        return Method(HttpMethod.Post);
    }
    public SetMethodPolicyBuilder Head()
    {
        return Method(HttpMethod.Head);
    }
    public SetMethodPolicyBuilder Delete()
    {
        return Method(HttpMethod.Delete);
    }
    public SetMethodPolicyBuilder Put()
    {
        return Method(HttpMethod.Put);
    }
    public SetMethodPolicyBuilder Options()
    {
        return Method(HttpMethod.Options);
    }

    public SetMethodPolicyBuilder Method(HttpMethod method)
    {
        return Method(method.Method);
    }

    public XElement Build()
    {
        if (_method == null) throw new NullReferenceException();

        return new XElement("set-method", _method.GetXText());
    }
}