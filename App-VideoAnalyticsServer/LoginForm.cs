using System.Windows.Forms;
using Constant;

namespace App_VideoAnalyticsServer
{
    public class LoginForm : App.LoginForm
    {
        public LoginForm()
        {
            DefaultPort = 8088;
            Icon = Properties.Resources.icon;

            Localization.Add("LoginForm_VAS", "VAS");

            AppProperties = new AppClientProperties();
            sslCheckBox.Visible = false;
        }

        public override void ShowSplash()
        {
            //ExecutionFile = "VAS.exe";
            App = new VideoAnalyticsServer
            {
                Credential = new ServerCredential
                {
                    Port = Port,
                    Domain = Host,
                    UserName = Account,
                    Password = Password,
                },
                Language = AppProperties.DefaultLanguage
            };

            base.ShowSplash();

            IsLoading = false;
            Application.Idle -= LoginServer;
            Application.Idle += LoginServer;
        }

        protected override void UpdateUI()
        {
            base.UpdateUI();

            signInLabel.Text += @" " + Localization["LoginForm_VAS"];
        }
    }
}
