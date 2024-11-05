namespace Azure.ApiManagement.PolicyToolkit.Authoring.Implementations;

public interface IUrlContentEncoder
{
    string? Encode(IDictionary<string, IList<string>>? dictionary);
}