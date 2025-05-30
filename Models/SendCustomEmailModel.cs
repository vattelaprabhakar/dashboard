using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FortuneTechPvtLtd.Models
{
    public class SendCustomEmailModel
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Compose { get; set; }
        public int companyid { get; set; }
        public int branchid { get; set; }
    }
}