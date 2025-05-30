using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FortuneTechPvtLtd.Models
{
    public class EmailAlertsModel
    {
        public int AlertId { get; set; }
        public int AlertMasterId { get; set; }
        public string AlertMasterName { get; set; }
        public string AlertCategory { get; set; }
        public string AlertType { get; set; }
        public string AlertComments { get; set; }
        public int CompId { get; set; }
        public int BrId { get; set; }
        public string AlertFlag { get; set; }
    }
    public class AlertMasterName
    {
        public int AlertMastId { get; set; }
        public string AlertMastName { get; set; }
        public bool isselected { get; set; }
      
    }
    class EmailAlertStatus
    {
        public int alertstatusid { get; set; }
        public string alertstatusname { get; set; }
    }
}