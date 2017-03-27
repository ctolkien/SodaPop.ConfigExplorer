using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using RazorLight;
using System.Linq;

namespace AspNetOptionsExplorer
{
    public class AspNetOptionsExplorerMiddleware
    {
        private readonly IConfigurationRoot _config;
        private readonly AspNetOptionsExplorerOptions _explorerOptions;

        public AspNetOptionsExplorerMiddleware(RequestDelegate next, IConfigurationRoot config, AspNetOptionsExplorerOptions explorerOptions)
        {
            _config = config;
            _explorerOptions = explorerOptions;

        }

        public async Task Invoke(HttpContext context)
        {
            var options = CreateOptionList(_config.GetChildren()).ToList();


            var engine = EngineFactory.CreateEmbedded(typeof(AspNetOptionsExplorerMiddleware));


            var result = engine.Parse("Options", options);

            await context.Response.WriteAsync(result);

            //await ResponseWriter(context, options);

        }


        /// <summary>
        /// Writes out the response via recursively crawling the tree of options
        /// </summary>
        /// <param name="context"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private async Task ResponseWriter(HttpContext context, IEnumerable<Option> options)
        {
            var iter = options.GetEnumerator();
            while (iter.MoveNext())
            {
                var item = iter.Current;
                await context.Response.WriteAsync($"Path: '{item.Path}, 'Key: '{item.Key}', Value: '{item.Value}' {Environment.NewLine}");
                await ResponseWriter(context, item.Children);
            }
        }

        /// <summary>
        /// Builds a tree of options via recursion
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private IEnumerable<Option> CreateOptionList(IEnumerable<IConfigurationSection> config)
        {
            var iter = config.GetEnumerator();
            while (iter.MoveNext())
            {
                var c = iter.Current;
                var o = new Option { Path = c.Path, Key = c.Key, Value = c.Value };
                if (_explorerOptions.TryRedactConnectionStrings)
                {
                    if (o.Path.Contains("Connection"))
                    {
                        o.Value = "REDACTED";
                    }
                }
                o.Children = CreateOptionList(c.GetChildren());
                yield return o;
            }
        }

    }
}
