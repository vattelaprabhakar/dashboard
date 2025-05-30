using FortuneTechPvtLtd.DataModel;
using FortuneTechPvtLtd.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGrease;

namespace FortuneTechPvtLtd.Controllers
{
    public class MultiLeadsAssignController : Controller
    {
        //
        // GET: /MultiLeadsAssign/
        FortuneSoftEntities entity = new FortuneSoftEntities();
        Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ActionResult LeadAssign()
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

            //leadowner
            var user = entity.tbl_FortuneUsers.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            ViewBag.users = user;

            var currentloginusername=Session["CurrentUser"].ToString();
            //leadowner except current user for assignedto field
            var user1 = entity.tbl_FortuneUsers.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId && m.UserName!=currentloginusername).ToList();
            ViewBag.usersforassignedto = user1;

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
        //lead assignment
        public JsonResult MultiLeadAssignment(DateTime? fdate, DateTime? tdate, string leadowner, string product, string status, string source, string assignedto)
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            var cuser = Session["CurrentUser"].ToString();
            List<EnquiryModel> data = new List<EnquiryModel>();
            if (fdate != null || tdate != null || leadowner != "All" || product != "All" || status != "All" || source != "All")
            {
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
                if (leadowner!="All")
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


                //if (fdate != null || tdate != null || !string.IsNullOrEmpty(leadowner) || product != "All" || status != "All" || source != "All" && !string.IsNullOrEmpty(assignedto))
                //{
                //    if (product == "All")
                //    {
                //        product = null;
                //    }
                //    if (source == "All")
                //    {
                //        source = null;
                //    }
                //    if (status == "All")
                //    {
                //        status = null;
                //    }
                //    SqlParameter param1 = new SqlParameter("@CompId", CurrentCompanyId);
                //    SqlParameter param2 = new SqlParameter("@BrId", CurrentCompanyBranchId);
                //    SqlParameter param3 = new SqlParameter("@fromdate", ((object)fdate) ?? DBNull.Value);
                //    SqlParameter param4 = new SqlParameter("@todate", ((object)tdate) ?? DBNull.Value);
                //    SqlParameter param5 = new SqlParameter("@leadowner", ((object)leadowner) ?? DBNull.Value);
                //    SqlParameter param6 = new SqlParameter("@product", ((object)product) ?? DBNull.Value);
                //    SqlParameter param7 = new SqlParameter("@source", ((object)source) ?? DBNull.Value);
                //    SqlParameter param8 = new SqlParameter("@status", ((object)status) ?? DBNull.Value);
                //    SqlParameter param9 = new SqlParameter("@assignedto", ((object)assignedto) ?? DBNull.Value);
                //    SqlParameter param10 = new SqlParameter("@Record_ModifiedBy", cuser);
                //    //SqlParameter param11 = new SqlParameter("@Count",DbType.Int16);
                //    //param11.SqlValue = 0;
                //    //param11.Direction = ParameterDirection.Output;
                //    var data1 = entity.Database.ExecuteSqlCommand("Proc_MultiLeadAssignment  @CompId,@BrId,@fromdate,@todate,@leadowner,@product,@source,@status,@assignedto,@Record_ModifiedBy", param1, param2, param3, param4, param5, param6, param7, param8, param9, param10);
                //    entity.SaveChanges();
                //    //var a=param11.Value;
                //}



                //if (tdate != null)
                //{
                //    totaldata = totaldata.Where(m => m.LeadDate <= tdate).ToList();
                //}
                //if (!string.IsNullOrEmpty(leadowner))
                //{
                //    totaldata = totaldata.Where(m => m.LeadOwner.ToUpper() == leadowner.ToUpper()).ToList();
                //}
                //if (product != "All")
                //{
                //    totaldata = totaldata.Where(m => m.Product == product).ToList();
                //}
                //if (status != "All")
                //{
                //    totaldata = totaldata.Where(m => m.Status == status).ToList();
                //}
                //if (source != "All")
                //{
                //    totaldata = totaldata.Where(m => m.Reference == source).ToList();
                //}
                //if (!string.IsNullOrEmpty(assignedto))
                //{
                //   // totaldata = totaldata.Where(m => m.Reference == source).ToList();
                //}
                //  var totaldata = entity.tbl_EnquiryForm.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList().OrderBy(m => m.LeadDate).ToList();
              
                if (totaldata.Count!=0)
                {
                   // TempData["Assignedto"] = "assignedtodiv";
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
              
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateMultiLeadAssignment(DateTime? fdate, DateTime? tdate, string leadowner, string product, string status, string source, string assignedto)
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            var cuser = Session["CurrentUser"].ToString();
            string data = string.Empty;
            if (fdate != null || tdate != null || leadowner!="All" || product != "All" || status != "All" || source != "All" && !string.IsNullOrEmpty(assignedto))
            {
                if (leadowner=="All")
                {
                    leadowner = null;
                }
                if (product == "All")
                {
                    product = null;
                }
                if (source == "All")
                {
                    source = null;
                }
                if (status == "All")
                {
                    status = null;
                }
                SqlParameter param1 = new SqlParameter("@CompId", CurrentCompanyId);
                SqlParameter param2 = new SqlParameter("@BrId", CurrentCompanyBranchId);
                SqlParameter param3 = new SqlParameter("@fromdate", ((object)fdate) ?? DBNull.Value);
                SqlParameter param4 = new SqlParameter("@todate", ((object)tdate) ?? DBNull.Value);
                SqlParameter param5 = new SqlParameter("@leadowner", ((object)leadowner) ?? DBNull.Value);
                SqlParameter param6 = new SqlParameter("@product", ((object)product) ?? DBNull.Value);
                SqlParameter param7 = new SqlParameter("@source", ((object)source) ?? DBNull.Value);
                SqlParameter param8 = new SqlParameter("@status", ((object)status) ?? DBNull.Value);
                SqlParameter param9 = new SqlParameter("@assignedto", ((object)assignedto) ?? DBNull.Value);
                SqlParameter param10 = new SqlParameter("@Record_ModifiedBy", cuser);
                //SqlParameter param11 = new SqlParameter("@Count",DbType.Int16);
                //param11.SqlValue = 0;
                //param11.Direction = ParameterDirection.Output;
                var data1 = entity.Database.ExecuteSqlCommand("Proc_MultiLeadAssignment  @CompId,@BrId,@fromdate,@todate,@leadowner,@product,@source,@status,@assignedto,@Record_ModifiedBy", param1, param2, param3, param4, param5, param6, param7, param8, param9, param10);
                entity.SaveChanges();
                 data = "true";
                //var a=param11.Value;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}
