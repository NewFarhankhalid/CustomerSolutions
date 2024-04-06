using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class UserConfiguration
    {
        public int userid { get; set; }
        public int FranchiseID { get; set; }
        public int BranchID { get; set; }
        public int AreaID { get; set; }
        public string Title { get; set; }
        public bool Allowed { get; set; }
    }
}