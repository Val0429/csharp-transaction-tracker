using System.Windows.Forms;
using Constant;

namespace App_FailoverSystem
{
    public class LoginForm : App.LoginForm
    {
        public LoginForm()
        {
            DefaultPort = 8888;
            Icon = Properties.Resources.icon;

            Localization.Add("LoginForm_Failover", "Failover System");

            sslCheckBox.Visible = false;
            AppProperties = new AppClientProperties();
        }

        public override void ShowSplash()
        {
            //ExecutionFile = "Failover.exe";
            App = new FailoverSystem
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

            signInLabel.Text += @" " + Localization["LoginForm_Failover"];
        }
    }
}
