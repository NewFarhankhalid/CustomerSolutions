using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml.Linq;


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
        public ActionResult GetEmployeeLogin(string UserName, string Password)
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
        public async Task<ActionResult> AddAttendance(int EmployeeID,string Location,string IPAddress,string MacAddress,string DeviceName,double Longtitude,double Latitude,int AttendanceType)
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
                        string EmployeeName = dt.Rows[0]["Name"].ToString();
                        DataTable dt2 = General.FetchData($@"Select * from AdminMobileKey ");
                        foreach (DataRow dr in dt2.Rows)
                        {
                            await SendMessageAsync(dr["MobileKey"].ToString(), EmployeeName, AttendanceType == 1 ? "Check In" : " Check Out", DateTime.Now, Location);
                        }
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
        public async Task SendMessageAsync(string MobileKey,string EmployeeName, string AttendanceType,DateTime dateTime,string Location)
        {
            string jsonFilePath = "H:\\Sajjad\\Customer Solution WebApp\\Customer Solution WebApp\\WebAccounts\\softinn-solutions-firebase-adminsdk-7mtt3-b0024ccb7d.json";
            string jsonData = System.IO.File.ReadAllText(jsonFilePath);

            // Deserialize JSON data into FirebaseCredentialModel object
            FirebaseCredentialModel firebaseCredential = JsonConvert.DeserializeObject<FirebaseCredentialModel>(jsonData);

            // Set up credentials
            //string json = File.("H:\\Sajjad\\Customer Solution WebApp\\Customer Solution WebApp\\WebAccounts\\softinn-solutions-firebase-adminsdk-7mtt3-b0024ccb7d.json");
            //GoogleCredential credential = GoogleCredential.FromFile("H:\\Sajjad\\Customer Solution WebApp\\Customer Solution WebApp\\WebAccounts\\softinn-solutions-firebase-adminsdk-7mtt3-b0024ccb7d.json");

            GoogleCredential credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(firebaseCredential));
            //FirebaseApp firebaseApp = FirebaseApp.Create(new AppOptions
            //{
            //    Credential = credential
            //});

            FirebaseApp firebaseApp = FirebaseApp.DefaultInstance;
            if (firebaseApp == null)
            {
                // FirebaseApp doesn't exist, create a new one
                firebaseApp = FirebaseApp.Create(new AppOptions
                {
                    Credential = credential
                });
            }

            // Get the messaging instance
            var messaging = FirebaseMessaging.GetMessaging(firebaseApp);

            // Construct your message
            var message = new FirebaseAdmin.Messaging.Message
            {
                Notification = new Notification
                {
                    Title = EmployeeName,
                    Body = AttendanceType +" On "+ dateTime +" From "+ Location
                },
                Token = MobileKey //"dfRx5O0XR6KsaI_urvP-Ik:APA91bEW1-4KD-XUTXfwZhv4c6o9W9-9XK0b9urc-aIIngIfGuv46U8i8McL0xmMAwgbrLZHsolCjOCBHymE3ZKawGAZJzN9hJ5EbkAZ-tszP0VcIxxM0_InpRQY92jX69ARE_ToGJTt"
            };

            // Send the message
            string response = await messaging.SendAsync(message);
        }
    }
}