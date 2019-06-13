using System;
using System.Reflection;

namespace App_NetworkVideoRecorder
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main(String[] arguments)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            if (new LoginForm().Initialize(arguments))
            {
                System.Windows.Forms.Application.Run();
            }
        }
    }
}
