using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace SodaPop.ConfigExplorer.Sample
{
    public class Startup
    {
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

            app.Run((context) => context.Response.WriteAsync("Hello Sample World!"));
        }
    }
}
