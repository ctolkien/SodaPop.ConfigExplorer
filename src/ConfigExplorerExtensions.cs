
namespace SodaPop.ConfigExplorer
{
    public static class ConfigExplorerExtensions
    {
        /// <summary>
        /// Add the config explorer services.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddConfigExplorer(this IServiceCollection services)
        {
            return services;
        }

        /// <summary>
        /// Add and configure the config explorer services.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="configure">Configuration action.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddConfigExplorer(this IServiceCollection services, Action<ConfigExplorerOptions> configure)
        {
            if (configure != null)
                services.Configure(configure);

            return services;
        }

        /// <summary>
        /// Add the config explorer middleware.
        /// </summary>
        /// <param name="builder">Application builder.</param>
        /// <returns>Application builder.</returns>
        public static IApplicationBuilder UseConfigExplorer(this IApplicationBuilder builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            return builder.UseMiddleware<ConfigExplorerMiddleware>();
        }

        /// <summary>
        /// Add the config explorer middleware with an explicit configuration.
        /// </summary>
        /// <param name="builder">Application builder.</param>
        /// <param name="options">Config explorer options.</param>
        /// <returns>Application builder.</returns>
        /// <remarks>The specified options will overwrite the global configuration.</remarks>
        public static IApplicationBuilder UseConfigExplorer(this IApplicationBuilder builder, ConfigExplorerOptions options)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            return builder.UseMiddleware<ConfigExplorerMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Add the config explorer middleware for a custom configuration.
        /// </summary>
        /// <param name="builder">Application builder.</param>
        /// <param name="config">Configuration to display.</param>
        /// <param name="options">Optional config explorer options.</param>
        /// <returns>Application builder.</returns>
        /// <remarks>The specified options will overwrite the global configuration.</remarks>
        public static IApplicationBuilder UseConfigExplorer(this IApplicationBuilder builder, IConfiguration config, ConfigExplorerOptions options = null)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (config is null)
                throw new ArgumentNullException(nameof(config));

            if (options != null)
                return builder.UseMiddleware<ConfigExplorerMiddleware>(config, Options.Create(options));
            else
                return builder.UseMiddleware<ConfigExplorerMiddleware>(config);
        }
    }
}
