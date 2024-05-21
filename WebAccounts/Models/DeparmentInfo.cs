using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class DeparmentInfo
    {

        public int DeparmentId { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Department Title")]
        public string DeparmentTitle { get; set; }
        public bool InActive { get; set; }
        public string Description { get; set; }
    }
}