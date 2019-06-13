using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Constant.Utility
{
    public static class GenericUtility
    {
        private const string AlphaPattern = @"^[a-zA-Z]*$";

        private const string RegexPattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        internal static readonly ILogger Logger = LoggerManager.Instance.GetLogger();


        public static bool IsEmail(string text)
        {
            return !string.IsNullOrEmpty(text) && Regex.IsMatch(text, RegexPattern);
        }

        public static bool IsIPAddress(string ipString)
        {
            IPAddress address;
            return !string.IsNullOrEmpty(ipString) && IPAddress.TryParse(ipString, out address);
        }

        public static bool IsNumberAndAlphaOnly(char keyChar)
        {
            if (keyChar == (Char)13 || keyChar == (Char)8)
            {
                return true;
            }

            if (char.IsDigit(keyChar))
            {
                return true;
            }

            if (char.IsLetter(keyChar) && Regex.IsMatch(new string(new[] { keyChar }), AlphaPattern))
            {
                return true;
            }

            // (char)22 stands for Ctrl + v

            return false;
        }

        public static bool IsNumberOnly(char keyChar)
        {
            //\r and backspace
            if (keyChar == (Char)13 || keyChar == (Char)8)
            {
                return true;
            }

            if (char.IsDigit(keyChar))
            {
                return true;
            }

            return false;
        }

        public static bool HasColumn(this DataColumnCollection dataColumnCollection, string columnName)
        {
            return dataColumnCollection.OfType<DataColumn>().Any(c => c.ColumnName == columnName);
        }

        public static bool HasColumn(this DataTable dataTable, string columnName)
        {
            return dataTable.Columns.HasColumn(columnName);
        }

        public static int Count(this IEnumerable collection)
        {
            return collection == null ? 0 : Enumerable.Count(collection.Cast<object>());
        }

        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : default(TValue);
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : defaultValue;
        }

        public static object GetPropertyValue(this object obj, string propertyName, params object[] parameters)
        {
            var type = obj.GetType();
            var propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo == null) return null;

            var val = propertyInfo.GetValue(obj, parameters);

            return val;
        }

        public static T GetPropertyValue<T>(this object obj, string propertyName, params object[] parameters)
        {
            var val = obj.GetPropertyValue(propertyName, parameters);
            if (val is T)
            {
                return (T)val;
            }

            return default(T);
        }

        public static string ToPlainText(this byte[] data, string separator = ",")
        {
            return string.Join(separator, data.Select(d => d.ToString("X2")).ToArray());
        }

        public static string Concat(this byte[] data)
        {
            if (data == null || data.Length == 0) return null;

            var sb = new StringBuilder(data.Length * 2);
            foreach (var b in data)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        public static string Join<T>(this IEnumerable<T> collection, Func<T, string> toString, string separator = ",")
        {
            return string.Join(separator, collection.Select(toString).ToArray());
        }

        public static void Dump<T>(this IEnumerable<T> collection, Func<T, string> dumpFunc = null)
        {
            if (dumpFunc == null) dumpFunc = arg => arg.ToString();

            foreach (var data in collection.Select(dumpFunc))
            {
                Console.WriteLine(data);
            }
        }

        public static T GetValue<T>(this PropertyDataCollection collection, string name)
        {
            return (T)collection[name].Value;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static void Restart(this Stopwatch sw)
        {
            sw.Reset();
            sw.Start();
        }

        public static void MonitoringServiceRestart(string registryPath, string registryName, string serviceName)
        {
            MonitoringServiceRestart(registryPath, registryName, serviceName, TimeSpan.FromSeconds(3));
        }

        public static void MonitoringServiceRestart(string registryPath, string registryName, string serviceName, TimeSpan timeout)
        {
            try
            {
                Registry.SetValue(registryPath, registryName, "Stopped");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            using (var sc = new ServiceController(serviceName))
            {
                try
                {
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                }
                catch (System.ServiceProcess.TimeoutException ex)
                {
                    sc.Kill();

                    Logger.Info(ex);
                }
            }

            try
            {
                Registry.SetValue(registryPath, registryName, "Running");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public static T ToEnum<T>(this string enumText, bool ignoreCase = true)
        {
            return (T)Enum.Parse(typeof(T), enumText, ignoreCase);
        }

        public static T ToEnum<T>(this string enumText, T defaultValue, bool ignoreCase = true)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), enumText, ignoreCase);
            }
            catch (ArgumentException)
            {
                Logger.WarnFormat("Parsing enum text failed. EnumText: {0}.", enumText);
                return defaultValue;
            }
        }

        public static IEnumerable<T> GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)).OfType<T>();
        }

        public static bool IsGenericTypeOf(this Type type, Type genericType)
        {
            if (!type.IsGenericType) return false;

            var argType = type.GetGenericArguments();
            return type == genericType.MakeGenericType(argType);
        }

        public static bool HasInterfaceOf(this Type type, Type targeType)
        {
            var interfaces = type.FindInterfaces(AreEqualType, targeType);
            return interfaces.Any();
        }

        private static bool AreEqualType(Type type, object criteria)
        {
            Debug.Assert(criteria is Type);
            return type.FullName == (criteria as Type).FullName;
        }


        // Path
        /// <summary>
        /// Get the file full path under working directory.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileFullPath(string fileName)
        {
            var dir = GetWorkingDirectory();
            var filePath = Path.Combine(dir, fileName);

            return filePath;
        }

        public static string GetWorkingDirectory()
        {
            var asm = System.Reflection.Assembly.GetEntryAssembly();
            var path = Path.GetDirectoryName(asm != null ? asm.Location : AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            return path;
        }

        public static string ProgramFiles
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles); }
        }

        public static void MoveTo(this FileInfo fileInfo, string destFileName, bool overwrite)
        {
            if (overwrite)
            {
                fileInfo.CopyTo(destFileName, true);
                fileInfo.Delete();
            }
            else
            {
                fileInfo.MoveTo(destFileName);
            }
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public static FileInfo[] GetDirectoryFiles(string dir, string searchPattern = null)
        {
            if (Directory.Exists(dir))
            {
                var dirInfo = new DirectoryInfo(dir);
                var files = searchPattern == null ? dirInfo.GetFiles() : dirInfo.GetFiles(searchPattern);

                return files;
            }

            return new FileInfo[] { };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <exception cref="System.IO.IOException">
        /// The specified file is in use.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The caller does not have the required permission.-or- path is a directory.-or-
        /// path specified a read-only file.
        /// </exception>
        public static void SafeDelete(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// To validate directory path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool ValidatePath(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;

            var hasInvalidChar = path.Any(p => Path.GetInvalidPathChars().Contains(p));
            if (hasInvalidChar) return false;

            return path.Length < 248;
        }


        // Command line
        public static string GetCommandArgs(string key)
        {
            string keyValue = Environment.GetCommandLineArgs().FirstOrDefault(a => a.StartsWith(key + "="));
            if (!string.IsNullOrEmpty(keyValue))
            {
                string[] result = keyValue.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                return result.Length == 2 ? result[1] : null;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">Starting at 1</param>
        /// <returns></returns>
        public static string GetCommandArgs(ushort index)
        {
            var args = Environment.GetCommandLineArgs();
            return args.Length > index ? args[index] : null;
        }


        // Memory
        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetCurrentProcess", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetCurrentProcess();


        public static SecureString ToSecureString(this string corpus)
        {
            var target = new SecureString();
            foreach (var c in corpus)
            {
                target.AppendChar(c);
            }

            return target;
        }

        public static string ToPlainText(this SecureString secretString)
        {
            IntPtr bstr = Marshal.SecureStringToBSTR(secretString);
            try
            {
                return Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                Marshal.FreeBSTR(bstr);
            }
        }

        public static string ToBase64String(string originString)
        {
            var data = Encoding.UTF8.GetBytes(originString);
            return Convert.ToBase64String(data);
        }

        public static string FromBase64String(string base64String)
        {
            var data = Convert.FromBase64String(base64String);
            return Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// \r -> \r\n
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string LF2CRLF(string text)
        {
            return string.IsNullOrEmpty(text) ? text : Regex.Replace(text, "(?<!\r)\n", "\r\n", RegexOptions.Multiline);
        }

        public static string SubString(this string corpus, int position, int length)
        {
            if (position >= corpus.Length)
            {
                throw new ArgumentOutOfRangeException("position", position, "Position is out range.");
            }

            if (corpus.Length < length)
            {
                return corpus.Substring(position);
            }

            var len = corpus.Length - position;
            if (len < length)
            {
                return corpus.Substring(position);
            }

            return corpus.Substring(position, length);
        }

        public static string[] SplitByLength(this string text, int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException("length", length, "Length must be greater than 0");
            }
            if (string.IsNullOrEmpty(text))
            {
                return new string[] { };
            }

            var len = (double)length;
            var arrayLength = (int)Math.Ceiling(text.Length / len);
            var array = new string[arrayLength];
            for (int i = 0; i < arrayLength; i++)
            {
                array[i] = text.SubString(i * length, length);
            }

            return array;
        }

        public static string ToHexString(this byte[] data)
        {
            var sb = new StringBuilder();
            foreach (var b in data)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        public static byte[] HexToBytes(this string hexString)
        {
            const int length = 2;
            var arrayLength = (int)Math.Ceiling(hexString.Length / (double)length);
            var array = new byte[arrayLength];
            for (int i = 0; i < arrayLength; i++)
            {
                var hex = hexString.SubString(i * length, length);
                array[i] = ConvertUtility.ToByte(hex, NumberStyles.HexNumber);
            }

            return array;
        }

        public static int IndexOf(this byte[] source, byte[] pattern, int startIndex = 0)
        {
            if (source.Length == 0)
            {
                throw new ArgumentException("The length of source array cannot be 0.", "source");
            }

            if (source.Length <= startIndex)
            {
                throw new ArgumentOutOfRangeException("startIndex", startIndex, "startIndex must be less than source array.");
            }

            if (pattern.Length > source.Length)
            {
                return -1;
            }

            var textPattern = Encoding.ASCII.GetString(pattern);
            for (int i = startIndex; i <= source.Length - pattern.Length; i++)
            {
                var textSource = Encoding.ASCII.GetString(source, i, pattern.Length);
                if (textSource == textPattern)
                {
                    return i;
                }
            }

            return -1;
        }

        public static List<byte[]> Split(this byte[] data, byte separator)
        {
            var results = new List<byte[]>();
            int delimiter = -1;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == separator)
                {
                    int len = i - delimiter;
                    if (len > 1)
                    {
                        var t = new byte[len - 1];
                        Array.Copy(data, delimiter + 1, t, 0, len - 1);
                        results.Add(t);
                    }

                    delimiter = i;
                }
                else if (i == data.Length - 1)
                {
                    int len = i - delimiter;
                    if (len > 1)
                    {
                        var t = new byte[len];
                        Array.Copy(data, delimiter + 1, t, 0, len);
                        results.Add(t);
                    }
                }
            }

            return results;
        }

        public static bool EqualTo<T>(this T[] ary1, T[] ary2)
        {
            if (ary1.Length != ary2.Length)
            {
                return false;
            }

            for (int i = 0; i < ary1.Length; i++)
            {
                if ((ary1[i] != null && !ary1[i].Equals(ary2[i])) || (ary1[i] == null && ary2[i] != null))
                {
                    return false;
                }
            }

            return true;
        }

        public static T[] SubArray<T>(this T[] source, int startIndex)
        {
            if (source.Length == 0)
            {
                throw new ArgumentException("The length of source array cannot be 0.", "source");
            }

            if (source.Length <= startIndex)
            {
                throw new ArgumentOutOfRangeException("startIndex", startIndex, "startIndex must be less than source array.");
            }

            var size = source.Length - startIndex;
            var buffer = new T[size];
            Array.Copy(source, startIndex, buffer, 0, size);

            return buffer;
        }

        public static T[] SubArray<T>(this T[] source, int startIndex, int length)
        {
            if (source.Length == 0)
            {
                throw new ArgumentException("The length of source array cannot be 0.", "source");
            }

            if (source.Length <= startIndex)
            {
                throw new ArgumentOutOfRangeException("startIndex", startIndex, "'startIndex' must be less than source array.");
            }

            if (source.Length < (startIndex + length))
            {
                throw new ArgumentOutOfRangeException("length", length, "The length of destination array cannot be greater than source array.");
            }

            var buffer = new T[length];
            Array.Copy(source, startIndex, buffer, 0, length);

            return buffer;
        }

        /// <summary>
        /// Remove null item and resize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T[] Compact<T>(this T[] source)
        {
            return source.Where(s => s != null).ToArray();
        }

        public static int IndexOf<T>(this T[] source, T target, int startIdx = 0)
        {
            for (int i = startIdx; i < source.Length; i++)
            {
                var item = source[i];
                if (item.Equals(target))
                {
                    return i;
                }
            }

            return -1;
        }

        public static int LastIndexOf<T>(this T[] source, T target)
        {
            for (int i = source.Length - 1; i > 0; i--)
            {
                var item = source[i];
                if (item.Equals(target))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
