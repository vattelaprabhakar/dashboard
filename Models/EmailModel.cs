using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FortuneTechPvtLtd.Models
{
    public class EmailModel
    {
        public int emailid { get; set; }
        public string emailsubject { get; set; }
        public string emailbody { get; set; }
        public string interestedcourse { get; set; }
        public string leadstutus { get; set; }
        public int? companyid { get; set; }
        public int? branchid { get; set; }
    }
}