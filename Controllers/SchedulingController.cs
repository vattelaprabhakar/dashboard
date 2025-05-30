using FortuneTechPvtLtd.DataModel;
using FortuneTechPvtLtd.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace FortuneTechPvtLtd.Controllers
{
    public class SchedulingController : Controller
    {
        //
        // GET: /Scheduling/

        FortuneSoftEntities entity = new FortuneSoftEntities();
        Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            List<MailSchedulingModel> emailscheduling = new List<MailSchedulingModel>();
            try
            {

                var products = entity.tbl_campaign.Where(m => m.compId == CurrentCompanyId && m.brId == CurrentCompanyBranchId && m.camp_type.ToUpper() == "SMS".ToUpper()).ToList();
                foreach (var item in products)
                {
                    MailSchedulingModel r = new MailSchedulingModel();
                    r.campid = item.campId;
                    r.campname = item.camp_name;
                    r.camptype = item.camp_type;
                    r.campcategory = item.camp_Category;
                    r.campproduct = item.camp_product;
                    r.campstatus = item.camp_status;
                    r.campflag = item.camp_flag;
                    r.campday = item.camp_day;
                    //r.camptime = item.camp_time;
                    r.campstartdate = item.camp_start_dt;
                    r.campenddate = item.camp_end_dt;
                    r.companyid = item.compId;
                    r.branchid = item.brId;
                    emailscheduling.Add(r);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }

            return View(emailscheduling);
        }
        public ActionResult AddSchedule()
        {
            DropdownProperties();


            MailSchedulingModel r = new MailSchedulingModel();
            return View(r);
        }

        private void DropdownProperties()
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

            //camp type
            //List<CampaignType> camptypeobj = new List<CampaignType>() { 
            //new CampaignType(){camptypeid=1,camptypename="EMAIL"},
            //   new CampaignType(){camptypeid=2,camptypename="SMS"},
            //};
            //ViewBag.camptypeobj = camptypeobj;

            //camp category
            List<CampaignCategory> campcategoryobj = new List<CampaignCategory>() { 
            new CampaignCategory(){campcategoryid=1,campcategoryname="DAILY"},
               new CampaignCategory(){campcategoryid=2,campcategoryname="WEEKLY"},
            };
            ViewBag.campcategoryobj = campcategoryobj;

            //camp Day
            List<CampaignDay> campdayobj = new List<CampaignDay>() { 
            new CampaignDay(){campdayid=1,campdayname="EVERYDAY"},
               new CampaignDay(){campdayid=2,campdayname="MONDAY"},
                  new CampaignDay(){campdayid=3,campdayname="TUESDAY"},
                     new CampaignDay(){campdayid=4,campdayname="WEDNSEDAY"},
                        new CampaignDay(){campdayid=5,campdayname="THURSDAY"},
                           new CampaignDay(){campdayid=6,campdayname="FRIDAY"},
                              new CampaignDay(){campdayid=7,campdayname="SATURDAY"},
                                 new CampaignDay(){campdayid=8,campdayname="SUNDAY"},
            };
            ViewBag.campdayobj = campdayobj;
            //camp status
            //camp category
            List<CampaignStatus> campstatusobj = new List<CampaignStatus>() { 
            new CampaignStatus(){campstatusid=1,campstatusname="ACTIVE"},
               new CampaignStatus(){campstatusid=2,campstatusname="INACTIVE"},
            };
            ViewBag.campstatusobj = campstatusobj;
        }
        [HttpPost]
        public ActionResult AddSchedule(MailSchedulingModel data, FormCollection formdata)
        {
            tbl_campaign comp = new tbl_campaign();
            comp.compId = data.companyid;
            comp.brId = data.branchid;
            comp.camp_name = data.campname;
            comp.camp_product = formdata["hdncampproduct"];
            comp.camp_productId = Convert.ToInt32(data.campproduct);
            comp.camp_status = formdata["hdncampstatus"];
            comp.camp_statusId = Convert.ToInt32(data.campstatus);
            comp.camp_Category = formdata["hdncampcategory"];
            comp.camp_CategoryId = Convert.ToInt32(data.campcategory);
            //comp.camp_type = formdata["hdncamptype"];
            //comp.camp_typeId = Convert.ToInt32(data.camptype);
            comp.camp_type = "SMS";
            comp.camp_day = formdata["hdncampday"];
            comp.camp_dayId = Convert.ToInt32(data.campday);
            comp.camp_flag = formdata["hdncampflag"];
            comp.camp_flagId = Convert.ToInt32(data.campflag);
            comp.camp_start_dt = data.campstartdate;
            comp.camp_end_dt = data.campenddate;
            //comp.camp_time = data.camptime;
            entity.tbl_campaign.Add(comp);
            entity.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            MailSchedulingModel r = new MailSchedulingModel();
            try
            {
                DropdownProperties();
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

                var data = entity.tbl_campaign.Where(m => m.campId == id && m.compId == CurrentCompanyId && m.brId == CurrentCompanyBranchId).FirstOrDefault();

                r.campid = data.campId;
                r.campname = data.camp_name;
                r.campproduct = Convert.ToString(data.camp_productId);
                r.campstatus = Convert.ToString(data.camp_statusId);
                r.campcategory = Convert.ToString(data.camp_CategoryId);
                //r.camptype = Convert.ToString(data.camp_typeId);
                r.campday = Convert.ToString(data.camp_dayId);
                //r.camptime = data.camp_time;
                r.campstartdate = data.camp_start_dt;
                r.campenddate = data.camp_end_dt;
                r.campflag = Convert.ToString(data.camp_flagId);
                r.companyid = data.compId;
                r.branchid = data.brId;
                //prop's


            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return View(r);
        }
        [HttpPost]
        public ActionResult Update(MailSchedulingModel data, FormCollection formdata)
        {
            var data1 = entity.tbl_campaign.Where(m => m.campId == data.campid && m.compId == data.companyid && m.brId == data.branchid).SingleOrDefault();

            if (data1 != null)
            {
                try
                {

                    data1.compId = data.companyid;
                    data1.brId = data.branchid;
                    data1.camp_name = data.campname;
                    data1.camp_product = formdata["hdncampproduct"];
                    data1.camp_productId = Convert.ToInt32(data.campproduct);
                    data1.camp_status = formdata["hdncampstatus"];
                    data1.camp_statusId = Convert.ToInt32(data.campstatus);
                    data1.camp_Category = formdata["hdncampcategory"];
                    data1.camp_CategoryId = Convert.ToInt32(data.campcategory);
                    //data1.camp_type = formdata["hdncamptype"];
                    //data1.camp_typeId = Convert.ToInt32(data.camptype);
                    data1.camp_type = "SMS";
                    data1.camp_day = formdata["hdncampday"];
                    data1.camp_dayId = Convert.ToInt32(data.campday);
                    data1.camp_flag = formdata["hdncampflag"];
                    data1.camp_flagId = Convert.ToInt32(data.campflag);
                    data1.camp_start_dt = data.campstartdate;
                    data1.camp_end_dt = data.campenddate;
                    //data1.camp_time = data.camptime;
                    entity.Entry(data1);
                    entity.SaveChanges();
                }
                catch (Exception ex)
                {
                    logger.ErrorException("error occurred at", ex);
                }
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
                var data = entity.tbl_campaign.Where(m => m.campId == id && m.compId == CurrentCompanyId && m.brId == CurrentCompanyBranchId).FirstOrDefault();
                entity.Entry(data).State = (System.Data.Entity.EntityState)System.Data.EntityState.Deleted;
                entity.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return RedirectToAction("Index");
        }

        //public ActionResult CampaignSchdule()
        //{

        //    //var currentuser = Session["CurrentUser"].ToString();
        //    //var currentuserrole = Session["UserRole"].ToString();
        //    //int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
        //    //int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

        //    DateTime? vfromdate = DateTime.Now;
        //    DateTime? vtodate = DateTime.Now;
        //    //string campaignstatus = "";
        //    //string campaigntype = "";
        //    int? companypid = 0;
        //    int? branchid = 0;
        //    DateTime? compaigndate = DateTime.Now;
        //    DateTime currentdate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
        //    string frommailid="";


        //    //verify valid from date and to date,email flag and email alert flag
        //    var data1 = entity.tbl_FortuneBasicInfo.Where(m => m.FromDate <= currentdate && vtodate >= currentdate && m.email_flag.ToUpper() == "Yes".ToUpper() && m.email_alert.ToUpper() == "Yes".ToUpper()).ToList();//&& m.sms_flag.ToUpper() == "Yes".ToUpper()
        //    if (data1.Count!=0)
        //    {
        //        foreach (var item in data1)
        //        {
        //            vfromdate = item.FromDate;
        //            vtodate = item.ToDate;
        //            companypid = item.CompId;
        //            branchid = item.BrId;
        //            frommailid=item.OrgEmail;
        //            string currentday = Convert.ToString(DateTime.Now.DayOfWeek);

        //            //verify camp start date and end date ,camp_flag is active or not
        //            var data2 = entity.tbl_campaign.Where(m => m.compId == companypid && m.brId == branchid && m.camp_start_dt <= currentdate && m.camp_end_dt >= currentdate && m.camp_flag.ToUpper() == "ACTIVE").ToList();
        //            //  int curdaycount = entity.tbl_campaign.Where(m => m.compId == companypid && m.brId == branchid && m.camp_day == currentday).ToList().Count ;
        //            if (data2.Count != 0)
        //            {
        //                MailSchedulingModel mou = new MailSchedulingModel();
        //                foreach (var item2 in data2)
        //                {
        //                    mou.campid = item2.campId;
        //                    mou.campproduct = item2.camp_product;
        //                    mou.campstatus = item2.camp_status;
        //                    mou.camptype = item2.camp_type;
        //                    mou.campcategory = item2.camp_Category;
        //                    string campaignday = item2.camp_day;
        //                    //  mou.campstartdate = item2.camp_start_dt;
        //                    //  mou.campenddate = item2.camp_end_dt;

        //                    string daily = "DAILY";
        //                    string weekly = "Weekly";
        //                    string email = "EMAIL";
        //                    //DateTime currentdate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
        //                    //checking daily for email
        //                    if (mou.campcategory.ToUpper() == daily.ToUpper() && mou.camptype.ToUpper() == email.ToUpper())//&& campaignday==currentday
        //                    {

        //                        // variables for send email
        //                        string product = "";
        //                        string leadstatus = "";
        //                        string leadmail = "";
        //                        string leadname = "";
        //                        //int? companypid = 0;
        //                        //int? branchid = 0;

        //                        var data3 = entity.tbl_EnquiryForm.Where(m => m.CompId == companypid && m.BrId == branchid && m.Product == mou.campproduct && m.Status == mou.campstatus).ToList();
        //                        if (data3.Count != 0)
        //                        {
        //                            foreach (var item4 in data3)
        //                            {
        //                                //product = item4.Product;
        //                                //leadstatus = item4.Status;
        //                                companypid = item4.CompId;
        //                                branchid = item4.BrId;
        //                                leadmail = item4.Email;
        //                                leadname = item4.LeadName;

        //                                //organization mail
        //                                //var orgbasicdata = entity.tbl_FortuneBasicInfo.Where(m => m.CompId == companypid && m.BrId == branchid).ToList();
        //                                //string FromEmail = "";
        //                                //string Org = "";
        //                                //foreach (var org in orgbasicdata)
        //                                //{
        //                                //    FromEmail = item.OrgEmail;
        //                                //    Org = item.OrganizationName;
        //                                //}
        //                                //var senderEmail = new MailAddress("info@fortunesofttech.com", Org);
        //                                //var receiverEmail = new MailAddress(leadmail, "Receiver");

        //                                //sending an email
        //                                var courseemail = entity.tbl_LeadEmailContent.Where(m => m.InterestedCourse == mou.campproduct && m.leadstatus == mou.campstatus && m.CompId == companypid && m.BrId == branchid).ToList();
        //                                //
        //                                MailModel mailmodel = new MailModel();
        //                                if (courseemail != null)
        //                                {
        //                                    foreach (var item1 in courseemail)
        //                                    {
        //                                        mailmodel.Subject = item1.EmailSubject;
        //                                        mailmodel.Message = item1.EmailBody;
        //                                    }
        //                                    tbl_Emailtemp temp = new tbl_Emailtemp();
        //                                    temp.CompId = companypid;
        //                                    temp.BrId = branchid;
        //                                    temp.EmailFrom = frommailid;
        //                                    temp.EmailTo = leadmail;
        //                                    temp.Emailsubject = mailmodel.Subject;
        //                                    temp.EmailBody = mailmodel.Message;
        //                                    temp.LoginUserName=Session["CurrentUser"].ToString();
        //                                    temp.Product = mou.campproduct;
        //                                    temp.Status = mou.campstatus;
        //                                    temp.LeadName = leadname;
        //                                    temp.SentDate = currentdate;
        //                                    //push to emailtemp table for future verifications
        //                                    entity.tbl_Emailtemp.Add(temp);
        //                                    entity.SaveChanges();

        //                                    //calling sendmail action for send an email
        //                                    //SendMail(mailmodel.Subject, mailmodel.Message, leadmail, leadname, mou.campproduct, mou.campstatus, companypid, branchid);
        //                                    ////  updating current date for identifying the mails sending or not today itself
        //                                    //var updatedate = entity.tbl_campaign.Where(m => m.compId == companypid && m.brId == branchid && m.camp_flag.ToUpper() == "ACTIVE" && m.campId == mou.campid).SingleOrDefault();
        //                                    //updatedate.UpdatedDate = currentdate;
        //                                    //entity.Entry(updatedate);
        //                                    //entity.SaveChanges();
                                         
        //                                }
        //                            }
        //                        }
        //                    }
        //                    //checking weekly with day for email
        //                    if (mou.campcategory.ToUpper() == weekly.ToUpper() && mou.camptype == email.ToUpper() && campaignday == currentday)
        //                    {
        //                        // variables for send email
        //                        //string product = "";
        //                        //string leadstatus = "";
        //                        string leadmail = "";
        //                        string leadname = "";
        //                        //int? companypid = 0;
        //                        //int? branchid = 0;

        //                        var data5 = entity.tbl_EnquiryForm.Where(m => m.CompId == companypid && m.BrId == branchid && m.Product == mou.campproduct && m.Status == mou.campstatus).ToList();
        //                        if (data5.Count != 0)
        //                        {
        //                            foreach (var item4 in data5)
        //                            {
        //                                //product = item4.Product;
        //                                //leadstatus = item4.Status;
        //                                companypid = item4.CompId;
        //                                branchid = item4.BrId;
        //                                leadmail = item4.Email;
        //                                leadname = item4.LeadName;
        //                                //sending an email
        //                                var courseemail = entity.tbl_LeadEmailContent.Where(m => m.InterestedCourse == mou.campproduct && m.leadstatus == mou.campstatus && m.CompId == companypid && m.BrId == branchid).ToList();

        //                                MailModel mailmodel = new MailModel();
        //                                if (courseemail.Count != 0)
        //                                {
        //                                    foreach (var item1 in courseemail)
        //                                    {
        //                                        mailmodel.Subject = item1.EmailSubject;
        //                                        mailmodel.Message = item1.EmailBody;
        //                                    }
        //                                    tbl_Emailtemp temp = new tbl_Emailtemp();
        //                                    temp.CompId = companypid;
        //                                    temp.BrId = branchid;
        //                                    temp.EmailFrom = frommailid;
        //                                    temp.EmailTo = leadmail;
        //                                    temp.Emailsubject = mailmodel.Subject;
        //                                    temp.EmailBody = mailmodel.Message;
        //                                    temp.LoginUserName = Session["CurrentUser"].ToString();
        //                                    temp.Product = mou.campproduct;
        //                                    temp.Status = mou.campstatus;
        //                                    temp.LeadName = leadname;
        //                                    temp.SentDate = currentdate;
        //                                    //push to emailtemp table for future verifications
        //                                    entity.tbl_Emailtemp.Add(temp);
        //                                    entity.SaveChanges();

        //                                    //calling sendmail action for send an email
        //                                 //   SendMail(mailmodel.Subject, mailmodel.Message, leadmail, leadname, mou.campproduct, mou.campstatus, companypid, branchid);
        //                                    //  updating current date for identifying the mails sending or not today itself
        //                                 //   var updatedate = entity.tbl_campaign.Where(m => m.compId == companypid && m.brId == branchid && m.camp_flag.ToUpper() == "ACTIVE" && m.campId == mou.campid).SingleOrDefault();
        //                                 //   updatedate.UpdatedDate = currentdate;
        //                                 //   entity.Entry(updatedate);
        //                                 //   entity.SaveChanges();
        //                                }

        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                //from date or  end date or campflag is not active in tbl_campaign
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //fromdate and end date and emailaerts flag for campaign is not active in tbl_FortuneBasicInfo
        //    }
        //    return RedirectToAction("Index", "Dashboard");
        //}

        //public ActionResult RunCampaign()
        //{
        //    try
        //    {
        //        var campdata = entity.tbl_Emailtemp.ToList();
        //        if (campdata.Count != 0)
        //        {
        //            int? compid = 0;
        //            int? brid = 0;
        //            string leadname = "";
        //            string body = "";
        //            string message = "";
        //            string frommailid = "";
        //            string leadmailid = "";
        //            string status = "";
        //            string product = "";
        //            string loginusername = "";
        //            foreach (var item in campdata)
        //            {
        //                compid = item.CompId;
        //                brid = item.BrId;
        //                frommailid = item.EmailFrom;
        //                leadmailid = item.EmailTo;
        //                leadname = item.LeadName;
        //                product = item.Product;
        //                status = item.Status;
        //                message = item.Emailsubject;
        //                loginusername = item.LoginUserName;
        //                body = item.EmailBody;

        //                //calling sendmail action for send an email
        //                SendMail(message, body, leadmailid, leadname, product, status, compid, brid);
        //                ////  updating current date for identifying the mails sending or not today itself
        //                //var updatedate = entity.tbl_campaign.Where(m => m.compId == companypid && m.brId == branchid && m.camp_flag.ToUpper() == "ACTIVE" && m.campId == mou.campid).SingleOrDefault();
        //                //updatedate.UpdatedDate = currentdate;
        //                //entity.Entry(updatedate);
        //                //entity.SaveChanges();

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.ErrorException("error occurred at", ex);
        //    }
        //    return RedirectToAction("Index", "Dashboard");
        //}

        //public void SendMail(string body, string message, string leademail, string leadname, string product, string status, int? CurrentCompanyId, int? CurrentCompanyBranchId)
        //{

        //    try
        //    {
        //        //int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
        //        //int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

        //        var orgbasicdata = entity.tbl_FortuneBasicInfo.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
        //        string FromEmail = "";
        //        string Org = "";
        //        foreach (var item in orgbasicdata)
        //        {
        //            FromEmail = item.OrgEmail;
        //            Org = item.OrganizationName;
        //        }



        //        var mailbody = body;
        //        var mailmessage = message;
        //        var tomail = leademail;

        //        var senderEmail = new MailAddress("info@fortunesofttech.com", Org);
        //        var receiverEmail = new MailAddress(tomail, "Receiver");
        //        //string body = "";
        //        var smtp = new SmtpClient
        //        {

        //            Port = 587,//25
        //            Host = "webmail.fortunesofttech.com",
        //            //EnableSsl = true,
        //            // DeliveryMethod = SmtpDeliveryMethod.Network,
        //            UseDefaultCredentials = false,
        //            Credentials = new NetworkCredential("info@fortunesofttech.com", "@@hyd123"),


        //        };

        //        using (var mess = new MailMessage(senderEmail, receiverEmail)
        //        {
        //            Subject = mailbody,
        //            Body = mailmessage
        //        })
        //        {
        //            smtp.Send(mess);
        //            //log email details
        //            string sendermailid = FromEmail;
        //            tbl_EmailLog email = new tbl_EmailLog();
        //            email.LoginUserName = Session["CurrentUser"].ToString();
        //            email.CompId = CurrentCompanyId;
        //            email.BrId = CurrentCompanyBranchId;
        //            email.EmailFrom = sendermailid;
        //            email.EmailTo = tomail;
        //            email.LeadName = leadname;
        //            email.Product = product;
        //            email.Status = status;
        //            email.SentDate = DateTime.Now;
        //            email.IdentityFlag = 1;
        //            entity.tbl_EmailLog.Add(email);
        //            entity.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.ErrorException("error occurred at", ex);
        //    }
        //}

    }
}
