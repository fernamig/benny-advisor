using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Dynamic;

namespace BennyAdvisor.TagHelpers
{
    [HtmlTargetElement("js-template")]
    public class JsTemplateTagHelper : TagHelper
    {
        public string Id { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.PreContent.AppendHtml($"<script id=\"{Id}\" type=\"text/x-jsrender\">");
            output.Content.AppendHtml(await output.GetChildContentAsync());
            output.PostContent.AppendHtml(@"</script>");
        }
    }
}
