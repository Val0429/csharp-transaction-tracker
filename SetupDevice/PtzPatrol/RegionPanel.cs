using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace SetupDevice
{
    public sealed class RegionPanel : Panel
    {
        public Dictionary<String, String> Localization;

        private readonly HotKeyTextBox _nameTextBox;
        private readonly PictureBox _gotoPictureBox;
        private readonly PictureBox _addPictureBox;
        private readonly PictureBox _deletePictureBox;

        public IServer Server;
        private Boolean _isTitle;
        public Boolean IsTitle {
            get { return _isTitle; }
            set
            {
                _isTitle = value;
                if (_isTitle)
                {
                    _nameTextBox.Visible = _gotoPictureBox.Visible = _addPictureBox.Visible = _deletePictureBox.Visible = false;
                }
            }
        }
        public ICamera Camera;
        public IVideoWindow VideoWindow;
        public UInt16 PointId;
        public Boolean IsEditing;

        private static Image _add = Resources.GetResources(Properties.Resources.add, Properties.Resources.IMGAdd);
        private static Image _delete = Resources.GetResources(Properties.Resources.delete, Properties.Resources.IMGDelete);
        private static Image _goto = Resources.GetResources(Properties.Resources._goto, Properties.Resources.IMGGoto);

        public RegionPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"PointPanel_ID", "ID"},
                                   {"PointPanel_Name", "Name"},
                                   {"PointPanel_Add", "Add"},
                                   {"PointPanel_Delete", "Delete"},
                                   {"PointPanel_GoTo", "Go to"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Default;
            Height = 40;
            BackColor = Color.Transparent;

            _addPictureBox = new PictureBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Location = new Point(240, 8),
                Size = new Size(25, 25),
                Cursor = Cursors.Hand,
                BackgroundImage = _add,
                BackgroundImageLayout = ImageLayout.Stretch,
            };

            _deletePictureBox = new PictureBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Location = new Point(340, 8),
                Size = new Size(25, 25),
                Cursor = Cursors.Hand,
                BackgroundImage = _delete,
                BackgroundImageLayout = ImageLayout.Stretch,
            };

            _gotoPictureBox = new PictureBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Location = new Point(13, 8),
                Size = new Size(25, 25),
                Cursor = Cursors.Hand,
                BackgroundImage = _goto,
                BackgroundImageLayout = ImageLayout.Stretch,
            };

            _nameTextBox = new HotKeyTextBox
            {
                MaxLength = 30,
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Location = new Point(80, 10),
                Size = new Size(115, 22),
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0)
            };
            Controls.Add(_nameTextBox);
            Controls.Add(_gotoPictureBox);
            Controls.Add(_addPictureBox);
            Controls.Add(_deletePictureBox);

            //SharedToolTips.SharedToolTip.SetToolTip(_addPictureBox, Localization["PointPanel_Add"]);
            //SharedToolTips.SharedToolTip.SetToolTip(_deletePictureBox, Localization["PointPanel_Delete"]);
            //SharedToolTips.SharedToolTip.SetToolTip(_gotoPictureBox, Localization["PointPanel_GoTo"]);

            var toolTipAdd = new ToolTip { ShowAlways = true };
            toolTipAdd.SetToolTip(_addPictureBox, Localization["PointPanel_Add"]);

            var toolTipDelete = new ToolTip { ShowAlways = true };
            toolTipDelete.SetToolTip(_deletePictureBox, Localization["PointPanel_Delete"]);

            var toolTipGoto = new ToolTip { ShowAlways = true };
            toolTipGoto.SetToolTip(_gotoPictureBox, Localization["PointPanel_GoTo"]);

            _nameTextBox.TextChanged += NameTextBoxTextChanged;
            _addPictureBox.MouseClick += AddPictureBoxMouseClick;
            _deletePictureBox.MouseClick += DeletePictureBoxMouseClick;
            _gotoPictureBox.MouseClick += GotoPictureBoxMouseClick;
            Paint += PointPanelPaint;
        }
        
        public void ParseDevice()
        {
            IsEditing = false;

            if (Camera == null || !Camera.PatrolPoints.ContainsKey(PointId) || Camera.PatrolPoints[PointId] == null)
            {
                _nameTextBox.Text = "";
                _deletePictureBox.Visible = _gotoPictureBox.Visible = false;
            }
            else
            {
                _nameTextBox.Text = Camera.PatrolPoints[PointId].Name;
                _deletePictureBox.Visible = _gotoPictureBox.Visible = true;
            }

            IsEditing = true;
        }

        private void GotoPictureBoxMouseClick(Object sender, MouseEventArgs e)
        {
            if (!IsEditing || Camera == null || !Camera.PatrolPoints.ContainsKey(PointId) || Camera.PatrolPoints[PointId] == null) return;

            VideoWindow.Viewer.SetDigitalPtzRegion(Camera.PatrolPoints[PointId].RegionXML.OuterXml);
        }

        private void AddPictureBoxMouseClick(Object sender, MouseEventArgs e)
        {
            if (Camera == null) return;

            Camera.AddPresetPoint(PointId);

            var pointName = _nameTextBox.Text;
            
            _deletePictureBox.Visible = _gotoPictureBox.Visible = true;

            var regionXml = VideoWindow.Viewer.GetDigitalPtzRegion();
            var windowPTZRegionLayout = new WindowPTZRegionLayout
                                            {
                                                RegionXML =
                                                    (XmlElement) Xml.LoadXml(regionXml).SelectSingleNode("/PTZRegions"),
                                                Id = (short) PointId,
                                                Name = _nameTextBox.Text
                                            };
            if (!Camera.PatrolPoints.ContainsKey(PointId))
                Camera.PatrolPoints.Add(PointId, windowPTZRegionLayout);
            else
                Camera.PatrolPoints[PointId] = windowPTZRegionLayout;

            if (String.IsNullOrEmpty(pointName))
            {
                _nameTextBox.Text = pointName = PointId.ToString();
            }

            Camera.ReadyState = ReadyState.Modify;
            Server.DeviceModify(Camera);
        }

        private void DeletePictureBoxMouseClick(Object sender, MouseEventArgs e)
        {
            if (Camera == null) return;

            _deletePictureBox.Visible = _gotoPictureBox.Visible = false;
            _nameTextBox.Text = "";
            Camera.PatrolPoints[PointId] = null;

            Camera.ReadyState = ReadyState.Modify;
            Server.DeviceModify(Camera);
        }

        private void NameTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing || Camera == null || !Camera.PatrolPoints.ContainsKey(PointId) || Camera.PatrolPoints[PointId] == null) return;

            Camera.PatrolPoints[PointId].Name = _nameTextBox.Text;

            Camera.ReadyState = ReadyState.Modify;
            Server.DeviceModify(Camera);
        }

        private void PointPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;

            if(IsTitle)
            {
                SetupBase.Manager.PaintTitleTopInput(g, this);
                PaintTitle(g);
                return;
            }

            SetupBase.Manager.Paint(g, this);

            if (Width <= 200) return;

            SetupBase.Manager.PaintText(g, PointId.ToString().PadLeft(2, '0'));
        }

        private void PaintTitle(Graphics g)
        {
            if (Width <= 200) return;
            SetupBase.Manager.PaintTitleText(g, Localization["PointPanel_ID"]);

            g.DrawString(Localization["PointPanel_Name"], SetupBase.Manager.Font, SetupBase.Manager.TitleTextColor, 74, 13);

            if (Width <= 340) return;
            g.DrawString(Localization["PointPanel_Add"], SetupBase.Manager.Font, SetupBase.Manager.TitleTextColor, 240, 13);
            
            if (Width <= 440) return;
            g.DrawString(Localization["PointPanel_Delete"], SetupBase.Manager.Font, SetupBase.Manager.TitleTextColor, 340, 13);
        }
    }
}
