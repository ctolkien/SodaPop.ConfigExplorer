using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SodaPop.ConfigExplorer;


namespace SodaPop.ConfigExplorer.Sample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();


            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseConfigExplorer(config, new ConfigExplorerOptions //optional
                {
                    LocalHostOnly = true, //default
                    PathMatch = "/config", //default
                    TryRedactConnectionStrings = true //default
                });
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello Sample World!");
            });

        }
    }
}
