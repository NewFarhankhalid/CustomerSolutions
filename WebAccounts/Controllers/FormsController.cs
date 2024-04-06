using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Installments.Controllers
{
    public class FormsController : Controller
    {
        // GET: Forms
        public ActionResult Index()
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Allowed) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }

            DataTable dtFormInfo = General.FetchData("Select * from FormInfo");
            List<FormInfo> lstFormInfo = DataTableToObject(dtFormInfo);
            return View(lstFormInfo);
        }

        // GET: FormInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FormInfo/Create
        public ActionResult Create()
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.New) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            Installments.Models.FormInfo objCategoryInfo = new FormInfo();
            //objCategoryInfo.FormID = int.Parse(General.FetchData("Select isnull(max(FormID),0)+1 as FormID from FormInfo").Rows[0]["FormID"].ToString());
            return View(objCategoryInfo);
        }

        // POST: FormInfo/Create
        [HttpPost]
        public ActionResult Create(FormInfo objFormInfo)
        {
            try
            {
                if(objFormInfo.FormID==0)
                {

                    // TODO: Add insert logic here
                    string Query = "Insert into FormInfo (FormTitle,Inactive,Description) ";
                    Query = Query + "Values ('" + objFormInfo.FormTitle + "'," + (objFormInfo.InActive == true ? "1" : "0") + ",'" + objFormInfo.Description + "')";
                    General.ExecuteNonQuery(Query);
                }
                else
                {
                    string Query = "";
                    Query = Query + "UPDATE [dbo].[FormInfo] ";
                    Query = Query + " SET    [FormTitle] ='" + objFormInfo.FormTitle + "' ";
                    Query = Query + "    ,[InActive] = " + (objFormInfo.InActive == true ? "1" : "0") + "";
                    Query = Query + "    ,[Description] ='" + objFormInfo.Description + "' ";
                    Query = Query + "WHERE FormID=" + objFormInfo.FormID;
                    General.ExecuteNonQuery(Query);
                }

                return Json("true");
            }
            catch
            {
                return View();
            }
        }

        // GET: FormInfo/Edit/5
        public ActionResult Edit(int id)
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Edit) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            DataTable dtFormInfo = General.FetchData("Select * from FormInfo where FormID=" + id);
            List<FormInfo> lstFormInfo = DataTableToObject(dtFormInfo);
            if (lstFormInfo.Count > 0)
            {
                return View("Create",lstFormInfo[0]);
            }
            return View();
        }

        // POST: FormInfo/Edit/5
        [HttpPost]
        public ActionResult Edit(FormInfo objFormInfo)
        {
            try
            {
                // TODO: Add update logic here


                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        // GET: FormInfo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Removed) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            String SQL = "Delete From FormInfo where FormID=" + id;
            General.ExecuteNonQuery(SQL);
            return Json("true");
        }
        public ActionResult CheckForms(string formTitle)
        {
            string sql = $@"Select * from FormInfo where FormTitle like '%{formTitle}%'";
            DataTable dt = General.FetchData(sql);
            if(dt.Rows.Count>0)
            {
                return Json("true," + dt.Rows[0]["FormTitle"].ToString());
            }
            else
            {
                return Json("false,");
            }
        }
        List<FormInfo> DataTableToObject(DataTable dt)
        {
            List<FormInfo> lstForm = new List<FormInfo>();
            FormInfo bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new FormInfo();

                if (dr["FormID"] != DBNull.Value)
                {
                    bi.FormID = int.Parse(dr["FormID"].ToString());
                }
                if (dr["FormTitle"] != DBNull.Value)
                {
                    bi.FormTitle = (dr["FormTitle"].ToString());
                }
                if (dr["InActive"] != DBNull.Value)
                {
                    bi.InActive = bool.Parse(dr["InActive"].ToString());
                }
                if (dr["Description"] != DBNull.Value)
                {
                    bi.Description = (dr["Description"].ToString());
                }
                lstForm.Add(bi);
            }
            return lstForm;

        }
    }
}
