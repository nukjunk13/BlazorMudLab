using BlazorMudLab.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorMudLab.Pages.Users.BaseClass
{
    public class UserListBase :ComponentBase
    {
        [Inject]
        protected UserService UserService { get; set; } = null!; // Inject UserService

        [Inject]
        protected ISnackbar Snackbar { get; set; } = null!; // Inject ISnackbar

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = null!;
        protected List<User> Users = new();

        protected bool enabled = true;

        protected override async Task OnInitializedAsync() {
            Users = await UserService.GetUsersAsync();
        }

        protected async Task DeleteUser(string id) {
            if(await UserService.DeleteUserAsync(id)) {
                Users = await UserService.GetUsersAsync();
                Snackbar.Add("Delete success.", Severity.Success);
            } else {
                Snackbar.Add("There were some problems, try again.", Severity.Error);
            }
        }

        protected async Task SaveUser() {
            Users = await UserService.GetUsersAsync();
        }
    }
}
