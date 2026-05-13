using OfficeConnect_Web.Models;
using System.Web;
using System.Web.Mvc;

namespace OfficeConnect_Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthAttribute());
        }
    }
}
