using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Installments.Models
{
    public class ColourInfo
    {
        public int ColourID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Colour Title")]
        public string ColourTitle { get; set; }
        public string ColourRGBCode { get; set; }
        public string Description { get; set; }
        public bool InActive { get; set; }

    }
}