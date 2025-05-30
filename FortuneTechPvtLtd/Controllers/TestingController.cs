
using RoleBasedSecurity.CustomSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FortuneTechPvtLtd.DataModel;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.EntityClient;


namespace FortuneTechPvtLtd.Controllers
{
    public class TestingController : Controller
    {
        //
        // GET: /Account/
        FortuneSoftEntities entity = new FortuneSoftEntities();
        
      
       
        public ActionResult Index()
        {
            //Session["currenttime"] = DateTime.Now.ToString();
            //ViewBag.time = Session["currenttime"].ToString();
           
            int sessionTimeout = Session.Timeout;
            DateTime timeoutDate = DateTime.Now.AddMinutes(sessionTimeout);
            Session["crt"] = DateTime.Now;
            ViewBag.cur = Session["crt"].ToString();
            ViewBag.time = timeoutDate;
            return View();
        }
        [CustomAuthorization("Manager")]
        public ActionResult manager()
        {
            return View();
        }
        [CustomAuthorization("Staff")]
        public ActionResult staff()
        {
            return View();
        }
        [CustomAuthorization("Executieve")]
        public ActionResult executieve()
        {
            return View();
        }
         //[CustomExceptionHandlerFilter]
        public ActionResult LoggingTest()
        {

            
            ////EntityConnectionStringBuilder entityConnection = new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FortuneSoftEntities"].ConnectionString);
            ////SqlConnection connectionString = new SqlConnection(entityConnection.ProviderConnectionString);
            //try
            //{
            //    connectionString.Open();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}  
            try
            {
                int x = 0;
                int y = 5;
                int z = y / x;
            }
            catch (Exception ex)
            {
                throw ex;

            }  
            return View();
        }

    }
}
