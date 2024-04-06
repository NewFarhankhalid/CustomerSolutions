using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class PaymentStatus
    {
        public int PaymentStatusID { get; set; }
        public int CustomerID { get; set; }
        public int AmountReceived { get; set; }
        public DateTime DateReceived { get; set; }
        public int PaymentType { get; set; }
        public string Description { get; set; }
        public string CustomerCompanytitle { get; set; }
    }
}