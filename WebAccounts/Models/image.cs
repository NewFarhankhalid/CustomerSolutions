using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Installments.Models
{
    public class image
    {
        public int imageID { get; set; }
        public string Title { get; set; }
        [DisplayName("Upload File")]
        public string imagepath { get; set; }
        public HttpPostedFileBase imagefile { get; set; }
    }
}