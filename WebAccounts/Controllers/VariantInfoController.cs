using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Installments.Models;
using System.Data;
namespace Installments.Controllers
{
    public class VariantInfoController : Controller
    {
        // GET: Designation
        public ActionResult Index()
        {
            DataTable dtvariant = General.FetchData("Select * from VariantsInfo");
            List<VariantInfo> lstVariant = DataTableToObject(dtvariant);
            return View(lstVariant);
        }

        // GET: Designation/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Designation/Create
        public ActionResult Create()
        {
            
            return View(new VariantInfo());
        }

        // POST: Designation/Create
        [HttpPost]
        public ActionResult Create(VariantInfo objdesignation)
        {

            try
            {
                if (objdesignation.VariantID == 0)
                {
                    string Query = "Insert into VariantsInfo (Title,ShortTitle,Measurement,InActive,Remarks) ";
                    Query = Query + "Values ('" + objdesignation.Title + "','" + objdesignation.ShortTitle + "',"+objdesignation.Measurement+"," + (objdesignation.inactive == true ? "1" : "0") + ",'" + objdesignation.remarks + "')";
                    General.ExecuteNonQuery(Query);
                }
                else
                {
                    string Query = "";
                    Query = Query + "UPDATE [dbo].[VariantsInfo] ";
                    Query = Query + " SET    [Title] ='" + objdesignation.Title+ "' ";
                    Query = Query + " ,[ShortTitle] ='" + objdesignation.ShortTitle+ "' ";
                    Query = Query + "    ,[Measurement] = " + (objdesignation.Measurement) + "";
                    Query = Query + "    ,[InActive] = " + (objdesignation.inactive == true ? "1" : "0") + "";
                    Query = Query + "    ,[remarks] ='" + objdesignation.remarks + "' ";
                    Query = Query + "WHERE VariantID=" + objdesignation.VariantID;
                    General.ExecuteNonQuery(Query);
                }
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
            DataTable dtdesignation = General.FetchData("Select * from VariantsInfo where VariantId=" + id);
            List<VariantInfo> lstdesignation = DataTableToObject(dtdesignation);
            if (lstdesignation.Count > 0)
            {
                return View("Create",lstdesignation[0]);
            }
            return View();
        }
        // POST: Designation/Edit/5
        [HttpPost]
        public ActionResult Edit(VariantInfo objdesignation)
        {
            try
            {
                try
                {
                    string Query = "";
                    Query = Query + "UPDATE [dbo].[VariantsInfo] ";
                    Query = Query + " SET    [Title] ='" + objdesignation.Title + "' ";
                    Query = Query + " [ShortTitle] ='" + objdesignation.ShortTitle + "' ";
                    Query = Query + "    ,[Measurement] = " + (objdesignation.Measurement) + "";
                    Query = Query + "    ,[InActive] = " + (objdesignation.inactive == true ? "1" : "0") + "";
                    Query = Query + "    ,[remarks] ='" + objdesignation.remarks + "' ";
                    Query = Query + "WHERE VariantID=" + objdesignation.VariantID;
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
            string Query = "Delete from VariantsInfo Where VariantId=" + id;
            General.ExecuteNonQuery(Query);
            return Json("true");
        }


        List<VariantInfo> DataTableToObject(DataTable dt)
        {
            List<VariantInfo> lstbranch = new List<VariantInfo>();
            VariantInfo bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new VariantInfo();
                if (dr["VariantID"] != DBNull.Value)
                {
                    bi.VariantID = int.Parse(dr["VariantID"].ToString());
                }

                if (dr["VariantTitle"] != DBNull.Value)
                {
                    bi.Title = (dr["VariantTitle"].ToString());
                }

                if (dr["ShortTitle"] != DBNull.Value)
                {
                    bi.ShortTitle = (dr["ShortTitle"].ToString());
                }
                if (dr["Measurement"] != DBNull.Value)
                {
                    bi.Measurement = double.Parse(dr["Measurement"].ToString());
                }
                if (dr["InActive"] != DBNull.Value)
                {
                    bi.inactive = bool.Parse(dr["InActive"].ToString());
                }

                if (dr["remarks"] != DBNull.Value)
                {
                    bi.remarks = (dr["remarks"].ToString());
                }
                lstbranch.Add(bi);
            }
            return lstbranch;
        }
    }
}
