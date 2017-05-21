using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace SodaPop.ConfigExplorer
{
    public static class MiddlwareExtensions
    {
        public static IApplicationBuilder UseConfigExplorer(
            this IApplicationBuilder builder,
            IConfigurationRoot configRoot,
            ConfigExplorerOptions options = null)
        {
            options = options ?? new ConfigExplorerOptions();

            return builder.MapWhen(context => context.IsValid(options),
                x => x.UseMiddleware<ConfigExplorerMiddleware>(configRoot, options));
        }

        //todo: make this less terribad
        private static bool IsValid(this HttpContext context, ConfigExplorerOptions options)
        {
            var valid = context.Request.Path.Equals(options.PathMatch);

            if (options.LocalHostOnly && valid)
            {
                return context.Request.Host.Host.Equals("localhost");
            }

            return valid;
        }
    }
}
