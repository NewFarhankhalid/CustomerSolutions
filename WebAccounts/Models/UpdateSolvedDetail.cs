using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class UpdateSolvedDetail
    {
        public int NewProblemStatementID { get; set; }
        public bool Solved { get; set; }
    }
}