using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class DBName
    {
        public int CustomerID { get; set; }
        public string DbName { get; set; }
        public int PaymentTypeID { get; set; }
        public string PaymentType { get; set; }
        public decimal Payment {  get; set; }
    }
}