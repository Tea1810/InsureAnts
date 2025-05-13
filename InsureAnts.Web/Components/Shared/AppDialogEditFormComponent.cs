using AutoMapper;
using InsureAnts.Application.Features.Abstractions;
using Microsoft.AspNetCore.Components;

namespace InsureAnts.Web.Components.Shared;

public abstract class AppDialogEditFormComponent<TCommand, TModel> : AppDialogAddFormComponent<TCommand, TModel>
    where TCommand : ICommand<IResponse<TModel>>, new()
{
    #region Properties
    [Inject]
    protected IMapper Mapper { get; set; }

    [Parameter]
    public TModel Model { get; set; }
    #endregion


    #region Overrides
    protected override Task OnParametersSetAsync()
    {
        Mapper.Map(Model, Command);

        return base.OnParametersSetAsync();
    }
    #endregion
}