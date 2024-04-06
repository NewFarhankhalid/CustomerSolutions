using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class OperatorDetail
    {
        public int CustomerID { get; set; }
        public int ProblemStatementID { get; set; }
        public string OperatorName { get; set; }

        public string OperatorPhone { get; set; }
        public int DesignationID { get; set; }
        public string Designation {  get; set; }

        public bool IsContactPerson { get; set; }
    }
}