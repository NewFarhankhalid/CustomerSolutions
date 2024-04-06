using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class ProductBarcodes
    {
        public int ProductID { get; set; }
        public string Barcode { get; set; }
        public int Qty { get; set; }

    }
}