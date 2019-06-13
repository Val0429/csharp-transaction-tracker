using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;
using Device;
using DeviceConstant;
using Interface;

namespace ServerProfile
{
    public partial class DeviceManager
    {
        private const String CgiLoadAllEvent = @"cgi-bin/eventconfig?action=loadalleventhandler";
        //private const String CgiLoadEvent = @"cgi-bin/eventconfig?channel=channel%1&action=load";
        private const String CgiSaveAllEvent = @"cgi-bin/eventconfig?action=savealleventhandler";
        //private const String CgiSaveEvent = @"cgi-bin/eventconfig?channel=channel%1&action=save";
        //private const String CgiDeleteAllEvent = @"cgi-bin/eventconfig?action=deletealleventhandler";
        //private const String CgiDeleteEvent = @"cgi-bin/eventconfig?channel=channel%1&action=delete";

        public void LoadAllEvent()
        {
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllEvent, Server.Credential);

            if(Server is ICMS)
            {
                var cms = Server as ICMS;

                if(xmlDoc != null)
                {
                    var nvrList = xmlDoc.GetElementsByTagName("NVR");
                    foreach (XmlElement nvrNode in nvrList)
                    {
                        var nvrId = nvrNode.GetAttribute("id");
                        if (String.IsNullOrEmpty(nvrId)) continue;
                        if(!cms.NVRManager.NVRs.ContainsKey(Convert.ToUInt16(nvrId))) continue;
                        var nvr = cms.NVRManager.NVRs[Convert.ToUInt16(nvrId)];

                        var eventsList = nvrNode.GetElementsByTagName("EventHandlerConfiguration");
                        foreach (XmlElement node in eventsList)
                        {
                            var deviceId = Xml.GetFirstElementValueByTagName(node, "DeviceId");
                            if (String.IsNullOrEmpty(deviceId)) continue;
                            UInt16 id = Convert.ToUInt16(deviceId);
                            if (!nvr.Device.Devices.ContainsKey(id)) continue;

                            if (nvr.Device.Devices[id] is ICamera)
                            {
                                LoadEvent((ICamera)(nvr.Device.Devices[id]), node);
                            }
                        }
                    }
                }

                foreach (KeyValuePair<ushort, INVR> nvr in cms.NVRManager.NVRs)
                {
                    foreach (KeyValuePair<UInt16, IDevice> obj in nvr.Value.Device.Devices)
                    {
                        if (obj.Value is ICamera && ((ICamera)obj.Value).EventSchedule == null)
                        {
                            ((ICamera)obj.Value).EventSchedule = new Schedule();
                            ((ICamera)obj.Value).EventHandling = new EventHandling();
                            ((ICamera)obj.Value).EventHandling.SetDefaultEventHandling(((ICamera)obj.Value).Model);
                        }
                    }
                }

                return;
            }

             if (xmlDoc != null)
            {
                var eventsList = xmlDoc.GetElementsByTagName("EventHandlerConfiguration");
                foreach (XmlElement node in eventsList)
                {
                    if (node.GetAttribute("id") == "") continue;
                    UInt16 id = Convert.ToUInt16(node.GetAttribute("id"));
                    if (!Devices.ContainsKey(id)) continue;

                    if (Devices[id] is ICamera)
                    {
                        LoadEvent((ICamera)(Devices[id]), node);
                    }
                }
            }

            foreach (KeyValuePair<UInt16, IDevice> obj in Devices)
            {
                if (obj.Value is ICamera && ((ICamera)obj.Value).EventSchedule == null)
                {
                    ((ICamera)obj.Value).EventSchedule = new Schedule();
                    ((ICamera)obj.Value).EventHandling = new EventHandling();
                    ((ICamera)obj.Value).EventHandling.SetDefaultEventHandling(((ICamera)obj.Value).Model);
                }
            }
        }

