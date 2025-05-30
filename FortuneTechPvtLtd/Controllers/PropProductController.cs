using FortuneTechPvtLtd.DataModel;
using FortuneTechPvtLtd.Models;
using NLog;
using RoleBasedSecurity.CustomSecurity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FortuneTechPvtLtd.Controllers
{
    public class PropProductController : Controller
    {
        //
        // GET: /PropProduct/
        FortuneSoftEntities entity = new FortuneSoftEntities();
        Logger logger = LogManager.GetCurrentClassLogger();

        //[CustomExceptionHandlerFilter]
        [CustomAuthorization("Admin,Manager")]
        public ActionResult Index()
        {
            int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
            int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());
            List<ProductModel> productmodel = new List<ProductModel>();
            try
            {

                var products = entity.tbl_Productlist.Where(m=>m.CompId==CurrentCompanyId && m.BrId==CurrentCompanyBranchId).ToList();
                foreach (var item in products)
                {
                    ProductModel r = new ProductModel();
                    r.productid = item.ProductId;
                    r.productname = item.ProductName;
                    r.productcost = item.ProductAmount;
                    productmodel.Add(r);
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
        public ActionResult AddProduct()
        {
            ProductModel r = new ProductModel();
            return View(r);
        }
        [HttpPost]
        public ActionResult Submit(ProductModel model)
        {
            try
            {
                tbl_Productlist tblprodcuts = new tbl_Productlist();
                tblprodcuts.CompId = model.companyid;
                tblprodcuts.BrId = model.branchid;
                tblprodcuts.ProductName = model.productname;
                tblprodcuts.ProductAmount = model.productcost;
                entity.tbl_Productlist.Add(tblprodcuts);
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
            ProductModel r = new ProductModel();
            try
            {
                int CurrentCompanyId = Convert.ToInt32(Session["CurrentCompanyId"].ToString());
                int CurrentCompanyBranchId = Convert.ToInt32(Session["CurrentCompanyBranchId"].ToString());

                var empdatabyid = entity.tbl_Productlist.Where(m => m.ProductId == id && m.CompId==CurrentCompanyId && m.BrId==CurrentCompanyBranchId).FirstOrDefault();

                r.productid = empdatabyid.ProductId;
                r.companyid = empdatabyid.CompId;
                r.branchid = empdatabyid.BrId;
                r.productname = empdatabyid.ProductName;
                r.productcost = empdatabyid.ProductAmount;
            }
            catch (Exception ex)
            {
                logger.ErrorException("error occurred at", ex);
            }
            return View(r);
        }
        [HttpPost]
        public ActionResult Update(ProductModel s)
        {
            
            var data = entity.tbl_Productlist.Where(m => m.ProductId == s.productid && m.CompId == s.companyid && m.BrId == s.branchid).SingleOrDefault();

            if (data != null)
            {
                try
                {
                   
                    data.ProductName = s.productname;
                    data.ProductAmount = s.productcost;
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
                var data = entity.tbl_Productlist.Where(m => m.ProductId == id && m.CompId == CurrentCompanyId && m.BrId == CurrentCompanyBranchId).FirstOrDefault();
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
