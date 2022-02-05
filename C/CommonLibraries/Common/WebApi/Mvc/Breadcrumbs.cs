using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uheaa.Common.WebApi
{
    public class Breadcrumbs : IEnumerable<Breadcrumbs.Breadcrumb>
    {
        public static Breadcrumb Dashboard => new Breadcrumb() { Text = "Dashboard", Url = "/" };
        public static Breadcrumb Home => new Breadcrumb() { Text = "Home", Url = "/" };

        private List<Breadcrumb> crumbs = new List<Breadcrumb>();
        public void Clear()
        {
            crumbs.Clear();
        }
        public Breadcrumbs Add(string text, string url)
        {
            var crumb = new Breadcrumb()
            {
                Text = text,
                Url = url
            };
            return Add(crumb);
        }
        public Breadcrumbs Add(Breadcrumb crumb)
        {
            crumbs.Add(crumb);
            return this;
        }

        public IEnumerator<Breadcrumb> GetEnumerator()
        {
            return crumbs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class Breadcrumb
        {
            public string Text { get; set; }
            public string Url { get; set; }
        }
    }
}