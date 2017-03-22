using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;

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
            var flattenedOptions = _config.GetChildren()
                .SelectMany(x => x.AsEnumerable())
                .Where(x => !x.Key.Contains("ConnectionString"))
                .Where(x => !string.IsNullOrEmpty(x.Value));

            foreach (var item in flattenedOptions)
            {
                await context.Response.WriteAsync($"Key: '{item.Key}', Value: '{item.Value}' {Environment.NewLine}");

            }
        }

    }
}
