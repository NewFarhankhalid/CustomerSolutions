using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class SystemDetail
    {
        public int CustomerID { get; set; }
        public string IPAddress { get; set; }
        public string MacAddress { get; set; }
        public int Type { get; set; }
    }
}