using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace AspNetOptionsExplorer
{
    public static class Extensions
    {
        public static IApplicationBuilder UseAspNetOptionsExplorer(
            this IApplicationBuilder builder,
            IConfigurationRoot configRoot,
            string pathMatch = "/options")
        {

            return builder.MapWhen(x =>
            {
                return x.Request.Path.Equals(pathMatch) && x.Request.Host.Host == "localhost";
            }, x => x.UseMiddleware<AspNetOptionsExplorerMiddleware>(configRoot));


        }
    }
}
