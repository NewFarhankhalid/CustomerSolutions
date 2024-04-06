using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Installments.Models;

namespace Installments.Controllers
{
    public class SizeInfoController : Controller
    {
        // GET: SizeInfo
        public ActionResult Index()
        {
            DataTable dtSizeInfo = General.FetchData("Select * from SizeInfo");
            List<SizeInfo> lstSizeInfo = DataTableToObject(dtSizeInfo);
            return View(lstSizeInfo);
            
        }

        // GET: SizeInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SizeInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SizeInfo/Create
        [HttpPost]
        public ActionResult Create(SizeInfo objSizeInfo)
        {
                try
                {
                    string Query = "Insert into SizeInfo (SizeTitle,InActive,Description) ";
                    Query = Query + "Values ('" + objSizeInfo.SizeTitle + "'," + (objSizeInfo.InActive == true ? "1" : "0") + ",'" + objSizeInfo.Description + "')";
                    General.ExecuteNonQuery(Query);
                    return RedirectToAction("Index");
                }
                catch
                {
                return View();
                }
        }

        // GET: SizeInfo/Edit/5
        public ActionResult Edit(int id)
        {
            DataTable dtSizeInfo = General.FetchData("Select * from SizeInfo where SizeID=" + id);
            List<SizeInfo> lstSizeInfo = DataTableToObject(dtSizeInfo);
            if (lstSizeInfo.Count > 0)
            {
                return View(lstSizeInfo[0]);
            }
            return View();
        }

        // POST: SizeInfo/Edit/5
        [HttpPost]
        public ActionResult Edit(SizeInfo objSizeInfo)
        {
            try
            {
                // TODO: Add update logic here
                try
                {
                    string Query = "";
                    Query = Query + "UPDATE [dbo].[SizeInfo] ";
                    Query = Query + " SET    [SizeTitle] ='" + objSizeInfo.SizeTitle + "' ";
                    Query = Query + "    ,[InActive] = " + (objSizeInfo.InActive == true ? "1" : "0") + "";
                    Query = Query + "    ,[Description] ='" + objSizeInfo.Description + "' ";
                    Query = Query + "WHERE SizeID=" + objSizeInfo.SizeID;
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

        // GET: SizeInfo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            String SQL = "Delete From SizeInfo where SizeID=" + id;
            General.ExecuteNonQuery(SQL);
            return Json("true");
            
        }

        List<SizeInfo> DataTableToObject(DataTable dt)
        {
            List<SizeInfo> lstbranch = new List<SizeInfo>();
            SizeInfo bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new SizeInfo();
                if (dr["SizeID"] != DBNull.Value)
                {
                    bi.SizeID = int.Parse(dr["SizeID"].ToString());
                }

                if (dr["SizeTitle"] != DBNull.Value)
                {
                    bi.SizeTitle = (dr["SizeTitle"].ToString());
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
