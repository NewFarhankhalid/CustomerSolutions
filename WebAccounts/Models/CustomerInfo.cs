using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class CustomerInfo
    {
        public int CustomerID { get; set; }
        [Required]
        [Display(Name = "Customer Company Name")]
        public string CustomerCompanytitle { get; set; }
        [Required]
        [Display(Name = "Owner Name")]
        public string OwnerName { get; set; }
        [Display(Name = "Owner No")]
        public string OwnerContactNo { get; set; }
        [Display(Name = "City Name")]     
        public string CityName { get; set; }
        public bool InActive { get; set; }
        public bool Updated { get; set; }
        public bool PaymentStatus { get; set; }
        public bool LegalUser { get; set; }
        [Display(Name = "Employee Phone No")]
        public string EmployeePhoneNo { get; set; } 
        public string BusinessNature { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; }
        public int CityID { get; set; }
        [StringLength(50)]
        [Display(Name = "Person To Contact")]
        public string Persontocontact { get; set; }
        public string OffDay { get; set; }

        [StringLength(50)]
        [Display(Name = "Contact No.")]
        public string contactno { get; set; }
        public string Address { get; set; }
        [Display(Name = "Payment Type")]

        public string Email { get; set; }
        [Display(Name ="Software Version")]
        public string SoftwareVersion { get; set; }
        public string OpenTime { get; set; }
        public string OffTime { get; set; }
        public string Branch {  get; set; }
        public string Designation {  get; set; }
        public DateTime ExpiryDate { get; set; }

        public List<SystemDetail> lstSystemDetail = new List<SystemDetail>();
        public List<DBName> lstDBName = new List<DBName>();
        public List<OperatorDetail> lstOperatorName = new List<OperatorDetail>();
    }
}