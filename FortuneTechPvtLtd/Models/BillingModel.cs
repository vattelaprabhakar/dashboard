using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FortuneTechPvtLtd.Models
{
    public class BillingModel
    {
        public int custid { get; set; }
        public string custname { get; set; }
        public string productname { get; set; }
        public decimal? productactualprice { get; set; }
        public decimal? productrevisedprice { get; set; }
        public DateTime? paymentdate { get; set; }
        public string paymentmode { get; set; }
        public decimal? payamout { get; set; }
        public decimal? balanceamount { get; set; }
        public DateTime? followupdate { get; set; }
        public int? companyid { get; set; }
        public int? branchid { get; set; }
        public string mobile { get; set; }
        public string Comments { get; set; }
        //payment mode
       // public PaymentMode PaymentMode { get; set; }
    }
   public    class paymenttype
    {
        public int paymentid { get; set; }
        public string paymentmode { get; set; }
    }
    //public enum PaymentMode
    //{
    //    Cash,
    //    DebitCard,
    //    CreditCard,
    //    Other
    //}
}