using System;
using System.Collections.Generic;
using System.Xml;
using Constant;
using DeviceConstant;
using Interface;

namespace ServerProfile
{
    public partial class DeviceManager
    {
        private const String CgiLoadAllBookmark = @"cgi-bin/bookmark?action=loadallbookmark";
        private const String CgiLoadBookmark = @"cgi-bin/bookmark?channel=channel%1&action=load&nvr=nvr{NVRID}";
        private const String CgiSaveBookmark = @"cgi-bin/bookmark?channel=channel%1&action=save&nvr=nvr{NVRID}";
        private const String CgiDeleteBookmark = @"cgi-bin/bookmark?channel=channel%1&action=delete&nvr=nvr{NVRID}"; // for CMS

        private readonly System.Timers.Timer _saveBookmarkTimer = new System.Timers.Timer();

        public void InitializeBookmarkTimer()
        {
            _saveBookmarkTimer.Elapsed += SaveBookmarkTimer;
            _saveBookmarkTimer.Interval = 3000;//1 sec
            _saveBookmarkTimer.SynchronizingObject = Server.Form;
        }

        private void LoadAllBookmark()
        {
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllBookmark, Server.Credential);
            if (xmlDoc == null) return;

            XmlNodeList bookmarksList = xmlDoc.GetElementsByTagName("Bookmarks");
            foreach (XmlElement node in bookmarksList)
            {
                if (node.GetAttribute("id") == "") continue;
                UInt16 id = Convert.ToUInt16(node.GetAttribute("id"));
                if (!Devices.ContainsKey(id)) continue;

                if (Devices[id] is ICamera)
                {
                    LoadBookmark((ICamera)(Devices[id]), node);
                }
            }
        }

        //<Bookmarks id="1">
        //  <Creator name ="">//DES & BASE64 User Name
        //      <Bookmark>
        //          <DateTime>1234567890</DateTime> //DateTime.Ticks
        //          <CreateDateTime>1234567890</CreateDateTime> //DateTime.Ticks
        //      </Bookmark>
        //  <Creator>
        //</Bookmarks>
        private void LoadBookmark(ICamera camera, XmlElement rootNode)
        {
            var creators = rootNode.GetElementsByTagName("Creator");
            if (creators.Count <= 0) return;

            foreach (XmlElement node in creators)
            {
                String name = Encryptions.DecryptDES(node.GetAttribute("name"));
                if (name != Server.Credential.UserName) continue;

                XmlNodeList bookmarks = node.GetElementsByTagName("Bookmark");
                foreach (XmlElement bookmark in bookmarks)
                {
                    camera.Bookmarks.Add(new Bookmark
                                             {
                                                 Creator = Server.Credential.UserName,
                                                 DateTime = new DateTime(Convert.ToInt64(Xml.GetFirstElementValueByTagName(bookmark, "DateTime"))),
                                                 CreateDateTime = new DateTime(Convert.ToInt64(Xml.GetFirstElementValueByTagName(bookmark, "CreateDateTime"))),
                                             });
                }

                camera.Bookmarks.Sort((x, y) => x.DateTime.CompareTo(y.DateTime));

                //camera.DescBookmarks = new List<Bookmark>(camera.Bookmarks);
                //camera.DescBookmarks.Reverse();
                break;
            }
        }

