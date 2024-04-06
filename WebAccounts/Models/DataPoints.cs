using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
namespace Installments.Models

{
    public class DataPoint
    {

        public DataPoint(string _x, decimal _y)
        {
            this.x = _x;
            this.y = _y;
        }
        //Explicitly setting the name to be used while serializing to JSON.
        public string x = "";
        //Explicitly setting the name to be used while serializing to JSON.
        public decimal y = 0;
    }
}