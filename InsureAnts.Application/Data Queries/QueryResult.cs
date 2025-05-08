namespace InsureAnts.Application.Data_Queries;

public class QueryResult<T>
{
    public required int Total { get; set; }

    public required IReadOnlyList<T> Items { get; set; }
}