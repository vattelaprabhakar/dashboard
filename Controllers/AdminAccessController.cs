using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FortuneTechPvtLtd.Models;
using FortuneTechPvtLtd.DataModel;
using System.Data.Entity.Validation;
using System.Diagnostics;
using RoleBasedSecurity.CustomSecurity;
using WebGrease;
using NLog;

namespace FortuneTechPvtLtd.Controllers
{
    public class AdminAccessController : Controller
    {
        //
        // GET: /AdminAccess/
        FortuneSoftEntities entity = new FortuneSoftEntities();
        Logger logger = NLog.LogManager.GetCurrentClassLogger();
        
    
        [CustomAuthorization("Admin,Manager")]
        public ActionResult Index()
        {
             int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
        int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

            List<RoleModel> rolemodel = new List<RoleModel>();
            try
            {

                var getallroles = entity.tbl_FortuneRoles.Where(x => x.CompId == CurrentCompanyId && x.BrId == CurrentCompanyBranchId).ToList();
                foreach (var item in getallroles)
                {
                    RoleModel r = new RoleModel();
                    r.companyid = item.CompId;
                    r.branchid = item.BrId;
                    r.roleid = item.RoleId;
                    r.rolename = item.RoleName;
                    rolemodel.Add(r);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            
            return View(rolemodel);
        }
        [HttpGet]
        [CustomAuthorization("Admin,Manager")]
        public ActionResult AddRole()
        {
            RoleModel r = new RoleModel();
            return View(r);
        }
        [HttpPost]
        public ActionResult Submit(RoleModel model)
        {
            try
            {
                tbl_FortuneRoles tblroles = new tbl_FortuneRoles();
                tblroles.CompId = model.companyid;
                tblroles.BrId = model.branchid;
                tblroles.RoleName = model.rolename;
                entity.tbl_FortuneRoles.Add(tblroles);
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
            RoleModel r = new RoleModel();
            try
            {
                 int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

                var empdatabyid = entity.tbl_FortuneRoles.Where(m => m.RoleId == id && m.CompId==CurrentCompanyId && m.BrId==CurrentCompanyBranchId).FirstOrDefault();

                r.companyid = empdatabyid.CompId;
                r.branchid = empdatabyid.BrId;
                r.roleid = empdatabyid.RoleId;
                r.rolename = empdatabyid.RoleName;
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return View(r);
        }
        [HttpPost]
        public ActionResult Update(RoleModel s)
        {
            var data = entity.tbl_FortuneRoles.Where(m => m.RoleId == s.roleid && m.CompId==s.companyid && m.BrId==s.branchid).SingleOrDefault();

            if (data!=null)
            {
                try
                {
                    data.RoleName = s.rolename;
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

                var data = entity.tbl_FortuneRoles.Where(m => m.RoleId == id && m.CompId == CurrentCompanyId && m.BrId==CurrentCompanyBranchId).FirstOrDefault();
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
