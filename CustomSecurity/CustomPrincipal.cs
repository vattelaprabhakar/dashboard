//using FortuneTechPvtLtd.DataModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Principal;
//using System.Web;
//using System.Web.WebPages.Html;

//namespace FortuneTechPvtLtd.CustomSecurity
//{
//    public class CustomPrinciple : IPrincipal
//    {
//        public CustomPrinciple(string username)
//        {
//            Identity = new GenericIdentity(username);
//        }

//        public IIdentity Identity
//        {
//            get;
//            private set;

//        }

//        public bool IsInRole(string role)
//        {
//            if (role == rolename)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }


//        public string password { get; set; }
//        public string rolename { get; set; }
//        public int compid { get; set; }
//        public int branid { get; set; }

//    }

//}