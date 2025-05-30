using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FortuneTechPvtLtd.Models
{
    public class CompanyBasicDataModel
    {
        public int orgid { get; set; }
        public string organizationname { get; set; }
        public string logopath { get; set; }
        public string organizationemail { get; set; }
        public string organizationphone { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public int? smscount { get; set; }
        public int companyid { get; set; }
        public int branchid { get; set; }
        public int CompBasicId { get; set; }
        public int? NumOfUserToAllow { get; set; }
        public byte[] OrganizationLogo { get; set; }
        public string smsflag { get; set; }
        public string emailflag { get; set; }
        public string analyticsflag { get; set; }
    }
}