using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace SodaPop.ConfigExplorer
{
    public class ConfigExplorerMiddleware
    {
        private readonly IConfiguration _config;
        private readonly ConfigExplorerOptions _explorerOptions;
        private readonly RequestDelegate _next;

        public ConfigExplorerMiddleware(RequestDelegate next, IConfiguration config, ConfigExplorerOptions explorerOptions)
        {
            _config = config;
            _explorerOptions = explorerOptions;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var configs = CreateConfigurationList(_config.GetChildren()).ToList();

            // render configuration
            await RenderConfigurationAsync(context.Response, configs);
        }

        /// <summary>
        /// Builds a tree of options via recursion
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private IEnumerable<ConfigurationItem> CreateConfigurationList(IEnumerable<IConfigurationSection> config)
        {
            foreach (var c in config)
            {
                var o = new ConfigurationItem { Path = c.Path, Key = c.Key, Value = c.Value };
                if (_explorerOptions.TryRedactConnectionStrings)
                {
                    //todo: make this less bad
                    if (o.Path.IndexOf("ConnectionString", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        o.Value = "REDACTED";
                    }
                }
                o.Children = CreateConfigurationList(c.GetChildren());
                yield return o;
            }
        }

        /// <summary>
        /// Render the configuration.
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <param name="configuration">Configuration to render.</param>
        private async Task RenderConfigurationAsync(HttpResponse response, IEnumerable<ConfigurationItem> configuration)
        {
            response.StatusCode = 200;
            await response.WriteAsync("<!DOCTYPE html>\n<html>\n<head>\n");
            await response.WriteAsync("  <meta charset=\"utf-8\" />\n");
            await response.WriteAsync("  <title>ASP.NET Core Config Explorer</title>\n");
            await response.WriteAsync("  <style>html { font: 14px/1.4 sans-serif; color: #333; background: #f8f8f8; } body { margin: 1rem auto; padding: 1rem; max-width: 1200px; background: white; border: 1px solid #e7e7e7; } h1 { border-bottom: 1px solid #e7e7e7; padding: 0 .5rem .5rem; color: #777; font-size: 1.3rem; } ul { padding-left: 2rem; } li { margin-bottom: .5rem; }</style>\n");
            await response.WriteAsync("</head>\n<body>\n");
            await response.WriteAsync("  <h1>ASP.NET Core Config Explorer</h1>\n");

            // render items
            await RenderItemsAsync(configuration);

            await response.WriteAsync("\n</body>\n</html>\n");


            async Task RenderItemsAsync(IEnumerable<ConfigurationItem> items)
            {
                await response.WriteAsync("<ul>\n");
                foreach (var item in items)
                {
                    await response.WriteAsync($"<li>\n<strong>Path:</strong> {item.Path}<br />\n<strong>Key:</strong> {item.Key}");
                    if (!string.IsNullOrEmpty(item.Value))
                        await response.WriteAsync($"<br />\n<strong>Value:</strong> {item.Value}\n");

                    if (item.Children.Any())
                        await RenderItemsAsync(item.Children);

                    await response.WriteAsync("</li>\n");
                }
                await response.WriteAsync("</ul>\n");
            }
        }
    }
}
