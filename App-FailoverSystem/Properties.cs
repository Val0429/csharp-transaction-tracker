using System;
using System.Collections.Generic;
using App;
using Constant;

namespace App_FailoverSystem
{
    public class AppClientProperties : App.AppClientPropertiesBase
    {
        public override String DefaultLanguage
        {
            get
            {
                try
                {
                    return Properties.Settings.Default.Language;
                }
                catch (Exception)
                {
                    return String.Empty;
                }
            }
            set
            {
                try
                {
                    Properties.Settings.Default.Language = value;
                }
                catch (Exception)
                {
                }
            }
        }

        public override String DefaultHistory
        {
            get
            {
                try
                {
                    return Properties.Settings.Default.History;
                }
                catch (Exception)
                {
                    return String.Empty;
                }
            }
            set
            {
                try
                {
                    Properties.Settings.Default.History = value;
                }
                catch (Exception)
                {
                }
            }
        }

        public override String DefaultScreenName
        {
            get
            {
                try
                {
                    return Properties.Settings.Default.ScreenName;
                }
                catch (Exception)
                {
                    return String.Empty;
                }
            }
            set
            {
                try
                {
                    Properties.Settings.Default.ScreenName = value;
                }
                catch (Exception)
                {
                }
            }
        }

        public override String DefaultWindowState
        {
            get
            {
                try
                {
                    return Properties.Settings.Default.WindowState;
                }
                catch (Exception)
                {
                    return String.Empty;
                }
            }
            set
            {
                try
                {
                    Properties.Settings.Default.WindowState = value;
                }
                catch (Exception)
                {
                }
            }
        }

        public override Int32 DefaultWindowLocationX
        {
            get
            {
                try
                {
                    return Properties.Settings.Default.WindowLocationX;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            set
            {
                try
                {
                    Properties.Settings.Default.WindowLocationX = value;
                }
                catch (Exception)
                {
                }
            }
        }

        public override Int32 DefaultWindowLocationY
        {
            get
            {
                try
                {
                    return Properties.Settings.Default.WindowLocationY;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            set
            {
                try
                {
                    Properties.Settings.Default.WindowLocationY = value;
                }
                catch (Exception)
                {
                }
            }
        }

        public override Int32 DefaultWindowWidth
        {
            get
            {
                try
                {
                    return Properties.Settings.Default.WindowWidth;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            set
            {
                try
                {
                    Properties.Settings.Default.WindowWidth = value;
                }
                catch (Exception)
                {
                }
            }
        }

        public override Int32 DefaultWindowHeight
        {
            get
            {
                try
                {
                    return Properties.Settings.Default.WindowHeight;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            set
            {
                try
                {
                    Properties.Settings.Default.WindowHeight = value;
                }
                catch (Exception)
                {
                }
            }
        }

        public override Boolean DefaultRemeberMe
        {
            get
            {
                try
                {
                    return Properties.Settings.Default.RememberMe;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            set
            {
                try
                {
                    Properties.Settings.Default.RememberMe = value;
                }
                catch (Exception)
                {
                }
            }
        }

        public override Boolean DefaultAutoSignIn
        {
            get
            {
                try
                {
                    return Properties.Settings.Default.AutoSignIn;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            set
            {
                try
                {
                    Properties.Settings.Default.AutoSignIn = value;
                }
                catch (Exception)
                {
                }
            }
        }

        public override void SaveProperties()
        {
            try
            {
                Properties.Settings.Default.Save();
            }
            catch (Exception exception)
            {
                //if property is damaged, DELETE it!!!
                Propertys.Delete(exception);
            }
        }

        public override void RemoveProperties(ServerCredential credential)
        {
            try
            {
                Dictionary<String, String> hostHistory = ServerProperties.LoadCredentialHistory(Properties.Settings.Default.History);
                if (hostHistory != null)
                {
                    hostHistory.Remove(credential.Domain);

                    Properties.Settings.Default.History = ServerProperties.EncCredentialHistory(hostHistory, credential.Domain);

                    Properties.Settings.Default.Save();
                }
            }
            catch (Exception exception)
            {
                Propertys.Delete(exception);
            }
        }
    }
}
