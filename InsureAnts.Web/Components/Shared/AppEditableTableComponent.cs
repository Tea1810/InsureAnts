using FluentValidation;
using InsureAnts.Application.Data_Queries;
using InsureAnts.Application.Features.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InsureAnts.Web.Components.Shared;

public abstract class AppEditableTableComponent<TQuery, TModel> : AppTableComponent<TQuery, TModel>
    where TQuery : AbstractQueryRequest<TModel>, new()
{
    #region Properties
    [Inject]
    protected IDialogService DialogService { get; set; }

    [Inject]
    protected ISnackbar Snackbar { get; set; }
    #endregion

    protected virtual Task<string> GetDeleteTitle(TModel model) => Task.FromResult("Are you sure you want to delete this item?");
    protected virtual Task<string> GetBulkDeleteTitle() => Task.FromResult("Are you sure you want to delete all items?");
    protected async Task DeleteItemAsync(TModel model)
    {
        var message = await GetDeleteTitle(model);

        var shouldDelete = await DialogService.AskConfirmation("Delete", message);
        if (shouldDelete)
        {
            IResponse response;

            try
            {
                response = await OnDeleteAsync(model);
            }
            catch (ValidationException e)
            {
                Snackbar.AddValidationError(e);
                return;
            }

            Snackbar.AddConditional(response);

            if (response.IsSuccess)
            {
                await ReloadTableServerData();
            }
        }
    }

    protected async Task DeleteItemsAsync(ICollection<TModel> items)
    {
        var message = await GetBulkDeleteTitle();

        var shouldDelete = await DialogService.AskConfirmation("Delete confirmation", message);
        if (shouldDelete)
        {
            IResponse response;
            bool reloadTableServerData = false;

            foreach (var item in items)
            {
                try
                {
                    response = await OnDeleteAsync(item);
                }
                catch (ValidationException e)
                {
                    Snackbar.AddValidationError(e);
                    return;
                }
                reloadTableServerData = true;
                Snackbar.AddConditional(response);
            }

            if (reloadTableServerData)
            {
                await ReloadTableServerData();
            }
        }
    }
    protected virtual Task<IResponse> OnDeleteAsync(TModel model) => throw new NotImplementedException($"{nameof(OnDeleteAsync)} must be implemented");
}

public abstract class AppEditableTableComponent<TQuery, TEditCommand, TModel> : AppEditableTableComponent<TQuery, TModel>
    where TQuery : AbstractQueryRequest<TModel>, new()
    where TEditCommand : EditCommand<TModel, TModel, int>, new()
{
    protected virtual Task<string> GetEditTitle(TEditCommand model) => Task.FromResult("Are you sure you want to edit this item?");

    protected async Task EditItemAsync(TEditCommand query)
    {
        var message = await GetEditTitle(query);

        var shouldEdit = await DialogService.AskConfirmation("Edit", message);
        if (shouldEdit)
        {
            IResponse response;

            try
            {
                response = await Mediator.Send(query);
            }
            catch (ValidationException e)
            {
                Snackbar.AddValidationError(e);
                return;
            }

            Snackbar.AddConditional(response);

            if (response.IsSuccess)
            {
                await ReloadTableServerData();
            }
        }
    }
}

public abstract class AppEditableTableComponent<TQuery, TModel, TAddComponent, TEditComponent> : AppEditableTableComponent<TQuery, TModel>
where TQuery : AbstractQueryRequest<TModel>, new()
where TAddComponent : ComponentBase
where TEditComponent : ComponentBase
{
    #region Protected methods
    protected virtual string GetEditTitle(TModel model) => "Edit";

    protected async Task EditItemAsync(TModel model)
    {
        var parameters = model.ToModelDialogParameters();
        parameters.Add("Title", GetEditTitle(model));

        var dialog = await DialogService.ShowAsync<TEditComponent>(
            GetEditTitle(model),
            parameters,
            new DialogOptions { CloseOnEscapeKey = false, BackdropClick = false, NoHeader = true }
        );

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await ReloadTableServerData();
        }
    }

    protected virtual string GetAddTitle() => "Add new";

    protected virtual void ConfigureAddParameters(DialogParameters parameters, DialogOptions options)
    {
    }

    protected async Task AddItem()
    {
        var parameters = new DialogParameters();

        var options = new DialogOptions { CloseOnEscapeKey = false, BackdropClick = false, NoHeader = true };

        ConfigureAddParameters(parameters, options);

        var dialog = await DialogService.ShowAsync<TAddComponent>(
            GetAddTitle(),
            parameters,
            options
        );

        var result = ProcessAddDialogResult(await dialog.Result);

        if (!result.Canceled)
        {
            await ReloadTableServerData();
        }
    }

    protected virtual DialogResult ProcessAddDialogResult(DialogResult dialogResult) => dialogResult;
    #endregion
}