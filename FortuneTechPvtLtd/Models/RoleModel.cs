using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FortuneTechPvtLtd.Models
{
    public class RoleModel
    {
        public int roleid { get; set; }
        [Required(ErrorMessage="Please Enter Role Name")]
        public string rolename { get; set; }
        public int? companyid { get; set; }
        public int? branchid { get; set; }
    }
}