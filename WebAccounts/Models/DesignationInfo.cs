using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Installments.Models
{
    public class DesignationInfo
    {

        public int DesignationId { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Designation Title")]
        public string DesignationTitle { get; set; }
        public bool InActive { get; set; }
        public string Description { get; set; }
    }
}