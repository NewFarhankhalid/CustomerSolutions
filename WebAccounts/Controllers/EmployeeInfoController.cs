using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Installments.Controllers
{
    public class EmployeeInfoController : Controller
    {
        // GET: EmployeeInfo
        public ActionResult Index()
        {
            DataTable dtemployee = General.FetchData("Select * from EmployeeInfo");
            List<EmployeeInfo> obj = DataTableToObject(dtemployee);
            return View(obj);
        }

        // GET: EmployeeInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeInfo/Create
        public ActionResult Create()
        {
         //ViewBag.BranchList=new DropDown().GetBranchSelectList();
         ViewBag.DesignationList=new DropDown().GetDesignationList();
            ViewBag.DepartmentList = new DropDown().GetDeparmentList();
            ViewBag.GenderList = new DropDown().GetGender();

            EmployeeInfo obj = new EmployeeInfo();
            obj.DOB = DateTime.Now;
            obj.ConfirmationDate = DateTime.Now;
            obj.JoiningDate = DateTime.Now;
            //ViewBag.FormattedOpenTime = obj.OpenTime.ToString("hh:mm tt");
            //ViewBag.FormattedOffTime = obj.OffTime.ToString("hh:mm tt");
            obj.OpenTime = DateTime.Now;
            obj.OffTime = DateTime.Now;
            return View(obj);
        }

        // POST: EmployeeInfo/Create
        [HttpPost]
        public ActionResult Create(EmployeeInfo objemployee)
        {
            try
            {
                if(objemployee.EmployeeID==0)
                {
                    string Query = @"Insert into EmployeeInfo (Department,Designation,Name,FatherName,CNIC,Nationality,Gender,DateOfBirth,BloodGroup,Salary,UtilityAllowance,MedicalAllowance,GPFundAccNo,KkFund,Other,Qualifications,DateOfJoining,DateOfConfirmation,BankAccountNo,BankName,BranchName,Address,Domicile,HomeTel,MobileNo,OpenTime,OffTime,UserName,Password,AllowHunderdMeters,IsAdmin) 
Values (" + objemployee.DepartmentID + "," + objemployee.DesignationID + ",'" + objemployee.Name + "','" + objemployee.FatherName + "','" + objemployee.CNIC + "','" + objemployee.Nationality + "','" + objemployee.Gender + "','" + objemployee.DOB + "','" + objemployee.BloodGroup + "'," + objemployee.Salary + ",'" + objemployee.UtilityAllowance + "','" + objemployee.MedicalAllowance + "','" + objemployee.GPFundNo + "','" + objemployee.KFFund + "','" + objemployee.Other + "','" + objemployee.Qualifications + "','" + objemployee.JoiningDate + "','" + objemployee.ConfirmationDate + "','" + objemployee.BankAccount + "','" + objemployee.BankName + "','" + objemployee.BankBranch + "','" + objemployee.Address + "','" + objemployee.Domicile + "','" + objemployee.HomeTel + "','" + objemployee.MobileTel + "','" + objemployee.OpenTime + "','" + objemployee.OffTime + "','" + objemployee.UserName + "','" + objemployee.Password + "'," + (objemployee.AllowHunderdMeters == true ? "1" : "0") + "," + (objemployee.IsAdmin == true ? "1" : "0") + ")";
                    General.ExecuteNonQuery(Query);
                    Query = "";
                    return Json("true");
                }
                else
                {
                    string Query = "";

                    Query = Query + $@"   UPDATE [dbo].[EmployeeInfo]
    SET [Department] = {objemployee.DepartmentID}
       ,[Designation] = {objemployee.DesignationID}
       ,[Name] = '{objemployee.Name}'
       ,[FatherName] = '{objemployee.FatherName}'
       ,[CNIC] = '{objemployee.CNIC}'
       ,[Nationality] = '{objemployee.Nationality}'
       ,[Gender] = '{objemployee.Gender}'
       ,[DateOfBirth] = '{objemployee.DOB}'
       ,[BloodGroup] = '{objemployee.BloodGroup}'
       ,[Salary] = {objemployee.Salary}
       ,[UtilityAllowance] = {objemployee.UtilityAllowance}
       ,[MedicalAllowance] = {objemployee.MedicalAllowance}
       ,[GPFundAccNo] = {objemployee.GPFundNo}
       ,[KkFund] = {objemployee.KFFund}
       ,[Other] = '{objemployee.Other}'
       ,[Qualifications] = '{objemployee.Qualifications}'
       ,[DateOfJoining] = '{objemployee.JoiningDate}'
       ,[DateOfConfirmation] = '{objemployee.ConfirmationDate}'
       ,[BankAccountNo] = '{objemployee.BankAccount}'
       ,[BankName] = '{objemployee.BankName}'
       ,[BranchName] = '{objemployee.BankBranch}'
       ,[Address] = '{objemployee.Address}'
       ,[Domicile] = '{objemployee.Domicile}'
       ,[HomeTel] = '{objemployee.HomeTel}'
       ,[MobileNo] = '{objemployee.MobileTel}'
       ,[OpenTime] = '{objemployee.OpenTime}'
       ,[OffTime] = '{objemployee.OffTime}'
       ,[Username] = '{objemployee.UserName}'
       ,[Password] = '{objemployee.Password}'
       ,[AllowHunderdMeters] = {(objemployee.AllowHunderdMeters ? "1" : "0")}
       ,[IsAdmin] = {(objemployee.IsAdmin ? "1" : "0")}
    WHERE EmployeeID = {objemployee.EmployeeID}";
                    //UPDATE [dbo].[EmployeeInfo]
                    //   SET [Department] = <Department, int,>
                    //      ,[Designation] = <Designation, int,>
                    //      ,[Name] = <Name, nvarchar(50),>
                    //      ,[FatherName] = <FatherName, nvarchar(50),>
                    //      ,[CNIC] = <CNIC, nvarchar(50),>
                    //      ,[Nationality] = <Nationality, nvarchar(50),>
                    //      ,[Gender] = <Gender, nvarchar(50),>
                    //      ,[DateOfBirth] = <DateOfBirth, nvarchar(50),>
                    //      ,[BloodGroup] = <BloodGroup, nvarchar(50),>
                    //      ,[Salary] = <Salary, nvarchar(50),>
                    //      ,[UtilityAllowance] = <UtilityAllowance, int,>
                    //      ,[MedicalAllowance] = <MedicalAllowance, int,>
                    //      ,[GPFundAccNo] = <GPFundAccNo, int,>
                    //      ,[KkFund] = <KkFund, int,>
                    //      ,[Other] = <Other, nvarchar(50),>
                    //      ,[Qualifications] = <Qualifications, nvarchar(50),>
                    //      ,[DateOfJoining] = <DateOfJoining, datetime,>
                    //      ,[DateOfConfirmation] = <DateOfConfirmation, datetime,>
                    //      ,[BankAccountNo] = <BankAccountNo, nvarchar(50),>
                    //      ,[BankName] = <BankName, nvarchar(50),>
                    //      ,[BranchName] = <BranchName, nvarchar(50),>
                    //      ,[Address] = <Address, nvarchar(max),>
                    //      ,[Domicile] = <Domicile, nvarchar(50),>
                    //      ,[HomeTel] = <HomeTel, nvarchar(50),>
                    //      ,[MobileNo] = <MobileNo, nvarchar(50),>
                    //      ,[OpenTime] = <OpenTime, datetime,>
                    //      ,[OffTime] = <OffTime, datetime,>
                    //      ,[FingerID] = <FingerID, nvarchar(max),>
                    //      ,[Username] = <Username, nvarchar(50),>
                    //      ,[Password] = <Password, nvarchar(50),>
                    //      ,[AllowHunderdMeters] = <AllowHunderdMeters, bit,>
                    //      ,[IsAdmin] = <IsAdmin, bit,>
                    // WHERE <Search Conditions,,>
                    //                    string Query = "";
                    //                    Query = Query + "UPDATE [dbo].[EmployeeInfo] SET ";
                    //                    Query = Query + "     [Name] = '" + objemployee.Name + "' ";
                    //                    Query = Query + "    ,[FatherName] = '" + objemployee.FatherName + "' ";
                    //                    Query = Query + "    ,[CNIC] = '" + objemployee.CNIC + "' ";
                    //                    Query = Query + "    ,[Address] = '" + objemployee.Address + "' ";
                    //                    Query = Query + "    ,[Domicile] = '" + objemployee.Domicile + "' ";
                    //                    Query = Query + "    ,[BloodGroup] = '" + objemployee.BloodGroup + "' ";
                    //                    Query = Query + "    ,[Salary] = '" + objemployee.Salary + "' ";
                    //                    Query = Query + "    ,[Loan] = '" + objemployee.Loan + "' ";
                    //                    Query = Query + "    ,[GPFundNo] = '" + objemployee.GPFundNo + "' ";
                    //                    Query = Query + "    ,[KFFund] = '" + objemployee.KFFund + "' ";
                    //                    Query = Query + "    ,[Other] = '" + objemployee.Other + "' ";
                    //                    Query = Query + "    ,[Qualifications] = '" + objemployee.Qualifications + "' ";
                    //                    Query = Query + "    ,[Gender] = '" + objemployee.Gender + "' ";
                    //                    Query = Query + "    ,[DOB] = '" + objemployee.DOB + "' ";
                    //                    Query = Query + "    ,[Nationality] = '" + objemployee.Nationality + "' ";
                    //                    Query = Query + "    ,[BankAccount] = '" + objemployee.BankAccount + "' ";
                    //                    Query = Query + "    ,[BankName] = '" + objemployee.BankName + "' ";
                    //                    Query = Query + "    ,[BankBranch] = '" + objemployee.BankBranch + "' ";
                    //                    Query = Query + "    ,[HomeTel] ='" + objemployee.HomeTel + "' ";
                    //                    Query = Query + "    ,[MobileTel] ='" + objemployee.MobileTel + "' ";
                    //                    Query = Query + "    ,[Designation] =" + objemployee.DesignationID + " ";
                    //                    Query = Query + "    ,[Department] =" + objemployee.DepartmentID + " ";
                    //                    Query = Query + "    ,[JoiningDate] ='" + objemployee.JoiningDate + "' ";
                    //                    Query = Query + "    ,[ConfirmationDate] ='" + objemployee.ConfirmationDate + "' ";
                    //                    Query = Query + "    ,[BasicSalary] ='" + objemployee.BasicSalary + "' ";
                    //                    Query = Query + "    ,[UtilityAllowance] ='" + objemployee.UtilityAllowance + "' ";
                    //                    Query = Query + "    ,[MedicalAllowance] ='" + objemployee.MedicalAllowance + "' ";
                    //                    Query = Query + "    ,[OpenTime] ='" + objemployee.OpenTime + "' ";
                    //                    Query = Query + "    ,[OffTime] ='" + objemployee.OffTime + "' ";
                    //                    Query = Query + "    ,[UserName] ='" + objemployee.UserName + "' ";
                    //                    Query = Query + "    ,[Password] ='" + objemployee.Password + "' ";
                    //                    Query = Query + "    ,[AllowHunderdMeters] =" + (objemployee.AllowHunderdMeters == true ? "1" : "0") + "";
                    //                    Query = Query + "    ,[IsAdmin] =" + (objemployee.IsAdmin == true ? "1" : "0") + "";
                    //                    Query = Query + "WHERE EmployeeID=" + objemployee.EmployeeID;
                    //                    ViewBag.DesignationList = new DropDown().GetDesignationList();
                    //                    ViewBag.GenderList = new DropDown().GetGender();
                    //                    General.ExecuteNonQuery(Query);
                    ViewBag.DesignationList = new DropDown().GetDesignationList();
                    ViewBag.GenderList = new DropDown().GetGender();
                    General.ExecuteNonQuery(Query);
                    return Json("true");
                }

            }
            catch (Exception ex)
            {
                //ViewBag.BranchList = new DropDown().GetBranchSelectList("", objemployee.BranchID);
                ViewBag.DesignationList = new DropDown().GetDesignationList("",objemployee.DesignationID);
                ViewBag.DepartmentList = new DropDown().GetDeparmentList("",objemployee.DepartmentID);
                ViewBag.GenderList = new DropDown().GetGender((objemployee.Gender = true ? 1:0));
                ViewBag.Error = "Error Inserting Employee Error: "+(ex.InnerException);
                return View();
            }
        }

        // GET: EmployeeInfo/Edit/5
        public ActionResult Edit(int id)
        {
            DataTable dtemployeeinfo = General.FetchData("Select * from EmployeeInfo where employeeid=" + id);
            List<EmployeeInfo> obj = DataTableToObject(dtemployeeinfo);
            if (obj.Count > 0)
            {
                string formattedOpenTime = obj[0].OpenTime.ToString("hh:mm tt");
                string formattedOffTime = obj[0].OffTime.ToString("hh:mm tt");

                // Pass the formatted time strings to the view
                ViewBag.FormattedOpenTime = formattedOpenTime;
                ViewBag.FormattedOffTime = formattedOffTime;
                //ViewBag.BranchList = new DropDown().GetBranchSelectList("",obj[0].BranchID);
                ViewBag.DesignationList = new DropDown().GetDesignationList("", obj[0].DesignationID);
                ViewBag.DepartmentList = new DropDown().GetDeparmentList("", obj[0].DepartmentID);
                ViewBag.GenderList = new DropDown().GetGender((obj[0].Gender = true ? 1:0));
                return View("Create",obj[0]);
            }
            return RedirectToAction("index");
        }

        // POST: EmployeeInfo/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(EmployeeInfo objemployee)
        {
            try
            { 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeInfo/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            string Query = "Delete from EmployeeInfo Where EmployeeID=" + id;
            General.ExecuteNonQuery(Query);
            return Json("true");
        }

        public ActionResult CheckUserName(string UserName,int EmployeeID)
        {
            string sql = $@"Select * from EmployeeInfo where UserName like '%{UserName}%' and EmployeeID!={EmployeeID}";
            DataTable dt = General.FetchData(sql);
            if (dt.Rows.Count > 0)
            {
                return Json("true," + dt.Rows[0]["UserName"].ToString());
            }
            else
            {
                return Json("false,");
            }
        }
        List<EmployeeInfo> DataTableToObject(DataTable dt)
        {
            List<EmployeeInfo> lstemployee = new List<EmployeeInfo>();
            EmployeeInfo bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new EmployeeInfo();
                if (dr["EmployeeID"] != DBNull.Value)
                {
                    bi.EmployeeID = int.Parse(dr["EmployeeID"].ToString());
                }
                if (dr["Name"] != DBNull.Value)
                {
                    bi.Name = (dr["Name"].ToString());
                }
                if (dr["FatherName"] != DBNull.Value)
                {
                    bi.FatherName = (dr["FatherName"].ToString());
                }
                if (dr["CNIC"] != DBNull.Value)
                {
                    bi.CNIC = (dr["CNIC"].ToString());
                }
                if (dr["Address"] != DBNull.Value)
                {
                    bi.Address = (dr["Address"].ToString());
                }
                if (dr["Domicile"] != DBNull.Value)
                {
                    bi.Domicile = (dr["Domicile"].ToString());
                }
                if (dr["BloodGroup"] != DBNull.Value)
                {
                    bi.BloodGroup = (dr["BloodGroup"].ToString());
                }
                if (dr["Salary"] != DBNull.Value)
                {
                    bi.Salary = decimal.Parse(dr["Salary"].ToString());
                }
                if (dr["GPFundAccNo"] != DBNull.Value)
                {
                    bi.GPFundNo = (dr["GPFundAccNo"].ToString());
                }
                if (dr["KkFund"] != DBNull.Value)
                {
                    bi.KFFund = (dr["KkFund"].ToString());
                }
                if (dr["Other"] != DBNull.Value)
                {
                    bi.Other = (dr["Other"].ToString());
                }
                if (dr["Qualifications"] != DBNull.Value)
                {
                    bi.Qualifications = (dr["Qualifications"].ToString());
                }
                if (dr["Gender"] != DBNull.Value)
                {
                    bi.Gender = int.Parse(dr["Gender"].ToString());
                }
                if (dr["DateOfBirth"] != DBNull.Value)
                {
                    bi.DOB = DateTime.Parse(dr["DateOfBirth"].ToString());
                }
                if (dr["Nationality"] != DBNull.Value)
                {
                    bi.Nationality = (dr["Nationality"].ToString());
                }
                if (dr["BankAccountNo"] != DBNull.Value)
                {
                    bi.BankAccount = (dr["BankAccountNo"].ToString());
                }
                if (dr["OpenTime"] != DBNull.Value)
                {
                    bi.OpenTime = DateTime.Parse(dr["OpenTime"].ToString());
                }
                if (dr["OffTime"] != DBNull.Value)
                {
                    bi.OffTime = DateTime.Parse(dr["OffTime"].ToString());
                }
                if (dr["BankName"] != DBNull.Value)
                {
                    bi.BankName = (dr["BankName"].ToString());
                }
                if (dr["BranchName"] != DBNull.Value)
                {
                    bi.BankBranch = (dr["BranchName"].ToString());
                }
                if (dr["HomeTel"] != DBNull.Value)
                {
                    bi.HomeTel = (dr["HomeTel"].ToString());
                }
                if (dr["MobileNo"] != DBNull.Value)
                {
                    bi.MobileTel = (dr["MobileNo"].ToString());
                }
                if (dr["Department"] != DBNull.Value)
                {
                    bi.DepartmentID = int.Parse(dr["Department"].ToString());
                }
                if (dr["Designation"] != DBNull.Value)
                {
                    bi.DesignationID = int.Parse(dr["Designation"].ToString());
                }
                if (dr["DateOfJoining"] != DBNull.Value)
                {
                    bi.JoiningDate = DateTime.Parse(dr["DateOfJoining"].ToString());
                }
                if (dr["DateOfConfirmation"] != DBNull.Value)
                {
                    bi.ConfirmationDate = DateTime.Parse(dr["DateOfConfirmation"].ToString());
                }
                if (dr["Salary"] != DBNull.Value)
                {
                    bi.BasicSalary = decimal.Parse(dr["Salary"].ToString());
                }
               
                if (dr["UtilityAllowance"] != DBNull.Value)
                {
                    bi.UtilityAllowance = decimal.Parse(dr["UtilityAllowance"].ToString());
                }
                if (dr["MedicalAllowance"] != DBNull.Value)
                {
                    bi.MedicalAllowance = decimal.Parse(dr["MedicalAllowance"].ToString());
                }
                if (dr["UserName"] != DBNull.Value)
                {
                    bi.UserName = dr["UserName"].ToString();
                }
                if (dr["Password"] != DBNull.Value)
                {
                    bi.Password = dr["Password"].ToString();
                }
                if (dr["AllowHunderdMeters"] != DBNull.Value)
                {
                    bi.AllowHunderdMeters = bool.Parse(dr["AllowHunderdMeters"].ToString());
                }
                if (dr["IsAdmin"] != DBNull.Value)
                {
                    bi.IsAdmin = bool.Parse(dr["IsAdmin"].ToString());
                }
                lstemployee.Add(bi);
            }
            return lstemployee;

        }
    }
}
