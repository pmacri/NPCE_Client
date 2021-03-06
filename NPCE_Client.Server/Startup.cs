using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NPCE_Client.AppComponents.Services;
using System;

namespace NPCE_Client.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            

            // Use of IHttpClientFactory
            services.AddHttpClient<IAnagraficheDataService, AnagraficheDataService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:60909/");
            });

            services.AddHttpClient<IDocumentiDataService, DocumentiDataService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:60909/");
            });
            services.AddHttpClient<IAmbientiDataService, AmbientiDataService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:60909/");
            });

            services.AddHttpClient<IServiziDataService, ServiziDataService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:60909/");
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();



            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
