using FortuneTechPvtLtd.DataModel;
using FortuneTechPvtLtd.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoleBasedSecurity.CustomSecurity;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Web.Helpers;
using System.Web.UI;
using FortuneTechPvtLtd.SMSCountry;
using NLog;
using System.IO;

namespace FortuneTechPvtLtd.Controllers
{
    public class EnquiryController : Controller
    {
        //
        // GET: /Enquiry/

        FortuneSoftEntities entity = new FortuneSoftEntities();
        Logger logger = LogManager.GetCurrentClassLogger();

        //[CustomExceptionHandlerFilter]
        public ActionResult Index()
        {
            //  EnquiryModel studentmodel = new EnquiryModel();
            return View();
        }
        //[CustomExceptionHandlerFilter]
        public JsonResult GetAllLeads()
        {
            //var currentdate = DateTime.Now.ToString("yyyy-mm-dd");
            DateTime currentdate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

            var currentuser = Session["CurrentUser"].ToString();
            var currentuserrole = Session["UserRole"].ToString();
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            //get all leads
            //    var studentdata = entity.tbl_StudentEnquiryForm.ToList();

            //get user related leads

            var studentdata = entity.tbl_EnquiryForm.Where(m => m.LeadAssignedTo.ToUpper() == currentuser.ToUpper() || m.LeadAssignedby.ToUpper() == currentuser.ToUpper() || m.LeadOwner.ToUpper() == currentuser.ToUpper() || currentuserrole == "Admin" || currentuserrole == "Manager").Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).OrderByDescending(m => m.Id).ToList();
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

                studentmodel.companyid = item.CompId;
                studentmodel.branchid = item.BrId;

                data.Add(studentmodel);

            }

            return Json(data, JsonRequestBehavior.AllowGet);


