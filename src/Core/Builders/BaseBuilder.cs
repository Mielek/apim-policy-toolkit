using System.Xml.Linq;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders;

public abstract class BaseBuilder<T> where T : BaseBuilder<T>
{
    private string? _id;
    
    public T Id(string id)
    {
        _id = id;
        return (T)this;
    }

    protected XElement CreateElement(string name)
    {
        var element = new XElement(name);
        if(_id != null) element.Add(new XAttribute("id", _id));
        return element;
    }
}