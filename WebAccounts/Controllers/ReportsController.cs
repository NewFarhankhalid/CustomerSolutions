using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Installments.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Logs(DateTime? DateFrom, DateTime? DateTo, int? Source, int? UserID, string LogSourceID)
        {
            ViewBag.DateFrom = "";
            ViewBag.DateTo = "";
            if (DateFrom is null)
            {
                DateFrom = DateTime.Now.AddDays(-1);
            }

            if (DateTo is null)
            {
                DateTo = DateTime.Now;
            }
            if (Source == 3)
            {
                Source = null;
            }
            
            string sql = @"SELECT  * FROM logs 
WHERE LogDateTime <= '" + DateTo + @"' 
AND LogDateTime >= '" + DateFrom + "' " + (Source == null ? "" : "and LogType =" + Source + @"") + @" 
" + (UserID == null ? "" : "and LogUserID =" + UserID + @"") + @" ";
            if (LogSourceID != null)
            {
                sql = sql + " and  source like '" + LogSourceID + "'";
            }
            sql = sql + "     Order by LogDateTime desc";
            DataTable dt = General.FetchData(sql);
            DataTable dtUsers = General.FetchData(" select userid,username from userinfo ");
            ViewBag.DtUsers = dtUsers;
            ViewBag.GetLogSource = new DropDown().GetLogSource();
            ViewBag.GetUser = new DropDown().GetUserList();
            ViewBag.logSourceIDDropDown = new DropDown().GetDDLSource();
            //ViewBag.TotalValue = TopValue;
            ViewBag.DateFrom = DateTime.Parse(DateFrom.ToString()).ToString("dd/MM/yyyy");
            ViewBag.DateTo = DateTime.Parse(DateTo.ToString()).ToString("dd/MM/yyyy");
            return View(dt);
        }
    }
}