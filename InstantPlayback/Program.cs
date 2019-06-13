using System;
using System.Windows.Forms;

namespace InstantPlayback
{
    static class Program
    {
        [STAThread]
        static void Main(String[] arguments)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Application.Run(new InstantPlaybackForm(arguments));
            }
            catch (Exception exception)
            {
                //MessageBox.Show(exception.ToString());
                //Application.Exit();
            }
        }
    }
}
