namespace InsureAnts.Common;

public static class Tasks
{
    public static ValueTask<T> ToValueTask<T>(this Task<T> task)
    {
        return task.IsCompletedSuccessfully ? new ValueTask<T>(task.Result) : new ValueTask<T>(task);
    }
}