using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class FormName
    {
        public int FormID { get; set; }
        [Display(Name = "Form Title")]
        public string FormTitle { get; set; }
        public int ProblemStatementID { get; set; }
        public string FormDescription { get; set; }
    }
}