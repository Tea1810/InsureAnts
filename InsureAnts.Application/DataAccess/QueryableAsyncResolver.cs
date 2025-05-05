using InsureAnts.Application.DataAccess.Interfaces;

namespace InsureAnts.Application.DataAccess;

public static class QueryableAsyncResolver
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static IQueryableAsyncResolver Instance { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}