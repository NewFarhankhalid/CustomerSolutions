using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class ProblemSolutions
    {
        public int ProblemSolutionID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Problem Title")]
        public string ProblemTitle { get; set; }
        [Display(Name = "Problem Image")]
        public string ProblemImage { get; set; }
        public HttpPostedFileBase Imagefile { get; set; }

        public string Description { get; set; }
    }
}