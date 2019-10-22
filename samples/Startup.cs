using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SodaPop.ConfigExplorer.Sample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // configure the global config explorer
            services.AddConfigExplorer(options =>
            {
                options.LocalHostOnly = false;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration config)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // add config explorer for global configuration (configured above)
                app.UseConfigExplorer();

                // add config explorer for subsection with explicit configuration
                app.UseConfigExplorer(config.GetSection("Tier1"), new ConfigExplorerOptions
                {
                    PathMatch = "/config/example",
                });
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("<!DOCTYPE html><html>" +
                    "<head><title>SodaPop.ConfigExplorer.Sample</title></head>" +
                    "<body>" +
                    "<p>Browse to the global demo here: <a href=\"/config\">Show global configuration</a></p>" +
                    "<p>Browse to the subsection demo here: <a href=\"/config/example\">Show 'Tier1' section configuration</a></p>" +
                    "</body>" +
                    "</html>");
            });
        }
    }
}
