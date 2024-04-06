using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Installments.Models
{
    public class SizeInfo
    {
        public int SizeID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Size Title")]
        public string SizeTitle { get; set; }
        public bool InActive { get; set; }
        public string Description { get; set; }

    }
}