using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Installments.Models;


namespace Installments.Controllers
{
    public class ThemeController : Controller
    {

        //private List<CartInfo> listofCartInfo;
        //// GET: Theme
        //public ThemeController()
        //{
        //    listofCartInfo = new List<CartInfo>();
        //}
        //public JsonResult GetCartInfo()
        //{
        //    var CartProductsCookie = Request.Cookies["CartProducts"];
        //    DataTable dtcookie = new DataTable();
        //    if (CartProductsCookie != null)
        //    {
        //        var productIDs = CartProductsCookie.Value;
        //        var ids = productIDs.Split('-');
        //        var pids = ids.ToList();
        //        //var ProductIDS = CartProductsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();
        //        if (pids != null)
        //        {
        //            ViewBag.ids = string.Join(", ", pids);
        //            dtcookie = General.FetchData(@"Select ProductName,Price-((ISNULL(Discount,0)/100)*Price) as Price From ProductInfo Where ProductID IN (" + (ViewBag.ids) + ")");
        //            ViewBag.dtCookie = dtcookie;
        //        }

        //    }
        //    List<Dictionary<string, object>> dbrows = GetTableRows(dtcookie);
        //    return new JsonResult()
        //    {
        //        Data = dbrows,
        //        ContentType = "application/json",
        //        ContentEncoding = System.Text.Encoding.UTF8,
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        //        MaxJsonLength = Int32.MaxValue
        //    };
        //}
        //[ValidateInput(false)]
        //public List<Dictionary<string, object>> GetTableRows(DataTable dtData)
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
        //    return lstRows;
        //}
        public ActionResult Index()
        {
            DataTable dtaddtocart = Addtocart();
            ViewBag.dtcookie = dtaddtocart;
            DataTable dtProduct = General.FetchData(@"Select * From ProductInfo");
            ViewBag.dtProduct = dtProduct;
            string query = @"SELECT        ProductInfo.ProductID, ProductInfo.ProductCode, ProductInfo.ProductName, ProductInfo.AlternateTitle, ProductInfo.Price, ProductInfo.InActive, ProductInfo.Description, ProductInfo.Discount, ProductInfo.ProductImage, 
                         ProductInfo.CatagoryID, ProductInfo.PurchasePrice, ProductInfo.DefaultVariant,ImageInfo.Imagepath, ProductInfo.Discount, CategoryInfo.CategoryTitle
FROM            ProductInfo INNER JOIN
                         CategoryInfo ON ProductInfo.CatagoryID = CategoryInfo.CategoryID inner join ImageInfo on ProductInfo.ProductID=ImageInfo.ProductID ";
            System.Data.DataTable dt = General.FetchData(query);
            if (dt.Rows.Count > 0)
            {
                ViewBag.Product = new ProductInfoController().DataTableToObject(dt)[0];
            }
            DataTable dtCategory = General.FetchData(@"SELECT        CategoryID, CategoryTitle, Inactvie, CategoryInfo.Description, ISNULL(ParentCategory, 0) As ParentCategory
FROM CategoryInfo");
            return View(dtCategory);
        }
        // GET: Theme/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: Theme/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Theme/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: Theme/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: Theme/Edit/5
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
        // GET: Theme/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: Theme/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                DataTable dtaddtocart = Addtocart();
                ViewBag.dtcookie = dtaddtocart;
                return View();
            }
        }
        public ActionResult ShopList(int? id)
        {
            Addtocart();
            if (id == null)
            {
                return RedirectToAction("Index", "Theme");
            }
            else
            {
                DataTable dtaddtocart = Addtocart();
                ViewBag.dtcookie = dtaddtocart;
                string query = @"SELECT        ProductInfo.ProductID, ProductInfo.ProductCode, ProductInfo.ProductName, ProductInfo.AlternateTitle, ProductInfo.Price, ProductInfo.InActive, ProductInfo.Description, ProductInfo.Discount, ProductInfo.ProductImage, 
                         ProductInfo.CatagoryID, ImageInfo.Imagepath,ProductInfo.PurchasePrice, ProductInfo.DefaultVariant, ProductInfo.Discount, CategoryInfo.CategoryTitle
FROM            ProductInfo INNER JOIN
                         CategoryInfo ON ProductInfo.CatagoryID = CategoryInfo.CategoryID inner join ImageInfo on ProductInfo.ProductID=ImageInfo.ProductID Where ProductInfo.CatagoryID=" + id + "or ProductInfo.ProductID=" + id;
                System.Data.DataTable dt = General.FetchData(query);
                if (dt.Rows.Count > 0)
                {
                    ViewBag.Product = new ProductInfoController().DataTableToObject(dt)[0];
                    System.Data.DataTable dtshoplist = General.FetchData(@"Select * from ProductInfo where CatagoryID = " + dt.Rows[0]["CatagoryID"] + "OR ProductID = " + dt.Rows[0]["ProductID"]);
                    //string sql = @"Select * from ProductInfo where CategoryID = " + id;
                    return View(dtshoplist);
                }
            }
            return View("Index");
        }
        public ActionResult ProductList(int? id)
        {
            //var CartProductsCookie = Request.Cookies["CartProducts"];

            //if (CartProductsCookie != null)
            //{
            //    var productIDs = CartProductsCookie.Value;
            //    var ids = productIDs.Split('-');
            //    var pids = ids.ToList();

            //    //var ProductIDS = CartProductsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();
            //    if (pids != null)
            //    {
            //        ViewBag.ids = string.Join(", ", pids);
            //        DataTable dtcookie = General.FetchData(@"Select * From ProductInfo Where ProductID IN (" + (ViewBag.ids) + ")");
            //        ViewBag.dtCookie = dtcookie;
            //    }
            //    else
            //    {
            //        return View();
            //    }
            //}
            
            if (id == null)
            {
                return RedirectToAction("Index", "Theme");
            }
            if (id != 0)
            {
                DataTable dtaddtocart = Addtocart();
                ViewBag.dtcookie = dtaddtocart;
                string query = @"Select * From ProductInfo Inner Join CategoryInfo On ProductInfo.CatagoryID= CategoryInfo.CategoryID
                                   Inner Join ImageInfo On ProductInfo.ProductID= ImageInfo.ProductID Where ProductInfo.ProductID=" + id;
                DataTable dt = General.FetchData(query);
                if (dt.Rows.Count > 0)
                {
                    ViewBag.Product = new ProductInfoController().DataTableToObject(dt)[0];
                    System.Data.DataTable dtproductlist = General.FetchData(@"Select * From ProductInfo where CatagoryID = " + dt.Rows[0]["CatagoryID"]);
                    return View(dtproductlist);
                }
            }
            return View("Index");
        }
        //public ActionResult Cart()
        //{
        //    var CartProductsCookie = Request.Cookies["CartProducts"];

