using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Installments.Controllers
{
    public class AddEmployeeController : Controller
    {
        // GET: AddEmployee
        public ActionResult Index()
        {
            List<AddEmployee> emp = new List<AddEmployee>();
            ViewBag.Department = new DropDown().GetDesignation();
            ViewBag.Designation = new DropDown().GetDesignation();

            return View(emp);
        }

        public ActionResult Create()
        {
            AddEmployee emp = new AddEmployee();
            ViewBag.Department = new DropDown().GetDesignation();
            ViewBag.Designation = new DropDown().GetDesignation();
            return View(emp);
        }

        [HttpPost]
         public ActionResult Create(AddEmployee emp)
        {
            try
            {
                if(emp.EmployeeID == 0)
                {
                    string Query = "Insert Into AddEmployee(Department,Designation,EmployeeName,EmployeeFatherName,Address,City,MobileNo,PhoneNo,CNICNo,Salary,TimeIn,TimeOut)";
                    Query = Query + "Values ('" + emp.Department + "','" + emp.Designation + "','" + emp.EmployeeName + "','" + emp.EmployeeFatherName + "','" + emp.Address + "' ,'" + emp.City + "','" + emp.MobileNo + "','" + emp.PhoneNo + "','" + emp.CNICNo + "','" + emp.Salary + "','" + emp.TimeIn + "','" + emp.TimeOut + "')";
                    Query = Query + @" Select @@IDENTITY as EmployeeID";
                    DataTable dt = General.FetchData(Query);
                    emp.EmployeeID = int.Parse(dt.Rows[0]["EmployeeID"].ToString());   
                }
                else
                {
                    string Query = "";
                   
                    Query = Query + "UPDATE [dbo].[AddEmployee] ";
                  
                    Query = Query + "    ,[Department] = '" + emp.Department + "' ";
                    Query = Query + "    ,[Designation] = '" + emp.Designation + "' ";
                    Query = Query + "    ,[EmployeeName] ='" + emp.EmployeeName + "' ";
                    Query = Query + "    ,[EmployeeFatherName] ='" + emp.EmployeeFatherName + "' ";
                    Query = Query + "    ,[Address] ='" + emp.Address + "' ";
                    Query = Query + "    ,[City] ='" + emp.City + "' ";
                    Query = Query + "    ,[MobileNo] ='" + emp.MobileNo + "' ";
                    Query = Query + "    ,[PhoneNo] ='" + emp.PhoneNo + "' ";
                    Query = Query + "    ,[CNICNo] ='" + emp.CNICNo + "' ";
                    Query = Query + "    ,[Sallery] ='" + emp.Salary + "' ";
                    Query = Query + "    ,[TimeIn] ='" + emp.TimeIn + "' ";
                    Query = Query + "    ,[TimeOut] ='" + emp.TimeOut + "' ";
                    Query = Query + " WHERE EmployeeID=" + emp.EmployeeID;
                    General.ExecuteNonQuery(Query);
                }
            }
            catch {
                return Json("false");
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            DataTable dtemployee = General.FetchData("Select * from AddEmployee");
            List<AddEmployee> lstemployee = DataTableToObject(dtemployee);
            if (lstemployee.Count > 0 )
            {
                return View("Create", lstemployee[0]);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            string Query = "Delete from AddEmployee where EmployeeID=" + id;
            General.ExecuteNonQuery(Query);
            return RedirectToAction("Index");
        }

        public ActionResult EmployeeInformation(int id)
        {
            string Query = @" SELECT  *  from
    AddEmployee where EmployeeID=" + id;
            DataTable dtEmployee= General.FetchData(Query);
            List<AddEmployee> obj = DataTableToObject(dtEmployee);
            //if (dtEmployee.Rows.Count > 0)
            //{
            //    CustomerInfo objCustomerInfo = new CustomerInfo();
            //    ViewBag.Customerid = int.Parse(dtCustomer.Rows[0]["CustomerID"].ToString());
            //    ViewBag.Customertitle = dtCustomer.Rows[0]["CustomerCompanyTitle"].ToString();
            //}
            List<AddEmployee> lstCustomer = DataTableToObject(dtEmployee);
            return View(obj[0]);
        }
        List<AddEmployee> DataTableToObject(DataTable dt)
        {
            List<AddEmployee> lstEmploye = new List<AddEmployee>();
            AddEmployee bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new AddEmployee();
                if (dr["EmployeeID"] != DBNull.Value)
                {
                    bi.EmployeeID = int.Parse(dr["EmployeeID"].ToString());
                }
                if (dr["Department"] != DBNull.Value)
                {
                    bi.Department = (dr["Department"].ToString());
                }
                if (dr["Designation"] != DBNull.Value)
                {
                    bi.Designation = dr["Designation"].ToString();
                }

                if (dr["EmployeeName"] != DBNull.Value)
                {
                    bi.EmployeeName = (dr["EmployeeName"].ToString());
                }
                if (dr["EmployeeFatherName"] != DBNull.Value)
                {
                    bi.EmployeeFatherName = (dr["EmployeeFatherName"].ToString());
                }
                if (dr["Address"] != DBNull.Value)
                {
                    bi.Address = (dr["Address"].ToString());
                }
                if (dr["City"] != DBNull.Value)
                {
                    bi.City = (dr["City"].ToString());
                }
                if (dr["MobileNO"] != DBNull.Value)
                {
                    bi.MobileNo = (dr["MobileNO"].ToString());
                }
                if (dr["PhoneNo"] != DBNull.Value)
                {
                    bi.PhoneNo = (dr["PhoneNo"].ToString());
                }
                if (dr["CNICNo"] != DBNull.Value)
                {
                    bi.CNICNo = (dr["CNICNo"].ToString());
                }
                if (dr["Salary"] != DBNull.Value)
                {
                    bi.Salary = (dr["Salary"].ToString());
                }
                lstEmploye.Add(bi);
            }
            return lstEmploye;
        }

    }
}