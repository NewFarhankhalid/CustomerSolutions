using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Installments.Models
{
    public class VariantInfo
    {
        public int VariantID {get;set;}
        public string Title{get;set;}
        public string ShortTitle { get;set;}
        public double Measurement { get;set;}
        public bool inactive { get;set;}
        public string remarks { get;set;}
    }
}