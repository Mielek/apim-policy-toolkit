namespace Mielek.Model;

public interface IVisitor
{
    void Visit<T>(T obj) where T : IVisitable;
}