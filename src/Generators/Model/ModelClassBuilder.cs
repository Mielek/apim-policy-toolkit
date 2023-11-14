namespace Mielek.Generators.Model;

public class ModelClassBuilder
{
    private record Property(string Name, string Type);

    private readonly string name;
    private readonly List<Property> properties = new List<Property>();
    private bool addPolicyInterfaces = false;
    private readonly List<string> subClasses = new List<string>();

    public ModelClassBuilder(string name)
    {
        this.name = name;
    }

    public ModelClassBuilder WithProperty(string name, string type)
    {
        properties.Add(new(name, type));
        return this;
    }


    public ModelClassBuilder WithSubClass(string subClassBuilder)
    {
        subClasses.Add(subClassBuilder);
        return this;
    }

    public ModelClassBuilder WithAddPolicyInterfaces(bool addPolicyInterfaces)
    {
        this.addPolicyInterfaces = addPolicyInterfaces;
        return this;
    }

    public string Build()
    {
        var props = string.Join(", ", properties.Select(p => $"{p.Type} {p.Name}"));
        var subTypes = string.Join("\n", subClasses);
        var addNewLine = subTypes != string.Empty ? "\n" : "";
        var policyInterfaces = addPolicyInterfaces ? $", IPolicy" : "";
        return $"{subTypes}{addNewLine}public sealed record {name}({props}): Visitable<{name}>{policyInterfaces};";
    }

}