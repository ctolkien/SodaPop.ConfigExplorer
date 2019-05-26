using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RazorLight;
using System.Linq;

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

            var engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(ConfigExplorerMiddleware))
                .Build();

            var result = await engine.CompileRenderAsync("Configs", configs);

            await context.Response.WriteAsync(result);
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
    }
}
