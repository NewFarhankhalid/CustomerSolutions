using Installments.Models;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
namespace Installments
{
    public class DropDown
    {
        public SelectList GetRightRoles(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from rightsrole  Where Inactive=0" + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();
                si.Text = dr["RoleTitle"].ToString();
                si.Value = dr["RoleID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        public SelectList GetLogSource(int selectedvalue = 3)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            objsli.Add(new SelectListItem() { Text = "Select Source", Value = 3.ToString() });
            objsli.Add(new SelectListItem() { Text = "Add", Value = 0.ToString() });
            objsli.Add(new SelectListItem() { Text = "Edit", Value = 1.ToString() });
            objsli.Add(new SelectListItem() { Text = "Delete", Value = 2.ToString() });
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue.ToString());
            return sl;
        }
        public SelectList GetPaymentMethod(int? selectedvalue = 0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            objsli.Add(new SelectListItem() { Text = "JazzCash", Value = 1.ToString() });
            objsli.Add(new SelectListItem() { Text = "Bank Account", Value = 2.ToString() });
            objsli.Add(new SelectListItem() { Text = "Easy Paisa", Value = 3.ToString() });
            objsli.Add(new SelectListItem() { Text = "By Hand", Value = 4.ToString() });
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue.ToString());
            return sl;
        }
    
