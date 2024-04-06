using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;

namespace Installments.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult PageNotAllowed()
        {
            return View();
        }
        public ActionResult Index()
        {
            if (Request.Cookies["UserID"]== null)
            {
                return RedirectToAction("Login");
            }
            DataTable dt = General.FetchData("Select CompanyName from CompanyInformation");
            if(dt.Rows.Count>0)
            {
                ViewBag.CompanyName = dt.Rows[0]["CompanyName"];
                General.CompanyName = dt.Rows[0]["CompanyName"].ToString();
            }
            else
            {
                ViewBag.CompanyName = "Please Enter Company Name";
            }
//            ViewBag.GetOrder = General.FetchData("Select count(*) as Total from SaleInfo where Status=0").Rows[0]["Total"];
//            ViewBag.GetConfirmed = General.FetchData("Select Count(*) as Total from SaleInfo where Status=1").Rows[0]["Total"];
//            ViewBag.GetDispatch = General.FetchData("Select count(*) as Total from SaleInfo where Status = 2").Rows[0]["Total"];
//            ViewBag.GetDelivered = General.FetchData("Select Count(*) as Total from SaleInfo where Status=3").Rows[0]["Total"];
//            DataTable dtdetail = General.FetchData(@"SELECT
//    Top(5) ProductInfo.ProductName,ProductInfo.ProductID,ProductInfo.ProductCode
//    FROM ProductInfo
//        INNER JOIN (SELECT
//                        SaleDetail.ProductID, COUNT(SaleDetail.ProductID) AS CountOf
//                        FROM SaleDetail
//                        GROUP BY SaleDetail.ProductID 
//                    ) SaleDetail ON ProductInfo.ProductID=SaleDetail.ProductID Order by SaleDetail.ProductID asc
//");
//            ViewBag.DtDetail = dtdetail;
//            DataTable dtPreviousSales = new DataTable();
//            ViewBag.PreviousSale = dtPreviousSales = GetPreviousetenDaySalesAmount();

//            List<Models.DataPoint> dataPoints = new List<Models.DataPoint>();

//            foreach (DataRow dr in dtPreviousSales.Rows)
//            {
//                dataPoints.Add(new Models.DataPoint(DateTime.Parse(dr["SalesDate"].ToString()).ToString("dd-MMM-yyyy"), decimal.Parse(dr["TotalAmount"].ToString())));
//            }
//            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();
        }
        public ActionResult Login()
        {
            Models.Login objlogin = new Models.Login();
            ViewBag.Message = "";
            return View(objlogin);  
        }
        public ActionResult Logout()
        {
            var UserName = Request.Cookies["UserName"].Value.ToString();
            int UserID = int.Parse(Request.Cookies["UserID"].Value.ToString());
            Response.Cookies["UserID"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["IsSuperAdmin"].Expires = DateTime.Now.AddDays(-1);
            new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Logout, GeneralAPIsController.LogSource.Logout, UserID, " Log out with Username " + UserName + " at " + DateTime.Now);
            return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult Login(Models.Login objlogin)
        {
            if(objlogin.UserName is null && objlogin.Password is null)
            {
                ViewBag.Message = "Please Enter User Name and Password";
                return View("Login", objlogin);
            }
            if(objlogin.UserName is null)
            {
                ViewBag.Message = "Please Enter User Name";
                return View("Login", objlogin);
            }
            if(objlogin.Password is null)
            {
                ViewBag.Message = "Please Enter Password";
                return View("Login", objlogin);
            }
            string SQL = @"Select * from UserInfo where username like '"+objlogin.UserName.Trim().Replace("'","")+ "' and Password='" + objlogin.Password.Trim().Replace("'", "") + "' and Inactive like 0";
            System.Data.DataTable CurrentUser = General.FetchData(SQL);
            if (CurrentUser.Rows.Count == 0)
            {
                ViewBag.Message = "Invalid User Name or Password";
                return View("Login",objlogin);
            }
            else
            {
                General.StartUpSettings();
                Response.Cookies["UserID"].Value = CurrentUser.Rows[0]["userid"].ToString();
                Response.Cookies["UserName"].Value = CurrentUser.Rows[0]["username"].ToString();
                Response.Cookies["IsSuperAdmin"].Value = CurrentUser.Rows[0]["IsSuperAdmin"].ToString();
                General.userID = int.Parse(Response.Cookies["UserID"].Value);
                new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Login, GeneralAPIsController.LogSource.Login, int.Parse(CurrentUser.Rows[0]["UserID"].ToString()), " Log In with Username " + CurrentUser.Rows[0]["UserName"].ToString() + " at " + DateTime.Now);
                return RedirectToAction("index");
            }
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        DataTable GetPreviousetenDaySalesAmount()
        {
            string SQL = @" Select CAST(SaleDate AS Date) As SalesDate,SUM(SaleDetail.Quantity* SaleDetail.Price) AS TotalAmount from SaleInfo
inner join SaleDetail on SaleInfo.SaleID=SaleDetail.SaleID Where SaleDate>=DATEADD(Day,-10,GETDATE()) and Status=3
group by CAST(SaleDate AS Date)
order by CAST(SaleDate AS Date) desc";
            DataTable Sales = General.FetchData(SQL);
            return Sales;
        }
    }
}