using Microsoft.AspNetCore.Html;
namespace Microsoft.AspNetCore.Mvc.Rendering {
    public static class MyHelper {
        public static IHtmlContent ColorfulHeading(this IHtmlHelper htmlHelper, int level, string color, string content) {
            //two next lines: if level > 6 or < 1, so it's equal to 6 or 1. That's because we have the sequence h1, h2, h3..,h6 in HTML
            level = level < 1 ? 1 : level;
            level = level > 6 ? 6 : level;
            var tagName = $"h{level}";
            //we use TagBuilder to create a HTML mark
            var tagBuilder = new TagBuilder(tagName);
            tagBuilder.Attributes.Add("style", $"color:{color ?? "green"}");
            tagBuilder.InnerHtml.Append(content ?? string.Empty);
            return tagBuilder;
        }
    }
}