namespace Mielek.Builders.Policies
{
    using System.Xml.Linq;

    using Mielek.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class IncludeFragmentPolicyBuilder
    {
        private string? _fragmentId;
        public XElement Build()
        {
            if (_fragmentId == null) throw new NullReferenceException();

            return new XElement("include-fragment", new XAttribute("fragment-id", _fragmentId));
        }
    }
}
namespace Mielek.Builders
{
    using Mielek.Builders.Policies;
    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder IncludeFragment(Action<IncludeFragmentPolicyBuilder> configurator)
        {
            var builder = new IncludeFragmentPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }

    }
}