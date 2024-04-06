using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Installments.Controllers
{
    public class CustomerInfoController : Controller
    {
        // GET: CustomerInfo
        public ActionResult Index()
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Allowed) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            DataTable dtCustomer = General.FetchData(@"SELECT 
    CustomerInfo.*,  CityName  FROM CustomerInfo     
LEFT OUTER JOIN PakCity ON PakCity.CityID = CustomerInfo.CityID ");
            List<CustomerInfo> lstCustomer = DataTableToObject(dtCustomer);
            return View(lstCustomer);
        }
        
        public JsonResult GetCustomerExpiry(string SystemCustomerID)
        {
            DateTime ExpiryDate = DateTime.Now;
            if(SystemCustomerID== "Null")
            {
                return Json("Please Specify System Customer ID");
            }
            else
            {
                ExpiryDate = DateTime.Parse(General.FetchData($@"Select CustomerExpiryDate.ExpiryDate from CustomerExpiryDate inner join
CustomerInfo on CustomerExpiryDate.CustomerID = CustomerInfo.CustomerID 
Where SystemCustomerID = '{SystemCustomerID}'").Rows[0]["ExpiryDate"].ToString());
            }
            return Json(new { ExpiryDate = ExpiryDate.ToString("MM/dd/yyyy") }, JsonRequestBehavior.AllowGet);
        }
        // GET: CustomerInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CustomerInfo/Create
        public ActionResult Create()
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.New) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            CustomerInfo objCustomer = new CustomerInfo();
            objCustomer.CreatedDate = DateTime.Now;
            objCustomer.ExpiryDate = DateTime.Now;
            ViewBag.City = new DropDown().GetCity();
            ViewBag.PaymentType = new DropDown().GetPaymentType();
            ViewBag.Type = new DropDown().GetSysType();
            ViewBag.Designation = new DropDown().GetDesignation();
            return View(objCustomer);
        }

        // POST: CustomerInfo/Create
        [HttpPost]
        public ActionResult Create(CustomerInfo objCustomer, List<SystemDetail> lstSystemDetail, List<DBName> lstDBName, List<OperatorDetail> lstEmpName)
        {
            try
            {
                if (objCustomer.CustomerID == 0)
                {
                    string Query = "Insert into CustomerInfo (OwnerName,OwnerContactNo,CustomerCompanytitle,Branch,InActive,Description,JoiningDate,CityID,Persontocontact,contactno,Address,BusinessNature,Email,Password,ExpiryDate,LegalUser,Updated,PaymentStatus,SystemCustomerID)";
                    Query = Query + "Values ('" + objCustomer.OwnerName + "','" + objCustomer.OwnerContactNo+ "','" + objCustomer.Branch+"','" + objCustomer.CustomerCompanytitle.Trim() + "'," + (objCustomer.InActive == true ? "1" : "0") + ",'" + objCustomer.Description + "','"+objCustomer.CreatedDate+"',"+objCustomer.CityID+",'"+objCustomer.Persontocontact+"','" + objCustomer.contactno + "','" + objCustomer.Address + "','" + objCustomer.BusinessNature + "','" + objCustomer.Email + "','" + objCustomer.Password + "','" + objCustomer.ExpiryDate + "'," + (objCustomer.LegalUser == true ? "1" : "0") + "," + (objCustomer.Updated == true ? "1" : "0") + "," + (objCustomer.PaymentStatus == true ? "1" : "0") + ",'"+objCustomer.SystemCustomerID+"')";
                    Query = Query + @" Select @@IDENTITY as CustomerID";
                    DataTable dt = General.FetchData(Query);
                    objCustomer.CustomerID = int.Parse(dt.Rows[0]["CustomerID"].ToString());
                    Query = "";
                    foreach (SystemDetail ass in lstSystemDetail)
                    {
                        Query = Query + " Insert into SystemDetail(CustomerID,IPAddress,MacAddress,Type) Values(" + objCustomer.CustomerID + ",'" + ass.IPAddress + "','" + ass.MacAddress + "'," + ass.Type + ")";
                    }
                    if (Query != "")
                    {
                        General.ExecuteNonQuery(Query);
                    }
                    Query = "";
                    foreach (DBName ass in lstDBName)
                    {
                        Query = Query + " Insert into DBName(CustomerID,DBName,PaymentTypeID,Payment) Values(" + objCustomer.CustomerID + ",'" + ass.DbName + "','"+ass.PaymentTypeID+"',"+ass.Payment+")";
                    }
                    if (Query != "")
                    {
                        General.ExecuteNonQuery(Query);
                    }
                    Query = "";
                    foreach (OperatorDetail ass in lstEmpName)
                    {
                        Query = Query + " Insert into EmployeePhone(CustomerID,EmployeeName,EmployeePhone,DesignationID,IsContactPerson) Values(" + objCustomer.CustomerID + ",'" + ass.OperatorName + "','" + ass.OperatorPhone + "'," + ass.DesignationID + "," + (ass.IsContactPerson == true ? "1" : "0") +")";
                    }
                    if (Query != "")
                    {
                        General.ExecuteNonQuery(Query);
                    }
                    new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.New, GeneralAPIsController.LogSource.Customer, objCustomer.CustomerID, " Company Name " + objCustomer.CustomerCompanytitle);
                    return Json("true");
                }
                else
                {
                    string Query = "";
                    Query = Query + "UPDATE [dbo].[CustomerInfo] ";
                    Query = Query + " SET    [CustomerCompanytitle] ='" + objCustomer.CustomerCompanytitle.Trim() + "' ";
                    Query = Query + "    ,[InActive] = " + (objCustomer.InActive == true ? "1" : "0") + "";
                    Query = Query + "    ,[LegalUser] = " + (objCustomer.LegalUser == true ? "1" : "0") + "";
                    Query = Query + "    ,[OwnerName] ='" + objCustomer.OwnerName + "' ";
                    Query = Query + "    ,[OwnerContactNo] ='" + objCustomer.OwnerContactNo + "' ";
                    Query = Query + "    ,[Branch] ='" + objCustomer.Branch + "' ";
                    Query = Query + "    ,[Description] ='" + objCustomer.Description + "' ";
                    Query = Query + "    ,[JoiningDate] ='" + objCustomer.CreatedDate + "' ";
                    Query = Query + "    ,[CityID] =" + objCustomer.CityID + " ";
                    Query = Query + "    ,[Persontocontact] ='" + objCustomer.Persontocontact + "' ";
                    Query = Query + "    ,[contactno] =" + objCustomer.contactno + " ";
                    Query = Query + "    ,[Address] ='" + objCustomer.Address + "' ";
                    Query = Query + "    ,[BusinessNature] ='" + objCustomer.BusinessNature + "' ";
                    Query = Query + "    ,[Email] ='" + objCustomer.Email + "' ";
                    Query = Query + "    ,[Password] ='" + objCustomer.Password + "' ";
                    Query = Query + "    ,[ExpiryDate] ='" + objCustomer.ExpiryDate + "' ";
                    Query = Query + "    ,[Updated] = " + (objCustomer.Updated == true ? "1" : "0") + "";
                    Query = Query + "    ,[PaymentStatus] = " + (objCustomer.PaymentStatus == true ? "1" : "0") + "";
                    Query = Query + "    ,[SystemCustomerID] = '" + objCustomer.SystemCustomerID+ "'";
                    Query = Query + " WHERE CustomerID=" + objCustomer.CustomerID;
                    General.ExecuteNonQuery(Query);
                    Query = "";
                    Query = " Delete from SystemDetail  where CustomerID=" + objCustomer.CustomerID;
                    foreach (SystemDetail ass in lstSystemDetail)
                    {
                        Query = Query + " Insert into SystemDetail(CustomerID,IPAddress,MacAddress,Type) Values(" + objCustomer.CustomerID + ",'" + ass.IPAddress + "','" + ass.MacAddress + "'," + ass.Type + ")";
                    }
                    if (Query != "")
                    {
                        General.ExecuteNonQuery(Query);
                    }
                    Query = "";
                    Query = " Delete from DBName  where CustomerID=" + objCustomer.CustomerID;
                    foreach (DBName ass in lstDBName)
                    {
                        Query = Query + " Insert into DBName(CustomerID,DBName,PaymentTypeID,Payment) Values(" + objCustomer.CustomerID + ",'" + ass.DbName + "','" + ass.PaymentTypeID + "'," + ass.Payment + ")";
                    }
                    if (Query != "")
                    {
                        General.ExecuteNonQuery(Query);
                    }
                    Query = "";
                    Query = " Delete from EmployeePhone  where CustomerID=" + objCustomer.CustomerID;
                    foreach (OperatorDetail ass in lstEmpName)
                    {
                        Query = Query + " Insert into EmployeePhone(CustomerID,EmployeeName,EmployeePhone,DesignationID,IsContactPerson) Values(" + objCustomer.CustomerID + ",'" + ass.OperatorName + "','" + ass.OperatorPhone+ "'," + ass.DesignationID + "," + (ass.IsContactPerson == true ? "1" : "0") + ")";
                    } 
                    if (Query != "")
                    {
                        General.ExecuteNonQuery(Query);
                    }
                    new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Edit, GeneralAPIsController.LogSource.Customer, objCustomer.CustomerID, " Company Name " + objCustomer.CustomerCompanytitle);
                    return Json("true");
                }
            }
            catch (Exception ex)
            {
                return Json("false");
            }
        }
        public ActionResult AddOperator(string OperatorName,string OperatorPhone,int DesignationID, bool IsContactPerson ,int CustomerID )
        {
            DataTable dt = General.FetchData($@"Select * from EmployeePhone Where CustomerID={CustomerID} and EmployeePhone='{OperatorPhone}'");
            if(dt.Rows.Count>0)
            {
                return Json("false,");
            }
            string sql = $@"INSERT INTO EmployeePhone(CustomerID,EmployeeName,EmployeePhone,DesignationID,IsContactPerson) values({CustomerID},'{OperatorName}','{OperatorPhone}',{DesignationID},{(IsContactPerson==true?"1":"0")}
)Select @@identity as OperatorID
";
            int operatorID = int.Parse(General.FetchData(sql).Rows[0]["OperatorID"].ToString());
            return Json("true,"+operatorID);
        }
        [HttpPost]
        public ActionResult ExternalCreate(string Customer, string Contact,int City,string Address,DateTime? CreatedDate,DateTime? ExpiryDate)
        {
            try
            {
                // TODO: Add insert logic here
                string Query = "Insert into CustomerInfo (Customertitle,CityID,Address,JoiningDate,ExpiryDate,ContactNo) ";
                Query = Query + "Values ('" + Customer + "'," + City + ",'"+Address+"','"+CreatedDate+"','"+ExpiryDate+"','"+Contact+"')";
                Query = Query + "   Select @@IDENTITY AS CustomerID";
                int CustomerID = int.Parse(General.FetchData(Query).Rows[0]["CustomerID"].ToString());
                new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.New, GeneralAPIsController.LogSource.Customer, CustomerID, " Customer Name " + Customer);
                return Json(CustomerID);
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerInfo/Edit/5
        public ActionResult Edit(int id)
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Edit) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            DataTable dtCustomer = General.FetchData(@"Select CustomerInfo.*,CityName from CustomerInfo left Outer join PakCity on PakCity.CityID = CustomerINfo.CityID where Customerid=" + id);
            List<CustomerInfo> lstCustomer = DataTableToObject(dtCustomer);
            if (lstCustomer.Count > 0)
            {
                ViewBag.City = new DropDown().GetCity(lstCustomer[0].CityID);
                ViewBag.PaymentType = new DropDown().GetPaymentType();
                ViewBag.Type = new DropDown().GetSysType();
                ViewBag.Designation = new DropDown().GetDesignation();
                return View("Create",lstCustomer[0]);
            }
            return RedirectToAction("index");
        }

        // POST: CustomerInfo/Edit/5
       
        //[HttpPost]
        //public ActionResult Edit(CustomerInfo objCustomer, List<SystemDetail> lstSystemDetail, List<DBName> lstDBName, List<OperatorDetail> lstOperatorName)
        //{
        //    string Query = "Update CustomerInfo Set  CustomerCompanytitle='" + objCustomer.CustomerCompanytitle + "', OwnerName='" + objCustomer.OwnerName + "',OwnerContactNo='" + objCustomer.OwnerContactNo + "',CityName='" + 
        //        objCustomer.CityName + "', InActive=" + objCustomer.InActive+ ", Updated=" + objCustomer.Updated + ",PaymentStatus= " + objCustomer.PaymentStatus + ", LegalUser=" + objCustomer.LegalUser + ", EmployeePhoneNo='" + 
        //        objCustomer.EmployeePhoneNo + "',BusinessNature='" + objCustomer.BusinessNature + "',Password='" + objCustomer.Password + "',Description='" + objCustomer.Description + "',CreatedDate='" + 
        //        objCustomer.CreatedDate + "',CityID=" + objCustomer.CityID + ",Persontocontact='" + objCustomer.Persontocontact + "',contactno='" + objCustomer.contactno + "',Address='" + 
        //        objCustomer.Address + "',PaymentType=" + objCustomer.PaymentType + ",Payment=" + objCustomer.Payment + ",Email='" + objCustomer.Email + "',SystemCustomerID='" +
        //        objCustomer.SystemCustomerID + "',ExpiryDate='" + objCustomer.ExpiryDate + "', ";
        //        General.ExecuteNonQuery(Query);
                
        //    foreach(var item in lstSystemDetail)
        //    {
        //        string Query1 = $@"Insert Into SystemDetail (CustomerID, IpAddress, MacAddress, Type)
        //             Values ({objCustomer.CustomerID},{item.IPAddress},{item.MacAddress},{item.Type})";
        //        General.ExecuteNonQuery(Query1);
        //    }

        //    foreach (var item in lstDBName)
        //    {
        //        string Query2 = $@"Insert Into DBName (CustomerID, DBName)
        //             Values ({objCustomer.CustomerID},{item.DbName})";
        //        General.ExecuteNonQuery(Query2);
        //    }

        //    foreach (var item in lstOperatorName)
        //    {
        //        string Query3 = $@"Insert Into EmployeePhone (CustomerID, EmployeeName, EmployeePhone)
        //             Values ({objCustomer.CustomerID},{item.OperatorName},{item.OperatorPhone})";
        //        General.ExecuteNonQuery(Query3);
        //    }

        //    return Json("true");
        //}
        //public ActionResult Edit(int id, CustomerInfo objCustomer)
        //{
        //    try
        //    {
        //        string Query = "";
        //        Query = Query + "UPDATE [dbo].[CustomerInfo] ";
        //        Query = Query + " SET    [CustomerCompanytitle] ='" + objCustomer.CustomerCompanytitle.Trim() + "' ";
        //        Query = Query + "    ,[InActive] = " + (objCustomer.InActive == true ? "1" : "0") + "";
        //        Query = Query + "    ,[LegalUser] = " + (objCustomer.LegalUser == true ? "1" : "0") + "";
        //        Query = Query + "    ,[OwnerName] ='" + objCustomer.OwnerName + "' ";
        //        Query = Query + "    ,[OwnerContactNo] ='" + objCustomer.OwnerContactNo + "' ";
        //        Query = Query + "    ,[Description] ='" + objCustomer.Description + "' ";
        //        Query = Query + "    ,[CreatedDate] ='" + objCustomer.CreatedDate + "' ";
        //        Query = Query + "    ,[CityID] =" + objCustomer.CityID + " ";
        //        Query = Query + "    ,[Persontocontact] ='" + objCustomer.Persontocontact + "' ";
        //        Query = Query + "    ,[contactno] =" + objCustomer.contactno + " ";
        //        Query = Query + "    ,[Address] ='" + objCustomer.Address + "' ";
        //        Query = Query + "    ,[PaymentType] =" + objCustomer.PaymentType + " ";
        //        Query = Query + "    ,[Payment] =" + objCustomer.Payment + " ";
        //        Query = Query + "    ,[BusinessNature] ='" + objCustomer.BusinessNature + "' ";
        //        Query = Query + "    ,[Email] ='" + objCustomer.Email + "' ";
        //        Query = Query + "    ,[Password] ='" + objCustomer.Password + "' ";
        //        Query = Query + "    ,[ExpiryDate] ='" + objCustomer.ExpiryDate + "' ";
        //        Query = Query + "    ,[Updated] = " + (objCustomer.Updated == true ? "1" : "0") + "";
        //        Query = Query + "    ,[PaymentStatus] = " + (objCustomer.PaymentStatus == true ? "1" : "0") + "";
        //        Query = Query + "    ,[SystemCustomerID] = '" + objCustomer.SystemCustomerID + "'";
        //        Query = Query + " WHERE CustomerID=" + objCustomer.CustomerID;

        //        General.ExecuteNonQuery(Query);
        //        //new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Edit, GeneralAPIsController.LogSource.Customer, objCustomer.CustomerID, " Customer Name " + objCustomer.CustomerCompanytitle);
        //        //new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Edit, GeneralAPIsController.LogSource.ChartofAccount, objCustomer.AccountID, " Account Title " + objCustomer.Customertitle);
        //        return RedirectToAction("Index");
        //        // TODO: Add update logic here

        //        //return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: CustomerInfo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Removed) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            string CompanyTitle = General.FetchData("Select CustomerCompanytitle from CustomerInfo Where CustomerID=" + id).Rows[0]["CustomerCompanytitle"].ToString();
            string Query = "Delete from CustomerInfo Where CustomerID=" + id;
            Query = Query + " Delete From DBName Where CustomerID="+id;
            Query = Query + " Delete From SystemDetail Where CustomerID=" + id;
            General.ExecuteNonQuery(Query);
            new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Delete, GeneralAPIsController.LogSource.Customer, id, " Company Name " + CompanyTitle);
            return Json("true");
        }

        List<CustomerInfo> DataTableToObject(DataTable dt)
        {
            List<CustomerInfo> lstbranch = new List<CustomerInfo>();
            CustomerInfo bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new CustomerInfo();
                if (dr["CustomerID"] != DBNull.Value)
                {
                    bi.CustomerID = int.Parse(dr["CustomerID"].ToString());
                }
                if (dr["OwnerName"] != DBNull.Value)
                {
                    bi.OwnerName = (dr["OwnerName"].ToString());
                }
                if (dr["CityName"] != DBNull.Value)
                {
                    bi.CityName = dr["CityName"].ToString();
                }
                if (dr["OwnerContactNo"] != DBNull.Value)
                {
                    bi.OwnerContactNo = (dr["OwnerContactNo"].ToString());
                }
                if (dr["CustomerCompanytitle"] != DBNull.Value)
                {
                    bi.CustomerCompanytitle = (dr["CustomerCompanytitle"].ToString());
                }
                if (dr["InActive"] != DBNull.Value)
                {
                    bi.InActive = bool.Parse(dr["InActive"].ToString());
                }
                if (dr["Updated"] != DBNull.Value)
                {
                    bi.Updated = bool.Parse(dr["Updated"].ToString());
                }
                if (dr["PaymentStatus"] != DBNull.Value)
                {
                    bi.PaymentStatus = bool.Parse(dr["PaymentStatus"].ToString());
                }
                if (dr["LegalUser"] != DBNull.Value)
                {
                    bi.LegalUser = bool.Parse(dr["LegalUser"].ToString());
                }
                if (dr["Description"] != DBNull.Value)
                {
                    bi.Description = (dr["Description"].ToString());
                }
                if (dr["JoiningDate"] != DBNull.Value)
                {
                    bi.CreatedDate = DateTime.Parse(dr["JoiningDate"].ToString());
                }
                if (dr["CityID"] != DBNull.Value)
                {
                    bi.CityID = int.Parse(dr["CityID"].ToString());
                }
                if (dr["Persontocontact"] != DBNull.Value)
                {
                    bi.Persontocontact = (dr["Persontocontact"].ToString());
                }
                if (dr["contactno"] != DBNull.Value)
                {
                    bi.contactno = (dr["contactno"].ToString());
                }
                if (dr["Address"] != DBNull.Value)
                {
                    bi.Address = (dr["Address"].ToString());
                }                          
                if (dr["Email"] != DBNull.Value)
                {
                    bi.Email = (dr["Email"].ToString());
                }
                if (dr["BusinessNature"] != DBNull.Value)
                {
                    bi.BusinessNature = (dr["BusinessNature"].ToString());
                }
                if (dr["Password"] != DBNull.Value)
                {
                    bi.Password = (dr["Password"].ToString());
                }
                if (dr["SystemCustomerID"] != DBNull.Value)
                {
                    bi.SystemCustomerID = (dr["SystemCustomerID"].ToString());
                }
                if (dr["ExpiryDate"] != DBNull.Value)
                {
                    bi.ExpiryDate = DateTime.Parse(dr["ExpiryDate"].ToString());
                }
                bi.lstSystemDetail = SystemAssociation(bi.CustomerID);
                bi.lstOperatorName = OperatorAssociation(bi.CustomerID);
                bi.lstDBName = DBAssociation(bi.CustomerID);
                lstbranch.Add(bi);
            }
            return lstbranch;
        }
        List<SystemDetail> SystemAssociation(int CustomerID)
        {
            List<SystemDetail> lsSystemDetailAssociation = new List<SystemDetail>();
            DataTable dtVariant = General.FetchData(@"Select * from SystemDetail Where CustomerID=" + CustomerID);
            foreach (DataRow dr in dtVariant.Rows)
            {
                SystemDetail pva = new SystemDetail();
                pva.CustomerID = CustomerID;
                pva.IPAddress = dr["IpAddress"].ToString();
                pva.MacAddress = dr["MacAddress"].ToString();
                pva.Type = int.Parse(dr["Type"].ToString());
                lsSystemDetailAssociation.Add(pva);
            }
            return lsSystemDetailAssociation;
        }
        List<OperatorDetail> OperatorAssociation(int CustomerID)
        {
            List<OperatorDetail> lsSystemDetailAssociation = new List<OperatorDetail>();
            DataTable dtVariant = General.FetchData(@" Select EmployeePhone.*,OperatorDesignation.Name from EmployeePhone
left join OperatorDesignation on EmployeePhone.DesignationID = OperatorDesignation.Id  Where CustomerID=" + CustomerID);
            foreach (DataRow dr in dtVariant.Rows)
            {
                OperatorDetail pva = new OperatorDetail();
                pva.CustomerID = CustomerID;
                pva.OperatorName = dr["EmployeeName"].ToString();
                pva.OperatorPhone = dr["EmployeePhone"].ToString();
                pva.Designation = dr["Name"].ToString();
                if (dr["DesignationID"]!=DBNull.Value)
                {
                    pva.DesignationID = int.Parse(dr["DesignationID"].ToString());
                }
                if (dr["IsContactPerson"]!=DBNull.Value)
                {
                    pva.IsContactPerson = bool.Parse(dr["IsContactPerson"].ToString());
                }

                lsSystemDetailAssociation.Add(pva);
            }
            return lsSystemDetailAssociation;
        }
        List<DBName> DBAssociation(int CustomerID)
        {
            List<DBName> lsDBDetailAssociation = new List<DBName>();
            DataTable dtVariant = General.FetchData(@"Select * from DBName 
left outer join PaymentType on DbName.PaymentTypeID = PaymentType.ID
 Where CustomerID=" + CustomerID);
            foreach (DataRow dr in dtVariant.Rows)
            {
                DBName pva = new DBName();
                pva.CustomerID = CustomerID;
                pva.DbName = dr["DbName"].ToString();
                pva.PaymentType = dr["Name"].ToString();
                if (dr["PaymentTypeID"]!=DBNull.Value)
                {
                    pva.PaymentTypeID = int.Parse(dr["PaymentTypeID"].ToString());
                }
                if (dr["Payment"]!=DBNull.Value)
                {
                    pva.Payment = decimal.Parse(dr["Payment"].ToString());
                }

                if (dr["Amount"] != DBNull.Value)
                {
                    pva.Payment = int.Parse(dr["Amount"].ToString());
                }
                lsDBDetailAssociation.Add(pva);
            }
            return lsDBDetailAssociation;
        }
        
        public ActionResult CustomerInformation(int id)
        {
            string Query = @" SELECT 
    CustomerInfo.*,  CityName FROM CustomerInfo     
LEFT OUTER JOIN PakCity ON PakCity.CityID = CustomerInfo.CityID      
WHERE CustomerInfo.CustomerID =" + id;
            DataTable dtCustomer = General.FetchData(Query);
            List<CustomerInfo> obj = DataTableToObject(dtCustomer);
            if (dtCustomer.Rows.Count > 0)
            {
                CustomerInfo objCustomerInfo = new CustomerInfo();
                ViewBag.Customerid = int.Parse(dtCustomer.Rows[0]["CustomerID"].ToString());
                ViewBag.Customertitle = dtCustomer.Rows[0]["CustomerCompanyTitle"].ToString();
            }
            List<CustomerInfo> lstCustomer = DataTableToObject(dtCustomer);
            return View(obj[0]);
        }

        public ActionResult CustomerExpiryDate(int? CustomerID,int? City, int? Type, DateTime? FromDate,DateTime? ToDate)
        {
            if (CustomerID==null||CustomerID==0)
            {
                CustomerID = 0;
            }
            if (City == null || City == 0)
            {
                City = 0;
            }
            if (Type == null || Type == 0)
            {
                Type = 0;
            }
            if (FromDate == null)
            {
                FromDate = DateTime.Now.AddDays(-30);
            }
            if (ToDate == null)
            {
                ToDate = DateTime.Now;
            }
            ViewBag.Customer = new DropDown().GetCustomer();
            ViewBag.City = new DropDown().GetCity();
            ViewBag.Type = new DropDown().GetPaymentType();
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            ViewBag.ExpiryDate = DateTime.Now.AddDays(30);
            string sql = @"Select * from CustomerInfo left outer join CustomerExpiryDate on CustomerInfo.CustomerID
= CustomerExpiryDate.CustomerID inner join PakCity on CustomerInfo.CityID = PakCity.CityID Where 1=1 " + (CustomerID == 0 ? "" : " and CustomerInfo.CustomerID =" + CustomerID + "") + (City == 0 ? "" : " and CustomerInfo.CityID =" + City)
+(Type==0?"": " and CustomerInfo.PaymentType="+Type) + " and CAST(CustomerInfo.ExpiryDate as Date) between CAST('" + FromDate + "' as date) and Cast('" + ToDate + "' as date)";
            DataTable dt = General.FetchData(sql);
            return View(dt);
        }
        public ActionResult SaveChangeExpiryDate(List<ChangeExpiryDetail> lstChangeExpiryDate,DateTime? ExpiryDate)
        {
            if (lstChangeExpiryDate is null)
            {
                lstChangeExpiryDate = new List<ChangeExpiryDetail>();
            }
            string SQLQuery = "";
            foreach (ChangeExpiryDetail sa in lstChangeExpiryDate)
            {
                SQLQuery = SQLQuery + "     Delete from CustomerExpiryDate Where CustomerID="+sa.CustomerID;
                SQLQuery = SQLQuery + " Insert into CustomerExpiryDate values(" + sa.CustomerID + ",'" + ExpiryDate + "'," + (sa.Inactive == true ? "1" : "0") + ")";
                SQLQuery = SQLQuery + "  Update CustomerInfo Set ExpiryDate='" + ExpiryDate + "' Where CustomerID=" + sa.CustomerID;
            }
            if (!string.IsNullOrEmpty(SQLQuery))
            {
                General.ExecuteNonQuery(SQLQuery);
            }
            return Json("true");
        }
        public ActionResult GetCustomerExpiryInfo(int? CustomerID)
        {
            string sql = @"Select CustomerInfo.ExpiryDate,CustomerExpiryDate.Inactive from CustomerInfo " +
                "left outer join CustomerExpiryDate on CustomerInfo.CustomerID = CustomerExpiryDate.CustomerID Where CustomerInfo.CustomerID=" + CustomerID;
            DataTable dt = General.FetchData(sql);
            DateTime ExpiryDate = DateTime.Now;
            bool Inactive = false;
            int value = 0;
            if(dt.Rows.Count>0)
            {
                ExpiryDate = DateTime.Parse(dt.Rows[0]["ExpiryDate"].ToString());
                Inactive = bool.Parse(dt.Rows[0]["Inactive"].ToString());
                if(Inactive==true)
                {
                    value = 1;
                }
                else
                {
                    value = 0;
                }
                return Json(ExpiryDate + "," + value);
            }
            else
            {
                return Json("0,0");
            }
        }
        public ActionResult AddSingleCustomerExpiry()
        {
            ViewBag.ExpiryDate = DateTime.Now;
            ViewBag.Customer = new DropDown().GetCustomer();
            return View();
        }
        public ActionResult SaveSingleCustomerExpiry(ChangeExpiryDetail objProblem)
        {
            string sql = " Delete from CustomerExpiryDate Where CustomerID=" + objProblem.CustomerID;
            sql = sql + " Insert Into CustomerExpiryDate values("+objProblem.CustomerID+",'"+objProblem.ExpiryDate+"',"+(objProblem.Inactive==true?"1":"0")+")";
            sql = sql + " Update CustomerInfo Set ExpiryDate='" + objProblem.ExpiryDate + "' where CustomerId=" + objProblem.CustomerID;
            General.ExecuteNonQuery(sql);
            return Json("true");
        }
    }
}

