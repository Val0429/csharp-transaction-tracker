using System.Windows.Forms;
using App;
using Constant;

namespace App_CentralManagementSystem
{
    public class LoginForm : App.LoginForm
    {
        public LoginForm()
        {
            DefaultPort = 8080;
            Icon = Properties.Resources.icon;

            Localization.Add("LoginForm_CMS", "CMS");

            AppProperties = new AppClientProperties();
        }

        public override void ShowSplash()
        {
            //ExecutionFile = "CMS.exe";
            // ====	Modify by Tulip
            // For Plugin use check if App is created in Plugin
            if (App == null)
            {
                App = CreateAppClient();
            }

            base.ShowSplash();

            IsLoading = false;
            Application.Idle -= LoginServer;
            Application.Idle += LoginServer;
        }

        protected virtual AppClient CreateAppClient()
        {
            var appClient = new CentralManagementSystem
            {
                Credential = new ServerCredential
                {
                    Port = Port,
                    Domain = Host,
                    UserName = Account,
                    Password = Password,
                    SSLEnable = SSLEnable,
                },
                Language = AppProperties.DefaultLanguage
            };

            return appClient;
        }

        protected override void UpdateUI()
        {
            base.UpdateUI();

            signInLabel.Text += @" " + Localization["LoginForm_CMS"];
        }
    }
}
