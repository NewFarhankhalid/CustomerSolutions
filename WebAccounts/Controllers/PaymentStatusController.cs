using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Installments.Controllers
{
    public class PaymentStatusController : Controller
    {
        // GET: PaymentStatus
        public ActionResult Index(int? CustomerID,DateTime? FromDate, DateTime? ToDate)
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Allowed) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            ViewBag.Customer = new DropDown().GetCustomer();
            ViewBag.PaymentType = new DropDown().GetPaymentMethod(0);
            if (FromDate==null)
            {
                FromDate = DateTime.Now.AddDays(-7);
            }
            if (ToDate == null)
            {
                ToDate = DateTime.Now;
            }
            if(CustomerID==0||CustomerID==null)
            {
                CustomerID = 0;
            }
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            DataTable dtProblem = General.FetchData(@"Select * from PaymentStatus inner join CustomerInfo on PaymentStatus.CustomerID = CustomerInfo.CustomerID  Where 1=1 "+(CustomerID==0?" ":" and CustomerInfo.CustomerID="+CustomerID)+ " and Cast(DateReceived as Date) between Cast('" + FromDate+ "' as Date) and Cast('" + ToDate + "' as Date)");
            List<PaymentStatus> lstProblem = DataTableToObject(dtProblem);
            return View(lstProblem);
        }
        List<PaymentStatus> DataTableToObject(DataTable dt)
        {
            List<PaymentStatus> lstbranch = new List<PaymentStatus>();
            PaymentStatus bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new PaymentStatus();
                if (dr["PaymentStatusID"] != DBNull.Value)
                {
                    bi.PaymentStatusID = int.Parse(dr["PaymentStatusID"].ToString());
                }
                if (dr["CustomerID"] != DBNull.Value)
                {
                    bi.CustomerID = int.Parse(dr["CustomerID"].ToString());
                }
                if (dr["CustomerCompanytitle"] != DBNull.Value)
                {
                    bi.CustomerCompanytitle = (dr["CustomerCompanytitle"].ToString());
                }
                if (dr["AmountReceived"] != DBNull.Value)
                {
                    bi.AmountReceived = int.Parse(dr["AmountReceived"].ToString());
                }
                if (dr["DateReceived"] != DBNull.Value)
                {
                    bi.DateReceived = DateTime.Parse(dr["DateReceived"].ToString());
                }
                if (dr["PaymentType"] != DBNull.Value)
                {
                    bi.PaymentType = int.Parse(dr["PaymentType"].ToString());
                }
                if (dr["Description"] != DBNull.Value)
                {
                    bi.Description = (dr["Description"].ToString());
                }
                lstbranch.Add(bi);
            }
            return lstbranch;
        }
        public ActionResult Create()
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.New) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            Installments.Models.PaymentStatus objProblemInfo = new PaymentStatus();
            ViewBag.Customer = new DropDown().GetCustomer();
            ViewBag.PaymentType = new DropDown().GetPaymentMethod(0);
            objProblemInfo.DateReceived = DateTime.Now;
            return View(objProblemInfo);
        }
        [HttpPost]
        public ActionResult Create(PaymentStatus objProblem)
        {
            try
            {
                if (objProblem.PaymentStatusID == 0)
                {
                    string Query = "Insert into PaymentStatus (CustomerID,AmountReceived,DateReceived,PaymentType,Description) ";
                    Query = Query + "Values (" + objProblem.CustomerID + "," + objProblem.AmountReceived + ",'" + objProblem.DateReceived + "'," + objProblem.PaymentType + ",'" + objProblem.Description + "')";
                    //General.ExecuteNonQuery(Query);
                    //Query = "";
                    Query = Query + " Select @@IDENTITY as PaymentStatusID";
                    objProblem.PaymentStatusID = int.Parse(General.FetchData(Query).Rows[0]["PaymentStatusID"].ToString());
                    string CustomerCompanytitle = General.FetchData("Select CustomerCompanytitle from CustomerInfo Where CustomerID=" + objProblem.CustomerID).Rows[0]["CustomerCompanytitle"].ToString();
                    new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.New, GeneralAPIsController.LogSource.PaymentStatus, objProblem.PaymentStatusID, " Customer Name " + CustomerCompanytitle);
                }
                else
                {
                    string Query = "";
                    Query = Query + "UPDATE [dbo].[PaymentStatus] ";
                    Query = Query + " SET    [CustomerID] =" + objProblem.CustomerID + " ";
                    Query = Query + "    ,[AmountReceived] =" + objProblem.AmountReceived + " ";
                    Query = Query + "    ,[DateReceived] ='" + objProblem.DateReceived + "' ";
                    Query = Query + ",[PaymentType]=" + objProblem.PaymentType + "";
                    Query = Query + ",[Description]='" + objProblem.Description + "'";
                    Query = Query + " WHERE PaymentStatusID=" + objProblem.PaymentStatusID;
                    General.FetchData(Query);
                    string CustomerCompanytitle = General.FetchData("Select CustomerCompanytitle from CustomerInfo Where CustomerID=" + objProblem.CustomerID).Rows[0]["CustomerCompanytitle"].ToString();
                    new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Edit, GeneralAPIsController.LogSource.PaymentStatus, objProblem.PaymentStatusID, " Customer Name " + CustomerCompanytitle);
                }
                return Json("true," + objProblem.PaymentStatusID);
            }
            catch
            {
                return Json("error");
            }
        }
        public ActionResult Edit(int id)
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Edit) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            DataTable dtProbleminfo = General.FetchData(@"Select * from PaymentStatus inner join CustomerInfo on CustomerInfo.CustomerID = PaymentStatus.CustomerID where PaymentStatusID=" + id);
            List<PaymentStatus> lstbranch = DataTableToObject(dtProbleminfo);
            ViewBag.Customer = new DropDown().GetCustomer();
            ViewBag.PaymentType = new DropDown().GetPaymentMethod(0);
            if (lstbranch.Count > 0)
            {
                return View("Create", lstbranch[0]);
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Removed) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            string ProblemTitle = General.FetchData("Select CustomerCompanytitle from CustomerInfo inner join PaymentStatus on CustomerInfo.CustomerID = PaymentStatus.CustomerID Where PaymentStatus.PaymentStatusID="+id).Rows[0]["CustomerCompanytitle"].ToString();
            string Query = "Delete from PaymentStatus Where PaymentStatusID=" + id;
            new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Delete, GeneralAPIsController.LogSource.PaymentStatus, id, " Customer name " + ProblemTitle);
            General.ExecuteNonQuery(Query);
            return Json("true");
        }
    }
}