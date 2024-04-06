using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Installments.Models
{
    public class TagsInfo
    {
        public int TagsID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Tags Title")]
        public string TagTitle { get; set; }
        public string Description { get; set; }
        public bool InActive { get; set; }

    }

}