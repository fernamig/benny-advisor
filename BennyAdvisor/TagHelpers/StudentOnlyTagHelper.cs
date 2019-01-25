using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BennyAdvisor.TagHelpers
{
    [HtmlTargetElement(Attributes = "student-only")]
    public class StudentOnlyTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Mvc.Rendering.ViewContext" /> for the current request.
        /// This is required to get the active page.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var activePage = ViewContext.RouteData.Values["page"] as string;

            if (!activePage.StartsWith("/Student/"))
                Hide(output);

            output.Attributes.RemoveAll("student-only");
        }

        void Hide(TagHelperOutput output)
        {
            var classAttr = output.Attributes.FirstOrDefault(a => a.Name == "class");
            if ((classAttr == null) || (classAttr.Value == null))
                output.Attributes.Add(new TagHelperAttribute("class", "d-none"));
            else
                output.Attributes.SetAttribute("class", classAttr.Value.ToString() + " d-none");
        }
    }
}
