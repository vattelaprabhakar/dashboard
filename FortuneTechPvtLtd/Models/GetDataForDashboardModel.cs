using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace FortuneTechPvtLtd.Models
{
    public class GetDataForDashboardModel
    {

        public int StatusCount { get; set; }
        public string StatusName { get; set; }
        public int companyid { get; set; }
        public int branchid { get; set; }
    }
}


   //       public int Id { get; set; }
   //     [Required(ErrorMessage="Please Lead Owner Name")]
   //     public string LeadName { get; set; }
   //      [Required(ErrorMessage = "Please Enter Mobile Number")]
   //     public string Mobile { get; set; }
   //     public string Email { get; set; }
   //   //  public string Qualification { get; set; }
   //     //public string YearOfPassedOut { get; set; }

   //     //public List<professionaldetails> ProfessionalDetails { get; set; }//radio 
   //     //public string selectedprofessionaldetails { get; set; }

   //   //  public string WorkExperienceDomain { get; set; }
   //     public string Address { get; set; }


   //     public List<Product> CourceInterestedIn { get; set; }//radio
   //       [Required(ErrorMessage = "Please Select Product")]
   //     public string selectedinterestedcourse { get; set; }

   //    // public List<Location> PreferredLocation { get; set; }//radio
   //    // public string selectedpreferredlocation { get; set; }

   //    // public List<Batch> PreferredBatch { get; set; }//radio
   //    // public string selectedpreferredbatch { get; set; }

   //       public List<Source> ReferredBy { get; set; }//radio
   //     public string selectedrefby { get; set; }

   //   //  public List<JoinTimings> WantToJoin { get; set; }//radio
   //   //  public string selectedwanttojoin { get; set; }

   //     public List<Alerts> WantAlerts { get; set; }//radio
   //     public string seletedwantalerts { get; set; }

   //      [Required(ErrorMessage = "Please Select Lead Assigned by")]
   //     public string LeadAssignedby { get; set; }//text box auto by username

   //      public List<Leadstatus> Status { get; set; }//radio
   //     public string selectedstatus { get; set; }

   //     public List<SelectListItem> LeadAssignedTo { get; set; }
        
   //     public string selectedleadassignedto { get; set; }

   //     public string Website { get; set; }
   //     public string Industry { get; set; }
   //     public string Rating { get; set; }
   //     public string Comment1 { get; set; }
   //     public string Comment2 { get; set; }
   //     public string Comment3 { get; set; }
   //     public string Commentsforassignedto { get; set; }
   //     public string LeadOwner { get; set; }
   //     public DateTime? Leaddate { get; set; }
   //     public DateTime? Followupdate { get; set; }

   //     public GetDataForDashboardModel()
   //     {
   //       //  ProfessionalDetails = new List<professionaldetails>();
   //         CourceInterestedIn = new List<Product>();
   //      //   PreferredBatch = new List<Batch>();
   //         ReferredBy = new List<Source>();
   //        // WantToJoin = new List<JoinTimings>();
   //         WantAlerts = new List<Alerts>();
   //         Status = new List<Leadstatus>();
   //     }
   // }
   ////public  class professionaldetails
   //// {
   ////     public int profid { get; set; }
   ////     public string profname { get; set; }
   ////     public bool isselected { get; set; }
       
   //// }
   //public class Product
   //{
   //    public int ProductId { get; set; }
   //    public string ProductName { get; set; }
   //    public bool isselected { get; set; }
   //}
   ////public class Location
   ////{
   ////    public int LocationId { get; set; }
   ////    public string LocationName { get; set; }
   ////    public bool isselected { get; set; }
   ////}
   ////public class Batch 
   ////{
   ////    public int BatchId { get; set; }
   ////    public string BatchName { get; set; }
   ////    public bool isselected { get; set; }
   ////}
   //public class Source
   //{
   //    public int SourceId { get; set; }
   //    public string SourceName { get; set; }
   //    public bool isselected { get; set; }
   //}
  //public  class JoinTimings
  // {
  //      public int JoinId { get; set; }
  //     public string JoinName { get; set; }
  //     public bool isselected { get; set; }
  // }
  //public class Alerts
  //{
  //    public int AlertId { get; set; }
  //     public string AlertName { get; set; }
  //     public bool isselected { get; set; }
  //}
  //public class Leadstatus
  //{
  //     public int StatusId { get; set; }
  //     public string StatusName { get; set; }
  //     public bool isselected { get; set; }
  //}
  //  }
