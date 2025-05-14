using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;

namespace InsureAnts.Web.Infrastructure;

public static class JsRuntimeExtensions
{
    public static async ValueTask InvokeIgnoreCancellationAsync(this IJSRuntime jsRuntime, string identifier, params object?[] args)
    {
        try
        {
            await jsRuntime.InvokeAsync<IJSVoidResult>(identifier, args);
        }
        catch (JSDisconnectedException)
        {
            // In case JS was disconnected we can ignore the code execution
        }
        catch (TaskCanceledException)
        {
            // In case the Task was disconnected we can ignore the code execution
        }
    }
}