using Mielek.Generator.Attributes;
using Mielek.Model.Policies;

namespace Mielek.Builders.Policies
{
    
    [GenerateBuilderSetters]
    public partial class IncludeFragmentPolicyBuilder
    {
        private string? _fragmentId;
        public IncludeFragmentPolicy Build()
        {
            if (_fragmentId == null) throw new NullReferenceException();

            return new IncludeFragmentPolicy(_fragmentId);
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