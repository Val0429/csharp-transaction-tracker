using System;
using System.IO;
using System.Reflection;

namespace HttpHandler
{
    public class LogFile
    {
        public void WriteToLogFile(String message)
        {
            // 1. get Exe/Dll file
            var assembly = Assembly.GetExecutingAssembly();

            // 2. define caller NameSpace
            var stackTrace = new System.Diagnostics.StackTrace();
            var ns = stackTrace.GetFrame(1).GetMethod().DeclaringType.Namespace;

            var logFile = ns + "_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

            // 3. Get Current Path
            var location = assembly.Location;
            var path = Path.GetDirectoryName(location);

            if (path != null)
                logFile = Path.Combine(path, logFile);

            try
            {
                File.AppendAllText(logFile, DateTime.Now.ToString("HH-mm-ss.fff") + " " + message + "\r\n");

                Console.WriteLine(DateTime.Now.ToString("HH-mm-ss.fff") + " " + message + "\r\n");
            }
            catch (Exception ex1)
            {
                Console.WriteLine(DateTime.Now.ToString("HH-mm-ss.fff") + " " + ex1.Message + "\r\n");
            }

        }
    }
}
