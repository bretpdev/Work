using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace COMPFAFSA
{
    public static class HtmlHelperGroup
    {
        static string container = @"<div id=""{0}"">{1}</div>";
        static string checkboxHtml = @"<input type=""checkbox"" name=""{0}"" value=""{1}"" {3} /> {2}</br>";

        public static string CheckBoxGroup(this HtmlHelper obj, string name, IEnumerable<SelectListItem> data)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var content in data)
            {
                sb.Append(string.Format(checkboxHtml, name, content.Value, content.Text, content.Selected ? "checked" : string.Empty));
            }
            return string.Format(container, "container_" + name, sb.ToString());
        }
    }
}