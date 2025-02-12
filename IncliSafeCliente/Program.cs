using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using IncliSafe.Cliente;
using MudBlazor.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IncliSafe.Cliente
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://0.0.0.0:5000") });
            builder.Services.AddMudServices();

            await builder.Build().RunAsync();
        }
    }
}