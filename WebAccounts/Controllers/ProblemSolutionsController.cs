using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Installments.Controllers
{
    public class ProblemSolutionsController : Controller
    {
        // GET: ProblemSolutions
        public ActionResult Index()
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Allowed) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            DataTable dtProblem = General.FetchData("Select * from ProblemSolution");
            List<ProblemSolutions> lstProblem = DataTableToObject(dtProblem);
            return View(lstProblem);
        }
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.New) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            Installments.Models.ProblemSolutions objProblemInfo = new ProblemSolutions();
            //ViewBag.CategoryInfo = new DropDown().GetCategoryInfo();
            ViewBag.ProblemImage = "";
            return View(objProblemInfo);
        }
        [HttpPost]
        public ActionResult Create(ProblemSolutions objProblem)
        {
            try
            {
                if(objProblem.ProblemSolutionID==0)
                {
                    string Query = "Insert into ProblemSolution (ProblemTitle,Description) ";
                    Query = Query + "Values ('" + objProblem.ProblemTitle + "','" + objProblem.Description + "')";
                    //General.ExecuteNonQuery(Query);
                    //Query = "";
                    Query = Query + " Select @@IDENTITY as ProblemSolutionID";
                    objProblem.ProblemSolutionID = int.Parse(General.FetchData(Query).Rows[0]["ProblemSolutionID"].ToString());
                    new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.New, GeneralAPIsController.LogSource.ProblemSolution, objProblem.ProblemSolutionID, " Problem Title " + objProblem.ProblemTitle);
                }
                else
                {
                    string Query = "";
                    Query = Query + "UPDATE [dbo].[ProblemSolution] ";
                    Query = Query + " SET    [ProblemTitle] ='" + objProblem.ProblemTitle + "' ";
                    Query = Query + "    ,[Description] ='" + objProblem.Description + "' ";
                    Query = Query + ",[ProblemImage]='" + objProblem.ProblemImage + "'";
                    Query = Query + "WHERE ProblemID=" + objProblem.ProblemSolutionID;
                    Query = Query + " Select @@IDENTITY as ProblemID";
                    objProblem.ProblemSolutionID = int.Parse(General.FetchData(Query).Rows[0]["ProblemID"].ToString());
                    if (Query != "")
                    {
                        General.ExecuteNonQuery(Query);
                    }
                    new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Edit, GeneralAPIsController.LogSource.ProblemSolution, objProblem.ProblemSolutionID, " Problem Title " + objProblem.ProblemTitle);

                }

                return Json("true," + objProblem.ProblemSolutionID);
            }
            catch
            {
                return Json("error");
            }
        }




        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveOrUpdateImage(HttpPostedFileBase txtfile)
        {

            if (txtfile != null && txtfile.ContentLength > 0)
            {
                string[] filecontent = txtfile.FileName.ToString().Split(',');
                string filename = filecontent[0] + "-" + filecontent[1] + ".jpeg";
                int ProblemID = int.Parse(filecontent[0]);
                string fileext = Path.GetExtension(filename);
                if (fileext == ".jpg" || fileext == ".png" || fileext == ".jpeg")
                {
                    bool exists = Directory.Exists(Server.MapPath("/ProblemSolutionImage/"));
                    if (!exists)
                    {
                        var createfolder = Path.Combine(Server.MapPath("/ProblemSolutionImage/").ToString());
                        System.IO.Directory.CreateDirectory(createfolder);
                        exists = true;
                    }
                    string filepath = ("/ProblemSolutionImage/") + filename;
                    //string filepath = Server.MapPath("~/ProductImages/")+ filename;
                    string sql = @"update ProblemSolution set ProblemImage ='" + filepath + "'  where ProblemSolution.ProblemSolutionID=" + ProblemID + "";
                    General.ExecuteNonQuery(sql);
                    txtfile.SaveAs(Server.MapPath(filepath));
                }
            }
            return Json("true");
        }
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase imagefile)
        {
            string _FileName = Path.GetFileName(imagefile.FileName);

            if (Request.Files.Count > 0)
            {
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/ProblemSolutionImage";
                    string filename = Path.GetFileName(Request.Files[i].FileName);

                    HttpPostedFileBase filex = files[i];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = filex.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = filex.FileName;
                        var roothpath = Server.MapPath("/ProblemSolutionImage/");
                        var filesavepath = System.IO.Path.Combine(roothpath, fname);
                        imagefile.SaveAs(filesavepath);
                        //string sql = "insert into productInfo(ProductImage) values ('" + filesavepath + "')";
                        return View("Index");
                    }

                    // Get the complete folder path and store the file inside it.
                    //string _path = Path.Combine(Server.MapPath("~/ProductImages/"), fname);
                    //txtfile.SaveAs(_path);

                }
            }
            return Json("Image Uploaded");
        }



        public ActionResult Edit(int id)
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Edit) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            DataTable dtProbleminfo = General.FetchData("Select * from ProblemSolution where ProblemSolutionID=" + id);
            List<ProblemSolutions> lstbranch = DataTableToObject(dtProbleminfo);
            if (lstbranch.Count > 0)
            {
                return View("Create",lstbranch[0]);
            }
            return RedirectToAction("index");
        }

        // POST: CategoryInfo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CategoryInfo obj)
        {
            try
            {
                
                return Json("true," + obj.CategoryID);
            }
            catch
            {
                return Json("error");
            }
        }

        // GET: CategoryInfo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Removed) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            string ProblemTitle = General.FetchData("Select ProblemTitle from ProblemSolution").Rows[0]["ProblemTitle"].ToString();
            string Query = "Delete from ProblemSolution Where ProblemSolutionID=" + id;
            General.ExecuteNonQuery(Query);
            new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Delete, GeneralAPIsController.LogSource.ProblemSolution, id, " Problem Title " + ProblemTitle);
            return Json("true");
        }

        List<ProblemSolutions> DataTableToObject(DataTable dt)
        {
            List<ProblemSolutions> lstbranch = new List<ProblemSolutions>();
            ProblemSolutions bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new ProblemSolutions();
                if (dr["ProblemSolutionID"] != DBNull.Value)
                {
                    bi.ProblemSolutionID = int.Parse(dr["ProblemSolutionID"].ToString());
                }
                if (dr["ProblemTitle"] != DBNull.Value)
                {
                    bi.ProblemTitle = (dr["ProblemTitle"].ToString());
                }
                if (dr["Description"] != DBNull.Value)
                {
                    bi.Description = (dr["Description"].ToString());
                }
                if (dr["ProblemImage"] != DBNull.Value)
                {
                    bi.ProblemImage = (dr["ProblemImage"].ToString());
                    ViewBag.ProblemImage = (dr["ProblemImage"].ToString());
                }
                lstbranch.Add(bi);
            }
            return lstbranch;
        }
        public ActionResult ProblemInformation(int id)
        {
            string Query = @" Select * from ProblemSolution Where ProblemSolutionID=" + id;
            DataTable dtparty = General.FetchData(Query);
            if (dtparty.Rows.Count > 0)
            {
                ProblemSolutions objpartyInfo = new ProblemSolutions();
                ViewBag.partyid = int.Parse(dtparty.Rows[0]["ProblemSolutionID"].ToString());
                ViewBag.Partytitle = dtparty.Rows[0]["ProblemTitle"].ToString();
                ViewBag.ProblemImage = (dtparty.Rows[0]["ProblemImage"].ToString());
            }
            List<ProblemSolutions> lstparty = DataTableToObject(dtparty);
            return View(lstparty[0]);
        }
    }
}
