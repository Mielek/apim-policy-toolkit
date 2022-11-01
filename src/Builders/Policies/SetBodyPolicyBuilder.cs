namespace Mielek.Builders.Policies
{
    using Mielek.Builders.Expressions;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    public class SetBodyPolicyBuilder
    {
        IExpression? _body;
        IExpression? _template;
        IExpression? _xsiNil;

        public SetBodyPolicyBuilder Body(string text)
        {
            return Body(config => config.Constant(text));
        }
        
        public SetBodyPolicyBuilder Body(Action<ExpressionBuilder> configurator)
        {
            _body = ExpressionBuilder.BuildFromConfiguration(configurator);
            return this;
        }

        public SetBodyPolicyBuilder Template(Action<ExpressionBuilder> configurator)
        {
            _template = ExpressionBuilder.BuildFromConfiguration(configurator);
            return this;
        }

        public SetBodyPolicyBuilder Template(BodyTemplate template)
        {
            return Template(config => config.Constant(TranslateTemplate(template)));
        }

        private string TranslateTemplate(BodyTemplate template) => template switch
        {
            BodyTemplate.Liquid => "liquid",
            _ => throw new Exception(),
        };

        public SetBodyPolicyBuilder XsiNil(Action<ExpressionBuilder> configurator)
        {
            _xsiNil = ExpressionBuilder.BuildFromConfiguration(configurator);
            return this;
        }

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
            this.sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}
