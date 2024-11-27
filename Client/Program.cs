using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TaskTracker.Client;
using TaskTracker.Client.Services;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HTTP Client
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Add MudBlazor services
builder.Services.AddMudServices();

// Add Local Storage services
builder.Services.AddBlazoredLocalStorage();

// Authentication services
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Task, Project, and User services
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddScoped(sp => {
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    var localStorage = sp.GetRequiredService<ILocalStorageService>();
    var httpClient = new HttpClient(new AuthenticatedHttpClientHandler(localStorage))
    {
        BaseAddress = new Uri(navigationManager.BaseUri)
    };
    return httpClient;
});

// Add Authorization
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
