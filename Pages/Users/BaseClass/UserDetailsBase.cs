using BlazorMudLab.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorMudLab.Pages.Users.BaseClass
{
    public class UserDetailsBase :ComponentBase
    {
        [Inject]
        protected UserService UserService { get; set; } = null!; // Inject UserService

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!; // Inject ISnackbar

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = null!;

        [Parameter]
        public string Id {
            get; set;
        }

        protected User User = new();

        protected override async Task OnInitializedAsync() {
            User = await UserService.GetUserByIdAsync(Id);
        }

        protected async Task EditUser() {
            if(string.IsNullOrWhiteSpace(User?.Name) || string.IsNullOrWhiteSpace(User?.Email) || User?.Age == 0) {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
                Snackbar.Add("Information cannot be empty", Severity.Error);
                return;
            }

            var res = await UserService.EditUserAsync(User);
            if(res) {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
                Snackbar.Add("Server update", Severity.Success);
                NavigationManager.NavigateTo($"/users");
            } else {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
                Snackbar.Add("There were some problems, try again.", Severity.Error);
            }
        }
    }
}
