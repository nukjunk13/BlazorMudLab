using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BlazorMudLab.Auth
{
    public class CustomAuthenticationStateProvider(ILocalStorageService localStorage) :AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage = localStorage;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
            var isLoggedIn = await _localStorage.GetItemAsync<bool>("isLoggedIn");

            Console.WriteLine($"GetAuthenticationStateAsync: {isLoggedIn}");

            if(!isLoggedIn) {
                Console.WriteLine($"anonymous: {isLoggedIn}");
                var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
                return new AuthenticationState(anonymous);
            }
            Console.WriteLine($"admin: {isLoggedIn}");
            var claims = new[] { new Claim(ClaimTypes.Name, "admin") };
            var identity = new ClaimsIdentity(claims, "CustomAuth");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        public async Task SetLoginStatusAsync(bool isLoggedIn) {
            Console.WriteLine($"SetLoginStatusAsync: {isLoggedIn}");
            await _localStorage.SetItemAsync("isLoggedIn", isLoggedIn);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync()); // รีเฟรชสถานะ
        }
    }

}
