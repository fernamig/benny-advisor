using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Razor;

namespace BennyAdvisor.TagHelpers
{
    [HtmlTargetElement("modal")]
    public class ModalTagHelper : TagHelper
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string OkTitle { get; set; } = "Ok";
        public string CancelTitle { get; set; } = "Cancel";
        public string OnOkClick { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("id", Id);
            output.Attributes.Add("class", "modal fade");
            output.Attributes.Add("tabindex", -1);
            output.Attributes.Add("role", "dialog");

            output.Content.AppendHtml(@"<div class=""modal-dialog"" role=""document"">");
            output.Content.AppendHtml(@"  <div class=""modal-content"">");
            output.Content.AppendHtml(@"    <div class=""modal-header"">");
            output.Content.AppendHtml($"      <h5 class=\"modal-title\">{Title}</h5>");
            output.Content.AppendHtml(@"      <button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">");
            output.Content.AppendHtml(@"        <i class=""fas fa-times""></i>");
            output.Content.AppendHtml(@"      </button>");
            output.Content.AppendHtml(@"    </div>");
            output.Content.AppendHtml(@"    <div class=""modal-body""> ");
            output.Content.AppendHtml(await output.GetChildContentAsync());
            output.Content.AppendHtml(@"    </div>");
            output.Content.AppendHtml(@"    <div class=""modal-footer"">");
            output.Content.AppendHtml($"      <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\">{CancelTitle}</button>");
            output.Content.AppendHtml($"      <button type=\"button\" class=\"btn btn-primary\" onclick=\"{OnOkClick}\" >{OkTitle}</button>");
            output.Content.AppendHtml(@"    </div>");
            output.Content.AppendHtml(@"  </div>");
            output.Content.AppendHtml(@"</div>");
        }
    }
}
