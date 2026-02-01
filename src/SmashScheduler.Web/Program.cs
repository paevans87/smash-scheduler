using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SmashScheduler.Application.Interfaces.Repositories;
using SmashScheduler.Application.Services.ClubManagement;
using SmashScheduler.Infrastructure.Web;
using SmashScheduler.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMudServices();

builder.Services.AddSingleton<IClubRepository, InMemoryClubRepository>();
builder.Services.AddScoped<IClubService, ClubService>();

await builder.Build().RunAsync();
