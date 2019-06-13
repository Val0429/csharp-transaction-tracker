using Constant;
using Interface;
using Layout;
using PanelBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Constant.Utility;
using ApplicationForms = App.ApplicationForms;

namespace SetupNVR
{
    public partial class SetupForm : Form
    {
        public IApp App;
        public INVR NVR;
        public Dictionary<String, String> Localization;

        public SetupForm()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Common_UsedSeconds", "(%1 seconds)"},

								   {"MessageBox_Information", "Information"},
								   {"MessageBox_Error", "Error"},

								   {"Application_SaveCompleted", "Save Completed"},
								   {"Application_SaveTimeout", "Saving timeout. Some settings can't be saved to server."},
								   {"Application_NetworkVideoRecorder", "Network Video Recorder"},

								   {"SetupNVR_SaveSetup", "Save Setup"},
								   {"Application_StorageUsage", "Usage %1%    %2 GB used    %3 GB free"},
							   };

            Localizations.Update(Localization);

            InitializeComponent();

            saveButton.Text = Localization["SetupNVR_SaveSetup"];
            saveButton.Click += SaveButtonClick;

            savePanel.BackgroundImage = Resources.GetResources(Properties.Resources.banner, Properties.Resources.BGBanner);

            _tickDateTimeTimer.Elapsed += TickDateTimeTimer;
            _tickDateTimeTimer.Interval = 1000;

