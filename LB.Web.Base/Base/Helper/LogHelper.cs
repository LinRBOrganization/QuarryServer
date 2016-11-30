using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LB.Web.Base.Base.Helper
{
    public class LogHelper
    {
        public static void WriteLog( string strLog)
        {
            string strLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin\\LogServer.log");
            if (!File.Exists(strLogPath))
            {
                File.Create(strLogPath);
            }
            StreamWriter sw = File.AppendText(strLogPath);
            sw.WriteLine(DateTime.Now.ToString("yyMMdd HH:mm:ss")+"----"+strLog);
            sw.Close();
        }
    }
}
