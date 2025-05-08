using InsureAnts.Domain.Abstractions;

namespace InsureAnts.Application.Features.Abstractions;

public static class Texts
{
    public const string SuccessOp = "Operation completed successfully.";

    public static string FailureOp(long id) => $"[{id}] Operation failed due to an unexpected error.";

    public static string Created(string entityName, string name) => $"Successfully created {entityName} {name}.";

    public static string Created<T>(string name) => Created(EntityNameAttribute<T>.Name, name);

    public static string Updated(string entityName, string name) => $"Successfully updated {entityName} {name}.";

    public static string Updated<T>(string name) => Updated(EntityNameAttribute<T>.Name, name);

    public static string Deleted<TKey>(string entityName, TKey id) => $"Successfully deleted {entityName} with Id {id}.";

    public static string Deleted<T, TKey>(TKey id) => Deleted(EntityNameAttribute<T>.Name, id);

    public static string NotFound<TKey>(string entityName, TKey id) => $"Could not find {entityName} with Id {id}. It may have been deleted in the meantime.";

    public static string NotFound<T, TKey>(TKey id) => NotFound(EntityNameAttribute<T>.Name, id);

    public static string RemovedHeader(string header, string detailType) => $"Header {header} was removed.\nDetail Type {detailType} was affected by this update!";

    public static string NewHeader(string header) => $"Header {header} was added!";
}