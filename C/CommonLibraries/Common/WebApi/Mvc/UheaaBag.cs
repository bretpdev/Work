using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Uheaa.Common.WebApi
{
    /// <summary>
    /// A strongly-typed wrapper around the ViewBag object.
    /// </summary>
    public class UheaaBag
    {
        protected dynamic bag;
        public void SetBag(dynamic bag)
        {
            this.bag = bag;
        }

        public string Title
        {
            get
            {
                return bag.Title;
            }
            set
            {
                bag.Title = value;
            }
        }

        public Breadcrumbs Breadcrumbs
        {
            get
            {
                if (bag.Breadcrumbs == null)
                    bag.Breadcrumbs = new Breadcrumbs();
                return bag.Breadcrumbs;
            }
        }
    }
}