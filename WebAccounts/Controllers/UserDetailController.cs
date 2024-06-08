using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Installments.Controllers
{
    public class UserDetailController : Controller
    {
        // GET: UserDetail
        [HttpGet]
        public JsonResult GetUserInfo(string UserName, string Password)
        {
            string sql1 = $@"Select UserID,UserName from UserInfo Where UserName COLLATE Latin1_General_CS_AS ='{UserName}' 
and Password COLLATE Latin1_General_CS_AS ='{Password}' and Inactive = 0 and IsSuperAdmin=1";
            DataTable dtproductinfo = General.FetchData(sql1);
            List<Dictionary<string, object>> dbrows = GetProductRows(dtproductinfo);
            Dictionary<string, object> JSResponse = new Dictionary<string, object>();
            if (JSResponse.Count <= 0)
            {
                JSResponse.Add("Status", false);
            }
            else
            {
                JSResponse.Add("Status", true);
            }
            JSResponse.Add("Message", "Data for Login User");
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
        [HttpGet]
        public JsonResult GetEmployeeLogin(string UserName, string Password)
        {
            string sql1 = $@"Select EmployeeID,isnull(AllowHunderdMeters,0)AllowHunderdMeters,Name,isnull(IsAdmin,0)IsAdmin from EmployeeInfo Where UserName COLLATE Latin1_General_CS_AS ='{UserName}' 
and Password COLLATE Latin1_General_CS_AS ='{Password}' ";
            DataTable dtproductinfo = General.FetchData(sql1);
            List<Dictionary<string, object>> dbrows = GetProductRows(dtproductinfo);
            Dictionary<string, object> JSResponse = new Dictionary<string, object>();
            if (dbrows.Count <= 0)
            {
                JSResponse.Add("Status", false);
            }
            else
            {
                JSResponse.Add("Status", true);
            }
            JSResponse.Add("Message", "Data for Login Employee");
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
        [HttpPost]
        public ActionResult AddUserFinger(string FingerID,int EmployeeID)
        {
            try
            {
                string sql = $@"Update EmployeeInfo Set FingerID='{FingerID}' Where EmployeeID={EmployeeID}";
                   General.ExecuteNonQuery(sql);
                return Json("true",JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View("false",JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AddAdminKey(int EmployeeID,string key)
        {
            try
            {
                string sql = $@"Delete from AdminMobileKey Where EmployeeID = {EmployeeID}";
                sql = sql + $@" Insert into AdminMobileKey (EmployeeID,MobileKey) values ({EmployeeID},'{key}')";
                General.ExecuteNonQuery(sql);
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View("false", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult TotalEmployeesDetail()
        {
            string sql1 = $@"Select COUNT(EmployeeID)TotalEmployee,(Select COUNT(EmployeeID)TotalCheckIn from EmployeeAttendance Where AttendanceType = 1 and Cast(TimeIn as date)  = CAST(GETDATE() as date))TotalCheckIn,
(Select COUNT(EmployeeID)TotalCheckOut from EmployeeAttendance Where AttendanceType = 2 and Cast(TimeIn as date)  = CAST(GETDATE() as date))TotalCheckOut
 from EmployeeInfo";
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
            JSResponse.Add("Message", "Total Detail");
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
        [HttpPost]
        public JsonResult TotalCheckDetail(int AttendanceType)
        {
            string sql1 = "";
            if(AttendanceType==1)
            {
                sql1 = sql1 + $@" Select EmployeeAttendance.EmployeeID,Name,Cast(TimeIn as time)TimeIn,Cast(OpenTime as time)OpenTime,
Cast(DATEDIFF(MINUTE ,Cast(TimeIn as time), Cast(OpenTime as time))/60 as nvarchar) as Hours,
Cast(DATEDIFF(MINUTE ,Cast(TimeIn as time), Cast(OpenTime as time))%60 as nvarchar) AS Minutes,
Location
	from EmployeeAttendance
inner join EmployeeInfo on EmployeeAttendance.EmployeeID = EmployeeInfo.EmployeeID
";
            }
            else
            {
                sql1 = sql1 + $@" Select EmployeeAttendance.EmployeeID,Name,Cast(TimeIn as time)TimeOut,Cast(OffTime as time)OffTime,
Cast(DATEDIFF(MINUTE ,Cast(OffTime as time), Cast(TimeIn as time))/60 as nvarchar) as Hours,
Cast(DATEDIFF(MINUTE ,Cast(OffTime as time), Cast(TimeIn as time))%60 as nvarchar) AS Minutes,
Location
	from EmployeeAttendance
inner join EmployeeInfo on EmployeeAttendance.EmployeeID = EmployeeInfo.EmployeeID";
            }
            sql1 = sql1 + $@" Where AttendanceType = {AttendanceType} and Cast(TimeIn as date)  = CAST(GETDATE() as date) ";
 //           string sql1 = $@"  Select EmployeeAttendance.EmployeeID,Name,TimeIn,Location,DeviceName from EmployeeAttendance 
 //inner join EmployeeInfo on EmployeeAttendance.EmployeeID = EmployeeInfo.EmployeeID
 //Where AttendanceType = {AttendanceType} and Cast(TimeIn as date)  = CAST(GETDATE() as date)";
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
            JSResponse.Add("Message", "Total Detail");
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
        [HttpGet]
        public JsonResult GetEmployees()
        {
            string sql1 = $@"Select EmployeeID,Name from EmployeeInfo Where FingerID is null";
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
            JSResponse.Add("Message", "Employee Detail");
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
        [HttpPost]
        public ActionResult AddAttendance(int EmployeeID,string Location,string IPAddress,string MacAddress,string DeviceName,double Longtitude,double Latitude,int AttendanceType)
        {
            try
            {
                string query = $@"Select EmployeeID,'Duplicate Entry' as Name from EmployeeAttendance Where EmployeeID = {EmployeeID} and AttendanceType ={AttendanceType} and Cast(TimeIn as date)= Cast(GetDate() as date) and Cast(TimeOut as date)= Cast(GetDate() as date) ";
                Dictionary<string, object> JSResponse = new Dictionary<string, object>();
                DataTable Value = General.FetchData(query);
                List<Dictionary<string, object>> dbrows = GetProductRows(Value);
                if(Value.Rows.Count>0)
                {
                    JSResponse.Add("Status", false);
                }
                else
                {
                    DataTable dt = General.FetchData($@"Select EmployeeID,Name,''as value from EmployeeInfo Where EmployeeID='{EmployeeID}'");
                    if (dt.Rows.Count > 0)
                    {
                        string sql = $@"
INSERT INTO [dbo].[EmployeeAttendance]
           ([EmployeeID]
           ,[TimeIn]
           ,[TimeOut]
           ,[Location]
           ,[MobileIPAddress]
           ,[MacAddress]
           ,[DeviceName]
           ,[longtitude]
           ,[latitude]
           ,[AttendanceType])
     VALUES
           ({dt.Rows[0]["EmployeeID"]}
           ,GetDate()
           ,GetDate()
           ,'{Location}'
           ,'{IPAddress}'
           ,'{MacAddress}'
           ,'{DeviceName}'
           ,'{Longtitude}'
           ,'{Latitude}'
           ,{AttendanceType})";
                        General.ExecuteNonQuery(sql);
                    }
                    dbrows = GetProductRows(dt);
                    if (dbrows.Count <= 0)
                    {
                        JSResponse.Add("Status", false);
                    }
                    else
                    {
                        JSResponse.Add("Status", true);
                    }
                }
                //Dictionary<string, object> JSResponse = new Dictionary<string, object>();
                JSResponse.Add("Message", "Employee Data");
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
            catch
            {
                return View("false", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult UpdatePassword(int EmployeeID,string OldPassword,string NewPassword)
        {
            try
            {
                string query = $@"Select EmployeeID,Name from EmployeeInfo Where EmployeeID = {EmployeeID} and Password COLLATE Latin1_General_CS_AS = '{OldPassword}'";
                Dictionary<string, object> JSResponse = new Dictionary<string, object>();
                DataTable Value = General.FetchData(query);
                List<Dictionary<string, object>> dbrows = GetProductRows(Value);
                if (Value.Rows.Count <= 0)
                {
                    JSResponse.Add("Status", false);
                }
                else
                {
                    General.ExecuteNonQuery($@"Update EmployeeInfo set Password = '{NewPassword}' Where EmployeeID = {EmployeeID}");
                    JSResponse.Add("Status", true);
                }
                //Dictionary<string, object> JSResponse = new Dictionary<string, object>();
                JSResponse.Add("Message", "Employee Data");
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
            catch
            {
                return View("false", JsonRequestBehavior.AllowGet);
            }
        }
    }
}