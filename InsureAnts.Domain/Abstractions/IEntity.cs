namespace InsureAnts.Domain.Abstractions;

public interface IEntity
{
    object? Id { get; }
}


public interface IEntity<out T> : IEntity
{
    new T? Id { get; }

    object? IEntity.Id => Id;
}