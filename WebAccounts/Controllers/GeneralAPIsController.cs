using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Installments.Models;
namespace Installments.Controllers
{
    public class GeneralAPIsController : UserDetailController
    {
        // GET: GeneralAPIs
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetEmployeesMonthlyDetails()
        {
            string sql1 = $@"
WITH MatchedAttendance AS (
    SELECT 
        ei.Name,
        ea_in.EmployeeID,
        ea_in.TimeIn,
        ea_out.TimeOut,
        DATEDIFF(MINUTE, ea_in.TimeIn, ea_out.TimeOut) AS WorkingMinutes
    FROM 
        [dbo].[EmployeeAttendance] ea_in
    INNER JOIN 
        [dbo].[EmployeeAttendance] ea_out ON ea_in.EmployeeID = ea_out.EmployeeID
            AND ea_in.TimeIn <= ea_out.TimeOut
            AND DATEDIFF(DAY, ea_in.TimeIn, ea_out.TimeOut) = 0
            AND ea_in.AttendanceType = 1
            AND ea_out.AttendanceType = 2
    INNER JOIN 
        [dbo].[EmployeeInfo] ei ON ei.EmployeeID = ea_in.EmployeeID
    WHERE 
        ea_in.TimeIn BETWEEN DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0) AND EOMONTH(GETDATE())
        AND DATEPART(WEEKDAY, ea_in.TimeIn) <> 1  -- Exclude Sundays
)
, MonthlyDays AS (
    SELECT 
        COUNT(*) AS TotalWorkingDaysInMonth
    FROM 
        (SELECT DISTINCT CAST(ea.TimeIn AS DATE) AS WorkingDate
         FROM [dbo].[EmployeeAttendance] ea
         WHERE 
             ea.TimeIn BETWEEN DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0) AND EOMONTH(GETDATE())
             AND DATEPART(WEEKDAY, ea.TimeIn) <> 1) AS DistinctDates
)
, TotalMonthDays AS (
    SELECT 
        DAY(EOMONTH(GETDATE())) AS TotalMonthDays
)
, WorkingHours AS (
    SELECT 
        Name,
        EmployeeID,
        SUM(WorkingMinutes) AS TotalMinutes,
        COUNT(DISTINCT CAST(TimeIn AS DATE)) AS TotalWorkingDays
    FROM 
        MatchedAttendance
    GROUP BY 
        Name, EmployeeID
)

SELECT 
    wh.Name,
    wh.TotalWorkingDays,
    wh.TotalMinutes / 60 AS TotalHours,
    wh.TotalMinutes % 60 AS TotalMinutes,
    CAST((wh.TotalWorkingDays * 1.0 / tm.TotalMonthDays) * 100 AS DECIMAL(5, 2)) AS WorkingDaysPercentage
FROM 
    WorkingHours wh
CROSS JOIN 
    TotalMonthDays tm;
";
            // Assuming General.FetchData() method retrieves data from the database
            DataTable dtproductinfo = General.FetchData(sql1);
            List<Dictionary<string, object>> dbrows = GetProductRows(dtproductinfo);
            Dictionary<string, object> JSResponse = new Dictionary<string, object>();
            if (dbrows == null)
            {
                JSResponse.Add("Status", false);
            }
            else
            {
                JSResponse.Add("Status", true);
            }
            JSResponse.Add("Message", "Employee Monthly Detail");
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
        public ActionResult GetEmployeesSelectList(string whereclause = "")
        {

            DataTable dtEmployee = General.FetchData("Select EmployeeId,Name from EmployeeInfo " + whereclause);
            List<Dictionary<string, object>> dbrows = GetTableRows(dtEmployee);
            return Json(dbrows, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetWarehouses(string BranchID)
        {
            if(string.IsNullOrEmpty(BranchID))
            {
                BranchID = "0";
            }
            DataTable dtWarehouse = General.FetchData("Select WarehouseID,Warehousetitle from Warehouseinfo where branchid= " + BranchID);
            List<Dictionary<string, object>> dbrows = GetTableRows(dtWarehouse);
            return Json(dbrows, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustomerOperatorNumber(int CustomerID)
        {
            DataTable dtEmployee = General.FetchData($@"Select OperatorID,(EmployeeName+'-'+EmployeePhone+'-'+Name+'-'+CASE 
           WHEN IsContactPerson = 1 THEN 'Yes'
           ELSE 'No'
       END)OperatorName from EmployeePhone
inner join OperatorDesignation on OperatorDesignation.Id = DesignationID
Where CustomerID= {CustomerID}" );
            List<Dictionary<string, object>> dbrows = GetTableRows(dtEmployee);
            return Json(dbrows, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetCustomerOperatorNumber(int CustomerID)
        //{
        //    DataTable dtEmployee = General.FetchData($@"Select CustomerID,EmployeeName,EmployeePhone from EmployeePhone Where CustomerID = {CustomerID}");
        //    List<Dictionary<string, object>> dbrows = GetTableRows(dtEmployee);
        //    return Json(dbrows, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult CheckCompanyTitle(string companyTitle)
        {
            try
            {
                DataTable dt = General.FetchData($@"SELECT *  FROM CustomerInfo WHERE ltrim(rtrim(CustomerCompanytitle)) like ltrim(rtrim('%{companyTitle}%')) ");
                bool exists = dt.Rows.Count > 0;

                return Json(new { exists = exists });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
        public ActionResult CheckForms(string formTitle)
        {
            try
            {
                DataTable dt = General.FetchData($@"SELECT FormTitle FROM FormInfo WHERE LTRIM(RTRIM(FormTitle)) LIKE LTRIM(RTRIM('%{formTitle}%'))");

                // Extract form titles from DataTable
                List<string> foundFormTitles = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    foundFormTitles.Add(row["FormTitle"].ToString());
                }

                return Json(new { exists = foundFormTitles.Count > 0, formTitles = foundFormTitles });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }



        public ActionResult GetFranchiseSelectList(string whereclause = "")
        {

            DataTable dtEmployee = General.FetchData("Select FranchiseTitle,FranchiseID from FranchiseInfo " + whereclause);
            List<Dictionary<string, object>> dbrows = GetTableRows(dtEmployee);
            return Json(dbrows, JsonRequestBehavior.AllowGet);
        }
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

        public ActionResult GetPayment(string PaymentType)
        {
            DataTable dtdata = General.FetchData($"SELECT Amount FROM PaymentType WHere ID = {PaymentType}");
            string amount = "0";
            if (dtdata.Rows.Count > 0)
            {
                amount = (dtdata.Rows[0]["Amount"].ToString());
            }
            return Json(amount, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetAreaSelectList(string whereclause = "")
        {

            DataTable dtEmployee = General.FetchData("Select AreaID,AreaTitle from AreaInfo  " + whereclause);
            List<Dictionary<string, object>> dbrows = GetTableRows(dtEmployee);
            return Json(dbrows, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult GetSalebySaleNo(string SaleNo)
        //{

        //    DataTable dtSaleInfo = General.FetchData("Select SaleID from SaleInfo Where SaleNo like '" + SaleNo.Trim().Replace("'", "") + "'");
        //    if (dtSaleInfo.Rows.Count > 0)
        //    {
        //        //SalesInfo si = GetSalesByID(int.Parse(dtSaleInfo.Rows[0]["SaleID"].ToString()));
        //        //return Json(si);
        //    }
        //    else
        //    {
        //        return Json("0");
        //    }
        //}

        public ActionResult GetProductStock(string branch,string productid)
        {
            if(branch=="" || branch is null)
            {
                return Json("0");
            }
            if (productid == "" || branch is null)
            {
                return Json("0");
            }
            DataTable dtSaleInfo = General.FetchData("exec GetProductStock "+branch+", "+productid);
            if (dtSaleInfo.Rows.Count > 0)
            {
                string Quantity = dtSaleInfo.Rows[0]["Quantity"].ToString();
                return Json(Quantity);
            }
            else
            {
                return Json("0");
            }
        }
        public string GenerateVoucheno(int voucherTypeID, int branchid)
        {
            string Query = @"Select Prefix+'-'+(Select BranchCode from BranchInformation where BranchID=" + branchid + @")+'-'+(
Select CAST(count(*)+1 as nvarchar(10)) from AccountVoucherInfo  where VoucherTypeId = " + voucherTypeID + " and BranchID = " + branchid + ") as Vno from VoucherType where VoucherTypeID = " + voucherTypeID + "";
            string vno = General.FetchData(Query).Rows[0]["Vno"].ToString();
            return vno;
        }
        public string GetAccountNo(string ParentAccountID, string Nature)
        {
            string AccountNo = "";
            if (Nature == 1.ToString())
            {

                string SQL = @"
Declare @AnyChildsExist int
Select @AnyChildsExist = Count(*) From chartofAccount Where PArentAccountID = " + ParentAccountID + @" and Nature = 1
if @AnyChildsExist = 0
Begin
    select ( AccountNo + 1) AS AccountNo From chartofAccount Where AccountID = " + ParentAccountID + @"
    --if Child Does not Exist Assign it a Number,that is 1 greater than Parent Account No
    End
else 
Begin
   select ( MAX(AccountNo) + 1) AS AccountNo from chartofaccount where PArentAccountID = " + ParentAccountID + @" and Nature = 1
      --if Child Does Exist Assign it a Number, that is 1 greater than Max Number of That Group
       End 
";
                AccountNo = General.FetchData(SQL).Rows[0]["AccountNo"].ToString();
            }
            else
            {


                string SQL = @"begin
                Declare @ParentaccountNo int
                DEclare @PArentAccountLEvel int
                Select @ParentAccountno = Accountno,@PArentAccountLEvel = AccountLEvel from ChartofAccount Where ACcountID = " + ParentAccountID + @"
                Declare @PreFix nvarchar(50)
Select @PRefix = (Cast((Select Count(*) + 1 from chartofAccount Where PArentAccountID = " + ParentAccountID + @" and Nature = 0) as nvarchar(10))+RIGHT(CAST(@parentAccountNo AS NVARCHAR(10)), 6 - (@PArentAccountLEvel + 1)))
Select ( @ParentAccountNo + CAST(@PreFix as int)) AccountNo
end
";
                AccountNo = General.FetchData(SQL).Rows[0]["AccountNo"].ToString();
            }

            return AccountNo;



        }

        public Decimal COGS(int ProductID)
        {
            decimal COGS = 0;

            DataTable dt = General.FetchData("Exec ProductCOGS " +ProductID);
            if(dt.Rows.Count>0)
            {
                COGS = decimal.Parse(dt.Rows[0]["COGS"].ToString());
            }
            return COGS;
        }


        public enum LogTypes
        {
            New,
            Edit,
            Delete,
            Logout,
            Login,
            APICall
        }
        public enum LogSource
        {
            Customer,
            Company,
            RightsRole,
            Logout,
            Login,
            NewProblemStatement,
            ProblemSolution,
            UserInfo,
            PaymentStatus,
            TasksType,
            WorkStatus,
            WorkCategory,
            PaymentType,
            OperatorDesignation
        }
        public DateTime PakistanStandardTime()
        {
            DateTime date1 = DateTime.UtcNow;

            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");

            DateTime date2 = TimeZoneInfo.ConvertTime(date1, tz);
            return date2;
        }
        public bool InsertLog(LogTypes Log, LogSource Source, int SourceID, string LogDescription)
        {
            try
            {
                string Query = "Insert into Logs(LogType,LogDateTime,LogTerminal,LogUserID,Source,SourceID,LogDescription)";
                Query = Query + "Values('" + (int)Log + "','" + PakistanStandardTime() + "','Web Terminal'," + General.userID + ",'" + Source.ToString() + "'," + SourceID + ",'" + LogDescription + "')";
                General.ExecuteNonQuery(Query);
                return true;
            }
            catch
            {
                //Error Inserting Log
                return false;
            }
        }
    }
}