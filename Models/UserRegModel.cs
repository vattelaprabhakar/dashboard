using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FortuneTechPvtLtd.Models
{
    public class UserRegModel
    {
        public int userid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        [Required(ErrorMessage="Please Enter User Name")]
        public string username { get; set; }
         [Required(ErrorMessage = "Please Enter Password")]
        public string password { get; set; }
        public List<RoleName> role { get; set; }
         [Required(ErrorMessage = "Please Enter RoleName")]
        public string selectedrole { get; set; }
        //public string selectedrolename { get; set; }
         [Required(ErrorMessage = "Please Enter Email")]
        public string email { get; set; }
         public int? companyid { get; set; }
         public int? branchid { get; set; }

         public int? IsActiveid { get; set; }
         public string IsActiveName { get; set; }
         public IsUserAcive IsUserActive { get; set; }
    }

    public class IsUserAcive
    {
        public int id { get; set; }
        public string name { get; set; }
        
    }
    public class RoleName
    {
        public int rId { get; set; }
        public string rName { get; set; }
    }
}