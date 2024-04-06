using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Installments.Models
{
    public class TermsandconditionInfo
    {
        public int TermsandConditionID { get; set; }
        
        [Required]
        [Display(Name = "Terms Title")]
        public string TermsTitle { get; set; }
        public string TermsDescription { get; set; }
        public bool Inactive { get; set; }

    }
}