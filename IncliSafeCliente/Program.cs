
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using MudBlazor.Services;
using IncliSafe.Cliente.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://0.0.0.0:80") });
builder.Services.AddScoped<HttpService>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
