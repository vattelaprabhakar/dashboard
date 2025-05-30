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
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Web.Hosting;
using System.Text;
using FortuneTechPvtLtd.SMSCountry;
using System.Data;
using NLog;

namespace FortuneTechPvtLtd.Controllers
{
    public class UserRegistrationController : Controller
    {
        //
        // GET: /UserRegistration/
        FortuneSoftEntities entity = new FortuneSoftEntities();
        Logger logger = LogManager.GetCurrentClassLogger();

      
        [CustomAuthorization("Admin,Manager")]
        public ActionResult Index()
        {
            List<UserRegModel> userlist = new List<UserRegModel>();
            try
            {
               

                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

                var usercount = entity.tbl_FortuneBasicInfo.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
                foreach (var item in usercount)
                {
                    int a =Convert.ToInt16( item.NumberOfUserToAllowed);
                    if (a!=0)
                    {
                        //Session["NumOfUsersCount"] = a;
                        ViewBag.NumOfUsersCount = a;
                    }
                    //else
                    //{
                    //    Session["NumOfUsersCount"] = null;
                    //}
                }
                int currcount = entity.tbl_FortuneUsers.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).Count();
                if (currcount!=0)
                {
                   // Session["CurrentUserCount"] = currcount;
                    ViewBag.CurrentUserCount = currcount;
                }
                //else
                //{
                //    Session["CurrentUserCount"] = null;
                //}
              

                var data = entity.tbl_FortuneUsers.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();

              
                if (data != null)
                {

                    foreach (var item in data)
                    {
                        UserRegModel userreg = new UserRegModel();
                        userreg.userid = item.UserId;
                        userreg.username = item.UserName;
                        userreg.password = item.Password;
                        userreg.selectedrole = item.RoleName;
                        userreg.email = item.EmailId;
                        userreg.IsActiveName =Convert.ToString( item.IsActiveName);
                        userlist.Add(userreg);
                    }
                 
                }
               
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return View(userlist);
        }
        [HttpGet]
        [CustomAuthorization("Admin,Manager")]
        public ActionResult AddUser()
        {

            
            UserRegModel userreg = new UserRegModel();
            try
            {
                
                //get roles and bind to dropdownlist
                RolseDropdown();
               
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return View(userreg);
        }

        private void RolseDropdown()
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
        
            List<RoleName> roles = new List<RoleName>();
            var getallroles = entity.tbl_FortuneRoles.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).ToList();
            foreach (var item in getallroles)
            {
                RoleName r = new RoleName();
                r.rId = item.RoleId;
                r.rName = item.RoleName;
                roles.Add(r);
            }
            ViewBag.roles = roles;

