using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Constant;
using DownloadCaseForm;
using Interface;
using PanelBase;

namespace App_POSTransactionServer
{
    public class DownloadCaseForm
    {
        public Dictionary<String, String> Localization;
        private readonly DownloadCase _downloadCase = new DownloadCase();
        private readonly DownloadCaseQueue _downloadCaseQueue = new DownloadCaseQueue();

        public IServer Server
        {
            set
            {
                _downloadCase.Server = value;
                _downloadCase.Icon = value.Form.Icon;

                _downloadCaseQueue.Server = value;
                _downloadCaseQueue.Icon = value.Form.Icon;
            }
        }
        public IApp App;

        public XmlDocument AttachXmlDoc
        {
            set { _downloadCase.AttachXmlDoc = value; }
        }

        public Boolean IsDownloading { get { return _downloadCase.IsDownloading; } }

        public DownloadCaseForm()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"MessageBox_Information", "Information"},
                                   
                                   {"DownloadCase_AddDeviceBeforeDownload", "Select device before export case."},
                                   {"DownloadCase_DownloadCaseInProgress", "Export case already in progress.\r\nPlease complete or cancel previous operation."},
                                   {"ExportVideo_NoPermissionToExport", "You don't have permission to export the following devices video."},
                               };
            Localizations.Update(Localization);

            _downloadCase.OnDownloadCaseAddToQueue += DownloadCaseOnDownloadCaseAddToQueue;
        }

        public void StopDownload()
        {
            _downloadCase.Hide();
            _downloadCase.StopDownload();
        }

        public void DownloadCase(IDevice[] usingDevices, DateTime start, DateTime end)
        {
            _downloadCase.AttachXmlDoc = null;
            
            if (_downloadCase.Visible)
            {
                TopMostMessageBox.Show(Localization["DownloadCase_DownloadCaseInProgress"], Localization["MessageBox_Information"],
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);

                _downloadCase.Show();
                _downloadCase.BringToFront();

                return;
            }
            
            if (usingDevices == null)
            {
                TopMostMessageBox.Show(Localization["DownloadCase_AddDeviceBeforeDownload"], Localization["MessageBox_Information"],
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!_downloadCase.IsDownloading)
            {
                _downloadCase.ExportDevices.Clear();

                var noPermission = new List<String>();
                
                foreach (IDevice device in usingDevices)
                {
                    if (device != null)
                    {
                        //User have no permission to setting, so why check permission ==> user can access playback , mean he has download permission
                        //if (!device.CheckPermission(Permission.ExportVideo))
                        //{
                        //    noPermission.Add(device.ToString());
                        //    continue;
                        //}

                        if (device is ICamera)
                            _downloadCase.ExportDevices.Enqueue((ICamera)device);
                    }
                }

                if (noPermission.Count > 0)
                {
                    DialogResult result = TopMostMessageBox.Show(Localization["ExportVideo_NoPermissionToExport"] +
                                    Environment.NewLine + @"""" + String.Join(",", noPermission.ToArray()) + @"""", Localization["MessageBox_Information"],
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    if (result == DialogResult.Cancel) return;
                    if (_downloadCase.ExportDevices.Count == 0) return;
                }

                if (_downloadCase.ExportDevices.Count == 0)
                {
                    TopMostMessageBox.Show(Localization["DownloadCase_AddDeviceBeforeDownload"], Localization["MessageBox_Information"],
                                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _downloadCase.StartDateTime = start;

                _downloadCase.EndDateTime = end;

                _downloadCase.Initialize();
            }
            _downloadCase.ClientSize = new Size(_downloadCase.ClientSize.Width, 509);
            _downloadCase.Show();
            _downloadCase.BringToFront();
        }

        private void DownloadCaseOnDownloadCaseAddToQueue(Object sender, EventArgs<DownloadCaseConfig> e)
        {
            _downloadCaseQueue.DownloadCaseConfigs.Add(e.Value);

            ShowDownloadCaseQueue();
        }

        public void ShowDownloadCaseQueue()
        {
            _downloadCaseQueue.RefreshQueue();

            _downloadCaseQueue.Show();
            _downloadCaseQueue.BringToFront();
        }
    }
}