        private void LoadEvent(ICamera camera, XmlElement rootNode)
        {
            camera.EventSchedule = new Schedule();
            camera.EventHandling = new EventHandling();
            camera.EventHandling.SetDefaultEventHandling(camera.Model);
            camera.EventHandling.ReadyState = ReadyState.Ready;

            if (rootNode == null) return;

            var scheduleNode = rootNode.SelectSingleNode("Schedule");
            if (scheduleNode == null) return;

            camera.EventSchedule.AddRange(ConvertWeekScheduleToScheduleData(
                    Xml.GetFirstElementValueByTagName(scheduleNode, "Mon") +
                    Xml.GetFirstElementValueByTagName(scheduleNode, "Tue") +
                    Xml.GetFirstElementValueByTagName(scheduleNode, "Wed") +
                    Xml.GetFirstElementValueByTagName(scheduleNode, "Thu") +
                    Xml.GetFirstElementValueByTagName(scheduleNode, "Fri") +
                    Xml.GetFirstElementValueByTagName(scheduleNode, "Sat") +
                    Xml.GetFirstElementValueByTagName(scheduleNode, "Sun"), "Event"));
            camera.EventSchedule.Description = ScheduleModes.CheckMode(camera.EventSchedule);

            //copy schedule when it's CUSTOM
            if (camera.EventSchedule.Description == ScheduleMode.CustomSchedule)
            {
                camera.EventSchedule.CustomSchedule = new List<ScheduleData>();
                ScheduleModes.Clone(camera.EventSchedule.CustomSchedule, camera.EventSchedule);
            }


            var events = rootNode.GetElementsByTagName("Event");
            if (events.Count <= 0) return;

            //List<EventCondition> eventConditions = new List<EventCondition>();
            foreach (XmlElement eventNode in events)
            {
                //String description = Xml.GetFirstElementsValueByTagName(eventNode, "Description");

                var conditionNodes = eventNode.GetElementsByTagName("Condition");

                List<EventHandle> eventHandle = null;
                if (conditionNodes.Count == 0) continue;

                //eventConditions.Clear();
                foreach (XmlElement conditionNode in conditionNodes)
                {
                    String type = conditionNode.GetAttribute("type");

                    if (!Enum.IsDefined(typeof(EventType), type)) continue;

                    var cameraEvent = new CameraEvent
                                          {
                                              Type = (EventType)Enum.Parse(typeof(EventType), type, true)
                                          };

                    String id = conditionNode.GetAttribute("id");
                    if (id != "")
                        cameraEvent.Id = Convert.ToUInt16(id);

                    String interval = conditionNode.GetAttribute("interval");

                    String value = conditionNode.GetAttribute("value");
                    if (value != "")
                        cameraEvent.Value = (value == "1");

                    foreach (KeyValuePair<EventCondition, List<EventHandle>> obj in camera.EventHandling)
                    {
                        if (obj.Key.CameraEvent.Type == cameraEvent.Type &&
                            obj.Key.CameraEvent.Id == cameraEvent.Id &&
                            obj.Key.CameraEvent.Value == cameraEvent.Value)
                        {
                            if (interval != "")
                                obj.Key.Interval = Convert.ToUInt16(interval);
                            eventHandle = obj.Value;
                            break;
                        }
                    }
                }

                if (eventHandle == null)
                    continue;

                var beepNodes = eventNode.GetElementsByTagName("Beep");
                if (beepNodes.Count > 0)
                {
                    foreach (XmlElement beepNode in beepNodes)
                    {
                        BeepHandleFromXml(beepNode, eventHandle);
                    }
                }

                var audioNodes = eventNode.GetElementsByTagName("Audio");
                if (audioNodes.Count > 0)
                {
                    foreach (XmlElement audioNode in audioNodes)
                    {
                        AudioHandleFromXml(audioNode, eventHandle);
                    }
                }

                var execNodes = eventNode.GetElementsByTagName("ExecCmd");
                if (execNodes.Count > 0)
                {
                    foreach (XmlElement execNode in execNodes)
                    {
                        ExecHandleFromXml(execNode, eventHandle);
                    }
                }

                var gotoNodes = eventNode.GetElementsByTagName("GoToPreset");
                if (gotoNodes.Count > 0)
                {
                    foreach (XmlElement gotoNode in gotoNodes)
                    {
                        GotoPresetHandleFromXml(gotoNode, eventHandle, Server is ICMS ? (ICMS)Server : null);
                    }
                }

                var popupNodes = eventNode.GetElementsByTagName("Popup");
                if (popupNodes.Count > 0)
                {
                    foreach (XmlElement popupNode in popupNodes)
                    {
                        PopupPlaybackHandleFromXml(popupNode, eventHandle, Server is ICMS ? (ICMS)Server : null);
                    }
                }

                var popupLiveNodes = eventNode.GetElementsByTagName("PopupLive");
                if (popupLiveNodes.Count > 0)
                {
                    foreach (XmlElement popupNode in popupLiveNodes)
                    {
                        PopupLiveHandleFromXml(popupNode, eventHandle, Server is ICMS ? (ICMS)Server : null);
                    }
                }

                var hotspotNodes = eventNode.GetElementsByTagName("HotSpot");
                if (hotspotNodes.Count > 0)
                {
                    foreach (XmlElement hotspotNode in hotspotNodes)
                    {
                        HotSpotHandleFromXml(hotspotNode, eventHandle, Server is ICMS ? (ICMS)Server : null);
                    }
                }

                var doNodes = eventNode.GetElementsByTagName("DigitalOut");
                if (doNodes.Count > 0)
                {
                    foreach (XmlElement doNode in doNodes)
                    {
                        TriggerDigitalOutHandleFromXml(doNode, eventHandle, Server is ICMS ? (ICMS)Server : null);
                    }
                }

                var mailNodes = eventNode.GetElementsByTagName("SendMail");
                if (mailNodes.Count > 0)
                {
                    foreach (XmlElement mailNode in mailNodes)
                    {
                        SendMailHandleFromXml(mailNode, eventHandle, Server is ICMS ? (ICMS)Server : null);
                    }
                }

                var ftpNodes = eventNode.GetElementsByTagName("UploadFTP");
                if (ftpNodes.Count > 0)
                {
                    foreach (XmlElement ftpNode in ftpNodes)
                    {
                        UploadFtpHandleFromXml(ftpNode, eventHandle, Server is ICMS ? (ICMS)Server : null);
                    }
                }
            }
        }

