using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RazorLight;

namespace SodaPop.ConfigExplorer
{
    public class ConfigExplorerMiddleware
    {
        private readonly IConfiguration _config;
        private readonly ConfigExplorerOptions _explorerOptions;
        private readonly RequestDelegate _next;

        public ConfigExplorerMiddleware(RequestDelegate next, IConfiguration config, IOptions<ConfigExplorerOptions> explorerOptions)
        {
            _config = config;
            _explorerOptions = explorerOptions.Value;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!IsValid(context))
            {
                await _next(context);
                return;
            }

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

        /// <summary>
        /// Determine whether the request is a valid config explorer request.
        /// </summary>
        /// <param name="context">HTTP context.</param>
        /// <returns>Whether the request is valid.</returns>
        private bool IsValid(HttpContext context)
        {
            if (!context.Request.Path.Equals(_explorerOptions.PathMatch))
                return false;

            if (_explorerOptions.LocalHostOnly && !context.Request.Host.Host.Equals("localhost"))
                return false;

            return true;
        }
    }
}
