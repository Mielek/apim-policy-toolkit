using Mielek.Model;
using Mielek.Model.Policies;

namespace Mielek.Builders;

public partial class PolicySectionBuilder {
    public PolicySectionBuilder Base() {
        sectionPolicies.Add(new BasePolicy());
        return this;
    }
}