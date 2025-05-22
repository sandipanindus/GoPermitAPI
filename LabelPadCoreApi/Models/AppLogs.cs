using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LabelPadCoreApi.Models
{
    public class AppLogs
    {
        public IConfiguration Configuration { get; }
        #region Method for logs
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        private static void WriteLog(string logType, string Message)
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

        public static void InfoLogs(string Message)
        {
            WriteLog("Info", Message);
        }

        public static void LogError(string Message)
        {
            WriteLog("Error", Message);
        }

        public static void LogWarning(string Message)
        {
            WriteLog("Warning", Message);
        }
    }
}
