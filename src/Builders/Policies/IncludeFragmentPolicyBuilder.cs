using Mielek.Model.Policies;

namespace Mielek.Builders.Policies
{
    public class IncludeFragmentPolicyBuilder
    {

        string? _fragmentId;

        public IncludeFragmentPolicyBuilder FragmentId(string fragmentId)
        {
            _fragmentId = fragmentId;
            return this;
        }

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