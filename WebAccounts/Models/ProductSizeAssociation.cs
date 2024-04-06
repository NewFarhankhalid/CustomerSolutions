using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class ProductSizeAssociation
    {
        public int ProductID { get; set; }
        public int SizeID { get; set; }
        public string SizeTitle { get; set; }
    }
}