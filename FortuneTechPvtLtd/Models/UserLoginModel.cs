using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FortuneTechPvtLtd.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Please Enter Company Id")]
        public int companyid { get; set; }
        public int branchid { get; set; }
        [Required(ErrorMessage="Please Enter User Name")]
        [MaxLength(30,ErrorMessage = "User Name cannot be longer than 30 characters")]
        public string username { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [MaxLength(30, ErrorMessage = "User Name cannot be longer than 30 characters")]
        public string password { get; set; }

        public string OTP { get; set; }
        public string SetPassword { get; set; }
        public string ConfirmPassword { get; set; }
       
    }
}