            Closing += ClosingSetupForm;
        }

        public void Reset()
        {
            if (NVR != null)
                NVR.Server.OnStorageStatusUpdate -= ServerOnStorageStatusUpdate;

            _tickDateTimeTimer.Enabled = false;
        }

        private Label _timeLabel;
        private Panel _storagePanel;
        private Panel _statePanel;
        private readonly System.Timers.Timer _tickDateTimeTimer = new System.Timers.Timer();

        private IPage _setupPage;
        public void Initialize()
        {
            if (NVR == null) return;

            _tickDateTimeTimer.SynchronizingObject = App.Form;
            if (!App.Pages.ContainsKey("Setup")) return;

            NVR.Form = this;
            _setupPage = new Page
            {
                App = App,
                Server = NVR,
                Name = "Setup",
                PageNode = App.Pages["Setup"].PageNode,
            };

            Text = NVR.Credential.UserName + @"@" + NVR.Credential.Domain + @":" + NVR.Credential.Port + @" - " + Localization["Application_NetworkVideoRecorder"];

            _statePanel = ApplicationForms.StatusPanel();

            _storagePanel = ApplicationForms.StoragePanel();

            _timeLabel = new DoubleBufferLabel
            {
                Dock = DockStyle.Right,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                AutoSize = true,
                Padding = new Padding(0, 0, 10, 0),
                MinimumSize = new Size(100, 20),
            };

            _timeLabel.Text = NVR.Server.Location + @" " + NVR.Server.DateTime.ToDateTimeString();
            _tickDateTimeTimer.Enabled = true;

            _statePanel.Controls.Add(_timeLabel);
            _statePanel.Controls.Add(_storagePanel);

            _setupPage.LoadConfig();
            _setupPage.Activate();
            Controls.Add(_statePanel);
            Controls.Add(_setupPage.Content);
            savePanel.SendToBack();

            Show();
            BringToFront();

            _storagePanel.Paint += StoragePanelPaint;
            NVR.Server.OnStorageStatusUpdate -= ServerOnStorageStatusUpdate;
            NVR.Server.OnStorageStatusUpdate += ServerOnStorageStatusUpdate;
        }

        public void UpdateServer()
        {
            if (NVR == null) return;
            var sameNVR = (_setupPage.Server == NVR);

            _setupPage.Server = NVR;

            foreach (var blockPanel in _setupPage.Layout.BlockPanels)
            {
                foreach (var controlPanel in blockPanel.ControlPanels)
                {
                    if (!sameNVR)
                    {
                        if (!(controlPanel.Control is IServerUse)) continue;
                        ((IServerUse)controlPanel.Control).Server = NVR;
                    }
                }
            }

            Text = NVR.Credential.UserName + @"@" + NVR.Credential.Domain + @":" + NVR.Credential.Port + @" - " + Localization["Application_NetworkVideoRecorder"];
            _timeLabel.Text = NVR.Server.Location + @" " + NVR.Server.DateTime.ToDateTimeString();
            _tickDateTimeTimer.Enabled = true;

            _setupPage.Activate();

            Show();
            BringToFront();

            NVR.Server.OnStorageStatusUpdate -= ServerOnStorageStatusUpdate;
            NVR.Server.OnStorageStatusUpdate += ServerOnStorageStatusUpdate;
        }

        public void ClosingSetupForm(Object sender, CancelEventArgs e)
        {
            Reset();
            NVR = null;
            Hide();
            //foreach (var blockPanel in _setupPage.Layout.BlockPanels)
            //{
            //    foreach (var controlPanel in blockPanel.ControlPanels)
            //    {
            //        ((Control)controlPanel.Control).Dispose();
            //        ((Control)controlPanel).Dispose();
            //    }
            //    ((Control)blockPanel).Dispose();
            //}
            //foreach (var controlPanel in _setupPage.Layout.Function)
            //{
            //    ((Control)controlPanel).Dispose();
            //}
            //foreach (Control control in Controls)
            //{
            //    control.Dispose();
            //}
            //_setupPage = null;
            //Controls.Clear();
            //Dispose(true);
            //Close();

            //GC.Collect();
            //GC.WaitForPendingFinalizers();

            //switch to page 1
            var flag = false;
            foreach (var blockPanel in _setupPage.Layout.BlockPanels)
            {
                foreach (var controlPanel in blockPanel.ControlPanels)
                {
                    if (!(controlPanel.Control is SetupBase.Icon) || controlPanel.Control.Name != "Server Icon")
                        continue;

                    flag = true;
                    ((SetupBase.Icon)controlPanel.Control).ActiveSetup();
                    break;
                }
                if (flag)
                    break;
            }

            //hide form, dont close it(re-use)
            e.Cancel = true;
        }

        public void ServerOnStorageStatusUpdate(Object sender, EventArgs e)
        {
            _storagePanel.Invalidate();
        }

        private void TickDateTimeTimer(Object sender, EventArgs e)
        {
            if (NVR == null) return;
            _timeLabel.Text = NVR.Server.Location + @" " + NVR.Server.DateTime.ToDateTimeString();
        }

        private static readonly Image _storageBg = StorageImage.StorageBg2();
        private static readonly Image _storageBar = StorageImage.StorageUsage();
        private static readonly Image _keep = StorageImage.StorageKeep();
        private readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);

        private const UInt32 Gb2Byte = 1073741824;

        private void StoragePanelPaint(Object sender, PaintEventArgs e)
        {
            if (NVR == null) return;
            if (NVR.Server.StorageInfo.Count == 0) return;

            Int64 totalSpace = 0;
            Int64 usedspace = 0;
            Int64 freespace = 0;
            Int64 keepSpace = 0;

            foreach (Storage storage in NVR.Server.Storage)
            {
                if (!NVR.Server.StorageInfo.ContainsKey(storage.Key)) continue;

                keepSpace += storage.KeepSpace;
                totalSpace += NVR.Server.StorageInfo[storage.Key].Total;
                usedspace += NVR.Server.StorageInfo[storage.Key].Used;
                freespace += NVR.Server.StorageInfo[storage.Key].Free;
            }

            if (totalSpace == 0) return;

            keepSpace *= 1073741824;
            Graphics g = e.Graphics;

            g.DrawImage(_storageBg, 0, 7, _storageBg.Width, _storageBg.Height);

            var userec = new Rectangle
            {
                X = 0,
                Y = 7,
                Width = Convert.ToUInt16(usedspace * _storageBg.Width / totalSpace),
                Height = _storageBar.Height
            };
            var usesrc = new Rectangle
            {
                X = 0,
                Y = 0,
                Width = userec.Width,
                Height = userec.Height
            };
            g.DrawImage(_storageBar, userec, usesrc, GraphicsUnit.Pixel);

            //-----------
            Int32 keepWidth = Convert.ToUInt16(Math.Round((keepSpace * _storageBg.Width) / (totalSpace * 1.0)));
            var keeprec = new Rectangle
            {
                X = 0 + _storageBg.Width - keepWidth,
                Y = 7,
                Width = keepWidth,
                Height = _keep.Height
            };
            var keepsrc = new Rectangle
            {
                X = _keep.Width - keepWidth,
                Y = 0,
                Width = keeprec.Width,
                Height = keeprec.Height
            };
            g.DrawImage(_keep, keeprec, keepsrc, GraphicsUnit.Pixel);

            g.DrawString(Localization["Application_StorageUsage"].Replace("%1", (usedspace * 100 / totalSpace).ToString("0"))
                .Replace("%2", Math.Floor(usedspace * 1.0 / Gb2Byte).ToString("N0")).Replace("%3", Math.Floor(freespace * 1.0 / Gb2Byte).ToString("N0")),
                _font, Brushes.Black, _storageBg.Width + 5, 4);
        }

        private Boolean _isNew;
        private Boolean _isModifly;
        private readonly Stopwatch _watch = new Stopwatch();
        private void SaveButtonClick(Object sender, EventArgs e)
        {
            _watch.Reset();
            _watch.Start();

            NVR.OnSaveFailure -= NvrOnSaveFailure;
            NVR.OnSaveFailure += NvrOnSaveFailure;
            NVR.OnSaveComplete -= NvrOnSaveComplete;
            NVR.OnSaveComplete += NvrOnSaveComplete;

            _isNew = (NVR.ReadyState == ReadyState.New);
            _isModifly = (NVR.ReadyState == ReadyState.Modify);
            NVR.Save();
        }

        private delegate void NvrOnSaveCompleteDelegate(Object sender, EventArgs<String> e);
        private void NvrOnSaveComplete(Object sender, EventArgs<String> e)
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new NvrOnSaveCompleteDelegate(NvrOnSaveComplete), sender, e);
                }
                catch (Exception)
                {
                }
                return;
            }

            NVR.OnSaveFailure -= NvrOnSaveFailure;
            NVR.OnSaveComplete -= NvrOnSaveComplete;

            _watch.Stop();
            if (NVR.Utility != null)
            {
                NVR.Utility.StopEventReceive();
                NVR.Utility.Server = NVR;
                NVR.Utility.StartEventReceive();
            }

            if (_isNew)
                NVR.ReadyState = ReadyState.New;
            else if (_isModifly)
                NVR.ReadyState = ReadyState.Modify;
            else
                NVR.ReadyState = NVR.ReadyState;// (_isNew ? ReadyState.New : ReadyState.Modify);

            NVR.Credential.Port = NVR.Server.Port;

            //new Form { TopMost = true, StartPosition = FormStartPosition.CenterScreen },
            TopMostMessageBox.Show("\"" + NVR + "\" " + Localization["Application_SaveCompleted"],
                //+ @" " + Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00")),
                Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

            ApplicationForms.HideProgressBar();
            //remove transparent panel before window is close(object will be clear)
            Application.RaiseIdle(null);

            Close();
        }

        private delegate void NvrOnSaveFailureDelegate(Object sender, EventArgs<String> e);
        private void NvrOnSaveFailure(Object sender, EventArgs<String> e)
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new NvrOnSaveFailureDelegate(NvrOnSaveFailure), sender, e);
                }
                catch (Exception)
                {
                }
                return;
            }

            NVR.OnSaveFailure -= NvrOnSaveFailure;
            NVR.OnSaveComplete -= NvrOnSaveComplete;

            _watch.Stop();

            //new Form { TopMost = true, StartPosition = FormStartPosition.CenterScreen },
            TopMostMessageBox.Show("\"" + NVR + "\" " + Localization["Application_SaveTimeout"] + Environment.NewLine + e.Value, Localization["MessageBox_Error"],
                MessageBoxButtons.OK, MessageBoxIcon.Stop);

            if (NVR.Utility != null)
            {
                NVR.Utility.Server = NVR;
                NVR.Utility.UpdateEventRecive();
            }

            NVR.ReadyState = ReadyState.Unavailable;

            ApplicationForms.HideProgressBar();
            //remove transparent panel before window is close(object will be clear)
            Application.RaiseIdle(null);

            Close();
        }
    }
}
