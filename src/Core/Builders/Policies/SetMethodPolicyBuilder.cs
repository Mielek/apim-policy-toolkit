using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OnErrorSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class SetMethodPolicyBuilder : BaseBuilder<SetMethodPolicyBuilder>
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
        if (_method == null) throw new PolicyValidationException("Method is required for SetMethod");
        var element = CreateElement("set-method");
        element.Add(_method.GetXText());
        return element;
    }
}