using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using Installments.Models;
using System.Data.SqlClient;

namespace Installments.Controllers
{
    public class TasksController : Controller
    {
        // GET: Tasks
        public ActionResult Index()
        {
            //DataTable dt = General.FetchData(@"SELECT * FROM NewProblemStatement where AsignTo =" + id);
            //return View(dt);
            DataTable dt = new DataTable();


            dt = General.FetchData($@"Select UserName,NewProblemStatement.*,CustomerCompanytitle,Name from NewProblemStatement left join UserInfo 
on NewProblemStatement.AssignTo = UserInfo.UserID
inner join TasksType 
on TasksType.Id = NewProblemStatement.WorkPriority
inner join CustomerInfo on CustomerInfo.CustomerID= NewProblemStatement.CustomerID ");


            List<NewProblemStatement> problemStatements = new List<NewProblemStatement>();
            foreach (DataRow row in dt.Rows)
            {
                NewProblemStatement statement = new NewProblemStatement
                {
                    AssignToName = row["UserName"].ToString(),
                    ProblemTitle = row["ProblemTitle"].ToString(),
                    PromiseDate = DateTime.Parse(row["PromiseDate"].ToString()),
                    CustomerTitle = row["CustomerCompanytitle"].ToString(),
                    ProblemStatementID = int.Parse(row["ProblemStatementID"].ToString()),
                    WorkPriorityName = row["Name"].ToString(),

                };
                problemStatements.Add(statement);
            }

            return View(problemStatements);
        }

        //public ActionResult TasksInformation(string id)
        //{
        //    DataTable dt = General.FetchData(@" 
        //SELECT *, UserInfo.UserName, CustomerInfo.CustomerCompanytitle 
        //FROM NewProblemStatement 
        //INNER JOIN CustomerInfo ON NewProblemStatement.CustomerID = CustomerInfo.CustomerID
        //LEFT JOIN UserInfo ON NewProblemStatement.AssignTo = UserInfo.UserID
        //WHERE ProblemStatementID = " + id);

        //    NewProblemStatement model = new NewProblemStatement();
        //    if (dt.Rows.Count > 0)
        //    {
        //        DataRow row = dt.Rows[0];
        //        model.AssignToName = row["AssignTo"].ToString();
        //        model.CustomerTitle = row["CustomerCompanytitle"].ToString();
        //        model.ProblemTitle = row["ProblemTitle"].ToString();
        //    }

        //    return View(model);
        //}

        public ActionResult TasksInformation(int? id)
        {
           
            DataTable dt = General.FetchData($@"Select *,UserInfo.UserName,CustomerInfo.CustomerCompanytitle,WorkCategory.Name as Category from NewProblemStatement 
inner join CustomerInfo on NewProblemStatement.CustomerID = CustomerInfo.CustomerID
left join UserInfo on NewProblemStatement.AssignTo = UserInfo.UserID
left join TasksType on NewProblemStatement.WorkPriority = TasksType.Id
left join WorkCategory on NewProblemStatement.WorkCategory = WorkCategory.Id
 Where ProblemStatementID=" + id);

            NewProblemStatement obj = new NewProblemStatement();
            if (dt.Rows.Count > 0)
            {
                obj.ProblemStatementID = int.Parse(dt.Rows[0]["ProblemStatementID"].ToString());
                obj.ProblemTitle = dt.Rows[0]["ProblemTitle"].ToString();
                obj.Description = dt.Rows[0]["Description"].ToString();
                obj.ProblemImagePath = dt.Rows[0]["ProblemImagePath"].ToString();
                obj.AssignToName = dt.Rows[0]["AssignTo"].ToString();
                obj.ProblemImage = dt.Rows[0]["ProblemImage"].ToString();
                obj.AssignToName = dt.Rows[0]["UserName"].ToString();
                obj.WorkCategory = dt.Rows[0]["Category"].ToString();



                obj.lstFormName = new List<FormName>();
                DataTable dtDetail = General.FetchData($@"Select FormName.FormID,FormName.Description,FormInfo.FormTitle,NewProblemStatement.ProblemStatementID from FormName inner join FormInfo on FormName.FormID = FormInfo.FormID
           inner join NewProblemStatement on FormName.ProblemStatementID = NewProblemStatement.ProblemStatementID  Where FormName.ProblemStatementID=" + id);

                foreach (DataRow item in dtDetail.Rows)
                {
                    FormName lstdt = new FormName();
                    //lstdt.ProblemStatementID = Convert.ToInt32(ProblemStatementID);
                    lstdt.FormID = Convert.ToInt32(item["FormID"]);
                    lstdt.FormTitle = (item["FormTitle"].ToString());
                    lstdt.FormDescription = (item["Description"].ToString());
                  
                    obj.lstFormName.Add(lstdt);
                }

                obj.lstProblemRemarks = new List<ProblemRemarks>();
                DataTable dtt = General.FetchData($@"select * from TasksInformation
Inner join NewProblemStatement
on NewProblemStatement.ProblemStatementID = TasksInformation.ProblemStatementID
left join WorkStatus on TasksInformation.WorkStatus= WorkStatus.Id
 where TasksInformation.ProblemStatementID=" + id);

                foreach (DataRow item in dtt.Rows)
                {
                    ProblemRemarks lstdt = new ProblemRemarks();
                    //lstdt.ProblemStatementID = Convert.ToInt32(ProblemStatementID);
                    lstdt.ProblemStatementID = Convert.ToInt32(item["ProblemStatementID"]);
                    lstdt.WorkStatus = (item["Name"].ToString());
                    lstdt.Remarks = (item["Remarks"].ToString());

                    obj.lstProblemRemarks.Add(lstdt);
                }
            }
            ViewBag.WorkStatus = new DropDown().GetWorkStatus();
            return View(obj);
        }

        [HttpPost]
        public ActionResult TasksInformation(Models.NewProblemStatement obj)
        {
            try
            {
                
                    string Query = "Insert into TasksInformation (UserID, Remarks, WorkStatus, ProblemStatementID)";
                    Query = Query + "Values (" + General.userID + ",'" + obj.Remarks + "','" + obj.WorkStatus + "',"+obj.ProblemStatementID+")";
                    General.ExecuteNonQuery(Query);
           
                return Json("true," + obj.ProblemStatementID);
            }
            catch
            {
                return Json("error");
            }
        }
    }
}