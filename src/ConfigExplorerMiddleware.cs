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
        private readonly IConfigurationRoot _config;
        private readonly ConfigExplorerOptions _explorerOptions;

        public ConfigExplorerMiddleware(RequestDelegate next, IConfigurationRoot config, ConfigExplorerOptions explorerOptions)
        {
            _config = config;
            _explorerOptions = explorerOptions;

        }

        public async Task Invoke(HttpContext context)
        {
            var configs = CreateConfigurationList(_config.GetChildren()).ToList();


            var engine = EngineFactory.CreateEmbedded(typeof(ConfigExplorerMiddleware));


            var result = engine.Parse("Configs", configs);

            await context.Response.WriteAsync(result);

        }

        /// <summary>
        /// Builds a tree of options via recursion
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private IEnumerable<ConfigurationItem> CreateConfigurationList(IEnumerable<IConfigurationSection> config)
        {
            //todo: make this less bad
            var iter = config.GetEnumerator();
            while (iter.MoveNext())
            {
                var c = iter.Current;
                var o = new ConfigurationItem { Path = c.Path, Key = c.Key, Value = c.Value };
                if (_explorerOptions.TryRedactConnectionStrings)
                {
                    //todo: especially this bit
                    if (o.Path.Contains("Connection"))
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
