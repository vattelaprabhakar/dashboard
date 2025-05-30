using FortuneTechPvtLtd.DataModel;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace FortuneTechPvtLtd
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

           WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
           // FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);


        }
        //protected void Session_Start(object sender, EventArgs e)
        //{
        //    Session.Timeout = 300;
        //}
        //protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        //{
        //    HttpCookie cookie = Request.Cookies.Get(FormsAuthentication.FormsCookieName);
        //    if (cookie != null)
        //    {
        //        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
        //        if (ticket.IsPersistent)
        //        {
        //            if (User.Identity.IsAuthenticated)
        //            {
        //                string UData = ticket.UserData;
        //                //split the values and stores into string array
        //                string[] userdata = UData.Split('|');
        //                CustomPrinciple customprincipal = new CustomPrinciple(userdata[0]);
        //                customprincipal.password = userdata[1];
        //                customprincipal.rolename = userdata[2];
        //                customprincipal.compid = Convert.ToInt32(userdata[3]);
        //                customprincipal.branid = Convert.ToInt32(userdata[4]);
        //                HttpContext.Current.User = customprincipal;


                        
        //            }
        //        }
        //    }
        //}
      

        
    }
}