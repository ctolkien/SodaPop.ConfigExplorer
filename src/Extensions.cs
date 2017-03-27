using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Html;

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
                x => x.UseMiddleware<AspNetOptionsExplorerMiddleware>(configRoot, options));


        }

        //todo: make this less terribad
        private static bool IsValid(this HttpContext context, AspNetOptionsExplorerOptions options)
        {
            var valid = context.Request.Path.Equals(options.PathMatch);

            if (options.LocalHostOnly && valid)
            {
                valid = context.Request.Host.Host.Equals("localhost");
            }

            return valid;
        }

        public static HtmlString RenderUls(IEnumerable<Option> options )
        {
            var sb = new StringBuilder();
            var iter = options.GetEnumerator();
            while (iter.MoveNext())
            {
                var item = iter.Current;

                sb.AppendLine("<ul>");
                sb.AppendLine($"<li>Path: {item.Path}</li>");
                sb.AppendLine($"<li>Key: {item.Key}</li>");
                if (!string.IsNullOrEmpty(item.Value))
                {
                    sb.AppendLine($"<li>Value: {item.Value}</li>");
                }
                sb.Append($"{RenderUls(item.Children)}");
                sb.AppendLine("</ul>");

            }
            return new HtmlString(sb.ToString());
        }
    }
}
