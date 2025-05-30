using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FortuneTechPvtLtd.Models
{
    public class StatusModel
    {
        public int statusid { get; set; }
        public string statusname { get; set; }
        public int? companyid { get; set; }
        public int? branchid { get; set; }
    }
}