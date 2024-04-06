using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class PaymentType
    {
        public int Id { get; set; }
        [Display(Name="Payment Type")]
        public string Name { get; set; }
        public int Amount { get; set; }
    }
}