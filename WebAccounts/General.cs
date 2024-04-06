using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Installments.Models;
namespace Installments
{
    public class General
    {
        internal static int userID;
        internal static string username;
        public static string CompanyName { get; set; }
        public static string ConnectionString { get; set; }
        public static string BranchLabel { get; set; }

        public static bool ReportsAllowed(string ReportName, ReportNature methodnature)
        {
            int ReportID = General.GetReportIDByName(ReportName);
            DataTable dt = FetchData(@" Select * from ReportDetail inner join RightsRole on RightsRole.RoleID = ReportDetail.RoleID
inner join UserInfo on RightsRole.RoleID  = UserInfo.RoleID Where UserInfo.UserID = " + (System.Web.HttpContext.Current.Request.Cookies["UserID"].Value.ToString()) + " and ReportDetail.ReportID = " + ReportID);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0][methodnature.ToString()] != DBNull.Value)
                {
                    if (bool.Parse(dt.Rows[0][methodnature.ToString()].ToString()) == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static int GetReportIDByName(string Name)
        {
            int ReportID = 0;

            DataTable dt = FetchData("Select ReportID from Reports where ReportTitle like '" + Name.Trim() + "'");

            if (dt.Rows.Count > 0)
            {
                ReportID = int.Parse(dt.Rows[0]["ReportID"].ToString());
            }

            return ReportID;
        }
        public enum ReportNature
        {
            ReportAllowed
        }
        public static DataTable FetchData(string query)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(query,con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            con.Close();
            return dt;
      }
        public static bool CheckConnectivity(string _ConnectionString="")
        {
            if(_ConnectionString == "")
            {
                _ConnectionString = ConnectionString;
            }
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();
                con.Close();
                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }
        public static void ExecuteNonQuery(string query)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(query,con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public static void ExecuteNonQuerywithtransaction(string query)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlTransaction trans;
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Transaction = trans;
                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (SqlException ex)
            {
                trans.Rollback();
                throw new Exception("Error:",ex);
            }
            con.Close();
        }
        public static void StartUpSettings( )
        {
          
        }
        public static bool ThisMethodRightAllowed(string ControllerName, MethodNature methodnature)
        {
            int MenuID = GetMenuIDByControllerName(ControllerName);
            DataTable dt = FetchData(@" Select * from RightsRoleDetail 
inner join UserInfo on RightsRoleDetail.RoleID=UserInfo.RoleID Where UserID="+ (System.Web.HttpContext.Current.Request.Cookies["UserID"].Value.ToString()) + " and RightsRoleDetail.MenuID="+ MenuID);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0][methodnature.ToString()] != DBNull.Value)
                {
                    if (bool.Parse(dt.Rows[0][methodnature.ToString()].ToString()) == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public enum MethodNature
        {
            New,
            Edit,
            Removed,
            Allowed
        }
        public static int GetMenuIDByControllerName(string ControllerName)
        {
            int MenuId = 0;
            DataTable dt = FetchData("Select MenuID from Menus where Controller like '" + ControllerName.Trim() + "'");
            if (dt.Rows.Count > 0)
            {
                MenuId = int.Parse(dt.Rows[0]["MenuID"].ToString());
            }
            return MenuId;
        }
    }
}