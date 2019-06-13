using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;

namespace SetupDevice
{
    public sealed class PointPanel : Panel
    {
        public Dictionary<String, String> Localization;

        private readonly HotKeyTextBox _nameTextBox;
        private readonly PictureBox _gotoPictureBox;
        private readonly PictureBox _addPictureBox;
        private readonly PictureBox _deletePictureBox;
        private readonly ToolTip _toolTip = new ToolTip();

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
        public ICamera Device;
        public UInt16 PointId;
        public Boolean IsEditing;

        public PointPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"PointPanel_ID", "ID"},
                                   {"PointPanel_Name", "Name"},
                                   {"PointPanel_GoTo", "Go to"},
                                   {"PointPanel_Add", "Add"},
                                   {"PointPanel_Delete", "Delete"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Default;
            Height = 40;
            BackColor = Color.Transparent;

            _gotoPictureBox = new PictureBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Location = new Point(240, 8),
                Size = new Size(25, 25),
                Cursor = Cursors.Hand,
                BackgroundImage = Resources.GetResources(Properties.Resources._goto, Properties.Resources.IMGGoto),
                BackgroundImageLayout = ImageLayout.Stretch,
            };

            _addPictureBox = new PictureBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Location = new Point(340, 8),
                Size = new Size(25, 25),
                Cursor = Cursors.Hand,
                BackgroundImage = Resources.GetResources(Properties.Resources.add, Properties.Resources.IMGAdd),
                BackgroundImageLayout = ImageLayout.Stretch,
            };

            _deletePictureBox = new PictureBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Location = new Point(440, 8),
                Size = new Size(25, 25),
                Cursor = Cursors.Hand,
                BackgroundImage = Resources.GetResources(Properties.Resources.delete, Properties.Resources.IMGDelete),
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

            _toolTip.SetToolTip(_gotoPictureBox, Localization["PointPanel_GoTo"]);
            _toolTip.SetToolTip(_addPictureBox, Localization["PointPanel_Add"]);
            _toolTip.SetToolTip(_deletePictureBox, Localization["PointPanel_Delete"]);

            _nameTextBox.TextChanged += NameTextBoxTextChanged;
            _gotoPictureBox.MouseClick += GotoPictureBoxMouseClick;
            _addPictureBox.MouseClick += AddPictureBoxMouseClick;
            _deletePictureBox.MouseClick += DeletePictureBoxMouseClick;
            Paint += PointPanelPaint;
        }
        
        public void ParseDevice()
        {
            IsEditing = false;

            if (Device == null || !Device.PresetPoints.ContainsKey(PointId))
            {
                _nameTextBox.Text = "";
            }
            else
            {
                _nameTextBox.Text = Device.PresetPoints[PointId].Name;
            }

            IsEditing = true;
        }

        private void GotoPictureBoxMouseClick(Object sender, MouseEventArgs e)
        {
            if (Device == null || !Device.PresetPoints.ContainsKey(PointId)) return;

            Device.PresetPointGo = PointId;
        }

        private void AddPictureBoxMouseClick(Object sender, MouseEventArgs e)
        {
            if (Device == null) return;

            Device.AddPresetPoint(PointId);

            var pointName = _nameTextBox.Text;
            if (String.IsNullOrEmpty(pointName))
            {
                _nameTextBox.Text = pointName = PointId.ToString();
            }

            if (!Device.PresetPoints.ContainsKey(PointId))
                Device.PresetPoints.Add(PointId, new PresetPoint { Id = PointId, Name = pointName });
            else
                Device.PresetPoints[PointId].Name = pointName;
        }

        private void DeletePictureBoxMouseClick(Object sender, MouseEventArgs e)
        {
            if (Device == null) return;

            Device.DeletePresetPoint(PointId);
            Device.PresetPoints.Remove(PointId);
            _nameTextBox.Text = "";
        }

        private void NameTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing || Device == null || !Device.PresetPoints.ContainsKey(PointId)) return;

            Device.PresetPoints[PointId].Name = _nameTextBox.Text;
        }

        private void PointPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;
            
            Graphics g = e.Graphics;

            Manager.Paint(g, this);

            if(IsTitle)
            {
                PaintTitle(g);
                return;
            }

            if (Width <= 200) return;

            Manager.PaintText(g, PointId.ToString().PadLeft(2, '0'));
        }

        private void PaintTitle(Graphics g)
        {
            if (Width <= 200) return;

            Manager.PaintText(g, Localization["PointPanel_ID"]);
            g.DrawString(Localization["PointPanel_Name"], Manager.Font, Brushes.Black, 74, 13);

            if (Width <= 340) return;
            g.DrawString(Localization["PointPanel_GoTo"], Manager.Font, Brushes.Black, 240, 13);

            if (Width <= 440) return;
            g.DrawString(Localization["PointPanel_Add"], Manager.Font, Brushes.Black, 340, 13);
            
            if (Width <= 540) return;
            g.DrawString(Localization["PointPanel_Delete"], Manager.Font, Brushes.Black, 440, 13);
        }
    }
}
