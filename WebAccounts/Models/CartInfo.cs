using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class CartInfo
    {
        public int itemID { get; set;}
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string Imagepath { get; set; }
        public string ImageName { get; set; }
    }
}