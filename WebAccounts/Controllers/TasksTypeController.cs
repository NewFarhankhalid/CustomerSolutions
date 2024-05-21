using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Installments.Controllers
{
    public class TasksTypeController : Controller
    {
        // GET: TasksType
        public ActionResult Index()
        { 
            DataTable dt = new DataTable();
            dt = General.FetchData($@"Select * From TasksType");
            return View(dt);
        }

        public ActionResult Create()
        {
            TasksType taskType = new TasksType();  
            return View(taskType);
        }

        [HttpPost]
        public ActionResult Create(TasksType objProblem)
        {
            try
            {
                if (objProblem.Id == 0)
                {

                    string Query = "Insert into TasksType ( Name) ";
                    Query = Query + "Values ('" + objProblem.Name + "')";
                    General.ExecuteNonQuery(Query);
                    //Query = "";
                    //Query = Query + " Select @@IDENTITY as Id";
                    //objProblem.Id = int.Parse(General.FetchData(Query).Rows[0]["Id"].ToString());


                    new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.New, GeneralAPIsController.LogSource.TasksType, objProblem.Id, " Name " + objProblem.Name);

                }
                else
                {
                    string Query = "";
                    Query = Query + "UPDATE [dbo].[TasksType] ";
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

        public ActionResult Edit(int Id)
        {
            string sql = $@"Select * from TasksType Where ID ={Id}";
         
            DataTable dt = General.FetchData(sql);
            List<TasksType> lstbranch = DataTableToObject(dt);
            if (lstbranch.Count > 0)
            {
                return View("Create",lstbranch[0]);
            }
            // Insert details
            //foreach (var item in lstDetail)
            //{
            //    string childQuery = $@"INSERT INTO SaleDetailInfo (SaleID, ProductID, Qty, Price)
            //                       VALUES ({collection.SaleID}, {item.ProductID}, {item.Qty}, {item.Price})";
            //    General.ExecuteNonQuery(childQuery);
            //}

            return Json("true");
        }

        public ActionResult CheckTasksType(string TasksType)
        {
            string sql = $@"Select * from TasksType where Name like '%{TasksType}%'";
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

        List<TasksType> DataTableToObject(DataTable dt)
        {
            List<TasksType> lstbranch = new List<TasksType>();
            TasksType bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new TasksType();
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
            string query = "delete from TasksType where Id=" + id;
            General.ExecuteNonQuery(query);
            return Json("true");
        }

    }
}