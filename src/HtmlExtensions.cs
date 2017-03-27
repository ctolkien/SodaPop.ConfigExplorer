using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Html;

namespace SodaPop.ConfigExplorer
{
    public class HtmlExtensions
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
