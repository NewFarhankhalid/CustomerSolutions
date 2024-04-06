using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class RightsRoleDetail
    {
        public int RoleID { get; set; }
        public int MenuID { get; set; }
        public string MenuTitle { get; set; }
        public int ParentID { get; set; }
        public bool New { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool Allowed { get; set; }
    }
}