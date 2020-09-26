using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NPCE_Client.AppComponents;
using NPCE_Client.AppComponents.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NPCE_Client.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            // Use of IHttpClientFactory

            builder.Services.AddHttpClient<IAnagraficheDataService, AnagraficheDataService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:60909/");
            });
            builder.Services.AddHttpClient<IDocumentiDataService, DocumentiDataService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:60909/");
            });

            await builder.Build().RunAsync();
        }
    }
}
