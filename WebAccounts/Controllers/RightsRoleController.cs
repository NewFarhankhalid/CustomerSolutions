using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Installments.Controllers
{
    public class RightsRoleController : Controller
    {
        public ActionResult Index()
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Allowed) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            DataTable dt = General.FetchData("Select * from RightsRole"); 
            return View(dt);
        }
        public ActionResult Create()
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.New) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            RightsRole rl = new RightsRole();
            rl.LstRoleDetail = GetMenuDetail();
            //rl.LstReportDetail = GetReportDetail();
            return View(rl);
        }
        List<RightsRoleDetail> GetMenuDetail(int id=0)
        {
            List<RightsRoleDetail> lstmenu = new List<RightsRoleDetail>();
            string SQL = @"Select MenuID,MenuTitle,ParentID from Menus  order by sortid";
           DataTable dt = General.FetchData(SQL);
            RightsRoleDetail objdetail;
            foreach(DataRow dr in dt.Rows)
            {
                objdetail = new RightsRoleDetail();
                  objdetail.MenuID = int.Parse(dr["MenuID"].ToString());
                objdetail.MenuTitle = (dr["MenuTitle"].ToString());
                 objdetail.ParentID = int.Parse(dr["ParentID"].ToString());
                objdetail.Allowed = false;
                objdetail.New = false;
                objdetail.Edit = false;
                objdetail.Delete = false;
                lstmenu.Add(objdetail);
            }
            DataTable thisidroles = General.FetchData(" select * from RightsRoleDetail where roleid=" + id);
            foreach(DataRow dr in thisidroles.Rows)
            {
                if(lstmenu.Where(x=>x.MenuID==int.Parse(dr["MenuID"].ToString())).Count()>0)
                {
                    int index=lstmenu.IndexOf(lstmenu.Where(x => x.MenuID == int.Parse(dr["MenuID"].ToString())).FirstOrDefault());
                    if (dr["Allowed"] != DBNull.Value)
                    {
                        lstmenu[index].Allowed = bool.Parse(dr["Allowed"].ToString());
                    }

                    if (dr["New"] != DBNull.Value)
                    {
                        lstmenu[index].New = bool.Parse(dr["New"].ToString());
                    }
                    if (dr["Edit"] != DBNull.Value)
                    {
                        lstmenu[index].Edit = bool.Parse(dr["Edit"].ToString());
                    }
                    if (dr["Removed"] != DBNull.Value)
                    {
                        lstmenu[index].Delete = bool.Parse(dr["Removed"].ToString());
                    }
                }
            }
            return lstmenu;
        }

        //List<ReportsDetail> GetReportDetail(int id = 0)
        //{
        //    List<ReportsDetail> lstdetail = new List<ReportsDetail>();
        //    string SQL = @"Select ReportTitle,Section,ReportID from Reports order by Section";
        //    DataTable dt = General.FetchData(SQL);
        //    ReportsDetail objdetail;
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        objdetail = new ReportsDetail();
        //        objdetail.ReportID = int.Parse(dr["ReportID"].ToString());
        //        objdetail.ReportTitle = (dr["ReportTitle"].ToString());
        //        objdetail.SectionTitle = (dr["Section"].ToString());
        //        objdetail.ReportAllowed = false;
        //        lstdetail.Add(objdetail);
        //    }
        //    DataTable thisiddashboard = General.FetchData(" select * from ReportDetail where roleid=" + id);
        //    foreach (DataRow dr in thisiddashboard.Rows)
        //    {
        //        if (lstdetail.Where(x => x.ReportID == int.Parse(dr["ReportID"].ToString())).Count() > 0)
        //        {
        //            int index = lstdetail.IndexOf(lstdetail.Where(x => x.ReportID == int.Parse(dr["ReportID"].ToString())).FirstOrDefault());
        //            lstdetail[index].ReportAllowed = true;
        //            if (dr["ReportAllowed"] != DBNull.Value)
        //            {
        //                lstdetail[index].ReportAllowed = bool.Parse(dr["ReportAllowed"].ToString());
        //            }
        //        }
        //    }
        //    return lstdetail;
        //}
        [HttpPost]
        public ActionResult Create(RightsRole objrightsrole)
        {
            if(string.IsNullOrEmpty(objrightsrole.RoleTitle))
            {
                ViewBag.Messsage = "Please enter Role Title";
                return View(objrightsrole);
            }
            else if (objrightsrole.LstRoleDetail.Where(x=>x.Allowed==true).Count()==0)
            {
                ViewBag.Messsage = "Cannot save with zero menu selection";
                return View(objrightsrole);
            }
            if (objrightsrole.RoleID==0)
            {
                
                string SQL = "Insert into RightsRole(RoleTitle,RoleDescription,Inactive) Values('"+objrightsrole.RoleTitle+"','"+objrightsrole.Description+"',"+(objrightsrole.Inactive==true?"1":"0")+")";
                SQL = SQL + "    Select @@IDENTITY As RoleID";
                int RoleID=int.Parse( General.FetchData(SQL).Rows[0]["RoleID"].ToString());
                SQL = "";
                foreach (RightsRoleDetail rrd in objrightsrole.LstRoleDetail)
                {
                    SQL = SQL + "     Insert into RightsRoleDetail(RoleID,MenuID,Allowed,New,Edit,Removed) Values(" + RoleID + "," + rrd.MenuID + "," + (rrd.Allowed == true ? "1" : "0") + "," + (rrd.New == true ? "1" : "0") + "," + (rrd.Edit == true ? "1" : "0") + "," + (rrd.Delete == true ? "1" : "0") + ")";
                }
                General.ExecuteNonQuery(SQL);
                //SQL = "";
                //foreach (ReportsDetail ddd in objrightsrole.LstReportDetail)
                //{
                //    SQL = SQL + "  Insert Into ReportDetail(RoleID,ReportID,ReportAllowed) values(" + RoleID + "," + ddd.ReportID + "," + (ddd.ReportAllowed == true ? "1" : "0") + ")";
                //}
                //General.ExecuteNonQuery(SQL);

                new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.New, GeneralAPIsController.LogSource.RightsRole, objrightsrole.RoleID, " Role Name " + objrightsrole.RoleTitle);
            }
            else
            {
                int RoleID = objrightsrole.RoleID;

                string SQL = "Update RightsRole set RoleTitle='" + objrightsrole.RoleTitle + "',RoleDescription='" + objrightsrole.Description + "',Inactive=" + (objrightsrole.Inactive == true ? "1" : "0") + " ";
                SQL = SQL + " Where RoleID="+ RoleID;
                General.ExecuteNonQuery(SQL);
                
                SQL =  "  Delete from RightsRoleDetail Where RoleID=" +RoleID;
                foreach (RightsRoleDetail rrd in objrightsrole.LstRoleDetail)
                {
                    SQL = SQL + "     Insert into RightsRoleDetail(RoleID,MenuID,Allowed,New,Edit,Removed) Values(" + RoleID + "," + rrd.MenuID + "," + (rrd.Allowed == true ? "1" : "0") + "," + (rrd.New == true ? "1" : "0") + "," + (rrd.Edit == true ? "1" : "0") + "," + (rrd.Delete == true ? "1" : "0") + ")";
                }
                General.ExecuteNonQuery(SQL);
                //SQL = "";
                //SQL = " Delete from ReportDetail Where RoleID=" + RoleID;
                //foreach (ReportsDetail ddd in objrightsrole.LstReportDetail)
                //{
                //    SQL = SQL + "  Insert Into ReportDetail(RoleID,ReportID,ReportAllowed) values(" + RoleID + "," + ddd.ReportID + "," + (ddd.ReportAllowed == true ? "1" : "0") + ")";
                //}
                //General.ExecuteNonQuery(SQL);
                new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Edit, GeneralAPIsController.LogSource.RightsRole, objrightsrole.RoleID, " Role Name " + objrightsrole.RoleTitle);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Edit) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            RightsRole rl = new RightsRole();
            DataTable dt = General.FetchData("Select * from RightsRole Where roleID="+id);
            if (dt.Rows.Count > 0)
            {
                rl.RoleID = id;
                rl.RoleTitle = dt.Rows[0]["Roletitle"].ToString();
                rl.Description = dt.Rows[0]["RoleDescription"].ToString();
                rl.Inactive =bool.Parse(dt.Rows[0]["Inactive"].ToString());
                rl.LstRoleDetail = GetMenuDetail(id);
                //rl.LstReportDetail = GetReportDetail(id);
                return View("Create",rl);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        public ActionResult Delete(int id)
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Removed) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            try
            {
                string SQL = " Select * from UserInfo where UserID=" + General.userID + " and RoleID=" + id;
                DataTable constraints = General.FetchData(SQL);
                if (constraints.Rows.Count == 0)
                {
                    string sql = "Select Roletitle from RightsRole where RoleID=" + id;
                    DataTable dt = General.FetchData(sql);
                    var RoleTitle = dt.Rows[0]["Roletitle"].ToString();
                    SQL = "Delete from RightsRole where RoleID=" + id + "   Delete from RightsRoleDetail Where RoleID=" + id+ " Delete from ReportDetail Where RoleID=" + id;
                    General.ExecuteNonQuery(SQL);
                    new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Delete, GeneralAPIsController.LogSource.RightsRole, id, " Role Name " + RoleTitle);
                }
                else
                {
                    return Json("Cannot delete This Role, this is being used by " + constraints.Rows[0]["UserName"].ToString());
                }
                return Json("true");
            }
            catch (Exception ex)
            {
                return Json("error" + ex.InnerException);
            }
        }
    }
}
