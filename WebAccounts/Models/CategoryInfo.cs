using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Installments.Models
{
    public class CategoryInfo
    {
        public int CategoryID { get; set; }
        public int ParentID { get; set; }
        
        [Required]
        [StringLength(50)]
        [Display(Name = "Category Title")]
        public string CategoryTitle { get; set; }
        [Display(Name = "Category Image")]
        public string CategoryImage { get; set; }
        public HttpPostedFileBase Imagefile { get; set; }
        
        public string Description { get; set; }
        public bool Inactvie { get; set; }
    }
}