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
    public class CategoryInfoController : Controller
    {
        public ActionResult Index()
        {
            DataTable dtcatagory = General.FetchData("Select * from CategoryInfo");
            List<CategoryInfo> lstcatagory = DataTableToObject(dtcatagory);
            return View(lstcatagory);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            Installments.Models.CategoryInfo objCategoryInfo = new CategoryInfo();
            ViewBag.CategoryInfo = new DropDown().GetCategoryInfo();
            return View(objCategoryInfo);
        }

        [HttpPost]
        public ActionResult Create(CategoryInfo objproduct)
        {
            try
            {
                string Query = "Insert into CategoryInfo (CategoryTitle,Inactvie,Description,ParentCategory) ";
                Query = Query + "Values ('" + objproduct.CategoryTitle + "'," + (objproduct.Inactvie == true ? "1" : "0") + ",'" + objproduct.Description + "','"+ objproduct.ParentID+ "')";
                //General.ExecuteNonQuery(Query);
                //Query = "";
                Query = Query + " Select @@IDENTITY as CategoryID";
                objproduct.CategoryID = int.Parse(General.FetchData(Query).Rows[0]["CategoryID"].ToString());

                return Json("true," + objproduct.CategoryID);
            }
            catch
            {
                return Json("error");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveOrUpdateImage(HttpPostedFileBase txtfile)
        {

            if (txtfile != null && txtfile.ContentLength > 0)
            {
                string[] filecontent = txtfile.FileName.ToString().Split(',');
                string filename = filecontent[0] + "-" + filecontent[1] + ".jpeg";
                int categoryID = int.Parse(filecontent[0]);
                string fileext = Path.GetExtension(filename);
                if (fileext == ".jpg" || fileext == ".png" || fileext == ".jpeg")
                {
                    bool exists = Directory.Exists(Server.MapPath("/CategoryImages/"));
                    if (!exists)
                    {
                        var createfolder = Path.Combine(Server.MapPath("/CategoryImages/").ToString());
                        System.IO.Directory.CreateDirectory(createfolder);
                        exists = true;
                    }
                    string filepath = ("/CategoryImages/") + filename;
                    //string filepath = Server.MapPath("~/ProductImages/")+ filename;
                    string sql = @"update CategoryInfo set CategoryImage ='" + filepath + "'  where CategoryInfo.CategoryID=" + categoryID + "";
                    General.ExecuteNonQuery(sql);
                    txtfile.SaveAs(Server.MapPath(filepath));
                }
            }
            return Json("true");
        }
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase imagefile)
        {
            string _FileName = Path.GetFileName(imagefile.FileName);

            if (Request.Files.Count > 0)
            {
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/CategoryImages";
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
                        var roothpath = Server.MapPath("/CategoryImages/");
                        var filesavepath = System.IO.Path.Combine(roothpath, fname);
                        imagefile.SaveAs(filesavepath);
                        //string sql = "insert into productInfo(ProductImage) values ('" + filesavepath + "')";
                        return View("Index");
                    }

                    // Get the complete folder path and store the file inside it.
                    //string _path = Path.Combine(Server.MapPath("~/ProductImages/"), fname);
                    //txtfile.SaveAs(_path);

                }
            }
            return Json("Image Uploaded");
        }



        public ActionResult Edit(int id)
        {

            ViewBag.CategoryInfo = new DropDown().GetCategoryInfo();
            DataTable dtcatagoryinfo = General.FetchData("Select * from CategoryInfo where CategoryID=" + id);
            List<CategoryInfo> lstbranch = DataTableToObject(dtcatagoryinfo);
            if (lstbranch.Count > 0)
            {
                return View(lstbranch[0]);
            }
            return RedirectToAction("index");
        }

        // POST: CategoryInfo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CategoryInfo obj)
        {
            try
            {
                string Query = "";
                Query = Query + "UPDATE [dbo].[CategoryInfo] ";
                Query = Query + " SET    [CategoryTitle] ='" + obj.CategoryTitle + "' ";
                Query = Query + "    ,[Inactvie] = " + (obj.Inactvie == true ? "1" : "0") + "";
                Query = Query + "    ,[Description] ='" + obj.Description + "' ";
                Query = Query + ",[ParentCategory]='"+obj.ParentID+"' ";
                Query = Query + ",[CategoryImage]='" + obj.CategoryImage + "'";
                Query = Query + "WHERE CategoryID=" + id;
                Query = Query + " Select @@IDENTITY as CategoryID";
                obj.CategoryID = int.Parse(General.FetchData(Query).Rows[0]["CategoryID"].ToString());
                if (Query != "")
                {
                    General.ExecuteNonQuery(Query);
                }
                return Json("true," + obj.CategoryID);
            }
            catch
            {
                return Json("error");
            }
        }

        // GET: CategoryInfo/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            string Query = "Delete from CategoryInfo Where CategoryID=" + id + " and CategoryID not in (Select CatagoryID from ProductInfo)";
            General.ExecuteNonQuery(Query);
            return Json("true");
        }
        
        List<CategoryInfo> DataTableToObject(DataTable dt)
        {
            List<CategoryInfo> lstbranch = new List<CategoryInfo>();
            CategoryInfo bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new CategoryInfo();
                if(dr["ParentCategory"] !=DBNull.Value)
                {
                    bi.ParentID = int.Parse(dr["ParentCategory"].ToString());
                }
                if (dr["CategoryID"] != DBNull.Value)
                {
                    bi.CategoryID = int.Parse(dr["CategoryID"].ToString());
                }
                if (dr["CategoryTitle"] != DBNull.Value)
                {
                    bi.CategoryTitle = (dr["CategoryTitle"].ToString());
                }
                if (dr["Description"] != DBNull.Value)
                {
                    bi.Description = (dr["Description"].ToString());
                }              
                if(dr["CategoryImage"] !=DBNull.Value)
                {
                    bi.CategoryImage = (dr["CategoryImage"].ToString());
                    ViewBag.CategoryImage = (dr["CategoryImage"].ToString());
                }
                if (dr["Inactvie"] != DBNull.Value)
                {
                    bi.Inactvie = bool.Parse(dr["Inactvie"].ToString());
                }
                lstbranch.Add(bi);
            }
            return lstbranch;
        }
    }
}
