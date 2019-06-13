using System;
using System.IO;

namespace Constant
{
    public static class Propertys
    {
        public static void Delete(Exception exception)
        {
            try
            {
                if (exception.InnerException != null && File.Exists(((System.Configuration.ConfigurationErrorsException)(exception.InnerException)).Filename))
                    File.Delete(((System.Configuration.ConfigurationErrorsException)(exception.InnerException)).Filename);   
            }
            catch(Exception)
            {
            }
        }
    }
}
