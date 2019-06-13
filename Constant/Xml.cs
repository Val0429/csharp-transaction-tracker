using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using Constant.Utility;

namespace Constant
{
    public static class Xml
    {
        public static XmlElement CreateXmlElementWithText(this XmlDocument xmlDoc, String tagName, String text)
        {
            var xmlelm = xmlDoc.CreateElement(tagName);
            if (text != "")
                xmlelm.InnerText = text;

            return xmlelm;
        }

        public static XmlElement CreateXmlElementWithText(this XmlDocument xmlDoc, String tagName, Double text)
        {
            return xmlDoc.CreateXmlElementWithText(tagName, text.ToString(CultureInfo.InvariantCulture));
        }

        public static XmlElement CreateXmlElementWithText(this XmlDocument xmlDoc, String tagName, UInt16 text)
        {
            return xmlDoc.CreateXmlElementWithText(tagName, text.ToString(CultureInfo.InvariantCulture));
        }

        public static XmlElement CreateXmlElementWithText(this XmlDocument xmlDoc, String tagName, UInt32 text)
        {
            return xmlDoc.CreateXmlElementWithText(tagName, text.ToString(CultureInfo.InvariantCulture));
        }

        //-----------------------------------------------------------------------------------------------

        public static String GetFirstElementValueByTagName(XmlDocument xmlDoc, String tagName)
        {
            var list = xmlDoc.GetElementsByTagName(tagName);
            return (list.Count > 0) ? list[0].InnerText : "";
        }

        public static String GetFirstElementValueByTagName(XmlElement element, String tagName)
        {
            if (element == null) return null;
            var list = element.GetElementsByTagName(tagName);
            return (list.Count > 0) ? list[0].InnerText : "";
        }

        public static string FirstOrDefaultElement(this XmlElement element, string tagName)
        {
            var list = element.GetElementsByTagName(tagName);
            return (list.Count > 0) ? list[0].OuterXml : null;
        }

        public static String GetFirstElementValueByTagName(XmlNode xmlnode, String tagName)
        {
            return GetFirstElementValueByTagName((XmlElement)xmlnode, tagName);
        }

        //-----------------------------------------------------------------------------------------------

        public static XmlElement GetFirstElementByTagName(XmlDocument xmlDoc, String tagName)
        {
            var list = xmlDoc.GetElementsByTagName(tagName);
            return (list.Count > 0) ? list[0] as XmlElement : null;
        }

        public static XmlElement GetFirstElementByTagName(XmlElement xmlelm, String tagName)
        {
            var list = xmlelm.GetElementsByTagName(tagName);
            return (list.Count > 0) ? list[0] as XmlElement : null;
        }

        public static XmlElement GetFirstElementByTagName(XmlNode xmlnode, String tagName)
        {
            return GetFirstElementByTagName((XmlElement)xmlnode, tagName);
        }

        //-----------------------------------------------------------------------------------------------

        public static XmlDocument LoadXml(String xml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return xmlDoc;
        }

        public static XmlDocument LoadXmlFromFile(String filename)
        {
            var xmlDoc = new XmlDocument();

            try
            {
                if (!File.Exists(filename))
                {
                    LoggerManager.Instance.GetLogger().WarnFormat("File '{0}' not found.", filename);
                    return xmlDoc;
                }
                xmlDoc.Load(filename);
            }
            catch (XmlException)
            {
            }

            return xmlDoc;
        }

        //-----------------------------------------------------------------------------------------------

        //retry 3 times
        //private static readonly Dictionary<HttpWebRequest, UInt16> RetryDictionary = new Dictionary<HttpWebRequest, UInt16>();
        private static readonly CultureInfo Enus = new CultureInfo("en-US");

        //-----------------------------------------------------------------------------------------------

        private static string ParseResponseStream(WebResponse response)
        {
            var gzip = response.Headers.Get("Content-Encoding");
            var stream = (gzip != null && gzip.ToLower(Enus) == "gzip") ? new GZipStream(response.GetResponseStream(), CompressionMode.Decompress) : response.GetResponseStream();

            if (stream == null)
            {
                response.Close();
                return null;
            }

            var text = new StreamReader(stream).ReadToEnd();

            stream.Close();
            response.Close();

            return text;
        }

