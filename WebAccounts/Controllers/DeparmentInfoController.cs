using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Installments.Controllers
{
    public class DeparmentInfoController : Controller
    {
        // GET: DeparmentInfo
        // GET: Deparment
        public ActionResult Index()
        {
            DataTable dtDeparmentinfo = General.FetchData("Select * from DeparmentInfo");
            List<DeparmentInfo> lstDeparment = DataTableToObject(dtDeparmentinfo);
            return View(lstDeparment);
        }

        // GET: Deparment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Deparment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Deparment/Create
        [HttpPost]
        public ActionResult Create(DeparmentInfo objDeparment)
        {

            try
            {
                string Query = "Insert into DeparmentInfo (DeparmentTitle,InActive,Description) ";
                Query = Query + "Values ('" + objDeparment.DeparmentTitle + "'," + (objDeparment.InActive == true ? "1" : "0") + ",'" + objDeparment.Description + "')";
                General.ExecuteNonQuery(Query);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: Deparment/Edit/5
        public ActionResult Edit(int id)
        {
            DataTable dtDeparment = General.FetchData("Select * from DeparmentInfo where Deparmentid=" + id);
            List<DeparmentInfo> lstDeparment = DataTableToObject(dtDeparment);
            if (lstDeparment.Count > 0)
            {
                return View(lstDeparment[0]);
            }
            return View();
        }
        // POST: Deparment/Edit/5
        [HttpPost]
        public ActionResult Edit(DeparmentInfo objDeparment)
        {
            try
            {
                try
                {
                    string Query = "";
                    Query = Query + "UPDATE [dbo].[DeparmentInfo] ";
                    Query = Query + " SET    [DeparmentTitle] ='" + objDeparment.DeparmentTitle + "' ";
                    Query = Query + "    ,[InActive] = " + (objDeparment.InActive == true ? "1" : "0") + "";
                    Query = Query + "    ,[Description] ='" + objDeparment.Description + "' ";
                    Query = Query + "WHERE DeparmentID=" + objDeparment.DeparmentId;
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
        // GET: Deparment/Delete/5
        public ActionResult Delete(int id)
        {
            string Query = "Delete from DeparmentInfo Where DeparmentID=" + id;
            General.ExecuteNonQuery(Query);
            return Json("true");
        }


        List<DeparmentInfo> DataTableToObject(DataTable dt)
        {
            List<DeparmentInfo> lstbranch = new List<DeparmentInfo>();
            DeparmentInfo bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new DeparmentInfo();
                if (dr["DeparmentId"] != DBNull.Value)
                {
                    bi.DeparmentId = int.Parse(dr["DeparmentId"].ToString());
                }

                if (dr["DeparmentTitle"] != DBNull.Value)
                {
                    bi.DeparmentTitle = (dr["DeparmentTitle"].ToString());
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