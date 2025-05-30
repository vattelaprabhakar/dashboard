using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FortuneTechPvtLtd.Models
{
    public class MailSchedulingModel
    {
        public int campid { get; set; }
        public string campname { get; set; }
        public DateTime? campstartdate { get; set; }
        public DateTime? campenddate { get; set; }
        public string campcategory { get; set; }
        public string camptype { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan? camptime { get; set; }
        public string campday { get; set; }
        public string campproduct { get; set; }
        public string campstatus { get; set; }
        public string campflag { get; set; }
        public int companyid { get; set; }
        public int branchid { get; set; }
    }
    class CampaignType
    {
        public int camptypeid { get; set; }
        public string camptypename { get; set; }

    }
    class CampaignCategory
    {
        public int campcategoryid { get; set; }
        public string campcategoryname { get; set; }
    }
    class CampaignDay
    {
        public int campdayid { get; set; }
        public string campdayname { get; set; }
    }
    class CampaignStatus
    {
        public int campstatusid { get; set; }
        public string campstatusname { get; set; }
    }
}