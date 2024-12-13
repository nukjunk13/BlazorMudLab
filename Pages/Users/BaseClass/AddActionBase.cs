using BlazorMudLab.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorMudLab.Pages.Users.BaseClass;

public class AddActionBase :ComponentBase
{
    [Inject]
    protected UserService UserService { get; set; } = null!; // Inject UserService

    [Inject]
    protected ISnackbar Snackbar { get; set; } = null!; // Inject ISnackbar

    protected User User = new();

    [Parameter]
    public EventCallback<bool> OnAfterSave {
        get; set;
    }

    protected bool _visible;
    protected DialogOptions _dialogOptions = new() { FullWidth = true };

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }


    protected void Open() {
        _visible = true;
    }
    protected void Cancel() => _visible = false;


    protected virtual async Task Confirm() {
        if(string.IsNullOrWhiteSpace(User?.Name) || string.IsNullOrWhiteSpace(User?.Email) || User?.Age == 0) {
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
            Snackbar.Add("Information cannot be empty", Severity.Error);
            return;
        }

        var res = await UserService.SaveUserAsync(User);
        if(res) {
            _visible = false;
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
            Snackbar.Add("Server new data.", Severity.Success);
            await OnAfterSave.InvokeAsync(true);
            StateHasChanged();
        } else {
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
            Snackbar.Add("There were some problems, try again.", Severity.Error);
        }
    }
}