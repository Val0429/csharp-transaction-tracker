using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using Point = System.Drawing.Point;

namespace PeopleCounting
{
    public partial class PeopleCountingPanel : UserControl, IControl, IDrop
    {
        public event EventHandler OnVerify;
        public event EventHandler OnVerifyStop;
        public String TitleName { get; set; }
        public Dictionary<String, String> Localization;
        private const Int32 RectangleCountLimit = 1;
        private const String CgiLoadFeature = @"cgi-bin/sysconfig?action=loadfeature";
        private const String CgiSaveFeature = @"cgi-bin/sysconfig?action=savefeature";
        public IVAS VAS;
        private ICamera _camera;
        public ICamera Camera
        {
            get { return _camera; }
            set
            {
                peopleCountingControl.DeleteSetting();
                _camera = value;
                if (_camera == null)
                {
                    peopleCountingControl.DeleteSetting();
                    return;
                }
                //peopleCountingControl.Visibility = Visibility.Visible;
                GetCameraSnapshot(false);
            }
        }

        private Image _displaySnapshot = null;
        private void GetCameraSnapshot(Boolean isRefresh)
        {
            if (Camera == null) return;
            UInt16 count = 0;
            while (count < 2)
            {
                _displaySnapshot = _camera.GetSnapshot();
                count++;
                if (_displaySnapshot != null)
                {
                    if (isRefresh)
                    {
                        peopleCountingControl.RefreshByImage(_displaySnapshot);
                        //peopleCountingControl.DrawSettingByPeopleCountingList();
                    }
                    else
                    {
                        peopleCountingControl.CreatePhotoByImage(_displaySnapshot);
                    }
                    return;
                }
            }
            TopMostMessageBox.Show(Localization["PeopleCounting_MessageBoxDragSnapshotFail"], Localization["MessageBox_Information"]);
        }

        private List<PeopleCountingRectangle> _rectangles;
        public List<PeopleCountingRectangle> Rectangles
        {
            get { return _rectangles; }
            set
            {
                _rectangles = value;
                peopleCountingControl.Rectangles = value;
                peopleCountingControl.DrawSettingByPeopleCountingList();
            }
        }