        //    if (CartProductsCookie != null)
        //    {
        //        var productIDs = CartProductsCookie.Value;
        //        var ids = productIDs.Split(',');

        //        //var ProductIDS = CartProductsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();
        //        if (ids != null)
        //        {
        //            DataTable dtcookie = General.FetchData(@"Select * From ProductInfo Where ProductID IN=" + (ids));
        //            ViewBag.dtCookie = dtcookie;

        //        }
        //        else
        //        {
        //            return View();
        //        }
        //    }

        //    return View();
        //}
        public ActionResult About()
        {
            DataTable dtaddtocart = Addtocart();
            ViewBag.dtcookie = dtaddtocart;
            return View();
        }
        public ActionResult faq()
        {
            DataTable dtaddtocart = Addtocart();
            ViewBag.dtcookie = dtaddtocart;
            return View();
        }
        public ActionResult notfound()
        {
            DataTable dtaddtocart = Addtocart();
            ViewBag.dtcookie = dtaddtocart;
            return View();
        }

        public ActionResult login()
        {
            DataTable dtaddtocart = Addtocart();
            ViewBag.dtcookie = dtaddtocart;
            return View();
        }
        public ActionResult signup()
        {
            DataTable dtaddtocart = Addtocart();
            ViewBag.dtcookie = dtaddtocart;
            return View();
        }

        public ActionResult termandcondition()
        {
            DataTable dtaddtocart = Addtocart();
            ViewBag.dtcookie = dtaddtocart;
            return View();
        }

        public ActionResult contactus()
        {
            DataTable dtaddtocart = Addtocart();
            ViewBag.dtcookie = dtaddtocart;
            return View();
        }
        public ActionResult Checkout()
        {
            //var CartProductsCookie = Request.Cookies["CartProducts"];

            //if (CartProductsCookie != null)
            //{
            //    var productIDs = CartProductsCookie.Value;
            //    var ids = productIDs.Split('-');
            //    var pids = ids.ToList();

            //    //var ProductIDS = CartProductsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();
            //    if (pids != null)
            //    {
            //        ViewBag.ids = string.Join(", ", pids);
            //        DataTable dtcookie = General.FetchData(@"Select * From ProductInfo Where ProductID IN (" + (ViewBag.ids) + ")");
            //        ViewBag.dtCookie = dtcookie;
            //    }
            //    else
            //    {
            //        return View();
            //    }
            //}

            DataTable dtaddtocart = Addtocart();
            ViewBag.dtcookie = dtaddtocart;

            return View();
        }

        public DataTable Addtocart()
        {
            DataTable dtcookie = new DataTable();
            var CartProductsCookie = Request.Cookies["CartProducts"];

            if (CartProductsCookie != null)
            {
                var productIDs = CartProductsCookie.Value;
                var ids = productIDs.Split('-');
                var pids = ids.ToList();

                //var ProductIDS = CartProductsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();
                if (pids != null)
                {
                    ViewBag.ids = string.Join(", ", pids);
                     dtcookie = General.FetchData(@"Select * From ProductInfo Where ProductID IN (" + (ViewBag.ids) + ")");
                    ViewBag.dtcookie = dtcookie; 
                }
                
            }
            //return RedirectToAction("Index", "Theme", dtcookie);
            return dtcookie;
        }
    }
    

}
