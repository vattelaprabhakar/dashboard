using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FortuneTechPvtLtd.Models;
using System.Web.Security;
using FortuneTechPvtLtd.DataModel;
using RoleBasedSecurity.CustomSecurity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Configuration;
using NLog;
using System.Net.Mail;
using System.Net;


namespace FortuneTechPvtLtd.Controllers
{
    public class homeController : Controller
    {
        //
        // GET: /home/
        Logger logger = LogManager.GetCurrentClassLogger();
        FortuneSoftEntities entity = new FortuneSoftEntities();

        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult ForgetPassword()
        {

            return View();

        }
        [HttpPost]
        public ActionResult ForgetPassword(UserLoginModel u)
        {
            string usermailid = u.username;
            var emailid = entity.tbl_FortuneUsers.Where(m => m.EmailId.ToUpper() == usermailid.ToUpper()).FirstOrDefault();
            if (emailid != null)
            {
                Session["UserEmailIdForUpdate"] = usermailid;
                Random r = new Random();
                int randNum = r.Next(1, 10000);
                Session["OTP"] = randNum;
                string fourdigitnum = randNum.ToString("D4");
                string subject = "Send OTP For set New Password";//heading
                SendMail(usermailid, fourdigitnum, subject);
                return RedirectToAction("SetNewPassword");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Mailid Doesnot Exist');window.location.href='ForgetPassword'</script>");
            }


        }
        public ActionResult SetNewPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SetNewPassword(UserLoginModel u)
        {
            string user_entered_otp = u.OTP;
            string user_recieved_otp = Session["OTP"].ToString();
            if (user_recieved_otp != user_entered_otp)
            {
                //send an alert popup otp doesnot match
                return Content("<script language='javascript' type='text/javascript'>alert('Otp Doesnot Match');window.location.href='SetNewPassword'</script>");
            }
            if (u.SetPassword.ToUpper() != u.ConfirmPassword.ToUpper())
            {
                //send an alert popup passwords doesnot match
                return Content("<script language='javascript' type='text/javascript'>alert('Password Doesnot Match');window.location.href='SetNewPassword'</script>");
            }
            string usermail = Session["UserEmailIdForUpdate"].ToString();
            var data = entity.tbl_FortuneUsers.Where(m => m.EmailId == usermail).SingleOrDefault();
            if (data != null)
            {
                data.Password = u.SetPassword;
                entity.Entry(data);
                entity.SaveChanges();
                //send againg mail for remembering password updation
                string breakTag = Environment.NewLine;
                string body = "Successfully Updated Your UserName or Password." + breakTag +
                                "UserName :" + u.username + breakTag +
                                "Password :" + u.SetPassword + breakTag;
                var sub = "Login Credentials Are Successfully Updated...!";
                SendMail(data.EmailId, body, sub);
                //alert popup for setting new password is ...
                return Content("<script language='javascript' type='text/javascript'>alert('Password Updated Successfully');window.location.href='Cancel'</script>");
            }
            else
            {
                return RedirectToAction("SetNewPassword");
            }
        }
        public void SendMail(string tomailaddress, string body, string subject)
        {
            try
            {
                // int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                // int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
                //var orgbasicdata = entity.tbl_FortuneBasicInfo.ToList();
                //string FromEmail = "";
                //string Org = "";
                //foreach (var item in orgbasicdata)
                //{
                //    FromEmail = item.OrgEmail;
                //    Org = item.OrganizationName;
                //}
                string frommailid = "info.fortunesofttech@gmail.com";
                MailModel mailmodel = new MailModel();
                var senderEmail = new MailAddress(frommailid, "Fortune CRM");
                var receiverEmail = new MailAddress(tomailaddress, "Receiver");
                string composeemail = body;
                string sub = subject;
                var smtp = new SmtpClient
                {

                    Port = 587,//25
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("info.fortunesofttech@gmail.com", "!!hyderabad123")

                };

                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = sub,
                    Body = composeemail
                })
                {
                    smtp.Send(mess);
                    //log email details
                    string sendermailid = frommailid;
                    tbl_EmailLog email = new tbl_EmailLog();
                    email.LoginUserName = Session["CurrentUser"].ToString();
                    email.EmailFrom = sendermailid;
                    email.EmailTo = tomailaddress;
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

        public ActionResult Cancel()
        {
            return RedirectToAction("Index");
        }
        public ActionResult GetPdf()
        {//need to get file path from db
            string path = AppDomain.CurrentDomain.BaseDirectory + "ProjectContent/";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + "CRMGuideInfo.pdf");
            string fileName = "CRMGuideInfo.pdf";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public ActionResult DownloadApp()
        {//need to get file path from db
            string path = AppDomain.CurrentDomain.BaseDirectory + "ProjectContent/";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + "app-debug.apk");
            string fileName = "app-debug.apk";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        [HttpPost]
        public ActionResult Index(UserLoginModel userlogin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = entity.tbl_FortuneUsers.Where(u => u.EmailId.ToUpper() == userlogin.username.ToUpper() && u.Password.ToUpper() == userlogin.password.ToUpper() && u.IsActiveId == 1).FirstOrDefault();//&& u.CompId==userlogin.companyid && u.BrId==userlogin.branchid
                    List<UserRegModel> userdata = new List<UserRegModel>();
                    if (user != null)
                    {
                        UserRegModel uregmodel = new UserRegModel();
                        uregmodel.username = user.UserName;
                        uregmodel.password = user.Password;
                        uregmodel.companyid = user.CompId;
                        uregmodel.branchid = user.BrId;
                        var currentuserid = user.EmailId;
                        Session["currentuserid"] = user.UserId;
                        //role name
                        RoleName rolename = new RoleName();
                        rolename.rName = user.RoleName;
                        userdata.Add(uregmodel);
                        if (userdata != null)
                        {
                            //create forms authentication cookie for persisting user datails
                            //string userData = string.Format("{0}|{1}|{2}{3}{4}", uregmodel.username, uregmodel.password, user.RoleName,uregmodel.companyid,uregmodel.branchid);
                            //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, uregmodel.username, DateTime.Now, DateTime.Now.AddMinutes(300), false, userData);
                            ////encrypt auth cookie
                            //string EncAuthCookie = FormsAuthentication.Encrypt(ticket);
                            ////set auth cookie 
                            //HttpCookie userdetcookie = new HttpCookie(FormsAuthentication.FormsCookieName, EncAuthCookie);
                            //Response.Cookies.Add(userdetcookie);

                            Session["CurrentUser"] = uregmodel.username;
                            Session["CurrentCompanyId"] = uregmodel.companyid;
                            Session["CurrentCompanyBranchId"] = uregmodel.branchid;
                            Session["CurrentLoginUser"] = currentuserid;
                            // Session["Role"] = user.RoleName;
                            //getting menu based on rolename

                            List<MenuModel> menus = entity.tbl_Menu.Where(x => x.RoleName == user.RoleName && x.CompId == uregmodel.companyid && x.BrId == uregmodel.branchid).Select(x => new MenuModel
                            {
                                MenuName = x.Text,
                                ControllerName = x.ControllerName,
                                ActionName = x.ActionName,
                                RoleId = x.RoleId,
                                RoleName = x.RoleName

                            }).ToList();
                            Session["MenuMaster"] = menus; //Bind the _menus list to MenuMaster session  
                            Session["UserRole"] = user.RoleName;

                            //organization details
                            var org = entity.tbl_FortuneBasicInfo.Where(x => x.CompId == uregmodel.companyid && x.BrId == uregmodel.branchid).ToList();
                            if (org != null)
                            {
                                List<OrganizationModel> orgdetails = new List<OrganizationModel>();
                                foreach (var item in org)
                                {
                                    OrganizationModel o = new OrganizationModel();
                                    o.OrganizationName = item.OrganizationName;
                                    o.LogoUrl = item.LogoPath;
                                    orgdetails.Add(o);
                                    Session["OrganizationName"] = o.OrganizationName;
                                    if (item.email_flag != null)
                                    {
                                        Session["emailflag"] = item.email_flag;
                                    }
                                    if (item.sms_flag != null)
                                    {
                                        Session["smsflag"] = item.sms_flag;
                                    }
                                    if (item.analytics_flag != null)
                                    {
                                        Session["analyticsflag"] = item.analytics_flag;
                                    }
                                    if (item.email_alert != null)
                                    {
                                        Session["emailalert"] = item.email_alert;
                                    }
                                    if (item.sms_alert != null)
                                    {
                                        Session["smsalert"] = item.sms_alert;
                                    }
                                    //if (item.email_flag == null || item.sms_flag == null || item.analytics_flag == null)
                                    //{
                                    //    TempData["notregisterd"] = "notregisterd";
                                    //     return View();
                                    //}
                                    
                                }
                                Session["organizationdetails"] = orgdetails;
                            }
                            DateTime currentdate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            var secdate = entity.tbl_FortuneBasicInfo.Where(m => m.CompId == uregmodel.companyid).ToList();
                            if (secdate != null)
                            {
                                DateTime d;
                                foreach (var item in secdate)
                                {
                                    d = (DateTime)item.ToDate;
                                    if (d <= currentdate || d>= currentdate)
                                    {
                                        return RedirectToAction("Index", "Dashboard");
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        TempData["notvalid"] = "notvaliduser";
                        return View();
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
            }

            return View();
        }
        //public ActionResult Error()
        //{
        //    return View();
        //}

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Index", "home");
        }


    }
}
