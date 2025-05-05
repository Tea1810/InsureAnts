namespace InsureAnts.Domain.Abstractions;

public abstract class Entity<T> : IEntity<T>
{
    public T? Id { get; set; }

    public override string ToString() => $"[{EntityNameAttribute.Get(GetType())} {Id}]";
}
