using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Installments.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index(DateTime? DateFrom, DateTime? DateTo, int? UserID)
        {
            string sql = $@" Select NewProblemStatement.*,CustomerInfo.CustomerCompanytitle,TasksType.Name,EmployeePhone.EmployeeName from NewProblemStatement
inner join CustomerInfo on NewProblemStatement.CustomerID = CustomerInfo.CustomerID
inner join TasksType on TasksType.Id = WorkPriority
inner join EmployeePhone on EmployeePhone.OperatorID =NewProblemStatement.OperatorID ";
            DataTable dt = General.FetchData(sql);
            ViewBag.GetUser = new DropDown().GetUserList();
            return View(dt);
        }
    }
}