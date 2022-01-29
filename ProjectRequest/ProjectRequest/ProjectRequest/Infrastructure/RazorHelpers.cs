using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace System.Web.Mvc.Html
{
    public static class RazorHelpers
    {
        public static MvcHtmlString DropdownFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, SelectList selectList)
        {
            // get the metadata
            ModelMetadata fieldmetadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            // get the field name
            var name = ExpressionHelper.GetExpressionText(expression);

            // get value
            var value = fieldmetadata.Model.ToString();

            var button = new TagBuilder("button")
            {
                Attributes =
                {
                    { "id", name },
                    { "type", "button"},
                    { "data-toggle", "dropdown"}
                }
            };

            button.AddCssClass("btn");
            button.AddCssClass("btn-default");
            button.AddCssClass("dropdown-toggle");

            button.SetInnerText(value);
            button.InnerHtml += " " + BuildCaret();

            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("dropdown");

            wrapper.InnerHtml += button;
            wrapper.InnerHtml += BuildDropdown(value, selectList);

            return new MvcHtmlString(wrapper.ToString());
        }

        private static string BuildCaret()
        {
            var caret = new TagBuilder("span");
            caret.AddCssClass("caret");

            return caret.ToString();
        }

        private static string BuildDropdown(string id, SelectList items)
        {
            var list = new TagBuilder("ul")
            {
                Attributes =
            {
                {"class", "dropdown-menu"},
                {"role", "menu"},
                {"aria-labelledby", id}
            }
            };

            var listItem = new TagBuilder("li");
            listItem.Attributes.Add("role", "presentation");

            items.ForEach(x => list.InnerHtml += "<li role=\"presentation\">" + BuildListRow(x) + "</li>");
            return list.ToString();
        }

        private static string BuildListRow(SelectListItem item)
        {
            var anchor = new TagBuilder("a")
            {
                Attributes =
                {
                    { "role", "menuitem"},
                    { "tabindex", "-1"},
                    { "href", item.Value}
                }
            };

            anchor.SetInnerText(item.Text);

            return anchor.ToString();
        }
    }
}