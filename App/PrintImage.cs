using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using PrintImageForm;

namespace App
{
    public class PrintImageForm
    {
        public Dictionary<String, String> Localization;

        private PrintImage _printImageWindow;

        private PrintImage PrintImageWindow
        {
            get
            {
                if (_printImageWindow == null)
                {
                    _printImageWindow = CreatePrintImageWindow();
                    _printImageWindow.OnClosed += PrintImageFormOnClosed;
                }

                return _printImageWindow;
            }
        }

        public IApp App { get; set; }


        // Constructor
        public PrintImageForm()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},

								   {"PrintImage_AddDeviceBeforePrint", "Select device before print image."},
								   {"PrintImage_PrintImageInProgress", "Print image already in progress.\r\nPlease complete or cancel previous operation."},
								   {"PrintImage_NoPermissionToPrint", "You don't have permission to print the following devices image."},
							   };
            Localizations.Update(Localization);
        }


        protected virtual PrintImage CreatePrintImageWindow()
        {
            return new PrintImage();
        }

        public void PrintImage(IServer server, List<ICamera> printDevices, Dictionary<ICamera, Image> printImages, DateTime dateTime)
        {
            PrintImageWindow.App = App;
            PrintImageWindow.Server = server;
            PrintImageWindow.Icon = server.Form.Icon;

            if (PrintImageWindow.Visible)
            {
                TopMostMessageBox.Show(Localization["PrintImage_PrintImageInProgress"], Localization["MessageBox_Information"],
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);

                PrintImageWindow.Show();
                PrintImageWindow.BringToFront();

                return;
            }

            if (printDevices.Count == 0)
            {
                TopMostMessageBox.Show(Localization["PrintImage_AddDeviceBeforePrint"], Localization["MessageBox_Information"],
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            PrintImageWindow.PrintDevices.Clear();

            var noPermission = new List<String>();

            foreach (ICamera camera in printDevices)
            {
                if (camera == null) continue;

                if (camera is IDeviceLayout)
                {
                    var hasPermission = true;
                    foreach (var device in ((IDeviceLayout)camera).Items)
                    {
                        if (device == null) continue;

                        if (!device.CheckPermission(Permission.PrintImage))
                        {
                            hasPermission = false;
                            noPermission.Add(camera.ToString());
                            break;
                        }
                    }
                    if (hasPermission)
                        PrintImageWindow.PrintDevices.Add(camera);
                    continue;
                }

                if (camera is ISubLayout)
                {
                    var hasPermission = true;
                    foreach (var device in ((ISubLayout)camera).DeviceLayout.Items)
                    {
                        if (device == null) continue;

                        if (!device.CheckPermission(Permission.PrintImage))
                        {
                            hasPermission = false;
                            noPermission.Add(camera.ToString());
                            break;
                        }
                    }

                    if (hasPermission)
                        PrintImageWindow.PrintDevices.Add(camera);
                    continue;
                }

                if (!camera.CheckPermission(Permission.PrintImage))
                {
                    noPermission.Add(camera.ToString());
                    continue;
                }

                PrintImageWindow.PrintDevices.Add(camera);
            }

            if (noPermission.Count > 0)
            {
                DialogResult result = TopMostMessageBox.Show(Localization["PrintImage_NoPermissionToPrint"] +
                                Environment.NewLine + @"""" + String.Join(",", noPermission.ToArray()) + @"""", Localization["MessageBox_Information"],
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Cancel) return;
                if (PrintImageWindow.PrintDevices.Count == 0) return;
            }

            if (PrintImageWindow.PrintDevices.Count == 0) return;


            PrintImageWindow.DateTime = dateTime;
            PrintImageWindow.Initialize();

            foreach (ICamera camera in PrintImageWindow.PrintDevices)
            {
                if (camera == null) continue;

                if (printImages.ContainsKey(camera))
                    PrintImageWindow.PrintImages[camera] = printImages[camera];
            }

            //will take some time
            PrintImageWindow.LoadImage();


            // Add By Tulip for DVR Mode

            if (App.IsLock)
            {
                App.Form.TopMost = false;

                //do  AFTER imag loading completed
                PrintImageWindow.TopMost = true;
            }

            PrintImageWindow.Show();
            PrintImageWindow.BringToFront();

            //ApplicationForms.HideLoadingIcon();
        }

        private void PrintImageFormOnClosed(Object sender, EventArgs e)
        {
            if (!App.IsLock) return;

            // Add By Tulip for DVR Mode

            App.Form.TopMost = true;
            PrintImageWindow.TopMost = false;
        }
    }
}
