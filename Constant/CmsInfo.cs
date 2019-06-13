using System.Linq;
using System.Xml.Serialization;

namespace Constant
{
    [XmlRoot("CMSInfo")]
    public class CmsInfo
    {
        [XmlElement("UserGroups")]
        public UserGroup UserGroup { get; set; }

        public ServerConfig Config { get; set; }

        public string GetUserName()
        {
            var user = UserGroup.Users.FirstOrDefault();

            return user == null ? null : Encryptions.DecryptDES(user.EncryptedName);
        }

        public string GetPassword()
        {
            var user = UserGroup.Users.FirstOrDefault();

            return user == null ? null : Encryptions.DecryptDES(user.EncryptedPassword);
        }
    }

    public class UserGroup
    {
        [XmlArray("Users"), XmlArrayItem("User")]
        public CmsUser[] Users { get; set; }
    }

    public class CmsUser
    {
        [XmlAttribute("id")]
        public ushort Id { get; set; }


        [XmlAttribute("name")]
        public string EncryptedName { get; set; }

        [XmlElement("Password")]
        public string EncryptedPassword { get; set; }

        public string Email { get; set; }

        public string Group { get; set; }
    }

    public class ServerConfig
    {
        public ushort Port { get; set; }

        public ushort SSLPort { get; set; }
    }
}