# Azure API Management policy toolkit

**Azure API management policy toolkit** is a set of libraries and tools for **Azure API Management** which target 
**policy document**. The toolkit was design to help **create** and **test** policy documents with complex expressions.

Before the Policy toolkit, policy documents were written in Razor format.
The Razor format is hard to read and understand, especially when there are a lot of expressions.
The feedback loop on new documents or event the smallest changes was very long.
It required live Azure API Management instance,
a policy document deployment and manual testing through the API request.

The policy toolkit changes that. It allows you to write policy documents in C# language.
The C# language is more natural and shows a process nature of policy.
There is as well, no need to jump between C# and XML for expression creation.
Creating policy documents in C# brings another advantage.
Simple C# code can test the policy document in the unit test.

## Documentation

#### Azure API Management policy toolkit documentation for users.
* [Quick start](docs/QuickStart.md)
* [Available policies](docs/AvailablePolicies.md)
* [Solution structure recommendation](docs/SolutionStructureRecommendation.md)
* [Steps for deploying policies created by the policy toolkit](docs/IntegratePolicySolution.md)
* [Integrate policy solution with APIOps](docs/IntegratePolicySolutionWithApiOps.md)

#### Azure API Management policy toolkit documentation for contributors.
* [Contributor guide](CONTRIBUTING.md)
* [Development environment setup](docs/DevEnvironmentSetup.md)
