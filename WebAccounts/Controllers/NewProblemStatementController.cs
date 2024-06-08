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
    public class NewProblemStatementController : Controller
    {
        // GET: NewProblemStatement
        public ActionResult Index()
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Allowed) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            ViewBag.SolvedType = new DropDown().GetSolved();
            DataTable dtProblem = General.FetchData(@"Select *,UserInfo.UserName,CustomerInfo.CustomerCompanytitle,WorkCategory.Name as Category from NewProblemStatement 
inner join CustomerInfo on NewProblemStatement.CustomerID = CustomerInfo.CustomerID
left join UserInfo on NewProblemStatement.AssignTo = UserInfo.UserID
left join TasksType on NewProblemStatement.WorkPriority = TasksType.Id
left join WorkCategory on NewProblemStatement.WorkCategory = WorkCategory.Id  order by ProblemStatementID desc
");
            List<NewProblemStatement> lstProblem = DataTableToObject(dtProblem);
            return View(lstProblem);
        }
        public ActionResult Create()
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.New) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            Installments.Models.NewProblemStatement objProblemInfo = new NewProblemStatement();
            //ViewBag.CategoryInfo = new DropDown().GetCategoryInfo();
            ViewBag.ProblemImage = "";
            ViewBag.Customer = new DropDown().GetCustomerInNewProblem();
            ViewBag.FormID = new DropDown().GetForms();
            objProblemInfo.PromiseDate = DateTime.Now;
            objProblemInfo.EntryDate = DateTime.Now;
            ViewBag.AssignToList = new DropDown().GetUserList();
            ViewBag.WorkPriority = new DropDown().GetWorkPriority();
            ViewBag.OperatorName = new DropDown().GetEmptyList();
            ViewBag.Designation = new DropDown().GetDesignation();
            ViewBag.WorkCategory = new DropDown().GetWorkCategory();
            objProblemInfo.ProblemStatmentNo = General.FetchData("Select 'SS-CN-'+Cast((isnull(Count(ProblemStatementID),1)+1)as nvarchar)ComplaintNo from NewProblemStatement").Rows[0][0].ToString();
            return View(objProblemInfo);
        }
        [HttpPost]
        public ActionResult Create(NewProblemStatement objProblem, List<FormName> lstFormName, HttpPostedFileBase VoiceFile)
        {
            try
            {
                if (lstFormName == null)
                {
                    lstFormName = new List<FormName>();
                }

                // Save voice file
                if (VoiceFile != null && VoiceFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(VoiceFile.FileName);
                    string directoryPath = Server.MapPath("~/VoiceUploads/");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    string filePath = Path.Combine(directoryPath, fileName);
                    VoiceFile.SaveAs(filePath);
                    objProblem.Voice = filePath;
                }

                if (objProblem.ProblemStatementID == 0)
                {
                    // Insert new problem statement
                    string query = "INSERT INTO NewProblemStatement (ComplaintNo, CustomerID, ProblemTitle, PromiseDate, EntryDate, AssignTo, WorkPriority, ProblemImagePath, Voice, Solved, CreatedID, CreatedDate, CallTimeDuration, OperatorID, WorkCategory) ";
                    query += "VALUES ('" + objProblem.ProblemStatmentNo + "', " + objProblem.CustomerID + ", '" + objProblem.ProblemTitle + "', '" + objProblem.PromiseDate + "', '" + objProblem.EntryDate + "', " + objProblem.AssignTo + ", " + objProblem.WorkPriority + ", '" + objProblem.ProblemImagePath + "', '" + objProblem.Voice + "', 0, " + General.userID + ", GETDATE(), '" + objProblem.Calltime + "', " + objProblem.OperatorID + ", '" + objProblem.WorkCategory + "')";
                    query += @" Select @@IDENTITY as ProblemStatementID";
                    // Execute query to insert new problem statement
                    DataTable dt = General.FetchData(query);

                    objProblem.ProblemStatementID = int.Parse(dt.Rows[0]["ProblemStatementID"].ToString());


                    // Insert related form names
                    query = "";
                    foreach (FormName ass in lstFormName)
                    {
                        query += " INSERT INTO FormName(ProblemStatementID, FormID, Description) VALUES(" + objProblem.ProblemStatementID + ", " + ass.FormID + ", '" + ass.FormDescription + "')";
                    }
                    if (!string.IsNullOrEmpty(query))
                    {
                        General.ExecuteNonQuery(query);
                    }
                    //foreach(OperatorDetail ass in lstOperatorDetail)
                    //{
                    //    Query = Query + "Insert Into OperatorInfoNPS(ProblemStatementID,OperatorName,OperatorPhone,IsContactPerson) Values(" + objProblem.ProblemStatementID + ",'" + ass.OperatorName + "','" + ass.OperatorPhone + "'," + (ass.IsContactPerson == true ? "1" : "0") + ")";
                    //}
                    //if (Query != "")
                    //{
                    //    General.ExecuteNonQuery(Query);
                    //}
                    new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.New, GeneralAPIsController.LogSource.NewProblemStatement, objProblem.ProblemStatementID, " Problem Title " + objProblem.ProblemTitle );
                }
                else
                {
                    string query = "UPDATE [dbo].[NewProblemStatement] ";
                    query += "SET [ProblemTitle] = '" + objProblem.ProblemTitle + "', ";
                    query += "[CustomerID] = " + objProblem.CustomerID + ", ";
                    query += "[ComplaintNo] = '" + objProblem.ProblemStatmentNo + "', "; // Wrapped in single quotes
                    query += "[PromiseDate] = '" + objProblem.PromiseDate + "', "; // Wrapped in single quotes
                    query += "[EntryDate] = '" + objProblem.EntryDate + "', "; // Wrapped in single quotes
                    query += "[ProblemImage] = '" + objProblem.ProblemImage + "', "; // Wrapped in single quotes
                    query += "[AssignTo] = " + objProblem.AssignTo + ", ";
                    query += "[WorkPriority] = " + objProblem.WorkPriority + ", ";
                    query += "[ProblemImagePath] = '" + objProblem.ProblemImagePath + "', "; // Wrapped in single quotes
                    query += "[Voice] = '" + objProblem.Voice + "', "; // Wrapped in single quotes
                    query += "[CallTimeDuration] = '" + objProblem.Calltime + "', "; // Wrapped in single quotes
                    query += "[OperatorID] = " + objProblem.OperatorID + ", ";
                    query += "[WorkCategory] = '" + objProblem.WorkCategory + "' ";
                    query += "WHERE ProblemStatementID = " + objProblem.ProblemStatementID;

                    General.FetchData(query);


                    // Delete old form names and insert new ones
                    query = "DELETE FROM FormName WHERE ProblemStatementID = " + objProblem.ProblemStatementID;
                    foreach (FormName ass in lstFormName)
                    {
                        query += " INSERT INTO FormName(ProblemStatementID, FormID, Description) VALUES(" + objProblem.ProblemStatementID + ", " + ass.FormID + ", '" + ass.FormDescription + "')";
                    }
                    if (!string.IsNullOrEmpty(query))
                    {
                        General.ExecuteNonQuery(query);
                    }
                    //foreach (OperatorDetail ass in lstOperatorDetail)
                    //{
                    //    Query = Query + "Insert Into OperatorInfoNPS(ProblemStatementID,OperatorName,OperatorPhone,IsContactPerson) Values(" + objProblem.ProblemStatementID + ",'" + ass.OperatorName + "','" + ass.OperatorPhone + "'," + (ass.IsContactPerson == true ? "1" : "0") + ")";
                    //}
                    //if (Query != "")
                    //{
                    //    General.ExecuteNonQuery(Query);
                    //}
                    new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Edit, GeneralAPIsController.LogSource.NewProblemStatement, objProblem.ProblemStatementID, " Problem Title " + objProblem.ProblemTitle);
                }
                return Json(new { success = true, problemId = objProblem.ProblemStatementID });
            }
            catch (Exception ex)
            {
                // Log the exception
                // You can log the exception details here using a logging framework like log4net or write to a log file
                // Example: log.Error("Error in Create method", ex);
                return Json(new { success = false, message = ex.Message });
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
                    bool exists = Directory.Exists(Server.MapPath("/NewProblemStatementImage/"));
                    if (!exists)
                    {
                        var createfolder = Path.Combine(Server.MapPath("/NewProblemStatementImage/").ToString());
                        System.IO.Directory.CreateDirectory(createfolder);
                        exists = true;
                    }
                    string filepath = ("/NewProblemStatementImage/") + filename;
                    //string filepath = Server.MapPath("~/ProductImages/")+ filename;
                    string sql = @"update NewProblemStatement set ProblemImage ='" + filepath + "'  where ProblemStatementID=" + ProblemID + "";
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
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/NewProblemStatementImage";
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
                        var roothpath = Server.MapPath("/NewProblemStatementImage/");
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

        public ActionResult ProblemStatementInformation(int id)
        {
            string Query = @" 
Select *,UserInfo.UserName,CustomerInfo.CustomerCompanytitle,WorkCategory.Name as Category from NewProblemStatement 
inner join CustomerInfo on NewProblemStatement.CustomerID = CustomerInfo.CustomerID
left join UserInfo on NewProblemStatement.AssignTo = UserInfo.UserID
left join TasksType on NewProblemStatement.WorkPriority = TasksType.Id
left join WorkCategory on NewProblemStatement.WorkCategory = WorkCategory.Id
 Where ProblemStatementID=" + id;
            DataTable dtparty = General.FetchData(Query);
            if (dtparty.Rows.Count > 0)
            {
                
                //NewProblemStatement objpartyInfo = new NewProblemStatement();
                //ViewBag.CustomerName = dtparty.Rows[0]["CustomerCompanytitle"].ToString();
                //ViewBag.partyid = int.Parse(dtparty.Rows[0]["ProblemStatementID"].ToString());
                //ViewBag.Partytitle = dtparty.Rows[0]["ProblemTitle"].ToString();
                //ViewBag.User = dtparty.Rows[0]["AssignTo"].ToString();
                //ViewBag.ProblemImage = (dtparty.Rows[0]["ProblemImage"].ToString());
                //ViewBag.Solved = bool.Parse(dtparty.Rows[0]["Solved"].ToString());
            }
            List<NewProblemStatement> lstparty = DataTableToObject(dtparty);
            return View(lstparty[0]);
        }
        [HttpPost]
        public ActionResult UploadVoice(HttpPostedFileBase voiceFile)
        {
            try
            {
                if (voiceFile != null && voiceFile.ContentLength > 0)
                {
                    // Save the voice file to the server or perform any necessary operations
                    var fileName = Path.GetFileName(voiceFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/VoiceUploads/"), fileName);
                    voiceFile.SaveAs(path);

                    // Optionally, you can return some data to the client-side to indicate success
                    return Json(new { success = true, message = "Voice file uploaded successfully" });
                }
                else
                {
                    return Json(new { success = false, message = "No voice file received" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error uploading voice file: " + ex.Message });
            }
        }

        public ActionResult ProblemSolved(int id)
        {
            string ProblemTitle = General.FetchData("Select * from NewProblemStatement Where ProblemStatementID=" + id).Rows[0]["ProblemTitle"].ToString();
            string Query = @" UPDATE [dbo].[NewProblemStatement] 
Set Solved=1 
Where ProblemStatementID=" + id;
            General.ExecuteNonQuery(Query);
            new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Edit, GeneralAPIsController.LogSource.NewProblemStatement, id, " Problem Title " + ProblemTitle);
            //return Json("true", JsonRequestBehavior.AllowGet);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Edit) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
//            DataTable dtProbleminfo = General.FetchData(@"Select *,CustomerInfo.CustomerCompanytitle from NewProblemStatement 
//inner join CustomerInfo
//on NewProblemStatement.CustomerID = CustomerInfo.CustomerID where ProblemStatementID=" + id);
            DataTable dtProbleminfo = General.FetchData(@"Select *,UserInfo.UserName,CustomerInfo.CustomerCompanytitle,WorkCategory.Name as Category from NewProblemStatement 
inner join CustomerInfo on NewProblemStatement.CustomerID = CustomerInfo.CustomerID
left join UserInfo on NewProblemStatement.AssignTo = UserInfo.UserID
left join TasksType on NewProblemStatement.WorkPriority = TasksType.Id
left join WorkCategory on NewProblemStatement.WorkCategory = WorkCategory.Id
 Where ProblemStatementID=" + id);
            List<NewProblemStatement> lstbranch = DataTableToObject(dtProbleminfo);
            if (lstbranch.Count > 0)
            { 
                ViewBag.Customer = new DropDown().GetCustomerInNewProblem("", lstbranch[0].CustomerID);
            ViewBag.FormID = new DropDown().GetForms();
            ViewBag.AssignToList = new DropDown().GetUserList("", lstbranch[0].UserID);
            ViewBag.WorkPriority = new DropDown().GetWorkPriority("", lstbranch[0].WorkPriority);
                ViewBag.Designation = new DropDown().GetDesignation();
                ViewBag.WorkCategory = new DropDown().GetWorkCategory();
                ViewBag.OperatorName = new DropDown().GetEmptyList();
                ViewBag.Voice = lstbranch[0].Voice;
                return View("Create", lstbranch[0]);
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
            string ProblemTitle = General.FetchData("Select ProblemTitle From NewProblemStatement Where ProblemStatementID=" + id).Rows[0]["ProblemTitle"].ToString();
            string Query = "Delete from NewProblemStatement Where ProblemStatementID=" + id;
            Query = Query + "Delete from FormInfo Where FormID=" + id;
            new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Delete, GeneralAPIsController.LogSource.NewProblemStatement, id, " Problem Title " + ProblemTitle);
            General.ExecuteNonQuery(Query);
            return Json("true");
        }
        List<NewProblemStatement> DataTableToObject(DataTable dt)
        {
            List<NewProblemStatement> lstbranch = new List<NewProblemStatement>();
            NewProblemStatement bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new NewProblemStatement();
                if (dr["ProblemStatementID"] != DBNull.Value)
                {
                    bi.ProblemStatementID = int.Parse(dr["ProblemStatementID"].ToString());
                }
                if (dr["CustomerID"] != DBNull.Value)
                {
                    bi.CustomerID = int.Parse(dr["CustomerID"].ToString());
                }
                if (dr["ComplaintNo"] != DBNull.Value)
                {
                    bi.ProblemStatmentNo = (dr["ComplaintNo"].ToString());
                }
                if (dr["Solved"] != DBNull.Value)
                {
                    bi.Solved = bool.Parse(dr["Solved"].ToString());
                }
                if (dr["CustomerCompanytitle"] != DBNull.Value)
                {
                    bi.CustomerTitle = dr["CustomerCompanytitle"].ToString();
                }
                if (dr["ProblemTitle"] != DBNull.Value)
                {
                    bi.ProblemTitle = (dr["ProblemTitle"].ToString());
                }
                if (dr["Description"] != DBNull.Value)
                {
                    bi.Description = (dr["Description"].ToString());
                }
                if (dr["ProblemImagePath"] != DBNull.Value)
                {
                    bi.ProblemImagePath = (dr["ProblemImagePath"].ToString());
                }
                if (dr["PromiseDate"] != DBNull.Value)
                {
                    bi.PromiseDate = DateTime.Parse(dr["PromiseDate"].ToString());
                }
                if (dr["UserName"] != DBNull.Value)
                {
                    bi.AssignToName = dr["UserName"].ToString();
                }
                if (dr["Name"] != DBNull.Value)
                {
                    bi.WorkPriorityName = dr["Name"].ToString();
                }
                if (dr["OperatorID"] != DBNull.Value)
                {
                    bi.OperatorID = int.Parse(dr["OperatorID"].ToString());
                }
                if (dr["CallTimeDuration"] != DBNull.Value)
                {
                    bi.Calltime = dr["CallTimeDuration"].ToString();
                }
                if (dr["WorkPriority"] != DBNull.Value)
                {
                    bi.WorkPriority = int.Parse(dr["WorkPriority"].ToString());

                }
                if (dr["UserID"] != DBNull.Value)
                {
                    bi.UserID = int.Parse(dr["UserID"].ToString());

                }
                if (dr["Category"] != DBNull.Value)
                {
                    bi.WorkCategory = (dr["Category"].ToString());
                }
                if (dr["Voice"] != DBNull.Value)
                {
                    bi.Voice = (dr["Voice"].ToString());
                }
                if (dr["EntryDate"] != DBNull.Value)
                {
                    bi.EntryDate = DateTime.Parse(dr["EntryDate"].ToString());
                }
                if (dr["ProblemImage"] != DBNull.Value)
                {
                    bi.ProblemImage = (dr["ProblemImage"].ToString());
                    ViewBag.ProblemImage = (dr["ProblemImage"].ToString());
                }
                bi.lstFormName = FormAssociation(bi.ProblemStatementID);
                bi.lstOperatorDetail = OperatorAssociation(bi.ProblemStatementID);
                lstbranch.Add(bi);
            }
            return lstbranch;
        }
        List<FormName> FormAssociation(int ProblemStatementID)
        {
            List<FormName> lstFormNameAssociation = new List<FormName>();
            DataTable dtVariant = General.FetchData(@"Select FormName.FormID,FormName.Description,FormInfo.FormTitle,NewProblemStatement.ProblemStatementID from FormName inner join FormInfo on FormName.FormID = FormInfo.FormID
inner join NewProblemStatement on FormName.ProblemStatementID = NewProblemStatement.ProblemStatementID  Where FormName.ProblemStatementID=" + ProblemStatementID);
            foreach (DataRow dr in dtVariant.Rows)
            {
                FormName pva = new FormName();
                pva.ProblemStatementID = ProblemStatementID;
                pva.FormID = int.Parse(dr["FormID"].ToString());
                pva.FormTitle = dr["FormTitle"].ToString();
                pva.FormDescription = (dr["Description"].ToString());
                lstFormNameAssociation.Add(pva);
            }
            return lstFormNameAssociation;
        }
        List<OperatorDetail> OperatorAssociation(int ProblemStatementID)
        {
            List<OperatorDetail> lstFormNameAssociation = new List<OperatorDetail>();
            DataTable dtVariant = General.FetchData(@" 
 select EmployeePhone.*,OperatorDesignation.Name from EmployeePhone inner join NewProblemStatement on NewProblemStatement.OperatorID = EmployeePhone.OperatorID
 inner join OperatorDesignation on EmployeePhone.DesignationID = OperatorDesignation.Id Where ProblemStatementID =" + ProblemStatementID);
            foreach (DataRow dr in dtVariant.Rows)
            {
                OperatorDetail pva = new OperatorDetail();
                pva.ProblemStatementID = ProblemStatementID;
                pva.OperatorName = dr["EmployeeName"].ToString();
                pva.OperatorPhone = dr["EmployeePhone"].ToString();
                pva.Designation = (dr["Name"].ToString());
                lstFormNameAssociation.Add(pva);
            }
            return lstFormNameAssociation;
        }
        public ActionResult UpdateSolved(int? AllSolved, DateTime? Date,DateTime? DateTo)
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.New) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            if (Request.Cookies["UserID"] == null || Request.Cookies["UserID"].ToString() == "")
            {
                return Redirect("/Home/Login");
            }
            ViewBag.SolvedType = new DropDown().GetSolved();
            if (AllSolved == 0 || AllSolved == 1 || AllSolved == null)
            {
                AllSolved = 0;
            }
            else
            {
                AllSolved = 1;
            }
            if (Date == null)
            {
                Date = DateTime.Now;
            }
            if (DateTo == null)
            {
                DateTo = DateTime.Now;
            }
            ViewBag.Date = Date;
            ViewBag.DateTo = DateTo;
            DataTable dtProductinformation = new DataTable();
            dtProductinformation = General.FetchData(@"Select  ProblemStatementID,ProblemTitle,Solved,CustomerInfo.CustomerCompanytitle  from NewProblemStatement inner join CustomerInfo on NewProblemStatement.CustomerID = CustomerInfo.CustomerID
Where Solved="+AllSolved+ " and CAST(EntryDate  AS Date) between CAST('" + Date + "' AS Date) and CAST('" + DateTo + "' AS Date) ");
            return View(dtProductinformation);
        }

        public ActionResult SaveChangePrice(List<UpdateSolvedDetail> lstChangePrice, DateTime? SolvedDate)
        {
            if (lstChangePrice is null)
            {
                lstChangePrice = new List<UpdateSolvedDetail>();
            }
            string SQLQuery = "";
            foreach (UpdateSolvedDetail sa in lstChangePrice)
            {
                SQLQuery = SQLQuery + "     Update NewProblemStatement Set Solved=" + (sa.Solved == true ? "1" : "0") + ",SolvedDate='" + SolvedDate + "' Where ProblemStatementID=" + sa.NewProblemStatementID + "";
                string ProblemTitle = General.FetchData("Select ProblemTitle from NewProblemStatement Where ProblemStatementID=" + sa.NewProblemStatementID).Rows[0]["ProblemTitle"].ToString();
                new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Edit, GeneralAPIsController.LogSource.NewProblemStatement, sa.NewProblemStatementID, " Problem Title " + ProblemTitle);
            }
            if (!string.IsNullOrEmpty(SQLQuery))
            {
                General.ExecuteNonQuery(SQLQuery);
            }
            return Json("true");
        }
    }
}