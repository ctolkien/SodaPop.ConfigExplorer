using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Html;

namespace SodaPop.ConfigExplorer
{
    public static class HtmlExtensions
    {
        //Todo: don't do this...
        public static HtmlString RenderUls(IEnumerable<ConfigurationItem> configItems)
        {
            var sb = new StringBuilder();
            var iter = configItems.GetEnumerator();
            while (iter.MoveNext())
            {
                var item = iter.Current;

                sb.AppendLine("<ul>");
                sb.AppendLine($"<li><strong>Path:</strong> {item.Path}</li>");
                sb.AppendLine($"<li><strong>Key:</strong> {item.Key}</li>");
                if (!string.IsNullOrEmpty(item.Value))
                {
                    sb.AppendLine($"<li><strong>Value:</strong> {item.Value}</li>");
                }
                sb.Append($"{RenderUls(item.Children)}");
                sb.AppendLine("</ul>");
            }
            return new HtmlString(sb.ToString());
        }
    }
}
