using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constant
{
    public class Logininfo
    {
        public class MyLogin
        {
            public string Host
            {
                get;
                set;
            }

            /*
             * private ushort _port;
              public ushort Port
              {
                  get { return _port; }
                  set
                  {
                      _port = value;
                  }
              }

             */
            public ushort Port
            {
                get;
                set;
            }
            public string Account
            {
                get;
                set;
            }

            public string Password
            {
                get;
                set;
            }

            public bool SSLEnable
            {
                get;
                set;
            }

            public bool RememberMe
            {
                get;
                set;
            }

            public bool Autologin
            {
                get;
                set;
            }

            public string ADDomain
            {
                get;
                set;
            }
            public string Lang
            {
                get;
                set;
            }
            public string Mode
            {
                get;
                set;
            }

            public Dictionary<string, string> localizationList
            {
                get;
                set;
            }
        }

    }
}
