using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FortuneTechPvtLtd.Models
{
    public class MenuModel
    {
        public string MenuName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public int companyid { get; set; }
        public int branchid { get; set; }
    }
}