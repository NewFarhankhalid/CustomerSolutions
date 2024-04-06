using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class ReportsDetail
    {
        public int RoleID { get; set; }
        public int ReportID { get; set; }
        public string ReportTitle { get; set; }
        public string SectionTitle { get; set; }
        public bool ReportAllowed { get; set; }
    }
}