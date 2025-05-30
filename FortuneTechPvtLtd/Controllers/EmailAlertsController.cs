using FortuneTechPvtLtd.DataModel;
using FortuneTechPvtLtd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FortuneTechPvtLtd.Controllers
{
    public class EmailAlertsController : Controller
    {
        //
        // GET: /EmailAlerts/
        FortuneSoftEntities entity = new FortuneSoftEntities();
        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            List<EmailAlertsModel> emailalert = new List<EmailAlertsModel>();
            try
            {
                var alertobj = entity.tbl_alerts.Where(m => m.compId == CurrentCompanyId && m.brId == CurrentCompanyBranchId && m.alert_type.ToUpper() == "EMAIL".ToUpper()).ToList().OrderByDescending(m=>m.alert_Id);
                foreach (var item in alertobj)
                {
                    EmailAlertsModel r = new EmailAlertsModel();
                    r.AlertId = item.alert_Id;
                    r.AlertType = item.alert_type;
                    r.AlertCategory = item.alert_Category;
                    r.AlertMasterId = item.alert_mast_Id;
                    r.AlertMasterName = item.alert_mast__name;
                    r.AlertComments = item.alert_comments;
                    r.AlertFlag = item.alert_flag;
                    emailalert.Add(r);
                }
            }
            catch (Exception e)
            {
                logger.ErrorException("error occurred at", e);
            }
            return View(emailalert);
        }
        public ActionResult AddEmailAlerts()
        {
            Props();
            EmailAlertsModel r = new EmailAlertsModel();
            return View(r);
        }
        [HttpPost]
        public ActionResult AddEmailAlerts(EmailAlertsModel data, FormCollection formdata)
        {
            tbl_alerts alert = new tbl_alerts();
            alert.alert_mast__name = formdata["hdnalertmastname"];
            alert.alert_flag = formdata["hdnalertflag"];
            alert.alert_mast_Id =Convert.ToInt32( data.AlertMasterName);
            alert.alert_comments = data.AlertComments;
            alert.alert_type = "EMAIL";
            alert.alert_Category = "DAILY";
            alert.compId = data.CompId;
            alert.brId = data.BrId;
            entity.tbl_alerts.Add(alert);
            entity.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            EmailAlertsModel r = new EmailAlertsModel();
            try
            {
                Props();
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

                var data = entity.tbl_alerts.Where(m => m.alert_Id == id && m.compId == CurrentCompanyId && m.brId == CurrentCompanyBranchId).FirstOrDefault();
                if (data!=null)
                {
                    r.AlertId = data.alert_Id;
                    r.AlertMasterName = Convert.ToString(data.alert_mast_Id);
                    r.AlertFlag = data.alert_flag;
                    r.AlertComments = data.alert_comments;
                    r.CompId = data.compId;
                    r.BrId = data.brId;
                }
                   
                
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return View(r);
        }
        [HttpPost]
        public ActionResult Update(EmailAlertsModel data, FormCollection formdata)
        {
            var alert = entity.tbl_alerts.Where(m => m.alert_Id == data.AlertId && m.compId == data.CompId && m.brId == data.BrId).SingleOrDefault();

                try
                {
                    alert.alert_mast__name = formdata["hdnalertmastname"];
                    alert.alert_flag = formdata["hdnalertflag"];
                    alert.alert_mast_Id = data.AlertMasterId;
                    alert.alert_mast__name = formdata["hdnalertmastname"];
                    alert.alert_comments = data.AlertComments;
                    alert.alert_type = "EMAIL";
                    alert.alert_Category = "DAILY";
                    alert.compId = data.CompId;
                    alert.brId = data.BrId;
                    entity.Entry(alert);
                    entity.SaveChanges();
                }
                catch (Exception ex)
                {
                    logger.ErrorException("error occurred at", ex);
                }
            

            return RedirectToAction("Index");
        }
        public ActionResult Cancel()
        {
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            try
            {
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
                var data = entity.tbl_alerts.Where(m => m.alert_Id == id && m.compId == CurrentCompanyId && m.brId == CurrentCompanyBranchId).FirstOrDefault();
                entity.Entry(data).State = (System.Data.Entity.EntityState)System.Data.EntityState.Deleted;
                entity.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return RedirectToAction("Index");
        }
        private void Props()
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            //alert name from alertmaster
            var alertmaster = entity.tbl_alerts_master.Where(m => m.alert_master_type == "EMAIL").ToList();
            List<AlertMasterName> alert = new List<AlertMasterName>();
            foreach (var item in alertmaster)
            {
                AlertMasterName al = new AlertMasterName();
                al.AlertMastId = item.alert_master_Id;
                al.AlertMastName = item.alert_master__name;
                alert.Add(al);
            }
            ViewBag.alert = alert;

            List<EmailAlertStatus> alertstatus = new List<EmailAlertStatus>() { 
            new EmailAlertStatus(){ alertstatusid=1,alertstatusname="ACTIVE"},
               new EmailAlertStatus(){alertstatusid=2,alertstatusname="INACTIVE"},
            };
            ViewBag.alertstatus = alertstatus;
        }

    }
}
