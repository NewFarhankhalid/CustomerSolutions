using Installments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace Installments.Controllers
{
    public class TermsandConditionInfoController : Controller
    {
        // GET: TermsandConditionInfo
        public ActionResult Index()
        {
            DataTable dtTerms = General.FetchData("Select * from TermsandconditionInfo");
            List<TermsandconditionInfo> lstTerms = DataTableToObject(dtTerms);
            return View(lstTerms);
        }

        // GET: TermsandConditionInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TermsandConditionInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TermsandConditionInfo/Create
        [HttpPost]
        public ActionResult Create(TermsandconditionInfo TandCInfo)
        {
            try
            {
                // TODO: Add insert logic here

                string Query = "Insert into TermsandConditionInfo (TermsTitle,Inactive,TermsDescription) ";
                Query = Query + "Values ('" + TandCInfo.TermsTitle + "'," + (TandCInfo.Inactive == true ? "1" : "0") + ",'" + TandCInfo.TermsDescription + "')";
                General.ExecuteNonQuery(Query);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TermsandConditionInfo/Edit/5
        public ActionResult Edit(int id)
        {
            DataTable dtTermsandCondition = General.FetchData("Select * from TermsandConditionInfo where TermsandConditionID=" + id);
            List<TermsandconditionInfo> lstTermsandConditionInfo = DataTableToObject(dtTermsandCondition);
            if (lstTermsandConditionInfo.Count > 0)
            {
                return View(lstTermsandConditionInfo[0]);
            }
            return RedirectToAction("index");
        }

        // POST: TermsandConditionInfo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, TermsandconditionInfo obj)
        {
            try
            {
                // TODO: Add update logic here

                string Query = "";
                Query = Query + "UPDATE [dbo].[TermsandConditionInfo] ";
                Query = Query + " SET    [TermsTitle] ='" + obj.TermsTitle + "' ";
                Query = Query + "    ,[Inactive] = " + (obj.Inactive == true ? "1" : "0") + "";
                Query = Query + "    ,[TermsDescription] ='" + obj.TermsDescription + "' ";
                Query = Query + "WHERE TermsandConditionID=" + id;
                General.ExecuteNonQuery(Query);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TermsandConditionInfo/Delete/5
        public JsonResult Delete(int id)
        {

            string Query = "Delete from TermsandCondition Where TermsandConditionID=" + id + " and TermsandConditionID not in (Select TermsandConditionID from ProductInfo)";
            General.ExecuteNonQuery(Query);
            return Json("true");
        }

        

        List<TermsandconditionInfo> DataTableToObject(DataTable dt)
        {
            List<TermsandconditionInfo> lstbranch = new List<TermsandconditionInfo>();
            TermsandconditionInfo bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new TermsandconditionInfo();
                if (dr["TermsandConditionID"] != DBNull.Value)
                {
                    bi.TermsandConditionID = int.Parse(dr["TermsandConditionID"].ToString());
                }
                if (dr["TermsTitle"] != DBNull.Value)
                {
                    bi.TermsTitle = (dr["TermsTitle"].ToString());
                }
                if (dr["TermsDescription"] != DBNull.Value)
                {
                    bi.TermsDescription = (dr["TermsDescription"].ToString());
                }
                if (dr["Inactive"] != DBNull.Value)
                {
                    bi.Inactive = bool.Parse(dr["Inactive"].ToString());
                }
                lstbranch.Add(bi);
            }
            return lstbranch;
        }
    }
}
