using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Installments.Controllers
{
    public class WorkCategoryController : Controller
    {
        // GET: WorkStatus
        public ActionResult Index()
        {
            DataTable dt = new DataTable();
            dt = General.FetchData($@"Select * From WorkCategory");
            return View(dt);
        }

        public ActionResult Create()
        {
            WorkCategory WorkCategory = new WorkCategory();
            return View(WorkCategory);
        }

        [HttpPost]
        public ActionResult Create(Models.WorkCategory objProblem)
        {
            try
            {


                if (objProblem.Id == 0)
                {
                    string Query = "Insert into WorkCategory (Name) ";
                    Query = Query + "Values ('" + objProblem.Name + "')";
                    //General.ExecuteNonQuery(Query);
                    //Query = "";
                    Query = Query + " Select @@IDENTITY as Id";
                    objProblem.Id = int.Parse(General.FetchData(Query).Rows[0]["Id"].ToString());


                    new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.New, GeneralAPIsController.LogSource.WorkCategory, objProblem.Id, " Name " + objProblem.Name);

                }

                else
                {
                    string Query = "";
                    Query = Query + "UPDATE [dbo].[WorkCategory] ";
                    Query = Query + " SET    [Name] ='" + objProblem.Name + "' ";
                    Query = Query + " WHERE Id=" + objProblem.Id;
                    General.FetchData(Query);


                }
                return Json("true," + objProblem.Id);
            }
            catch
            {
                return Json("error");
            }
        }
        public ActionResult CheckCategory(string Category)
        {
            string sql = $@"Select * from WorkCategory where Name like '%{Category}%'";
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
            string sql = $@"Select * from WorkCategory Where ID ={Id}";

            DataTable dt = General.FetchData(sql);
            List<WorkCategory> lstbranch = DataTableToObject(dt);
            if (lstbranch.Count > 0)
            {
                return View("Create", lstbranch[0]);
            }
            return Json("true");
        }

        List<WorkCategory> DataTableToObject(DataTable dt)
        {
            List<WorkCategory> lstbranch = new List<WorkCategory>();
            WorkCategory bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new WorkCategory();
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
            string query = "delete from WorkCategory where Id=" + id;
            General.ExecuteNonQuery(query);
            return Json("true");
        }

    }
}