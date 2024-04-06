using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class ProductTagAssociation
    {

        public int ProductID { get; set; }

        public int TagsID { get; set; }
        public string TagTitle { get; set; }
    }
}