            //SqlParameter param1 = new SqlParameter("@LeadName", studentmodel.LeadName);
            //SqlParameter param2 = new SqlParameter("@Address", studentmodel.Address);

        }
        [HttpGet]
        [CustomAuthorization("Admin,Staff,Manager,Executive,Supervisor")]
        public ActionResult Create()
        {

            if (ModelState.IsValid)
            {
                try
                {
                    RadiobuttonPropertyvalues();
                    EnquiryModel empmodel = new EnquiryModel();
                    return View(empmodel);
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
                    throw dbEx;
                }
                catch (Exception ex)
                {
                    logger.ErrorException("error occurred at", ex);
                }
            }
            return RedirectToAction("UnAuthorize", "Error");
        }

        private void RadiobuttonPropertyvalues()
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
            //List<Product> course = new List<Product>() { 

            //new Courses(){CourseId=1,CourseName="Advanced Digital Marketing",isselected=false},
            //new Courses(){CourseId=2,CourseName="Advanced Digital Marketing ",isselected=false},
            //new Courses(){CourseId=3,CourseName="Phython Programming",isselected=false},
            //new Courses(){CourseId=4,CourseName="Internet Of Things",isselected=false},
            //            };
            //ViewBag.courses = course;
            ////location
            //List<Location> location = new List<Location>() { 
            //new Location(){LocationId=1,LocationName="Mythrivanam",isselected=false},
            //new Location(){LocationId=2,LocationName="Ameerpet",isselected=false},
            //new Location(){LocationId=3,LocationName="Kphb",isselected=false},
            //new Location(){LocationId=4,LocationName="Dilshuknagar",isselected=false},
            //};
            //  ViewBag.location = location;
            //batch
            //List<Batch> batch = new List<Batch>() { 
            //new Batch(){BatchId=1,BatchName="Week Days(Mon-Fri-2 Hrs)",isselected=false},
            //new Batch(){BatchId=1,BatchName="Weekends(Saturday 11AM-5PM)",isselected=false},
            //new Batch(){BatchId=3,BatchName="Weekdays(Mon-Fri-7AM-9AM)",isselected=false},
            //new Batch(){BatchId=4,BatchName="Weekdays(Mon-Fri-7PM-9PM)",isselected=false},
            //};
            //  ViewBag.batch = batch;
            //Reference

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

            //List<Reference> reference = new List<Reference>() { 
            //new Reference(){ReferenceId=1,ReferenceName="Walk-In",isselected=false},
            //new Reference(){ReferenceId=2,ReferenceName="Reference",isselected=false},
            //new Reference(){ReferenceId=3,ReferenceName="Sulekha",isselected=false},
            //new Reference(){ReferenceId=4,ReferenceName="Urbanpro",isselected=false},
            //new Reference(){ReferenceId=5,ReferenceName="Justdial",isselected=false},
            //new Reference(){ReferenceId=6,ReferenceName="Facebook",isselected=false},
            //new Reference(){ReferenceId=7,ReferenceName="Website",isselected=false},
            //new Reference(){ReferenceId=8,ReferenceName="Other",isselected=false},
            //};
            //ViewBag.reference = reference;
            //timings
            //List<JoinTimings> jointimings = new List<JoinTimings>() { 
            //new JoinTimings(){JoinId=1,JoinName="Just Enquiry",isselected=false},
            //new JoinTimings(){JoinId=2,JoinName="Immediate",isselected=false},
            //new JoinTimings(){JoinId=3,JoinName="Later",isselected=false},
            //};
            //ViewBag.jointimings = jointimings;
            //alerts
            List<Alerts> alert = new List<Alerts>() { 
            new Alerts(){AlertId=1,AlertName="Yes"},
            new Alerts(){AlertId=2,AlertName="No",isselected=true},
            };
            ViewBag.alters = alert;

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
        }
        [HttpPost]
        [CustomAuthorization("Admin,Staff,Manager,Executive,Supervisor")]
        public ActionResult Create(EnquiryModel data, FormCollection formdata)
        {
            if (ModelState.IsValid)
            {
                if (data != null)
                {
                    try
                    {
                        //alert type
                      //  var alerttype = formdata["hdnalerttype"];
                        var selectedcourse = formdata["hdnselectedinterestedcourse"];
                     
                       
                      
                        var leademail = data.Email;
                        var mobilenum = data.Mobile;
                        var leadstatus = formdata["hdnselectedstatus"];

                        tbl_EnquiryForm studentmodel = new tbl_EnquiryForm();
                        studentmodel.LeadName = data.LeadName;
                        studentmodel.Mobile = data.Mobile;
                        studentmodel.Email = data.Email;
                        studentmodel.Product = formdata["hdnselectedinterestedcourse"];
                        int selectedproductid = Convert.ToInt16(data.selectedinterestedcourse);
                        studentmodel.Reference = formdata["hdnselectedrefby"];
                        int selectedsourceid = Convert.ToInt16(data.selectedrefby);
                        studentmodel.Status = formdata["hdnselectedstatus"];
                        int selectedstatusid = Convert.ToInt16(data.selectedstatus);
                        studentmodel.WantSMSAlerts = formdata["hdnseletedwantsmsalerts"];
                        studentmodel.WantMailAlerts = formdata["hdnseletedwantmailalerts"];
                        studentmodel.Address = data.Address;
                        studentmodel.LeadAssignedby = data.LeadAssignedby;
                        studentmodel.LeadAssignedTo = data.selectedleadassignedto;

                        studentmodel.LeadOwner = data.LeadOwner;
                        studentmodel.Industry = data.Industry;
                        studentmodel.Website = data.Website;
                        studentmodel.Rating = data.Rating;
                        studentmodel.Comment1 = data.Comment1;
                        studentmodel.Comment2 = data.Comment2;
                        studentmodel.Comment3 = data.Comment3;

                        studentmodel.LeadDate = data.Leaddate;
                        studentmodel.FollowUpdate = data.Followupdate;
                        studentmodel.CompId = data.companyid;
                        studentmodel.BrId = data.branchid;
                        //studentmodel.Wanttojoin = formdata["hdnselectedwanttojoin"];
                        //studentmodel.Preferredlocation = formdata["hdnselectedpreferredlocation"];
                        //studentmodel.Prefbatch = formdata["hdnselectedpreferredbatch"];
                        // studentmodel.Workexpdomain = data.WorkExperienceDomain;
                        //  studentmodel.Yearofpassedout = Convert.ToString(data.YearOfPassedOut);
                        //studentmodel.Qualification = data.Qualification;
                        //studentmodel.ProfessionalDetails = formdata["hdnselectedprofession"];

                        SqlParameter param1 = new SqlParameter("@LeadName", studentmodel.LeadName);
                        SqlParameter param2 = new SqlParameter("@Address", ((object)studentmodel.Address) ?? DBNull.Value);
                        SqlParameter param3 = new SqlParameter("@Mobile", studentmodel.Mobile);
                        SqlParameter param4 = new SqlParameter("@Email", studentmodel.Email);
                        SqlParameter param5 = new SqlParameter("@Product", studentmodel.Product);
                        SqlParameter param6 = new SqlParameter("@Reference", studentmodel.Reference);
                        SqlParameter param7 = new SqlParameter("@WantSMSAlerts", ((object)studentmodel.WantSMSAlerts) ?? DBNull.Value);
                        SqlParameter param8 = new SqlParameter("@LeadAssignedby", studentmodel.LeadAssignedby);
                        SqlParameter param9 = new SqlParameter("@Status", studentmodel.Status);
                        //if (!String.IsNullOrEmpty(studentmodel.Website))
                        //{

                        //}
                        SqlParameter param10 = new SqlParameter("@Website", ((object)studentmodel.Website) ?? DBNull.Value);
                        SqlParameter param11 = new SqlParameter("@Industry", ((object)studentmodel.Industry) ?? DBNull.Value);
                        SqlParameter param12 = new SqlParameter("@Rating", ((object)studentmodel.Rating) ?? DBNull.Value);
                        SqlParameter param13 = new SqlParameter("@Comment1", ((object)studentmodel.Comment1) ?? DBNull.Value);
                        SqlParameter param14 = new SqlParameter("@Comment2", ((object)studentmodel.Comment2) ?? DBNull.Value);
                        SqlParameter param15 = new SqlParameter("@Comment3", ((object)studentmodel.Comment3) ?? DBNull.Value);
                        SqlParameter param16 = new SqlParameter("@LeadOwner", studentmodel.LeadOwner);
                        SqlParameter param17 = new SqlParameter("@LeadDate", studentmodel.LeadDate);
                        SqlParameter param18 = new SqlParameter("@FollowUpdate", ((object)studentmodel.FollowUpdate) ?? DBNull.Value);

                        SqlParameter param19 = new SqlParameter("@CompId", studentmodel.CompId);
                        SqlParameter param20 = new SqlParameter("@BrId", studentmodel.BrId);

                        SqlParameter param21 = new SqlParameter("@SelectedProductId",selectedproductid);
                        SqlParameter param22 = new SqlParameter("@SelectedStatusId", selectedstatusid);
                        SqlParameter param23 = new SqlParameter("@SelectedReference",selectedsourceid);
                        SqlParameter param24 = new SqlParameter("@WantMailAlerts", ((object)studentmodel.WantMailAlerts) ?? DBNull.Value);

                        //SqlParameter param8 = new SqlParameter("@stu_Preferredlocation", studentmodel.Preferredlocation);
                        //SqlParameter param9 = new SqlParameter("@stu_Prefbatch", studentmodel.Prefbatch);
                        //   SqlParameter param4 = new SqlParameter("@stu_Yearofpassedout", studentmodel.Yearofpassedout);
                        // SqlParameter param5 = new SqlParameter("@stu_Qualification", studentmodel.Qualification);
                        //SqlParameter param6 = new SqlParameter("@stu_ProfessionalDetails", studentmodel.ProfessionalDetails);
                        //SqlParameter param14 = new SqlParameter("@stu_Workexpdomain", studentmodel.Workexpdomain);
                        //SqlParameter param11 = new SqlParameter("@stu_Wanttojoin", studentmodel.Wanttojoin);

                        var data1 = entity.Database.ExecuteSqlCommand("Proc_AddLead @CompId,@BrId, @LeadName,@Address, @Mobile,@Email,@Product,@Reference,@WantSMSAlerts,@LeadAssignedby,@Status,@Website,@Industry,@Rating,@Comment1,@Comment2,@Comment3,@LeadOwner,@LeadDate,@FollowUpdate,@SelectedProductId,@SelectedStatusId,@SelectedReference,@WantMailAlerts", param19, param20, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14, param15, param16, param17, param18, param21, param22, param23, param24);

                        // entity.tbl_StudentEnquiryForm.Add(studentmodel);
                        //        entity.Proc_AddLead(stu_Name, stu_Address, stu_Mobile, stu_Email, professionaldetails, stu_Qualification, stu_Yearofpassedout, stu_Workexpdomain, stu_InterestedCourse, stu_Preferredlocation, stu_Prefbatch, stu_Reference, stu_Wanttojoin, stu_Wantalerts, stu_LeadAssignedby, stu_Status, created_Date);
                        entity.SaveChanges();

                        //get lead selected course wise content for email and alerts checked
                        if (studentmodel.WantSMSAlerts == "Yes")
                        {
                                //for sending an SMS
                                var smscontent = entity.tbl_SmsContent.Where(m => m.InterestedCourse == selectedcourse && m.leadstatus == leadstatus && m.CompId == data.companyid && m.BrId == data.branchid).ToList();
                                string sendcontent = "";
                                if (smscontent != null)
                                {
                                    foreach (var item in smscontent)
                                    {
                                        sendcontent = item.SmsContent;
                                        SendSms(mobilenum, sendcontent);
                                    }
                                }
                        }
                            if (studentmodel.WantMailAlerts == "Yes")
                            {
                                //sending an email
                                var courseemail = entity.tbl_LeadEmailContent.Where(m => m.InterestedCourse == selectedcourse && m.leadstatus == leadstatus && m.CompId == data.companyid && m.BrId == data.branchid).ToList();

                                MailModel mailmodel = new MailModel();
                                if (courseemail != null)
                                {
                                    foreach (var item in courseemail)
                                    {
                                        mailmodel.Subject = item.EmailSubject;
                                        mailmodel.Message = item.EmailBody;
                                    }

                                    //calling sendmail action for send an email
                                    SendMail(mailmodel.Subject, mailmodel.Message, leademail, data.LeadName, selectedcourse, leadstatus);
                                }
                            }
                            return RedirectToAction("Index");
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
                }
            }
            return RedirectToAction("UnAuthorize", "Error");
        }
        [HttpGet]
        //[CustomExceptionHandlerFilter]
        [CustomAuthorization("Admin,Staff,Manager,Executive,Supervisor")]
        public ActionResult Edit(int? Id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                    int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

                    var data = entity.tbl_EnquiryForm.Where(m => m.Id == Id && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).FirstOrDefault();
                    //radiobutton property values for radiobutton generation
                    RadiobuttonPropertyvalues();
                    EnquiryModel s = new EnquiryModel();
                    s.companyid = data.CompId;
                    s.branchid = data.BrId;
                    s.Id = data.Id;
                    s.LeadName = data.LeadName;
                    s.Mobile = data.Mobile;
                    s.Email = data.Email;
                    s.Address = data.Address;
                    s.selectedinterestedcourse = data.Product;
                    s.selectedrefby = data.Reference;
                    s.seletedwantsmsalerts = data.WantSMSAlerts;
                    s.seletedwantmailalerts = data.WantMailAlerts;
                    s.LeadAssignedby = data.LeadAssignedby;
                    s.LeadOwner = data.LeadOwner;
                    s.selectedstatus = data.Status;
                    s.Rating = data.Rating;
                    s.Comment1 = data.Comment1;
                    s.Comment2 = data.Comment2;
                    s.Comment3 = data.Comment3;
                    s.Website = data.Website;
                    s.Industry = data.Industry;
                    s.Leaddate = data.LeadDate;
                    s.Followupdate = data.FollowUpdate;


                    //  s.Qualification = data.Qualification;
                    //s.YearOfPassedOut = data.Yearofpassedout;
                    //s.selectedprofessionaldetails = data.ProfessionalDetails;
                    //s.WorkExperienceDomain = data.Workexpdomain;
                    //s.selectedpreferredlocation = data.Preferredlocation;
                    //s.selectedpreferredbatch = data.Prefbatch;
                    // s.selectedwanttojoin = data.Wanttojoin;
                    return View(s);
                }
                catch (Exception ex)
                {
                    logger.ErrorException("error occurred at", ex);
                }
            }

            return RedirectToAction("UnAuthorize", "Error");
        }

        [HttpPost]
        [CustomAuthorization("Admin,Staff,Manager,Executive,Supervisor")]
        public ActionResult Update(EnquiryModel data, FormCollection formcoll)
        {
            //mail
            var selectedcourse = formcoll["hdnselectedinterestedcourse"];
            var leadmail = data.Email;
            if (ModelState.IsValid)
            {
                try
                {
                    var studentmodel = entity.tbl_EnquiryForm.Where(m => m.Id == data.Id && m.CompId == data.companyid && m.BrId == data.branchid).SingleOrDefault();
                    if (studentmodel != null)
                    {
                        var leadname = data.LeadName;
                        //alert type
                        var alerttype = formcoll["hdnalerttype"];
                        var leadstatus = formcoll["hdnselectedstatus"];
                        var product = formcoll["hdnselectedinterestedcourse"];
                        var mobilenum = data.Mobile;
                        int selectedproductid = Convert.ToInt16(data.selectedinterestedcourse);
                        int selectedsourceid = Convert.ToInt16(data.selectedrefby);
                        int selectedstatusid = Convert.ToInt16(data.selectedstatus);

                        studentmodel.CompId = data.companyid;
                        studentmodel.BrId = data.branchid;
                        studentmodel.LeadName = data.LeadName;
                        studentmodel.Mobile = data.Mobile;
                        studentmodel.Email = data.Email;
                        studentmodel.Product = formcoll["hdnselectedinterestedcourse"];
                        studentmodel.Reference = formcoll["hdnselectedrefby"];
                        studentmodel.WantSMSAlerts = formcoll["hdnseletedwantsmsalerts"];
                        studentmodel.WantMailAlerts = formcoll["hdnseletedwantmaialerts"];
                        studentmodel.Status = formcoll["hdnselectedstatus"];
                        studentmodel.Address = data.Address;
                        studentmodel.LeadAssignedby = data.LeadAssignedby;
                        studentmodel.Website = data.Website;
                        studentmodel.Industry = data.Industry;
                        studentmodel.Id = data.Id;
                        studentmodel.LeadOwner = data.LeadOwner;
                        studentmodel.Rating = data.Rating;
                        studentmodel.Comment1 = data.Comment1;
                        studentmodel.Comment2 = data.Comment2;
                        studentmodel.Comment3 = data.Comment3;
                        studentmodel.LeadDate = data.Leaddate;
                        studentmodel.FollowUpdate = data.Followupdate;



                        //    studentmodel.Yearofpassedout = (data.YearOfPassedOut);
                        //  studentmodel.Qualification = data.Qualification;
                        // studentmodel.ProfessionalDetails = formcoll["hdnselectedprofession"];
                        // studentmodel.Preferredlocation = formcoll["hdnselectedpreferredlocation"];
                        //studentmodel.Prefbatch = formcoll["hdnselectedpreferredbatch"];
                        //studentmodel.Wanttojoin = formcoll["hdnselectedwanttojoin"];
                        //studentmodel.Workexpdomain = data.WorkExperienceDomain;

                        //update using sp
                        SqlParameter param1 = new SqlParameter("@LeadName", studentmodel.LeadName);
                        SqlParameter param2 = new SqlParameter("@Address", ((object)studentmodel.Address) ?? DBNull.Value);
                        SqlParameter param3 = new SqlParameter("@Mobile", studentmodel.Mobile);
                        SqlParameter param4 = new SqlParameter("@Email", studentmodel.Email);
                        SqlParameter param5 = new SqlParameter("@Product", studentmodel.Product);
                        SqlParameter param6 = new SqlParameter("@Reference", studentmodel.Reference);
                        SqlParameter param7 = new SqlParameter("@WantSMSAlerts", ((object)studentmodel.WantSMSAlerts) ?? DBNull.Value);
                        SqlParameter param8 = new SqlParameter("@LeadAssignedby", studentmodel.LeadAssignedby);
                        SqlParameter param9 = new SqlParameter("@Status", studentmodel.Status);
                        //if (!String.IsNullOrEmpty(studentmodel.Website))
                        //{

                        //}
                        SqlParameter param10 = new SqlParameter("@Website", ((object)studentmodel.Website) ?? DBNull.Value);
                        SqlParameter param11 = new SqlParameter("@Industry", ((object)studentmodel.Industry) ?? DBNull.Value);
                        SqlParameter param12 = new SqlParameter("@Rating", ((object)studentmodel.Rating) ?? DBNull.Value);
                        SqlParameter param13 = new SqlParameter("@Comment1", ((object)studentmodel.Comment1) ?? DBNull.Value);
                        SqlParameter param14 = new SqlParameter("@Comment2", ((object)studentmodel.Comment2) ?? DBNull.Value);
                        SqlParameter param15 = new SqlParameter("@Comment3", ((object)studentmodel.Comment3) ?? DBNull.Value);
                        SqlParameter param16 = new SqlParameter("@LeadOwner", studentmodel.LeadOwner);
                        SqlParameter param17 = new SqlParameter("@LeadDate", studentmodel.LeadDate);
                        SqlParameter param18 = new SqlParameter("@FollowUpdate", ((object)studentmodel.FollowUpdate) ?? DBNull.Value);
                        SqlParameter param20 = new SqlParameter("@CompId", studentmodel.CompId);
                        SqlParameter param21 = new SqlParameter("@BrId", studentmodel.BrId);
                        SqlParameter param22 = new SqlParameter("@SelectedProductId", selectedproductid);
                        SqlParameter param23 = new SqlParameter("@SelectedStatusId", selectedstatusid);
                        SqlParameter param24 = new SqlParameter("@SelectedReference", selectedsourceid);
                        SqlParameter param25 = new SqlParameter("@WantMailAlerts", ((object)studentmodel.WantMailAlerts) ?? DBNull.Value);

                        SqlParameter param19 = new SqlParameter("@Id", studentmodel.Id);
                        // SqlParameter param4 = new SqlParameter("@stu_Yearofpassedout", studentmodel.Yearofpassedout);
                        // SqlParameter param5 = new SqlParameter("@stu_Qualification", studentmodel.Qualification);
                        //SqlParameter param6 = new SqlParameter("@stu_ProfessionalDetails", studentmodel.ProfessionalDetails);
                        // SqlParameter param8 = new SqlParameter("@stu_Preferredlocation", studentmodel.Preferredlocation);
                        // SqlParameter param9 = new SqlParameter("@stu_Prefbatch", studentmodel.Prefbatch);
                        // SqlParameter param11 = new SqlParameter("@stu_Wanttojoin", studentmodel.Wanttojoin);
                        //SqlParameter param14 = new SqlParameter("@stu_Workexpdomain", studentmodel.Workexpdomain);
                        var data2 = entity.Database.ExecuteSqlCommand("Proc_UpdateLead  @Id,@CompId,@BrId,@LeadName,@Address, @Mobile,@Email,@Product,@Reference,@WantSMSAlerts,@LeadAssignedby,@Status,@Website,@Industry,@Rating,@Comment1,@Comment2,@Comment3,@LeadOwner,@LeadDate,@FollowUpdate,@SelectedProductId,@SelectedStatusId,@SelectedReference,@WantMailAlerts", param19, param20, param21, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14, param15, param16, param17, param18, param22, param23, param24, param25);

                        //entity.Entry(studentmodel);
                        entity.SaveChanges();
                      
                            if (studentmodel.WantSMSAlerts == "Yes")
                            {
                                //for sending an SMS
                                var smscontent = entity.tbl_SmsContent.Where(m => m.InterestedCourse == selectedcourse && m.leadstatus == leadstatus && m.CompId == data.companyid && m.BrId == data.branchid).ToList();
                                string sendcontent = "";
                                if (smscontent != null)
                                {
                                    foreach (var item in smscontent)
                                    {
                                        sendcontent = item.SmsContent;
                                        SendSms(mobilenum, sendcontent);
                                    }
                                }
                            }

                            if (studentmodel.WantMailAlerts == "Yes")
                            {

                                var courseemail = entity.tbl_LeadEmailContent.Where(m => m.InterestedCourse == selectedcourse && m.CompId == data.companyid && m.BrId == data.branchid).ToList();

                                MailModel mailmodel = new MailModel();
                                if (mailmodel != null)
                                {
                                    foreach (var item in courseemail)
                                    {
                                        mailmodel.Subject = item.EmailSubject;
                                        mailmodel.Message = item.EmailBody;
                                    }

                                    //calling sendmail action for send an email
                                    SendMail(mailmodel.Subject, mailmodel.Message, leadmail, leadname, product, leadstatus);

                                }
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
            return RedirectToAction("UnAuthorize", "Error");

        }
        public ActionResult Cancel()
        {
            return RedirectToAction("Index");
        }
        //[CustomExceptionHandlerFilter]
        [CustomAuthorization("Admin,Staff,Manager,Executive,Supervisor")]
        public JsonResult Delete(int? Id)
        {
            try
            {
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

                var data = entity.tbl_EnquiryForm.Where(m => m.Id == Id && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).FirstOrDefault();
                if (Id == null)
                {
                    return Json(data: "Not Deleted", behavior: JsonRequestBehavior.AllowGet);
                }
                entity.Entry(data).State = (System.Data.Entity.EntityState)System.Data.EntityState.Deleted;
                entity.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [CustomAuthorization("Admin,Staff,Manager,Executive,Supervisor")]
        public ActionResult AssignedTo(int Id)
        {
            EnquiryModel s = new EnquiryModel();
            try
            {
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
                var currentuser = Session["CurrentUser"].ToString();

                //dropdown for all users
                var users = entity.tbl_FortuneUsers.Where(m => m.UserName != currentuser && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
                ViewBag.users = users;

                var data = entity.tbl_EnquiryForm.Where(m => m.Id == Id && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).FirstOrDefault();

                s.companyid = data.CompId;
                s.branchid = data.BrId;
                s.Id = data.Id;
                s.LeadName = data.LeadName;
                s.Mobile = data.Mobile;
                s.Email = data.Email;
                // s.Address = data.Address;
                s.selectedinterestedcourse = data.Product;
                s.selectedrefby = data.Reference;
                s.seletedwantsmsalerts = data.WantSMSAlerts;
                s.seletedwantmailalerts = data.WantMailAlerts;
                s.LeadAssignedby = data.LeadAssignedby;
                s.selectedleadassignedto = data.LeadAssignedTo;
                s.selectedstatus = data.Status;
                s.Industry = data.Industry;
                s.Website = data.Website;
                s.Commentsforassignedto = data.CommentsForAssignment;
                //  s.Qualification = data.Qualification;
                // s.YearOfPassedOut = data.Yearofpassedout;
                // s.selectedprofessionaldetails = data.ProfessionalDetails;
                // s.WorkExperienceDomain = data.Workexpdomain;
                // s.selectedpreferredlocation = data.Preferredlocation;
                // s.selectedpreferredbatch = data.Prefbatch;
                // s.selectedwanttojoin = data.Wanttojoin;

            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return View(s);
        }
        [HttpPost]
        [CustomAuthorization("Admin,Staff,Manager,Executive,Supervisor")]
        public ActionResult TaskUpdate(EnquiryModel data, FormCollection f)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var studentmodel = entity.tbl_EnquiryForm.Where(m => m.Id == data.Id && m.CompId == data.companyid && m.BrId == data.branchid).SingleOrDefault();
                    if (studentmodel != null)
                    {
                        studentmodel.CompId = data.companyid;
                        studentmodel.BrId = data.branchid;
                        studentmodel.LeadName = data.LeadName;
                        studentmodel.Mobile = data.Mobile;
                        studentmodel.Email = data.Email;
                        studentmodel.Product = data.selectedinterestedcourse;
                        studentmodel.Reference = data.selectedrefby;
                        studentmodel.WantSMSAlerts = data.seletedwantsmsalerts;
                        studentmodel.WantMailAlerts = data.seletedwantmailalerts;
                        studentmodel.Status = data.selectedstatus;
                        //   studentmodel.Address = data.Address;
                        studentmodel.LeadAssignedby = data.LeadAssignedby;
                        studentmodel.LeadAssignedTo = f["hdnselectedtextofassignedto"];
                        studentmodel.Industry = data.Industry;
                        studentmodel.Website = data.Website;
                        studentmodel.CommentsForAssignment = data.Commentsforassignedto;
                        //  studentmodel.Yearofpassedout = data.YearOfPassedOut;
                        //studentmodel.Qualification = data.Qualification;
                        //studentmodel.ProfessionalDetails = data.selectedprofessionaldetails;
                        // studentmodel.Preferredlocation = data.selectedpreferredlocation;
                        // studentmodel.Prefbatch = data.selectedpreferredbatch;
                        // studentmodel.Wanttojoin = data.selectedwanttojoin;
                        // studentmodel.Workexpdomain = data.WorkExperienceDomain;

                        entity.Entry(studentmodel);
                        entity.SaveChanges();
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
            return RedirectToAction("UnAuthorize", "Error");
        }
        public void SendMail(string body, string message, string leademail, string leadname, string product, string status)
        {
            //var toemail = "ramojireddychalla@gmail.com";
            ////WebMail.SmtpServer = "mail.fortunesofttech.com";

            //WebMail.SmtpPort = 587;
            //WebMail.SmtpUseDefaultCredentials = false;
            //WebMail.EnableSsl = true;
            //WebMail.UserName = "itsupport@fortunesofttech.com";
            //WebMail.Password = "@@hyd123";
            //WebMail.From = "itsupport@fortunesofttech.com";


            //WebMail.Send(toemail, body, message);
            try
            {
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

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
                    entity.tbl_EmailLog.Add(email);
                    entity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
        }
        public void SendSms(string mob, string smscontent)
        {

            try
            {
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

                int count = 0;
                int actualcount;

                DateTime currentdate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                // DateTime currentdate = DateTime.Now;
                var smscount = entity.tbl_FortuneBasicInfo.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();

                foreach (var item in smscount)
                {
                    count = Convert.ToInt32(item.SmsCount);
                }
                //actualcount = entity.tbl_SmsLog.Where(C => C.SentDate==currentdate).ToList().Count();
                actualcount = entity.tbl_SmsLog.Where(m => m.SentDate == currentdate && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).Count();


                if (actualcount <= count)
                {
                    string breakTag = Environment.NewLine;
                    var orgname = entity.tbl_FortuneBasicInfo.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
                    string org = "";
                    foreach (var item in orgname)
                    {
                        org = item.OrganizationName;
                    }
                    SMSCAPI obj = new SMSCAPI();
                    string strPostResponse;
                    // strPostResponse = obj.SendSMS("fortunesoft", "@@hyd123", "9133188819", "hi", "N&DR=Y", "N", "FORTUN");
                    strPostResponse = obj.SendSMS("fortunesoft", "@@hyd123", "91" + mob + "", "Greetings From+" + org + "" + breakTag + smscontent + "", "FORTUN", "N&DR=Y");

                    //log sms details
                    tbl_SmsLog sms = new tbl_SmsLog();
                    sms.LoginUserName = Session["CurrentUser"].ToString();
                    sms.OrganizationName = Session["OrganizationName"].ToString();
                    sms.MobileNum = mob;
                    sms.SentDate = currentdate;
                    entity.tbl_SmsLog.Add(sms);
                    entity.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
        }
        [HttpGet]
        public ActionResult SendCustomEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendCustomEmail(SendCustomEmailModel s, List<HttpPostedFileBase> attachments)
        {
            try
            {
                var leadmail = s.ToEmail;
                var sub = s.Subject;
                var body = s.Compose;
                // SendMail(body, sub, tomail);
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

                var orgbasicdata = entity.tbl_FortuneBasicInfo.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
                string FromEmail = "";
                string Org = "";
                foreach (var item in orgbasicdata)
                {
                    FromEmail = item.OrgEmail;
                    Org = item.OrganizationName;
                }
                var mailbody = sub;
                var mailmessage = body;
                var tomail = leadmail;

                var senderEmail = new MailAddress("info@fortunesofttech.com", Org);
                var receiverEmail = new MailAddress(tomail, "Receiver");
                //string body = "";
                var smtp = new SmtpClient
                {

                    Port = 25,
                    Host = "webmail.fortunesofttech.com",
                    //EnableSsl = true,
                    // DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("info@fortunesofttech.com", "@@hyd123"),
                };
                var mess = new MailMessage(senderEmail, receiverEmail);
                mess.Subject = mailbody;
                mess.Body = mailmessage;
                foreach (HttpPostedFileBase attachment in attachments)
                {
                    if (attachment != null)
                    {
                        string fileName = Path.GetFileName(attachment.FileName);
                        mess.Attachments.Add(new Attachment(attachment.InputStream, fileName));
                    }
                }

                smtp.Send(mess);
                //log email details
                string sendermailid = FromEmail;
                tbl_EmailLog email = new tbl_EmailLog();
                email.LoginUserName = Session["CurrentUser"].ToString();
                email.CompId = CurrentCompanyId;
                email.BrId = CurrentCompanyBranchId;
                email.EmailFrom = sendermailid;
                email.EmailTo = tomail;
                email.LeadName = "Custom";
                email.Product = "Custom";
                email.Status = "Custom";
                email.SentDate = DateTime.Now;
                entity.tbl_EmailLog.Add(email);
                entity.SaveChanges();
            }

            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return RedirectToAction("Index", "Dashboard");
        }
        //public ActionResult Dashboard()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {

        //            var currentuser = Session["CurrentUser"].ToString();

        //            //get all leads
        //            //    var studentdata = entity.tbl_StudentEnquiryForm.ToList();

        //            //get user related leads
        //            var studentdata = entity.tbl_EnquiryForm.Where(m => m.LeadAssignedTo == currentuser || m.LeadAssignedby == currentuser).ToList();
        //            List<EnquiryModel> studentlist = new List<EnquiryModel>();
        //            if (studentdata != null)
        //            {
        //                foreach (var item in studentdata)
        //                {
        //                    EnquiryModel studentmodel = new EnquiryModel();
        //                    studentmodel.Id = item.Id;
        //                    studentmodel.LeadName = item.LeadName;
        //                    studentmodel.Mobile = item.Mobile;
        //                    studentmodel.Email = item.Email;
        //                    studentmodel.Address = item.Address;
        //                    studentmodel.selectedinterestedcourse = item.Product;
        //                    studentmodel.selectedrefby = item.Reference;
        //                    studentmodel.seletedwantalerts = item.Wantalerts;
        //                    studentmodel.selectedstatus = item.Status;
        //                    studentmodel.LeadAssignedby = item.LeadAssignedby;
        //                    studentmodel.selectedleadassignedto = item.LeadAssignedTo;

        //                    studentmodel.Website = item.Website;
        //                    studentmodel.Industry = item.Industry;
        //                    studentmodel.Rating = item.Rating;
        //                    studentmodel.LeadOwner = item.LeadOwner;
        //                    studentlist.Add(studentmodel);
        //                    // studentmodel.selectedwanttojoin = item.Wanttojoin;
        //                    // studentmodel.YearOfPassedOut = item.Yearofpassedout;
        //                    // studentmodel.Qualification = item.Qualification;
        //                    // studentmodel.selectedprofessionaldetails = item.ProfessionalDetails;
        //                    // studentmodel.WorkExperienceDomain = item.Workexpdomain;
        //                    // studentmodel.selectedpreferredlocation = item.Preferredlocation;
        //                    // studentmodel.selectedpreferredbatch = item.Prefbatch;
        //                }

        //                return View(studentlist);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }

        //    }
        //    return RedirectToAction("UnAuthorize", "Error");

        //}

    }

}
