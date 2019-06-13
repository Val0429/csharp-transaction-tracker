using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Constant.Utility
{
    public sealed class IniUtitlity : IDisposable
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private readonly ILogger _logger = LoggerManager.Instance.GetLogger();

        private bool _bDisposed;
        private readonly string _filePath;


        public string FilePath
        {
            get
            {
                return _filePath ?? string.Empty;
            }
        }


        // Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Ini file path</param>
        public IniUtitlity(string path)
        {
            if (!File.Exists(path))
            {
                var ex = new FileNotFoundException("Ini file not found", "path");

                _logger.Error(ex);

                throw ex;
            }

            _filePath = path;
        }

        ~IniUtitlity()
        {
            OnDispose();
        }

        public void Dispose()
        {
            OnDispose();

            GC.SuppressFinalize(this);
        }

        private void OnDispose()
        {
            if (_bDisposed)
                return;

            _bDisposed = true;
        }


        // Methods
        public void SetKeyValue(string section, string key, object value)
        {
            WritePrivateProfileString(section, key, value.ToString(), _filePath);
        }

        public string GetKeyValue(string section, string key)
        {
            var temp = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", temp, 255, _filePath);
            return temp.ToString();
        }

        public string GetKeyValue(string section, string key, string defaultValue)
        {
            try
            {
                StringBuilder sbResult = new StringBuilder(255);

                GetPrivateProfileString(section, key, "", sbResult, 255, _filePath);

                return (sbResult.Length > 0) ? sbResult.ToString() : defaultValue;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
