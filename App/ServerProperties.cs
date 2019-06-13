using System;
using System.Collections.Generic;
using System.Xml;
using Constant;

namespace App
{
    public static class ServerProperties
    {
        public static Dictionary<String, String> LoadCredentialHistory(String property)
        {
            if (property == "") return new Dictionary<String, String>();

            var credentials = new Dictionary<String, String>();

            String[] desLists = Encryptions.DecryptDES(property).Split(',');
            foreach (String desList in desLists)
            {
                var str = Encryptions.DecryptDES(desList);
                
            //new xml format
                if (str.IndexOf("<XML>") == 0)
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(str);

                    var value = Xml.GetFirstElementValueByTagName(xmlDoc, "Host");
                    if (!String.IsNullOrEmpty(value))
                    {
                        var host = Encryptions.DecryptDES(value).Trim();

                        if (!credentials.ContainsKey(host))
                            credentials.Add(host, desList);
                        else
                            credentials[host] = desList;
                    }
                }
                else //old XXX;YYY;ZZZ format
                {
                    String[] temp = str.Split(',');

                    if (temp.Length < 4) continue;

                    temp[0] = Encryptions.DecryptDES(temp[0]);

                    if (!credentials.ContainsKey(temp[0]))
                        credentials.Add(temp[0], desList);
                    else
                        credentials[temp[0]] = desList;
                }
            }

            return credentials;
        }

        public static String EncCredentialHistory(Dictionary<String, String> credentials, String domain)
        {
            var saveHistory = new List<String>();
            if (credentials.ContainsKey(domain))
            {
                saveHistory.Add(credentials[domain]); //keep the last login record as next time's default
            }

            foreach (KeyValuePair<String, String> obj in credentials)
            {
                if (!saveHistory.Contains(obj.Value))
                    saveHistory.Add(obj.Value);
            }

            return (saveHistory.Count > 0) ? Encryptions.EncryptDES(String.Join(",", saveHistory.ToArray())) : "";
        }

        public static ServerCredential ParseDesStringToCredential(String desString)
        {
            var str = Encryptions.DecryptDES(desString);

            var credential = new ServerCredential();
            //new xml format
            if (str.IndexOf("<XML>") == 0)
            {
                //<XML>
                //  <Host>127.0.0.1<Host>
                //  <Port>82<Port>
                //  <Account>Admin<Account>
                //  <Password><Password>
                //  <SSL>false<SSL>
                //  <RememberMe>false<RememberMe>
                //</XML>
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(str);

                var value = Xml.GetFirstElementValueByTagName(xmlDoc, "Host");
                if (!String.IsNullOrEmpty(value))
                    credential.Domain = Encryptions.DecryptDES(value).Trim();

                value = Xml.GetFirstElementValueByTagName(xmlDoc, "Port");
                if (!String.IsNullOrEmpty(value))
                    credential.Port = Convert.ToUInt16(Encryptions.DecryptDES(value));

                value = Xml.GetFirstElementValueByTagName(xmlDoc, "Account");
                if (!String.IsNullOrEmpty(value))
                    credential.UserName = Encryptions.DecryptDES(value);

                value = Xml.GetFirstElementValueByTagName(xmlDoc, "Password");
                if (!String.IsNullOrEmpty(value))
                    credential.Password = Encryptions.DecryptDES(value);

                value = Xml.GetFirstElementValueByTagName(xmlDoc, "SSL");
                if (!String.IsNullOrEmpty(value))
                    credential.SSLEnable = (value == "true");

                value = Xml.GetFirstElementValueByTagName(xmlDoc, "RememberMe");
                if (!String.IsNullOrEmpty(value))
                    credential.RememberMe = (value == "true");
            }
            else //old XXX;YYY;ZZZ format
            {
                var config = str.Split(',');

                if (config.Length > 0)
                    credential.Domain = Encryptions.DecryptDES(config[0]).Trim();

                if (config.Length > 1)
                    credential.Port = Convert.ToUInt16(Encryptions.DecryptDES(config[1]));

                if (config.Length > 2)
                    credential.UserName = Encryptions.DecryptDES(config[2]);

                if (config.Length > 3)
                    credential.Password = Encryptions.DecryptDES(config[3]);
            }

            return credential;
        }
    }
}
