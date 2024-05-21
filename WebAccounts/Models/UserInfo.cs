using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Installments.Models
{
    public class UserInfo
    {
        public int UserID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public bool Inactive { get; set; }
        public bool IsSuperAdmin { get; set; }
        public int RoleID { get; set; }
        public string RoleTitle { get; set; }
        public string FingerID { get; set; }
    }
}