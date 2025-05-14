using FluentValidation;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Web.Infrastructure;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InsureAnts.Web.Components.Shared;

internal static class UiHelper
{
    public static async Task<bool> AskConfirmation(this IDialogService dialogService, string title, string markupString)
    {
        MarkupString message = new(markupString);

        var parameters = new DialogParameters { { nameof(ConfirmDialog.Message), message } };

        var dialog = await dialogService.ShowAsync<ConfirmDialog>(title, parameters);

        var result = await dialog.Result;

        return !result.Canceled;
    }

    public static DialogParameters ToModelDialogParameters<T>(this T model) => new() { { "Model", model } };

    public static void AddConditional(this ISnackbar snackbar, IResponse response)
    {
        if (!response.IsSuccess)
        {
            var errorMessage = string.Join("<br/>", response.FormatMessages());
            snackbar.AddErrorMessage(errorMessage);
        }
        else
        {
            snackbar.Add(response.Message, MudBlazor.Severity.Success, o => o.VisibleStateDuration = 3500);
        }
    }

    public static void AddValidationError(this ISnackbar snackbar, ValidationException validationException)
    {
        var message = validationException.Message.Replace(Environment.NewLine, "<br/>");
        snackbar.AddErrorMessage(message);
    }

    public static void CloseOk<T>(this MudDialogInstance mudDialogInstance, T data) => mudDialogInstance.Close(DialogResult.Ok(data));

    public static void AddErrorMessage(this ISnackbar snackbar, string message) => snackbar.Add(message, MudBlazor.Severity.Error);
}