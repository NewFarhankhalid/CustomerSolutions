using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Installments.Models
{
    public class EmployeeInfo
    {
        public int EmployeeID { get; set; }
        [Required]
        [Display(Name = "Branch")]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select Branch")]
        public int BranchID { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Father Name")]
        public string FatherName { get; set; }
        [Required]
        [StringLength(15)]
        [Display(Name = "CNIC")]
        public string CNIC { get; set; }
        public string Address { get; set; }
        public string Domicile { get; set; }
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }
        public decimal Salary { get; set; }
        public decimal Loan { get; set; }
        [Display(Name = "GP Fund Acc No.")]
        public string GPFundNo { get; set; }

        [Display(Name = "KK Fund ")]
        public string KFFund { get; set; }
        public string Other { get; set; }
        public string Qualifications { get; set; }
        public int Gender { get; set; }
        [Required]
        [Display(Name = "Date of Birth")]

        public DateTime DOB { get; set; }
        public string Nationality { get; set; }
        [Display(Name = "Bank Account No.")]
        public string BankAccount { get; set; }
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Display(Name = "Branch Name")]
        public string BankBranch { get; set; }
        [Display(Name = "Home Tel.")]
        public string HomeTel { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Mobile No.")]
        public string MobileTel { get; set; }
        [Required]

        [Display(Name = "Designation")]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select Designation")]
        public int DesignationID { get; set; }
        [Required]

        [Display(Name = "Date of Joining")]
        public DateTime JoiningDate { get; set; }
        [Required]

        [Display(Name = "Date of Confirmation")]
        public DateTime ConfirmationDate { get; set; }

        [Display(Name = "Basic Salary")]
        public decimal BasicSalary { get; set; }

        
        [Display(Name = "Utility Allowance")]
        public decimal UtilityAllowance { get; set; }

        [Display(Name = "Medical Allowance")]
        public decimal MedicalAllowance { get; set; }
    }
}