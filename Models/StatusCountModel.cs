using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace FortuneTechPvtLtd.Models
{
    public class StatusCountModel
    {
        public int Count { get; set; }
        public string StatusName { get; set; }
        public int companyid { get; set; }
        public int branchid { get; set; }
    }
    public class SourceCountModel
    {
        public int Count { get; set; }
        public string SourceName { get; set; }
        public int companyid { get; set; }
        public int branchid { get; set; }
    }

    public class ProductCountModel
    {
        public int Count { get; set; }
        public string ProductName { get; set; }
        public int companyid { get; set; }
        public int branchid { get; set; }
    }
    
}