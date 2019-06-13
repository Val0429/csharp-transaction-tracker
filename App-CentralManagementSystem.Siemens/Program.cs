using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace App_CentralManagementSystem.Siemens
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] arguments)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (new LoginForm().Initialize(arguments))
            {
                System.Windows.Forms.Application.Run();
            }
        }
    }
}
