using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Installments.Models
{
    public class Login
    {

        public int UserID { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        
        public string Password { get; set; }
    }
}