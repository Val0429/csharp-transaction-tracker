using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Constant;
using DeviceConstant;
using Interface;

namespace VideoMonitor
{
    public partial class VideoMonitor
    {
        private static readonly ILogger _logger = LoggerManager.Instance.GetLogger();

        protected virtual IVideoMenu ToolMenu { get; set; }

        public virtual void InitializeMenu()
        {
            if (ToolMenu == null)
            {
                ToolMenu = CreateVideoMenu(windowsPanel.Location, App.Pages.ContainsKey("Playback"), Server);
            }

            var toolMenu = ToolMenu as Control;
            if (toolMenu != null)
            {
                Controls.Add(toolMenu);
                toolMenu.BringToFront();
            }

            ToolMenu.OnButtonClick += ToolManuButtonClick;
        }

        protected virtual IVideoMenu CreateVideoMenu(Point panelPoint, bool hasPlaybackPage, IServer server)
        {
            return new VideoMenu
            {
                PanelPoint = panelPoint,
                HasPlaybackPage = hasPlaybackPage,
                Server = server,
                App = App
            };
        }

        private Dictionary<UInt16, List<String>> _audioOutDevices = new Dictionary<UInt16, List<String>>();
        public virtual void BroadcastStart(Object sender, EventArgs e)
        {
            var visibleDevices = (from videoWindow in VideoWindows
                                  where (videoWindow.Visible && videoWindow.Camera != null && videoWindow.Camera.CheckPermission(Permission.AudioOut))
                                  select videoWindow.Camera).ToList();

            if (visibleDevices.Count == 0) return;

            var identifyNames = new List<String>();
            _audioOutDevices.Clear();
            foreach (ICamera camera in visibleDevices)
            {
                //camera.IsAudioOut = true;
                if (CMS != null)
                {
                    if (_audioOutDevices.ContainsKey(camera.Server.Id))
                    {
                        if (!_audioOutDevices[camera.Server.Id].Contains(camera.Id.ToString()))
                            _audioOutDevices[camera.Server.Id].Add(camera.Id.ToString());
                    }
                    else
                    {
                        _audioOutDevices.Add(camera.Server.Id, new List<String> { camera.Id.ToString() });
                    }
                }
                else
                {
                    identifyNames.Add("channel" + camera.Id);
                }
            }

            if (CMS != null)
            {
                foreach (KeyValuePair<UInt16, List<String>> audioOutDevice in _audioOutDevices)
                {
                    identifyNames.Add(String.Format("nvr{0}:{1}", audioOutDevice.Key, String.Join(",", audioOutDevice.Value.ToArray())));
                }
            }

            Int32 result = CMS != null ? CMS.Utility.StartAudioTransfer(String.Join(";", identifyNames.ToArray())) : Server.Utility.StartAudioTransfer(String.Join(",", identifyNames.ToArray()));

            if (result == 0)
            {
                foreach (ICamera camera in visibleDevices)
                {
                    camera.IsAudioOut = false;
                }
            }

            if (ActivateVideoWindow != null)
                ActivateVideoWindow.ToolMenu.CheckStatus();

        }

        public virtual void BroadcastStop(Object sender, EventArgs e)
        {
            var visibleDevices = (from videoWindow in VideoWindows
                                  where (videoWindow.Visible && videoWindow.Camera != null && videoWindow.Camera.CheckPermission(Permission.AudioOut))
                                  select videoWindow.Camera).ToList();

            foreach (ICamera camera in visibleDevices)
            {
                camera.IsAudioOut = false;
            }
            Server.Utility.StopAudioTransfer();

            if (ActivateVideoWindow != null)
                ActivateVideoWindow.ToolMenu.CheckStatus();
            //Console.WriteLine("BroadcastStop");
        }

        protected static String PTZChangeXml(String status)
        {
            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("XML");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Status", status));

            return xmlDoc.InnerXml;
        }

