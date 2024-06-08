using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class NewProblemStatement
    {
        public int ProblemStatementID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Complaint No")]
        public string ProblemStatmentNo { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Problem Title")]
        public string ProblemTitle { get; set; }
        public int CustomerID { get; set; }
        [Display(Name = "Customer Title")]
        public string CustomerTitle { get; set; }

        [Display(Name = "Problem Image")]
        public string ProblemImage { get; set; }
        public HttpPostedFileBase Imagefile { get; set; }
        public string ProblemImagePath { get; set; }
         public string Voice {  get; set; }
        public string Description { get; set; }
        [Display(Name = "Promise Date")]
        public DateTime PromiseDate { get; set; }
        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }
        [Display(Name = "Assign To:")]
        public int AssignTo {  get; set; }
        public string AssignToName { get; set; }
        [Display(Name = "Work Priority")]
        public int WorkPriority { get; set; }
        public string WorkPriorityName { get; set; }

        public string WorkStatus { get; set; }
        public int UserID { get; set; }
  
        public string Remarks { get; set; }
        public bool Solved { get; set; }
        public int ContactedPerson { get; set; }
        public string EmpName { get; set; }
        public string EmpPhoneNo { get; set; }
        [Required]
        [Display(Name = "Call Time Duration")]
        public string Calltime { get; set; }
        public int OperatorID { get; set; }
        public string Designation { get; set; }
        public int DesignationID { get; set; }
        public string WorkCategory { get; set; }
        public bool IsContactPerson { get; set; }
        public List<FormName> lstFormName = new List<FormName>();

        public List<ProblemRemarks> lstProblemRemarks = new List<ProblemRemarks>();
        public List<OperatorDetail> lstOperatorDetail = new List<OperatorDetail>();
    }
}