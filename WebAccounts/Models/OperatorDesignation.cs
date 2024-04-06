using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class OperatorDesignation
    {
        public int Id { get; set; }
        [Display(Name="Designation")]
        public string Name { get; set; }
    }
}