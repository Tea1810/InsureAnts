using FluentValidation;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Web.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InsureAnts.Web.Components.Shared;

public abstract class AppDialogAddFormComponent<TCommand, TModel> : ComponentBase
    where TCommand : ICommand<IResponse<TModel>>, new()
{
    #region Properties
    [Inject]
    protected IBlazorMediator Mediator { get; set; }

    [Inject]
    protected ISnackbar Snackbar { get; set; }

    protected MudForm Form { get; set; }


    [CascadingParameter]
    protected MudDialogInstance MudDialog { get; set; }

    protected TCommand Command { get; } = new();
    #endregion


    #region Protected methods
    protected Task<bool> Submit()
    {
        return Submit(autoClose: true);
    }

    protected async Task<bool> Submit(bool autoClose)
    {
        await Form.Validate();

        if (Form.IsValid)
        {
            OnSendCommand();

            try
            {
                var response = await Mediator.Send(Command);

                Snackbar.AddConditional(response);

                if (response.IsSuccess)
                {
                    await OnSubmitSuccessAsync(response.Data!);
                }

                if (autoClose)
                {
                    MudDialog.CloseOk(response.Data);
                }

                return true;
            }
            catch (ValidationException e)
            {
                Snackbar.AddValidationError(e);
            }
        }

        return false;
    }

    protected virtual void OnSendCommand() { }

    protected virtual Task OnSubmitSuccessAsync(TModel model) => Task.CompletedTask;

    protected void Close() => MudDialog.Close();

    protected void Cancel() => MudDialog.Cancel();
    #endregion
}