using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FortuneTechPvtLtd.Models
{
    public class SourceModel
    {
        public int sourceid { get; set; }
        public string sourcename { get; set; }
        public int? companyid { get; set; }
        public int? branchid { get; set; }
        public decimal? JanAmount { get; set; }
        public decimal? FebAmount { get; set; }
        public decimal? MarAmount { get; set; }
        public decimal? AprAmount { get; set; }
        public decimal? MayAmount { get; set; }
        public decimal? JunAmount { get; set; }
        public decimal? JulAmount { get; set; }
        public decimal? AugAmount { get; set; }
        public decimal? SepAmount { get; set; }
        public decimal? OctAmount { get; set; }
        public decimal? NovAmount { get; set; }
        public decimal? DecAmount { get; set; }
    }
}