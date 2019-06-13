using System;

namespace App_FailoverSystem
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            if (new LoginForm().Initialize())
            {
                System.Windows.Forms.Application.Run();
            }
        }
    }
}