        public SelectList GetDesignation(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtDesignation = General.FetchData("Select * from OperatorDesignation " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtDesignation.Rows)
            {
                si = new SelectListItem();
                si.Value = dr["Id"].ToString();
                si.Text = dr["Name"].ToString();

                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        public SelectList GetUserList(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtUsers = General.FetchData("Select * from UserInfo  where Inactive=0 " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtUsers.Rows)
            {
                si = new SelectListItem();
                si.Value = dr["UserID"].ToString();
                si.Text = dr["UserName"].ToString();
                
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }

        public SelectList GetWorkPriority(string whereclause = "", int SelectedValue = 0)
        {
            DataTable dtPriority = General.FetchData("Select * from TasksType " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach(DataRow dr in dtPriority.Rows)
            {
                si = new SelectListItem();
                si.Text = dr["Name"].ToString();
                si.Value  = dr["Id"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", SelectedValue);
            return sl;
        }
        public SelectList GetEmptyList(string whereclause = "", int selectedvalue = 0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            si.Value = "0";
            si.Text = "No Item";
            objsli.Add(si);
            SelectList sl = new SelectList(objsli, "Value", "Text");
            return sl;
        }
        public SelectList GetWorkStatus(string whereclause = "", int SelectedValue = 0)
        {
            DataTable dtPriority = General.FetchData("Select * from WorkStatus " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtPriority.Rows)
            {
                si = new SelectListItem();
                si.Text = dr["Name"].ToString();
                si.Value = dr["Id"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", SelectedValue);
            return sl;
        }
        public SelectList GetWorkCategory(string whereclause = "", int SelectedValue = 0)
        {
            DataTable dtPriority = General.FetchData("Select * from WorkCategory " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtPriority.Rows)
            {
                si = new SelectListItem();
                si.Text = dr["Name"].ToString();
                si.Value = dr["Id"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", SelectedValue);
            return sl;
        }
        public SelectList GetDDLSource(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("select distinct Source from Logs " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["Source"].ToString();
                si.Value = dr["Source"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        public SelectList GetEmployeeTypes(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployeeType = General.FetchData("Select * from EmployeeType " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployeeType.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["EmployeeTypeTitle"].ToString();
                si.Value = dr["EmployeeTypeID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        public SelectList GetCustomerInNewProblem(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployeeType = General.FetchData(" Select C.CustomerID,(C.CustomerCompanytitle +' ('+ CityName +') , '+ cast(Address as nvarchar))CustomerName from CustomerInfo C inner join PakCity on PakCity.CityID = C.CityID Where Inactive = 0  " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployeeType.Rows)
            {
                si = new SelectListItem();
                si.Text = dr["CustomerName"].ToString();
                si.Value = dr["CustomerID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }

        public SelectList GetCustomer(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployeeType = General.FetchData(" Select CustomerID,CustomerCompanytitle from CustomerInfo Where Inactive = 0 " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployeeType.Rows)
            {
                si = new SelectListItem();
                si.Text = dr["CustomerCompanytitle"].ToString();
                si.Value = dr["CustomerID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }

        public SelectList GetBranchInfo(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployeeType = General.FetchData("Select * from BranchInformation ");
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployeeType.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["BranchTitle"].ToString();
                si.Value = dr["BranchID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        public SelectList GetProductInfo(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployeeType = General.FetchData("Select * from ProductInfo ");
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployeeType.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["ProductName"].ToString();
                si.Value = dr["ProductID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        public SelectList GetShiftInfo(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtShiftInfo = General.FetchData("Select * from ShiftInfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtShiftInfo.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["ShiftTitle"].ToString();
                si.Value = dr["ShiftID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        public SelectList GetCategoryInfo(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtShiftInfo = General.FetchData("Select * from CategoryInfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtShiftInfo.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["CategoryTitle"].ToString();
                si.Value = dr["CategoryID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        public SelectList GetSubCategoryInfo(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtSubCatInfo = General.FetchData("SELECT CategoryID, CategoryTitle,  ISNULL(ParentCategory,0) As ParentCategory FROM CategoryInfo Where ParentCategory!=0 ");
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtSubCatInfo.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["CategoryTitle"].ToString();
                si.Value = dr["CategoryID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        public SelectList GetDepartmentTypes(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtDepartmentTypes = General.FetchData("Select * from DepartmentInfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtDepartmentTypes.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["DepartmentTitle"].ToString();
                si.Value = dr["DepartmentID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        public SelectList GetBranchList(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtBranch = General.FetchData("Select * from Branchinfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtBranch.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["BranchTitle"].ToString();
                si.Value = dr["BranchID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        public SelectList GetPaymentType(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtPayment = General.FetchData("Select * from PaymentType " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtPayment.Rows)
            {
                si = new SelectListItem();               
                si.Value = dr["Id"].ToString();
                si.Text = dr["Name"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        public SelectList GETMaritalStatusList(int selectedvalue = 0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            objsli.Add(new SelectListItem() { Text = "Married", Value = 1.ToString() });
            objsli.Add(new SelectListItem() { Text = "Single", Value = 2.ToString() });
             SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue.ToString());
            return sl;
        }

        public SelectList GetSolved(int selectedvalue = 0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            objsli.Add(new SelectListItem() { Text = "Not Solved", Value = "1" });
            objsli.Add(new SelectListItem() { Text = "Solved", Value = "2" });
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue.ToString());
            return sl;
        }

        public SelectList GetWeaklyDays(int selectedvalue = 0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();          
            objsli.Add(new SelectListItem() { Text = "Friday", Value = 1.ToString() });
            objsli.Add(new SelectListItem() { Text = "Saturday", Value = 2.ToString() });
            objsli.Add(new SelectListItem() { Text = "Sunday", Value = 3.ToString() });
            objsli.Add(new SelectListItem() { Text = "Monday", Value = 4.ToString() });
            objsli.Add(new SelectListItem() { Text = "Tuesday", Value = 5.ToString() });
            objsli.Add(new SelectListItem() { Text = "Wednesday", Value = 6.ToString() });
            objsli.Add(new SelectListItem() { Text = "Thursday", Value = 7.ToString() });
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue.ToString());
            return sl;
        }
        
        public SelectList GetCity(int selectedvalue = 0)
        {
            DataTable dtBranch = General.FetchData("Select * from PakCity ");
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtBranch.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["CityName"].ToString();
                si.Value = dr["CityID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        public SelectList GetForms(int selectedvalue = 0)
        {
            DataTable dtBranch = General.FetchData("Select * from FormInfo ");
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtBranch.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["FormTitle"].ToString();
                si.Value = dr["FormID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
    
        public SelectList GetSysType(int selectedvalue = 0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            objsli.Add(new SelectListItem() { Text = "Server", Value = 1.ToString() });
            objsli.Add(new SelectListItem() { Text = "Client", Value = 2.ToString() });
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue.ToString());
            return sl;
        }
        
        public SelectList GetDeliveryDetail(int selectedvalue = 0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            objsli.Add(new SelectListItem() { Text = "Un Confirmed", Value = 0.ToString() });
            objsli.Add(new SelectListItem() { Text = "Confirmed", Value = 1.ToString() });
            objsli.Add(new SelectListItem() { Text = " Dispatch ", Value = 2.ToString() });
            objsli.Add(new SelectListItem() { Text = " Delivered ", Value = 3.ToString() });
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue.ToString());
            return sl;
        }
        public SelectList GetDDlDel(int selectedvalue = 0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            objsli.Add(new SelectListItem() { Text = "Cash Received", Value = 1.ToString() });
            objsli.Add(new SelectListItem() { Text = " Product Return ", Value = 2.ToString() });
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue.ToString());
            return sl;
        }

        public SelectList GetLeaveDetail(int selectedvalue = 0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            objsli.Add(new SelectListItem() { Text = "Employee", Value = 1.ToString() });
            objsli.Add(new SelectListItem() { Text = "Daily", Value = 2.ToString() });
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue.ToString());
            return sl;
        }
        public SelectList GetBloodGroup(int selectedvalue = 0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            objsli.Add(new SelectListItem() { Text = "A+", Value = 1.ToString() });
            objsli.Add(new SelectListItem() { Text = "A-", Value = 2.ToString() });
            objsli.Add(new SelectListItem() { Text = "B+", Value = 3.ToString() });
            objsli.Add(new SelectListItem() { Text = "B-", Value = 4.ToString() });
            objsli.Add(new SelectListItem() { Text = "O+", Value = 5.ToString() });
            objsli.Add(new SelectListItem() { Text = "O-", Value = 6.ToString() });
            objsli.Add(new SelectListItem() { Text = "AB+", Value = 7.ToString() });
            objsli.Add(new SelectListItem() { Text = "AB-", Value = 8.ToString() });
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue.ToString());
            return sl;
        }
        public SelectList GetDaysName(int selectedvalue = 0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            objsli.Add(new SelectListItem() { Text = "Monday", Value = 1.ToString() });
            objsli.Add(new SelectListItem() { Text = "Tuesday", Value = 2.ToString() });
            objsli.Add(new SelectListItem() { Text = "Wednesday", Value = 3.ToString() });
            objsli.Add(new SelectListItem() { Text = "Thursday", Value = 4.ToString() });
            objsli.Add(new SelectListItem() { Text = "Friday", Value = 5.ToString() });
            objsli.Add(new SelectListItem() { Text = "Saturday", Value = 6.ToString() });
            objsli.Add(new SelectListItem() { Text = "Sunday", Value = 7.ToString() });
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue.ToString());
            return sl;
        }
        public SelectList GetGender(int selectedvalue = 0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            objsli.Add(new SelectListItem() { Text = "Male", Value = 1.ToString() });
            objsli.Add(new SelectListItem() { Text = "Female", Value = 2.ToString() });
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue.ToString());
            return sl;
        }
        public SelectList GetLeaveType(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from LeaveTypesInfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["LeaveTypeTitle"].ToString();
                si.Value = dr["LeaveTypeID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        public SelectList GetEmployee(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select EmployeeCode+' - '+EmployeeName As EmployeeName, * from employeeinfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["EmployeeName"].ToString();
                si.Value = dr["EmployeeID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }


        public SelectList EmptySelectList(string whereclause = "", int selectedvalue = 0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            si.Value = "0";
            si.Text = "No Item";
            objsli.Add(si);
            SelectList sl = new SelectList(objsli, "Value", "Text");
            return sl;
        }
        public SelectList PageSize(int selectedvalue = 0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            objsli.Add(new SelectListItem() { Text = "Thermal Printer Receipt", Value = 1.ToString() });
            objsli.Add(new SelectListItem() { Text = "A4 Page", Value = 2.ToString() });
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue.ToString());
            return sl;

        }
        public SelectList GetGateInOut(int selectedvalue = 0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            objsli.Add(new SelectListItem() { Text = "Gate In", Value = 1.ToString() });
            objsli.Add(new SelectListItem() { Text = "Gate Out", Value = 2.ToString() });
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue.ToString());
            return sl;

        }
        public SelectList GetPartyTypeSelectList(int selectedvalue = 0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            objsli.Add(new SelectListItem() { Text = "Vendor", Value = 1.ToString() });
            objsli.Add(new SelectListItem() { Text = "Customer", Value = 2.ToString() });
            objsli.Add(new SelectListItem() { Text = "Both", Value = 3.ToString() });

            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue.ToString());
            return sl;

        }
        public SelectList GetEmployeesSelectList(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from EmployeeInfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["Name"].ToString();
                si.Value = dr["EmployeeID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }

        public SelectList StockType(string selectedvalue = "")
        {
            string SQL = @"Select '01' As StockType,'Net Stock' Title
                            union all
                            Select 'A','Detail Stock'
                            union All
                            Select '02','Sale'
                            union all
                            Select '03','Purchase'
                            union all
                            Select CAST(StockManagementTypeID as nvarchar(20)),Title from StockManagementType";
            DataTable dtEmployee = General.FetchData(SQL);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["Title"].ToString();
                si.Value = dr["StockType"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        public SelectList GetStockInManagementType(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from StockManagementType where nature=1");
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["Title"].ToString();
                si.Value = dr["StockManagementTypeID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        public SelectList GetStockOutManagementType(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from StockManagementType where nature=2");
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["Title"].ToString();
                si.Value = dr["StockManagementTypeID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        public SelectList GetBranchSelectList(string whereclause = "", int selectedvalue = 0)
        {

            string branches=AuthorizedBrances(int.Parse(System.Web.HttpContext.Current.Request.Cookies["UserID"].Value.ToString()));


            DataTable dtEmployee = General.FetchData("Select * from (Select * from BranchInformation Where branchid in ("+branches+ ")) BranchInformation" + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["Branchtitle"].ToString();
                si.Value = dr["BranchID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;

        }
        public SelectList GetSalePAymentNature(int Selectedvalue=0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            objsli.Add(new SelectListItem() { Text = "Cash", Value = 1.ToString() });
            objsli.Add(new SelectListItem() { Text = "Card", Value = 2.ToString() });
            objsli.Add(new SelectListItem() { Text = "Bank", Value = 3.ToString() });
            objsli.Add(new SelectListItem() { Text = "Credit", Value = 4.ToString() });
            SelectList sl = new SelectList(objsli, "Value", "Text", Selectedvalue.ToString());
            return sl;
        }

        public SelectList GetDesignationList(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from DesignationInfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["DesignationTitle"].ToString();
                si.Value = dr["DesignationID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;

        }

        public SelectList GetDeparmentList(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from DeparmentInfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["DeparmentTitle"].ToString();
                si.Value = dr["DeparmentID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;

        }
        public SelectList GetWarehouseList(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from Warehouseinfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();
                si.Text = dr["Warehousetitle"].ToString();
                si.Value = dr["warehouseid"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;

        }
        public SelectList GetProductCategoryList(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from CategoryInfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["CategoryTitle"].ToString();
                si.Value = dr["CategoryID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;

        }
        public SelectList GetVariants(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from VariantsInfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["VariantTitle"].ToString();
                si.Value = dr["VariantID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;

        }
        public SelectList GetColour(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from ColourInfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["ColourTitle"].ToString();
                si.Value = dr["ColourID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;

        }
        public SelectList GetTags(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from TagsInfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["TagTitle"].ToString();
                si.Value = dr["TagsID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;

        }
        public SelectList GetSize(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from SizeInfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["SizeTitle"].ToString();
                si.Value = dr["SizeID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;

        }

        public SelectList GetProductType(int selectedvalue = 0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            objsli.Add(new SelectListItem() { Text = "Battery", Value = 1.ToString() });
            objsli.Add(new SelectListItem() { Text = "UPS/Inverter", Value = 2.ToString() });
            objsli.Add(new SelectListItem() { Text = "Solar Pannel", Value = 3.ToString() });
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue.ToString());
            return sl;
        }
        public SelectList GetTandCInfo(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from TermsandConditionInfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["TermsTitle"].ToString();
                si.Value = dr["TermsandConditionID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;

        }
        public SelectList GetProductList(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from ProductInfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["ProductCode"]+" - " +dr["ProductName"].ToString();
                si.Value = dr["ProductID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        


        public SelectList GetFranchiseInfo(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from FranchiseInfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["FranchiseTitle"].ToString();
                si.Value = dr["FranchiseID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;

        }
        public SelectList GetAreaInfo(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from areainfo" + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["AreaTitle"].ToString();
                si.Value = dr["AreaID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;

        }
        public SelectList GetGroupAccounts(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from ChartofAccount Where Nature=0 " + whereclause + " order by AccountNo");
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["AccountNo"] + " - " + dr["AccountTitle"].ToString();
                si.Value = dr["AccountID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;

        }
        public SelectList GetTransactionalAccounts(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from ChartofAccount Where Nature=1 " + whereclause + " order by AccountNo");
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["AccountNo"] + " - " + dr["AccountTitle"].ToString();
                si.Value = dr["AccountID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;

        }
        public SelectList GetAccountNature(string whereclause = "", int selectedvalue = 0)
        {

            List<SelectListItem> objsli = new List<SelectListItem>();
            objsli.Add(new SelectListItem() { Text = "Group", Value = "0" });
            objsli.Add(new SelectListItem() { Text = "Transactional", Value = "1" });
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;

        }
        public SelectList GetVoucherNature(string whereclause = "", int selectedvalue = 0)
        {
            List<SelectListItem> objsli = new List<SelectListItem>();
            objsli.Add(new SelectListItem() { Text = "Journal Voucher", Value = "1" });
            objsli.Add(new SelectListItem() { Text = "Cash Payment", Value = "2" });
            objsli.Add(new SelectListItem() { Text = "Cash Reciept", Value = "3" });
            objsli.Add(new SelectListItem() { Text = "Bank Payment", Value = "4" });
            objsli.Add(new SelectListItem() { Text = "Bank Receipt", Value = "5" });
            objsli.Add(new SelectListItem() { Text = "Opening Voucher", Value = "6" });

            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }
        public SelectList GetVoucherTypes(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from VoucherType  " + whereclause );
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["voucherTypeTitle"].ToString();
                si.Value = dr["VoucherTypeID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;

        }
        public SelectList GetReceiptType(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData("Select * from receiptType  " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();
                si.Text = dr["ReceiptTypeTitle"].ToString();
                si.Value = dr["ReceiptTypeID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;

        }


        public SelectList GetCashAccounts(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData(@"Select ChartofAccount.AccountID,CAST(AccountNo as nvarchar(50))+' - '+AccountTitle As AccountTitle from CashAccounts
inner join ChartofAccount on CashAccounts.CashAccountID=ChartofAccount.AccountID  " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["AccountTitle"].ToString();
                si.Value = dr["AccountID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;

        }
        public string AuthorizedBrances(int userid)
        {

             
            string Branches = "";
            DataTable dtEmployee = General.FetchData(@"Select * from userconfiguration Where UserID= " + userid);
            foreach (DataRow dr in dtEmployee.Rows)
            {
                Branches = Branches + dr["BranchID"] + ",";

            }
            Branches = Branches.Substring(0,Branches.Length-1);
            return Branches;
        }






        public SelectList GetBankAccounts(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtEmployee = General.FetchData(@"Select ChartofAccount.AccountID,CAST(AccountNo as nvarchar(50))+' - '+AccountTitle As AccountTitle from BankAccounts
inner join ChartofAccount on BankAccounts.BankAccountID=ChartofAccount.AccountID  " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtEmployee.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["AccountTitle"].ToString();
                si.Value = dr["AccountID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }


        public SelectList GetPartySelectList(string whereclause = "", int selectedvalue = 0)
        {
            DataTable dtParty = General.FetchData("Select * from PartyInfo " + whereclause);
            List<SelectListItem> objsli = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            foreach (DataRow dr in dtParty.Rows)
            {
                si = new SelectListItem();

                si.Text = dr["Partytitle"].ToString();
                si.Value = dr["PartyID"].ToString();
                objsli.Add(si);
            }
            SelectList sl = new SelectList(objsli, "Value", "Text", selectedvalue);
            return sl;
        }



    }
}