        protected virtual void ToolManuButtonClick(Object sender, EventArgs<String> e)
        {
            if (ActivateVideoWindow == null) return;

            var xmlDoc = Xml.LoadXml(e.Value);
            var button = Xml.GetFirstElementValueByTagName(xmlDoc, "Button");
            var status = Xml.GetFirstElementValueByTagName(xmlDoc, "Status");

            OnMenuButtonCommand(button, status);
        }

        protected virtual void OnMenuButtonCommand(string command, string status)
        {
            switch (command)
            {
                case "Disconnect":
                    try
                    {
                        if (ActivateVideoWindow.Viewer.Camera != null)
                        {
                            Server.WriteOperationLog("Remove Device %1".Replace("%1", ActivateVideoWindow.Viewer.Camera.Id.ToString()));
                        }

                        DisconnectWindow(ActivateVideoWindow);

                        ActivateVideoWindow = null;

                        if (OnSelectionChange != null)
                            OnSelectionChange(this, new EventArgs<ICamera, PTZMode>(null, PTZMode.Disable));

                        RaiseOnContentChange(ToArray());

                        var deviceCount = (Devices == null) ? 0 : Devices.Count;
                        var layoutCount = (WindowLayout == null) ? 0 : WindowLayout.Count;

                        foreach (var popupVideoMonitor in UsingPopupVideoMonitor)
                        {
                            deviceCount += popupVideoMonitor.DeviceCount;
                            layoutCount += popupVideoMonitor.LayoutCount;
                        }

                        var count = Math.Min(layoutCount, deviceCount);

                        if (OnViewingDeviceNumberChange != null)
                            OnViewingDeviceNumberChange(this, new EventArgs<Int32>(count));

                        if (OnViewingDeviceListChange != null)
                        {
                            OnViewingDeviceListChange(this, new EventArgs<List<IDevice>>(ReadViewDeviceList()));
                        }

                        if (OnDiscountVideoWindowByMenu != null)
                            OnDiscountVideoWindowByMenu(this, null);

                        if (WindowLayout != null)
                        {
                            AutoChangeProfileMode();

                            if (Server.Configure.EnableAutoSwitchDecodeIFrame)
                            {
                                //When Layout.Count >= 16 , Auto Switch to Decode-I Frame
                                if (count >= Server.Configure.AutoSwitchDecodeIFrameCount)
                                    DecodeIframe();
                                else //When Layout.Count < 16 , Auto Switch to autoDrop Frame
                                    AutoDropFrame();
                            }
                            //keep current decode setting
                            //else
                            //{
                            //    AutoDropFrame();
                            //}

                            if (count > 0)
                            {
                                UpdateBitrateTimer.Enabled = true;
                            }
                            else
                            {
                                UpdateBitrateTimer.Enabled = false;

                                var layout = new List<WindowLayout>(WindowLayout.ToArray());
                                windowsPanel.Visible = false;
                                ClearLayout();
                                SetLayout(layout);
                                windowsPanel.Visible = true;

                                if (OnBitrateUsageChange != null)
                                    OnBitrateUsageChange(this, new EventArgs<String>("N/A"));
                            }
                        }
                        ReadPopupWindowsAndSave();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                    }
                    break;

                case "Playback":
                    if (ActivateVideoWindow.Camera != null && ActivateVideoWindow.Viewer != null && ActivateVideoWindow.Track != null)
                    {
                        App.SwitchPage("Playback", new PlaybackParameter
                        {
                            Device = ActivateVideoWindow.Camera,
                            Timecode = DateTimes.ToUtc(ActivateVideoWindow.Track.DateTime, Server.Server.TimeZone),
                            TimeUnit = ActivateVideoWindow.Track.UnitTime
                        });
                    }
                    break;

                case "Optical PTZ":
                    //if (status == "Activate")
                    //    ActivateVideoWindow.Viewer.PtzMode = PTZMode.Optical;
                    //else
                    //    ActivateVideoWindow.Viewer.PtzMode = PTZMode.Digital;

                    if (OnPTZModeChange != null)
                        OnPTZModeChange(this, new EventArgs<String>(PTZChangeXml(status)));
                    break;
            }
        }
    }
}
