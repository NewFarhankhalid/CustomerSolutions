using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Installments.Controllers
{
    public class CollectionCenterController : Controller
    {
        // GET: CollectionCenter
        public ActionResult Index()
        {
            DataTable dtcatagory = General.FetchData("Select * from CategoryInfo");
            List<CategoryInfo> lstcatagory = DataTableToObject(dtcatagory);
            return View(lstcatagory);
           
        }

        // GET: CollectionCenter/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CollectionCenter/Create
        public ActionResult Create()
        {
            ViewBag.CategoryInfo = new DropDown().GetBranchInfo();
            return View();
        }

        // POST: CollectionCenter/Create
        [HttpPost]
        public ActionResult Create(CollectionCenter collection)
        {
            try
            {
                string Query = "Insert into CollectionCenter (CenterTitle,Remarks,BranchID) ";
                Query = Query + "Values ('" + collection.CenterTitle + "','" + collection.Remarks + "','" + collection
                    .BranchID + "')";
                General.ExecuteNonQuery(Query);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CollectionCenter/Edit/5
        public ActionResult Edit(int id)
        {
            DataTable dtcatagoryinfo = General.FetchData("Select * from CategoryInfo where CategoryID=" + id);
            List<CategoryInfo> lstbranch = DataTableToObject(dtcatagoryinfo);
            if (lstbranch.Count > 0)
            {
                return View(lstbranch[0]);
            }
            return RedirectToAction("index");

        }

        // POST: CollectionCenter/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CollectionCenter obj)
        {
            try
            {
                string Query = "";
                Query = Query + "UPDATE [dbo].[CollectionCenter] ";
                Query = Query + " SET    [CenterTitle] ='" + obj.CenterTitle + "' ";
                Query = Query + "    ,[Remarks] ='" + obj.Remarks + "' ";
                Query = Query + ",[BranchID]='" + obj.BranchID + "'";

                Query = Query + "WHERE CollectionCenterID=" + id;
                General.ExecuteNonQuery(Query);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CollectionCenter/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CollectionCenter/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        List<CategoryInfo> DataTableToObject(DataTable dt)
        {
            List<CategoryInfo> lstbranch = new List<CategoryInfo>();
            CategoryInfo bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new CategoryInfo();
                if (dr["CategoryID"] != DBNull.Value)
                {
                    bi.CategoryID = int.Parse(dr["CategoryID"].ToString());
                }
                if (dr["CategoryTitle"] != DBNull.Value)
                {
                    bi.CategoryTitle = (dr["CategoryTitle"].ToString());
                }
                if (dr["Description"] != DBNull.Value)
                {
                    bi.Description = (dr["Description"].ToString());
                }
                if (dr["Inactvie"] != DBNull.Value)
                {
                    bi.Inactvie = bool.Parse(dr["Inactvie"].ToString());
                }
                lstbranch.Add(bi);
            }
            return lstbranch;
        }
    }
}