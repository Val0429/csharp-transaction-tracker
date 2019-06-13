using System;

namespace App_ConfigurationToOrder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
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
