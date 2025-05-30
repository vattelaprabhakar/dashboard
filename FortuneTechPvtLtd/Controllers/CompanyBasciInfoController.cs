using FortuneTechPvtLtd.DataModel;
using FortuneTechPvtLtd.Models;
using NLog;
using RoleBasedSecurity.CustomSecurity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FortuneTechPvtLtd.Controllers
{
    public class CompanyBasciInfoController : Controller
    {
        //
        // GET: /CompanyBasciInfo/

        FortuneSoftEntities entity = new FortuneSoftEntities();
        Logger logger = LogManager.GetCurrentClassLogger();

        //[CustomExceptionHandlerFilter]
        [CustomAuthorization("Admin,Manager")]
        public ActionResult Index()
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            var count = entity.tbl_FortuneBasicInfo.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).Count();
            Session["basicinfocount"] = count;
            ViewBag.count = count;
            List<CompanyBasicDataModel> productmodel = new List<CompanyBasicDataModel>();
            try
            {

                var products = entity.tbl_FortuneBasicInfo.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
                if (products != null)
                {
                    foreach (var item in products)
                    {
                        CompanyBasicDataModel r = new CompanyBasicDataModel();
                        r.CompBasicId = item.basic_id;
                        r.companyid = item.CompId;
                        r.branchid = item.BrId;
                        r.organizationname = item.OrganizationName;
                        r.logopath = item.LogoPath;
                        r.organizationemail = item.OrgEmail;
                        r.organizationphone = item.OrgPhone;
                        r.smscount = item.SmsCount;
                        r.NumOfUserToAllow = item.NumberOfUserToAllowed;
                        r.todate = item.ToDate;
                        r.smsflag = item.sms_flag;
                        r.emailflag = item.email_flag;
                        r.analyticsflag = item.analytics_flag;
                        productmodel.Add(r);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }

            return View(productmodel);
        }
        [HttpGet]
        [CustomAuthorization("Admin,Manager")]
        public ActionResult AddBasicInfo()
        {
            properties();

            CompanyBasicDataModel r = new CompanyBasicDataModel();
            return View(r);
        }

        private void properties()
        {
            List<Alerts> alert = new List<Alerts>() { 
            new Alerts(){AlertId=1,AlertName="Yes"},
            new Alerts(){AlertId=2,AlertName="No",isselected=true},
            };
            ViewBag.alters = alert;
        }
        [HttpPost]
        public ActionResult Submit(CompanyBasicDataModel model, HttpPostedFileBase file,FormCollection f)
        {
            try
            {
                string fullpath = "";
                if (file == null)
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
                else if (file.ContentLength > 0)
                {
                    int MaxContentLength = 1024 * 1024 * 3; //3 MB
                    string[] AllowedFileExtensions = new string[] { ".jpg", ".png" };

                    if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                    {
                        ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                    }

                    else if (file.ContentLength > MaxContentLength)
                    {
                        ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                    }
                    else
                    {
                        //TO:DO
                        //var fileName = Path.GetFileName(file.FileName);
                        //var root = "~/ProjectContent/";
                        //var paths = new string[] { root, fileName };
                        //path = Path.Combine(Server.MapPath("~/ProjectContent/"), fileName);
                     //   fullpath = Path.Combine(Server.MapPath("~/ProjectContent"), Path.GetFileName(file.FileName));
                      //  file.SaveAs(fullpath);

                        //upload to db
                        byte[] bytes;
                        using (BinaryReader br = new BinaryReader(file.InputStream))
                        {
                            bytes = br.ReadBytes(file.ContentLength);
                        }


                        string fileName = System.IO.Path.GetFileName(file.FileName);

                        //Set the Image File Path.
                        fullpath = "~/ProjectContent/" + fileName;


                        tbl_FortuneBasicInfo tblbasicinfo = new tbl_FortuneBasicInfo();
                        tblbasicinfo.CompId = model.companyid;
                        tblbasicinfo.BrId = model.branchid;
                        tblbasicinfo.OrganizationName = model.organizationname;
                        tblbasicinfo.LogoPath = fullpath;
                        tblbasicinfo.FromDate = model.fromdate == null ? (DateTime?)null : Convert.ToDateTime(model.fromdate);
                        tblbasicinfo.ToDate = model.todate == null ? (DateTime?)null : Convert.ToDateTime(model.todate);
                        tblbasicinfo.NumberOfUserToAllowed = model.NumOfUserToAllow;
                        tblbasicinfo.OrgLogo = bytes;
                        //newly added
                        tblbasicinfo.sms_flag = f["hdnseletedwantsmsalerts"];
                        tblbasicinfo.email_flag = f["hdnseletedwantmailalerts"];
                        tblbasicinfo.analytics_flag = f["hdnseletedwantanalytics"];
                            //  tblbasicinfo.SmsCount = model.smscount;

                        entity.tbl_FortuneBasicInfo.Add(tblbasicinfo);
                        entity.SaveChanges();

                        //Save the Image File in Folder.
                        file.SaveAs(Server.MapPath(fullpath));
                        //ModelState.Clear();
                        //ViewBag.Message = "File uploaded successfully";
                    }
                }


              
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
            CompanyBasicDataModel r = new CompanyBasicDataModel();
            try
            {
                var empdatabyid = entity.tbl_FortuneBasicInfo.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId && m.basic_id == id).FirstOrDefault();

                r.companyid = empdatabyid.CompId;
                r.branchid = empdatabyid.BrId;
                r.CompBasicId = empdatabyid.basic_id;
                r.organizationname = empdatabyid.OrganizationName;
                r.logopath = empdatabyid.LogoPath;
                r.organizationemail = empdatabyid.OrgEmail;
                r.organizationphone = empdatabyid.OrgPhone;
                r.NumOfUserToAllow = empdatabyid.NumberOfUserToAllowed;
                r.OrganizationLogo = empdatabyid.OrgLogo;
                //organization logo must be specified in choose file 
                byte[] binaryString = (byte[])r.OrganizationLogo;
                if (binaryString!=null)
                {
                    // if the original encoding was ASCII
                    string x = Encoding.ASCII.GetString(binaryString);

                    // if the original encoding was UTF-8
                    string y = Encoding.UTF8.GetString(binaryString);
                }
           

                //  r.smscount = empdatabyid.SmsCount;
                //r.fromdate = empdatabyid.FromDate;
                //r.todate = empdatabyid.ToDate;
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return View(r);
        }
        [HttpPost]
        public ActionResult Update(CompanyBasicDataModel s, HttpPostedFileBase file,FormCollection f)
        {
            string fullpath = "";
            var data = entity.tbl_FortuneBasicInfo.Where(m => m.basic_id == s.CompBasicId && m.BrId == s.branchid && m.CompId == s.companyid).SingleOrDefault();

            if (data != null)
            {
                try
                {
                    if (file == null)
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                    else if (file.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 3; //3 MB
                        string[] AllowedFileExtensions = new string[] { ".jpg", ".png" };

                        if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                        {
                            ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                        }

                        else if (file.ContentLength > MaxContentLength)
                        {
                            ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                        }
                        else
                        {
                            string fileName = System.IO.Path.GetFileName(file.FileName);

                            //Set the Image File Path.
                            fullpath = "~/ProjectContent/" + fileName;

                            //Save the Image File in Folder.
                            file.SaveAs(Server.MapPath(fullpath));
                            
                            //TO:DO
                         //   var fileName = Path.GetFileName(file.FileName);
                          //  var root = "ProjectContent/";
                          //var  paths = new string[] { root, fileName };
                          //   fullpath = Path.Combine(Server.MapPath("~/ProjectContent"), fileName);
                          // fullpath = Path.Combine(paths);
                         //   fullpath = Path.Combine(Server.MapPath("~/ProjectContent"), Path.GetFileName(file.FileName));
                          //  file.SaveAs(fullpath);
                            ModelState.Clear();
                            ViewBag.Message = "File uploaded successfully";
                        }
                    }
                    data.CompId = s.companyid;
                    data.BrId = s.branchid;
                    data.OrganizationName = s.organizationname;
                    data.LogoPath = fullpath;
                    data.OrgEmail = s.organizationemail;
                    data.OrgPhone = s.organizationphone;
                    //data.FromDate = s.fromdate == null ? (DateTime?)null : Convert.ToDateTime(s.fromdate);
                    //data.ToDate = s.todate == null ? (DateTime?)null : Convert.ToDateTime(s.todate);
                    data.NumberOfUserToAllowed = s.NumOfUserToAllow;
                    data.sms_flag = f["hdnseletedwantsmsalerts"];
                    data.email_flag = f["hdnseletedwantmailalerts"];
                    data.analytics_flag = f["hdnseletedwantanalytics"];
                    //  data.SmsCount = s.smscount;
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
                var data = entity.tbl_FortuneBasicInfo.Where(m => m.basic_id == id && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).FirstOrDefault();
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
