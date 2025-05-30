using System.Web;
using System.Web.Mvc;

namespace FortuneTechPvtLtd
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        //public class FilterConfig
        //{
        //    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        //    {
        //        filters.Add(new FortuneTechPvtLtd.CustomSecurity.CustomException.ExceptionHandlerAttribute());
        //    }
        //}
    }
}