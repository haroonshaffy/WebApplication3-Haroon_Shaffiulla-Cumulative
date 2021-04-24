using System.Web;
using System.Web.Mvc;

namespace WebApplication3_Haroon_Shaffiulla_Cumulative
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
