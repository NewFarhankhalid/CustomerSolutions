using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Installments.Models
{
    public class RightsRole
    {
        public int RoleID { get; set; }
        [Required]
        public string RoleTitle { get; set; }
        public string Description { get; set; }
        public bool Inactive { get; set; }
       public List<RightsRoleDetail> LstRoleDetail { get; set; }
        public List<ReportsDetail> LstReportDetail { get; set; }

    }
}