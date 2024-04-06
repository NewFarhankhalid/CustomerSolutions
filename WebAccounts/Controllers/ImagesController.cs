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
    public class Bannner
    {
        public int ImageID;
        public string ImageLocation;
        public string Remarks;
    }
    public class ImagesController : Controller
    {
        // GET: Images
        public ActionResult Index()
        {
            string sql = @"Select * from ImagesInfo";
            DataTable dt = General.FetchData(sql);
            return View(dt);
        }


        [HttpGet]
        public ActionResult GetAllImages()
        {
//           string sql = 



            DataTable dtproductinfo = General.FetchData(@"Select 1 as ImageID,isnull(ImagePathConfig.GeneralImages, '') + '' + ImagesInfo.Image as ImageLocation,ImagesInfo.Remarks1 As Remarks from ImagesInfo cross join ImagePathConfig
union all
Select 2,isnull(ImagePathConfig.GeneralImages, '') + '' + ImagesInfo.Image2 as Image2,ImagesInfo.Remarks2 from ImagesInfo cross join ImagePathConfig
union all
Select 3,isnull(ImagePathConfig.GeneralImages, '') + '' + ImagesInfo.Image3 as Image3,ImagesInfo.Remarks3 from ImagesInfo cross join ImagePathConfig
union all
Select 4,isnull(ImagePathConfig.GeneralImages, '') + '' + ImagesInfo.Image4 as Image4,ImagesInfo.Remarks4 from ImagesInfo cross join ImagePathConfig
union all
Select 5,isnull(ImagePathConfig.GeneralImages, '') + '' + ImagesInfo.Image5 as Image5,ImagesInfo.Remarks5 from ImagesInfo cross join ImagePathConfig
union all
Select 6,isnull(ImagePathConfig.GeneralImages, '') + '' + ImagesInfo.Image6 as Image6,ImagesInfo.Remarks6 from ImagesInfo cross join ImagePathConfig
union all
Select 7,isnull(ImagePathConfig.GeneralImages, '') + '' + ImagesInfo.Image7 as Image7,ImagesInfo.Remarks7 from ImagesInfo cross join ImagePathConfig
");
            Dictionary<string, object> JSResponse = new Dictionary<string, object>();
            if (JSResponse == null)
            {
                JSResponse.Add("Status", false);
            }
            else
            {
                JSResponse.Add("Status", true);
            }

            JSResponse.Add("Message", "Data for All Rows ");
            List<Bannner> lstbanner = new List<Bannner>();
            foreach (DataRow dr in dtproductinfo.Rows)
            {
                if (!string.IsNullOrEmpty(dr["ImageLocation"].ToString()))
                {
                    Bannner objbanner = new Bannner();
                    objbanner.ImageID = int.Parse(dr["ImageID"].ToString());
                    objbanner.ImageLocation = (dr["ImageLocation"].ToString());
                    objbanner.Remarks = (dr["Remarks"].ToString());
                    lstbanner.Add(objbanner);
                }
            }


            JSResponse.Add("Data", lstbanner);

            // List<Dictionary<string, object>> dbrows = GetTableRows(dtproductinfo);


            JsonResult jr = new JsonResult()
            {
                Data = JSResponse,
                ContentType = "application/json",
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
            return jr;
        }
        [ValidateInput(false)]
        public List<Dictionary<string, object>> GetTableRows(DataTable dtData)
        {
            List<Dictionary<string, object>>
            lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow dr in dtData.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in dtData.Columns)
                {
                    if(dr[col] == DBNull.Value)
                    {
                        dictRow.Remove(col.ColumnName);
                    }
                    else
                    {
                        dictRow.Add(col.ColumnName, dr[col]);
                    }
                }
                lstRows.Add(dictRow);
            }
            return lstRows;
        }


        // GET: Images/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Images/Create
        public ActionResult Create()
        {
            Installments.Models.Images objImages = new Images();
           // ViewBag.ProductInfo = new DropDown().GetProductInfo();

            return View(objImages);
        }

        // POST: Images/Create
        [HttpPost]
        public ActionResult Create(Images Images)
        {
            try
            {
                // TODO: Add insert logic here
                if (Images.ImageID == 0)
                {
                    string Query = @"Delete from ImagesInfo";
                   
                    Query = Query + @" insert into ImagesInfo(Terminal)
                                      values(' Web Terminal')";
                    Query = Query + " Select @@IDENTITY as ImageID";
                    Images.ImageID = int.Parse(General.FetchData(Query).Rows[0]["ImageID"].ToString());
                    //if (Query != "")
                    //{
                    //    General.ExecuteNonQuery(Query);
                    //}
                    return Json("true," + Images.ImageID);
                }
                else
                {
                    string Query = @"Update ImagesInfo set Terminal = ' Web Terminal' where ImageID=" + Images.ImageID;
                                               //ProductID='" + Images.ProductID +"'" +
                                               //"Remarks='" + Images.Remarks +
                                               //"' where ImageID=" + Images.ImageID;
                    General.ExecuteNonQuery(Query);
                    return Json("true," + Images.ImageID);
                }
            }
            catch(Exception ex)
            {
                return View();
            }
        }



        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveOrUpdateImage(HttpPostedFileBase txtfile,string Remarks1, HttpPostedFileBase txtfile2, string Remarks2, HttpPostedFileBase txtfile3, string Remarks3,  HttpPostedFileBase txtfile4, string Remarks4, HttpPostedFileBase txtfile5, string Remarks5, HttpPostedFileBase txtfile6, string Remarks6, HttpPostedFileBase txtfile7, string Remarks7)
        {
            if (txtfile != null && txtfile.ContentLength > 0)
            {
                string[] filecontent = txtfile.FileName.ToString().Split(',');
                string filename = filecontent[0] + "-" + filecontent[1] + ".jpeg";
                //string Orderpath2 = filecontent[2] + ".jpeg";
                int ImageID = int.Parse(filecontent[0]);
                string fileext = Path.GetExtension(filename);
                if (fileext == ".jpg" || fileext == ".png" || fileext == ".jpeg")
                {
                    bool exists = Directory.Exists(Server.MapPath("/Images/"));
                    if (!exists)
                    {
                        var createfolder = Path.Combine(Server.MapPath("/Images/").ToString());
                        System.IO.Directory.CreateDirectory(createfolder);

                    }

                    string filepath = ("/Images/") + filename;
                    //string filepath = Server.MapPath("~/ProductImages/")+ filename;
                    string Remarks = Remarks1;
                    string sql = @"update ImagesInfo set Image ='"+filepath+"' , Remarks1 = '"+Remarks+"'where ImageID="+ImageID;
                    // string sql = @"update OrderInfo set OrderImage1='" + filepath + "' OrderImage2='"+ Orderpath1 + "' where OrderID = " + orderID;
                    General.ExecuteNonQuery(sql);
                    txtfile.SaveAs(Server.MapPath(filepath));
                }

            }
            if (txtfile2 != null && txtfile2.ContentLength > 0)
            {
                string[] filecontent = txtfile2.FileName.ToString().Split(',');
                string filename = filecontent[0] + "-" + filecontent[1] + ".jpeg";
                //string Orderpath2 = filecontent[2] + ".jpeg";
                int ImageID = int.Parse(filecontent[0]);
                string fileext = Path.GetExtension(filename);
                if (fileext == ".jpg" || fileext == ".png" || fileext == ".jpeg")
                {
                    string filepath = ("/Images/") + filename;
                    //string filepath = Server.MapPath("~/ProductImages/")+ filename;
                    // string sql = @"insert into ImageInfo (Title,Imagepath,OrderID) values('" + filename + "','" + filepath + "','" + orderID + "')";
                    string Remarks = Remarks2;
                    string sql = @"update ImagesInfo set Image2='" + filepath + "', Remarks2 = '"+Remarks+"' where ImageID = " + ImageID;
                    General.ExecuteNonQuery(sql);
                    txtfile2.SaveAs(Server.MapPath(filepath));
                }

            }
            if (txtfile3 != null && txtfile3.ContentLength > 0)
            {
                string[] filecontent = txtfile3.FileName.ToString().Split(',');
                string filename = filecontent[0] + "-" + filecontent[1] + ".jpeg";
                
                int ImageID = int.Parse(filecontent[0]);
                string fileext = Path.GetExtension(filename);
                if (fileext == ".jpg" || fileext == ".png" || fileext == ".jpeg")
                {
                    string filepath = ("/Images/") + filename;
                    string Remarks = Remarks3;
                    string sql = @"update ImagesInfo set Image3='" + filepath + "', Remarks3 ='"+Remarks+"' where ImageID = " + ImageID;
                    General.ExecuteNonQuery(sql);
                    txtfile3.SaveAs(Server.MapPath(filepath));
                }

            }
            if (txtfile4 != null && txtfile4.ContentLength > 0)
            {
                string[] filecontent = txtfile4.FileName.ToString().Split(',');
                string filename = filecontent[0] + "-" + filecontent[1] + ".jpeg";
                //string Orderpath2 = filecontent[2] + ".jpeg";
                int ImageID = int.Parse(filecontent[0]);
                string fileext = Path.GetExtension(filename);
                if (fileext == ".jpg" || fileext == ".png" || fileext == ".jpeg")
                {
                    string filepath = ("/Images/") + filename;
                    //string filepath = Server.MapPath("~/ProductImages/")+ filename;
                    // string sql = @"insert into ImageInfo (Title,Imagepath,OrderID) values('" + filename + "','" + filepath + "','" + orderID + "')";
                    string Remarks = Remarks4;
                    string sql = @"update ImagesInfo set Image4='" + filepath + "', Remarks4 ='"+Remarks+"' where ImageID = " + ImageID;
                    General.ExecuteNonQuery(sql);
                    txtfile4.SaveAs(Server.MapPath(filepath));
                }

            }
            if (txtfile5 != null && txtfile5.ContentLength > 0)
            {
                string[] filecontent = txtfile5.FileName.ToString().Split(',');
                string filename = filecontent[0] + "-" + filecontent[1] + ".jpeg";
                //string Orderpath2 = filecontent[2] + ".jpeg";
                int ImageID = int.Parse(filecontent[0]);
                string fileext = Path.GetExtension(filename);
                if (fileext == ".jpg" || fileext == ".png" || fileext == ".jpeg")
                {
                    string filepath = ("/Images/") + filename;
                    //string filepath = Server.MapPath("~/ProductImages/")+ filename;
                    // string sql = @"insert into ImageInfo (Title,Imagepath,OrderID) values('" + filename + "','" + filepath + "','" + orderID + "')";
                    string Remarks = Remarks5;
                    string sql = @"update ImagesInfo set Image5='" + filepath + "', Remarks5 ='" + Remarks + "' where ImageID = " + ImageID;
                    General.ExecuteNonQuery(sql);
                    txtfile5.SaveAs(Server.MapPath(filepath));
                }

            }
            if (txtfile6 != null && txtfile6.ContentLength > 0)
            {
                string[] filecontent = txtfile6.FileName.ToString().Split(',');
                string filename = filecontent[0] + "-" + filecontent[1] + ".jpeg";
                //string Orderpath2 = filecontent[2] + ".jpeg";
                int ImageID = int.Parse(filecontent[0]);
                string fileext = Path.GetExtension(filename);
                if (fileext == ".jpg" || fileext == ".png" || fileext == ".jpeg")
                {
                    string filepath = ("/Images/") + filename;
                    //string filepath = Server.MapPath("~/ProductImages/")+ filename;
                    // string sql = @"insert into ImageInfo (Title,Imagepath,OrderID) values('" + filename + "','" + filepath + "','" + orderID + "')";
                    string Remarks = Remarks6;
                    string sql = @"update ImagesInfo set Image6='" + filepath + "', Remarks6='" + Remarks + "' where ImageID = " + ImageID;
                    General.ExecuteNonQuery(sql);
                    txtfile6.SaveAs(Server.MapPath(filepath));
                }

            }
            if (txtfile7 != null && txtfile7.ContentLength > 0)
            {
                string[] filecontent = txtfile7.FileName.ToString().Split(',');
                string filename = filecontent[0] + "-" + filecontent[1] + ".jpeg";
                //string Orderpath2 = filecontent[2] + ".jpeg";
                int ImageID = int.Parse(filecontent[0]);
                string fileext = Path.GetExtension(filename);
                if (fileext == ".jpg" || fileext == ".png" || fileext == ".jpeg")
                {
                    string filepath = ("/Images/") + filename;
                    //string filepath = Server.MapPath("~/ProductImages/")+ filename;
                    // string sql = @"insert into ImageInfo (Title,Imagepath,OrderID) values('" + filename + "','" + filepath + "','" + orderID + "')";
                    string Remarks = Remarks7;
                    string sql = @"update ImagesInfo set Image7='" + filepath + "', Remarks7='" + Remarks + "' where ImageID = " + ImageID;
                    General.ExecuteNonQuery(sql);
                    txtfile7.SaveAs(Server.MapPath(filepath));
                }

            }

            return Json("true");
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase imagefile, HttpPostedFileBase imagefile2, HttpPostedFileBase imagefile3, HttpPostedFileBase imagefile4, HttpPostedFileBase imagefile5, HttpPostedFileBase imagefile6, HttpPostedFileBase imagefile7)
        {
            string _FileName = Path.GetFileName(imagefile.FileName);
            string _FileName2 = Path.GetFileName(imagefile2.FileName);
            string _FileName3 = Path.GetFileName(imagefile3.FileName);
            string _FileName4 = Path.GetFileName(imagefile4.FileName);
            string _FileName5 = Path.GetFileName(imagefile5.FileName);
            string _FileName6 = Path.GetFileName(imagefile6.FileName);
            string _FileName7 = Path.GetFileName(imagefile7.FileName);
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

                for (int i = 0; i < files.Count; i++)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/Images/";
                    string filename = Path.GetFileName(Request.Files[i].FileName);

                    HttpPostedFileBase filex2 = files[i];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = filex2.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = filex2.FileName;
                        var roothpath2 = Server.MapPath("/Images/");
                        var filesavepath2 = System.IO.Path.Combine(roothpath2, fname);

                        imagefile2.SaveAs(filesavepath2);
                        //string sql = "insert into OrderInfo(OrderImage1) values ('" + filesavepath + "')";
                        return View("Index");
                    }
                }



                for (int i = 0; i < files.Count; i++)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/Images/";
                    string filename3 = Path.GetFileName(Request.Files[i].FileName);
                    HttpPostedFileBase filex3 = files[i];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = filex3.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = filex3.FileName;
                        var roothpath3 = Server.MapPath("/Images/");
                        var filesavepath3 = System.IO.Path.Combine(roothpath3, fname);

                        imagefile3.SaveAs(filesavepath3);
                        //string sql = "insert into OrderInfo(OrderImage2) values ('" + filesavepath1 + "')";
                        return View("Index");
                    }
                }
                for (int i = 0; i < files.Count; i++)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/Images/";
                    string filename4 = Path.GetFileName(Request.Files[i].FileName);
                    HttpPostedFileBase filex4 = files[i];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = filex4.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = filex4.FileName;
                        var roothpath4 = Server.MapPath("/Images/");
                        var filesavepath4 = System.IO.Path.Combine(roothpath4, fname);

                        imagefile4.SaveAs(filesavepath4);
                        //string sql = "insert into OrderInfo(OrderImage2) values ('" + filesavepath1 + "')";
                        return View("Index");
                    }
                }

                for (int i = 0; i < files.Count; i++)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/Images/";
                    string filename5 = Path.GetFileName(Request.Files[i].FileName);
                    HttpPostedFileBase filex5 = files[i];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = filex5.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = filex5.FileName;
                        var roothpath5 = Server.MapPath("/Images/");
                        var filesavepath5 = System.IO.Path.Combine(roothpath5, fname);

                        imagefile5.SaveAs(filesavepath5);
                        //string sql = "insert into OrderInfo(OrderImage2) values ('" + filesavepath1 + "')";
                        return View("Index");
                    }
                }
                for (int i = 0; i < files.Count; i++)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/Images/";
                    string filename6 = Path.GetFileName(Request.Files[i].FileName);
                    HttpPostedFileBase filex6 = files[i];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = filex6.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = filex6.FileName;
                        var roothpath6 = Server.MapPath("/Images/");
                        var filesavepath6 = System.IO.Path.Combine(roothpath6, fname);

                        imagefile6.SaveAs(filesavepath6);
                        //string sql = "insert into OrderInfo(OrderImage2) values ('" + filesavepath1 + "')";
                        return View("Index");
                    }
                }
                for (int i = 0; i < files.Count; i++)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/Images/";
                    string filename7 = Path.GetFileName(Request.Files[i].FileName);
                    HttpPostedFileBase filex7 = files[i];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = filex7.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = filex7.FileName;
                        var roothpath7 = Server.MapPath("/Images/");
                        var filesavepath7 = System.IO.Path.Combine(roothpath7, fname);

                        imagefile7.SaveAs(filesavepath7);
                        //string sql = "insert into OrderInfo(OrderImage2) values ('" + filesavepath1 + "')";
                        return View("Index");
                    }
                }
            }
            return Json("Image Uploaded");
        }

        // GET: Images/Edit/5
        public ActionResult Edit(int id)
        {
            DataTable dt = General.FetchData(@"Select * from ImagesInfo inner join ProductInfo on 

ImagesInfo.ProductID=ProductInfo.ProductID where ImageID ="+id);
            
            if(dt.Rows.Count>0)
            {
                Installments.Models.Images objImages = new Images();
                 objImages.ImageID = int.Parse(dt.Rows[0]["ImageID"].ToString());
                objImages.ProductID = int.Parse(dt.Rows[0]["ProductID"].ToString());
                objImages.ImagePath = (dt.Rows[0]["Image"].ToString());
                objImages.ImagePath2 = (dt.Rows[0]["Image2"].ToString());
                objImages.ImagePath3 = (dt.Rows[0]["Image3"].ToString());
                objImages.ImagePath4 = (dt.Rows[0]["Image4"].ToString());
                objImages.ImagePath5 = (dt.Rows[0]["Image5"].ToString());
                objImages.ImagePath6 = (dt.Rows[0]["Image6"].ToString());
                objImages.ImagePath7 = (dt.Rows[0]["Image7"].ToString());
                ViewBag.ProductInfo = new DropDown().GetProductInfo("",objImages.ProductID);
                return View("Create", objImages);

            }
            return View();
        }

        // POST: Images/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]

        public JsonResult Delete(int id)
        {
            string Query = "Delete from ImagesInfo where ImageID =" + id;
            General.ExecuteNonQuery(Query);
            return Json("true");
        }

        
    }
}
