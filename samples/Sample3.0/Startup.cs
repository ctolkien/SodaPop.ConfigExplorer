using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SodaPop.ConfigExplorer.Sample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConfigExplorer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration config)
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

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("<!DOCTYPE html><html>" +
                        "<head><title>SodaPop.ConfigExplorer.Sample</title></head>" +
                        "<body>" +
                        "<p>Browse to the global demo here: <a href=\"/config\">Show global configuration</a></p>" +
                        "<p>Browse to the subsection demo here: <a href=\"/config/example\">Show 'Tier1' section configuration</a></p>" +
                        "</body>" +
                        "</html>");
                });
            });
        }
    }
}
