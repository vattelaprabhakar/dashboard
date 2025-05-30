//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using FortuneTechPvtLtd.DataModel;
//using System.Data.Entity.Validation;
//using System.Diagnostics;

//namespace FortuneTechPvtLtd.CustomSecurity
//{
//    public class CustomExceptionHandlerFilter : FilterAttribute, IExceptionFilter
//    {
//        //public void OnException(ExceptionContext filterContext)
//        //{
//        //    try
//        //    {
//        //        //if (filterContext.HttpContext.Session["CurrentUser"].ToString() == null)
//        //        //{
//        //        //    HttpContext.Current.Response.Redirect("~/home/Index");
//        //        //}
//        //        tbl_Logs logger = new tbl_Logs()
//        //        {
                    
//        //            UserName = filterContext.HttpContext.Session["CurrentUser"].ToString(),
//        //          IpAddress = filterContext.HttpContext.Request.UserHostAddress,
//        //            ExceptionMessage = filterContext.Exception.Message,
//        //            StackTrace = filterContext.Exception.StackTrace,
//        //            ControllerName = filterContext.RouteData.Values["controller"].ToString(),
//        //            ActionName = filterContext.RouteData.Values["action"].ToString(),
//        //            DateTime = DateTime.Now
//        //        };
//        //        FortuneSoftEntities entity = new FortuneSoftEntities();
//        //        entity.tbl_Logs.Add(logger);
//        //        entity.SaveChanges();

//        //        filterContext.ExceptionHandled = true;
//        //        filterContext.Result = new ViewResult()
//        //        {
//        //            ViewName = "Error"
//        //        };
//        //    }
//        //    catch (DbEntityValidationException dbEx)
//        //    {
//        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
//        //        {
//        //            foreach (var validationError in validationErrors.ValidationErrors)
//        //            {
//        //                Trace.TraceInformation("Property: {0} Error: {1}",
//        //                                        validationError.PropertyName,
//        //                                        validationError.ErrorMessage);
//        //            }
//        //        }
//        //    }
//        //}
//    }  
//}