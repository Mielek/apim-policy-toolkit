namespace Mielek.Builders.Policies
{
    using Mielek.Generators.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;


    [GenerateBuilderSetters]
    public partial class SetBodyPolicyBuilder
    {
        private IExpression<string>? _body;
        private IExpression<string>? _template;
        private IExpression<string>? _xsiNil;


        public SetBodyPolicyBuilder Template(BodyTemplate template)
        {
            return Template(config => config.Constant(TranslateTemplate(template)));
        }

        private string TranslateTemplate(BodyTemplate template) => template switch
        {
            BodyTemplate.Liquid => "liquid",
            _ => throw new Exception(),
        };

        public SetBodyPolicyBuilder XsiNil(XsiNilType xsiNil)
        {
            return XsiNil(config => config.Constant(TranslateXsiNil(xsiNil)));
        }

        private string TranslateXsiNil(XsiNilType xsiNil) => xsiNil switch
        {
            XsiNilType.Blank => "blank",
            XsiNilType.Null => "null",
            _ => throw new Exception(),
        };

        public SetBodyPolicy Build()
        {
            if (_body == null) throw new NullReferenceException();

            return new SetBodyPolicy(_body, _template, _xsiNil);
        }
    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder SetBody(Action<SetBodyPolicyBuilder> configurator)
        {
            var builder = new SetBodyPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}