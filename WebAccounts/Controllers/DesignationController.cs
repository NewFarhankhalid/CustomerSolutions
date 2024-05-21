using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
namespace Installments.Controllers
{
    public class DesignationController : Controller
    {
        // GET: Designation
        public ActionResult Index()
        {
            DataTable dtdesignationinfo = General.FetchData("Select * from DesignationInfo");
            List<DesignationInfo> lstDesignation = DataTableToObject(dtdesignationinfo);
            return View(lstDesignation);
        }

        // GET: Designation/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Designation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Designation/Create
        [HttpPost]
        public ActionResult Create(DesignationInfo objdesignation)
        {
            
                try
                {
                    string Query = "Insert into DesignationInfo (DesignationTitle,InActive,Description) ";
                    Query = Query + "Values ('" + objdesignation.DesignationTitle + "'," + (objdesignation.InActive == true ? "1" : "0") + ",'" + objdesignation.Description + "')";
                    General.ExecuteNonQuery(Query);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }              
        }
        // GET: Designation/Edit/5
        public ActionResult Edit(int id)
        {
            DataTable dtdesignation = General.FetchData("Select * from DesignationInfo where Designationid=" + id);
            List<DesignationInfo> lstdesignation = DataTableToObject(dtdesignation);
            if (lstdesignation.Count > 0)
            {
                return View(lstdesignation[0]);
            }
            return View();
        }
        // POST: Designation/Edit/5
        [HttpPost]
        public ActionResult Edit(DesignationInfo objdesignation)
        {
            try
            {
                try
                {
                    string Query = "";
                    Query = Query + "UPDATE [dbo].[DesignationInfo] ";
                    Query = Query + " SET    [DesignationTitle] ='" + objdesignation.DesignationTitle + "' ";
                    Query = Query + "    ,[InActive] = " + (objdesignation.InActive == true ? "1" : "0") + "";
                    Query = Query + "    ,[Description] ='" + objdesignation.Description + "' ";
                    Query = Query + "WHERE DesignationID=" + objdesignation.DesignationId;
                    General.ExecuteNonQuery(Query);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }
        // GET: Designation/Delete/5
        public ActionResult Delete(int id)
        {
            string Query = "Delete from DesignationInfo Where DesignationID=" + id;
            General.ExecuteNonQuery(Query);
            return Json("true");
        }


        List<DesignationInfo> DataTableToObject(DataTable dt)
        {
            List<DesignationInfo> lstbranch = new List<DesignationInfo>();
            DesignationInfo bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new DesignationInfo();
                if (dr["DesignationId"] != DBNull.Value)
                {
                    bi.DesignationId = int.Parse(dr["DesignationId"].ToString());
                }

                if (dr["DesignationTitle"] != DBNull.Value)
                {
                    bi.DesignationTitle = (dr["DesignationTitle"].ToString());
                }
                if (dr["InActive"] != DBNull.Value)
                {
                    bi.InActive = bool.Parse(dr["InActive"].ToString());
                }
              
                if (dr["Description"] != DBNull.Value)
                {
                    bi.Description = (dr["Description"].ToString());
                }
              
                lstbranch.Add(bi);


            }
            return lstbranch;

        }

    }
}
