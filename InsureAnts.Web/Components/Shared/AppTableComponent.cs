using InsureAnts.Application.Data_Queries;
using InsureAnts.Web.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using InsureAnts.Web.Infrastructure;

namespace InsureAnts.Web.Components.Shared;

public abstract class AppTableComponent<TQuery, TModel> : AbstractComponent, IAsyncDisposable
    where TQuery : AbstractQueryRequest<TModel>, new()
{
    #region Private members
    private bool _isRendered;
    #endregion


    #region Properties
    [Inject]
    private IJSRuntime JsRuntime { get; set; } = default!;

    [Inject]
    protected IBlazorMediator Mediator { get; set; } = default!;

    protected MudTable<TModel>? Table { get; set; }

    protected TQuery QueryRequest { get; } = new();

    protected virtual int DefaultRowsPerPage => 25;
    #endregion


    #region Overrides
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _isRendered = true;

            if (Table != null && Table.RowsPerPage != DefaultRowsPerPage)
            {
                Table.SetRowsPerPage(DefaultRowsPerPage);
            }

            await JsRuntime.InvokeIgnoreCancellationAsync("registerFilter");
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (_isRendered)
        {
            await ReloadTableServerData();
        }

        await base.OnParametersSetAsync();
    }
    #endregion


    #region IDisposable
    public virtual ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return JsRuntime.InvokeIgnoreCancellationAsync("unregisterFilter");
    }
    #endregion


    #region Protected methods
    protected Task ReloadTableServerData() => Table?.ReloadServerData() ?? Task.CompletedTask;

    protected Task ReloadTableOnEnter(KeyboardEventArgs e) => string.Equals("Enter", e.Key, StringComparison.Ordinal) ? ReloadTableServerData() : Task.CompletedTask;

    protected async Task<TableData<TModel>> TableServerData(TableState tableState, CancellationToken cancellationToken)
    {
        Configure(QueryRequest, tableState);

        var queryResult = await Mediator.Send(QueryRequest, cancellationToken);

        return new TableData<TModel>
        {
            TotalItems = queryResult.Total,
            Items = queryResult.Items
        };
    }

    private static void Configure(TQuery queryRequest, TableState tableState)
    {
        queryRequest.Skip = tableState.Page * tableState.PageSize;
        queryRequest.Take = tableState.PageSize;

        queryRequest.Sort.Clear();
        if (!string.IsNullOrEmpty(tableState.SortLabel) && tableState.SortDirection is not SortDirection.None)
        {
            queryRequest.Sort[tableState.SortLabel] = tableState.SortDirection is SortDirection.Ascending;
        }
    }
    #endregion
}