using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FortuneTechPvtLtd.Models;
using FortuneTechPvtLtd.DataModel;
using RoleBasedSecurity.CustomSecurity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using NLog;

namespace FortuneTechPvtLtd.Controllers
{
    public class PropStatusController : Controller
    {
        //
        // GET: /PropStatus/

        FortuneSoftEntities entity = new FortuneSoftEntities();
        Logger logger = LogManager.GetCurrentClassLogger();

        //[CustomExceptionHandlerFilter]
        [CustomAuthorization("Admin,Manager")]
        public ActionResult Index()
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            List<StatusModel> statusmodel = new List<StatusModel>();
            try
            {

                var getallstatus = entity.tbl_LeadStatus.Where(m=>m.CompId==CurrentCompanyId && m.BrId==CurrentCompanyBranchId).ToList();
                foreach (var item in getallstatus)
                {
                    StatusModel r = new StatusModel();
                    r.statusid = item.StatusId;
                    r.statusname = item.StatusName;
                    statusmodel.Add(r);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }

            return View(statusmodel);
        }
        [HttpGet]
        [CustomAuthorization("Admin,Manager")]
        public ActionResult AddStatus()
        {
            StatusModel r = new StatusModel();
            return View(r);
        }
        [HttpPost]
        public ActionResult Submit(StatusModel model)
        {
            try
            {
                tbl_LeadStatus tblstatus = new tbl_LeadStatus();
                tblstatus.StatusName = model.statusname;
                tblstatus.CompId = model.companyid;
                tblstatus.BrId = model.branchid;
                entity.tbl_LeadStatus.Add(tblstatus);
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
     
        [CustomAuthorization("Admin,Manager")]
        public ActionResult Edit(int id)
        {
            StatusModel r = new StatusModel();
            try
            {
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

                var empdatabyid = entity.tbl_LeadStatus.Where(m => m.StatusId == id && m.CompId==CurrentCompanyId && m.BrId==CurrentCompanyBranchId).FirstOrDefault();

                r.statusid = empdatabyid.StatusId;
                r.statusname = empdatabyid.StatusName;
                r.companyid = empdatabyid.CompId;
                r.branchid = empdatabyid.BrId;
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return View(r);
        }
        [HttpPost]
        public ActionResult Update(StatusModel s)
        {
            var data = entity.tbl_LeadStatus.Where(m => m.StatusId == s.statusid && m.CompId==s.companyid && m.BrId==s.branchid).SingleOrDefault();

            if (data != null)
            {
                try
                {
                    data.StatusName = s.statusname;
                    data.CompId = s.companyid;
                    data.BrId = s.branchid;
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

                var data = entity.tbl_LeadStatus.Where(m => m.StatusId == id && m.CompId==CurrentCompanyId && m.BrId==CurrentCompanyBranchId).FirstOrDefault();
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
