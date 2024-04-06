using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Installments.Models;

namespace Installments.Controllers
{
    public class TagsInfoController : Controller
    {
        // GET: TagsInfo
        public ActionResult Index()
        {
            DataTable dtTagsInfo = General.FetchData("Select * from TagsInfo");
            List<TagsInfo> lstTagsInfo = DataTableToObject(dtTagsInfo);
            return View(lstTagsInfo);
            
        }

        // GET: TagsInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TagsInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TagsInfo/Create
        [HttpPost]
        public ActionResult Create(TagsInfo objTagsInfo)
        {
            try
            {
                // TODO: Add insert logic here

                string Query = "Insert into TagsInfo (TagTitle,Inactive,Description) ";
                Query = Query + "Values ('" + objTagsInfo.TagTitle + "'," + (objTagsInfo.InActive == true ? "1" : "0") + ",'" + objTagsInfo.Description + "')";
                General.ExecuteNonQuery(Query);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TagsInfo/Edit/5
        public ActionResult Edit(int id)
        {
            DataTable dtTagsInfo = General.FetchData("Select * from TagsInfo where TagsID=" + id);
            List<TagsInfo> lstTagsInfo = DataTableToObject(dtTagsInfo);
            if (lstTagsInfo.Count > 0)
            {
                return View(lstTagsInfo[0]);
            }
            return View();
        }

        // POST: TagsInfo/Edit/5
        [HttpPost]
        public ActionResult Edit(TagsInfo objTagsInfo)
        {
            try
            {
                // TODO: Add update logic here

                string Query = "";
                Query = Query + "UPDATE [dbo].[TagsInfo] ";
                Query = Query + " SET    [TagTitle] ='" + objTagsInfo.TagTitle + "' ";
                Query = Query + "    ,[InActive] = " + (objTagsInfo.InActive == true ? "1" : "0") + "";
                Query = Query + "    ,[Description] ='" + objTagsInfo.Description + "' ";
                Query = Query + "WHERE TagsID=" + objTagsInfo.TagsID;
                General.ExecuteNonQuery(Query);

                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        // GET: TagsInfo/Delete/5
        public ActionResult Delete(int id)
        {
            string SQL = "Delete From TagsInfo where TagsID=" + id;
            General.ExecuteNonQuery(SQL);
            return Json("true");
        }

        // POST: TagsInfo/Delete/5
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

        List<TagsInfo> DataTableToObject(DataTable dt)
        {
            List<TagsInfo> lstTags = new List<TagsInfo>();
            TagsInfo bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new TagsInfo();

                if (dr["TagsID"] != DBNull.Value)
                {
                    bi.TagsID = int.Parse(dr["TagsID"].ToString());
                }

                if (dr["TagTitle"] != DBNull.Value)
                {
                    bi.TagTitle = (dr["TagTitle"].ToString());
                }
                if (dr["InActive"] != DBNull.Value)
                {
                    bi.InActive = bool.Parse(dr["InActive"].ToString());
                }

                if (dr["Description"] != DBNull.Value)
                {
                    bi.Description = (dr["Description"].ToString());
                }
                

                lstTags.Add(bi);


            }
            return lstTags;

        }
    }
}
