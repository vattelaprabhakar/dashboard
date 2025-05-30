using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FortuneTechPvtLtd.Models
{
    public class ProductModel
    {
        public int? companyid { get; set; }
        public int? branchid { get; set; }
        public int productid { get; set; }
        public string productname { get; set; }
        public decimal? productcost { get; set; }
    }
}