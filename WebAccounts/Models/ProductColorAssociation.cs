using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class ProductColorAssociation
    {
        public int ProductID { get; set; }
        public int ColorID { get; set; }
        public string ColorTitle { get; set; }
        public string RGBCode { get; set; }

    }
}