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
    public class PropSourceController : Controller
    {
        //
        // GET: /PropSource/
        FortuneSoftEntities entity = new FortuneSoftEntities();
        Logger logger = LogManager.GetCurrentClassLogger();

        //[CustomExceptionHandlerFilter]
        [CustomAuthorization("Admin,Manager")]
        public ActionResult Index()
        {

            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            List<SourceModel> source = new List<SourceModel>();
            try
            {

                var getallsources = entity.tbl_Source.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
                foreach (var item in getallsources)
                {
                    SourceModel r = new SourceModel();
                    r.sourceid = item.SourceId;
                    r.sourcename = item.SourceName;
                    r.JanAmount = item.JanBudgtet;
                    r.FebAmount = item.FebBudgtet;
                    r.MarAmount = item.MarBudgtet;
                    r.AprAmount = item.AprBudgtet;
                    r.MayAmount = item.MayBudgtet;
                    r.JunAmount = item.JunBudgtet;
                    r.JulAmount = item.JulBudgtet;
                    r.AugAmount = item.AugBudgtet;
                    r.SepAmount = item.SepBudgtet;
                    r.OctAmount = item.OctBudgtet;
                    r.NovAmount = item.NovBudgtet;
                    r.DecAmount = item.DecBudgtet;
                    source.Add(r);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }

            return View(source);
        }
        [HttpGet]
        [CustomAuthorization("Admin,Manager")]
        public ActionResult AddSOurce()
        {
            SourceModel r = new SourceModel();
            return View(r);
        }
        [HttpPost]
        public ActionResult Submit(SourceModel model)
        {
            try
            {
                tbl_Source tblsource = new tbl_Source();
                tblsource.SourceName = model.sourcename;
                tblsource.JanBudgtet = model.JanAmount;
                tblsource.FebBudgtet = model.FebAmount;
                tblsource.MarBudgtet = model.MarAmount;
                tblsource.AprBudgtet = model.AprAmount;
                tblsource.MayBudgtet = model.MayAmount;
                tblsource.JunBudgtet = model.JunAmount;
                tblsource.JulBudgtet = model.JulAmount;
                tblsource.AugBudgtet = model.AugAmount;
                tblsource.SepBudgtet = model.SepAmount;
                tblsource.OctBudgtet = model.OctAmount;
                tblsource.NovBudgtet = model.NovAmount;
                tblsource.DecBudgtet = model.DecAmount;
                tblsource.CompId = model.companyid;
                tblsource.BrId = model.branchid;
                entity.tbl_Source.Add(tblsource);
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
            SourceModel r = new SourceModel();
            try
            {
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
                var empdatabyid = entity.tbl_Source.Where(m => m.SourceId == id && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).FirstOrDefault();

                r.sourceid = empdatabyid.SourceId;
                r.sourcename = empdatabyid.SourceName;
                r.JanAmount = empdatabyid.JanBudgtet;
                r.FebAmount = empdatabyid.FebBudgtet;
                r.MarAmount = empdatabyid.MarBudgtet;
                r.AprAmount = empdatabyid.AprBudgtet;
                r.MayAmount = empdatabyid.MayBudgtet;
                r.JunAmount = empdatabyid.JunBudgtet;
                r.JulAmount = empdatabyid.JulBudgtet;
                r.AugAmount = empdatabyid.AugBudgtet;
                r.SepAmount = empdatabyid.SepBudgtet;
                r.OctAmount = empdatabyid.OctBudgtet;
                r.NovAmount = empdatabyid.NovBudgtet;
                r.DecAmount = empdatabyid.DecBudgtet;
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
        public ActionResult Update(SourceModel s)
        {
            var data = entity.tbl_Source.Where(m => m.SourceId == s.sourceid && m.CompId == s.companyid && m.BrId == s.branchid).SingleOrDefault();

            if (data != null)
            {
                try
                {
                    data.SourceName = s.sourcename;
                    data.JanBudgtet = s.JanAmount;
                    data.FebBudgtet = s.FebAmount;
                    data.MarBudgtet = s.MarAmount;
                    data.AprBudgtet = s.AprAmount;
                    data.MayBudgtet = s.MayAmount;
                    data.JunBudgtet = s.JunAmount;
                    data.JulBudgtet = s.JulAmount;
                    data.AugBudgtet = s.AugAmount;
                    data.SepBudgtet = s.SepAmount;
                    data.OctBudgtet = s.OctAmount;
                    data.NovBudgtet = s.NovAmount;
                    data.DecBudgtet = s.DecAmount;
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

                var getname = entity.tbl_Source.Where(m => m.SourceId == id && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
                string sourcename = "";
                foreach (var item in getname)
                {
                    sourcename = item.SourceName;
                }
                int sourcecount = entity.tbl_EnquiryForm.Where(m => m.Reference == sourcename && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).Count();
                if (sourcecount == 0)
                {
                    var data = entity.tbl_Source.Where(m => m.SourceId == id).FirstOrDefault();
                    if (data != null)
                    {
                        entity.Entry(data).State = (System.Data.Entity.EntityState)System.Data.EntityState.Deleted;
                        entity.SaveChanges();
                    }
                }
                else
                {
                    TempData["source"] = "sourcedelete";
                    return RedirectToAction("Index");
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
