using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Installments.Models;

namespace Installments.Controllers
{
    public class ColourInfoController : Controller
    {
        // GET: ColourInfo
        public ActionResult Index()
        {

            DataTable dtColourInfo = General.FetchData("Select * from ColourInfo");
            List<ColourInfo> lstColourInfo = DataTableToObject(dtColourInfo);
            return View(lstColourInfo);
        }

        // GET: ColourInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ColourInfo/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: ColourInfo/Create
        [HttpPost]
        public ActionResult Create(ColourInfo objColourInfo)
        {
            try
            {

                // TODO: Add insert logic here
                string Query = "Insert into ColourInfo (ColourTitle,ColourRGBCode,Inactive,Description) ";
                Query = Query + "Values ('" + objColourInfo.ColourTitle + "','" + objColourInfo.ColourRGBCode + "'," + (objColourInfo.InActive == true ? "1" : "0") + ",'" + objColourInfo.Description + "')";
                General.ExecuteNonQuery(Query);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ColourInfo/Edit/5
        public ActionResult Edit(int id)
        {
            DataTable dtColourInfo = General.FetchData("Select * from ColourInfo where ColourID=" + id);
            List<ColourInfo> lstColourInfo = DataTableToObject(dtColourInfo);
            if (lstColourInfo.Count > 0)
            {
                return View(lstColourInfo[0]);
            }
            return View();
        }

        // POST: ColourInfo/Edit/5
        [HttpPost]
        public ActionResult Edit(ColourInfo objColourInfo)
        {
            try
            {
                // TODO: Add update logic here

                string Query = "";
                Query = Query + "UPDATE [dbo].[ColourInfo] ";
                Query = Query + " SET    [ColourTitle] ='" + objColourInfo.ColourTitle + "' ";
                Query = Query + "    ,[ColourRGBCode] ='" + objColourInfo.ColourRGBCode + "' ";
                Query = Query + "    ,[InActive] = " + (objColourInfo.InActive == true ? "1" : "0") + "";
                Query = Query + "    ,[Description] ='" + objColourInfo.Description + "' ";
                Query = Query + "WHERE ColourID=" + objColourInfo.ColourID;
                General.ExecuteNonQuery(Query);
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        // GET: ColourInfo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            String SQL = "Delete From ColourInfo where ColourID=" + id;
            General.ExecuteNonQuery(SQL);
            return Json("true");

        }

       

        List<ColourInfo> DataTableToObject(DataTable dt)
        {
            List<ColourInfo> lstColour = new List<ColourInfo>();
            ColourInfo bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new ColourInfo();

                if (dr["ColourID"] != DBNull.Value)
                {
                    bi.ColourID = int.Parse(dr["ColourID"].ToString());
                }

                if (dr["ColourTitle"] != DBNull.Value)
                {
                    bi.ColourTitle = (dr["ColourTitle"].ToString());
                }
                if (dr["InActive"] != DBNull.Value)
                {
                    bi.InActive = bool.Parse(dr["InActive"].ToString());
                }

                if (dr["Description"] != DBNull.Value)
                {
                    bi.Description = (dr["Description"].ToString());
                }
                if(dr["ColourRGBCode"] != DBNull.Value)
                {
                    bi.ColourRGBCode = (dr["ColourRGBCode"].ToString());
                }

                lstColour.Add(bi);


            }
            return lstColour;

        }
    }
}
