using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class ProductTandCAssociation
    {

        public int ProductID { get; set; }

        public int TandCID { get; set; }
        public string TermsTitle { get; set; }
    }
}