using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FortuneTechPvtLtd.Models
{
    public class ReportsModel
    {
        public int companyid { get; set; }
        public int branchid { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string reportparameter { get; set; }

        public string LeadName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string selectedinterestedcourse { get; set; }
        public string selectedrefby { get; set; }
        public string seletedwantalerts { get; set; }
        public string LeadAssignedby { get; set; }
        public string selectedstatus { get; set; }
        public string selectedleadassignedto { get; set; }
        public string Website { get; set; }
        public string Industry { get; set; }
        public string Rating { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string Comment3 { get; set; }
        public string LeadOwner { get; set; }
        public string LeadAssignedTo { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class Reportsparameters
    {
        public int ParaId { get; set; }
        public string ParaName { get; set; }
        public bool isselected { get; set; }
    }
}