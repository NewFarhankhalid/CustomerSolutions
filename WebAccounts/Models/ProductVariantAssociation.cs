using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class ProductVariantAssociation
    {
        public int ProductID { get; set; }
        public int VariantId { get; set; }
        public string VariantTitle { get; set; }
        public decimal Measurement { get; set; }
    }
}