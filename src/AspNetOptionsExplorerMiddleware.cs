using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using System.Reflection;

namespace AspNetOptionsExplorer
{
    public class AspNetOptionsExplorerMiddleware
    {
        private readonly IConfigurationRoot _config;


        public AspNetOptionsExplorerMiddleware(RequestDelegate next, IConfigurationRoot config)
        {
            _config = config;

        }

        public async Task Invoke(HttpContext context)
        {
            var options = CreateOptionList(_config.GetChildren());

            await ResponseWriter(context, options);

            //now lets test
            var file = System.IO.File.ReadAllText("Dashboard/test.html");

            await context.Response.WriteAsync(file);
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
                o.Children = CreateOptionList(c.GetChildren());
                yield return o;
            }
        }

    }
}
