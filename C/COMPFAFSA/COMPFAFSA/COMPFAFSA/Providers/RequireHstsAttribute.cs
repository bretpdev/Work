using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace COMPFAFSA.Providers
{
    public class RequireHstsAttribute : RequireHttpsAttribute
    {
        private readonly uint _maxAge;

        public uint MaxAge { get { return _maxAge; } }

        public bool IncludeSubDomains { get; set; }

        public RequireHstsAttribute(uint maxAge)
            : base()
        {
            _maxAge = maxAge;
            IncludeSubDomains = false;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (filterContext.HttpContext.Request.IsSecureConnection)
            {
                StringBuilder headerBuilder = new StringBuilder();
                headerBuilder.AppendFormat("max-age={0}", _maxAge);

                if (IncludeSubDomains)
                {
                    headerBuilder.Append("; includeSubDomains");
                }

                filterContext.HttpContext.Response.AppendHeader("Strict-Transport-Security", headerBuilder.ToString());
            }
            else
            {
                HandleNonHttpsRequest(filterContext);
            }
        }
    }
}