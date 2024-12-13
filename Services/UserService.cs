using Blazored.LocalStorage;
using BlazorMudLab.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace BlazorMudLab.Services;

public class UserService(Api api, ILocalStorageService localStorage, AuthenticationStateProvider _authProvider)
{
    private readonly Api _api = api;
    private readonly ILocalStorageService _localStorage = localStorage;
    private readonly AuthenticationStateProvider authProvider = _authProvider;

    public event Action? OnLoginStatusChanged;

    public async Task LoginStatusAsync(bool isLoggedIn)
    {
        await _localStorage.SetItemAsync("isLoggedIn", isLoggedIn);
        OnLoginStatusChanged?.Invoke(); // แจ้งเตือนว่ามีการเปลี่ยนแปลง
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _api.ApiGet<List<User>>("/api/users") ?? [];
    }

    public async Task<User> GetUserByIdAsync(string id)
    {
        return await _api.ApiGet<User>($"/api/users/{id}") ?? new User();
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var customAuthProvider = (CustomAuthenticationStateProvider)authProvider;
        // Simulate login
        if (username == "admin" && password == "1234")
        {
            await customAuthProvider.SetLoginStatusAsync(true);
            await LoginStatusAsync(true);
            return true;
        }
        await customAuthProvider.SetLoginStatusAsync(false);
        await LoginStatusAsync(false);
        return false;
    }

    public async Task LogoutAsync()
    {
        var customAuthProvider = (CustomAuthenticationStateProvider)authProvider;
        await customAuthProvider.SetLoginStatusAsync(false);
        await LoginStatusAsync(false);
        await _localStorage.RemoveItemAsync("isLoggedIn");
    }

    // New method to save a user
    public async Task<bool> SaveUserAsync(User user)
    {
        var param = new
        {
            Name = user?.Name,
            Age= user?.Age,
            Email = user?.Email
        };

        // Send the POST request to save the user data
        var response = await _api.ApiPost("/api/users", param);

        // Check if the request was successful
        return response.IsSuccessStatusCode;
    }

    // New method to edit a user
    public async Task<bool> EditUserAsync(User user) {

        // Send the PUT request to save the user data
        var response = await _api.ApiPut($"/api/users/{user.Id}", user);

        // Check if the request was successful
        return response.IsSuccessStatusCode;
    }

    // New method to delete a user
    public async Task<bool> DeleteUserAsync(string id)
    {
        var response = await _api.ApiDelete($"/api/users/{id}");

        return response.IsSuccessStatusCode;
    }
}
