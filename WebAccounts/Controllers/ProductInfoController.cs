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
    public class ProductInfoController : Controller
    {
        // GET: ProductInfo
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetAllProducts()
        {
            DataTable dtproductinfo = General.FetchData(@"SELECT ProductInfo.ProductID, ProductInfo.ProductCode, ProductInfo.ProductName, ProductInfo.AlternateTitle, ProductInfo.Price, ProductInfo.InActive, ProductInfo.Description, ProductInfo.ProductImage, 
                         ProductInfo.CatagoryID, ProductInfo.PurchasePrice, ProductInfo.DefaultVariant, ProductInfo.Discount, CategoryInfo.CategoryTitle
FROM            ProductInfo INNER JOIN
                         CategoryInfo ON ProductInfo.CatagoryID = CategoryInfo.CategoryID ");
            List<Dictionary<string, object>> dbrows = GetTableRows(dtproductinfo);
            //Dictionary<string, object> JSResponse = new Dictionary<string, object>();
            //if(JSResponse==null)
            //{
            //    JSResponse.Add("Status", false);
            //}
            //else
            //{
            //    JSResponse.Add("Status", true);
            //}
            //JSResponse.Add("Message", "Data for All Products ");
            //JSResponse.Add("Data", dbrows);
            //JsonResult jr = new JsonResult()

            return new JsonResult()
            {
                Data = dbrows,
                ContentType = "application/json",
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
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
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }
            return lstRows;
        }

        [HttpGet]
        public JsonResult GetProducts(int? ProductID,int? CategoryID,int? NoofProducts,int? variantID,int? StartingPrice,int? EndingPrice,int? Ampere,int? ProductType)
        {
            string sql1;
           
                sql1 = sql1 = (@"Select "+(NoofProducts==null?"":"Top("+NoofProducts+ @")")+ @"ProductInfo.ProductID,ProductInfo.Productname,1 As Quantity,ProductInfo.Price, isnull(imagepathconfig.productimagepath,'')+''+ProductInfo.ProductImage as ProductImage,ProductInfo.Discount,ProductInfo.CatagoryID,
CategoryInfo.CategoryTitle ,Ampere,ProductInfo.ForUps, ProductInfo.Description,ProductInfo.ProductType,ProductInfo.Dimension,ProductInfo.NoofPlates,
ProductInfo.Weight,ProductInfo.TotalCapacity,ProductInfo.SolarCapacity,ProductInfo.BatteryChargerCapacity,ProductInfo.PMax,ProductInfo.VMax,ProductInfo.IMP,ProductInfo.ISC,ProductInfo.OCV,ProductInfo.MSV,
ProductInfo.TemperatureOperatingfrom,ProductInfo.TemperatureOperatingTo,VariantsInfo.VariantTitle,TermsandconditionInfo.TermsTitle,VariantsInfo.VariantID from ProductInfo
inner join CategoryInfo on ProductInfo.CatagoryID=CategoryInfo.CategoryID
left Outer join ProductTandCAssociation on ProductInfo.ProductID=ProductTandCAssociation.ProductID
left outer join VariantsInfo on ProductInfo.DefaultVariant=VariantsInfo.VariantID
Left outer join TermsandconditionInfo on ProductTandCAssociation.TermsandConditionID=TermsandconditionInfo.TermsandConditionID
cross join imagepathconfig where productinfo.available=1 ");
          
            if (ProductID != null)
            {
                sql1 = sql1 + " and productInfo.productID =" + ProductID;
            }
            if (CategoryID != null)
            {
                sql1 = sql1 + " and productInfo.CatagoryId=" + CategoryID;
            }
            //if(TagsID != null)
            //{
            //    sql1 = sql1 + " and ProductTagAssociation.TagsID=" + TagsID;
            //}
            if(variantID != null)
            {
                sql1 = sql1 + " and ProductInfo.DefaultVariant=" + variantID;
            }
            if (ProductType != null)
            {
                sql1 = sql1 + " and ProductInfo.ProductType=" + ProductType;
            }
            if (StartingPrice!=null && StartingPrice >= 0 )
            {
                sql1 = sql1 + " and Price >= "+StartingPrice;
            }
            if(EndingPrice!=null &&EndingPrice>=0)
            {
                sql1 = sql1 + " and Price <= " + EndingPrice;
            }
            if(Ampere!=null && Ampere>=0)
            {
                sql1 = sql1 + @" and ProductInfo.Ampere in (select Min(Ampere) from ProductInfo
Where Ampere > "+Ampere+@"
union all
select Max(Ampere) from ProductInfo
Where Ampere < "+Ampere+@"
union all
Select "+Ampere+")";
            }
            sql1 = sql1 + " order by ProductInfo.ProductPriority asc";
            DataTable dtproductinfo = General.FetchData(sql1);
            List<Dictionary<string, object>> dbrows = GetProductRows(dtproductinfo);
            // List<Dictionary<string, object>> dbrow = GetProduct(dtproductinfo);
            Dictionary<string, object> JSResponse = new Dictionary<string, object>();
            if (JSResponse == null)
            {
                JSResponse.Add("Status", false);
            }
            else
            {
                JSResponse.Add("Status", true);
            }
            JSResponse.Add("Message", "Data for All Products ");
            JSResponse.Add("Data",dbrows);

            JsonResult jr= new JsonResult()
            {
                Data = JSResponse,
                ContentType = "application/json",
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
            return jr;
        }


        [HttpGet]
        public JsonResult GetAmperes()
        {
            string sql1;
          
                sql1 = (@"Select VariantID,VariantTitle from variantsinfo
Where InActive=0");
           
            DataTable dtproductinfo = General.FetchData(sql1);
            List<Dictionary<string, object>> dbrows = GetProductRows(dtproductinfo);
            // List<Dictionary<string, object>> dbrow = GetProduct(dtproductinfo);
            Dictionary<string, object> JSResponse = new Dictionary<string, object>();
            if (JSResponse == null)
            {
                JSResponse.Add("Status", false);
            }
            else
            {
                JSResponse.Add("Status", true);
            }
            JSResponse.Add("Message", "Data for All Products ");
            JSResponse.Add("Data", dbrows);

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
        //public List<Dictionary<string, object>> GetProduct(DataTable dtData)
        //{
        //    List<Dictionary<string, object>>
        //    lstRows = new List<Dictionary<string, object>>();
        //    Dictionary<string, object> dictRow = null;

        //    foreach (DataRow dr in dtData.Rows)
        //    {
        //        dictRow = new Dictionary<string, object>();
        //        foreach (DataColumn col in dtData.Columns)
        //        {
        //            dictRow.Add(col.ColumnName, dr[col]);
        //        }
        //        lstRows.Add(dictRow);
        //    }
        //    return (lstRows);
        //}
        [ValidateInput(false)]
        public List<Dictionary<string, object>> GetProductRows(DataTable dtData)
        {
            List<Dictionary<string, object>>
            lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow dr in dtData.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in dtData.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }
            return (lstRows);
        }
        [HttpGet]
        public ActionResult GetCategory()
        {
            DataTable dtproductinfo = General.FetchData(@"Select CategoryInfo.CategoryID,CategoryInfo.CategoryTitle,CategoryInfo.Inactvie,CategoryInfo.Description,CategoryInfo.ParentCategory
,isnull(imagepathconfig.categoryimagepath,'')+''+CategoryInfo.CategoryImage as CategoryImage from CategoryInfo cross join imagepathconfig");
            List<Dictionary<string, object>> dbrows = GetCategoryRows(dtproductinfo);
            Dictionary<string, object> JSResponse = new Dictionary<string, object>();
            if (JSResponse == null)
            {
                JSResponse.Add("Status", false);
            }
            else
            {
                JSResponse.Add("Status", true);
            }
            JSResponse.Add("Message", "Data for All Category");
            JSResponse.Add("Data", dbrows);

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
        public List<Dictionary<string, object>> GetCategoryRows(DataTable dtData)
        {
            List<Dictionary<string, object>>
            lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow dr in dtData.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in dtData.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }
            return lstRows;
        }


        [HttpGet]
        public ActionResult GetTags()
        {
            DataTable dtproductinfo = General.FetchData(@"Select * from TagsInfo");
            List<Dictionary<string, object>> dbrows = GettagsRows(dtproductinfo);
            Dictionary<string, object> JSResponse = new Dictionary<string, object>();
            if (JSResponse == null)
            {
                JSResponse.Add("Status", false);
            }
            else
            {
                JSResponse.Add("Status", true);
            }
            JSResponse.Add("Message", "Data for All Tags ");
            JSResponse.Add("Data", dbrows);


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
        public List<Dictionary<string, object>> GettagsRows(DataTable dtData)
        {
            List<Dictionary<string, object>>
            lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow dr in dtData.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in dtData.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }
            return lstRows;
        }

        // GET: ProductInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductInfo/Create
        public ActionResult Create()
        {
            ViewBag.Color = new DropDown().GetColour();
            ViewBag.Tags = new DropDown().GetTags();
            ViewBag.Size = new DropDown().GetSize();
            ViewBag.Variants = new DropDown().GetVariants();
            ViewBag.CatagoryList = new DropDown().GetProductCategoryList();
            ViewBag.TandCInfo = new DropDown().GetTandCInfo();
            ViewBag.ProductType = new DropDown().GetProductType();
            ViewBag.Status = false;
            ProductInfo p = new ProductInfo();
            p.ProductCode = General.FetchData(@" Select MAX(cast(ProductCode as int )) +1 as ProductCode from ProductInfo").Rows[0]["ProductCode"].ToString();
            return View(p);
        }
        [HttpPost]
        public ActionResult AddCategory(string categorytitle)
        {
            try
            {
                string Query = "Insert into CategoryInfo (CategoryTitle,Inactvie,Description)";
                Query = Query + "Values ('" + categorytitle.Trim().Replace("'","") + "'," + "0" + ",'')";
                Query = Query + " Select @@IDENTITY as CategoryID";
               DataTable dt= General.FetchData(Query);
                return Json(dt.Rows[0]["CategoryID"]);
            }
            catch
            {
                return View();
            }
        }


        [HttpPost]
        public ActionResult AddTandC(string Termstitle)
        {
            try
            {
                string Query = "Insert into TermsandconditionInfo (TermsTitle,Inactive,TermsDescription)";
                Query = Query + "Values ('" + Termstitle.Trim().Replace("'", "") + "'," + "0" + ",'')";
                Query = Query + " Select @@IDENTITY as TermsandConditionID";
                DataTable dt = General.FetchData(Query);
                return Json(dt.Rows[0]["TermsandConditionID"]);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult AddColor(string colortitle,string colorRGBcode)
        {
            try
            {
                string Query = "Insert into ColourInfo (ColourTitle,ColorRGBCode,Inactvie,Description)";
                Query = Query + "Values ('" + colortitle.Trim().Replace("'", "") + "','"+colorRGBcode.Trim().Replace("'","")+"'," + "0" + ",'')";
                Query = Query + " Select @@IDENTITY as ColourID";
                DataTable dt = General.FetchData(Query);
                return Json(dt.Rows[0]["ColourID"]);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult AddTags(string tagtitle)
        {
            try
            {
                string Query = "Insert into TagsInfo (TagTitle,Inactvie,Description)";
                Query = Query + "Values ('" + tagtitle.Trim().Replace("'", "") + "'," + "0" + ",'')";
                Query = Query + " Select @@IDENTITY as TagsID";
                DataTable dt = General.FetchData(Query);
                return Json(dt.Rows[0]["TagsID"]);
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult AddSize(string Sizetitle)
        {
            try
            {
                string Query = "Insert into SizeInfo (SizeTitle,Inactvie,Description)";
                Query = Query + "Values ('" + Sizetitle.Trim().Replace("'", "") + "'," + "0" + ",'')";
                Query = Query + " Select @@IDENTITY as SizeID";
                DataTable dt = General.FetchData(Query);
                return Json(dt.Rows[0]["SizeID"]);
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult newvariant(string varianttitle)
        {
            try
            {
                string Query = "Insert into VariantsInfo (Title,ShortTitle,Measurement,inactive,remarks)";
                Query = Query + "Values ('" + varianttitle.Trim().Replace("'", "") + "','" + varianttitle.Trim().Replace("'", "") + "',1," + "0" + ",'')";
                Query = Query + " Select @@IDENTITY as VariantID";
                DataTable dt = General.FetchData(Query);
                return Json(dt.Rows[0]["VariantID"]);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult newcolor(string colortitle , string colorrgbcode)
        {
            try
            {
                string Query = "Insert into ColorInfo (ColourTitle,ColourRGBCode,inactive,Description)";
                Query = Query + "Values ('" + colortitle.Trim().Replace("'", "") + "','" + colorrgbcode.Trim().Replace("'", "") + "',1," + "0" + ",'')";
                Query = Query + " Select @@IDENTITY as ColourID";
                DataTable dt = General.FetchData(Query);
                return Json(dt.Rows[0]["ColourID"]);
            }
            catch
            {
                return View();
            }
        }
        public ActionResult newtag(string Tagtitle)
        {
            try
            {
                string Query = "Insert into TagsInfo (TagTitle,inactive,Description)";
                Query = Query + "Values ('" + Tagtitle.Trim().Replace("'", "") + "','" + Tagtitle.Trim().Replace("'", "") + "',1," + "0" + ",'')";
                Query = Query + " Select @@IDENTITY as TagsID";
                DataTable dt = General.FetchData(Query);
                return Json(dt.Rows[0]["TagsID"]);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult newcsize(string sizetitle)
        {
            try
            {
                string Query = "Insert into SizeInfo (SizeTitle,inactive,Description)";
                Query = Query + "Values ('" + sizetitle.Trim().Replace("'", "") + "','" + sizetitle.Trim().Replace("'","") + "',1," + "0" + ",'')";
                Query = Query + " Select @@IDENTITY as SizeID";
                DataTable dt = General.FetchData(Query);
                return Json(dt.Rows[0]["SizeID"]);
            }
            catch
            {
                return View();
            }
        }


        public bool CheckifProductcodeExist(int productid, string productcode)
        {
            int count = int.Parse(General.FetchData(" Select Count(*) As C from ProductInfo where productcode like '" + productcode + "' and ProductID != " + productid).Rows[0]["c"].ToString());

            if (count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        // POST: ProductInfo/Create
        [HttpPost]
        public ActionResult GetVariantMeasurement(int variantid)
        {
            string sql = "Select * from VariantsInfo where VariantID=" + variantid;

            DataTable dtVariant = General.FetchData(sql);
            if (dtVariant.Rows.Count > 0)
            {
                return Json("" + dtVariant.Rows[0]["Measurement"].ToString());
            }
            return Json("1");
        }
        [HttpPost]
        public ActionResult GetRGBColour(int colorid)
        {
            string sql = "Select * from ColourInfo where ColourID=" + colorid;

            DataTable dtColor = General.FetchData(sql);
            if (dtColor.Rows.Count > 0)
            {
                return Json("" + dtColor.Rows[0]["ColourRGBCode"].ToString());
            }
            return Json("1");
        }
        [HttpPost]
        public ActionResult GetSizeInfo(int sizeid)
        {
            string sql = "Select * from SizeInfo where SizeID=" + sizeid;
            DataTable dtSize = General.FetchData(sql);
            return Json("1");
        }
        [HttpPost]
        public ActionResult GetTagsInfo(int Tagsid)
        {
            string sql = "Select * from TagsInfo where TagsID=" + Tagsid;
            DataTable dttags = General.FetchData(sql);
            return Json("1");
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveOrUpdateImage(HttpPostedFileBase txtfile)
        {
            if (txtfile != null && txtfile.ContentLength > 0)
            {
                string[] filecontent = txtfile.FileName.ToString().Split(',');
                string filename = filecontent[0]+"-"+ filecontent[1] + ".jpeg";
                int productid = int.Parse(filecontent[0]);
                string fileext = Path.GetExtension(filename);
                if (fileext == ".jpg" || fileext == ".png" || fileext == ".jpeg")
                {
                    bool exists = Directory.Exists(Server.MapPath("/ProductImages/"));
                    if (!exists)
                    {
                        var createfolder = Path.Combine(Server.MapPath("/ProductImages/").ToString());
                        System.IO.Directory.CreateDirectory(createfolder);
                        exists = true;
                    }
                    string filepath = ("/ProductImages/") + filename;
                    //string filepath = Server.MapPath("~/ProductImages/")+ filename;
                    string sql = @"update ProductInfo set ProductImage='"+filepath+"'  where ProductInfo.ProductID="+ productid+"";
                    //string sql = @"Update ProductInfo set ProductImage='/ProductImages/"+ filename + "' where ProductID=" + productid;
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
                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "~/ProductsImages/";  
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
                        var roothpath = Server.MapPath("/ProductImages");
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
        [HttpPost]
        public ActionResult Create(ProductInfo objproduct, List<ProductVariantAssociation> lstProductVariantAssociation, List<ProductBarcodes> lstBarcode ,  List<ProductColorAssociation> lstProductColorAssociation, List<ProductSizeAssociation> lstProductSizeAssociation,List<ProductTagAssociation> lstproductTagAssociation, List<ProductTandCAssociation> lstTandCAssociation)
        {
            try
            {
               
                if (CheckifProductcodeExist(objproduct.ProductID, objproduct.ProductCode))
                {
                    return Json("rcode,");
                }
                if (lstProductVariantAssociation == null)
                {
                    lstProductVariantAssociation = new List<ProductVariantAssociation>();
                }
                if (lstBarcode == null)
                {
                    lstBarcode = new List<ProductBarcodes>();
                }
                if (lstProductColorAssociation == null)
                {
                    lstProductColorAssociation = new List<ProductColorAssociation>();
                }
                if (lstProductSizeAssociation == null)
                {
                    lstProductSizeAssociation = new List<ProductSizeAssociation>();
                }
                if(lstproductTagAssociation==null)
                {
                    lstproductTagAssociation = new List<ProductTagAssociation>();
                }
                if(lstTandCAssociation==null)
                {
                    lstTandCAssociation = new List<ProductTandCAssociation>();
                }
                if (objproduct.ProductID == 0)
                {
                            string Query = "Insert into ProductInfo (CatagoryID,ProductCode,ProductName,InActive,ForUPS,AlternateTitle,Description,Price,SalesAccountID,PurchaseAccountID,PurchasePrice,DefaultVariant,Discount,Ampere,ProductType,Dimension,NoofPlates,Weight,TotalCapacity,SolarCapacity,BatteryChargerCapacity,PMax,VMax,IMP,ISC,OCV,MSV,TemperatureOperatingfrom,TemperatureOperatingTo) ";
                            Query = Query + "Values ('" + objproduct.CatagoryID + "','" + objproduct.ProductCode + "',N'" + objproduct.ProductName + "'," + (objproduct.InActive == true ? "1" : "0") + "," + (objproduct.UPS == true ? "1" : "0") + ",N'" + objproduct.AlternateTitle + "',N'" + objproduct.Description + "','" + objproduct.Price + "'," + objproduct.SalesAccountID + "," + objproduct.PurchaseAccountID + "," + objproduct.PurchasePrice + "," + objproduct.DefaultVariant + ","+objproduct.Discount+",'"+objproduct.Ampere+ "'," + objproduct.ProductType + ",'" + objproduct.Dimension + "'," + objproduct.NoofPlates + "," + objproduct.Weight + "," + objproduct.TotalCapacity + "," + objproduct.SolarCapacity + "," + objproduct.BatteryChargerCapacity + "," + objproduct.PMax + "," + objproduct.VMax + "," + objproduct.IMP + "," + objproduct.ISC + "," + objproduct.OCV + "," + objproduct.MSV + "," + objproduct.TemperatureOperatingfrom + "," + objproduct.TemperatureOperatingTo + ")";
                            Query = Query + " Select @@IDENTITY as ProductID";
                            objproduct.ProductID = int.Parse(General.FetchData(Query).Rows[0]["ProductID"].ToString());
                            Query = "";
                            foreach (ProductVariantAssociation ass in lstProductVariantAssociation)
                            {
                                Query = Query + " Insert into ProductVariantAssociation(ProductID,VariantID,Measurement) Values(" + objproduct.ProductID + "," + ass.VariantId + "," + ass.Measurement + ")";
                            }

                            foreach (ProductBarcodes barcode in lstBarcode)
                            {
                                Query = Query + " Insert into ProductBarcode(ProductID,Barcode,Qty) Values(" + objproduct.ProductID + "," + barcode.Barcode + "," + barcode.Qty + ")";
                            }
                            foreach (ProductColorAssociation color in lstProductColorAssociation)
                            {
                                Query = Query + "Insert into ColourProductAssociation(ProductID,ColourID) Values(" + objproduct.ProductID + "," + color.ColorID + ")";
                            }
                            foreach (ProductSizeAssociation Size in lstProductSizeAssociation)
                            {
                                Query = Query + "Insert into SizeProductAssociation(ProductID,SizeID) values(" + objproduct.ProductID + "," + Size.SizeID + ")";
                            }
                            foreach (ProductTagAssociation Tag in lstproductTagAssociation)
                            {
                                Query = Query + "Insert into ProductTagAssociation(ProductID,TagsID) values(" + objproduct.ProductID + "," + Tag.TagsID + ")";
                            }
                            foreach(ProductTandCAssociation TandC in lstTandCAssociation)
                            {
                                Query = Query + "Insert into ProductTandCAssociation(ProductID,TermsandConditionID) Values(" + objproduct.ProductID+","+TandC.TandCID+")";
                            }

                            if (Query != "")
                            {
                                General.ExecuteNonQuery(Query);
                            }
                    return Json("true," + objproduct.ProductID);
                }
                else
                {
                    string Query = "";
                    Query = Query + "UPDATE [dbo].[ProductInfo] ";
                    Query = Query + " SET    [ProductName] =N'" + objproduct.ProductName + "' ";
                    Query = Query + "    ,[InActive] = " + (objproduct.InActive == true ? "1" : "0") + "";
                    Query = Query + "    ,[ForUps] = " + (objproduct.UPS == true ? "true" : "false") + "";
                    Query = Query + "    ,[ProductCode] = '" + objproduct.ProductCode + "' ";
                    Query = Query + "    ,[AlternateTitle] = N'" + objproduct.AlternateTitle + "' ";
                    Query = Query + "    ,[Description] =N'" + objproduct.Description + "' ";
                    Query = Query + "    ,[Price] =" + objproduct.Price + " ";
                    Query = Query + "    ,[PurchasePrice] =" + objproduct.PurchasePrice + " ";
                    Query = Query + "    ,[Discount] =" + objproduct.Discount + " ";
                    Query = Query + "    ,[Ampere]=" + objproduct.Ampere + " ";
                    Query = Query + "    ,[CatagoryID] =" + objproduct.CatagoryID + "";
                    //Query = Query + "  ,[ProductImage] ='" + objproduct.ProductImage + "' ";
                    Query = Query + "    ,[SalesAccountID] =" + objproduct.SalesAccountID + " ";
                    Query = Query + "    ,[PurchaseAccountID] =" + objproduct.PurchaseAccountID + " ";
                    Query = Query + "    ,[DefaultVariant] =" + objproduct.DefaultVariant+ " ";
                    Query = Query + "    ,[ProductType] =" + objproduct.ProductType + " ";
                    Query = Query + "    ,[Dimension] ='" + objproduct.Dimension + "' ";
                    Query = Query + "    ,[NoofPlates] =" + objproduct.NoofPlates + " ";
                    Query = Query + "    ,[Weight] =" + objproduct.Weight + " ";
                    Query = Query + "    ,[TotalCapacity] =" + objproduct.TotalCapacity + " ";
                    Query = Query + "    ,[SolarCapacity] =" + objproduct.SolarCapacity + " ";
                    Query = Query + "    ,[BatteryChargerCapacity] =" + objproduct.BatteryChargerCapacity + " ";
                    Query = Query + "    ,[PMax] =" + objproduct.PMax + " ";
                    Query = Query + "    ,[VMax] =" + objproduct.VMax + " ";
                    Query = Query + "    ,[IMP] =" + objproduct.IMP + " ";
                    Query = Query + "    ,[ISC] =" + objproduct.ISC + " ";
                    Query = Query + "    ,[OCV] =" + objproduct.OCV + " ";
                    Query = Query + "    ,[MSV] =" + objproduct.MSV + " ";
                    Query = Query + "    ,[TemperatureOperatingfrom] =" + objproduct.TemperatureOperatingfrom + " ";
                    Query = Query + "    ,[TemperatureOperatingTo] =" + objproduct.TemperatureOperatingTo + " ";
                    Query = Query + "WHERE ProductID=" + objproduct.ProductID;
                    General.ExecuteNonQuery(Query);
                    Query = " Delete from ProductVariantAssociation  where productid="+objproduct.ProductID+ "    Delete from ProductBarcode  where productid=" + objproduct.ProductID+ " Delete from SizeProductAssociation  where productid=" + objproduct.ProductID+" Delete from ColourProductAssociation where productid = " + objproduct.ProductID+"Delete from ProductTagAssociation where productid="+objproduct.ProductID+ "Delete from ProductTandCAssociation where productid="+objproduct.ProductID;


                    foreach (ProductVariantAssociation ass in lstProductVariantAssociation)
                    {
                        Query = Query + " Insert into ProductVariantAssociation(ProductID,VariantID,Measurement) Values(" + objproduct.ProductID + "," + ass.VariantId + "," + ass.Measurement + ")";
                    }
                    foreach (ProductBarcodes barcode in lstBarcode)
                    {
                        Query = Query + " Insert into ProductBarcode(ProductID,Barcode,Qty) Values(" + objproduct.ProductID + "," + barcode.Barcode + "," + barcode.Qty + ")";
                    }
                    foreach (ProductColorAssociation col in lstProductColorAssociation)
                    {
                        Query = Query + " Insert into ColourProductAssociation(ProductID,ColourID) Values(" + objproduct.ProductID + "," + col.ColorID + ")";
                    }
                    foreach (ProductSizeAssociation siz in lstProductSizeAssociation)
                    {
                        Query = Query + " Insert into SizeProductAssociation(ProductID,SizeID) Values(" + objproduct.ProductID + "," + siz.SizeID + ")";
                    }
                    foreach (ProductTagAssociation tag in lstproductTagAssociation)
                    {
                        Query = Query + " Insert into ProductTagAssociation(ProductID,TagsID) Values(" + objproduct.ProductID + "," + tag.TagsID + ")";
                    }

                    foreach(ProductTandCAssociation tandC in lstTandCAssociation )
                    {
                        Query = Query + "Insert into ProductTandCAssociation(ProductID,TermsandConditionID) Values(" + objproduct.ProductID + "," + tandC.TandCID + ")";
                    }
                    General.ExecuteNonQuery(Query);

                }
                return Json("true," + objproduct.ProductID);

            }
            catch
            {
                return Json("error");
            }
        }

        // GET: ProductInfo/Edit/5
        public ActionResult Edit( int id)
        {
            DataTable dtproduct = General.FetchData(@"SELECT        ProductInfo.ProductID, ProductInfo.ProductCode, ProductInfo.ProductName, ProductInfo.AlternateTitle, ProductInfo.Price, ProductInfo.InActive, ProductInfo.Description, ProductInfo.Discount,ProductInfo.ProductImage, 
                         ProductInfo.CatagoryID,ProductInfo.ForUps, ProductInfo.PurchasePrice, ProductInfo.DefaultVariant, ProductInfo.Discount, ProductInfo.Ampere, ProductInfo.*,CategoryInfo.CategoryTitle
FROM            ProductInfo INNER JOIN
                         CategoryInfo ON ProductInfo.CatagoryID = CategoryInfo.CategoryID  Where ProductInfo.ProductID=" + id);
            List<ProductInfo> obj = DataTableToObject(dtproduct);
            if (obj.Count > 0)
            {
                ViewBag.CatagoryList = new DropDown().GetProductCategoryList("", obj[0].CatagoryID);
                ViewBag.Color = new DropDown().GetColour();
                ViewBag.Tags = new DropDown().GetTags();
                ViewBag.Size = new DropDown().GetSize();
                ViewBag.Variants = new DropDown().GetVariants();
                ViewBag.TandCInfo = new DropDown().GetTandCInfo();
                ViewBag.ProductType = new DropDown().GetProductType(obj[0].ProductType);
                ViewBag.Status = true;
                return View("Create", obj[0]);
            }
            return RedirectToAction("index");
        }

        // POST: ProductInfo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ProductInfo objproduct)
        {
            try
            {
                return Json("abc");
            }
            catch
            {
                ViewBag.CatagoryList = new DropDown().GetProductCategoryList("", objproduct.CatagoryID);
                ViewBag.COA = new DropDown().GetTransactionalAccounts();
                return View();
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            string Query = "Delete from ProductInfo Where ProductID=" + id;
            Query = Query +Environment.NewLine+ " Delete from ProductVariantAssociation Where ProductID="+id;
            Query = Query + Environment.NewLine + " Delete from ProductBarcode Where ProductID=" + id;
            Query = Query + Environment.NewLine + "Delete from ColourProductAssociation Where ProductID =" + id;
            Query = Query + Environment.NewLine + "Delete from SizeProductAssociation Where ProductID =" + id;
            Query = Query + Environment.NewLine + "Delete from ProductTagAssociation Where ProductID=" +id;
            Query = Query + Environment.NewLine + "Delete from ProductTandCAssociation Where ProductID=" + id;
            General.ExecuteNonQuery(Query);
            return Json("true");
        }


        public ActionResult MassDiscount()
        {
            DataTable dtProduct = General.FetchData("SELECT ProductID, ProductName,ProductCode,Price, ISNULL(Discount,0) AS Discount FROM ProductInfo");
            return View(dtProduct);
        }

        public ActionResult SaveDiscountPrice(List<StockAdjustmentDetail> lstStockAdjustment)
        {
            if (lstStockAdjustment is null)
            {
                lstStockAdjustment = new List<StockAdjustmentDetail>();
            }
            string SQLQuery = "";
            foreach (StockAdjustmentDetail os in lstStockAdjustment)
            {
                // , DiscountPer = "+os.DiscountPer+"
                SQLQuery = SQLQuery + Environment.NewLine + "  Update ProductInfo set Discount= " + os.DiscountPrice + " , Price="+os.Price+" Where ProductInfo.ProductID= " + os.ProductID;
            }
            General.ExecuteNonQuery(SQLQuery);

            return Json("true");
        }

        //public ActionResult MassPriority()
        //{
        //    DataTable dtProduct = General.FetchData("SELECT ProductID, ProductName,ProductCode,ProductPriority FROM ProductInfo");
        //    return View(dtProduct);
        //}

        //public ActionResult SaveMassPriority(List<StockAdjustmentDetail> lstStockAdjustment)
        //{
        //    if (lstStockAdjustment is null)
        //    {
        //        lstStockAdjustment = new List<StockAdjustmentDetail>();
        //    }
        //    string SQLQuery = "";
        //    foreach (StockAdjustmentDetail os in lstStockAdjustment)
        //    {
        //        SQLQuery = SQLQuery + Environment.NewLine + "Update ProductInfo set ProductPriority = " + os.ProductPriority + " Where ProductInfo.ProductID= " + os.ProductID;
        //    }
        //    General.ExecuteNonQuery(SQLQuery);

        //    return Json("true");
        //}


        public ActionResult MassAvailable()
        {
            DataTable dtProduct = General.FetchData("SELECT ProductID, ProductName,ProductCode,Available,ProductPriority FROM ProductInfo");
            return View(dtProduct);
        }

        public ActionResult SaveMassAvailable(List<StockAdjustmentDetail> lstStockAdjustment)
        {
            if (lstStockAdjustment is null)
            {
                lstStockAdjustment = new List<StockAdjustmentDetail>();
            }
            string SQLQuery = "";
            foreach (StockAdjustmentDetail os in lstStockAdjustment)
            {
                SQLQuery = SQLQuery + Environment.NewLine + "Update ProductInfo set Available = " + os.Available + ",ProductPriority="+os.ProductPriority+" Where ProductInfo.ProductID= " + os.ProductID;
            }
            General.ExecuteNonQuery(SQLQuery);

            return Json("true");
        }
        public  List<ProductInfo> DataTableToObject(DataTable dt)
        {
            List<ProductInfo> lstbranch = new List<ProductInfo>();
            ProductInfo bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new ProductInfo();
                if (dr["ProductID"] != DBNull.Value)
                {
                    bi.ProductID = int.Parse(dr["ProductID"].ToString());
                }
                if (dr["ProductCode"] != DBNull.Value)
                {
                    bi.ProductCode = (dr["ProductCode"].ToString());
                }
                if (dr["ProductName"] != DBNull.Value)
                {
                    bi.ProductName = (dr["ProductName"].ToString());
                }
                if (dr["InActive"] != DBNull.Value)
                {
                    bi.InActive = bool.Parse(dr["InActive"].ToString());
                }
                if(dr["ForUps"]!=DBNull.Value)
                {
                    bi.UPS = bool.Parse(dr["ForUps"].ToString());
                }
                if (dr["AlternateTitle"] != DBNull.Value)
                {
                    bi.AlternateTitle = (dr["AlternateTitle"].ToString());
                }
                if (dr["Description"] != DBNull.Value)
                {
                    bi.Description = (dr["Description"].ToString());
                }
                if (dr["Price"] != DBNull.Value)
                {
                    bi.Price = decimal.Parse(dr["Price"].ToString());
                }
                if (dr["PurchasePrice"] != DBNull.Value)
                {
                    bi.PurchasePrice = decimal.Parse(dr["PurchasePrice"].ToString());
                }
                if(dr["Ampere"]!=DBNull.Value)
                {
                    bi.Ampere = dr["Ampere"].ToString();
                }
                if (dr["Discount"] != DBNull.Value)
                {
                    bi.Discount = decimal.Parse(dr["Discount"].ToString());
                }
                if (dr["ProductImage"] != DBNull.Value)
                {
                    bi.ProductImage = (dr["ProductImage"].ToString());
                    ViewBag.ProductImage = (dr["ProductImage"].ToString());
                }
                if (dr["CatagoryID"] != DBNull.Value)
                {
                    bi.CatagoryID = int.Parse(dr["CatagoryID"].ToString());
                }
                //if (dr["ColourID"] != DBNull.Value)
                //{
                //    bi.ColourID = int.Parse(dr["ColourID"].ToString());
                //}
                //if (dr["SizeID"] != DBNull.Value)
                //{
                //    bi.SizeID = int.Parse(dr["SizeID"].ToString());
                //}
                //if (dr["TagsID"] != DBNull.Value)
                //{
                //    bi.TagsID = int.Parse(dr["TagsID"].ToString());
                //}
                if (dr["DefaultVariant"] != DBNull.Value)
                {
                    bi.DefaultVariant = int.Parse(dr["DefaultVariant"].ToString());
                }
                if (dr["CategoryTitle"] != DBNull.Value)
                {
                    bi.CategoryTitle = (dr["CategoryTitle"].ToString());
                }

                if (dr["ProductType"] != DBNull.Value)
                {
                    bi.ProductType = int.Parse(dr["ProductType"].ToString());
                }
                if (dr["Dimension"] != DBNull.Value)
                {
                    bi.Dimension = (dr["Dimension"].ToString());
                }
                if (dr["NoofPlates"] != DBNull.Value)
                {
                    bi.NoofPlates = int.Parse(dr["NoofPlates"].ToString());
                }
                if (dr["Weight"] != DBNull.Value)
                {
                    bi.Weight = int.Parse(dr["Weight"].ToString());
                }
                if (dr["TotalCapacity"] != DBNull.Value)
                {
                    bi.TotalCapacity = int.Parse(dr["TotalCapacity"].ToString());
                }
                if (dr["SolarCapacity"] != DBNull.Value)
                {
                    bi.SolarCapacity = int.Parse(dr["SolarCapacity"].ToString());
                }
                if (dr["BatteryChargerCapacity"] != DBNull.Value)
                {
                    bi.BatteryChargerCapacity = int.Parse(dr["BatteryChargerCapacity"].ToString());
                }
                if (dr["PMax"] != DBNull.Value)
                {
                    bi.PMax = int.Parse(dr["PMax"].ToString());
                }
                if (dr["VMax"] != DBNull.Value)
                {
                    bi.VMax = int.Parse(dr["VMax"].ToString());
                }
                if (dr["IMP"] != DBNull.Value)
                {
                    bi.IMP = int.Parse(dr["IMP"].ToString());
                }
                if (dr["ISC"] != DBNull.Value)
                {
                    bi.ISC = int.Parse(dr["ISC"].ToString());
                }
                if (dr["OCV"] != DBNull.Value)
                {
                    bi.OCV = int.Parse(dr["OCV"].ToString());
                }
                if (dr["MSV"] != DBNull.Value)
                {
                    bi.MSV = int.Parse(dr["MSV"].ToString());
                }
                if (dr["TemperatureOperatingfrom"] != DBNull.Value)
                {
                    bi.TemperatureOperatingfrom = int.Parse(dr["TemperatureOperatingfrom"].ToString());
                }
                if (dr["TemperatureOperatingTo"] != DBNull.Value)
                {
                    bi.TemperatureOperatingTo = int.Parse(dr["TemperatureOperatingTo"].ToString());
                }

                bi.lstProductColorAssociations = ProductColorAssociation(bi.ProductID);
                bi.lstProductSizeAssociations = ProductSizeAssociation(bi.ProductID);
                bi.lstProductBarcodes = Barcodes(bi.ProductID);
                bi.lstProductVariantAssociation = ProductVariantAssociation(bi.ProductID);
                bi.lstproductTagAssociation = ProductTagAssociation(bi.ProductID);
                bi.lstTandCAssociation = ProductTandCAssociation(bi.ProductID);
                lstbranch.Add(bi);

            }
            return lstbranch;

        }
        List<ProductBarcodes> Barcodes(int ProductID)
        {
            List<ProductBarcodes> lstproductbarcodes = new List<ProductBarcodes>();
            DataTable dtBarcodes = General.FetchData("Select * from ProductBarcode Where ProductID=" + ProductID);

            foreach (DataRow dr in dtBarcodes.Rows)
            {
                ProductBarcodes br = new ProductBarcodes();
                br.ProductID = ProductID;
                br.Barcode = (dr["Barcode"].ToString());
                br.Qty = int.Parse(dr["Qty"].ToString());

                lstproductbarcodes.Add(br);

            }

            return lstproductbarcodes;

        }
        List<ProductVariantAssociation> ProductVariantAssociation(int ProductID)
        {
            List<ProductVariantAssociation> lstproductVariantAssociation = new List<ProductVariantAssociation>();
            DataTable dtVariant = General.FetchData("Select productVariantAssociation.*,VariantTitle from productVariantAssociation inner join VariantsInfo on ProductVariantAssociation.VariantID=VariantsInfo.VariantID  Where ProductID=" + ProductID);
            foreach (DataRow dr in dtVariant.Rows)
            {
                ProductVariantAssociation pva = new ProductVariantAssociation();
                pva.ProductID = ProductID;
                pva.Measurement = decimal.Parse(dr["Measurement"].ToString());
                pva.VariantId = int.Parse(dr["VariantID"].ToString());
                pva.VariantTitle = (dr["VariantTitle"].ToString());
                lstproductVariantAssociation.Add(pva);
            }
            return lstproductVariantAssociation;
        }

        List<ProductColorAssociation> ProductColorAssociation(int ProductID)
        {
            List<ProductColorAssociation> lstproductColorAssociation = new List<ProductColorAssociation>();
            DataTable dtColor = General.FetchData("Select ColourProductAssociation.*,ColourInfo.ColourTitle,ColourRGBCode from ColourProductAssociation inner join ColourInfo on ColourProductAssociation.ColourID=ColourInfo.ColourID Where ProductID=" + ProductID);
            foreach (DataRow dr in dtColor.Rows)
            {
                ProductColorAssociation pva = new ProductColorAssociation();
                pva.ProductID = ProductID;
                
                pva.ColorID = int.Parse(dr["ColourID"].ToString());
                pva.ColorTitle = (dr["ColourTitle"].ToString());
                pva.RGBCode = (dr["ColourRGBCode"].ToString());

                lstproductColorAssociation.Add(pva);
            }
            return lstproductColorAssociation;
        }
        List<ProductSizeAssociation> ProductSizeAssociation(int ProductID)
        {
            List<ProductSizeAssociation> lstproductSizeAssociation = new List<ProductSizeAssociation>();
            DataTable dtSize = General.FetchData("Select SizeProductAssociation.*,SizeTitle from SizeProductAssociation inner join SizeInfo on SizeProductAssociation.SizeID=SizeInfo.SizeID Where ProductID=" + ProductID);
            foreach (DataRow dr in dtSize.Rows)
            {
                ProductSizeAssociation pva = new ProductSizeAssociation();
                pva.ProductID = ProductID;

                pva.SizeID = int.Parse(dr["SizeID"].ToString());
                pva.SizeTitle = (dr["SizeTitle"].ToString());
                lstproductSizeAssociation.Add(pva);
            }
            return lstproductSizeAssociation;
        }

        List<ProductTandCAssociation> ProductTandCAssociation(int ProductID)
        {
            List<ProductTandCAssociation> lstTandCAssociation = new List<ProductTandCAssociation>();
            DataTable dtSize = General.FetchData("Select ProductTandCAssociation.*,TermsTitle from ProductTandCAssociation inner join TermsandconditionInfo on ProductTandCAssociation.TermsandConditionID=TermsandconditionInfo.TermsandConditionID Where ProductID=" + ProductID);
            foreach (DataRow dr in dtSize.Rows)
            {
                ProductTandCAssociation pva = new Models.ProductTandCAssociation();
                pva.ProductID = ProductID;

                pva.TandCID = int.Parse(dr["TermsandConditionID"].ToString());
                pva.TermsTitle = (dr["TermsTitle"].ToString());
                lstTandCAssociation.Add(pva);
            }
            return lstTandCAssociation;
        }

        List<ProductTagAssociation> ProductTagAssociation(int ProductID)
        {
            List<ProductTagAssociation> lstproductTagAssociation = new List<ProductTagAssociation>();
            DataTable dtTag = General.FetchData("Select ProductTagAssociation.*,TagTitle from ProductTagAssociation inner join TagsInfo on ProductTagAssociation.TagsID=TagsInfo.TagsID Where ProductID=" + ProductID);
            foreach (DataRow dr in dtTag.Rows)
            {
                ProductTagAssociation pva = new ProductTagAssociation();
                pva.ProductID = ProductID;

                pva.TagsID = int.Parse(dr["TagsID"].ToString());
                pva.TagTitle = (dr["TagTitle"].ToString());
                lstproductTagAssociation.Add(pva);
            }
            return lstproductTagAssociation;
        }

        public ActionResult InsertBulkProducts(List<ProductInfo> products)
        {
            DataTable dtSaleandPurchaseheadaccounts = General.FetchData("Select CreateAutoSaleAccountUnder,CreateAutoPurchaseAccountUnder,DifferenceInOpeningBalanceAccount,OpeningStockTransactionVoucher,OpeningStockAccount   from AppConfiguration");
            if (dtSaleandPurchaseheadaccounts.Rows.Count == 0)
            {
                return Json(" Please Configure Sales Account Parent Under Configuration");
            }
            int SaleAccountParent = int.Parse(dtSaleandPurchaseheadaccounts.Rows[0]["CreateAutoSaleAccountUnder"].ToString());


            if (General.FetchData(" Select AccountType from ChartofAccount where accountid=" + SaleAccountParent).Rows.Count == 0)
            {
                return Json(" Please Configure Sales Account Parent Under Configuration");
            }

            int SalesAccountType = int.Parse(General.FetchData(" Select AccountType from ChartofAccount where accountid=" + SaleAccountParent).Rows[0]["AccountType"].ToString());
            int SalesAccountLevel = int.Parse(General.FetchData(" Select ISNULL(AccountLevel,0)+1 AS Level from ChartofAccount where accountid=" + SaleAccountParent).Rows[0]["Level"].ToString());


            int PurchaseAccountParent = int.Parse(dtSaleandPurchaseheadaccounts.Rows[0]["CreateAutoPurchaseAccountUnder"].ToString());
            int PurchaseAccountType = int.Parse(General.FetchData(" Select AccountType from ChartofAccount where accountid=" + PurchaseAccountParent).Rows[0]["AccountType"].ToString()); ;
            int PurchaseAccountLevel = int.Parse(General.FetchData(" Select ISNULL(AccountLevel,0)+1 AS Level from ChartofAccount where accountid=" + PurchaseAccountParent).Rows[0]["Level"].ToString()); ;

            if (General.FetchData(" Select AccountType from ChartofAccount where accountid=" + PurchaseAccountParent).Rows.Count == 0)
            {
                return Json(" Please Configure purchase Account Parent Under Configuration");
            }

            int DifferenceInOpeningBalanceAccount = int.Parse("0"+dtSaleandPurchaseheadaccounts.Rows[0]["DifferenceInOpeningBalanceAccount"].ToString());
            if (General.FetchData(" Select AccountType from ChartofAccount where accountid=" + DifferenceInOpeningBalanceAccount).Rows.Count == 0)
            {
                return Json(" Please Configure Difference In Opening Balance Account  first");
            }

            int OpeningStockAccount = int.Parse("0" + dtSaleandPurchaseheadaccounts.Rows[0]["OpeningStockAccount"].ToString());
            if (General.FetchData(" Select AccountType from ChartofAccount where accountid=" + OpeningStockAccount).Rows.Count == 0)
            {
                return Json(" Please Configure Opening Stock account first");
            }

            int OpeningStockTransactionVoucher = int.Parse("0" + dtSaleandPurchaseheadaccounts.Rows[0]["OpeningStockTransactionVoucher"].ToString());
            if (General.FetchData(" Select * from VoucherType where vouchertypeid=" + OpeningStockTransactionVoucher).Rows.Count == 0)
            {
                return Json(" Please Configure Opening Stock Voucher First");
            }



            //Insert Distinct Categories
            try
            {
                List<string> categories = products.Select(o => o.CategoryTitle).Distinct().ToList();
                string SQL = "";
                foreach (string categorie in categories)
                {
                    SQL = SQL + Environment.NewLine + @"if not exists( Select CategoryID from CategoryInfo where CategoryTitle like '" + categorie.Trim().Replace("'", "") + @"')
                               Insert into CategoryInfo(CategoryTitle, Inactvie, Description) Values('" + categorie.Trim().Replace("'", "") + "', 0, '')";
                }
                if (SQL != "")
                {
                    General.ExecuteNonQuerywithtransaction(SQL);
                }
            }
            catch (Exception ex)
            {
                return Json("Error Associating Categories" + ex.InnerException);
            }
            
            //InsertDistinctSaleAccounts
            try
            {
                List<string> saleaccounts = products.Select(o => o.SalesAccountTitle).Distinct().ToList();
                string Query = "";
                foreach (string saleaccount in saleaccounts)
                {
                    //if (General.FetchData("Select AccountID from ChartofAccount where AccountTitle like '" + saleaccount.Trim().Replace("'", "") + "'").Rows.Count == 0)
                    //{
                    //    string AccountNo = new GeneralAPIsController().GetAccountNo(SaleAccountParent.ToString(), 1.ToString());
                    //    Query = Query + Environment.NewLine + "Insert into ChartofAccount (AccountNo,Inactive,AccountType,Nature,AccountTitle,Description,AccountLevel,ParentAccountID) ";
                    //    Query = Query + "Values ('" + AccountNo + "',0," + SalesAccountType + ",1,'" + saleaccount.Trim().Replace("'", "") + "',''," + SalesAccountLevel + "," + SaleAccountParent + ")";

                    //    General.ExecuteNonQuerywithtransaction(Query);
                    //    Query = "";
                    //}
                }
                
            }
            catch (Exception ex)
            {
                return Json("Error Associating Sales Accounts " + ex.InnerException);
            }

            //InsertDistinctPurchaseAccounts
            try
            {

                List<string> purchaseccounts = products.Select(o => o.PurchaseAccountTitle).Distinct().ToList();
                string Query = "";
                foreach (string purchaseaccount in purchaseccounts)
                {
                    //if (General.FetchData("Select AccountID from ChartofAccount where AccountTitle like '"+purchaseaccount.Trim().Replace("'","")+"'").Rows.Count == 0)
                    //{
                    //    string AccountNo = new GeneralAPIsController().GetAccountNo(PurchaseAccountParent.ToString(), 1.ToString());
                    //    Query = "";
                    //    Query = Query + Environment.NewLine + "Insert into ChartofAccount (AccountNo,Inactive,AccountType,Nature,AccountTitle,Description,AccountLevel,ParentAccountID) ";
                    //    Query = Query + "Values ('" + AccountNo + "',0," + PurchaseAccountType + ",1,'" + purchaseaccount.Trim().Replace("'", "") + "',''," + PurchaseAccountLevel + "," + PurchaseAccountParent + ")";
                    //    General.ExecuteNonQuerywithtransaction(Query);
                    //}
                }
        
            }
            catch (Exception ex)
            {
                return Json("Error Associating Purchase Accounts " + ex.InnerException);
            }


            //InsertDistinctBranches
            try
            {

                List<string> branches = products.Select(o => o.Branch).Distinct().ToList();
                string Query = "";
                foreach (string branch in branches)
                {
                    if (General.FetchData("Select BranchID from BranchInformation where BranchTitle like '" + branch.Trim().Replace("'", "") + "'").Rows.Count == 0)
                    {
                        Query = Query + Environment.NewLine + "Insert into BranchInformation (BranchTitle,BranchCode,Inactive,Description,Address,PhoneNo) Select '"+ branch.Trim().Replace("'", "") + "', (Count(*)+1) ,0,'','','' from BranchInformation";
                    }
                }
                if (Query != "")
                {
                    General.ExecuteNonQuerywithtransaction(Query);
                }
            }
            catch (Exception ex)
            {
                return Json("Error Creating Branches " + ex.InnerException);
            }
            //InsertDistinctWarehousesWithrespectofBranches
            try
            {
                string Query = "";
                var warehouses = products.Select(o => new { o.Warehouse,o.Branch}).Distinct().ToList();
                foreach (var warehouse in warehouses)
                {
                    string branchid = General.FetchData("Select BranchID from BranchInformation where BranchTitle like '" + warehouse.Branch.Trim().Replace("'", "") + "'").Rows[0]["BranchID"].ToString();

                    if (General.FetchData("Select WarehouseID from WarehouseInfo where WarehouseTitle like '" + warehouse.Warehouse.Trim().Replace("'", "") + "' and branchid="+branchid).Rows.Count == 0)
                    {
                        Query = Query + Environment.NewLine + "Insert into WarehouseInfo (WarehouseTitle,BranchID,Inactive,Description,Manager) Values('"+warehouse.Warehouse.Trim().Replace("'","")+"',"+branchid+",0,'',0)";
                    }
                }
                if (Query != "")
                {
                    General.ExecuteNonQuerywithtransaction(Query);
                }
            }
            catch (Exception ex)
            {
                return Json("Error Creating Warehous " + ex.InnerException);
            }
            //Associate CategoryId,ProductSaleAccountID and ProductPurchaseAccountID to Products
            DataTable dtCategories = General.FetchData(" Select CategoryID,CategoryTitle from CategoryInfo ");
            DataTable dtTransactionalAccounts = General.FetchData(" Select AccountID, AccountTitle from ChartofAccount Where Nature=1");
            DataTable dtBrances = General.FetchData(" Select BranchID,BranchTitle from BranchInformation");
            DataTable dtWarehouse = General.FetchData(" Select WarehouseTitle,WarehouseID,BranchID from WarehouseInfo ");
            try
            {
                foreach (ProductInfo product in products)
                {
                    DataRow[] drCategory = dtCategories.Select(" CategoryTitle like '" + product.CategoryTitle.Trim().Replace("'", "") + "'");
                    DataRow[] drPurchaseAccount = dtTransactionalAccounts.Select(" Accounttitle like '" + product.PurchaseAccountTitle.Trim().Replace("'", "") + "'");
                    DataRow[] drSaleAccount = dtTransactionalAccounts.Select(" Accounttitle like '" + product.SalesAccountTitle.Trim().Replace("'","") + "'");
                    DataRow[] drwarehouse = dtWarehouse.Select("WarehouseTitle like '"+product.Warehouse.Trim().Replace("'", "") + "'");
                    DataRow[] drBranch = dtBrances.Select("BranchTitle like '" + product.Branch.Trim().Replace("'", "") + "'");
                    if (drCategory.Count() > 0)
                    {
                        product.CatagoryID = int.Parse(drCategory[0]["CategoryID"].ToString());
                    }
                    if (drPurchaseAccount.Count() > 0)
                    {
                        product.PurchaseAccountID = int.Parse(drPurchaseAccount[0]["AccountID"].ToString());
                    }
                    if (drSaleAccount.Count() > 0)
                    {
                        product.SalesAccountID = int.Parse(drSaleAccount[0]["AccountID"].ToString());
                    }
                    if (drwarehouse.Count() > 0)
                    {
                        product.WarehouseID = int.Parse(drwarehouse[0]["WarehouseID"].ToString());
                    }
                    if (drBranch.Count() > 0)
                    {
                        product.BranchID = int.Parse(drBranch[0]["BranchID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Error Asscociating Ids (internal technial issue)"+ex.InnerException);
            }
            //Insert Distinct Products
            var distinctproducts = products.Select(x => new { x.ProductCode, x.ProductName, x.SalesAccountID, x.CatagoryID, x.PurchasePrice, x.Price, x.PurchaseAccountID }).Distinct().ToList();
            try
            {
                string Query = "";
                foreach (var product in distinctproducts)
                {
                    if (General.FetchData(" Select ProductID from ProductInfo Where ProductName like '" + product.ProductName.Trim().Replace("'", "") + "' and ProductCode Like '" + product.ProductCode.Trim().Replace("'", "") + "' and CatagoryID=" + product.CatagoryID).Rows.Count == 0)
                    {
                        Query = Query + Environment.NewLine + "Insert into ProductInfo (CatagoryID,ProductCode,ProductName,InActive,AlternateTitle,Description,Price,ProductImage,SalesAccountID,PurchaseAccountID,PurchasePrice) ";
                        Query = Query + "Values ('" + product.CatagoryID + "','" + product.ProductCode.Trim().Replace("'", "") + "',N'" + product.ProductName.Trim().Replace("'", "") + "',0,N'',N'','" + product.Price + "',''," + product.SalesAccountID + "," + product.PurchaseAccountID + "," + product.PurchasePrice + ")";
                    }
                }
                if (Query != "")
                {
                    General.ExecuteNonQuerywithtransaction(Query);
                }
            }
            catch (Exception ex)
            {
                return Json("Error Inserting Products " + ex.InnerException);
            }

            DataTable dtProducts = General.FetchData("Select ProductCode,ProductID,CatagoryID,ProductName from ProductInfo");
            foreach (ProductInfo p in products)
            {
                DataRow[] productid = dtProducts.Select(" ProductName like '" + p.ProductName.Trim().Replace("'","") + "' and CatagoryID=" + p.CatagoryID + " and ProductCode like '" + p.ProductCode.Trim().Replace("'","") + "'");
                if (productid.Count() > 0)
                {
                    p.ProductID = int.Parse(productid[0]["ProductID"].ToString());
                }
            }
            decimal totalstock = products.Sum(x => x.OpeningStock);
            if(totalstock==0)
            {
                return Json("true");
            }


            string OpeningStockQuery = "";
            var distinctbranchandwarehouse = products.Select(x => new { x.BranchID, x.WarehouseID }).Distinct();

            foreach (var item in distinctbranchandwarehouse)
            {
                OpeningStockQuery = OpeningStockQuery + Environment.NewLine + " Delete from AccountVoucherInfo Where VoucherID In (Select VoucherID from ProductOpeningStock Where BranchID=" + item.BranchID + " and Warehouseid=" + item.WarehouseID + ")";
                OpeningStockQuery = OpeningStockQuery + Environment.NewLine + " Delete from AccountVoucherDetail Where VoucherID In (Select VoucherID from ProductOpeningStock Where BranchID=" + item.BranchID + " and Warehouseid=" + item.WarehouseID + ")";
                OpeningStockQuery = OpeningStockQuery +Environment.NewLine+ " Delete from ProductOpeningStock Where branchid=" + item.BranchID + " and Warehouseid=" + item.WarehouseID;
            }
            foreach (ProductInfo p in products)
            {
                if (p.OpeningStock > 0)
                {
                    OpeningStockQuery = OpeningStockQuery + Environment.NewLine + "  Insert into ProductOpeningStock(BranchId,WarehouseID,ProductID,Qty,Price,VoucherID,EffectiveDate)";
                    OpeningStockQuery = OpeningStockQuery + " Values(" + p.BranchID + "," + p.WarehouseID + "," + p.ProductID + "," + p.OpeningStock + "," + p.PurchasePrice + ",0,'" + p.EffectiveDate + "')";
                }
            }
            General.ExecuteNonQuerywithtransaction(OpeningStockQuery);
            var distinctVoucher = products.Select(x => new { x.BranchID, x.EffectiveDate,x.WarehouseID }).Distinct();
            //try
            //{
            //     foreach (var master in distinctVoucher)
            //    {
            //       decimal TotalAmount= products.Where(x => x.BranchID == master.BranchID && x.EffectiveDate == master.EffectiveDate && x.WarehouseID==master.WarehouseID).ToList().Sum(x=>(x.OpeningStock*x.PurchasePrice));

            //        AccountVoucher objav = new AccountVoucher();
            //        objav.VoucherID = 0;
            //        objav.VoucherDate = master.EffectiveDate;
            //        objav.BranchID = master.BranchID;
            //        objav.Description = "Opening Stock (File Upload)";
            //        objav.VoucherTypeId = OpeningStockTransactionVoucher;
            //        objav.Vno = new GeneralAPIsController().GenerateVoucheno(OpeningStockTransactionVoucher, objav.BranchID);
            //        objav.lstAccountVoucherDetail = new List<AccountVoucherDetail>();
            //        AccountVoucherDetail avd = new AccountVoucherDetail();
            //        avd.AccountID = OpeningStockAccount;
            //        avd.Dr = TotalAmount;
            //        avd.Narration = "Net Stock Amount";
            //        objav.lstAccountVoucherDetail.Add(avd);
            //        avd = new AccountVoucherDetail();
            //        avd.AccountID = DifferenceInOpeningBalanceAccount;
            //        avd.Cr = TotalAmount;
            //        avd.Narration = "Net Difference In Opening Amount";
            //        objav.lstAccountVoucherDetail.Add(avd);
            //        objav = new GeneralAPIsController().InsertVoucher(objav);
            //        General.ExecuteNonQuery("Update  ProductOpeningStock set VoucherID=" + objav.VoucherID + " where Branchid=" +master.BranchID+" and Warehouseid="+master.WarehouseID+"and CAST(EffectiveDate AS Date)='"+ master.EffectiveDate+"'" );
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return Json("Error Posting Account transaction for opening balance"+ex.InnerException);
            //}
            //Insert Opening Stock Now
            return Json("true");
        }
    }
}