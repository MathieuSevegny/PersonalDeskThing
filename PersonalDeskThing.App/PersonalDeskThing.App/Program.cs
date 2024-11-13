using PersonalDeskThing.App.Components;
using PersonalDeskThing.App.Core.Cache;
using PersonalDeskThing.App.Core.Models.Options;
using PersonalDeskThing.App.Core.Spotify;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.Configure<CacheOptions>(builder.Configuration.GetSection(CacheOptions.Position));
builder.Services.Configure<SpotifyOptions>(builder.Configuration.GetSection(SpotifyOptions.Position));

builder.Services.AddSingleton<CacheService>();
builder.Services.AddSingleton<SpotifyAuthService>();

builder.Services.AddMudServices();

builder.Services.AddOptions();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(PersonalDeskThing.App.Client._Imports).Assembly);

app.Run();
