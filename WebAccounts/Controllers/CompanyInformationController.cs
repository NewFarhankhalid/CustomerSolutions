using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Installments.Controllers
{
    public class CompanyInformationController : Controller
    {
        // GET: CompanyInformation
     
        // GET: CompanyInformation/Create
        public ActionResult Create()
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.New) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            DataTable dtcompanyedit = General.FetchData("Select * from CompanyInformation");
            List<CompanyInformation> lstbranch = DataTableToObject(dtcompanyedit);
            if (lstbranch.Count > 0)
            {
                return View(lstbranch[0]);
            }
           else 
            {
                return View(new CompanyInformation());
            }
            
        }

        // POST: CompanyInformation/Create
        [HttpPost]
        public ActionResult Create(CompanyInformation company)
        {
            string sql = "Select CompanyID,CompanyName,NTNNo From CompanyInformation";
            DataTable dts = General.FetchData(sql);
            if (dts.Rows.Count > 0)
            {
                var Name = dts.Rows[0]["CompanyName"].ToString();
                var NTNno = dts.Rows[0]["NTNNo"].ToString();
                var id = int.Parse(dts.Rows[0]["CompanyID"].ToString());
                //new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Delete, GeneralAPIsController.LogSource.Company, id, " Company Name " + Name+"and Ntn No"+NTNno);
                string Query1 = "Delete from CompanyInformation";
                General.ExecuteNonQuery(Query1);
            }
            try
            {
                // TODO: Add insert logic here
                string Query = "Insert into CompanyInformation (CompanyName,Address,PhoneNo,NTNNo,EmailAddress,Logo) ";
                Query = Query + "Values ('" + company.CompanyName + "','" + company.Address + "','" + company.PhoneNo + "','" + company.NTNNo + "','" + company.EmailAddress + "','" + company.Logo + "')";
                Query = Query + (@"Select @@IDENTITY as CompanyID");
                DataTable dt = General.FetchData(Query);
                company.CompanyID = int.Parse(dt.Rows[0]["CompanyID"].ToString());
                //new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.New, GeneralAPIsController.LogSource.Company, company.CompanyID, " Company Name "+company.CompanyName);
                //General.ExecuteNonQuery(Query);
                General.CompanyName = company.CompanyName.Trim();
                new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.New, GeneralAPIsController.LogSource.Company, company.CompanyID, " Company Name " + company.CompanyName);
                return Json("true," + company.CompanyID);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveOrUpdateImage(HttpPostedFileBase txtfile)
        {
            if (txtfile != null && txtfile.ContentLength > 0)
            {
                string[] filecontent = txtfile.FileName.ToString().Split(',');
                string filename1 = filecontent[0] + "-" + filecontent[1] + ".jpeg";
                int CompanyID = int.Parse(filecontent[0]);
                string fileext = Path.GetExtension(filename1);
                if (fileext == ".jpg" || fileext == ".png" || fileext == ".jpeg")
                {
                    bool exists = Directory.Exists(Server.MapPath("/CompanyLogos/"));
                    if (!exists)
                    {
                        var createfolder = Path.Combine(Server.MapPath("/CompanyLogos/").ToString());
                        System.IO.Directory.CreateDirectory(createfolder);
                    }

                    int j = 1;
                    string filename = General.userID + "-" + j + ".jpeg";
                    string filepath = ("/CompanyLogos/") + filename1;
                    string _path = Path.Combine(Server.MapPath("~/CompanyLogos/"), filename);
                    //string fileext = "";
                    while (System.IO.File.Exists(_path))
                    {
                        filename = General.userID + "-" + j + ".jpeg";
                        j++;
                        _path = "";
                        filepath = "";
                        filepath = ("/CompanyLogos/") + filename;
                        _path = Path.Combine(Server.MapPath("~/CompanyLogos/"), filename);
                    }
                    string sql = @"update CompanyInformation set Logo ='" + filepath + "' where CompanyID=" + CompanyID;
                    General.ExecuteNonQuery(sql);
                    txtfile.SaveAs(Server.MapPath(filepath));
                }
            }
            return Json("true");
        }

        List<CompanyInformation> DataTableToObject(DataTable dt)
        {
            List<CompanyInformation> lstbranch = new List<CompanyInformation>();
            CompanyInformation bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new CompanyInformation();
                if (dr["CompanyID"] != DBNull.Value)
                {
                    bi.CompanyID = int.Parse(dr["CompanyID"].ToString());
                }

                if (dr["CompanyName"] != DBNull.Value)
                {
                    bi.CompanyName = (dr["CompanyName"].ToString());
                }
                if (dr["Address"] != DBNull.Value)
                {
                    bi.Address = (dr["Address"].ToString());
                }
               
                if (dr["PhoneNo"] != DBNull.Value)
                {
                    bi.PhoneNo = (dr["PhoneNo"].ToString());
                }
                if (dr["NTNNo"] != DBNull.Value)
                {
                    bi.NTNNo = (dr["NTNNo"].ToString());
                }
                if (dr["EmailAddress"] != DBNull.Value)
                {
                    bi.EmailAddress = (dr["EmailAddress"].ToString());
                }
                if (dr["Logo"] != DBNull.Value)
                {
                    bi.LogoPath = (dr["Logo"].ToString());
                }
                lstbranch.Add(bi);
            }
            return lstbranch;
        }


        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase imagefile)
        {
            string _FileName = Path.GetFileName(imagefile.FileName);
            if (Request.Files.Count > 0)
            {
                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/Images/";
                    string filename = Path.GetFileName(Request.Files[i].FileName);

                    HttpPostedFileBase filex = files[i];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = filex.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = filex.FileName;
                        var roothpath = Server.MapPath("/Images/");
                        var filesavepath = System.IO.Path.Combine(roothpath, fname);

                        imagefile.SaveAs(filesavepath);
                        //string sql = "insert into OrderInfo(OrderImage1) values ('" + filesavepath + "')";
                        return View("Index");
                    }
                }
            }
            return Json("Image Uploaded");
        }
    }
}
