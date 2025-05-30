using FortuneTechPvtLtd.DataModel;
using FortuneTechPvtLtd.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FortuneTechPvtLtd.Controllers
{
    public class BillingController : Controller
    {
        //
        // GET: /Billing/
        FortuneSoftEntities entity = new FortuneSoftEntities();
        Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetAllPaymentDetails()
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

            var studentdata = entity.tbl_Payment.Where(m => m.compid == CurrentCompanyId && m.brid == CurrentCompanyBranchId).OrderByDescending(m => m.cust_id).ToList();
            List<BillingModel> data = new List<BillingModel>();

            foreach (var item in studentdata)
            {
                BillingModel studentmodel = new BillingModel();
                studentmodel.custid = item.cust_id;
                studentmodel.custname = item.cust_name;
                studentmodel.productname = item.product_name;
                studentmodel.mobile = item.cust_mobile;
                studentmodel.productactualprice = item.product_actual_cost;
                studentmodel.productrevisedprice = item.product_revised_amount;
                studentmodel.paymentdate = item.payment_date;
                studentmodel.paymentmode = item.modeof_payment;
                studentmodel.payamout = item.paid_amount;
                studentmodel.balanceamount = item.balance_amount;
                studentmodel.followupdate = item.payment_followupdate == null ? (DateTime?)null : Convert.ToDateTime(item.payment_followupdate);
                studentmodel.Comments = item.comments;
                studentmodel.companyid = item.compid;
                studentmodel.branchid = item.brid;
                data.Add(studentmodel);

            }

            return Json(data, JsonRequestBehavior.AllowGet);


            //SqlParameter param1 = new SqlParameter("@LeadName", studentmodel.LeadName);
            //SqlParameter param2 = new SqlParameter("@Address", studentmodel.Address);

        }
      
        public JsonResult Delete(int? Id)
        {
            try
            {
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

                var data = entity.tbl_Payment.Where(m => m.cust_id == Id && m.compid == CurrentCompanyId && m.brid == CurrentCompanyBranchId).FirstOrDefault();
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
        public JsonResult GetCustomerNames(string Prefix)
        {
            string cus = "Customer";
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

            var getcustomers = (from c in entity.tbl_EnquiryForm
                                 where c.CompId == CurrentCompanyId && c.BrId == CurrentCompanyBranchId && c.Status.ToUpper()==cus.ToUpper()
                                 where c.LeadName.StartsWith(Prefix)
                                select new { username = c.LeadName + "-" + c.Mobile+"-"+c.Product});
            return Json(getcustomers, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Billing()
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
            //payment mode
            List<paymenttype> paymenttype = new List<paymenttype>() { 
            new paymenttype(){paymentid=1,paymentmode="Cash"},
             new paymenttype(){paymentid=2,paymentmode="Debit Card"},
              new paymenttype(){paymentid=3,paymentmode="Credit Card"},
                new paymenttype(){paymentid=4,paymentmode="Cheque"},
               new paymenttype(){paymentid=5,paymentmode="Other"},
            };

            ViewBag.paymenttype = paymenttype;
             return View();
        }
        public ActionResult Edit(int? Id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                    int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

                    var data = entity.tbl_Payment.Where(m => m.cust_id == Id && m.compid == CurrentCompanyId && m.brid == CurrentCompanyBranchId).FirstOrDefault();

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
                    //payment mode
                    List<paymenttype> paymenttype = new List<paymenttype>() { 
           new paymenttype(){paymentid=1,paymentmode="Cash"},
             new paymenttype(){paymentid=2,paymentmode="Debit Card"},
              new paymenttype(){paymentid=3,paymentmode="Credit Card"},
                new paymenttype(){paymentid=4,paymentmode="Cheque"},
               new paymenttype(){paymentid=5,paymentmode="Other"},
            };

                    ViewBag.paymenttype = paymenttype;

                    //radiobutton property values for radiobutton generation
                    if (data!=null)
                    {
                        BillingModel s = new BillingModel();
                        s.custid = data.cust_id;
                        s.companyid = data.compid;
                        s.branchid = data.brid;
                        s.custid = data.cust_id;
                        s.custname = data.cust_name + "-" + data.cust_mobile + "-" + data.product_name;
                        s.mobile = data.cust_mobile;
                        s.productactualprice = data.product_actual_cost;
                        s.productrevisedprice = data.product_revised_amount;
                        s.balanceamount = data.balance_amount;
                        s.payamout = data.paid_amount;
                        s.paymentmode = data.modeof_payment;
                        s.followupdate = data.payment_followupdate;
                        s.paymentdate = data.payment_date;
                        s.Comments = data.comments;
                        return View(s);
                    }
                  
                }
                catch (Exception ex)
                {
                    logger.ErrorException("error occurred at", ex);
                }
            }

            return RedirectToAction("UnAuthorize", "Error");
        }
        public ActionResult Cancel()
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Update(BillingModel m, FormCollection f)
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            //var data = entity.tbl_Productlist.Where(v => m.ProductId == s.productid && m.CompId == s.companyid && m.BrId == s.branchid).SingleOrDefault();
            var data = entity.tbl_Payment.Where(obj => obj.cust_id == m.custid && obj.compid == m.companyid && obj.brid == m.branchid).SingleOrDefault();

              if (data != null)
              {
                  try
                  {
                      string msg = m.custname;
                      string[] strarr = msg.Split('-');
                      string mobile = "";
                      string cuname = "";
                      string product = "";
                      if (!string.IsNullOrEmpty(msg))
                      {
                          for (int i = 0; i < strarr.Length; i++)
                          {
                              cuname = strarr[0];
                              mobile = strarr[1];
                              product = strarr[2];
                          }
                      }
                   
                    //  tbl_Payment payment = new tbl_Payment();
                      //payment.cust_id = m.custid;
                      data.cust_name = cuname;
                      data.cust_mobile = mobile;
                      data.product_name = product;
                      data.product_actual_cost = m.productactualprice;
                      data.product_revised_amount = m.productrevisedprice;
                      data.balance_amount = m.balanceamount;
                      data.modeof_payment = f["hdnpaymentmode"];
                      data.modeofpayment_id = Convert.ToInt32(m.paymentmode);
                      data.paid_amount = m.payamout;
                      data.payment_date = m.paymentdate == null ? (DateTime?)null : Convert.ToDateTime(m.paymentdate);
                      data.payment_followupdate = m.followupdate == null ? (DateTime?)null : Convert.ToDateTime(m.followupdate);
                      data.compid = CurrentCompanyId;
                      data.brid = CurrentCompanyBranchId;
                      data.comments = m.Comments;
                      entity.Entry(data);
                      entity.SaveChanges();
                  }
                  catch (Exception ex)
                  {
                      logger.ErrorException("error occurred at", ex);
                  }
              }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Billing(BillingModel m,FormCollection f)
        {
            string msg = m.custname;

            string[] strarr = msg.Split('-');
            string mobile = "";
            string cuname = "";
            string product = "";
            if (!string.IsNullOrEmpty(msg))
            {
                for (int i = 0; i < strarr.Length; i++)
                {
                    cuname = strarr[0];
                    mobile = strarr[1];
                    product = strarr[2];
                }
            }
           
             int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

            tbl_Payment payment = new tbl_Payment();
            payment.cust_name = cuname;
            payment.cust_mobile = mobile;
            payment.product_name = product;
            
            payment.product_actual_cost = m.productactualprice;
            payment.product_revised_amount = m.productrevisedprice;
            payment.balance_amount = m.balanceamount;
            payment.modeof_payment = f["hdnpaymentmode"];
            payment.modeofpayment_id =Convert.ToInt32( m.paymentmode);
            payment.paid_amount = m.payamout;
            payment.payment_date = m.paymentdate == null ? (DateTime?)null : Convert.ToDateTime(m.paymentdate);
            payment.payment_followupdate = m.followupdate == null ? (DateTime?)null : Convert.ToDateTime(m.followupdate);
            payment.compid = CurrentCompanyId;
            payment.brid = CurrentCompanyBranchId;
            payment.comments = m.Comments;
            entity.tbl_Payment.Add(payment);
            entity.SaveChanges();
            return RedirectToAction("Index");
        }
       
      
        public JsonResult GetCostofProduct(string productname)
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            //product
            var products = entity.tbl_Productlist.Where(m => m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId && m.ProductName==productname).ToList();
            string data = "";
            foreach (var item in products)
            {
                data =Convert.ToString( item.ProductAmount);
            }
           // data = "1000";
            return Json(data,JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerList()
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

            var cust = from customer in entity.tbl_EnquiryForm
                            where customer.Status=="Customer" && customer.BrId==CurrentCompanyBranchId && customer.CompId==CurrentCompanyId
                            select new
                            {
                                Employee_Id = customer.Id,
                                user_name = customer.Mobile + "-" + customer.LeadName

                            };

            return Json(cust, JsonRequestBehavior.AllowGet);
        }

    }
}
