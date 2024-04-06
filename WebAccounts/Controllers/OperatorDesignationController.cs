using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Installments.Controllers
{
    public class OperatorDesignationController : Controller
    {
        // GET: OperatorDesignation
        public ActionResult Index()
        {
            DataTable dt = new DataTable();
            dt = General.FetchData($@"Select * From OperatorDesignation");
            return View(dt);
        }

        public ActionResult Create()
        {
            OperatorDesignation OperatorDesignation = new OperatorDesignation();
            return View(OperatorDesignation);
        }

        [HttpPost]
        public ActionResult Create(Models.OperatorDesignation objProblem)
        {
            try
            {

                if (objProblem.Id == 0)
                {
                    string Query = "Insert into OperatorDesignation (Name) ";
                    Query = Query + "Values ('" + objProblem.Name + "')";
                    //General.ExecuteNonQuery(Query);
                    //Query = "";
                    Query = Query + " Select @@IDENTITY as Id";
                    objProblem.Id = int.Parse(General.FetchData(Query).Rows[0]["Id"].ToString());


                    new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.New, GeneralAPIsController.LogSource.OperatorDesignation, objProblem.Id, " Name " + objProblem.Name);

                }
                else
                {
                    string Query = "";
                    Query = Query + "UPDATE [dbo].[OperatorDesignation] ";
                    Query = Query + " SET    [Name] ='" + objProblem.Name + "' ";
                    Query = Query + " WHERE Id=" + objProblem.Id;
                    General.FetchData(Query);

                    new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Edit, GeneralAPIsController.LogSource.OperatorDesignation, objProblem.Id, " Name " + objProblem.Name);

                }
                return Json("true," + objProblem.Id);
            }
            catch
            {
                return Json("error");
            }
        }

        public ActionResult CheckDesignation(string Designation)
        {
            string sql = $@"Select * from OperatorDesignation where Name like '%{Designation}%'";
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
            string sql = $@"Select * from OperatorDesignation Where ID ={Id}";

            DataTable dt = General.FetchData(sql);
            List<OperatorDesignation> lstbranch = DataTableToObject(dt);
            if (lstbranch.Count > 0)
            {
                return View("Create", lstbranch[0]);
            }
            return Json("true");
        }

        List<OperatorDesignation> DataTableToObject(DataTable dt)
        {
            List<OperatorDesignation> lstbranch = new List<OperatorDesignation>();
            OperatorDesignation bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new OperatorDesignation();
                if (dr["Id"] != DBNull.Value)
                {
                    bi.Id = int.Parse(dr["Id"].ToString());
                }
                if (dr["Name"] != DBNull.Value)
                {
                    bi.Name = (dr["Name"].ToString());
                }

                lstbranch.Add(bi);
            }
            return lstbranch;
        }

        public ActionResult Delete(int id)
        {
            string CompanyTitle = General.FetchData("Select Name from OperatorDesignation Where ID=" + id).Rows[0]["Name"].ToString();
            string query = "delete from WorkStatus where Id=" + id;
            General.ExecuteNonQuery(query);
            new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Delete, GeneralAPIsController.LogSource.OperatorDesignation, id, " Operator Designation " + CompanyTitle);

            return Json("true");
        }

    }
}