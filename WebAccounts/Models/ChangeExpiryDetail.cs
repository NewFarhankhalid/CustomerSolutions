using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class ChangeExpiryDetail
    {
        public int CustomerID { get; set; }
        public bool Inactive { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}