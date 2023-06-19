using Bug_Tracker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Data;

namespace Bug_Tracker.Infrastructure
{
	[HtmlTargetElement("div", Attributes = "page-model")]
	public class PageLinkTagHelper : TagHelper
	{

		private readonly IUrlHelperFactory _urlHelperFactory;
		public PageLinkTagHelper(IUrlHelperFactory helperFactory)
		{
			_urlHelperFactory = helperFactory;
		}


		[ViewContext]
		[HtmlAttributeNotBound]
		public ViewContext ViewContext { get; set; }
		public PageInformation PageModel { get; set; }
		public string PageAction { get; set; }
		public string PageSortBy { get; set; }
		public string PageSearch { get; set; }


		public bool PageClassesEnabled { get; set; } = false;
		public string PageClass { get; set; }
		public string PageClassNormal { get; set; }
		public string PageClassSelected { get; set; }


		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			IUrlHelper helper = _urlHelperFactory.GetUrlHelper(ViewContext);

			TagBuilder builder = new TagBuilder("div");

			for(int i = 1; i <= PageModel.TotalPages; i++)
			{

				TagBuilder tag = new TagBuilder("a");

				tag.Attributes["href"] = helper.Action(PageAction, new { page = i, sortBy = PageSortBy, searchString = PageSearch });

				if(PageClassesEnabled)
				{
					tag.AddCssClass(PageClass);
					
					if(PageModel.CurrentPage == i)
					{
						tag.AddCssClass(PageClassSelected);
					}
					else
					{
						tag.AddCssClass(PageClassNormal);
					}

				}

				tag.InnerHtml.Append(i.ToString());
				builder.InnerHtml.AppendHtml(tag);

			}

			output.Content.AppendHtml(builder.InnerHtml);

		}

	}
}
