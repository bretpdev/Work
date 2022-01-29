using System.Web.Mvc;
using Uheaa.Common.WebApi;

namespace CSHRCPTFED
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
