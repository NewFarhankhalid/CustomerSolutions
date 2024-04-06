using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Installments.Models
{
    public class Images
    {
        public int ImageID { get; set; }

        [Display(Name ="Product")]
        public int ProductID { get; set; }
        [Required]
        public HttpPostedFileBase Image1 { get; set; }
        [Display(Name = "Image 1")]
        [Required]
        public string ImagePath { get; set; }
        [Required]
        public HttpPostedFileBase Image2 { get; set; }
        [Display(Name = "Image 2")]
        [Required]
        public string ImagePath2 { get; set; }
        [Required]
        public HttpPostedFileBase Image3 { get; set; }
        [Display(Name = "Image 3")]
        [Required]
        public string ImagePath3 { get; set; }
        
        [Required]
        public HttpPostedFileBase Image4 { get; set; }
        [Display(Name = "Image 4")]
        [Required]
        public string ImagePath4 { get; set; }
        [Required]
        public HttpPostedFileBase Image5 { get; set; }
        [Display(Name = "Image 5")]

        [Required]
        public string ImagePath5 { get; set; }
        [Required]
        public HttpPostedFileBase Image6 { get; set; }
        [Display(Name = "Image 6")]

        [Required]
        public string ImagePath6 { get; set; }
        [Required]
        public HttpPostedFileBase Image7 { get; set; }
        [Display(Name = "Image 7")]
        [Required]
        public string ImagePath7 { get; set; }
        public string Remarks { get; set; }
    }
}