        [DllImport("iMakePeopleCounterFeature.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr iPPLOpenInterface();
        [DllImport("iMakePeopleCounterFeature.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void iPPLCloseInterface(IntPtr h);
        [DllImport("iMakePeopleCounterFeature.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void iPPLAlgMakeFeature(IntPtr h, byte[] resultImage, byte[] pbyInImage, int nImageSize, int nImageWidth, int nImageHeight);

        public PeopleCountingPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                    {"MessageBox_Confirm", "Confirm"},
                                    {"MessageBox_Information", "Information"},
                                    {"PeopleCounting_MessageBoxClearRegions", "Do you want to clear region(s)?"},
                                    {"PeopleCounting_MessageBoxRemoveRegion", "Do you want to delete this region?"},
                                    {"PeopleCounting_MessageBoxDeleteSetting", "Do you want to delete this setting?"},
                                    {"PeopleCounting_MessageBoxDragSnapshotFail", "Get snapshot fail. Please try again."},
                                    {"PeopleCounting_MessageBoxRefreshSnapshotFail", "Refresh snapshot fail. Please try again."},
                                    {"PeopleCounting_MessageBoxClearFeature","Do you want to clear feature?"},
                                    {"PeopleCounting_ButtonDelete", "Delete"},
                                    {"PeopleCounting_ButtonRefresh", "Refresh Snapshot"},
                                    {"PeopleCounting_ButtonDrawing", "Draw Region"},
                                    {"PeopleCounting_ButtonClear", "Clear"},
                                    {"PeopleCounting_ButtonRemove", "Remove"},
                                    {"PeopleCounting_ButtonMakeFeature", "Make Feature"},
                                    {"PeopleCounting_ButtonClearFeature", "Delete Features"},
                                    {"PeopleCounting_ButtonSaveMakingFeature", "Save  Feature"},
                                    {"PeopleCounting_ButtonCancelMakingFeature", "Cancel"},
                                    {"PeopleCounting_ButtonRotateLine", "Rotate Line"},
                                    {"PeopleCounting_ButtonStartVerify", "Start Verify"},
                                    {"PeopleCounting_ButtonStopVerify", "Stop Verify"},
                                    {"PeopleCounting_ButtonSwitchPeopleInOut", "Switch People In/Out"},
                                    {"PeopleCounting_LabelPeopleIn", "People In"},
                                    {"PeopleCounting_LabelPeopleOut", "People Out"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            Dock = DockStyle.Fill;

            peopleCountingControl.RectangleLimitCount = RectangleCountLimit;
            peopleCountingControl.OnEnabledSnapshotButtons += OnEnabledSnapshotButtons;
            peopleCountingControl.OnEnabledClearButtons += OnEnabledClearButtons;
            peopleCountingControl.OnEnabledRegionControlButtons += OnEnabledRegionControlButtons;
            peopleCountingControl.OnEnabledDrawingButton += OnEnabledDrawinglButton;
            peopleCountingControl.OnEnabledPeopleDescriptionLabel += OnEnabledPeopleDescriptionLabel;
            peopleCountingControl.OnEnabledVerifyButton += OnEnabledVerifyButton;
            peopleCountingControl.OnSaveFeatureRetangle += OnSaveFeatureRetangle;
            peopleCountingControl.OnEnabledFeatureButton += OnEnabledFeatureButtons;

            buttonRefreshImage.Text = Localization["PeopleCounting_ButtonRefresh"];
            buttonDrawing.Text = Localization["PeopleCounting_ButtonDrawing"];
            buttonClear.Text = Localization["PeopleCounting_ButtonClear"];
            buttonRemove.Text = Localization["PeopleCounting_ButtonRemove"];
            buttonRotate.Text = Localization["PeopleCounting_ButtonRotateLine"];
            buttonSwitch.Text = Localization["PeopleCounting_ButtonSwitchPeopleInOut"];
            labelPeopleIn.Text = Localization["PeopleCounting_LabelPeopleIn"];
            labelPeopleOut.Text = Localization["PeopleCounting_LabelPeopleOut"];
            buttonVerify.Text = Localization["PeopleCounting_ButtonStartVerify"];
            buttonMakeFeature.Text = Localization["PeopleCounting_ButtonMakeFeature"];
            buttonCancelMakingFeature.Text = Localization["PeopleCounting_ButtonCancelMakingFeature"];
            buttonDeleteFeature.Text = Localization["PeopleCounting_ButtonClearFeature"];

            pictureBoxPeopleInArrow.BackgroundImage = Resources.GetResources(Properties.Resources.arrowPeopleIn, Properties.Resources.IMGPeopleIn);
            pictureBoxPeopleOutArrow.BackgroundImage = Resources.GetResources(Properties.Resources.arrowPeopleOut, Properties.Resources.IMGPeopleOut);

            peopleCountingControl.IsVerified = _isVerified = false;
        }

        private Int32 _featureCount = 1;
        private void OnSaveFeatureRetangle(Object sender, EventArgs<Rectangle> e)
        {
            if (_displaySnapshot == null) return;

            Rectangle feature = e.Value;
            feature.Width = Math.Max(feature.Width, 80);
            feature.Height = Math.Max(feature.Height, 80);

            Bitmap image = new Bitmap(_displaySnapshot);
            Byte[] imageBytes = Converter.ImageToByte2(image);

            IntPtr handle = iPPLOpenInterface();

            int width = image.Width / 8;
            int height = image.Height / 8;
            Byte[] result = new Byte[width * height * 4];

            iPPLAlgMakeFeature(handle, result, imageBytes, imageBytes.Length, image.Width, image.Height);

            iPPLCloseInterface(handle);

            var features = Xml.LoadTextFromHttp(CgiLoadFeature, VAS.Credential);
            if (!String.IsNullOrEmpty(features) && features != " ")
            {
                var array = features.Split('\n');
                try
                {
                    if (array.Length > 0)
                        _featureCount = Convert.ToUInt16(array[array.Length - 1].Split(',')[0]) + 1;
                }
                catch (Exception)
                {
                }
            }
            else
            {
                features = "";
                _featureCount = 1;
            }

            UInt16 featureX = Convert.ToUInt16(Math.Floor(feature.X / 8.0));
            UInt16 featureY = Convert.ToUInt16(Math.Floor(feature.Y / 8.0));
            UInt16 featureWidth = Convert.ToUInt16(Math.Ceiling(feature.Width / 8.0));
            UInt16 featureHeight = Convert.ToUInt16(Math.Ceiling(feature.Height / 8.0));

            List<String> featureValue = new List<String>();

            featureValue.Add(_featureCount.ToString());
            featureValue.Add("1");
            featureValue.Add(featureWidth.ToString());
            featureValue.Add(featureHeight.ToString());
            var resultCrop = new List<Byte>();

            for (var y = 0; y < height; y++)
            {
                if (y < featureY || y >= (featureHeight + featureY)) continue;

                for (var x = 0; x < width; x++)
                {
                    if (x < featureX || x >= (featureWidth + featureX)) continue;

                    var value = result[(y * width + x) * 4];
                    resultCrop.Add(value);
                    resultCrop.Add(value);
                    resultCrop.Add(value);
                    resultCrop.Add(0);
                    featureValue.Add(value.ToString());
                }
            }

            SaveCropImage(result, resultCrop.ToArray(), image, featureWidth, featureHeight);

            var featureStr = String.Join(", ", featureValue.ToArray()) + ",";
            if (String.IsNullOrEmpty(features))
                Xml.PostTextToHttp(CgiSaveFeature, featureStr, VAS.Credential);
            else
                Xml.PostTextToHttp(CgiSaveFeature, features + Environment.NewLine + featureStr, VAS.Credential);

            _featureCount++;
        }

        private void SaveCropImage(Byte[] result, Byte[] resultCrop, Bitmap image, UInt16 featureWidth, UInt16 featureHeight)
        {
            Directory.CreateDirectory("ivs_image");
            var resultImage = Converter.ImageFromArray(result, image.Width / 8, image.Height / 8);
            resultImage.Save("ivs_image/" + _featureCount + ".bmp");

            var resultImage2 = Converter.ImageFromArray(resultCrop, featureWidth, featureHeight);
            resultImage2.Save("ivs_image/" + _featureCount + "_crop.bmp");
        }

        private void OnCancleFeatureRetangle()
        {
            _featureCount = 1;
            Xml.PostTextToHttp(CgiSaveFeature, " ", VAS.Credential);
            if (!Directory.Exists("ivs_image")) return;
            try
            {
                var images = Directory.GetFiles("ivs_image");
                foreach (var image in images)
                {
                    File.Delete(image);
                }
            }
            catch (Exception)
            {
            }
        }

        private void OnEnabledVerifyButton(Object sender, EventArgs<Boolean> e)
        {
            if (_isMakingFeature) return;
            if(buttonVerify.Visible == e.Value)
                return;

            buttonVerify.Visible = e.Value;

            RefreshButtonOrder();
        }

        private void OnEnabledPeopleDescriptionLabel(Object sender, EventArgs<Boolean> e)
        {
            if (pictureBoxPeopleOutArrow.Visible == e.Value && pictureBoxPeopleInArrow.Visible == e.Value && labelPeopleIn.Visible == e.Value && labelPeopleOut.Visible == e.Value)
                return;

            pictureBoxPeopleOutArrow.Visible = pictureBoxPeopleInArrow.Visible = labelPeopleIn.Visible = labelPeopleOut.Visible = e.Value;

            RefreshButtonOrder();
        }

        private void OnEnabledDrawinglButton(Object sender, EventArgs<Boolean> e)
        {
            buttonDrawing.Enabled = e.Value;
        }

        private void OnEnabledRegionControlButtons(Object sender, EventArgs<Boolean> e)
        {
            if (_isMakingFeature) return;
            buttonRemove.Enabled = buttonRotate.Enabled = buttonSwitch.Enabled = e.Value;

            if (buttonRemove.Visible == e.Value && buttonRotate.Visible == e.Value && buttonSwitch.Visible == e.Value)
                return;

            buttonRemove.Visible = buttonRotate.Visible = buttonSwitch.Visible = e.Value;

            RefreshButtonOrder();
        }

        private void OnEnabledClearButtons(Object sender, EventArgs<Boolean> e)
        {
            if (_isMakingFeature) return;
            if (RectangleCountLimit > 1)
            {
                buttonClear.Enabled = e.Value;

                if (buttonClear.Visible == e.Value)
                    return;

                buttonClear.Visible = e.Value;

                RefreshButtonOrder();
            }
        }

        private void OnEnabledSnapshotButtons(Object sender, EventArgs<Boolean> e)
        {
            separatedLabel2.Visible = buttonRefreshImage.Visible = e.Value;
            if (_isMakingFeature) return;
            buttonDrawing.Enabled = e.Value;

            if (buttonDrawing.Visible == e.Value)
                return;

            buttonDrawing.Visible = e.Value;

            RefreshButtonOrder();
        }

        private void OnEnabledFeatureButtons(object sender, EventArgs<Boolean> e)
        {
            buttonMakeFeature.Enabled = buttonCancelMakingFeature.Enabled = e.Value;
        }

        public void Initialize()
        {
        }

        public void UpdateSnapshot(Image snapshot)
        {
            peopleCountingControl.RefreshByImage(snapshot);
        }

        public void Activate()
        {
            peopleCountingControl.IsVerified = _isVerified = false;
        }

        public void Deactivate()
        {
            peopleCountingControl.IsVerified = _isVerified = false;
            buttonVerify.Text = Localization["PeopleCounting_ButtonStartVerify"];
            if (_isMakingFeature) CancelMakingFeatureClick(this, null);
        }

        public Boolean CheckDragDataType(Object dragObj)
        {
            return (dragObj is INVR || dragObj is IDeviceGroup || dragObj is IDevice);
        }

        public void DragStop(Point point, EventArgs<Object> e)
        {
            if (!Drag.IsDrop(this, point)) return;
            Object dragObj = e.Value;

            if (dragObj is ICamera)
            {
                Camera = dragObj as ICamera;
            }
        }

        public void DragMove(MouseEventArgs e)
        {
        }

        public void UpdatePeopleCountingNumber()
        {
            peopleCountingControl.DrawSettingByPeopleCountingList();
        }

        private void DrawingClick(Object sender, EventArgs e)
        {
            peopleCountingControl.StartDrawingRectangle();
        }

        private void RotateClick(Object sender, EventArgs e)
        {
            peopleCountingControl.ChangeLineDirection();
        }

        private void SwitchClick(Object sender, EventArgs e)
        {
            peopleCountingControl.SwitchArrowAttribute();
        }

        private void ClearClick(Object sender, EventArgs e)
        {
            if(Camera == null) return;
            DialogResult result = TopMostMessageBox.Show(Localization["PeopleCounting_MessageBoxClearRegions"], Localization["MessageBox_Confirm"],
                                             MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if(result == DialogResult.OK)
            {
                peopleCountingControl.ClearRegions();
            }
        }

        private void RemoveClick(Object sender, EventArgs e)
        {
            if (Camera == null) return;
            DialogResult result = TopMostMessageBox.Show(Localization["PeopleCounting_MessageBoxRemoveRegion"], Localization["MessageBox_Confirm"],
                                             MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                peopleCountingControl.RemoveRegion();
            }
        }

        private void SaveClick(Object sender, EventArgs e)
        {
            if (Camera == null) return;

            GetCameraSnapshot(false);
            peopleCountingControl.DrawSettingByPeopleCountingList();
        }

        private void ButtonRefreshImageClick(Object sender, EventArgs e)
        {
            if (Camera == null) return;

            panelFunction.Enabled = false;
            GetCameraSnapshot(true);
            panelFunction.Enabled = true;
        }

        private Boolean _isVerified;

        private void VerifyClick(Object sender, EventArgs e)
        {
            peopleCountingControl.IsVerified = _isVerified = !_isVerified;

            if (_isVerified)
            {
                //peopleCountingControl.Visibility = Visibility.Hidden;
                buttonVerify.Text = Localization["PeopleCounting_ButtonStopVerify"];
                if (OnVerify != null)
                    OnVerify(this, new EventArgs());
            }
            else
            {
                //peopleCountingControl.Visibility = Visibility.Visible;
                buttonVerify.Text = Localization["PeopleCounting_ButtonStartVerify"];
                if (OnVerifyStop != null)
                    OnVerifyStop(this, new EventArgs());
            }

            peopleCountingControl.SwitchVerifyMode();
            buttonRotate.Enabled = buttonSwitch.Enabled = buttonRemove.Enabled = 
            buttonMakeFeature.Visible = separatedLabel.Visible = buttonRefreshImage.Visible = separatedLabel2.Visible = (!_isVerified);

            RefreshButtonOrder();
        }

        private Boolean _isMakingFeature;

        private void MakeFeatureClick(Object sender, EventArgs e)
        {
            _isMakingFeature = !_isMakingFeature;
            buttonMakeFeature.Text = !_isMakingFeature ? Localization["PeopleCounting_ButtonMakeFeature"] : Localization["PeopleCounting_ButtonSaveMakingFeature"];
            if (_isMakingFeature)
            {
                peopleCountingControl.StartDrawingFeatureRectangle();

                panelDesc.Visible = buttonDrawing.Visible = buttonVerify.Visible = buttonRemove.Visible = buttonRotate.Visible = buttonSwitch.Visible = false;
                buttonCancelMakingFeature.Visible = buttonDeleteFeature.Visible = true;
                buttonRefreshImage.Visible = separatedLabel2.Visible = separatedLabel.Visible = false;
                
            }
            else
            {
                buttonCancelMakingFeature.Visible = buttonDeleteFeature.Visible = false;
                peopleCountingControl.SaveDrawingFeatureRectangle();
                peopleCountingControl.StopDrawingFeatureRectangle();
                buttonRefreshImage.Visible = separatedLabel2.Visible = separatedLabel.Visible = true;
            }

            RefreshButtonOrder();
        }

        private void CancelMakingFeatureClick(Object sender, EventArgs e)
        {
            _isMakingFeature = false;
            peopleCountingControl.StopDrawingFeatureRectangle();
            buttonMakeFeature.Text = Localization["PeopleCounting_ButtonMakeFeature"];
           // panelDesc.Visible = buttonDrawing.Visible = buttonVerify.Visible = buttonRemove.Visible = buttonRotate.Visible = buttonSwitch.Visible =  true;
            buttonCancelMakingFeature.Visible = buttonDeleteFeature.Visible = false;
            separatedLabel.Visible = true;

            RefreshButtonOrder();
        }

        private void ClearFeatureClick(object sender, EventArgs e)
        {
            DialogResult result = TopMostMessageBox.Show(Localization["PeopleCounting_MessageBoxClearFeature"], Localization["MessageBox_Confirm"],
                                             MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                OnCancleFeatureRetangle();
            }
        }

        private void RefreshButtonOrder()
        {
            panelFunction.Controls.Add(buttonMakeFeature);
            panelFunction.Controls.Add(buttonDeleteFeature);
            panelFunction.Controls.Add(buttonCancelMakingFeature);
            panelFunction.Controls.Add(separatedLabel);
            panelFunction.Controls.Add(buttonDrawing);
            panelFunction.Controls.Add(buttonRemove);
            panelFunction.Controls.Add(buttonClear);
            panelFunction.Controls.Add(buttonRotate);
            panelFunction.Controls.Add(buttonVerify);
            panelFunction.Controls.Add(buttonSave);
            panelFunction.Controls.Add(buttonSwitch);
            panelFunction.Controls.Add(panelDesc);
            panelFunction.Controls.Add(separatedLabel2);
            panelFunction.Controls.Add(buttonRefreshImage);
        }
    }
}
