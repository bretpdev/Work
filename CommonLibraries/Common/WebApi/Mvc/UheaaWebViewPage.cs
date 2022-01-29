using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Uheaa.Common.WebApi
{
    public abstract class UheaaWebViewPage<T, BAG, SES> : WebViewPage<T> where BAG : UheaaBag, new() where SES : UheaaSession, new()
    {
        public new BAG ViewBag
        {
            get
            {
                var bag = new BAG();
                bag.SetBag(base.ViewBag);
                return bag;
            }
        }
        public new SES Session
        {
            get
            {
                var ses = new SES();
                ses.SetSession(base.Session);
                return ses;
            }
        }
    }
    public abstract class UheaaWebViewPage<BAG, SES> : WebViewPage where BAG : UheaaBag, new() where SES : UheaaSession, new()
    {
        public new BAG ViewBag
        {
            get
            {
                var bag = new BAG();
                bag.SetBag(base.ViewBag);
                return bag;
            }
        }
        public new SES Session
        {
            get
            {
                var ses = new SES();
                ses.SetSession(base.Session);
                return ses;
            }
        }
    }
}