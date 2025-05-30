using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FortuneTechPvtLtd.DataModel;
using FortuneTechPvtLtd.Models;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using NLog;
namespace FortuneTechPvtLtd.Controllers
{
    public class ReportsController : Controller
    {
        //
        // GET: /Reports/
        FortuneSoftEntities entity = new FortuneSoftEntities();
        Logger logger = LogManager.GetCurrentClassLogger();

        //[CustomExceptionHandlerFilter]
        public ActionResult Index()
        {
            //List<Reportsparameters> reportparameters = new List<Reportsparameters>() { 

            //new Reportsparameters(){ParaId=1,ParaName="Today's",isselected=false},
            //new Reportsparameters(){ParaId=2,ParaName="DateWise",isselected=false},
            //new Reportsparameters(){ParaId=3,ParaName="StatusWise",isselected=false},
            //new Reportsparameters(){ParaId=4,ParaName="ProductWise",isselected=false},
            // new Reportsparameters(){ParaId=5,ParaName="LeadOwnerWise",isselected=false},
            //  new Reportsparameters(){ParaId=6,ParaName="ReferenceWise",isselected=false},
            //            };
            //Reportsparameters reports = new Reportsparameters();
            //ViewBag.reportparameters = reportparameters;
            ReportsModel model = new ReportsModel();

            return View(model);

        }
        public ActionResult GetDatewise()
        {

            ReportsModel model = new ReportsModel();
            return View(model);

        }
        //[CustomExceptionHandlerFilter]
        //public ActionResult GetCurrentDateRecords()
        //{
        //    return View();
        //}
        //[CustomExceptionHandlerFilter]
        public JsonResult Getdatewisereports(DateTime fromdate, DateTime todate)
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            // DateTime fdate = System.Convert.ToDateTime(fromdate.ToString("yyyy-MM-dd"));

            var totaldata = entity.tbl_EnquiryForm.Where(m => m.Created_Date >= fromdate && m.Created_Date <= todate).Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            List<EnquiryModel> data = new List<EnquiryModel>();

            foreach (var item in totaldata)
            {
                EnquiryModel studentmodel = new EnquiryModel();
                studentmodel.Id = item.Id;
                studentmodel.LeadName = item.LeadName;
                studentmodel.Mobile = item.Mobile;
                studentmodel.Email = item.Email;
                studentmodel.Address = item.Address;
                studentmodel.selectedinterestedcourse = item.Product;
                studentmodel.selectedrefby = item.Reference;
                studentmodel.seletedwantsmsalerts = item.WantSMSAlerts;
                studentmodel.seletedwantmailalerts = item.WantMailAlerts;
                studentmodel.selectedstatus = item.Status;
                studentmodel.LeadAssignedby = item.LeadAssignedby;
                studentmodel.selectedleadassignedto = item.LeadAssignedTo;

                studentmodel.Website = item.Website;
                studentmodel.Industry = item.Industry;
                studentmodel.Rating = item.Rating;
                studentmodel.LeadOwner = item.LeadOwner;
                studentmodel.Leaddate = item.LeadDate;
                studentmodel.Followupdate = item.FollowUpdate;
                studentmodel.Comment1 = item.Comment1;
                studentmodel.Comment2 = item.Comment2;
                studentmodel.Comment3 = item.Comment3;
                data.Add(studentmodel);

            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //[CustomExceptionHandlerFilter]
        public JsonResult GetRecCurrent()
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            //var currentdate = DateTime.Now.ToString("yyyy-mm-dd");
            DateTime currentdate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

            var studentdata = entity.tbl_EnquiryForm.Where(m => m.LeadDate == currentdate).Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            List<EnquiryModel> data = new List<EnquiryModel>();

            foreach (var item in studentdata)
            {
                EnquiryModel studentmodel = new EnquiryModel();
                studentmodel.Id = item.Id;
                studentmodel.LeadName = item.LeadName;
                studentmodel.Mobile = item.Mobile;
                studentmodel.Email = item.Email;
                studentmodel.Address = item.Address;
                studentmodel.selectedinterestedcourse = item.Product;
                studentmodel.selectedrefby = item.Reference;
                studentmodel.Leaddate = item.LeadDate == null ? (DateTime?)null : Convert.ToDateTime(item.LeadDate);
                studentmodel.Followupdate = item.FollowUpdate == null ? (DateTime?)null : Convert.ToDateTime(item.FollowUpdate);
                studentmodel.selectedstatus = item.Status;
                studentmodel.LeadAssignedby = item.LeadAssignedby;
                studentmodel.selectedleadassignedto = item.LeadAssignedTo;
                studentmodel.seletedwantsmsalerts = item.WantSMSAlerts;
                studentmodel.seletedwantmailalerts = item.WantMailAlerts;
                studentmodel.Website = item.Website;
                studentmodel.Industry = item.Industry;
                studentmodel.Rating = item.Rating;
                studentmodel.LeadOwner = item.LeadOwner;
                studentmodel.Comment1 = item.Comment1;
                studentmodel.Comment2 = item.Comment2;
                studentmodel.Comment3 = item.Comment3;

                data.Add(studentmodel);

            }

            return Json(data, JsonRequestBehavior.AllowGet);


            //SqlParameter param1 = new SqlParameter("@LeadName", studentmodel.LeadName);
            //SqlParameter param2 = new SqlParameter("@Address", studentmodel.Address);

        }
        public ActionResult ProductWise()
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
            return View();
        }

