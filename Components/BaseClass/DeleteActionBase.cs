using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Shared.Components.BaseClass;

public class DeleteActionBase :ComponentBase
{

    [Parameter]
    public string ConfirmMessage { get; set; } = "Are you sure to delete this record?";


    [Parameter]
    public EventCallback<bool> OnAfterDelete {
        get; set;
    }

    protected bool _visible;
    protected DialogOptions _dialogOptions = new() { FullWidth = true };

    protected override async Task OnInitializedAsync() {
        await base.OnInitializedAsync();
    }


    protected void Open() {
        _visible = true;
    }
    protected void Cancel() => _visible = false;

    protected virtual async Task Confirm() {
        _visible = false;
        await OnAfterDelete.InvokeAsync(true);
        StateHasChanged();
    }
}