        private delegate void SaveBookmarkDelegate(UInt16 id);
        private readonly List<UInt16> _queueSaveBookmark = new List<UInt16>();
        private readonly List<UInt16> _processSaveBookmark = new List<UInt16>();
        private void SaveBookmark(UInt16 id)
        {
            if (!Devices.ContainsKey(id)) return;
            if (!(Devices[id] is ICamera)) return;

            //Last save not yet complete, and comes a new one, wait until last finish.
            if (_processSaveBookmark.Contains(id))
            {
                if (!_queueSaveBookmark.Contains(id))
                    _queueSaveBookmark.Add(id);

                _saveBookmarkTimer.Enabled = false;
                _saveBookmarkTimer.Enabled = true;
                return;
            }

            if (!_processSaveBookmark.Contains(id))
                _processSaveBookmark.Add(id);

            var camera = Devices[id] as ICamera;
            if(camera == null) return;

            var loadBookMarkCgi = CgiLoadBookmark.Replace("%1", id.ToString());
            if (camera.CMS != null)
                loadBookMarkCgi = loadBookMarkCgi.Replace("{NVRID}", camera.Server.Id.ToString());

            var xmlDoc = Xml.LoadXmlFromHttp(loadBookMarkCgi, camera.CMS != null ? camera.CMS.Credential : Server.Credential);

            if (xmlDoc != null && xmlDoc.InnerXml != "")
            {
                var xmlRoot = xmlDoc.FirstChild;
                var creators = xmlDoc.GetElementsByTagName("Creator");
                if (creators.Count > 0)
                {
                    foreach (XmlElement node in creators)
                    {
                        String name = Encryptions.DecryptDES(node.GetAttribute("name"));
                        if (camera.CMS != null)
                        {
                            if (name != camera.CMS.Credential.UserName) continue;
                        }
                        else
                        {
                            if (name != Server.Credential.UserName) continue;
                        }
                        xmlRoot.RemoveChild(node);
                        break;
                    }
                }
            }
            else
            {
                xmlDoc = new XmlDocument();
                var rootNode = xmlDoc.CreateElement("Bookmarks");
                if (camera.CMS != null)
                {
                    if (!camera.CMS.NVRManager.DeviceChannelTable.ContainsKey(camera)) return;
                    rootNode.SetAttribute("id", camera.CMS.NVRManager.DeviceChannelTable[camera].ToString());

                    rootNode.SetAttribute("device_id", camera.Id.ToString());
                }
                else
                {
                    rootNode.SetAttribute("id", camera.Id.ToString());
                }
                xmlDoc.AppendChild(rootNode);
            }

            var saveBookMarkCgi = CgiSaveBookmark.Replace("%1", id.ToString());
            if (camera.CMS != null)
            {
                //if (camera.Bookmarks.Count == 0) //delete bookmark
                //{
                //    Xml.LoadXmlFromHttp(CgiDeleteBookmark.Replace("%1", camera.Id.ToString()).Replace("{NVRID}", camera.Server.Id.ToString()), camera.CMS.Credential);
                //}
                //else
                //{
                //    Xml.PostXmlToHttp(saveBookMarkCgi.Replace("{NVRID}", camera.Server.Id.ToString()), ParseBookmarkToXml(camera, xmlDoc), camera.CMS.Credential);
                //}
                Xml.LoadXmlFromHttp(CgiDeleteBookmark.Replace("%1", camera.Id.ToString()).Replace("{NVRID}", camera.Server.Id.ToString()), camera.CMS.Credential);
                Xml.PostXmlToHttp(saveBookMarkCgi.Replace("{NVRID}", camera.Server.Id.ToString()), ParseBookmarkToXml(camera, xmlDoc), camera.CMS.Credential);
            }
            else
            {
                Xml.PostXmlToHttp(saveBookMarkCgi, ParseBookmarkToXml(camera, xmlDoc), Server.Credential);
            }
            _processSaveBookmark.Remove(id);
        }

        private void SaveBookmarkTimer(Object sender, EventArgs e)
        {
            try
            {
                _saveBookmarkTimer.Enabled = false;
                while (_queueSaveBookmark.Count > 0)
                {
                    SaveBookmarkDelegate saveBookmarkDelegate = SaveBookmark;
                    saveBookmarkDelegate.BeginInvoke(_queueSaveBookmark[0], null, null);
                    _queueSaveBookmark.RemoveAt(0);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private XmlDocument ParseBookmarkToXml(ICamera camera, XmlDocument xmlDoc)
        {
            //<Bookmarks>
            //  <Creator name ="">//DES & BASE64 User Name
            //      <Bookmark>
            //          <DateTime>1234567890</DateTime> //DateTime.Ticks
            //          <CreateDateTime>1234567890</CreateDateTime> //DateTime.Ticks
            //      </Bookmark>
            //  <Creator>
            //</Bookmarks>

            XmlNode xmlRoot = xmlDoc.FirstChild;
            XmlElement creator = xmlDoc.CreateElement("Creator");
            xmlRoot.AppendChild(creator);

            if (camera.CMS != null)
            {
                creator.SetAttribute("name", Encryptions.EncryptDES(camera.CMS.Credential.UserName));
            }
            else
            {
                creator.SetAttribute("name", Encryptions.EncryptDES(Server.Credential.UserName));
            }
            
            foreach (Bookmark bookmark in camera.Bookmarks)
            {
                XmlElement bookmarkNode = xmlDoc.CreateElement("Bookmark");
                creator.AppendChild(bookmarkNode);

                bookmarkNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "DateTime", bookmark.DateTime.Ticks.ToString()));
                bookmarkNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "CreateDateTime", bookmark.CreateDateTime.Ticks.ToString()));
            }

            return xmlDoc;
        }
    }
}