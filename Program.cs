using Blazored.LocalStorage;
using BlazorMudLab;
using BlazorMudLab.Auth;
using BlazorMudLab.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.DataProtection;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
// builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices(); // เพิ่ม MudBlazor Services
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<UserService>(); // สำหรับ Dependency Injection
builder.Services.AddScoped<Api>(); // สำหรับ Dependency Injection

builder.Services.AddSingleton<Base64EncryptionService>();
builder.Services.AddScoped<JweService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://cad31be6b6f7ea5e4cea.free.beeceptor.com") });
builder.Services.AddHttpClient("Api1",
    client => { client.BaseAddress = new Uri("https://ca7e60cf86d516d389be.free.beeceptor.com"); });

await builder.Build().RunAsync();