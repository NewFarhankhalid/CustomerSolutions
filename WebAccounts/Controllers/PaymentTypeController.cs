using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Installments.Controllers
{
    public class PaymentTypeController : Controller
    {
        // GET: PaymentType
        public ActionResult Index()
        {
            DataTable dt = new DataTable();
            dt = General.FetchData($@"Select * From PaymentType");
            return View(dt);
        }

        public ActionResult Create()
        {
            PaymentType PaymentType = new PaymentType();
            return View(PaymentType);
        }

        [HttpPost]
        public ActionResult Create(Models.PaymentType objProblem)
        {
            try
            {

                if (objProblem.Id == 0)
                {
                    string Query = "Insert into PaymentType (Name,Amount) ";
                    Query = Query + "Values ('" + objProblem.Name + "',";
                    Query = Query +   + objProblem.Amount + ")";
                    //General.ExecuteNonQuery(Query);
                    //Query = "";
                    Query = Query + " Select @@IDENTITY as Id";
                    objProblem.Id = int.Parse(General.FetchData(Query).Rows[0]["Id"].ToString());


                    new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.New, GeneralAPIsController.LogSource.PaymentType, objProblem.Id, " Name " + objProblem.Name);

                }
                else
                {
                    string Query = "";
                    Query = Query + "UPDATE [dbo].[PaymentType] ";
                    Query = Query + " SET    [Name] ='" + objProblem.Name + "' ";
                    Query = Query + " ,   [Amount] =" + objProblem.Amount + " ";
                    Query = Query + " WHERE Id=" + objProblem.Id;
                    General.FetchData(Query);

                    new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Edit, GeneralAPIsController.LogSource.PaymentType, objProblem.Id, " Name " + objProblem.Name);

                }
                return Json("true," + objProblem.Id);
            }
            catch
            {
                return Json("error");
            }
        }

        public ActionResult ChecKPaymentType(string PaymentType)
        {
            string sql = $@"Select * from PaymentType where Name like '%{PaymentType}%'";
            DataTable dt = General.FetchData(sql);
            if (dt.Rows.Count > 0)
            {
                return Json("true," + dt.Rows[0]["Name"].ToString());
            }
            else
            {
                return Json("false,");
            }
        }

        public ActionResult Edit(int Id)
        {
            string sql = $@"Select * from PaymentType Where ID ={Id}";

            DataTable dt = General.FetchData(sql);
            List<PaymentType> lstbranch = DataTableToObject(dt);
            if (lstbranch.Count > 0)
            {
                return View("Create", lstbranch[0]);
            }
            return Json("true");
        }

        List<PaymentType> DataTableToObject(DataTable dt)
        {
            List<PaymentType> lstbranch = new List<PaymentType>();
            PaymentType bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new PaymentType();
                if (dr["Id"] != DBNull.Value)
                {
                    bi.Id = int.Parse(dr["Id"].ToString());
                }
                if (dr["Name"] != DBNull.Value)
                {
                    bi.Name = (dr["Name"].ToString());
                }
                if (dr["Amount"] != DBNull.Value)
                {
                    bi.Amount = int.Parse(dr["Amount"].ToString());
                }

                lstbranch.Add(bi);
            }
            return lstbranch;
        }

        public ActionResult Delete(int id)
        {
            string CompanyTitle = General.FetchData("Select * from PaymentType Where ID=" + id).Rows[0]["Name"].ToString();
            string query = "delete from PaymentType where Id=" + id;
            General.ExecuteNonQuery(query);
            new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Delete, GeneralAPIsController.LogSource.PaymentType, id, " Payment Type " + CompanyTitle);

            return Json("true");
        }

    }
}