using System.Reflection;

namespace InsureAnts.Domain.Abstractions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class EntityNameAttribute : Attribute
{
    public string Name { get; }

    public EntityNameAttribute(string name)
    {
        Name = name;
    }

    public static string Get(Type type)
    {
        var typeInfo = type.GetTypeInfo();

        return typeInfo.GetCustomAttribute<EntityNameAttribute>()?.Name ?? typeInfo.Name;
    }
}

public static class EntityNameAttribute<T>
{
    public static readonly string Name = Get();

    private static string Get() => EntityNameAttribute.Get(typeof(T));
}