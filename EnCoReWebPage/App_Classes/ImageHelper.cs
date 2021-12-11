using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnCoReWebPage.App_Classes
{
    public static class ImageHelper
    {
        public static MvcHtmlString Image(this HtmlHelper helper, string src, string altText, string height)
        {
            var builder = new TagBuilder("img");

            builder.MergeAttribute("src", src);

            builder.MergeAttribute("alt", altText);

            builder.MergeAttribute("height", height);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString Image(this HtmlHelper helper, string src, string altText, string width, string height)
        {
            var builder = new TagBuilder("img");

            builder.MergeAttribute("src", src);

            builder.MergeAttribute("alt", altText);

            builder.MergeAttribute("width", width);

            builder.MergeAttribute("height", height);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

    }
}