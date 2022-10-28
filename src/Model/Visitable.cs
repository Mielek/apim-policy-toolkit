namespace Mielek.Model;

public abstract record Visitable<T>: IVisitable where T : Visitable<T> {
    public void Accept(IVisitor visitor)
    {
        visitor.Visit<T>((T)this);
    }
}