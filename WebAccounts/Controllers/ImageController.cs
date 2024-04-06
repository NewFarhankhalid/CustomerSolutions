using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Installments.Controllers
{
    public class ImageController : Controller
    {
        
        public ActionResult Index()
        {
            return View("Add");
        }
        [HttpPost]
        public ActionResult Add(HttpPostedFileBase imagefile)
        {
            if(imagefile!=null && imagefile.ContentLength>0)
            {
                string filename = Path.GetFileName(imagefile.FileName);
                string fileext = Path.GetExtension(filename);
                if(fileext==".jpg"|| fileext==".png"|| fileext==".jpeg")
                {
                    string filepath = Path.Combine(Server.MapPath("~/ProductImages"), filename);
                    string sql = @"insert into ImageInfo(Title,ImagePath) values ('"+filename+"','"+filepath+"')";
                    
                    General.ExecuteNonQuery(sql);
                    imagefile.SaveAs(filepath);
                    
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveOrUpdateImage( HttpPostedFileBase txtfile )
        {
            //    #region UploadImage Save in Folder
            //    string fileName = "";
            //    if (txtfile != null)
            //    {
            //        string dPath = "~/" + Constants.UPLOADED_DOCS + "/" + Constants.UPLOADED_Session_DOCS + "/" + eventId.ToString();
            //        bool exists = System.IO.Directory.Exists(Server.MapPath(dPath));
            //        if (!exists)
            //            System.IO.Directory.CreateDirectory(Server.MapPath(dPath));

            //        //string path = "~/" + Constants.UPLOADED_DOCS + "/";
            //        fileName = Path.GetFileNameWithoutExtension("File" + "_" + eventId.ToString()) + "_" + Guid.NewGuid() + Path.GetExtension(txtfile.FileName);

            //        txtfile.SaveAs(Server.MapPath(dPath) + "/" + fileName);
            //    }

            //#endregion


            if (txtfile != null && txtfile.ContentLength > 0)
            {
                string filename = Path.GetFileName(txtfile.FileName);
                string fileext = Path.GetExtension(filename);
                if (fileext == ".jpg" || fileext == ".png" || fileext == ".jpeg")
                {
                    string filepath = Path.Combine(Server.MapPath("~/ProductImages"), filename);
                    string sql = @"insert into ImageInfo(Title,ImagePath) values ('" + filename + "','" + filepath + "')";

                    General.ExecuteNonQuery(sql);
                    txtfile.SaveAs(filepath);

                }
            }


            return Json("true");

    
            }

    }
}