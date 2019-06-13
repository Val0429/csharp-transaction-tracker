using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Interface;

namespace ServerProfile.Plugin
{
	public class UserManager: ServerProfile.UserManager
	{
		// for SD Card Permission
		//protected override void ParsePagePermissionFromXml(XmlElement node, IUserGroup group)
		//{
		//    if (node == null) return;

		//    switch (group.Name)
		//    {
		//        case "Administrator":
		//        case "Speruser":
		//        case "User":
		//            var doc = node.OwnerDocument;
		//            doc.LoadXml("<XML><Permissions><Permission name=\"SD Playback\">Access</Permission></Permissions></XML>");
		//            var sdNode = doc.SelectSingleNode("//Permissions//Permission");
		//            node.AppendChild(sdNode);
		//            break;
		//    }

		//    XmlNodeList permissionNodes = node.GetElementsByTagName("Permission");

		//    var temp = new List<String>();
		//    foreach (XmlElement permissionNode in permissionNodes)
		//    {
		//        String name = permissionNode.GetAttribute("name");
		//        String[] permissions = permissionNode.InnerText.Split(',');
		//        temp.AddRange(permissions.Select(permission => name + "." + permission));

		//        AddPagePermission(group, name, permissions);
		//    }

		//}


		// For I/O Model Permission
		protected override void ParsePagePermissionFromXml(XmlElement node, IUserGroup group)
		{
		    if (node == null) return;

		    var setupNode = node.SelectSingleNode("Permission[@name='Setup']");

		    if ( setupNode != null )
		    {
		        setupNode.InnerText += ",IOModel,IOHandle";
		    }

		    XmlNodeList permissionNodes = node.GetElementsByTagName("Permission");

		    var temp = new List<String>();
		    foreach (XmlElement permissionNode in permissionNodes)
		    {
		        String name = permissionNode.GetAttribute("name");
		        String[] permissions = permissionNode.InnerText.Split(',');
		        temp.AddRange(permissions.Select(permission => name + "." + permission));

		        AddPagePermission(group, name, permissions);
		    }

		}
	}
}
