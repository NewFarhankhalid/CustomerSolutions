using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Installments.Models
{
    public class SaleInfo
    {
        public int SaleID { get; set; }
        
            [Required]
            [Display(Name = "Date")]
           
            public DateTime SaleDate { get; set; }

        
        [Required]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Required]
        [Display(Name = "Customer CNIC")]
        public string CustomerCNIC { get; set; }
        [Required]
        [Display(Name = "Customer Address")]
        public string CustomerAddress { get; set; }
        [Required]
        [Display(Name = "Customer City")]
        public string CustomerCity { get; set; }
        [Required]
        [Display(Name = "Customer Mobile Number")]
        public string CustomerMobileNO { get; set; }
        [Required]
        [Display(Name = "Customer Mobile Number 2")]
        public string CustomerMobileNo2 { get; set; }
        [Required]
        [Display(Name = "Sale No")]
        public string SaleNo { get; set; }
        [Required]
        [Display(Name = "Total Cost")]
        public decimal TotalCost { get; set; }
        [Required]
        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }
        [Display(Name ="Delivery Status")]
        public int Status { get; set; }
        public string TransportService { get; set; }
        public string BultiyNo { get; set; }
        public int ddlDel { get; set; }
        public decimal CashReceived { get; set; }

        public List<SaleDetail> lstSaleDetail = new List<SaleDetail>();
        public List<SaleDetail> lstShortSaleDetail = new List<SaleDetail>();

    }
}