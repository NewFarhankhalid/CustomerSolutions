using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Installments.Models;
namespace Installments.Controllers
{
    public class UserInfoController : Controller
    {
        // GET: UserInfo
        public ActionResult Index()
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Allowed) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            DataTable dt = General.FetchData("Select *,RightsRole.RoleTitle from UserInfo left outer join RightsRole on UserInfo.RoleID = RightsRole.RoleID");
            List<UserInfo> lstUser = DataTableToObject(dt);
            return View(lstUser);
        }

        // GET: UserInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserInfo/Create
        public ActionResult Create()
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.New) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            ViewBag.Roles = new DropDown().GetRightRoles();
            Models.UserInfo userInfo = new Models.UserInfo();
            ViewBag.Error = "";
            return View(userInfo);
        }

        // POST: UserInfo/Create
        [HttpPost]
        public ActionResult Create(UserInfo us)
        {
            try
            {
                if (us.UserID == 0)
                {
                    string Query = "Insert into UserInfo (Username,Password,IsSuperAdmin,Inactive,RoleID)";
                    Query = Query + "Values ('" + us.UserName + "','" + us.Password + "'," + (us.IsSuperAdmin == true ? "1" : "0") + "," + (us.Inactive == true ? "1" : "0") + ","+us.RoleID+")";
                    Query = Query + " Select @@Identity as userID";
                    int userid = int.Parse(General.FetchData(Query).Rows[0]["userid"].ToString());
                    new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.New, GeneralAPIsController.LogSource.UserInfo, userid, " User name " + us.UserName);
                }
                else
                {
                    string Query = "";
                    Query = Query + "UPDATE [dbo].[UserInfo] ";
                    Query = Query + " SET    [Username] ='" + us.UserName + "' ";
                    Query = Query + " ,[Password] ='" + us.Password + "' ";
                    Query = Query + "    ,[InActive] = " + (us.Inactive == true ? "1" : "0") + "";
                    Query = Query + " ,[RoleID] =" + us.RoleID + " ";
                    Query = Query + "    ,[IsSuperAdmin] = " + (us.IsSuperAdmin == true ? "1" : "0") + "";
                    Query = Query + "WHERE userid =" + us.UserID;
                    General.ExecuteNonQuery(Query);
                    new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Edit, GeneralAPIsController.LogSource.UserInfo, us.UserID, " User name " + us.UserName);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserInfo/Edit/5
        public ActionResult Edit(int id)
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Edit) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            DataTable dtUser = General.FetchData("Select * from UserInfo left outer join RightsRole on RightsRole.RoleID = userInfo.RoleID where userid =" + id);
            List<UserInfo> lstUser = DataTableToObject(dtUser);
            ViewBag.Roles = new DropDown().GetRightRoles();
            if (lstUser.Count > 0)
            {
                return View("Create", lstUser[0]);
            }
            return View();
        }

        // POST: UserInfo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, UserInfo us)
        {
            try
            {
                string Query = "";
                Query = Query + "UPDATE [dbo].[UserInfo] ";
                Query = Query + " SET    [Username] ='" + us.UserName + "' ";
                Query = Query + " ,[Password] ='" + us.Password + "' ";
                Query = Query + " ,[RoleID] =" + us.RoleID + " ";
                Query = Query + " ,[InActive] = " + (us.Inactive == true ? "1" : "0") + "";
                Query = Query + " ,[IsSuperAdmin] = " + (us.IsSuperAdmin == true ? "1" : "0") + "";
                Query = Query + "WHERE userid =" + us.UserID;
                General.ExecuteNonQuery(Query);
                new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Edit, GeneralAPIsController.LogSource.UserInfo, us.UserID, " User name " + us.UserName);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserInfo/Delete/5
        public ActionResult Delete(int id)
        {
            if (General.ThisMethodRightAllowed(this.ControllerContext.RouteData.Values["controller"].ToString(), General.MethodNature.Removed) == false)
            {
                return RedirectToAction("PageNotAllowed", "Home", new { area = "" });
            }
            string Username = General.FetchData("Select UserName from UserInfo Where userid =" + id).Rows[0]["UserName"].ToString();
            string Query = "Delete from UserInfo Where userid =" + id;
            General.ExecuteNonQuery(Query);
            new GeneralAPIsController().InsertLog(GeneralAPIsController.LogTypes.Delete, GeneralAPIsController.LogSource.UserInfo, id, " User name " + Username);
            return Json("true");
        }
        
        List<UserInfo> DataTableToObject(DataTable dt)
        {
            List<UserInfo> lstUser = new List<UserInfo>();
            UserInfo bi;
            foreach (DataRow dr in dt.Rows)
            {
                bi = new UserInfo();
                if (dr["Userid"] != DBNull.Value)
                {
                    bi.UserID = int.Parse(dr["Userid"].ToString());
                }
                if (dr["Username"] != DBNull.Value)
                {
                    bi.UserName = (dr["Username"].ToString());
                }
                if (dr["Password"] != DBNull.Value)
                {
                    bi.Password = (dr["Password"].ToString());
                }
                if (dr["RoleTitle"] != DBNull.Value)
                {
                    bi.RoleTitle = (dr["RoleTitle"].ToString());
                }
                if (dr["RoleID"] != DBNull.Value)
                {
                    bi.RoleID = int.Parse(dr["RoleID"].ToString());
                }
                if (dr["InActive"] != DBNull.Value)
                {
                    bi.Inactive = bool.Parse(dr["InActive"].ToString());
                }
                if (dr["IsSuperAdmin"] != DBNull.Value)
                {
                    bi.IsSuperAdmin = bool.Parse(dr["IsSuperAdmin"].ToString());
                }
                lstUser.Add(bi);
            }
              return lstUser;
        }
    }
}
