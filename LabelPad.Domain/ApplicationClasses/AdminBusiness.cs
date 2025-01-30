using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace LabelPad.Domain.ApplicationClasses
{
   public class AdminBusiness
    {
        public DataSet GetVisitorBaynos(ArrayList array, string strConnection)
        {
            try
            {
                // string strConnection = _configuration["ConnectionStrings:Default"];
                return SqlHelper.ExecuteDataset(strConnection, "Sp_GetVisitorBayNos", array.ToArray());
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                 InfoLogs("Method:GetVisitorBaynos, SPName:Sp_GetVisitorBayNos, Exception: " + ex.Message.ToString());
                return null;
            }
        }
        public DataSet GetSearchUsers(ArrayList array, string strConnection)
        {
            try
            {
                // string strConnection = _configuration["ConnectionStrings:Default"];
                return SqlHelper.ExecuteDataset(strConnection, "Sp_SearchUserFilter", array.ToArray());
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                InfoLogs("Method:GetSearchUsers, SPName:Sp_SearchUserFilter, Exception: " + ex.Message.ToString());
                return null;
            }
        }
        public DataSet GetSearchTenants(ArrayList array, string strConnection)
        {
            try
            {
                // string strConnection = _configuration["ConnectionStrings:Default"];
                return SqlHelper.ExecuteDataset(strConnection, "Sp_SearchTenantFilter", array.ToArray());
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                InfoLogs("Method:GetSearchUsers, SPName:Sp_SearchTenantFilter, Exception: " + ex.Message.ToString());
                return null;
            }
        }
        public DataSet GetSearchSupports(ArrayList array, string strConnection)
        {
            try
            {
                // string strConnection = _configuration["ConnectionStrings:Default"];
                return SqlHelper.ExecuteDataset(strConnection, "Sp_SearchSupportFilter", array.ToArray());
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                InfoLogs("Method:GetSearchUsers, SPName:Sp_SearchSupportFilter, Exception: " + ex.Message.ToString());
                return null;
            }
        }

        public DataSet GetSearchPings(ArrayList array, string strConnection)
        {
            try
            {
                // string strConnection = _configuration["ConnectionStrings:Default"];
                return SqlHelper.ExecuteDataset(strConnection, "Sp_SearchPingFilter", array.ToArray());
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                InfoLogs("Method:GetSearchPings, SPName:Sp_SearchPingFilter, Exception: " + ex.Message.ToString());
                return null;
            }
        }


        public DataSet Getbaynamesbydates(ArrayList array, string strConnection)
        {
            try
            {
                // string strConnection = _configuration["ConnectionStrings:Default"];
                return SqlHelper.ExecuteDataset(strConnection, "Sp_baynamebydates", array.ToArray());
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                InfoLogs("Method:GetSearchUsers, SPName:Sp_baynamebydates, Exception: " + ex.Message.ToString());
                return null;
            }
        }


        public DataSet Getbaynametogropdown(ArrayList array, string strConnection)
        {
            try
            {
                // string strConnection = _configuration["ConnectionStrings:Default"];
                return SqlHelper.ExecuteDataset(strConnection, "Sp_baynemesforparkingshedule", array.ToArray());
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                InfoLogs("Method:GetSearchUsers, SPName:Sp_baynemesforparkingshedule, Exception: " + ex.Message.ToString());
                return null;
            }
        }
        public DataSet GetSearchSites(ArrayList array, string strConnection)
        {
            try
            {
                // string strConnection = _configuration["ConnectionStrings:Default"];
                return SqlHelper.ExecuteDataset(strConnection, "Sp_SearchSiteFilter", array.ToArray());
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                InfoLogs("Method:GetSearchSites, SPName:Sp_SearchSiteFilter, Exception: " + ex.Message.ToString());
                return null;
            }
        }
        public DataSet GetSearchZatpark(ArrayList array, string strConnection)
        {
            try
            {
                // string strConnection = _configuration["ConnectionStrings:Default"];
                return SqlHelper.ExecuteDataset(strConnection, "Sp_GetZatparkFilter", array.ToArray());
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                InfoLogs("Method:GetSearchZatpark, SPName:Sp_GetZatparkFilter, Exception: " + ex.Message.ToString());
                return null;
            }
        }
        public DataSet GetSearchAuditLog(ArrayList array, string strConnection)
        {
            try
            {
                // string strConnection = _configuration["ConnectionStrings:Default"];
                return SqlHelper.ExecuteDataset(strConnection, "Sp_GetAuditFilter", array.ToArray());
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                InfoLogs("Method:GetSearchAuditLog, SPName:Sp_GetAuditFilter, Exception: " + ex.Message.ToString());
                return null;
            }
        }
        public DataSet GetSearchTenantLog(ArrayList array, string strConnection)
        {
            try
            {
                // string strConnection = _configuration["ConnectionStrings:Default"];
                return SqlHelper.ExecuteDataset(strConnection, "Sp_GetTenantFilter", array.ToArray());
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                InfoLogs("Method:GetSearchAuditLog, SPName:Sp_GetTenantFilter, Exception: " + ex.Message.ToString());
                return null;
            }
        }

        #region Method for logs
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public static void InfoLogs(string Message)
        {
            string fullpath = "";
            try
            {

                if (true)
                {
                    string appPath = AppDomain.CurrentDomain.BaseDirectory + "Logs";
                    if (!Directory.Exists(appPath))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(appPath);
                    }
                    string Filename = "APPServiceLog" + DateTime.Now.ToString("dd-MM-yyyy"); //dateAndTime.ToString("dd/MM/yyyy")
                    fullpath = appPath + "\\" + Filename + ".txt";

                    if (!File.Exists(fullpath))
                    {
                        System.IO.FileStream f = System.IO.File.Create(fullpath);
                        f.Close();
                        TextWriter tw = new StreamWriter(fullpath);
                        tw.WriteLine(DateTime.Now + " " + Message);
                        tw.Close();
                    }
                    else if (File.Exists(fullpath))
                    {
                        using (StreamWriter w = File.AppendText(fullpath))
                        {
                            w.WriteLine(DateTime.Now + " " + Message);
                            w.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter w = File.AppendText(fullpath))
                {
                    w.WriteLine(DateTime.Now + " " + ex.StackTrace.ToString());
                    w.Close();
                }
            }
        }
        #endregion
    }
}
