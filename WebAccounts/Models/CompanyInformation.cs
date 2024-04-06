using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace Installments.Models
{
    public class CompanyInformation
    {
        public int CompanyID { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        public string Address { get; set; }
        [Required]
        [Display(Name = "Phone No.")]
        public string PhoneNo { get; set; }
        [Display(Name = "NTN No.")]
        public string NTNNo { get; set; }
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        public string Logo { get; set; }
        public string LogoPath { get; set; }
    }
}