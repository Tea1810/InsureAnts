using Microsoft.AspNetCore.Components;

namespace InsureAnts.Web.Components.Shared;


public abstract class AbstractComponent : ComponentBase
{
    protected sealed override void OnAfterRender(bool firstRender) => base.OnAfterRender(firstRender);

    protected sealed override void OnParametersSet() => base.OnParametersSet();

    protected override void OnInitialized() => base.OnInitialized();
}