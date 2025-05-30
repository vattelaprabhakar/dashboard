using FortuneTechPvtLtd.DataModel;
using FortuneTechPvtLtd.Models;
using FortuneTechPvtLtd.SMSCountry;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace FortuneTechPvtLtd.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/
        FortuneSoftEntities entity = new FortuneSoftEntities();
        Logger logger = LogManager.GetCurrentClassLogger();

        //[CustomExceptionHandlerFilter]
        public ActionResult Index()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DateTime currentdate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                    var currentuser = Session["CurrentUser"].ToString();
                    var currentuserrole = Session["UserRole"].ToString();
                    ProductCount();
                    //  var status = entity.Database.ExecuteSqlCommand("proc_total_lead_status");
                    // var data2 = entity.Database.SqlQuery<>("proc_total_lead_status").ToList();  
                    //  var data1=    entity.proc_total_lead_status();
                    //var j = entity.proc_total_lead_status();
                    //for dashboard data
                    // var =
                    // var status = entity.Database.ExecuteSqlCommand("proc_total_lead_status");
                    // var courseList = entity.Database.SqlQuery<tbl_EnquiryForm>("exec proc_total_lead_status").ToList<tbl_EnquiryForm>();
                    //  var status = entity.Database.SqlQuery("").ToList();
                    // var data2 = entity.Database.SqlQuery<tbl_EnquiryForm>("exec proc_total_lead_status").ToList();  
                    //var data=entity.proc_total_lead_status();
                    //  var data2 = entity.Database.ExecuteSqlCommand("exec proc_total_lead_status");

                    // var data3 = entity.Database.SqlQuery<tbl_EnquiryForm>("exec proc_total_lead_status");

                    //foreach (var item in data2)
                    //{
                    //    var a = item.Status;
                    //}

                    //var data = entity.proc_monthly_lead_status();

                    //  var data3 = entity.Database.SqlQuery<tbl_EnquiryForm>("proc_total_lead_status");
                    //get user related leads with foloupdate as currentdate

                    //dashboard
                    // entity.tbl_EnquiryForm.Where(m => m.Status == "").Count();
                    //var statuslist = entity.tbl_LeadStatus.ToList();
                    //if (statuslist !=null)
                    //{
                    //    List<int> arrr = new List<int>();
                    //    var count = entity.tbl_LeadStatus.Count();
                    //    foreach (var item in statuslist)
                    //    {
                    //        var name = item.StatusName;
                    //        //  OrderedDictionary mm = new OrderedDictionary();
                    //        //  List<string> ob = new List<string>();


                    //        if (name != null)
                    //        {
                    //            var leadcount = entity.tbl_EnquiryForm.Where(m => m.Status == name).Count();
                    //            arrr.Add(leadcount);
                    //        }
                    //    }
                    //    Session["statusnames"] = statuslist;
                    //    Session["statuscount"] = arrr;

                    //}


                    //List<GetDataForDashboardModel> l = new List<GetDataForDashboardModel>();
                    //if (statuslist!=null)
                    //{
                    //    foreach (var item in statuslist)
                    //    {
                    //        GetDataForDashboardModel s = new GetDataForDashboardModel();
                    //        s.StatusName = item.StatusName;
                    //        l.Add(s);
                    //    }
                    //}

                    //if (statuslist != null)
                    //{
                    //    string statusname = "";
                    //    foreach (var item in statuslist)
                    //    {
                    //        statusname = item.StatusName;
                    //    }
                    //    List<GetDataForDashboardModel> menus = entity.tbl_LeadStatus.Where(x => x.StatusName == statusname).Select(x => new GetDataForDashboardModel
                    //    {


                    //    }).ToList();
                    //    Session["MenuMaster"] = menus; //Bind the _menus list to MenuMaster session
                    //}

                    //if (statuslist!=null)
                    //{
                    //    List<GetDataForDashboardModel> getdata = null;
                    //    foreach (var item in statuslist)
                    //    {
                    //        var statusname = item.StatusName;
                    //         getdata = entity.tbl_EnquiryForm.Where(x => x.Status == statusname).Select(x => new GetDataForDashboardModel
                    //        {
                    //            StatusName = x.Status,
                    //            StatusCount = x.Status.Count()
                    //        }).ToList();
                    //    }
                    //    Session["getstatusdata"] = getdata;
                    //}


                    //product names and count start


                    int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                    int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
                    //current user id
                    int currentuserid = Convert.ToInt32(Session["currentuserid"].ToString());
                    int leadcount = 0;
                    int pendingfollowup = 0;
                    int notinterested = 0;
                    int customer = 0;
                    DateTime currentdate1 = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                    var fromlastweekdate = System.Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
                    var fromlastmonth = System.Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"));

                    int emailcount = 0;
                    int emailcountlastweek = 0;
                    int emailcountlastmonth = 0;
                    if (Session["UserRole"].ToString() != "Admin" && Session["UserRole"].ToString() != "Manager")
                    {
                        var cuser = Session["CurrentUser"].ToString();
                        //leadcount = entity.tbl_EnquiryForm.Count(m => m.Status == "Lead" && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId && m.LeadOwner.ToUpper() == cuser.ToUpper());
                        //pendingfollowup = entity.tbl_EnquiryForm.Count(m => m.Status == "Pending-FollowUp" && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId && m.LeadOwner.ToUpper() == cuser.ToUpper());
                        //notinterested = entity.tbl_EnquiryForm.Count(m => m.Status == "Not Interested" && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId && m.LeadOwner.ToUpper() == cuser.ToUpper());
                        //customer = entity.tbl_EnquiryForm.Count(m => m.Status == "Customer" && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId && m.LeadOwner.ToUpper() == cuser.ToUpper());
                        emailcount = entity.tbl_EmailLog.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId && m.LoginUserName.ToUpper() == cuser.ToUpper()).Count();
                        emailcountlastweek = entity.tbl_EmailLog.Where(m => m.SentDate >= fromlastweekdate && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId && m.LoginUserName.ToUpper() == cuser.ToUpper()).Count();
                        emailcountlastmonth = entity.tbl_EmailLog.Where(m => m.SentDate >= fromlastmonth && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId && m.LoginUserName.ToUpper() == cuser.ToUpper()).Count();
                    }
                    else
                    {
                        //leadcount = entity.tbl_EnquiryForm.Count(m => m.Status == "Lead" && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId);
                        //pendingfollowup = entity.tbl_EnquiryForm.Count(m => m.Status == "Pending-FollowUp" && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId);
                        //notinterested = entity.tbl_EnquiryForm.Count(m => m.Status == "Not Interested" && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId);
                        //customer = entity.tbl_EnquiryForm.Count(m => m.Status == "Customer" && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId);
                        emailcount = entity.tbl_EmailLog.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).Count();
                        emailcountlastweek = entity.tbl_EmailLog.Where(m => m.SentDate >= fromlastweekdate && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).Count();
                        emailcountlastmonth = entity.tbl_EmailLog.Where(m => m.SentDate >= fromlastmonth && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).Count();
                    }
                    //ViewBag.leadcount = leadcount;
                    //ViewBag.pendingfollowup = pendingfollowup;
                    //ViewBag.notinterested = notinterested;
                    //ViewBag.customer = customer;
                    ViewBag.emailcount = emailcount;
                    ViewBag.emailcountlastweek = emailcountlastweek;
                    ViewBag.emailcountlastmonth = emailcountlastmonth;


                    //end product names and count


                    //chart
                    //var query = entity.tbl_EnquiryForm
                    //.GroupBy(p => p.Status)
                    //.Select(g => new { product = g.Key, count = g.Count() });
                    //ViewBag.chart = query;


                    //int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                    //int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

                    var studentdata = entity.tbl_EnquiryForm.Where(m => m.LeadAssignedTo.ToUpper() == currentuser.ToUpper() || m.LeadAssignedby.ToUpper() == currentuser.ToUpper() || m.LeadOwner.ToUpper() == currentuser.ToUpper() || currentuserrole == "Admin").Where(m => m.FollowUpdate == currentdate && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).OrderByDescending(m => m.Id).ToList();
                    List<EnquiryModel> studentlist = new List<EnquiryModel>();
                    if (studentdata != null)
                    {
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
                            //studentmodel.seletedwantalerts = item.Wantalerts;
                            studentmodel.selectedstatus = item.Status;
                            studentmodel.LeadAssignedby = item.LeadAssignedby;
                            studentmodel.selectedleadassignedto = item.LeadAssignedTo;
                            studentmodel.Website = item.Website;
                            studentmodel.Industry = item.Industry;
                            studentmodel.Rating = item.Rating;
                            studentmodel.LeadOwner = item.LeadOwner;
                            studentmodel.Leaddate = item.LeadDate == null ? (DateTime?)null : Convert.ToDateTime(item.LeadDate);
                            studentmodel.Followupdate = item.FollowUpdate == null ? (DateTime?)null : Convert.ToDateTime(item.FollowUpdate);
                            studentmodel.Comment1 = item.Comment1;
                            studentmodel.Comment2 = item.Comment2;
                            studentmodel.Comment3 = item.Comment3;

                            studentlist.Add(studentmodel);

                        }


                        return View(studentlist);
                    }
                }
                catch (Exception ex)
                {
                    logger.ErrorException("error occurred at", ex);
                }

            }
            return RedirectToAction("UnAuthorize", "Error");


        }


        //public PartialViewResult View(int id)
        //{

        //        var data = entity.tbl_EnquiryForm.Where(m => m.Id == id).FirstOrDefault();


        //        EnquiryModel s = new EnquiryModel();
        //        try
        //        {
        //            if (data!=null)
        //            {
        //                s.Id = data.Id;
        //                s.LeadName = data.LeadName;
        //                s.Mobile = data.Mobile;
        //                s.Email = data.Email;
        //                s.Address = data.Address;
        //                s.selectedinterestedcourse = data.Product;
        //                s.selectedrefby = data.Reference;
        //                s.seletedwantalerts = data.Wantalerts;
        //                s.LeadAssignedby = data.LeadAssignedby;
        //                s.LeadOwner = data.LeadOwner;
        //                s.selectedstatus = data.Status;
        //                s.Rating = data.Rating;
        //                s.Comment1 = data.Comment1;
        //                s.Comment2 = data.Comment2;
        //                s.Comment3 = data.Comment3;
        //                s.Website = data.Website;
        //                s.Industry = data.Industry;
        //                s.Leaddate = data.LeadDate;
        //                s.Followupdate = data.FollowUpdate;
        //            }


        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //     // return PartialView(s);

        //       return PartialView("_LeadRecordsView",s);
        //}
        //[CustomExceptionHandlerFilter]
        public void ProductCount()
        {
            List<ProductCountModel> model = new List<ProductCountModel>();
            try
            {
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
                var cuser = Session["CurrentUser"].ToString();

                if (Session["UserRole"].ToString() != "Admin" && Session["UserRole"].ToString() != "Manager")
                {
                    var images = entity.tbl_EnquiryForm.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).GroupBy(n => n.Product)
                           .Select(g => new { ProductName = g.Key, Count = g.Count() }).ToList();

                    for (int i = 0; i < images.ToList().Count; i++)
                    {
                        model.Add(new ProductCountModel { ProductName = images[i].ProductName, Count = images[i].Count });
                    }
                }
                else
                {
                    var images = entity.tbl_EnquiryForm.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).GroupBy(n => n.Status)
                       .Select(g => new { ProductName = g.Key, Count = g.Count() }).ToList();

                    for (int i = 0; i < images.ToList().Count; i++)
                    {
                        model.Add(new ProductCountModel { ProductName = images[i].ProductName, Count = images[i].Count });
                    }
                }

            }

            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            Session["ProductCount"] = model;

        }

        public ActionResult GetChart()
        {
            List<StatusCountModel> model = new List<StatusCountModel>();
            try
            {
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
                var cuser = Session["CurrentUser"].ToString();

                if (Session["UserRole"].ToString() != "Admin" && Session["UserRole"].ToString() != "Manager")
                {
                    var images = entity.tbl_EnquiryForm.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId && m.LeadOwner.ToUpper() == cuser.ToUpper()).GroupBy(n => n.Status)
                           .Select(g => new { StatusName = g.Key, Count = g.Count() }).ToList();

                    for (int i = 0; i < images.ToList().Count; i++)
                    {
                        model.Add(new StatusCountModel { StatusName = images[i].StatusName, Count = images[i].Count });
                    }
                }
                else
                {
                    var images = entity.tbl_EnquiryForm.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).GroupBy(n => n.Status)
                       .Select(g => new { StatusName = g.Key, Count = g.Count() }).ToList();

                    for (int i = 0; i < images.ToList().Count; i++)
                    {
                        model.Add(new StatusCountModel { StatusName = images[i].StatusName, Count = images[i].Count });
                    }
                }

            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return Json(model, JsonRequestBehavior.AllowGet);

        }
        //[CustomExceptionHandlerFilter]
        public ActionResult GetChart1()
        {
            List<SourceCountModel> model = new List<SourceCountModel>();
            try
            {
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
                var cuser = Session["CurrentUser"].ToString();

                if (Session["UserRole"].ToString() != "Admin" && Session["UserRole"].ToString() != "Manager")
                {
                    var images = entity.tbl_EnquiryForm.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId && m.LeadOwner.ToUpper() == cuser.ToUpper()).GroupBy(n => n.Reference)
                             .Select(g => new { SourceName = g.Key, Count = g.Count() }).ToList();

                    for (int i = 0; i < images.ToList().Count; i++)
                    {
                        model.Add(new SourceCountModel { SourceName = images[i].SourceName, Count = images[i].Count });
                    }
                }
                else
                {
                    var images = entity.tbl_EnquiryForm.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).GroupBy(n => n.Reference)
                           .Select(g => new { SourceName = g.Key, Count = g.Count() }).ToList();

                    for (int i = 0; i < images.ToList().Count; i++)
                    {
                        model.Add(new SourceCountModel { SourceName = images[i].SourceName, Count = images[i].Count });
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return Json(model, JsonRequestBehavior.AllowGet);

        }
        //[CustomExceptionHandlerFilter]
        public ActionResult GetbarChart()
        {
            List<LeadDateCountModel> model = new List<LeadDateCountModel>();
            try
            {
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
                var images = entity.tbl_EnquiryForm.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).GroupBy(n => n.LeadDate)
                         .Select(g => new { LeadDate = g.Key, Count = g.Count() }).ToList();

                for (int i = 0; i < images.ToList().Count; i++)
                {
                    model.Add(new LeadDateCountModel { LeadDate = Convert.ToString(images[i].LeadDate), Count = images[i].Count });
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
            //List<LeadDateCountModel> lst = new List<LeadDateCountModel>();
            //var data = lst.Select(k => new { k.Deliverydate.Year, k.Deliverydate.Month, k.TotalCharge }).GroupBy(x => new { x.Year, x.Month }, (key, group) => new
            //{
            //    yr = key.Year,
            //    mnth = key.Month,
            //    tCharge = group.Sum(k => k.TotalCharge)
            //}).ToList();

        }


        public ActionResult CampaignSchdule()
        {

            //var currentuser = Session["CurrentUser"].ToString();
            //var currentuserrole = Session["UserRole"].ToString();
            //int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            //int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

            DateTime? vfromdate = DateTime.Now;
            DateTime? vtodate = DateTime.Now;
            //string campaignstatus = "";
            //string campaigntype = "";
            int? companypid = 0;
            int? branchid = 0;
            DateTime? compaigndate = DateTime.Now;
            DateTime currentdate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            string frommailid = "";
            string organizationname = "";


            //verify valid from date and to date,email flag and email alert flag
            var data1 = entity.tbl_FortuneBasicInfo.Where(m => m.FromDate <= currentdate && vtodate >= currentdate && m.email_flag.ToUpper() == "Yes".ToUpper()).ToList();//&& m.sms_flag.ToUpper() == "Yes".ToUpper() && m.email_alert.ToUpper() == "Yes".ToUpper()
            if (data1.Count != 0)
            {
                foreach (var item in data1)
                {
                    vfromdate = item.FromDate;
                    vtodate = item.ToDate;
                    companypid = item.CompId;
                    branchid = item.BrId;
                    frommailid = item.OrgEmail;
                    string currentday = Convert.ToString(DateTime.Now.DayOfWeek);
                    organizationname = item.OrganizationName;

                    //verify camp start date and end date ,camp_flag is active or not
                    var data2 = entity.tbl_campaign.Where(m => m.compId == companypid && m.brId == branchid && m.camp_start_dt <= currentdate && m.camp_end_dt >= currentdate && m.camp_flag.ToUpper() == "ACTIVE").ToList();
                    //  int curdaycount = entity.tbl_campaign.Where(m => m.compId == companypid && m.brId == branchid && m.camp_day == currentday).ToList().Count ;
                    if (data2.Count != 0)
                    {
                        MailSchedulingModel mou = new MailSchedulingModel();
                        foreach (var item2 in data2)
                        {
                            mou.campid = item2.campId;
                            mou.campproduct = item2.camp_product;
                            mou.campstatus = item2.camp_status;
                            mou.camptype = item2.camp_type;
                            mou.campcategory = item2.camp_Category;
                            string campaignday = item2.camp_day;
                            //  mou.campstartdate = item2.camp_start_dt;
                            //  mou.campenddate = item2.camp_end_dt;

                            string daily = "DAILY";
                            string weekly = "Weekly";
                            string email = "EMAIL";
                            string sms = "SMS";
                            //DateTime currentdate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            //checking daily for email
                            if (mou.campcategory.ToUpper() == daily.ToUpper() && mou.camptype.ToUpper() == email.ToUpper())//&& campaignday==currentday
                            {

                                // variables for send email
                                string product = "";
                                string leadstatus = "";
                                string leadmail = "";
                                string leadname = "";
                                //int? companypid = 0;
                                //int? branchid = 0;

                                var data3 = entity.tbl_EnquiryForm.Where(m => m.CompId == companypid && m.BrId == branchid && m.Product == mou.campproduct && m.Status == mou.campstatus).ToList();
                                if (data3.Count != 0)
                                {
                                    foreach (var item4 in data3)
                                    {
                                        //product = item4.Product;
                                        //leadstatus = item4.Status;
                                        companypid = item4.CompId;
                                        branchid = item4.BrId;
                                        leadmail = item4.Email;
                                        leadname = item4.LeadName;

                                        //organization mail
                                        //var orgbasicdata = entity.tbl_FortuneBasicInfo.Where(m => m.CompId == companypid && m.BrId == branchid).ToList();
                                        //string FromEmail = "";
                                        //string Org = "";
                                        //foreach (var org in orgbasicdata)
                                        //{
                                        //    FromEmail = item.OrgEmail;
                                        //    Org = item.OrganizationName;
                                        //}
                                        //var senderEmail = new MailAddress("info@fortunesofttech.com", Org);
                                        //var receiverEmail = new MailAddress(leadmail, "Receiver");

                                        //sending an email
                                        var courseemail = entity.tbl_LeadEmailContent.Where(m => m.InterestedCourse == mou.campproduct && m.leadstatus == mou.campstatus && m.CompId == companypid && m.BrId == branchid).ToList();
                                        //
                                        MailModel mailmodel = new MailModel();
                                        if (courseemail.Count != 0)
                                        {
                                            foreach (var item1 in courseemail)
                                            {
                                                mailmodel.Subject = item1.EmailSubject;
                                                mailmodel.Message = item1.EmailBody;
                                            }
                                            tbl_Emailtemp temp = new tbl_Emailtemp();
                                            temp.CompId = companypid;
                                            temp.BrId = branchid;
                                            temp.EmailFrom = frommailid;
                                            temp.EmailTo = leadmail;
                                            temp.Emailsubject = mailmodel.Subject;
                                            temp.EmailBody = mailmodel.Message;
                                            temp.LoginUserName = Session["CurrentUser"].ToString();
                                            temp.Product = mou.campproduct;
                                            temp.Status = mou.campstatus;
                                            temp.LeadName = leadname;
                                            temp.SentDate = currentdate;
                                            //push to emailtemp table for future verifications
                                            entity.tbl_Emailtemp.Add(temp);
                                            entity.SaveChanges();
                                        }

                                    }
                                }
                            }

                            //sms insert into tbl_smstemp
                            if (mou.campcategory.ToUpper() == daily.ToUpper() && mou.camptype.ToUpper() == sms.ToUpper())//&& campaignday==currentday
                            {
                                // variables for send sms
                                string product = "";
                                string leadstatus = "";
                                string leadmobilenumber = "";
                                string leadname = "";


                                var data3 = entity.tbl_EnquiryForm.Where(m => m.CompId == companypid && m.BrId == branchid && m.Product == mou.campproduct && m.Status == mou.campstatus).ToList();
                                if (data3.Count != 0)
                                {
                                    foreach (var item4 in data3)
                                    {

                                        companypid = item4.CompId;
                                        branchid = item4.BrId;
                                        leadmobilenumber = item4.Mobile;
                                        leadname = item4.LeadName;
                                        //sending an email
                                        var smscontent = entity.tbl_SmsContent.Where(m => m.InterestedCourse == mou.campproduct && m.leadstatus == mou.campstatus && m.CompId == companypid && m.BrId == branchid).ToList();
                                        SMSContentModel smsmodel = new SMSContentModel();
                                        if (smscontent.Count != 0)
                                        {
                                            foreach (var item1 in smscontent)
                                            {
                                                smsmodel.smscontent = item1.SmsContent;
                                                smsmodel.interestedcourse = item1.InterestedCourse;
                                                smsmodel.leadstutus = item1.leadstatus;
                                            }
                                            tbl_Smstemp temp = new tbl_Smstemp();
                                            temp.CompId = companypid;
                                            temp.BrId = branchid;
                                            temp.OrganizationName = organizationname;
                                            temp.MobileNum = leadmobilenumber;
                                            temp.LoginUserName = Session["CurrentUser"].ToString();
                                            temp.Product = smsmodel.interestedcourse;
                                            temp.Status = smsmodel.leadstutus;
                                            temp.SmsContent = smsmodel.smscontent;
                                            temp.SentDate = currentdate;
                                            //push to tbl_smstemp table for future verifications
                                            entity.tbl_Smstemp.Add(temp);
                                            entity.SaveChanges();
                                        }
                                        //else
                                        //{
                                        //    TempData["coursemail"] = "coursemail";
                                        //    return View();
                                        //}
                                    }
                                }
                            }
                            //send sms weekly
                            if (mou.campcategory.ToUpper() == weekly.ToUpper() && mou.camptype == sms.ToUpper() && campaignday == currentday)
                            {
                                // variables for send sms
                                string product = "";
                                string leadstatus = "";
                                string leadmobilenumber = "";
                                string leadname = "";


                                var data3 = entity.tbl_EnquiryForm.Where(m => m.CompId == companypid && m.BrId == branchid && m.Product == mou.campproduct && m.Status == mou.campstatus).ToList();
                                if (data3.Count != 0)
                                {
                                    foreach (var item4 in data3)
                                    {

                                        companypid = item4.CompId;
                                        branchid = item4.BrId;
                                        leadmobilenumber = item4.Mobile;
                                        leadname = item4.LeadName;
                                        //sending an email
                                        var smscontent = entity.tbl_SmsContent.Where(m => m.InterestedCourse == mou.campproduct && m.leadstatus == mou.campstatus && m.CompId == companypid && m.BrId == branchid).ToList();
                                        SMSContentModel smsmodel = new SMSContentModel();
                                        if (smscontent.Count != 0)
                                        {
                                            foreach (var item1 in smscontent)
                                            {
                                                smsmodel.smscontent = item1.SmsContent;
                                                smsmodel.interestedcourse = item1.InterestedCourse;
                                                smsmodel.leadstutus = item1.leadstatus;
                                            }
                                            tbl_Smstemp temp = new tbl_Smstemp();
                                            temp.CompId = companypid;
                                            temp.BrId = branchid;
                                            temp.OrganizationName = organizationname;
                                            temp.MobileNum = leadmobilenumber;
                                            temp.LoginUserName = Session["CurrentUser"].ToString();
                                            temp.Product = smsmodel.interestedcourse;
                                            temp.Status = smsmodel.leadstutus;
                                            temp.SmsContent = smsmodel.smscontent;
                                            temp.SentDate = currentdate;
                                            //push to tbl_smstemp table for future verifications
                                            entity.tbl_Smstemp.Add(temp);
                                            entity.SaveChanges();
                                        }
                                        //else
                                        //{
                                        //    TempData["coursemail"] = "coursemail";
                                        //    return View();
                                        //}
                                    }
                                }
                            }


                            //checking weekly with day for email
                            if (mou.campcategory.ToUpper() == weekly.ToUpper() && mou.camptype == email.ToUpper() && campaignday == currentday)
                            {
                                // variables for send email
                                //string product = "";
                                //string leadstatus = "";
                                string leadmail = "";
                                string leadname = "";
                                //int? companypid = 0;
                                //int? branchid = 0;

                                var data5 = entity.tbl_EnquiryForm.Where(m => m.CompId == companypid && m.BrId == branchid && m.Product == mou.campproduct && m.Status == mou.campstatus).ToList();
                                if (data5.Count != 0)
                                {
                                    foreach (var item4 in data5)
                                    {
                                        //product = item4.Product;
                                        //leadstatus = item4.Status;
                                        companypid = item4.CompId;
                                        branchid = item4.BrId;
                                        leadmail = item4.Email;
                                        leadname = item4.LeadName;
                                        //sending an email
                                        var courseemail = entity.tbl_LeadEmailContent.Where(m => m.InterestedCourse == mou.campproduct && m.leadstatus == mou.campstatus && m.CompId == companypid && m.BrId == branchid).ToList();

                                        MailModel mailmodel = new MailModel();
                                        if (courseemail.Count != 0)
                                        {
                                            foreach (var item1 in courseemail)
                                            {
                                                mailmodel.Subject = item1.EmailSubject;
                                                mailmodel.Message = item1.EmailBody;
                                            }
                                            tbl_Emailtemp temp = new tbl_Emailtemp();
                                            temp.CompId = companypid;
                                            temp.BrId = branchid;
                                            temp.EmailFrom = frommailid;
                                            temp.EmailTo = leadmail;
                                            temp.Emailsubject = mailmodel.Subject;
                                            temp.EmailBody = mailmodel.Message;
                                            temp.LoginUserName = Session["CurrentUser"].ToString();
                                            temp.Product = mou.campproduct;
                                            temp.Status = mou.campstatus;
                                            temp.LeadName = leadname;
                                            temp.SentDate = currentdate;
                                            //push to emailtemp table for future verifications
                                            entity.tbl_Emailtemp.Add(temp);
                                            entity.SaveChanges();
                                        }

                                    }

                                }
                            }
                        }
                        TempData["campcreated"] = "campcreated";
                        return View();
                    }
                    else
                    {
                        TempData["campnotcreated"] = "campnotcreated";
                        //from date or  end date or campflag is not active in tbl_campaign
                        return View();
                    }
                }
            }
            else
            {
                //fromdate and end date and emailaerts flag for campaign is not active in tbl_FortuneBasicInfo
                TempData["campnotcreatedforbasicinfo"] = "campnotcreatedforbasicinfo";
                return View();
            }
            return View();
        }

        public ActionResult RunCampaign()
        {
            try
            {
                var campdata = entity.tbl_Emailtemp.ToList();
                if (campdata.Count != 0)
                {
                    int? compid = 0;
                    int? brid = 0;
                    string leadname = "";
                    string body = "";
                    string message = "";
                    string frommailid = "";
                    string leadmailid = "";
                    string status = "";
                    string product = "";
                    string loginusername = "";
                    foreach (var item in campdata)
                    {
                        compid = item.CompId;
                        brid = item.BrId;
                        frommailid = item.EmailFrom;
                        leadmailid = item.EmailTo;
                        leadname = item.LeadName;
                        product = item.Product;
                        status = item.Status;
                        message = item.Emailsubject;
                        loginusername = item.LoginUserName;
                        body = item.EmailBody;

                        //calling sendmail action for send an email
                        SendMail(message, body, leadmailid, leadname, product, status, compid, brid);
                        ////  updating current date for identifying the mails sending or not today itself
                        //var updatedate = entity.tbl_campaign.Where(m => m.compId == companypid && m.brId == branchid && m.camp_flag.ToUpper() == "ACTIVE" && m.campId == mou.campid).SingleOrDefault();
                        //updatedate.UpdatedDate = currentdate;
                        //entity.Entry(updatedate);
                        //entity.SaveChanges();

                    }
                }

                    var sms = entity.tbl_Smstemp.ToList();
                    if (sms.Count != 0)
                    {
                        string mobile = "";
                        string content = "";
                        string orgname = "";
                        int? companyid;
                        int? branchid;
                        foreach (var item in sms)
                        {
                            mobile = item.MobileNum;
                            content = item.SmsContent;
                            orgname = item.OrganizationName;
                            companyid = item.CompId;
                            branchid = item.BrId;
                            SendSms(mobile, content, orgname, companyid, branchid);
                        }


                    }
                    if (campdata.Count == 0 && sms.Count==0)
                    {
                        TempData["nothingforrun"] = "nothingforrun";
                        return View();
                
                    }
                
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            TempData["runcampsuccessfully"] = "runcampsuccessfully";
            return View();
        }

        public void SendMail(string body, string message, string leademail, string leadname, string product, string status, int? CurrentCompanyId, int? CurrentCompanyBranchId)
        {

            try
            {
                //int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                //int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

                var orgbasicdata = entity.tbl_FortuneBasicInfo.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
                string FromEmail = "";
                string Org = "";
                foreach (var item in orgbasicdata)
                {
                    FromEmail = item.OrgEmail;
                    Org = item.OrganizationName;
                }



                var mailbody = body;
                var mailmessage = message;
                var tomail = leademail;

                var senderEmail = new MailAddress("info@fortunesofttech.com", Org);
                var receiverEmail = new MailAddress(tomail, "Receiver");
                //string body = "";
                var smtp = new SmtpClient
                {

                    Port = 587,//25
                    Host = "webmail.fortunesofttech.com",
                    //EnableSsl = true,
                    // DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("info@fortunesofttech.com", "@@hyd123"),


                };

                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = mailbody,
                    Body = mailmessage
                })
                {
                    smtp.Send(mess);
                    //log email details
                    string sendermailid = FromEmail;
                    tbl_EmailLog email = new tbl_EmailLog();
                    email.LoginUserName = Session["CurrentUser"].ToString();
                    email.CompId = CurrentCompanyId;
                    email.BrId = CurrentCompanyBranchId;
                    email.EmailFrom = sendermailid;
                    email.EmailTo = tomail;
                    email.LeadName = leadname;
                    email.Product = product;
                    email.Status = status;
                    email.SentDate = DateTime.Now;
                    email.IdentityFlag = 1;
                    entity.tbl_EmailLog.Add(email);
                    entity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
        }
        public void SendSms(string mob, string smscontent,string orgname,int? compid,int? brid)
        {

            try
            {
                int count = 0;
                int actualcount;

                DateTime currentdate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                // DateTime currentdate = DateTime.Now;
                var smscount = entity.tbl_FortuneBasicInfo.Where(m => m.CompId == compid && m.BrId == brid).ToList();

                foreach (var item in smscount)
                {
                    count = Convert.ToInt32(item.SmsCount);
                }
                //actualcount = entity.tbl_SmsLog.Where(C => C.SentDate==currentdate).ToList().Count();
                actualcount = entity.tbl_SmsLog.Where(m => m.SentDate == currentdate && m.CompId == compid && m.BrId == brid).Count();


                if (actualcount <= count)
                {
                    //if we want to restrict sms count appy code here
                }
                    string breakTag = Environment.NewLine;
                    SMSCAPI obj = new SMSCAPI();
                    string strPostResponse;
                    // strPostResponse = obj.SendSMS("fortunesoft", "@@hyd123", "9133188819", "hi", "N&DR=Y", "N", "FORTUN");
                    strPostResponse = obj.SendSMS("fortunesoft", "@@hyd123", "91" + mob + "", "Greetings From+" + orgname + "" + breakTag + smscontent + "", "FORTUN", "N&DR=Y");

                    //log sms details
                    tbl_SmsLog sms = new tbl_SmsLog();
                    sms.LoginUserName = Session["CurrentUser"].ToString();
                    sms.OrganizationName = Session["OrganizationName"].ToString();
                    sms.MobileNum = mob;
                    sms.SentDate = currentdate;
                    sms.CompId = compid;
                    sms.BrId = brid;
                    sms.IdentityFlag = 1;
                    entity.tbl_SmsLog.Add(sms);
                    entity.SaveChanges();
                

            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
        }
    }
}

//public ActionResult Analytics()
//{
//    return JavaScript("window.open('http://analytics.fortunesofttech.com')");
//}