        //private delegate void LoadEventDelegate(ICamera camera);
        //private void LoadEvent(ICamera camera)
        //{
        //    XmlDocument xmlDoc = Utility.LoadXmlFromHttpWithGzip(CgiLoadEvent.Replace("%1", camera.Id.ToString()));
        //    if (xmlDoc == null) return;
        //    LoadEvent(camera, xmlDoc.SelectSingleNode("EventHandlerConfiguration") as XmlElement);
        //}

        protected virtual void BeepHandleFromXml(XmlElement node, List<EventHandle> eventHandle)
        {
            //<Beep>
            //    <Times>2</Times>
            //    <Duration>2</Duration>
            //    <Interval>1</Interval>
            //</Beep>

            var handle = new BeepEventHandle
            {
                Times = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "Times")),
                Duration = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "Duration")),
                ReadyState = ReadyState.Ready,
            };
            eventHandle.Add(handle);
        }

        protected virtual void AudioHandleFromXml(XmlElement node, List<EventHandle> eventHandle)
        {
            //<Audio>
            //    <FileName>C:\\1.wav</FileName>
            //</Audio>

            var handle = new AudioEventHandle
            {
                FileName = Xml.GetFirstElementValueByTagName(node, "FileName"),
                ReadyState = ReadyState.Ready,
            };
            eventHandle.Add(handle);
        }

        protected virtual void ExecHandleFromXml(XmlElement node, List<EventHandle> eventHandle)
        {
            //<ExecCmd>
            //    <FileName>C:\\1.wav</FileName>
            //</ExecCmd>

            var handle = new ExecEventHandle
            {
                FileName = Xml.GetFirstElementValueByTagName(node, "FileName"),
                ReadyState = ReadyState.Ready,
            };
            eventHandle.Add(handle);
        }

        private static void GotoPresetHandleFromXml(XmlElement node, List<EventHandle> eventHandle, ICMS cms)
        {
            //<GoToPreset>
            //    <NVR>1</NVR>
            //    <Device>1</Device>
            //    <Point>2</Point>
            //</GoToPreset>

            var point = Xml.GetFirstElementValueByTagName(node, "Point");
            if(String.IsNullOrEmpty(point)) return;

            var handle = new GotoPresetEventHandle
            {
                PresetPoint = Convert.ToUInt16(point),
                ReadyState = ReadyState.Ready,
            };

            var deviceId = Xml.GetFirstElementValueByTagName(node, "Device");
            IDevice device;
            if (cms != null)
            {
                String nvrId = Xml.GetFirstElementValueByTagName(node, "NVR");
                if(!String.IsNullOrEmpty(nvrId))
                {
                    var nvr = cms.NVRManager.FindNVRById(Convert.ToUInt16(nvrId));
                    if (nvr != null)
                    {
                        device = nvr.Device.Devices.ContainsKey(Convert.ToUInt16(deviceId))
                                     ? nvr.Device.Devices[Convert.ToUInt16(deviceId)]
                                     : null;
                    }
                    else
                    {
                        device = null;
                    }
                }
                else    
                {
                    device = null;
                }
                
            }
            else
            {
                device = new BasicDevice { Id = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "Device")) };
                
            }
            handle.Device = device;

            eventHandle.Add(handle);
        }

        protected virtual void PopupPlaybackHandleFromXml(XmlElement node, List<EventHandle> eventHandle, ICMS cms)
        {
            //<Popup>
            //    <NVR>1</NVR>
            //    <Device>1</Device>
            //</Popup>
            String deviceId = Xml.GetFirstElementValueByTagName(node, "Device");
            var handle = new PopupPlaybackEventHandle
            {
                Device = new BasicDevice { Id = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "Device")) },
                ReadyState = ReadyState.Ready,
            };

            if (cms != null)
            {
                String nvrId = Xml.GetFirstElementValueByTagName(node, "NVR");
                var nvr = cms.NVRManager.FindNVRById(Convert.ToUInt16(nvrId));
                if (nvr != null)
                {
                    handle.Device = nvr.Device.Devices.ContainsKey(Convert.ToUInt16(deviceId)) ? nvr.Device.Devices[Convert.ToUInt16(deviceId)] : null;
                }
                else
                {
                    handle.Device = null;
                }
            }

            eventHandle.Add(handle);
        }

        protected virtual void PopupLiveHandleFromXml(XmlElement node, List<EventHandle> eventHandle, ICMS cms)
        {
            //<PopupLive>
            //    <NVR>1</NVR>
            //    <Device>1</Device>
            //</PopupLive>
            String deviceId = Xml.GetFirstElementValueByTagName(node, "Device");
            var handle = new PopupLiveEventHandle
            {
                Device = new BasicDevice { Id = Convert.ToUInt16(deviceId) },
                ReadyState = ReadyState.Ready,
            };

            if (cms != null)
            {
                String nvrId = Xml.GetFirstElementValueByTagName(node, "NVR");
                var nvr = cms.NVRManager.FindNVRById(Convert.ToUInt16(nvrId));
                if (nvr != null)
                {
                    handle.Device = nvr.Device.Devices.ContainsKey(Convert.ToUInt16(deviceId)) ? nvr.Device.Devices[Convert.ToUInt16(deviceId)] : null;
                }
                else
                {
                    handle.Device = null;
                }
            }

            eventHandle.Add(handle);
        }
        
        protected virtual void HotSpotHandleFromXml(XmlElement node, List<EventHandle> eventHandle, ICMS cms)
        {
            //<HotSpot>
            //    <NVR>1</NVR>
            //    <Device>1</Device>
            //</HotSpot>

            String deviceId = Xml.GetFirstElementValueByTagName(node, "Device");
            var handle = new HotSpotEventHandle
            {
                Device = new BasicDevice { Id = Convert.ToUInt16(deviceId) },
                ReadyState = ReadyState.Ready,
            };

            if (cms != null)
            {
                String nvrId = Xml.GetFirstElementValueByTagName(node, "NVR");
                var nvr = cms.NVRManager.FindNVRById(Convert.ToUInt16(nvrId));
                if (nvr != null)
                {
                    handle.Device = nvr.Device.Devices.ContainsKey(Convert.ToUInt16(deviceId)) ? nvr.Device.Devices[Convert.ToUInt16(deviceId)] : null;
                }
                else
                {
                    handle.Device = null;
                }
            }

            eventHandle.Add(handle);
        }

        private void TriggerDigitalOutHandleFromXml(XmlElement node, List<EventHandle> eventHandle, ICMS cms)
        {
            //<DigitalOut>
            //    <NVR>1</NVR> for CMS
            //    <Device>1</Device>
            //    <DigitalOutId>2</DigitalOutId>
            //    <Status>0</Status>
            //</DigitalOut>

            String deviceId = Xml.GetFirstElementValueByTagName(node, "Device");
            var handle = new TriggerDigitalOutEventHandle
            {
                Device = new BasicDevice { Id = Convert.ToUInt16(deviceId) },
                DigitalOutId = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(node, "DigitalOutId")),
                DigitalOutStatus = (Xml.GetFirstElementValueByTagName(node, "Status") == "1"),
                ReadyState = ReadyState.Ready,
            };

            if (cms != null)
            {
                String nvrId = Xml.GetFirstElementValueByTagName(node, "NVR");
                var nvr = cms.NVRManager.FindNVRById(Convert.ToUInt16(nvrId));
                if (nvr != null)
                {
                    handle.Device = nvr.Device.Devices.ContainsKey(Convert.ToUInt16(deviceId)) ? nvr.Device.Devices[Convert.ToUInt16(deviceId)] : null;
                }
                else
                {
                    handle.Device = null;
                }
            }
            eventHandle.Add(handle);
        }

        private static void SendMailHandleFromXml(XmlElement node, List<EventHandle> eventHandle, ICMS cms)
        {
            //<SendMail>
            //  <Recipient>egg032978@yahoo.com.tw</Recipient> 
            //  <Subject>Device: 15 ACTi ACM5001 - Event: Motion 1</Subject> 
            //  <Body>Device: 15 ACTi ACM5001 Channel ID: 1 Event: Motion 1 Server: GNR2000</Body> 
            //  <Attach>false</Attach> 
            //  <AttachNVRSource>1</AttachNVRSource> 
            //  <AttachSource>3</AttachSource> 
            //</SendMail>

            String deviceId = Xml.GetFirstElementValueByTagName(node, "AttachSource");
            var handle = new SendMailEventHandle
            {
                MailReceiver = Xml.GetFirstElementValueByTagName(node, "Recipient"),
                Subject = "",//Xml.GetFirstElementsValueByTagName(node, "Subject"),
                Body = "",//Xml.GetFirstElementsValueByTagName(node, "Body"),
                AttachFile = (Xml.GetFirstElementValueByTagName(node, "Attach") == "true"),
                Device = (deviceId != "") ? new BasicDevice { Id = Convert.ToUInt16(deviceId) } : null,
                ReadyState = ReadyState.Ready,
            };

            if(cms != null)
            {
                String nvrId = Xml.GetFirstElementValueByTagName(node, "AttachNVRSource");
                var nvr = cms.NVRManager.FindNVRById(Convert.ToUInt16(nvrId));
                if(nvr != null)
                {
                    handle.Device = nvr.Device.Devices.ContainsKey(Convert.ToUInt16(deviceId)) ? nvr.Device.Devices[Convert.ToUInt16(deviceId)] : null;
                }
                else
                {
                    handle.Device = null;
                }
            }

            eventHandle.Add(handle);
        }

        private static void UploadFtpHandleFromXml(XmlElement node, List<EventHandle> eventHandle, ICMS cms)
        {
            //<UploadFTP>
            //  <AttachNVRSource>1</AttachNVRSource> 
            //  <AttachSource>3</AttachSource> 
            //  <FileName>14 ACTi ACM5001</FileName> 
            //  <TimeStamp>true</TimeStamp> 
            //</UploadFTP>

            String deviceId = Xml.GetFirstElementValueByTagName(node, "AttachSource");
            var handle = new UploadFtpEventHandle
            {
                Device = new BasicDevice { Id = Convert.ToUInt16(deviceId) },
                FileName = Xml.GetFirstElementValueByTagName(node, "FileName"),
                TimeStamp = (Xml.GetFirstElementValueByTagName(node, "TimeStamp") == "true"),
                ReadyState = ReadyState.Ready,
            };

            if (cms != null)
            {
                String nvrId = Xml.GetFirstElementValueByTagName(node, "AttachNVRSource");
                var nvr = cms.NVRManager.FindNVRById(Convert.ToUInt16(nvrId));
                if (nvr != null)
                {
                    handle.Device = nvr.Device.Devices.ContainsKey(Convert.ToUInt16(deviceId)) ? nvr.Device.Devices[Convert.ToUInt16(deviceId)] : null;
                }
                else
                {
                    handle.Device = null;
                }
            }

            eventHandle.Add(handle);
        }

        private void SaveEvent()
        {
            //Save all
            var xmlDoc = new XmlDocument();
            var xmlRoot = xmlDoc.CreateElement("AllEventhandler");
            xmlDoc.AppendChild(xmlRoot);

            if(Server is ICMS)
            {
                var cms = Server as ICMS;
                foreach (KeyValuePair<UInt16, INVR> nvr in cms.NVRManager.NVRs)
                {
                    var nvrNode = xmlDoc.CreateElement("NVR");
                    nvrNode.SetAttribute("id", nvr.Key.ToString());
                    var allDeviceNode = xmlDoc.CreateElement("AllDevices");
                    foreach (KeyValuePair<ushort, IDevice> device in nvr.Value.Device.Devices)
                    {
                        var camera = device.Value as ICamera;
                        if (camera == null) continue;
                        if (camera.EventHandling == null) continue;

                        if (camera.EventHandling.ReadyState == ReadyState.Ready) continue;
                        camera.EventHandling.ReadyState = ReadyState.Ready;
                        var deviceNode = ParseEventHandleToXml(device.Value);
                        if (deviceNode.FirstChild != null)
                            allDeviceNode.AppendChild(xmlDoc.ImportNode(deviceNode.FirstChild, true));
                    }
                    nvrNode.AppendChild(allDeviceNode);
                    xmlRoot.AppendChild(nvrNode);
                }

                if (Xml.GetFirstElementByTagName(xmlDoc, "EventHandlerConfiguration") != null)
                    Xml.PostXmlToHttp(CgiSaveAllEvent, xmlDoc, Server.Credential);
            }
            else
            {
                foreach (KeyValuePair<UInt16, IDevice> obj in Devices)
                {
                    var camera = obj.Value as ICamera;
                    if (camera == null) continue;
                    if (camera.EventHandling.ReadyState == ReadyState.Ready) continue;
                    camera.EventHandling.ReadyState = ReadyState.Ready;

                    var deviceNode = ParseEventHandleToXml(obj.Value);
                    if (deviceNode.FirstChild != null)
                        xmlRoot.AppendChild(xmlDoc.ImportNode(deviceNode.FirstChild, true));
                }

                if (xmlRoot.ChildNodes.Count > 0)
                    Xml.PostXmlToHttp(CgiSaveAllEvent, xmlDoc, Server.Credential);
            }
        }

        private XmlDocument ParseEventHandleToXml(IDevice device)
        {
            var xmlDoc = new XmlDocument();
            if (!(device is ICamera)) return xmlDoc;

            var camera = device as ICamera;
            var xmlRoot = xmlDoc.CreateElement("EventHandlerConfiguration");
            if(Server is ICMS)
            {
                var cms = Server as ICMS;
                if(!cms.NVRManager.DeviceChannelTable.ContainsKey(device)) return xmlDoc;
                xmlRoot.SetAttribute("id", cms.NVRManager.DeviceChannelTable[device].ToString());
            }
            else
            {
                xmlRoot.SetAttribute("id", device.Id.ToString());
            }
            xmlDoc.AppendChild(xmlRoot);

            if(Server is ICMS) 
                xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "DeviceId", device.Id.ToString()));
            //----------------------------------Schedule
            String weekSchedule = ConvertScheduleDataToWeekSchedule(camera.EventSchedule);

            XmlElement schedule = xmlDoc.CreateElement("Schedule");
            xmlRoot.AppendChild(schedule);
            schedule.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Mon", weekSchedule.Substring(0, 144)));
            schedule.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Tue", weekSchedule.Substring(144, 144)));
            schedule.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Wed", weekSchedule.Substring(288, 144)));
            schedule.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Thu", weekSchedule.Substring(432, 144)));
            schedule.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Fri", weekSchedule.Substring(576, 144)));
            schedule.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Sat", weekSchedule.Substring(720, 144)));
            schedule.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Sun", weekSchedule.Substring(864, 144)));

            //----------------------------------Event
            foreach (var obj in camera.EventHandling)
            {
                var eventNode = xmlDoc.CreateElement("Event");
                xmlRoot.AppendChild(eventNode);

                eventNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Description", obj.Key.CameraEvent.ToString()));

                var conditionsNode = xmlDoc.CreateElement("Conditions");
                conditionsNode.SetAttribute("operation", "and");
                conditionsNode.SetAttribute("dwell", "60");
                conditionsNode.SetAttribute("interval", obj.Key.Interval.ToString());
                conditionsNode.AppendChild(ParseConditionToXml(xmlDoc, obj.Key));
                eventNode.AppendChild(conditionsNode);

                //Handle
                List<EventHandle> eventHandles = obj.Value;
                foreach (var eventHandle in eventHandles)
                {
                    switch (eventHandle.Type)
                    {
                        case HandleType.Beep:
                            ParseBeepHandleToXml(xmlDoc, (BeepEventHandle)eventHandle, eventNode);
                            break;

                        case HandleType.Audio:
                            ParseAudioHandleToXml(xmlDoc, (AudioEventHandle)eventHandle, eventNode);
                            break;

                        case HandleType.ExecCmd:
                            ParseExceHandleToXml(xmlDoc, (ExecEventHandle)eventHandle, eventNode);
                            break;

                        case HandleType.HotSpot:
                            ParseHotSpotHandleToXml(xmlDoc, (HotSpotEventHandle)eventHandle, eventNode);
                            break;

                        case HandleType.GoToPreset:
                            ParseGotoPresetHandleToXml(xmlDoc, (GotoPresetEventHandle)eventHandle, eventNode);
                            break;

                        case HandleType.PopupPlayback:
                            PopupPlaybackHandleToXml(xmlDoc, (PopupPlaybackEventHandle)eventHandle, eventNode);
                            break;

                        case HandleType.PopupLive:
                            PopupLiveHandleToXml(xmlDoc, (PopupLiveEventHandle)eventHandle, eventNode);
                            break;

                        case HandleType.TriggerDigitalOut:
                            TriggerDigitalOutHandleToXml(xmlDoc, (TriggerDigitalOutEventHandle)eventHandle, eventNode);
                            break;

                        case HandleType.SendMail:
                            SendMailHandleToXml(xmlDoc, (SendMailEventHandle)eventHandle, obj.Key, camera, eventNode);
                            break;

                        case HandleType.UploadFtp:
                            UploadFtpHandleToXml(xmlDoc, (UploadFtpEventHandle)eventHandle, eventNode);
                            break;
                    }
                    eventHandle.ReadyState = ReadyState.Ready;
                }
            }

            return xmlDoc;
        }

        private static void ParseBeepHandleToXml(XmlDocument xmlDoc, BeepEventHandle handle, XmlElement eventNode)
        {
            //<Beep>
            //    <Times>2</Times>
            //    <Duration>2</Duration>
            //    <Interval>2</Interval>
            //</Beep>
            var beepNode = xmlDoc.CreateElement("Beep");

            beepNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Times", handle.Times.ToString()));
            beepNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Duration", handle.Duration.ToString()));
            beepNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Interval", handle.Interval.ToString()));

            eventNode.AppendChild(beepNode);
        }

        private static void ParseAudioHandleToXml(XmlDocument xmlDoc, AudioEventHandle handle, XmlElement eventNode)
        {
            //<Audio>
            //    <FileName>C:\\1.wav</FileName>
            //</Audio>
            var audioNode = xmlDoc.CreateElement("Audio");

            audioNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "FileName", handle.FileName));

            eventNode.AppendChild(audioNode);
        }

        private static void ParseExceHandleToXml(XmlDocument xmlDoc, ExecEventHandle handle, XmlElement eventNode)
        {
            //<ExecCmd>
            //    <FileName>C:\\1.wav</FileName>
            //</ExecCmd>
            var execNode = xmlDoc.CreateElement("ExecCmd");

            execNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "FileName", handle.FileName));

            eventNode.AppendChild(execNode);
        }

        private static void ParseGotoPresetHandleToXml(XmlDocument xmlDoc, GotoPresetEventHandle handle, XmlElement eventNode)
        {
            //<GoToPreset>
            //    <NVR>1</NVR> 
            //    <Device>1</Device>
            //    <Point>2</Point>
            //</GoToPreset>
            if (handle.Device == null || handle.PresetPoint == 0) return;

            var gotoNode = xmlDoc.CreateElement("GoToPreset");
            gotoNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "NVR", handle.Device.Server.Id.ToString()));
            gotoNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Device", handle.Device.Id.ToString()));
            gotoNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Point", handle.PresetPoint.ToString()));

            eventNode.AppendChild(gotoNode);
        }

        private void PopupPlaybackHandleToXml(XmlDocument xmlDoc, PopupPlaybackEventHandle handle, XmlElement eventNode)
        {
            //<Popup>
            //    <NVR>1</NVR> 
            //    <Device>1</Device>
            //</Popup>
            if (handle.Device == null) return;

            var popupNode = xmlDoc.CreateElement("Popup");

            if (Server is ICMS)
                popupNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "NVR", handle.Device.Server.Id.ToString()));
            popupNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Device", handle.Device.Id.ToString()));

            eventNode.AppendChild(popupNode);
        }

        private void PopupLiveHandleToXml(XmlDocument xmlDoc, PopupLiveEventHandle handle, XmlElement eventNode)
        {
            //<PopupLive>
            //    <NVR>1</NVR> 
            //    <Device>1</Device>
            //</PopupLive>
            if (handle.Device == null) return;

            var popupNode = xmlDoc.CreateElement("PopupLive");

            if (Server is ICMS)
                popupNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "NVR", handle.Device.Server.Id.ToString()));
            popupNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Device", handle.Device.Id.ToString()));

            eventNode.AppendChild(popupNode);
        }
        
        private void ParseHotSpotHandleToXml(XmlDocument xmlDoc, HotSpotEventHandle handle, XmlElement eventNode)
        {
            //<HotSpot>
            //    <NVR>1</NVR> 
            //    <Device>1</Device>
            //</HotSpot>
            if (handle.Device == null) return;

            var hotspotNode = xmlDoc.CreateElement("HotSpot");

            if (Server is ICMS)
                hotspotNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "NVR", handle.Device.Server.Id.ToString()));
            hotspotNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Device", handle.Device.Id.ToString()));

            eventNode.AppendChild(hotspotNode);
        }

        private void TriggerDigitalOutHandleToXml(XmlDocument xmlDoc, TriggerDigitalOutEventHandle handle, XmlElement eventNode)
        {
            //<DigitalOut>
            //    <NVR>1</NVR>
            //    <Device>1</Device>
            //    <DigitalOutId>2</DigitalOutId>
            //    <Status>0</Status>
            //</DigitalOut>
            if (handle.Device == null) return;

            var doNode = xmlDoc.CreateElement("DigitalOut");

            if(Server is ICMS)
                doNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "NVR", handle.Device.Server.Id.ToString()));

            doNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Device", handle.Device.Id.ToString()));
            doNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "DigitalOutId", handle.DigitalOutId.ToString()));
            doNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Status", (handle.DigitalOutStatus) ? "1" : "0"));

            eventNode.AppendChild(doNode);
        }

        private void SendMailHandleToXml(XmlDocument xmlDoc, SendMailEventHandle handle, EventCondition condition, ICamera camera, XmlElement eventNode)
        {
            //<SendMail>
            //  <Recipient>egg032978@yahoo.com.tw</Recipient> 
            //  <Subject>Device: 15 ACTi ACM5001 - Event: Motion 1</Subject> 
            //  <Body>Device: 15 ACTi ACM5001 Channel ID: 1 Event: Motion 1 Server: GNR2000</Body> 
            //  <Attach>false</Attach> 
            //  <AttachNVRSource>1</AttachNVRSource> 
            //  <AttachSource>3</AttachSource> 
            //</SendMail>
            var mailNode = xmlDoc.CreateElement("SendMail");

            String subject = camera + " - " + condition.CameraEvent.ToLocalizationString();
            String body = camera + " - " + condition.CameraEvent.ToLocalizationString() + " (Server " + Server.Credential.Domain + ")";

            mailNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Recipient", handle.MailReceiver));
            mailNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Subject", subject));//handle.Subject
            mailNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Body", body)); // handle.Body
            mailNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Attach", (handle.AttachFile) ? "true" : "false"));
            if(Server is ICMS)
                mailNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "AttachNVRSource", (handle.Device != null) ? handle.Device.Server.Id.ToString() : ""));
            mailNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "AttachSource", (handle.Device != null) ? handle.Device.Id.ToString() : ""));
            eventNode.AppendChild(mailNode);
        }

        private void UploadFtpHandleToXml(XmlDocument xmlDoc, UploadFtpEventHandle handle, XmlElement eventNode)
        {
            //<UploadFTP>
            //  <AttachNVRSource>1</AttachNVRSource> 
            //  <AttachSource>3</AttachSource> 
            //  <FileName>14 ACTi ACM5001</FileName> 
            //  <TimeStamp>true</TimeStamp> 
            //</UploadFTP>
            if (handle.Device == null) return;

            var ftpNode = xmlDoc.CreateElement("UploadFTP");
            if (Server is ICMS)
                ftpNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "AttachNVRSource", handle.Device.Server.Id.ToString()));
            ftpNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "AttachSource", handle.Device.Id.ToString()));
            ftpNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "FileName", handle.FileName));
            ftpNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "TimeStamp", (handle.TimeStamp) ? "true" : "false"));

            eventNode.AppendChild(ftpNode);
        }

        private static XmlElement ParseConditionToXml(XmlDocument xmlDoc, EventCondition eventCondition)
        {
            var conditionNode = xmlDoc.CreateElement("Condition");

            conditionNode.SetAttribute("type", eventCondition.CameraEvent.Type.ToString());

            switch (eventCondition.CameraEvent.Type)
            {
                case EventType.Motion:
                    conditionNode.SetAttribute("id", eventCondition.CameraEvent.Id.ToString());
                    conditionNode.SetAttribute("value", "1");
                    break;

                case EventType.DigitalInput:
                case EventType.ObjectCountingIn:
                case EventType.ObjectCountingOut:
                    //case EventType.DigitalOutput:
                    conditionNode.SetAttribute("id", eventCondition.CameraEvent.Id.ToString());
                    conditionNode.SetAttribute("value", (eventCondition.CameraEvent.Value) ? "1" : "0");
                    break;

                case EventType.NetworkLoss:
                case EventType.NetworkRecovery:
                case EventType.VideoLoss:
                case EventType.VideoRecovery:
                case EventType.RecordFailed:
                case EventType.RecordRecovery:
                case EventType.UserDefine:
                case EventType.Panic:
                case EventType.CrossLine:
                case EventType.IntrusionDetection:
                case EventType.LoiteringDetection:
                case EventType.AudioDetection:
                case EventType.TamperDetection:
                    conditionNode.SetAttribute("id", "1");
                    conditionNode.SetAttribute("value", (eventCondition.CameraEvent.Value) ? "1" : "0");
                    break;
            }

            conditionNode.SetAttribute("trigger", "1"); //fixed 1
            conditionNode.SetAttribute("interval", eventCondition.Interval.ToString());

            return conditionNode;
        }

        private void ConvertEventDeviceIdToDevice()
        {
            foreach (KeyValuePair<UInt16, IDevice> device in Devices)
            {
                if (!(device.Value is ICamera)) continue;

                ICamera camera = device.Value as ICamera;

                foreach (KeyValuePair<EventCondition, List<EventHandle>> handle in camera.EventHandling)
                {
                    if (handle.Value.Count == 0) continue;

                    for (Int32 i = 0; i < handle.Value.Count; i++)
                    {
                        EventHandle eventHandle = handle.Value[i];

                        switch (eventHandle.Type)
                        {
                            case HandleType.GoToPreset:
                                var gotoHandle = eventHandle as GotoPresetEventHandle;
                                if (gotoHandle == null) continue;

                                gotoHandle.Device = FindDeviceById(gotoHandle.Device.Id);
                                if (gotoHandle.Device == null || !(gotoHandle.Device is ICamera))
                                {
                                    handle.Value.Remove(eventHandle);
                                    i--;
                                    continue;
                                }

                                if (!((ICamera)gotoHandle.Device).Model.IsSupportPTZ)
                                {
                                    handle.Value.Remove(eventHandle);
                                    i--;
                                    continue;
                                }

                                if (!((ICamera)gotoHandle.Device).PresetPoints.Keys.Contains(gotoHandle.PresetPoint))
                                {
                                    handle.Value.Remove(eventHandle);
                                    i--;
                                    continue;
                                }
                                break;

                            case HandleType.PopupPlayback:
                                var popHandle = eventHandle as PopupPlaybackEventHandle;
                                if (popHandle == null) continue;

                                popHandle.Device = FindDeviceById(popHandle.Device.Id);
                                if (popHandle.Device == null)
                                {
                                    handle.Value.Remove(eventHandle);
                                    i--;
                                }
                                break;

                            case HandleType.PopupLive:
                                var popLiveHandle = eventHandle as PopupLiveEventHandle;
                                if (popLiveHandle == null) continue;

                                popLiveHandle.Device = FindDeviceById(popLiveHandle.Device.Id);
                                if (popLiveHandle.Device == null)
                                {
                                    handle.Value.Remove(eventHandle);
                                    i--;
                                }
                                break;

                            case HandleType.HotSpot:
                                var hotHandle = eventHandle as HotSpotEventHandle;
                                if (hotHandle == null) continue;

                                hotHandle.Device = FindDeviceById(hotHandle.Device.Id);
                                if (hotHandle.Device == null)
                                {
                                    handle.Value.Remove(eventHandle);
                                    i--;
                                }
                                break;

                            case HandleType.TriggerDigitalOut:
                                var doHandle = eventHandle as TriggerDigitalOutEventHandle;
                                if (doHandle == null) continue;

                                doHandle.Device = FindDeviceById(doHandle.Device.Id);
                                if (doHandle.Device == null || !(doHandle.Device is ICamera))
                                {
                                    handle.Value.Remove(eventHandle);
                                    i--;
                                    continue;
                                }

                                if (((ICamera)doHandle.Device).Model.NumberOfDo < doHandle.DigitalOutId)
                                {
                                    handle.Value.Remove(eventHandle);
                                    i--;
                                }
                                break;

                            case HandleType.SendMail:
                                var mailHandle = eventHandle as SendMailEventHandle;
                                if (mailHandle == null) continue;
                                if (!mailHandle.AttachFile || mailHandle.Device == null) continue;
                                mailHandle.Device = FindDeviceById(mailHandle.Device.Id);
                                if (mailHandle.Device == null && mailHandle.AttachFile)
                                {
                                    handle.Value.Remove(eventHandle);
                                    i--;
                                }
                                break;

                            case HandleType.UploadFtp:
                                var ftpHandle = eventHandle as UploadFtpEventHandle;
                                if (ftpHandle == null) continue;

                                ftpHandle.Device = FindDeviceById(ftpHandle.Device.Id);
                                if (ftpHandle.Device == null)
                                {
                                    handle.Value.Remove(eventHandle);
                                    i--;
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}