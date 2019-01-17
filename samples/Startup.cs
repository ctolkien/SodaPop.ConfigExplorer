using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SodaPop.ConfigExplorer.Sample
{
    public class Startup
    {
        private const string DEFAULT_PATH = "/config";

        public void ConfigureServices(IServiceCollection services)
        { }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration config)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseConfigExplorer(config, new ConfigExplorerOptions // optional
                {
                    LocalHostOnly = true, // default
                    PathMatch = DEFAULT_PATH, // default
                    TryRedactConnectionStrings = true, // default
                });
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("<!DOCTYPE html><html>" +
                    "<head><title>SodaPop.ConfigExplorer.Sample</title></head>" +
                    "<body>" +
                    $"<p>Browse to the demo here: <a href=\"{DEFAULT_PATH}\">Show configuration</a></p>" +
                    "</body>" +
                    "</html>");
            });
        }
    }
}
