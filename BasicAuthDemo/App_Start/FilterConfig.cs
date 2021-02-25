using BasicAuthDemo.AuthAttribute;
using System.Web;
using System.Web.Mvc;

namespace BasicAuthDemo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            
        }
    }
}
