using FortuneTechPvtLtd.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoleBasedSecurity.CustomSecurity;
using FortuneTechPvtLtd.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;
using NLog;

namespace FortuneTechPvtLtd.Controllers
{
    public class SMSContentController : Controller
    {
        //
        // GET: /SMSContent/
        FortuneSoftEntities entity = new FortuneSoftEntities();
        Logger logger = LogManager.GetCurrentClassLogger();

        //[CustomExceptionHandlerFilter]
        [CustomAuthorization("Admin,Manager")]
        public ActionResult Index()
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            var count = entity.tbl_SmsContent.Where(m=>m.CompId==CurrentCompanyId && m.BrId==CurrentCompanyBranchId).Count();
            Session["smscount"] = count;
            List<SMSContentModel> smsmodel = new List<SMSContentModel>();
            try
            {

                var getsmscontent = entity.tbl_SmsContent.Where(m=>m.CompId==CurrentCompanyId && m.BrId==CurrentCompanyBranchId).ToList();
                if (getsmscontent!=null)
                {
                    foreach (var item in getsmscontent)
                    {
                        SMSContentModel r = new SMSContentModel();
                        r.smsid = item.SmsId;
                        r.interestedcourse = item.InterestedCourse;
                        r.leadstutus = item.leadstatus;
                        r.smscontent = item.SmsContent;
                        smsmodel.Add(r);
                    }
                }
                
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }

            return View(smsmodel);
        }
        [HttpGet]
        [CustomAuthorization("Admin,Manager")]
        public ActionResult AddSmsContent()
        {
             int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            var products = entity.tbl_Productlist.Where(m => m.CompId == CurrentCompanyId && m.BrId==CurrentCompanyBranchId).ToList();
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
            SMSContentModel r = new SMSContentModel();
            return View(r);
        }
        [HttpPost]
        public ActionResult Submit(SMSContentModel model,FormCollection f)
        {
            try
            {
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
                tbl_SmsContent tblbasicinfo = new tbl_SmsContent();
                tblbasicinfo.InterestedCourse = f["hdnselectedinterestedcourse"];
                tblbasicinfo.leadstatus = f["hdnselectedstatus"];
                tblbasicinfo.SmsContent = model.smscontent;
                tblbasicinfo.CompId = CurrentCompanyId;
                tblbasicinfo.BrId = CurrentCompanyBranchId;
                entity.tbl_SmsContent.Add(tblbasicinfo);
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
            SMSContentModel r = new SMSContentModel();
            try
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
                var empdatabyid = entity.tbl_SmsContent.Where(m => m.SmsId == id && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).FirstOrDefault();

                r.smsid = empdatabyid.SmsId;
                r.smscontent = empdatabyid.SmsContent;
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
        public ActionResult Update(SMSContentModel s,FormCollection f)
        {
            var data = entity.tbl_SmsContent.Where(m => m.SmsId == s.smsid && m.CompId==s.companyid && m.BrId==s.branchid).SingleOrDefault();

            if (data != null)
            {
                try
                {
                    int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                    int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
                    data.SmsContent = s.smscontent;
                    data.CompId = CurrentCompanyId;
                    data.BrId = CurrentCompanyBranchId;
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

                var data = entity.tbl_SmsContent.Where(m => m.SmsId ==id && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).FirstOrDefault();
                if (data!=null)
                {
                    entity.Entry(data).State = (System.Data.Entity.EntityState)System.Data.EntityState.Deleted;
                    entity.SaveChanges();
                }
                
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return RedirectToAction("Index");
        }

    }
}
