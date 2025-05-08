namespace InsureAnts.Application.Data_Queries;

public interface IQueryRequest<T>
{
    public int? Skip { get; }

    public int? Take { get; }

    IQueryable<T> ApplyFilter(IQueryable<T> source);

    IQueryable<T> ApplySort(IQueryable<T> source);

    IQueryable<T> ApplySkipTake(IQueryable<T> source);
}