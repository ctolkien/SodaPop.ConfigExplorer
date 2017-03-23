using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace AspNetOptionsExplorer
{
    public static class Extensions
    {
        public static IApplicationBuilder UseAspNetOptionsExplorer(
            this IApplicationBuilder builder,
            IConfigurationRoot configRoot,
            AspNetOptionsExplorerOptions options = null)
        {

            options = options ?? new AspNetOptionsExplorerOptions();

            return builder.MapWhen(context =>
            {
                return context.IsValid(options);
            },
                x => x.UseMiddleware<AspNetOptionsExplorerMiddleware>(configRoot));

        }

        //todo: make this less terribad
        public static bool IsValid(this HttpContext context, AspNetOptionsExplorerOptions options)
        {
            var valid = context.Request.Path.Equals(options.PathMatch);

            if (options.LocalHostOnly)
            {
                valid = context.Request.Host.Host.Equals("localhost");
            }

            return valid;
        }
    }
}