        private static String ParseResponseSting(WebResponse response)
        {
            try
            {
                var gzip = response.Headers.Get("Content-Encoding");
                var stream = (gzip != null && gzip.ToLower(Enus) == "gzip") ? new GZipStream(response.GetResponseStream(), CompressionMode.Decompress) : response.GetResponseStream();

                if (stream == null)
                {
                    response.Close();
                    return null;
                }

                String text = new StreamReader(stream).ReadToEnd();

                stream.Close();
                response.Close();

                return text;
            }
            catch (Exception)
            {
            }
            response.Close();

            return null;
        }

        public static Uri CreateUri(string pathAndQuery, ServerCredential credential)
        {
            try
            {
                var uriString = credential.GetCgiAbsoluteUriString(pathAndQuery);

                return new Uri(uriString);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetCgiAbsoluteUriString(this ServerCredential credential, string pathAndQuery, params object[] args)
        {
            if (args != null && args.Any())
            {
                pathAndQuery = string.Format(pathAndQuery, args);
            }

            var httpMode = (credential.SSLEnable) ? "https" : "http";

            var uriString = String.Format(@"{0}://{1}:{2}/{3}", httpMode, credential.Domain, credential.Port, pathAndQuery);

            return uriString;
        }

        public const UInt16 Timeout = 10;//20 sec
        private static Boolean _isSetServicePointManager;
        public static WebRequest GetHttpRequest(String url, ServerCredential credential, UInt16 timeout = Timeout)
        {
            //Bypass invalid SSL certificate errors when calling web services in .Net
            if (credential.SSLEnable && !_isSetServicePointManager)
            {
                _isSetServicePointManager = true;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }

            HttpWebRequest request;

            var uri = CreateUri(url, credential);

            if (uri == null)
                return null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(uri);
                request.ServicePoint.Expect100Continue = false; // add to disabled wait for 10 secs
            }
            catch (Exception exception)
            {
                //if config file is damaged, will cause exception, delete config file
                if (exception.InnerException != null && File.Exists(((System.Configuration.ConfigurationErrorsException)(exception.InnerException)).Filename))
                {
                    File.Delete(((System.Configuration.ConfigurationErrorsException)(exception.InnerException)).Filename);

                    try
                    {
                        Process.Start(Process.GetCurrentProcess().MainModule.ModuleName);
                    }
                    catch (Exception)
                    {
                    }

                    Environment.Exit(Environment.ExitCode);
                }

                return null;
            }

            try
            {
                //whill cause strange effect (cgi timeout)
                //request.PreAuthenticate = true;
                request.Credentials = credential;
                request.Timeout = Convert.ToInt32(timeout * 1000);

                return request;
            }
            catch (Exception)
            {
            }
            return null;
        }

        private static HttpWebRequestWithRetry CloneHttpWebRequest(HttpWebRequestWithRetry request)
        {
            var newRequest = (HttpWebRequest)WebRequest.Create(request.Request.RequestUri);

            newRequest.ServicePoint.Expect100Continue = false; // add to disabled wait for 10 secs

            //whill cause strange effect (cgi timeout)
            //newRequest.PreAuthenticate = true;
            newRequest.Credentials = request.Request.Credentials;
            newRequest.Timeout = request.Request.Timeout;
            newRequest.Method = request.Request.Method;
            newRequest.ContentType = request.Request.ContentType;

            var acceptGzip = request.Request.Headers["Accept-Encoding"];
            if (!String.IsNullOrEmpty(acceptGzip))
                newRequest.Headers.Add("Accept-Encoding", acceptGzip);

            var contentGzip = request.Request.Headers["Content-Encoding"];
            if (!String.IsNullOrEmpty(contentGzip))
                newRequest.Headers.Add("Content-Encoding", contentGzip);

            //Inheritance retry times);
            return new HttpWebRequestWithRetry { Request = newRequest, Retry = request.Retry };
        }

        //-----------------------------------------------------------------------------------------------
        internal static byte[] GZip(this byte[] data)
        {
            using (var temp = new MemoryStream())
            {
                using (var zipStream = new GZipStream(temp, CompressionMode.Compress, true))
                {
                    zipStream.Write(data, 0, data.Length);
                }

                return temp.ToArray();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="credential"></param>
        /// <param name="timeout">timeout value in millisecond for this request</param>
        /// <returns></returns>
        public static String LoadTextFromHttp(Uri uri, ServerCredential credential, int timeout)
        {
            // Bypass invalid SSL certificate errors when calling web services in .Net
            if ((credential.SSLEnable && !_isSetServicePointManager) || uri.Scheme == "https")
            {
                _isSetServicePointManager = true;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }

            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.ServicePoint.Expect100Continue = false; // add to disabled wait for 10 secs
            request.Credentials = credential;
            request.Timeout = timeout;

            var response = (HttpWebResponse)request.GetResponse();
            return ParseResponseSting(response);
        }

        public static String LoadTextFromHttp(String url, ServerCredential credential, Int16 retry = RetryTimes)
        {
            var request = new HttpWebRequestWithRetry(retry) { Request = GetHttpRequest(url, credential) };

            return LoadTextFromHttp(request);
        }

        private static String LoadTextFromHttp(HttpWebRequestWithRetry request)
        {
            try
            {
                var result = ParseResponseSting(request.Request.GetResponse());

                if (request.Retry != RetryTimes)
                    Log.Write(request.Request.RequestUri.ToString().PadRight(90, ' ') + "[Done]");

                return result;
            }
            catch (WebException exception)
            {
                if (IsThisExceptionNeedRetry(exception) && request.Retry-- > 0)
                {
                    Log.Write(request.Request.RequestUri.ToString().PadRight(90, ' ') + "[Retry]");

                    return LoadTextFromHttp(CloneHttpWebRequest(request));
                }

                Log.Write(request.Request.RequestUri.ToString().PadRight(90, ' ') + "[FAIL] " + exception.Status);
            }

            return null;
        }

        public static XmlDocument LoadXmlFromHttp(String url, ServerCredential credential, Boolean retry)
        {
            return LoadXmlFromHttp(url, credential, Timeout, true, retry);
        }

        public static XmlDocument LoadXmlFromHttp(String url, ServerCredential credential, UInt16 timeout = Timeout, Boolean gzip = true, Boolean retry = true)
        {
            var request = new HttpWebRequestWithRetry { Request = GetHttpRequest(url, credential, timeout) };
            if (!retry)
                request.Retry = 0;

            if (request.Request == null)
            {
                return null; // something srong
            }

            if (gzip)
                request.Request.Headers.Add("Accept-Encoding", "gzip");

            return LoadXmlFromHttp(request);
        }

        private static XmlDocument LoadXmlFromHttp(HttpWebRequestWithRetry request)
        {
            try
            {
                var result = ParseResponseStream((HttpWebResponse)request.Request.GetResponse());

                if (request.Retry != RetryTimes)
                    Log.Write(request.Request.RequestUri.ToString().PadRight(90, ' ') + "[Done]");

                var xmlDoc = new XmlDocument();
                try
                {
                    //will easy cause Exception when value is empty
                    //xmlDoc.Load(new XmlTextReader(stream));
                    if (!String.IsNullOrEmpty(result))
                    {
                        xmlDoc.LoadXml(result);
                    }
                        
                }
                catch (XmlException exception)
                {
                    Console.WriteLine(exception);
                }
                return xmlDoc;
            }
            catch (WebException exception)
            {
                if (IsThisExceptionNeedRetry(exception) && request.Retry-- > 0)
                {
                    Log.Write(request.Request.RequestUri.ToString().PadRight(90, ' ') + "[Retry]");

                    return LoadXmlFromHttp(CloneHttpWebRequest(request));
                }

                Log.Write(request.Request.RequestUri.ToString().PadRight(90, ' ') + "[FAIL] " + exception.Status);
            }

            return null;
        }

        public static Bitmap LoadImageFromHttp(String pathAndQuery, ServerCredential credential)
        {
            try
            {
                var request = GetHttpRequest(pathAndQuery, credential);
                request.Method = "GET";

                //request.ContentType = "multipart/form-data";
                var response = request.GetResponse();

                var stream = response.GetResponseStream();
                if (stream == null)
                {
                    response.Close();
                    return null;
                }

                var image = new Bitmap(stream);
                stream.Close();
                response.Close();

                return image;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Image LoadImageFromHttpWithPostData(String url, ServerCredential credential, String text)
        {
            try
            {
                var request = GetHttpRequest(url, credential);
                request.Method = "POST";

                //request.ContentType = "text/plain";
                var bytes = Encoding.UTF8.GetBytes(text);
                var postStream = request.GetRequestStream();
                postStream.Write(bytes, 0, bytes.Length);
                postStream.Close();

                //request.ContentType = "multipart/form-data";
                var response = request.GetResponse();

                var stream = response.GetResponseStream();
                //1MB
                //byte[] respBytesBuff = new byte[1024000];
                if (stream == null)
                {
                    response.Close();
                    return null;
                }

                var image = Image.FromStream(stream);

                stream.Close();
                response.Close();

                return image;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //-----------------------------------------------------------------------------------------------
        public static XmlDocument PostXmlToHttp(String url, XmlDocument xmlDoc, ServerCredential credential, UInt16 timeout = Timeout, Boolean retry = true)
        {
            var result = PostXmlToHttp(url, xmlDoc.InnerXml, credential, timeout, retry);
            var resultXml = new XmlDocument();
            try
            {
                if (!string.IsNullOrEmpty(result))
                {
                    resultXml.LoadXml(result);
                }
            }
            catch (XmlException ex)
            {
                Console.WriteLine(ex);
            }
            return resultXml;
        }

        public static string PostXmlToHttp(String url, String xmlText, ServerCredential credential, UInt16 timeout = Timeout, Boolean retry = true)
        {
            try
            {
                var request = new HttpWebRequestWithRetry {Request = GetHttpRequest(url, credential)};
                // Set the Method property of the request to POST.
                request.Request.Method = "POST";
                request.Request.Timeout = (timeout*1000);
                request.Request.ContentType = "text/xml; encoding='utf-8'";

                if (!retry)
                    request.Retry = 0;

                //accept RETURN CONTENT is GZIP
                request.Request.Headers.Add("Accept-Encoding", "gzip");

                //post data(XML) is GZIP
                request.Request.Headers.Add("Content-Encoding", "gzip");

                var bytes = Encoding.UTF8.GetBytes(xmlText);

                var result = PostXmlToHttp(request, bytes.GZip());

                return result;
            }
            catch (WebException ex)
            {
                Debug.WriteLine(ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return null;
        }

        private static string PostXmlToHttp(HttpWebRequestWithRetry request, Byte[] bytes)
        {
            try
            {
                // Get the request stream.
                var postStream = request.Request.GetRequestStream();
                // Write the data to the request stream.
                postStream.Write(bytes, 0, bytes.Length);
                // Close the Stream object.
                postStream.Close();

                // Get the response. need HttpWebResponse to get Status Code, WebResponse dont provide
                var result = ParseResponseStream(request.Request.GetResponse());

                if (result == null)
                {
                    if (request.Retry-- > 0)
                    {
                        Log.Write(request.Request.RequestUri.ToString().PadRight(90, ' ') + "[Retry]");

                        return PostXmlToHttp(CloneHttpWebRequest(request), bytes);
                    }

                    Log.Write(request.Request.RequestUri.ToString().PadRight(90, ' ') + "[FAIL]");

                    return null;
                }

                Log.Write(request.Request.RequestUri.ToString().PadRight(90, ' ') + "[Done]");

                return result;
            }
            catch (WebException exception)
            {
                if (IsThisExceptionNeedRetry(exception) && request.Retry-- > 0)
                {
                    Log.Write(request.Request.RequestUri.ToString().PadRight(90, ' ') + "[Retry]");

                    return PostXmlToHttp(CloneHttpWebRequest(request), bytes);
                }

                Log.Write(request.Request.RequestUri.ToString().PadRight(90, ' ') + "[FAIL] " + exception.Status);
            }

            return null;
        }

        private static Boolean IsThisExceptionNeedRetry(WebException exception)
        {
            //only retry when timeout and ConnectionClosed
            return (exception.Status == WebExceptionStatus.Timeout ||
                    exception.Status == WebExceptionStatus.ConnectionClosed ||
                    exception.Status == WebExceptionStatus.ReceiveFailure);
        }

        public static String PostTextToHttp(String url, String text, ServerCredential credential, UInt16 timeout = Timeout, Boolean gzip = false, short retry = RetryTimes)
        {
            var request = new HttpWebRequestWithRetry { Request = GetHttpRequest(url, credential), Retry = retry };
            request.Request.Method = "POST";
            request.Request.ContentType = "text/plain";

            if (gzip)
                request.Request.Headers.Add("Content-Encoding", "gzip");

            var bytes = Encoding.UTF8.GetBytes(text);

            return PostTextToHttp(request, gzip ? bytes.GZip() : bytes);
        }

        private static String PostTextToHttp(HttpWebRequestWithRetry request, Byte[] bytes)
        {
            try
            {
                //Post to server
                var postStream = request.Request.GetRequestStream();
                postStream.Write(bytes, 0, bytes.Length);
                postStream.Close();

                var result = ParseResponseSting(request.Request.GetResponse());

                if (result == null)
                {
                    if (request.Retry-- > 0)
                    {
                        Log.Write(request.Request.RequestUri.ToString().PadRight(90, ' ') + "[Retry]");

                        return PostTextToHttp(CloneHttpWebRequest(request), bytes);
                    }

                    Log.Write(request.Request.RequestUri.ToString().PadRight(90, ' ') + "[FAIL]");

                    return null;
                }

                //if (RetryDictionary[request] != RetryTimes)
                Log.Write(request.Request.RequestUri.ToString().PadRight(90, ' ') + "[Done]");

                return result;
            }
            catch (WebException exception)
            {
                if (IsThisExceptionNeedRetry(exception) && request.Retry-- > 0)
                {
                    Log.Write(request.Request.RequestUri.ToString().PadRight(90, ' ') + "[Retry]");

                    return PostTextToHttp(CloneHttpWebRequest(request), bytes);
                }

                Log.Write(request.Request.RequestUri.ToString().PadRight(90, ' ') + "[FAIL] " + exception.Status);
            }

            return null;
        }

        public static void PostImageToHttp(String url, Image image, ServerCredential credential)
        {
            if (image == null) return;

            try
            {
                var bytes = image.ToBytes(ImageFormat.Png);

                PostImageToHttp(url, bytes, credential);
            }
            catch (Exception)
            {

            }
        }

        public static void PostImageToHttp(String url, byte[] image, ServerCredential credential)
        {
            if (image == null) return;

            var request = GetHttpRequest(url, credential);
            request.Method = "POST";
            request.ContentType = "multipart/form-data";

            //Post to server
            var postStream = request.GetRequestStream();
            postStream.Write(image, 0, image.Length);
            postStream.Close();

            request.GetResponse();
        }

        public static String PostFileStreamToHttp(String url, Stream stream, ServerCredential credential)
        {
            try
            {
                var request = GetHttpRequest(url, credential);
                request.Method = "POST";
                request.Timeout = 120000;
                //request.MaximumResponseHeadersLength = 80;

                request.ContentType = "multipart/form-data";

                var bytes = new Byte[stream.Length];

                stream.Read(bytes, 0, Convert.ToInt32(stream.Length));

                stream.Flush();
                stream.Close();

                //Post to server
                using (var postStream = request.GetRequestStream())
                {
                    postStream.Write(bytes, 0, bytes.Length);
                    postStream.Close();
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var dataStream = response.GetResponseStream();
                    if (dataStream != null)
                    {
                        var reader = new StreamReader(dataStream);
                        var responseFromServer = reader.ReadToEnd();

                        reader.Close();
                        dataStream.Close();
                        response.Close();
                        return responseFromServer;
                    }
                    response.Close();
                    return String.Empty;
                }
            }
            catch (Exception)
            {
                return "Error";
            }
        }

        //-----------------------------------------------------------------------------------------------

        public const Int16 RetryTimes = 3;
        public class HttpWebRequestWithRetry
        {
            public HttpWebRequestWithRetry(Int16 retry = RetryTimes)
            {
                Retry = retry;
            }

            public WebRequest Request;
            public Int16 Retry { get; set; }
        }
    }
}