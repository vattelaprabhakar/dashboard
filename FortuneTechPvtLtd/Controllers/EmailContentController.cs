using FortuneTechPvtLtd.DataModel;
using FortuneTechPvtLtd.Models;
using NLog;
using RoleBasedSecurity.CustomSecurity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FortuneTechPvtLtd.Controllers
{
    public class EmailContentController : Controller
    {
        //
        // GET: /EmailContent/
        FortuneSoftEntities entity = new FortuneSoftEntities();
        Logger logger = LogManager.GetCurrentClassLogger(); 

        //[CustomExceptionHandlerFilter]
        [CustomAuthorization("Admin,Manager")]
        public ActionResult Index()
        {
            List<EmailModel> emailmodel = new List<EmailModel>();
            try
            {
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

                var getallmailcontent = entity.tbl_LeadEmailContent.Where(m => m.CompId == CurrentCompanyId && m.BrId==CurrentCompanyBranchId).ToList();
                foreach (var item in getallmailcontent)
                {
                    EmailModel r = new EmailModel();
                    r.companyid = item.CompId;
                    r.branchid = item.BrId;
                    r.emailid = item.Id;
                    r.emailsubject = item.EmailSubject;
                    r.emailbody = item.EmailBody;
                    r.interestedcourse = item.InterestedCourse;
                    r.leadstutus = item.leadstatus;
                    emailmodel.Add(r);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }

            return View(emailmodel);
        }
        [HttpGet]
        [CustomAuthorization("Admin,Manager")]
        public ActionResult AddNewEmailContent()
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            var products = entity.tbl_Productlist.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            List<Product> product = new List<Product>();
            foreach (var item in products)
            {
                Product prod = new Product();
                prod.ProductId = item.ProductId;
                prod.ProductName = item.ProductName;
                product.Add(prod);
            }
            ViewBag.products = product;



            //lead status
            var status = entity.tbl_LeadStatus.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();

            List<Leadstatus> stustatus = new List<Leadstatus>();
            foreach (var item in status)
            {
                Leadstatus l = new Leadstatus();
                l.StatusId = item.StatusId;
                l.StatusName = item.StatusName;
                stustatus.Add(l);
            }
            ViewBag.stustatus = stustatus;
            EmailModel r = new EmailModel();
            return View(r);
        }
        [HttpPost]
        public ActionResult Submit(EmailModel model,FormCollection f)
        {
            try
            {
                tbl_LeadEmailContent tblemail = new tbl_LeadEmailContent();
                tblemail.EmailSubject = model.emailsubject;
                tblemail.EmailBody = model.emailbody;
                tblemail.CompId = model.companyid;
                tblemail.BrId = model.branchid;
                tblemail.InterestedCourse = f["hdnselectedinterestedcourse"];
                tblemail.leadstatus = f["hdnselectedstatus"];
                entity.tbl_LeadEmailContent.Add(tblemail);
                entity.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
                logger.ErrorException("error occurred at", dbEx);

            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return RedirectToAction("Index");
        }
        //[CustomExceptionHandlerFilter]
        [CustomAuthorization("Admin,Manager")]
        public ActionResult Edit(int id)
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            var products = entity.tbl_Productlist.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            List<Product> product = new List<Product>();
            foreach (var item in products)
            {
                Product prod = new Product();
                prod.ProductId = item.ProductId;
                prod.ProductName = item.ProductName;
                product.Add(prod);
            }
            ViewBag.products = product;



            //lead status
            var status = entity.tbl_LeadStatus.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();

            List<Leadstatus> stustatus = new List<Leadstatus>();
            foreach (var item in status)
            {
                Leadstatus l = new Leadstatus();
                l.StatusId = item.StatusId;
                l.StatusName = item.StatusName;
                stustatus.Add(l);
            }
            ViewBag.stustatus = stustatus;
            EmailModel r = new EmailModel();
            try
            {
                 

            var empdatabyid = entity.tbl_LeadEmailContent.Where(m => m.Id == id && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).FirstOrDefault();

                r.emailid = empdatabyid.Id;
                r.branchid = empdatabyid.BrId;
                r.companyid = empdatabyid.CompId;
                r.emailsubject = empdatabyid.EmailSubject;
                r.emailbody = empdatabyid.EmailBody;
                r.interestedcourse = empdatabyid.InterestedCourse;
                r.leadstutus = empdatabyid.leadstatus;
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return View(r);
        }
        [HttpPost]
        public ActionResult Update(EmailModel s,FormCollection f)
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            var data = entity.tbl_LeadEmailContent.Where(m => m.Id == s.emailid &&  m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).SingleOrDefault();

            if (data != null)
            {
                try
                {
                    data.CompId = s.companyid;
                    data.BrId = s.branchid;
                    data.EmailSubject = s.emailsubject;
                    data.EmailBody = s.emailbody;
                    data.InterestedCourse = f["hdnselectedinterestedcourse"];
                    data.leadstatus = f["hdnselectedstatus"];
                    entity.Entry(data);
                    entity.SaveChanges();
                }
                catch (Exception ex)
                {
                    logger.ErrorException("error occurred at", ex);
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("UnAuthorize", "Error");
        }
        public ActionResult Cancel()
        {
            return RedirectToAction("Index");
        }
        //[CustomExceptionHandlerFilter]
        [CustomAuthorization("Admin,Manager")]
        public ActionResult Delete(int id)
        {
            try
            {
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

                var data = entity.tbl_LeadEmailContent.Where(m => m.Id == id && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).FirstOrDefault();
                entity.Entry(data).State = (System.Data.Entity.EntityState)System.Data.EntityState.Deleted;
                entity.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return RedirectToAction("Index");
        }
    }
}
