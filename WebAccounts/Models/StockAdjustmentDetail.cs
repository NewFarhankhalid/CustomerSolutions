using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class StockAdjustmentDetail
    {
        public int StockAdjustmentID { get; set; }
        public int ProductID { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public string Remarks { get; set; }
        public int Available { get; set; }
        public int ProductPriority { get; set; }
    }
}