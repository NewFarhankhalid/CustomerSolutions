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
         ViewBag.GenderList = new DropDown().GetGender();
            return View();
        }

        // POST: EmployeeInfo/Create
        [HttpPost]
        public ActionResult Create(EmployeeInfo objemployee)
        {
            try
            {
                string Query = "Insert into EmployeeInfo (Name,FatherName,CNIC,Address,Domicile,BloodGroup,Salary,Loan,GPFundNo,KFFund,Other,Qualifications,Gender,DOB,Nationality,BankAccount,BankName,BankBranch,HomeTel,MobileTel,DesignationID,JoiningDate,ConfirmationDate,BasicSalary,UtilityAllowance,MedicalAllowance) ";
                Query = Query + "Values ('" + objemployee.Name + "','" + objemployee.FatherName + "','" + objemployee.CNIC + "','"+objemployee.Address+"','" + objemployee.Domicile + "','" + objemployee.BloodGroup + "','" + objemployee.Salary+ "','" + objemployee.Loan+ "','" + objemployee.GPFundNo + "','" + objemployee.KFFund + "','" + objemployee.Other + "','" + objemployee.Qualifications + "','" + objemployee.Gender + "','" + objemployee.DOB.ToString() + "','" + objemployee.Nationality + "','" + objemployee.BankAccount + "','" + objemployee.BankName + "','" + objemployee.BankBranch + "','" + objemployee.HomeTel + "','" + objemployee.MobileTel + "','" + objemployee.DesignationID + "','" + objemployee.JoiningDate.ToString() + "','" + objemployee.ConfirmationDate.ToString() + "','" + objemployee.BasicSalary + "','" + objemployee.UtilityAllowance + "','" + objemployee.MedicalAllowance+ "')";
                General.ExecuteNonQuery(Query);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //ViewBag.BranchList = new DropDown().GetBranchSelectList("", objemployee.BranchID);
                ViewBag.DesignationList = new DropDown().GetDesignationList("",objemployee.DesignationID);
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

                //ViewBag.BranchList = new DropDown().GetBranchSelectList("",obj[0].BranchID);
                ViewBag.DesignationList = new DropDown().GetDesignationList("", obj[0].DesignationID);
                ViewBag.GenderList = new DropDown().GetGender((obj[0].Gender = true ? 1:0));
                return View(obj[0]);
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
                string Query = "";
                Query = Query + "UPDATE [dbo].[EmployeeInfo] SET ";
                Query = Query + "     [Name] = '" + objemployee.Name + "' ";
                Query = Query + "    ,[FatherName] = '" + objemployee.FatherName + "' ";
                Query = Query + "    ,[CNIC] = '" + objemployee.CNIC + "' ";
                Query = Query + "    ,[Address] = '" + objemployee.Address + "' ";
                Query = Query + "    ,[Domicile] = '" + objemployee.Domicile + "' ";
                Query = Query + "    ,[BloodGroup] = '" + objemployee.BloodGroup + "' ";
                Query = Query + "    ,[Salary] = '" + objemployee.Salary + "' ";
                Query = Query + "    ,[Loan] = '" + objemployee.Loan + "' ";
                Query = Query + "    ,[GPFundNo] = '" + objemployee.GPFundNo + "' ";
                Query = Query + "    ,[KFFund] = '" + objemployee.KFFund + "' ";
                Query = Query + "    ,[Other] = '" + objemployee.Other + "' ";
                Query = Query + "    ,[Qualifications] = '" + objemployee.Qualifications + "' ";
                Query = Query + "    ,[Gender] = '" + objemployee.Gender + "' ";
                Query = Query + "    ,[DOB] = '" + objemployee.DOB + "' ";
                Query = Query + "    ,[Nationality] = '" + objemployee.Nationality + "' ";
                Query = Query + "    ,[BankAccount] = '" + objemployee.BankAccount + "' ";
                Query = Query + "    ,[BankName] = '" + objemployee.BankName + "' ";
                Query = Query + "    ,[BankBranch] = '" + objemployee.BankBranch + "' ";
                Query = Query + "    ,[HomeTel] ='" + objemployee.HomeTel + "' ";
                Query = Query + "    ,[MobileTel] ='" + objemployee.MobileTel + "' ";
                Query = Query + "    ,[DesignationID] ='" + objemployee.DesignationID + "' ";
                Query = Query + "    ,[JoiningDate] ='" + objemployee.JoiningDate + "' ";
                Query = Query + "    ,[ConfirmationDate] ='" + objemployee.ConfirmationDate + "' ";
                Query = Query + "    ,[BasicSalary] ='" + objemployee.BasicSalary + "' ";
                Query = Query + "    ,[UtilityAllowance] ='" + objemployee.UtilityAllowance + "' ";
                Query = Query + "    ,[MedicalAllowance] ='" + objemployee.MedicalAllowance + "' ";
                Query = Query + "WHERE EmployeeID=" + objemployee.EmployeeID;
                ViewBag.DesignationList = new DropDown().GetDesignationList();
                ViewBag.GenderList = new DropDown().GetGender();
                General.ExecuteNonQuery(Query);
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
                if (dr["BranchId"] != DBNull.Value)
                {
                    bi.BranchID = int.Parse(dr["BranchId"].ToString());
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
                if (dr["Loan"] != DBNull.Value)
                {
                    bi.Loan = decimal.Parse(dr["Loan"].ToString());
                }
                if (dr["GPFundNo"] != DBNull.Value)
                {
                    bi.GPFundNo = (dr["GPFundNo"].ToString());
                }
                if (dr["KFFund"] != DBNull.Value)
                {
                    bi.KFFund = (dr["KFFund"].ToString());
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
                if (dr["DOB"] != DBNull.Value)
                {
                    bi.DOB = DateTime.Parse(dr["DOB"].ToString());
                }
                if (dr["Nationality"] != DBNull.Value)
                {
                    bi.Nationality = (dr["Nationality"].ToString());
                }
                if (dr["BankAccount"] != DBNull.Value)
                {
                    bi.BankAccount = (dr["BankAccount"].ToString());
                }
                if (dr["BankName"] != DBNull.Value)
                {
                    bi.BankName = (dr["BankName"].ToString());
                }
                if (dr["BankBranch"] != DBNull.Value)
                {
                    bi.BankBranch = (dr["BankBranch"].ToString());
                }
                if (dr["HomeTel"] != DBNull.Value)
                {
                    bi.HomeTel = (dr["HomeTel"].ToString());
                }
                if (dr["MobileTel"] != DBNull.Value)
                {
                    bi.MobileTel = (dr["MobileTel"].ToString());
                }
                if (dr["DesignationID"] != DBNull.Value)
                {
                    bi.DesignationID = int.Parse(dr["DesignationID"].ToString());
                }
                if (dr["JoiningDate"] != DBNull.Value)
                {
                    bi.JoiningDate = DateTime.Parse(dr["JoiningDate"].ToString());
                }
                if (dr["ConfirmationDate"] != DBNull.Value)
                {
                    bi.ConfirmationDate = DateTime.Parse(dr["ConfirmationDate"].ToString());
                }
                if (dr["BasicSalary"] != DBNull.Value)
                {
                    bi.BasicSalary = decimal.Parse(dr["BasicSalary"].ToString());
                }
               
                if (dr["UtilityAllowance"] != DBNull.Value)
                {
                    bi.UtilityAllowance = decimal.Parse(dr["UtilityAllowance"].ToString());
                }
                if (dr["MedicalAllowance"] != DBNull.Value)
                {
                    bi.MedicalAllowance = decimal.Parse(dr["MedicalAllowance"].ToString());
                }
                lstemployee.Add(bi);


            }
            return lstemployee;

        }
    }
}
