using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml;
using Constant;
using Interface;

namespace EMap
{
    public class MapHandler : IServerUse
    {
        private Int32 _checkNewIdCount;
        public int CheckFileNameCount { get; set; }
        public ICMS CMS;
        private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (value is ICMS)
                    CMS = value as ICMS;
            }
        }
        private XmlDocument _doc;

        public XmlDocument MakeMapSaveXmlDocument()
        {
            _doc = new XmlDocument();
            //XmlDeclaration dec = _doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            //_doc.AppendChild(dec);
            XmlElement root = _doc.CreateElement("Maps");
            _doc.AppendChild(root);

            foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
            {
                AddMapToXml(map.Value);
            }

            return _doc;
        }

        private void AddMapToXml(MapAttribute map)
        {
            AddMapToXmlDocument(map);

            foreach (KeyValuePair<String, CameraAttributes> camera in map.Cameras)
            {
                AddCameraToXmlDocument(map.Id, camera.Value);
            }

            foreach (KeyValuePair<String, ViaAttributes> via in map.Vias)
            {
                AddViaToXmlDocument(map.Id,via.Value);
            }

            foreach (KeyValuePair<String, NVRAttributes> nvr in map.NVRs)
            {
                AddNVRToXmlDocument(map.Id, nvr.Value);
            }

            foreach (KeyValuePair<String, MapHotZoneAttributes> hotzone in map.HotZones)
            {
                AddHotZoneToXmlDocument(map.Id,hotzone.Value);
            }

            if(map.Image != null)
            {
                CMS.NVRManager.UploadMap(map.Image, map.SystemFile);
                map.Image = null;
            }
        }

        public void AddMapToXmlDocument(MapAttribute map)
        {
            NumberFormatInfo provider = new NumberFormatInfo
            {
                NumberDecimalSeparator = ".",
                NumberGroupSeparator = "."
            };

            var maps = _doc.SelectSingleNode("Maps");

            var newMap = _doc.CreateNode(XmlNodeType.Element, "Map", null);

            if (newMap.Attributes != null)
            {
                //attribute id
                var newId = _doc.CreateAttribute("Id");
                newId.Value = map.Id;
                newMap.Attributes.Append(newId);

                //parent id
                var newParentId = _doc.CreateAttribute("parentId");
                newParentId.Value = map.ParentId;
                newMap.Attributes.Append(newParentId);

                //attribute name
                var newName = _doc.CreateAttribute("Name");
                if(String.IsNullOrEmpty(map.Name))
                    map.Name = String.Format("Empty name {0}", map.Id);
                newName.Value =map.Name;
                newMap.Attributes.Append(newName);

                //attribute original file
                var newPath = _doc.CreateAttribute("OriginalFile");
                newPath.Value = map.OriginalFile;
                newMap.Attributes.Append(newPath);

                //attribute system file
                var newSysFile = _doc.CreateAttribute("SystemFile");
                newSysFile.Value = map.SystemFile;
                newMap.Attributes.Append(newSysFile);

                //attribute width
                var newWidth = _doc.CreateAttribute("Width");
                newWidth.Value = map.Width.ToString();
                newMap.Attributes.Append(newWidth);

                //attribute height
                var newHeight = _doc.CreateAttribute("Height");
                newHeight.Value = map.Height.ToString();
                newMap.Attributes.Append(newHeight);

                //attribute isDefault
                var newIsDefault = _doc.CreateAttribute("IsDefault");
                newIsDefault.Value = map.IsDefault ? "Y" : "N";
                newMap.Attributes.Append(newIsDefault);

                //attribute scale
                var newScale = _doc.CreateAttribute("Scale");
                newScale.Value = map.Scale.ToString();
                newMap.Attributes.Append(newScale);

                //attribute X
                var newX = _doc.CreateAttribute("X");
                newX.Value = map.X.ToString("#.##", provider);
                newMap.Attributes.Append(newX);

                //attribute Y
                var newY = _doc.CreateAttribute("Y");
                newY.Value = map.Y.ToString("#.##", provider);
                newMap.Attributes.Append(newY);

                //attribute scalse center X
                var newCenterX = _doc.CreateAttribute("ScaleCenterX");
                newCenterX.Value = map.ScaleCenterX.ToString("#.##", provider);
                newMap.Attributes.Append(newCenterX);

                //attribute scalse center Y
                var newCenterY = _doc.CreateAttribute("ScaleCenterY");
                newCenterY.Value = map.ScaleCenterY.ToString("#.##", provider);
                newMap.Attributes.Append(newCenterY);

                //Cameras
                var newCams = _doc.CreateNode(XmlNodeType.Element, "Cameras", null);
                newMap.AppendChild(newCams);

                //NVRs
                var newNVRs = _doc.CreateNode(XmlNodeType.Element, "NVRs", null);
                newMap.AppendChild(newNVRs);

                //Vias
                var newVias = _doc.CreateNode(XmlNodeType.Element, "Vias", null);
                newMap.AppendChild(newVias);

                //Hot zone
                var newZone = _doc.CreateNode(XmlNodeType.Element, "HotZones", null);
                newMap.AppendChild(newZone);

                if (maps != null) maps.AppendChild(newMap);
            }
        }

        public void AddCameraToXmlDocument(String mapId, CameraAttributes camera)
        {
            var map = FindFirstNodeByAttribute(_doc, "/Maps/Map", "Id", mapId);
            NumberFormatInfo provider = new NumberFormatInfo
            {
                NumberDecimalSeparator = ".",
                NumberGroupSeparator = "."
            };
            if (map != null)
            {
                var cameras = map.SelectNodes("Cameras");
                if (cameras != null)
                    if (cameras.Count > 0)
                    {
                        var camRoot = cameras[0];

                        var newCam = _doc.CreateNode(XmlNodeType.Element, "Camera", null);

                        if (newCam.Attributes != null)
                        {

                            //attribute id
                            var newId = _doc.CreateAttribute("Id");
                            newId.Value = camera.Id;
                            newCam.Attributes.Append(newId);

                            //attribute name
                            var newName = _doc.CreateAttribute("Name");
                            newName.Value = camera.Name;
                            newCam.Attributes.Append(newName);

                            //attribute system id
                            var newSysId = _doc.CreateAttribute("SystemId");
                            newSysId.Value = camera.SystemId.ToString();
                            newCam.Attributes.Append(newSysId);

                            //attribute system nvr id
                            var newNVRSysId = _doc.CreateAttribute("NVRSystemId");
                            newNVRSysId.Value = camera.NVRSystemId.ToString();
                            newCam.Attributes.Append(newNVRSysId);

                            //attribute X
                            var newX = _doc.CreateAttribute("X");
                            newX.Value = camera.X.ToString("#.##", provider);
                            newCam.Attributes.Append(newX);

                            //attribute Y
                            var newY = _doc.CreateAttribute("Y");
                            newY.Value = camera.Y.ToString("#.##", provider);
                            newCam.Attributes.Append(newY);

                            //attribute rotate
                            var newrotate = _doc.CreateAttribute("Rotate");
                            newrotate.Value = camera.Rotate.ToString();
                            newCam.Attributes.Append(newrotate);

                            //attribute DescX
                            var newDescX = _doc.CreateAttribute("DescX");
                            newDescX.Value = camera.DescX.ToString("#.##", provider);
                            newCam.Attributes.Append(newDescX);

                            //attribute DescY
                            var newDescY = _doc.CreateAttribute("DescY");
                            newDescY.Value = camera.DescY.ToString("#.##", provider);
                            newCam.Attributes.Append(newDescY);

                            //attribute Speak
                            var newSpeak = _doc.CreateAttribute("IsSpeakerEnabled");
                            newSpeak.Value = camera.IsSpeakerEnabled ? "Y" : "N";
                            newCam.Attributes.Append(newSpeak);

                            //attribute SpeakerX
                            var newSpeakX = _doc.CreateAttribute("SpeakerX");
                            newSpeakX.Value = camera.SpeakerX.ToString("#.##", provider);
                            newCam.Attributes.Append(newSpeakX);

                            //attribute SpeakerY
                            var newSpeakY = _doc.CreateAttribute("SpeakerY");
                            newSpeakY.Value = camera.SpeakerY.ToString("#.##", provider);
                            newCam.Attributes.Append(newSpeakY);

                            //attribute Audio
                            var newAudio = _doc.CreateAttribute("IsAudioEnabled");
                            newAudio.Value = camera.IsAudioEnabled ? "Y" : "N";
                            newCam.Attributes.Append(newAudio);

                            //attribute AudioX
                            var newAudioX = _doc.CreateAttribute("AudioX");
                            newAudioX.Value = camera.AudioX.ToString("#.##", provider);
                            newCam.Attributes.Append(newAudioX);

                            //attribute AudioX
                            var newAudioY = _doc.CreateAttribute("AudioY");
                            newAudioY.Value = camera.AudioY.ToString("#.##", provider);
                            newCam.Attributes.Append(newAudioY);

                            //attribute IsOpenVideoWindow
                            var newIsOpenVideoWindow = _doc.CreateAttribute("IsOpenVideoWindow");
                            newIsOpenVideoWindow.Value = "N";
                            newCam.Attributes.Append(newIsOpenVideoWindow);

                            //attribute VideoWindowX
                            var newVideoWindowX = _doc.CreateAttribute("VideoWindowX");
                            newVideoWindowX.Value = camera.VideoWindowX.ToString("#.##", provider);
                            newCam.Attributes.Append(newVideoWindowX);

                            //attribute VideoWindowY
                            var newVideoWindowY = _doc.CreateAttribute("VideoWindowY");
                            newVideoWindowY.Value = camera.VideoWindowY.ToString("#.##", provider);
                            newCam.Attributes.Append(newVideoWindowY);

                            //attribute VideoWindowSize
                            var newVideoWindowSize = _doc.CreateAttribute("VideoWindowSize");
                            newVideoWindowSize.Value = camera.VideoWindowSize.ToString();
                            newCam.Attributes.Append(newVideoWindowSize);

                            if (camRoot != null) camRoot.AppendChild(newCam);

                        }
                    }
            }
        }

        public void AddViaToXmlDocument(String mapId, ViaAttributes via)
        {
            var map = FindFirstNodeByAttribute(_doc, "/Maps/Map", "Id", mapId);
            NumberFormatInfo provider = new NumberFormatInfo
            {
                NumberDecimalSeparator = ".",
                NumberGroupSeparator = "."
            };
            if (map != null)
            {
                var vias = map.SelectNodes("Vias");
                if (vias != null)
                    if (vias.Count > 0)
                    {
                        var viaRoot = vias[0];

                        var newVia = _doc.CreateNode(XmlNodeType.Element, "Via", null);

                        if (newVia.Attributes != null)
                        {

                            //attribute id
                            var newId = _doc.CreateAttribute("Id");
                            newId.Value = via.Id;
                            newVia.Attributes.Append(newId);

                            //attribute name
                            var newName = _doc.CreateAttribute("Name");
                            if (String.IsNullOrEmpty(via.Name))
                                via.Name = String.Format("Empty name {0}", via.Id);
                            newName.Value = via.Name;
                            newVia.Attributes.Append(newName);

                            //attribute name
                            var newType = _doc.CreateAttribute("Type");
                            newType.Value = via.Type;
                            newVia.Attributes.Append(newType);

                            //attribute X
                            var newX = _doc.CreateAttribute("X");
                            newX.Value = via.X.ToString("#.##", provider);
                            newVia.Attributes.Append(newX);

                            //attribute Y
                            var newY = _doc.CreateAttribute("Y");
                            newY.Value = via.Y.ToString("#.##", provider);
                            newVia.Attributes.Append(newY);

                            //attribute rotate
                            var newrotate = _doc.CreateAttribute("Rotate");
                            newrotate.Value = "0";
                            newVia.Attributes.Append(newrotate);

                            //attribute DescX
                            var newDescX = _doc.CreateAttribute("DescX");
                            newDescX.Value = via.DescX.ToString("#.##", provider);
                            newVia.Attributes.Append(newDescX);

                            //attribute DescY
                            var newDescY = _doc.CreateAttribute("DescY");
                            newDescY.Value = via.DescY.ToString("#.##", provider);
                            newVia.Attributes.Append(newDescY);

                            //attribute link to map
                            var newLink = _doc.CreateAttribute("LinkToMap");
                            newLink.Value = via.LinkToMap;
                            newVia.Attributes.Append(newLink);

                            if (viaRoot != null) viaRoot.AppendChild(newVia);
                        }
                    }
            }
        }

        public void AddNVRToXmlDocument(String mapId, NVRAttributes nvr)
        {
            var map = FindFirstNodeByAttribute(_doc, "/Maps/Map", "Id", mapId);
            NumberFormatInfo provider = new NumberFormatInfo
            {
                NumberDecimalSeparator = ".",
                NumberGroupSeparator = "."
            };
            if (map != null)
            {
                var nvrs = map.SelectNodes("NVRs");
                if (nvrs != null)
                    if (nvrs.Count > 0)
                    {
                        var nvrRoot = nvrs[0];

                        var newNVR = _doc.CreateNode(XmlNodeType.Element, "NVR", null);

                        if (newNVR.Attributes != null)
                        {

                            //attribute id
                            var newId = _doc.CreateAttribute("Id");
                            newId.Value = nvr.Id;
                            newNVR.Attributes.Append(newId);

                            //attribute name
                            var newName = _doc.CreateAttribute("Name");
                            newName.Value = nvr.Name;
                            newNVR.Attributes.Append(newName);

                            //attribute type
                            var newType = _doc.CreateAttribute("Type");
                            newType.Value = nvr.Type;
                            newNVR.Attributes.Append(newType);

                            //attribute system id
                            var newSysId = _doc.CreateAttribute("SystemId");
                            newSysId.Value = nvr.SystemId.ToString();
                            newNVR.Attributes.Append(newSysId);

                            //attribute X
                            var newX = _doc.CreateAttribute("X");
                            newX.Value = nvr.X.ToString("#.##", provider);
                            newNVR.Attributes.Append(newX);

                            //attribute Y
                            var newY = _doc.CreateAttribute("Y");
                            newY.Value = nvr.Y.ToString("#.##", provider);
                            newNVR.Attributes.Append(newY);

                            //attribute DescX
                            var newDescX = _doc.CreateAttribute("DescX");
                            newDescX.Value = nvr.DescX.ToString("#.##", provider);
                            newNVR.Attributes.Append(newDescX);

                            //attribute DescY
                            var newDescY = _doc.CreateAttribute("DescY");
                            newDescY.Value = nvr.DescY.ToString("#.##", provider);
                            newNVR.Attributes.Append(newDescY);

                            //attribute link to map
                            var newLink = _doc.CreateAttribute("LinkToMap");
                            newLink.Value = nvr.LinkToMap;
                            newNVR.Attributes.Append(newLink);

                            if (nvrRoot != null) nvrRoot.AppendChild(newNVR);
                        }
                    }
            }
        }

        public void AddHotZoneToXmlDocument(String mapId, MapHotZoneAttributes hotzone)
        {
            var map = FindFirstNodeByAttribute(_doc, "/Maps/Map", "Id", mapId);
            NumberFormatInfo provider = new NumberFormatInfo
            {
                NumberDecimalSeparator = ".",
                NumberGroupSeparator = "."
            };
            if (map != null)
            {
                var hotzones = map.SelectNodes("HotZones");
                if (hotzones != null)
                    if (hotzones.Count > 0)
                    {
                        var hotzoneRoot = hotzones[0];

                        var newZone = _doc.CreateNode(XmlNodeType.Element, "HotZone", null);

                        if (newZone.Attributes != null)
                        {

                            //attribute id
                            var newId = _doc.CreateAttribute("Id");
                            newId.Value = hotzone.Id;
                            newZone.Attributes.Append(newId);

                            //attribute link to map
                            var newLink = _doc.CreateAttribute("LinkToMap");
                            newLink.Value = hotzone.LinkToMap;
                            newZone.Attributes.Append(newLink);

                            //attribute opacity
                            var newOpacity = _doc.CreateAttribute("Opacity");
                            newOpacity.Value = hotzone.Opacity.ToString();
                            newZone.Attributes.Append(newOpacity);

                            //attribute color
                            var newColor = _doc.CreateAttribute("Color");
                            newColor.Value = hotzone.Color.ToArgb().ToString();
                            newZone.Attributes.Append(newColor);

                            //attribute name
                            var newName = _doc.CreateAttribute("Name");
                            if (String.IsNullOrEmpty(hotzone.Name))
                                hotzone.Name = String.Format("Empty name {0}", hotzone.Id);
                            newName.Value = hotzone.Name;
                            newZone.Attributes.Append(newName);

                            //attribute DescX
                            var newDescX = _doc.CreateAttribute("DescX");
                            newDescX.Value = hotzone.DescX.ToString("#.##", provider);
                            newZone.Attributes.Append(newDescX);

                            //attribute DescY
                            var newDescY = _doc.CreateAttribute("DescY");
                            newDescY.Value = hotzone.DescY.ToString("#.##", provider);
                            newZone.Attributes.Append(newDescY);

                            foreach (Point point in hotzone.Points)
                            {
                                var newPoint = _doc.CreateNode(XmlNodeType.Element, "Point", null);
                                if (newPoint.Attributes != null)
                                {
                                    //attribute X
                                    var newX = _doc.CreateAttribute("X");
                                    newX.Value = point.X.ToString("#.##", provider);
                                    newPoint.Attributes.Append(newX);

                                    //attribute Y
                                    var newY = _doc.CreateAttribute("Y");
                                    newY.Value = point.Y.ToString("#.##", provider);
                                    newPoint.Attributes.Append(newY);

                                    newZone.AppendChild(newPoint);
                                }
                            }

                            if (hotzoneRoot != null) hotzoneRoot.AppendChild(newZone);
                        }
                    }
            }
        }

        public XmlNode FindFirstNodeByAttribute(XmlDocument document, String tree, String attribute, String value)
        {
            var list = FindNodeByAttribute(document, tree, attribute, value);
            return list.Count > 0 ? list[0] : null;
        }

        public XmlNodeList FindNodeByAttribute(XmlDocument document, String tree, String attribute, String value)
        {
            return document.SelectNodes(String.Format("{0}[@{1}='{2}']", tree, attribute, value));
        }

        public void SyncDeviceAndNVRName(INVRManager nvrManager)
        {
            var deleteCam = new List<String>();
            var deleteNVR = new List<String>();
            foreach (KeyValuePair<string, MapAttribute> map in CMS.NVRManager.Maps)
            {
                //Cameras
                foreach (KeyValuePair<string, CameraAttributes> camera in map.Value.Cameras)
                {
                    INVR nvr = nvrManager.FindNVRById(Convert.ToUInt16(camera.Value.NVRSystemId));
                    if (nvr == null)
                    {
                        deleteCam.Add(camera.Key);
                        continue;
                    }

                    if (nvr.ReadyState != ReadyState.Ready && nvr.ReadyState != ReadyState.Modify) 
                    {
                        deleteCam.Add(camera.Key);
                        continue;
                    }

                    if (nvr.Device.FindDeviceById(Convert.ToUInt16(camera.Value.SystemId)) == null)
                    {
                        deleteCam.Add(camera.Key);
                    }
                    else
                    {
                        var cameraDevice = nvr.Device.FindDeviceById(Convert.ToUInt16(camera.Value.SystemId)) as ICamera;
                        if (cameraDevice == null)
                        {
                            deleteCam.Add(camera.Key);
                        }
                        else
                        {
                            camera.Value.Name = String.Format("{0} ({1})", cameraDevice, nvr.Name);
                        }
                        
                    }
                }

                foreach (string id in deleteCam)
                {
                    RemoveCameraById(id);
                }

                //NVRs
                foreach (KeyValuePair<string, NVRAttributes> nvr in map.Value.NVRs)
                {
                    INVR nvrServer = nvrManager.FindNVRById(Convert.ToUInt16(nvr.Value.SystemId));
                    if (nvrServer == null)
                    {
                        deleteNVR.Add(nvr.Key);
                        continue;
                    }
                    if (nvrServer.ReadyState != ReadyState.Ready)
                    {
                        deleteNVR.Add(nvr.Key);
                    }
                    else
                    {
                        nvr.Value.Name = nvrServer.Name;
                    }
                }

                foreach (string id in deleteNVR)
                {
                    RemoveNVRById(id);
                }

            }

        }

        //public String CheckAndRemoveMapImage(XmlDocument xml)
        //{
        //    try
        //    {
        //        var fileDir = new DirectoryInfo(Properties.Resources.MapPath);

        //        if (fileDir.Exists)
        //        {
        //            var files = fileDir.GetFiles();

        //            foreach (FileInfo file in files)
        //            {
        //                var image = FindFirstNodeByAttribute(xml, "/Maps/Map", "Path", file.Name);
        //                if (image == null)
        //                {
        //                    File.Delete(file.FullName);
        //                }
        //            }
        //        }

        //        return "OK";
        //    }
        //    catch (Exception e)
        //    {
        //        return e.Message;
        //    }
            
        //}



        //public MapAttribute CreateMapAttributes(XmlNode mapXml,INVRManager nvrManager)
        //{
        //    if (mapXml.Attributes != null)
        //    {
        //        var path = String.Empty;
        //        if(mapXml.Attributes["Path"] != null)
        //        {
        //            path = mapXml.Attributes["Path"].Value;
        //        }    
                    
        //        var map = new MapAttribute
        //        {
        //            Id = mapXml.Attributes["Id"].Value,
        //            Name = mapXml.Attributes["Name"].Value,
        //            OriginalFile = mapXml.Attributes["OriginalFile"].Value,
        //            SystemFile = mapXml.Attributes["SystemFile"].Value,
        //            Width = int.Parse(mapXml.Attributes["Width"].Value),
        //            Height = int.Parse(mapXml.Attributes["Height"].Value),
        //            IsDefault = mapXml.Attributes["IsDefault"].Value == "Y" ? true : false,
        //            Scale = int.Parse(mapXml.Attributes["Scale"].Value),
        //            X = Convert.ToDouble(mapXml.Attributes["X"].Value),
        //            Y = Convert.ToDouble(mapXml.Attributes["Y"].Value),
        //            ScaleCenterX = Convert.ToDouble(mapXml.Attributes["ScaleCenterX"].Value),
        //            ScaleCenterY = Convert.ToDouble(mapXml.Attributes["ScaleCenterY"].Value),
        //            //NVRs = new List<NVRAttributes>(),
        //            NVRs = new Dictionary<string, NVRAttributes>(),
        //            //Cameras = new List<CameraAttributes>(),
        //            Cameras = new Dictionary<string, CameraAttributes>(),
        //            //Vias = new List<ViaAttributes>()
        //            Vias = new Dictionary<string, ViaAttributes>()
        //        };

        //        //Add NVRs in maps
        //        XmlNodeList nvrs = ((XmlElement)mapXml).GetElementsByTagName("NVR");
        //        foreach (XmlElement nvr in nvrs)
        //        {
        //            INVR nvrServer = nvrManager.FindNVRById(Convert.ToUInt16(nvr.Attributes["SystemId"].Value));
        //            if (nvrServer == null) continue;
        //            if (nvrServer.ReadyState != ReadyState.Ready) continue;

        //            var newNVR = new NVRAttributes
        //            {
        //                Id = nvr.Attributes["Id"].Value,
        //                SystemId = Convert.ToUInt16(nvr.Attributes["SystemId"].Value),
        //                Name = nvrServer.Name,
        //                X = Convert.ToDouble(nvr.Attributes["X"].Value),
        //                Y = Convert.ToDouble(nvr.Attributes["Y"].Value),
        //                LinkToMap = nvr.Attributes["LinkToMap"].Value,
        //                DescX = Double.Parse(nvr.Attributes["DescX"].Value),
        //                DescY = Double.Parse(nvr.Attributes["DescY"].Value),
        //                Type = "NVR"
        //            };

        //            map.NVRs.Add(newNVR.Id,newNVR);
        //        }

        //        //Add Cameras in map
        //        XmlNodeList cams = ((XmlElement)mapXml).GetElementsByTagName("Camera");

        //        foreach (XmlElement cam in cams)
        //        {
        //            if (nvrManager == null)
        //            {
        //                continue;
        //            }

        //            INVR nvr = nvrManager.FindNVRById(Convert.ToUInt16(cam.Attributes["NVRSystemId"].Value));
        //            if (nvr == null) continue;

        //            if (nvr.ReadyState != ReadyState.Ready && nvr.ReadyState != ReadyState.Modify) continue;
        //            if (nvr.Device.FindDeviceById(Convert.ToUInt16(cam.Attributes["SystemId"].Value)) == null) continue;

        //            var cameraDevice = nvr.Device.FindDeviceById(Convert.ToUInt16(cam.Attributes["SystemId"].Value)) as ICamera;
        //            if (cameraDevice != null)
        //            {
        //                var audoiIn = cameraDevice.Model.NumberOfAudioIn;
        //                var audioOut = cameraDevice.Model.NumberOfAudioOut;

        //                var camera = new CameraAttributes
        //                {
        //                    Id = (cam.Attributes["Id"].Value),// Convert.ToUInt16
        //                    SystemId = Convert.ToUInt16(cam.Attributes["SystemId"].Value),
        //                    Name =  String.Format("{0} ({1})",cameraDevice.ToString(),nvr.Name) ,//  devices[Convert.ToUInt16(cam.Attributes["SystemId"].Value)].ToString(),
        //                    Type = "Camera",
        //                    X = Double.Parse(cam.Attributes["X"].Value),
        //                    Y = Double.Parse(cam.Attributes["Y"].Value),
        //                    Rotate = int.Parse(cam.Attributes["Rotate"].Value),
        //                    DescX = Double.Parse(cam.Attributes["DescX"].Value),
        //                    DescY = Double.Parse(cam.Attributes["DescY"].Value),
        //                    IsSpeakerEnabled = audoiIn == 0 ? false : true,
        //                    SpeakerX = Double.Parse(cam.Attributes["SpeakerX"].Value),
        //                    SpeakerY = Double.Parse(cam.Attributes["SpeakerY"].Value),
        //                    IsAudioEnabled = audioOut == 0 ? false : true,
        //                    AudioX = Double.Parse(cam.Attributes["AudioX"].Value),
        //                    AudioY = Double.Parse(cam.Attributes["AudioY"].Value)
        //                };

        //                map.Cameras.Add(camera.Id,camera);
        //            }
        //        }

        //        //Add Vias in map
        //        XmlNodeList vias = ((XmlElement)mapXml).GetElementsByTagName("Via");

        //        foreach (XmlElement via in vias)
        //        {

        //            var tVia = new ViaAttributes
        //            {
        //                Id = via.Attributes["Id"].Value,
        //                Name = via.Attributes["Name"].Value,
        //                Type = "Via",
        //                X = Double.Parse(via.Attributes["X"].Value),
        //                Y = Double.Parse(via.Attributes["Y"].Value),
        //                LinkToMap = via.Attributes["LinkToMap"].Value,
        //                DescX = Double.Parse(via.Attributes["DescX"].Value),
        //                DescY = Double.Parse(via.Attributes["DescY"].Value)
        //            };

        //            map.Vias.Add(tVia.Id,tVia);

        //        }


        //        return map;
        //    }

        //    return null;
        //}

        //public MapAttribute FindFirstMap()
        //{
        //    if (CMS.NVR.Maps == null || CMS.NVR.Maps.Count == 0)
        //        return null;

        //    return CMS.NVR.Maps.Values.First();
        //}

        public MapAttribute FindDefaultMap()
        {
            MapAttribute result = null;

            foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
            {
                //if(result == null)
                //{
                //    result = map.Value;
                //} 

                if(map.Value.IsDefault)
                {
                    return map.Value;
                }
            }

            return result;
        }

        //public MapAttribute FindMapById(String id)
        //{
        //    MapAttribute result = null;

        //    foreach (KeyValuePair<String, MapAttribute> map in CMS.NVR.Maps)
        //    {
        //        if (map.Key == id)
        //        {
        //            return map.Value;
        //        }
        //        result = FindMapById(map.Value.MapNodes, id);
        //        if (result != null) return result;
        //    }

        //    return result;
        //}

        //private MapAttribute FindMapById(Dictionary<String, MapAttribute> maps, String id)
        //{
        //    MapAttribute result = null;

        //    foreach (KeyValuePair<String, MapAttribute> map in maps)
        //    {
        //        if (map.Key == id)
        //        {
        //            return map.Value;
        //        }
        //        result = FindMapById(map.Value.MapNodes, id);
        //        if (result != null) return result;
        //    }

        //    return result;
        //}

        public MapAttribute FindMapByCameraId(String id)
        {
            foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
            {
                foreach (KeyValuePair<String, CameraAttributes> camera in map.Value.Cameras)
                {
                    if (camera.Key == id)
                    {
                        return map.Value;
                    }
                }
            }

            return null;
        }

        public CameraAttributes FindCameraById(String id)
        {
            foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
            {
                foreach (KeyValuePair<String, CameraAttributes> camera in map.Value.Cameras)
                {
                    if (camera.Key == id)
                    {
                        return camera.Value;
                    }
                }
            }
            return null;
        }

        public List<CameraAttributes> FindCameraByDeviceIdNVRSystemId(UInt16 deviceId, UInt16 nvrSystemId)
        {
            var result = new List<CameraAttributes>();

            INVR nvr = CMS.NVRManager.FindNVRById(nvrSystemId);
            if (nvr == null) return result;
            if (nvr.ReadyState != ReadyState.Ready && nvr.ReadyState != ReadyState.Modify) return result;

            foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
            {
                foreach (KeyValuePair<String, CameraAttributes> camera in map.Value.Cameras)
                {
                    if (camera.Value.SystemId == deviceId && camera.Value.NVRSystemId == nvrSystemId)
                    {
                        if (!result.Contains(camera.Value))
                        {
                            result.Add(camera.Value);
                        }
                    }
                }
            }
            return result;
        }

        public Dictionary<String, MapAttribute> FindCameraByDeviceIdNVRSystemIdReturnMap(UInt16 deviceId, UInt16 nvrSystemId)
        {
            var result = new Dictionary<String, MapAttribute>();
            INVR nvr = CMS.NVRManager.FindNVRById(nvrSystemId);
            if (nvr == null) return null;
            if (nvr.ReadyState != ReadyState.Ready && nvr.ReadyState != ReadyState.Modify) return null;

            foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
            {
                foreach (KeyValuePair<String, CameraAttributes> camera in map.Value.Cameras)
                {
                    if (camera.Value.SystemId == deviceId && camera.Value.NVRSystemId == nvrSystemId)
                    {
                        result.Add(map.Key, map.Value);
                    }
                }
            }
            return result.Count == 0 ? null : result;
        }

        public NVRAttributes FindNVRById(String id)
        {
            foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
            {
                foreach (KeyValuePair<String, NVRAttributes> nvr in map.Value.NVRs)
                {
                    if (nvr.Key == id)
                    {
                        return nvr.Value;
                    }
                }
            }
            return null;
        }

        public ViaAttributes FindViaById(String id)
        {
            foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
            {
                foreach (KeyValuePair<String, ViaAttributes> via in map.Value.Vias)
                {
                    if (via.Key == id)
                    {
                        return via.Value;
                    }
                }
            }
            return null;
        }

        public MapHotZoneAttributes FindHotZoneById(String id)
        {
            foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
            {
                foreach (KeyValuePair<String, MapHotZoneAttributes> hotzone in map.Value.HotZones)
                {
                    if (hotzone.Key == id)
                    {
                        return hotzone.Value;
                    }
                }
            }
            return null;
        }

        public void RemoveMapById(String id)
        {
            CMS.NVRManager.Maps.Remove(id);
            RemoveSubMapById(id);
            if (CMS.NVRManager.Maps.Count == 0) CMS.NVRManager.Maps.Clear();
        }

        private void RemoveSubMapById(String id)
        {
            var list = new List<String>();
            foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
            {
                if(map.Value.ParentId == id)
                {
                    list.Add(map.Value.Id);
                }
            }

            foreach (String mapId in list)
            {
                CMS.NVRManager.Maps.Remove(mapId);
                RemoveSubMapById(mapId);
            }
        }

        public void RemoveCameraById(String id)
        {
            foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
            {
                foreach (KeyValuePair<String, CameraAttributes> camera in map.Value.Cameras)
                {
                    if (camera.Key == id)
                    {
                        map.Value.Cameras.Remove(id);
                        return ;
                    }
                }
            }
        }

        public void RemoveNVRById(String id)
        {
            foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
            {
                foreach (KeyValuePair<String, NVRAttributes> nvr in map.Value.NVRs)
                {
                    if (nvr.Key == id)
                    {
                        map.Value.NVRs.Remove(id);
                        return ;
                    }
                }
            }
        }

        public void RemoveViaById(String id)
        {
            foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
            {
                foreach (KeyValuePair<String, ViaAttributes> via in map.Value.Vias)
                {
                    if (via.Key == id)
                    {
                        map.Value.Vias.Remove(id);
                        return ;
                    }
                }
            }
        }

        public void RemoveHotZoneById(String id)
        {
            foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
            {
                foreach (KeyValuePair<String, MapHotZoneAttributes> hotZone in map.Value.HotZones)
                {
                    if (hotZone.Key == id)
                    {
                        map.Value.HotZones.Remove(id);
                        return;
                    }
                }
            }
        }

        public String CreateNewItemIdByType(String type, String mapId = null)
        {
            var exists = false;
            var newId = String.Empty;
            
            switch (type)
            {
                case "Map":
                    newId = (_checkNewIdCount).ToString();

                    var checkMap = CMS.NVRManager.FindMapById(newId);
                    exists = checkMap != null;
                    //foreach (KeyValuePair<string, MapAttribute> map in CMS.NVR.Maps)
                    //{
                    //    if (map.Key == newId)
                    //    {
                    //        exists = true;
                    //        break;
                    //    }
                    //}
                    break;
                case "Camera":
                    {
                        var tempCount = 0;

                        foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
                        {
                            tempCount += map.Value.Cameras.Count;
                        }

                        newId = (tempCount + _checkNewIdCount).ToString();

                        foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
                        {
                            foreach (KeyValuePair<String, CameraAttributes> cam in map.Value.Cameras)
                            {
                                if (cam.Key == newId)
                                {
                                    exists = true;
                                    break;
                                }
                            }
                        }

                    }
                    break;
                case "NVR":
                    {
                        var tempCount = 0;
                        foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
                        {
                            tempCount += map.Value.NVRs.Count;
                        }

                        newId = (tempCount + _checkNewIdCount).ToString();

                        foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
                        {
                            foreach (KeyValuePair<String, NVRAttributes> nvr in map.Value.NVRs)
                            {
                                if (nvr.Key == newId)
                                {
                                    exists = true;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case "Via":
                    {
                        var tempCount = 0;
                        foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
                        {
                            tempCount += map.Value.Vias.Count;
                        }

                        newId = (tempCount + _checkNewIdCount).ToString();

                        foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
                        {
                            foreach (KeyValuePair<String, ViaAttributes> via in map.Value.Vias)
                            {
                                if (via.Key == newId)
                                {
                                    exists = true;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case "HotZone":
                    {
                        var tempCount = 0;
                        foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
                        {
                            tempCount += map.Value.HotZones.Count;
                        }

                        newId = (tempCount + _checkNewIdCount).ToString();

                        foreach (KeyValuePair<String, MapAttribute> map in CMS.NVRManager.Maps)
                        {
                            foreach (KeyValuePair<String, MapHotZoneAttributes> zone in map.Value.HotZones)
                            {
                                if (zone.Key == newId)
                                {
                                    exists = true;
                                    break;
                                }
                            }
                        }
                    }
                    break;
            }

            if (exists)
            {
                _checkNewIdCount++;
                newId = CreateNewItemIdByType(type);
            }

            return newId;
        }

        public String CreateMapReturnMapId(String name, String originalFile, String width, String height, Boolean isDefault, String scale, String parentMapId = null)
        {
            _checkNewIdCount = 1;
            var id = CreateNewItemIdByType("Map");
            var newMap = new MapAttribute
                             {
                                 Id = id,
                                 ParentId = parentMapId,
                                 Name = String.Format("{0}{1}", name, id),
                                 OriginalFile = originalFile,
                                 Width = Convert.ToInt32(width),
                                 Height = Convert.ToInt32(height),
                                 IsDefault = isDefault,
                                 Scale = Convert.ToInt32(scale),
                                 X = 0,
                                 Y = 0,
                                 ScaleCenterX = 0,
                                 ScaleCenterY = 0,
                                 Cameras = new Dictionary<String, CameraAttributes>(),
                                 NVRs = new Dictionary<String, NVRAttributes>(),
                                 Vias = new Dictionary<String, ViaAttributes>(),
                                 HotZones = new Dictionary<String, MapHotZoneAttributes>()
                             };

            CMS.NVRManager.Maps.Add(id, newMap);

            return id;
        }

        public void MoveMapIndex(String mapId, UInt16 index)
        {
            var copy = new Dictionary<String, MapAttribute>(CMS.NVRManager.Maps);
            var map = copy[mapId];

            CMS.NVRManager.Maps.Clear();
            var count = 0;
            foreach (KeyValuePair<String, MapAttribute> mapAttribute in copy)
            {
                if (count == index) CMS.NVRManager.Maps.Add(map.Id, map);
                count++;
                if(mapAttribute.Key == mapId) continue;
                CMS.NVRManager.Maps.Add(mapAttribute.Key, mapAttribute.Value);
            }
        }

        public String CreateCameraReturnId(String mapId, CameraAttributes camera)
        {
            _checkNewIdCount = 1;
            var id = CreateNewItemIdByType("Camera");

            camera.Id = id;

            if (!CMS.NVRManager.Maps.ContainsKey(mapId)) return null;
            if (CMS.NVRManager.Maps[mapId] == null) return null;

            CMS.NVRManager.Maps[mapId].Cameras.Add(id,camera);

            return id;
        }

        public String CreateNVRReturnId(String mapId, NVRAttributes nvr)
        {
            _checkNewIdCount = 1;
            var id = CreateNewItemIdByType("NVR");

            nvr.Id = id;

            if (CMS.NVRManager.Maps[mapId] == null) return null;

            CMS.NVRManager.Maps[mapId].NVRs.Add(id,nvr);

            return id;
        }

        public String CreateViaReturnId( String mapId, ViaAttributes via)
        {
            _checkNewIdCount = 1;
            var id = CreateNewItemIdByType("Via");

            via.Id = id;
            
            CMS.NVRManager.Maps[mapId].Vias.Add(id,via);

            return id;
        }

        public String CreateHotZoneReturnId(String mapId, MapHotZoneAttributes mapHotZoneAttributes)
        {
            _checkNewIdCount = 1;
            var id = CreateNewItemIdByType("HotZone");

            mapHotZoneAttributes.Id = id;

            CMS.NVRManager.Maps[mapId].HotZones.Add(id, mapHotZoneAttributes);

            return id;
        }

        public String ConvertXmlDocumentToString(XmlDocument document)
        {
            var sw = new StringWriter();
            var xw = new XmlTextWriter(sw);
            document.WriteTo(xw);
            return sw.ToString();

        }

    }
}
