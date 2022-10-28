namespace Mielek.Model;

public interface IVisitable
{
    void Accept(IVisitor visitor);
}