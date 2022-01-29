using System.Web;
using System.Web.Mvc;
using Uheaa.Common.WebApi;

namespace SCHRPT
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
