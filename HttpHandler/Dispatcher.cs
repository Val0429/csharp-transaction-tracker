using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace HttpHandler
{
    public class Dispatcher : IDispatcher
    {
        private static readonly LogFile LogFile = new LogFile();

        public String User { get; set; }
        public String Password { get; set; }
        public String Group { get; set; }

        public String Get(String queryString)
        {
            LogFile.WriteToLogFile(String.Format(" [Dispatcher]   [Get]:Enter [{0}]", queryString));

            var qsPair = ParseQueryString(queryString);

            var worker = ModelSelection(qsPair);

            if (worker == null)
            {
                return new XElement("Response", new XAttribute("status", 503), new XAttribute("contentType", "text/html"), "Service Unavailable").ToString();
            }

            worker.User = User;
            worker.Group = Group;

            var result = "";
            try
            {
                result = worker.Get(qsPair);
                LogFile.WriteToLogFile(String.Format(" [Dispatcher]   [Get]:Successful [{0}]", queryString));
            }
            catch (Exception ex)
            {
                LogFile.WriteToLogFile(String.Format(" [Dispatcher]   [Get]:Error [{0}]", queryString));
                LogFile.WriteToLogFile(ex.Message);
            }

            LogFile.WriteToLogFile(String.Format(" [Dispatcher]   [Get]:Return [{0}]", result));

            return result;

        }

        public String Post(String queryString, String param)
        {
            LogFile.WriteToLogFile(String.Format(" [Dispatcher]   [Post]:Enter [{0}] [{1}]", queryString, param));

            var qsPair = ParseQueryString(queryString);
            var worker = ModelSelection(qsPair);

            if (worker == null)
            {
                return new XElement("Response", new XAttribute("status", 503), new XAttribute("contentType", "text/html"), "Service Unavailable").ToString();
            }

            worker.User = User;
            worker.Group = Group;

            var result = "";
            try
            {
                result = worker.Post(qsPair, param);
                LogFile.WriteToLogFile(String.Format(" [Dispatcher]   [Post]:Successful [{0}]", queryString));
            }
            catch (Exception ex)
            {
                LogFile.WriteToLogFile(String.Format(" [Dispatcher]   [Post]:Error [{0}]", queryString));
                LogFile.WriteToLogFile(ex.Message);
            }

            LogFile.WriteToLogFile(String.Format(" [Dispatcher]   [Get]:Return [{0}]", result));

            return result;
        }

        public String Post(String queryString, IntPtr handle, int length)
        {
            return "";
        }

        private static Dictionary<String, String> ParseQueryString(String queryString)
        {
            var r = new Dictionary<String, String>();

            var pairs = queryString.Split('&');
            foreach (var pair in pairs)
            {
                if (!pair.Contains("="))
                    r.Add(pair, "");
                else
                {
                    var kv = pair.Split('=');
                    r.Add(kv[0].Trim(), kv[1].Trim());
                }
            }
            return r;
        }

        private static IDataWorker ModelSelection(Dictionary<String, String> qsPair)
        {
            IDataWorker worker = null;

            foreach (var pair in qsPair)
            {
                if (pair.Key != "model") continue;

                try
                {
                    var location = Assembly.GetExecutingAssembly().Location;
                    var path = Path.GetDirectoryName(location);

                    if (path != null)
                    {
                        var workerFile = Path.Combine(path, String.Format("{0}DataWorker.dll", pair.Value));

                        Assembly assembly = Assembly.LoadFrom(workerFile);
                        Type type = assembly.GetType(String.Format(pair.Value + "DataWorker.DataWorker"));

                        object obj = assembly.CreateInstance(type.FullName, true);

                        worker = (IDataWorker)obj;
                    }
                }
                catch (Exception ex)
                {
                    LogFile.WriteToLogFile(String.Format(" [Dispatcher]   [Selection]:Error [{0}]", pair.Value));
                    LogFile.WriteToLogFile(ex.Message);
                }

                LogFile.WriteToLogFile(String.Format(" [Dispatcher]   [Selection]:Successful [{0}]", pair.Value));

                break;
            }

            return worker;
        }
    }
}
