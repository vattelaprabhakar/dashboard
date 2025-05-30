using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using FortuneTechPvtLtd.DataModel;

namespace RoleBasedSecurity.CustomSecurity
{
    public class CustomAuthorization :AuthorizeAttribute  
    {
        private readonly string[] allowedroles;
        public CustomAuthorization(params string[] roles)  
        {  
            this.allowedroles = roles;  
        }  
        protected override bool AuthorizeCore(HttpContextBase httpContext)  
        {  
            bool authorize = false;
            var userId = Convert.ToString(httpContext.Session["CurrentLoginUser"]);  
            if (!string.IsNullOrEmpty(userId))  
                using (var context = new FortuneSoftEntities())  
                {  
                    var userRole = (from u in context.tbl_FortuneUsers where u.IsActiveId==1 
                                    join r in context.tbl_FortuneRoles on u.RoleId equals r.RoleId  
                                    where u.EmailId == userId  
                                    select new  
                                    {  
                                        r.RoleName  
                                    }).FirstOrDefault();  
                    foreach (var role in allowedroles)  
                    {
                        string msg = role;

                        string[] strarr = msg.Split(',');
                        string rolename = "";
                      
                        for (int i = 0; i < strarr.Length; i++)
                        {
                            rolename = strarr[i];
                            if (rolename == userRole.RoleName)
                            {
                                return true;
                            }
                        }
                    }  
                }  
  
  
            return authorize;  
        }  
  
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)  
        {  
            filterContext.Result = new RedirectToRouteResult(  
               new RouteValueDictionary  
               {  
                    { "controller", "Error" },  
                    { "action", "UnAuthorize" }  
               });  
        }  
       
    }
}