using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Installments.Controllers
{
    public class UploadUpdatesController : Controller
    {
        // GET: UploadUpdates
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadProductFile(HttpPostedFileBase file)
        {
            string ErrorCheck = "";
            string _path = "";
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string fname = _FileName;
                    // _path= Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/UploadedFiles/Products"), fname);
                    bool exists = Directory.Exists(Server.MapPath("/UploadedFiles/"));
                    if (!exists)
                    {
                        ErrorCheck = "TestExistFile";
                        var createfolder = Path.Combine(Server.MapPath("/UploadedFiles/").ToString());
                        System.IO.Directory.CreateDirectory(createfolder);
                        exists = true;
                    }
                    _path = Path.Combine(Server.MapPath("~/UploadedFiles/"), _FileName);
                    if(System.IO.File.Exists(_path))
                    {
                        ErrorCheck = "TestDelete";
                        System.IO.File.Delete(_path);
                    }
                    string filepath = ("/UploadedFiles/") + _FileName;
                    ErrorCheck = "Test1";
                    file.SaveAs(_path);
                    ViewBag.Message = "File Uploaded SuccessFully";
                    return View("Index");
                }
                ViewBag.Message = "Error Uploading File";
                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "File upload failed!! (exception: " + ex.Message + ")\n to path " + _path+"\n\n Error on"+ ErrorCheck ; ;
                return View("Index");
            }
        }
    }
}