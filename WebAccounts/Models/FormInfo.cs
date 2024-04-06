using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class FormInfo
    {
        public int FormID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Form Title")]
        public string FormTitle { get; set; }
        public string Description { get; set; }
        public bool InActive { get; set; }
    }
}