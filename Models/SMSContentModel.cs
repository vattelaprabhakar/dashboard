using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FortuneTechPvtLtd.Models
{
    public class SMSContentModel
    {
        public int smsid { get; set; }
        public string smscontent { get; set; }
        public string interestedcourse { get; set; }
        public string leadstutus { get; set; }
        public int companyid { get; set; }
        public int branchid { get; set; }
    }
}