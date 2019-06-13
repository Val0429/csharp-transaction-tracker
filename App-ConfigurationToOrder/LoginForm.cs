using Constant;
using NVR = App_NetworkVideoRecorder;

namespace App_ConfigurationToOrder
{
    public class LoginForm : NVR.LoginForm
    {
        public override void ShowSplash()
        {
            App = new ConfigurationToOrder
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

            base.ShowSplash();
        }

    }
}
