using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Installments.Models
{
    public class CollectionCenter
    {
        public int CollectionCenterID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Center Title")]
        public string CenterTitle { get; set; }
        public string Remarks { get; set; }
        [Required]
        [Display(Name = "Branch")]
        public int BranchID { get; set; }
    }
}