            //IsUserActive
            List<IsUserAcive> lstuser = new List<IsUserAcive>() { 
                new IsUserAcive(){id=1,name="Active"},
                 new IsUserAcive(){id=2,name="InActive"},
                };
            ViewBag.IsuserActive = lstuser;
        }
        [HttpPost]
        public ActionResult AddUser(UserRegModel regmodel,FormCollection f)
        {
            try
            {
                bool userExists = entity.tbl_FortuneUsers.FirstOrDefault(x => x.EmailId == regmodel.email) != null;
                if (userExists)
                {
                    //ModelState.AddModelError("UserName", "");
                    //return RedirectToAction("Index");
                    return Content("<script language='javascript' type='text/javascript'>alert('UserName Already Exists Try Another one');window.location.href='Index'</script>");
                }
                else
                { 
                    
                    tbl_FortuneUsers user = new tbl_FortuneUsers();
                    user.CompId = regmodel.companyid;
                    user.BrId = regmodel.branchid;
                    user.UserName = regmodel.username;
                    user.Password = regmodel.password;
                    user.RoleName = f["hdnselectedrole"];
                    user.RoleId = Convert.ToInt16(regmodel.selectedrole);
                    user.EmailId = regmodel.email;
                    user.IsActiveId =Convert.ToInt32( regmodel.IsActiveName);
                    user.IsActiveName = f["hdnactivestatus"];
                    user.Record_CreatedBy = Session["CurrentUser"].ToString();
                    var useremail = regmodel.email;
                    SqlParameter param1 = new SqlParameter("@UserName", user.UserName);
                    SqlParameter param2 = new SqlParameter("@Password", user.Password);
                    SqlParameter param3 = new SqlParameter("@RoleName", user.RoleName);
                    SqlParameter param4 = new SqlParameter("@EmailId", user.EmailId);
                    SqlParameter param5 = new SqlParameter("@Record_CreatedBy", user.Record_CreatedBy);
                    SqlParameter param6 = new SqlParameter("@RoleId", user.RoleId);
                    SqlParameter param7 = new SqlParameter("@CompId", user.CompId);
                    SqlParameter param8 = new SqlParameter("@BrId", user.BrId);

                    SqlParameter param9 = new SqlParameter("@IsActiveId", user.IsActiveId);
                    SqlParameter param10 = new SqlParameter("@IsActiveName", user.IsActiveName);

                   // entity.tbl_FortuneUsers.Add(user);
                    var data1 = entity.Database.ExecuteSqlCommand("Proc_AddUser @CompId,@BrId,@IsActiveId,@UserName,@RoleId,@IsActiveName,@Password, @RoleName,@EmailId,@Record_CreatedBy", param7, param8, param9, param1, param6, param10, param2, param3, param4, param5);
                    entity.SaveChanges();

                    

                    //email testing
                    string breakTag = Environment.NewLine;
                    string body = "Successfully Created Your Login Credentials with " + Session["OrganizationName"].ToString() + ".....!!" + breakTag +
                                    "UserName :" + user.UserName + breakTag +
                                    "Password :" + user.Password + breakTag;

                    var subject = "Greetings From " + Session["OrganizationName"].ToString() + "...! Your Login Credentials Has been Created Successfully";
                    SendMail(useremail, body, subject);


                    return Content("<script language='javascript' type='text/javascript'>alert('User Created Completed Successfully');window.location.href='Index'</script>");
                    //  SendSms();
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
        public ActionResult Edit(int id,FormCollection f)
        {
            UserRegModel userregmodel = new UserRegModel();
            try
            {
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
                var data = entity.tbl_FortuneUsers.Where(m => m.UserId == id &&m.CompId==CurrentCompanyId && m.BrId==CurrentCompanyBranchId).FirstOrDefault();
                RolseDropdown();
               
                userregmodel.userid = data.UserId;
                userregmodel.companyid = data.CompId;
                userregmodel.branchid = data.BrId;
                userregmodel.username = data.UserName;
                userregmodel.password = data.Password;
                userregmodel.selectedrole =Convert.ToString( data.RoleId);
                userregmodel.email = data.EmailId;
                userregmodel.IsActiveName = data.IsActiveName;
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }

            return View(userregmodel);
        }
        [HttpPost]
        public ActionResult Update(UserRegModel reg,FormCollection f)
        {
             bool userExists = entity.tbl_FortuneUsers.FirstOrDefault(x => x.EmailId == reg.email) != null;
             if (userExists)
             {
                 var data = entity.tbl_FortuneUsers.Where(m => m.UserId == reg.userid && m.CompId == reg.companyid && m.BrId == reg.branchid).SingleOrDefault();
                 var uname = data.UserName;
                 var password = data.Password;
                 var rolename = data.RoleName;
                 var email = data.EmailId;
                 if (data != null)
                 {
                     try
                     {
                         int count = entity.tbl_FortuneUsers.Where(x => x.UserName == reg.username).Count();
                         data.UserName = reg.username;
                         data.Password = reg.password;
                         data.RoleName = f["hdnselectedrole"];
                         data.IsActiveName = f["hdnactivestatus"];
                         data.IsActiveId =Convert.ToInt32( reg.IsActiveName);
                         data.RoleId = Convert.ToInt16(reg.selectedrole);
                         data.EmailId = reg.email;
                         data.Record_ModifiedBy = Session["CurrentUser"].ToString();
                         SqlParameter param1 = new SqlParameter("@UserName", data.UserName);
                         SqlParameter param2 = new SqlParameter("@Password", data.Password);
                         SqlParameter param3 = new SqlParameter("@RoleName", data.RoleName);
                         SqlParameter param4 = new SqlParameter("@EmailId", data.EmailId);
                         SqlParameter param5 = new SqlParameter("@Record_ModifiedBy", data.Record_ModifiedBy);
                         SqlParameter param6 = new SqlParameter("@UserId", data.UserId);
                         SqlParameter param7 = new SqlParameter("@RoleId", data.RoleId);
                         SqlParameter param8 = new SqlParameter("@CompId", data.CompId);
                         SqlParameter param9 = new SqlParameter("@BrId", data.BrId);

                         SqlParameter param10 = new SqlParameter("@IsActiveId", data.IsActiveId);
                         SqlParameter param11 = new SqlParameter("@IsActiveName", data.IsActiveName);
                         //entity.tbl_FortuneUsers.Add(user);
                         var data1 = entity.Database.ExecuteSqlCommand("Proc_UpdateUser @CompId,@BrId, @UserId, @UserName,@RoleId,@Password, @RoleName,@EmailId,@Record_ModifiedBy,@IsActiveId,@IsActiveName", param8, param9, param6, param1, param7, param2, param3, param4, param5,param10,param11);
                         // entity.Entry(data);
                         entity.SaveChanges();

                         if (uname != reg.username || password != reg.password || rolename != f["hdnselectedrole"] || email != reg.email)
                         {//sending email while username or password has been changed
                             string breakTag = Environment.NewLine;
                             string body = "Greetings From " + Session["OrganizationName"].ToString() + "...!Successfully Updated Your UserName or Password." + breakTag +
                                             "UserName :" + reg.username + breakTag +
                                             "Password :" + reg.password + breakTag;
                             var sub = "Login Credentials Are Successfully Updated...!";
                             SendMail(reg.email, body, sub);

                             return Content("<script language='javascript' type='text/javascript'>alert('User Credentials Updated Successfully');window.location.href='Index'</script>");
                         }
                     }


                     catch (Exception ex)
                     {
                         logger.ErrorException("error occurred at", ex);
                     }
                 }
             }
            return RedirectToAction("Index");
        }
        [HttpPost]
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

                var data = entity.tbl_FortuneUsers.Where(m => m.UserId == id && m.CompId==CurrentCompanyId && m.BrId==CurrentCompanyBranchId).FirstOrDefault();
                if (data!=null)
                {
                    entity.Entry(data).State = (System.Data.Entity.EntityState)System.Data.EntityState.Deleted;
                    entity.SaveChanges();
                }
              
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return RedirectToAction("Index");
        }
        public void SendMail(string tomailaddress, string body, string subject)
        {
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

                MailModel mailmodel = new MailModel();
                var senderEmail = new MailAddress(FromEmail, Org);
                var receiverEmail = new MailAddress(tomailaddress, "Receiver");
                string composeemail = body;
                string sub = subject;
                var smtp = new SmtpClient
                {

                    Port = 25,
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
                    string sendermailid = FromEmail;
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
    }
}