        //[CustomExceptionHandlerFilter]
        public JsonResult GetProductWise(string product)
        {

            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            //var currentdate = DateTime.Now.ToString("yyyy-mm-dd");
            DateTime currentdate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

            var studentdata = entity.tbl_EnquiryForm.Where(m => m.Product == product).Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            List<EnquiryModel> data = new List<EnquiryModel>();

            foreach (var item in studentdata)
            {
                EnquiryModel studentmodel = new EnquiryModel();
                studentmodel.Id = item.Id;
                studentmodel.LeadName = item.LeadName;
                studentmodel.Mobile = item.Mobile;
                studentmodel.Email = item.Email;
                studentmodel.Address = item.Address;
                studentmodel.selectedinterestedcourse = item.Product;
                studentmodel.selectedrefby = item.Reference;
                studentmodel.Leaddate = item.LeadDate == null ? (DateTime?)null : Convert.ToDateTime(item.LeadDate);
                studentmodel.Followupdate = item.FollowUpdate == null ? (DateTime?)null : Convert.ToDateTime(item.FollowUpdate);
                studentmodel.selectedstatus = item.Status;
                studentmodel.LeadAssignedby = item.LeadAssignedby;
                studentmodel.selectedleadassignedto = item.LeadAssignedTo;
                studentmodel.seletedwantsmsalerts = item.WantSMSAlerts;
                studentmodel.seletedwantmailalerts = item.WantMailAlerts;
                studentmodel.Website = item.Website;
                studentmodel.Industry = item.Industry;
                studentmodel.Rating = item.Rating;
                studentmodel.LeadOwner = item.LeadOwner;
                studentmodel.Comment1 = item.Comment1;
                studentmodel.Comment2 = item.Comment2;
                studentmodel.Comment3 = item.Comment3;

                data.Add(studentmodel);

            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StatusWise()
        {

            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            var sta = entity.tbl_LeadStatus.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            List<Leadstatus> status = new List<Leadstatus>();
            foreach (var item in sta)
            {
                Leadstatus sta1 = new Leadstatus();
                sta1.StatusId = item.StatusId;
                sta1.StatusName = item.StatusName;
                status.Add(sta1);
            }
            ViewBag.status = status;
            return View();
        }
        //[CustomExceptionHandlerFilter]
        public JsonResult GetStatusWise(string status)
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            //var currentdate = DateTime.Now.ToString("yyyy-mm-dd");
            DateTime currentdate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

            var studentdata = entity.tbl_EnquiryForm.Where(m => m.Status == status).Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            List<EnquiryModel> data = new List<EnquiryModel>();

            foreach (var item in studentdata)
            {
                EnquiryModel studentmodel = new EnquiryModel();
                studentmodel.Id = item.Id;
                studentmodel.LeadName = item.LeadName;
                studentmodel.Mobile = item.Mobile;
                studentmodel.Email = item.Email;
                studentmodel.Address = item.Address;
                studentmodel.selectedinterestedcourse = item.Product;
                studentmodel.selectedrefby = item.Reference;
                studentmodel.Leaddate = item.LeadDate == null ? (DateTime?)null : Convert.ToDateTime(item.LeadDate);
                studentmodel.Followupdate = item.FollowUpdate == null ? (DateTime?)null : Convert.ToDateTime(item.FollowUpdate);
                studentmodel.selectedstatus = item.Status;
                studentmodel.LeadAssignedby = item.LeadAssignedby;
                studentmodel.selectedleadassignedto = item.LeadAssignedTo;
                studentmodel.seletedwantsmsalerts = item.WantSMSAlerts;
                studentmodel.seletedwantmailalerts = item.WantMailAlerts;
                studentmodel.Website = item.Website;
                studentmodel.Industry = item.Industry;
                studentmodel.Rating = item.Rating;
                studentmodel.LeadOwner = item.LeadOwner;
                studentmodel.Comment1 = item.Comment1;
                studentmodel.Comment2 = item.Comment2;
                studentmodel.Comment3 = item.Comment3;

                data.Add(studentmodel);

            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //full name of leadowner bind to drp 
        public ActionResult LeadOwnerwise()
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            var user = entity.tbl_FortuneUsers.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            ViewBag.users = user;
            return View();
        }
       
        public JsonResult GetLeadOwnerWise(string leadowner)
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            //var currentdate = DateTime.Now.ToString("yyyy-mm-dd");
            DateTime currentdate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

            var studentdata = entity.tbl_EnquiryForm.Where(m => m.LeadOwner == leadowner || m.LeadAssignedTo == leadowner).Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            List<EnquiryModel> data = new List<EnquiryModel>();

            foreach (var item in studentdata)
            {
                EnquiryModel studentmodel = new EnquiryModel();
                studentmodel.Id = item.Id;
                studentmodel.LeadName = item.LeadName;
                studentmodel.Mobile = item.Mobile;
                studentmodel.Email = item.Email;
                studentmodel.Address = item.Address;
                studentmodel.selectedinterestedcourse = item.Product;
                studentmodel.selectedrefby = item.Reference;
                studentmodel.Leaddate = item.LeadDate == null ? (DateTime?)null : Convert.ToDateTime(item.LeadDate);
                studentmodel.Followupdate = item.FollowUpdate == null ? (DateTime?)null : Convert.ToDateTime(item.FollowUpdate);
                studentmodel.selectedstatus = item.Status;
                studentmodel.LeadAssignedby = item.LeadAssignedby;
                studentmodel.selectedleadassignedto = item.LeadAssignedTo;
                studentmodel.seletedwantsmsalerts = item.WantSMSAlerts;
                studentmodel.seletedwantmailalerts = item.WantMailAlerts;
                studentmodel.Website = item.Website;
                studentmodel.Industry = item.Industry;
                studentmodel.Rating = item.Rating;
                studentmodel.LeadOwner = item.LeadOwner;
                studentmodel.Comment1 = item.Comment1;
                studentmodel.Comment2 = item.Comment2;
                studentmodel.Comment3 = item.Comment3;

                data.Add(studentmodel);

            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LeadSourceWise()
        {

            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            var source = entity.tbl_Source.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            List<Source> sou = new List<Source>();
            foreach (var item in source)
            {
                Source s = new Source();
                s.SourceId = item.SourceId;
                s.SourceName = item.SourceName;
                sou.Add(s);
            }
            ViewBag.source = sou;
            return View();
        }
        //[CustomExceptionHandlerFilter]
        public JsonResult GetLeadSourceWise(string leadsource)
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            //var currentdate = DateTime.Now.ToString("yyyy-mm-dd");
            DateTime currentdate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

            var studentdata = entity.tbl_EnquiryForm.Where(m => m.Reference == leadsource).Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            List<EnquiryModel> data = new List<EnquiryModel>();

            foreach (var item in studentdata)
            {
                EnquiryModel studentmodel = new EnquiryModel();
                studentmodel.Id = item.Id;
                studentmodel.LeadName = item.LeadName;
                studentmodel.Mobile = item.Mobile;
                studentmodel.Email = item.Email;
                studentmodel.Address = item.Address;
                studentmodel.selectedinterestedcourse = item.Product;
                studentmodel.selectedrefby = item.Reference;
                studentmodel.Leaddate = item.LeadDate == null ? (DateTime?)null : Convert.ToDateTime(item.LeadDate);
                studentmodel.Followupdate = item.FollowUpdate == null ? (DateTime?)null : Convert.ToDateTime(item.FollowUpdate);
                studentmodel.selectedstatus = item.Status;
                studentmodel.LeadAssignedby = item.LeadAssignedby;
                studentmodel.selectedleadassignedto = item.LeadAssignedTo;
                studentmodel.seletedwantsmsalerts = item.WantSMSAlerts;
                studentmodel.seletedwantmailalerts = item.WantMailAlerts;
                studentmodel.Website = item.Website;
                studentmodel.Industry = item.Industry;
                studentmodel.Rating = item.Rating;
                studentmodel.LeadOwner = item.LeadOwner;
                studentmodel.Comment1 = item.Comment1;
                studentmodel.Comment2 = item.Comment2;
                studentmodel.Comment3 = item.Comment3;

                data.Add(studentmodel);

            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEmailReports()
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            //var currentdate = DateTime.Now.ToString("yyyy-mm-dd");
            DateTime currentdate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            //product
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
            //Source

            var source = entity.tbl_Source.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            List<Source> sou = new List<Source>();
            foreach (var item in source)
            {
                Source s = new Source();
                s.SourceId = item.SourceId;
                s.SourceName = item.SourceName;
                sou.Add(s);
            }
            ViewBag.source = sou;
            //Status
            var sta = entity.tbl_LeadStatus.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            List<Leadstatus> status = new List<Leadstatus>();
            foreach (var item in sta)
            {
                Leadstatus sta1 = new Leadstatus();
                sta1.StatusId = item.StatusId;
                sta1.StatusName = item.StatusName;
                status.Add(sta1);
            }
            ViewBag.status = status;
            return View();
        }
        public JsonResult GetEmail(DateTime? fdate, DateTime? tdate, string leadowner, string product, string status, string frommail)
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            //var currentdate = DateTime.Now.ToString("yyyy-mm-dd");
            DateTime currentdate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

            var cuser = Session["CurrentUser"].ToString();

            //var totaldata = entity.tbl_EnquiryForm.Where(m => m.Created_Date >= fdate && m.Created_Date <= tdate && m.LeadOwner == leadowner).Where(m => m.Product == product || (m.Product == product || m.Status == status) || (m.Product == product || m.Reference == source) || (m.Reference == source || m.Status ==status)).Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            // var totaldata = entity.tbl_EnquiryForm.Where(m => m.LeadDate >= fromdate && m.LeadDate <= todate || m.LeadOwner == leadowner || m.Product == product || m.Status == status || m.Reference == source).Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList().OrderBy(m=>m.LeadDate);
            var totaldata = entity.tbl_EmailLog.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList().OrderBy(m => m.SentDate).ToList();
            if (Session["UserRole"].ToString() != "Admin" && Session["UserRole"].ToString() != "Manager")
            {
                totaldata = totaldata.Where(m => m.LoginUserName.ToUpper() == cuser.ToUpper()).ToList();
            }

            if (fdate != null)
            {
                totaldata = totaldata.Where(m => m.SentDate >= fdate).ToList();
            }
            if (tdate != null)
            {
                totaldata = totaldata.Where(m => m.SentDate <= tdate).ToList();
            }
            if (!string.IsNullOrEmpty(leadowner))
            {
                totaldata = totaldata.Where(m => m.LoginUserName.ToUpper() == leadowner.ToUpper()).ToList();
            }
            if (product != "All")
            {
                totaldata = totaldata.Where(m => m.Product == product).ToList();
            }
            if (status != "All")
            {
                totaldata = totaldata.Where(m => m.Status == status).ToList();
            }
            if (frommail != null)
            {
                totaldata = totaldata.Where(m => m.EmailFrom.ToUpper() == frommail.ToUpper()).ToList();
            }
            //if (source != "All")
            //{
            //    totaldata = totaldata.Where(m => m.Reference == source).ToList();
            //}


            //var data = entity.tbl_EmailLog.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            var data = totaldata;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSmsReports()
        {
            return View();
        }
        public JsonResult GetSms(DateTime? fdate, DateTime? tdate, string leadowner)
        {

            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            //var currentdate = DateTime.Now.ToString("yyyy-mm-dd");
            DateTime currentdate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

            var cuser = Session["CurrentUser"].ToString();

            //var totaldata = entity.tbl_EnquiryForm.Where(m => m.Created_Date >= fdate && m.Created_Date <= tdate && m.LeadOwner == leadowner).Where(m => m.Product == product || (m.Product == product || m.Status == status) || (m.Product == product || m.Reference == source) || (m.Reference == source || m.Status ==status)).Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            // var totaldata = entity.tbl_EnquiryForm.Where(m => m.LeadDate >= fromdate && m.LeadDate <= todate || m.LeadOwner == leadowner || m.Product == product || m.Status == status || m.Reference == source).Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList().OrderBy(m=>m.LeadDate);
            var totaldata = entity.tbl_SmsLog.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList().OrderBy(m => m.SentDate).ToList();
            if (Session["UserRole"].ToString() != "Admin" && Session["UserRole"].ToString() != "Manager")
            {
                totaldata = totaldata.Where(m => m.LoginUserName.ToUpper() == cuser.ToUpper()).ToList();
            }

            if (fdate != null)
            {
                totaldata = totaldata.Where(m => m.SentDate >= fdate).ToList();
            }
            if (tdate != null)
            {
                totaldata = totaldata.Where(m => m.SentDate <= tdate).ToList();
            }
            if (!string.IsNullOrEmpty(leadowner))
            {
                totaldata = totaldata.Where(m => m.LoginUserName.ToUpper() == leadowner.ToUpper()).ToList();
            }

            //if (orgname != null)
            //{
            //    totaldata = totaldata.Where(m => m.OrganizationName.ToUpper() == orgname.ToUpper()).ToList();
            //}

            //var data = entity.tbl_SmsLog.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            var data = totaldata;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLeadOwners(string Prefix)
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            //var getLeadowners = (from c in entity.tbl_FortuneUsers.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).Where(m=>m.UserName.StartsWith(Prefix)).S;
            //                 where c.Name.StartsWith(Prefix)
            //                 select new { c.Name, c.Id });
            //List<UserLoginModel> l = new List<UserLoginModel>();
            var getLeadowners = (from c in entity.tbl_FortuneUsers
                                 where c.CompId == CurrentCompanyId && c.BrId == CurrentCompanyBranchId
                                 where c.UserName.StartsWith(Prefix)
                                 select new { username = c.UserName, userid = c.UserId });
            return Json(getLeadowners, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLeadNames(string Prefix)
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

            var getLeadowners = (from c in entity.tbl_enquirydetails
                                 where c.compId == CurrentCompanyId && c.brId == CurrentCompanyBranchId
                                 where c.dtl_leadname.StartsWith(Prefix)
                                 select new { username = c.dtl_leadname, userid = c.enquiryId });
            return Json(getLeadowners, JsonRequestBehavior.AllowGet);
            //var getLeadowners = (from c in entity.tbl_EmailLog
            //                     where c.CompId == CurrentCompanyId && c.BrId == CurrentCompanyBranchId
            //                     where c.LoginUserName.StartsWith(Prefix)
            //                     select new { username = c.LoginUserName, userid = c.Id });
            //return Json(getLeadowners, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMultiparameterwise()
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            //product
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
            //Source

            var source = entity.tbl_Source.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            List<Source> sou = new List<Source>();
            foreach (var item in source)
            {
                Source s = new Source();
                s.SourceId = item.SourceId;
                s.SourceName = item.SourceName;
                sou.Add(s);
            }
            ViewBag.source = sou;
            //Status
            var sta = entity.tbl_LeadStatus.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            List<Leadstatus> status = new List<Leadstatus>();
            foreach (var item in sta)
            {
                Leadstatus sta1 = new Leadstatus();
                sta1.StatusId = item.StatusId;
                sta1.StatusName = item.StatusName;
                status.Add(sta1);
            }
            ViewBag.status = status;

            return View();
        }

        public JsonResult GetMultiParaWise(DateTime? fdate, DateTime? tdate, string leadowner, string product, string status, string source)
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            var cuser = Session["CurrentUser"].ToString();

            //var totaldata = entity.tbl_EnquiryForm.Where(m => m.Created_Date >= fdate && m.Created_Date <= tdate && m.LeadOwner == leadowner).Where(m => m.Product == product || (m.Product == product || m.Status == status) || (m.Product == product || m.Reference == source) || (m.Reference == source || m.Status ==status)).Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            // var totaldata = entity.tbl_EnquiryForm.Where(m => m.LeadDate >= fromdate && m.LeadDate <= todate || m.LeadOwner == leadowner || m.Product == product || m.Status == status || m.Reference == source).Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList().OrderBy(m=>m.LeadDate);
            var totaldata = entity.tbl_EnquiryForm.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList().OrderBy(m => m.LeadDate).ToList();
            if (Session["UserRole"].ToString() != "Admin" && Session["UserRole"].ToString() != "Manager")
            {
                totaldata = totaldata.Where(m => m.LeadOwner.ToUpper() == cuser.ToUpper()).ToList();
            }

            if (fdate != null)
            {
                totaldata = totaldata.Where(m => m.LeadDate >= fdate).ToList();
            }
            if (tdate != null)
            {
                totaldata = totaldata.Where(m => m.LeadDate <= tdate).ToList();
            }
            if (!string.IsNullOrEmpty(leadowner))
            {
                totaldata = totaldata.Where(m => m.LeadOwner.ToUpper() == leadowner.ToUpper()).ToList();
            }
            if (product != "All")
            {
                totaldata = totaldata.Where(m => m.Product == product).ToList();
            }
            if (status != "All")
            {
                totaldata = totaldata.Where(m => m.Status == status).ToList();
            }
            if (source != "All")
            {
                totaldata = totaldata.Where(m => m.Reference == source).ToList();
            }
            List<EnquiryModel> data = new List<EnquiryModel>();
            if (totaldata != null)
            {
                foreach (var item in totaldata)
                {
                    EnquiryModel studentmodel = new EnquiryModel();
                    studentmodel.Id = item.Id;
                    studentmodel.LeadName = item.LeadName;
                    studentmodel.Mobile = item.Mobile;
                    studentmodel.Email = item.Email;
                    studentmodel.Address = item.Address;
                    studentmodel.selectedinterestedcourse = item.Product;
                    studentmodel.selectedrefby = item.Reference;
                    studentmodel.seletedwantsmsalerts = item.WantSMSAlerts;
                    studentmodel.seletedwantmailalerts = item.WantMailAlerts;
                    studentmodel.selectedstatus = item.Status;
                    studentmodel.LeadAssignedby = item.LeadAssignedby;
                    studentmodel.selectedleadassignedto = item.LeadAssignedTo;

                    studentmodel.Website = item.Website;
                    studentmodel.Industry = item.Industry;
                    studentmodel.Rating = item.Rating;
                    studentmodel.LeadOwner = item.LeadOwner;
                    studentmodel.Leaddate = item.LeadDate;
                    studentmodel.Followupdate = item.FollowUpdate;
                    studentmodel.Comment1 = item.Comment1;
                    studentmodel.Comment2 = item.Comment2;
                    studentmodel.Comment3 = item.Comment3;
                    data.Add(studentmodel);

                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
      
        
        public ActionResult PaymentReports()
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            //product
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



            return View();
        }
        public JsonResult GetPaymentList(DateTime? fdate, DateTime? tdate, string customer, string product)
        {

            string msg = customer;
            string[] strarr = msg.Split('-');
            string mobile = "";
            string cuname = "";
            if (!string.IsNullOrEmpty(msg))
            {
                for (int i = 0; i < strarr.Length; i++)
                {
                    cuname = strarr[0];
                    mobile = strarr[1];

                }
            }
          
           
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            var cuser = Session["CurrentUser"].ToString();

            //var totaldata = entity.tbl_EnquiryForm.Where(m => m.Created_Date >= fdate && m.Created_Date <= tdate && m.LeadOwner == leadowner).Where(m => m.Product == product || (m.Product == product || m.Status == status) || (m.Product == product || m.Reference == source) || (m.Reference == source || m.Status ==status)).Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            // var totaldata = entity.tbl_EnquiryForm.Where(m => m.LeadDate >= fromdate && m.LeadDate <= todate || m.LeadOwner == leadowner || m.Product == product || m.Status == status || m.Reference == source).Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList().OrderBy(m=>m.LeadDate);
            var totaldata = entity.tbl_Payment.Where(m => m.compid == CurrentCompanyId && m.brid == CurrentCompanyBranchId).ToList().OrderBy(m => m.payment_date).ToList();
            if (Session["UserRole"].ToString() != "Admin" && Session["UserRole"].ToString() != "Manager")
            {
                totaldata = totaldata.Where(m => m.cust_name.ToUpper() == cuser.ToUpper()).ToList();
            }

            if (fdate != null)
            {
                totaldata = totaldata.Where(m => m.payment_date >= fdate).ToList();
            }
            if (tdate != null)
            {
                totaldata = totaldata.Where(m => m.payment_date <= tdate).ToList();
            }
            if (!string.IsNullOrEmpty(cuname))
            {
                totaldata = totaldata.Where(m => m.cust_name.ToUpper() == cuname.ToUpper()).ToList();
            }
          
                if (product != "All")
                {
                    totaldata = totaldata.Where(m => m.product_name.ToUpper() == product.ToUpper()).ToList();
                }
            
          
            List<BillingModel> data = new List<BillingModel>();
             if (totaldata!=null)
            {
                foreach (var item in totaldata)
                {
                       BillingModel b = new BillingModel();
                       b.custname = item.cust_name;
                       b.mobile = item.cust_mobile;
                       b.productname = item.product_name;
                       b.productactualprice = item.product_actual_cost;
                       b.productrevisedprice = item.product_revised_amount;
                       b.paymentmode = item.modeof_payment;
                       b.payamout = item.paid_amount;
                       b.balanceamount = item.balance_amount;
                       b.paymentdate = item.payment_date;
                       b.followupdate = item.payment_followupdate;
                       b.Comments = item.comments;
                       data.Add(b);
                }
             }

             return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UserHistory()
        {
            return View();
        }
        public ActionResult GetUserHistory(string leadname)
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            var cuser = Session["CurrentUser"].ToString();
            var totaldata = entity.tbl_enquirydetails.Where(m => m.compId == CurrentCompanyId && m.brId == CurrentCompanyBranchId && m.dtl_leadname.ToUpper() == leadname.ToUpper()).ToList().OrderBy(m => m.dtl_createddt).ToList();
            List<EnquiryModel> data = new List<EnquiryModel>();
            if (totaldata != null)
            {
                foreach (var item in totaldata)
                {
                    EnquiryModel studentmodel = new EnquiryModel();
                    studentmodel.Id = item.enquiryId;
                    studentmodel.LeadName = item.dtl_leadname;
                    studentmodel.selectedinterestedcourse = item.dtl_product;
                    studentmodel.selectedstatus = item.dtl_status;
                    studentmodel.LeadOwner = item.dtl_leadowner;
                    studentmodel.Leaddate = item.dtl_createddt;
                    studentmodel.Followupdate = item.followup_date;
                    studentmodel.Comment1 = item.dtl_comments;
                    studentmodel.companyid = item.compId;
                    studentmodel.branchid = item.brId;
                    data.Add(studentmodel);

                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
