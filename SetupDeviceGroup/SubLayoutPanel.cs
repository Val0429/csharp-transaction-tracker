using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupDeviceGroup
{
    public sealed class SubLayoutPanel : Panel
    {
        public event EventHandler OnSubLayoutSetClick;
        public event EventHandler OnSubLayoutDoneClick;
        public event EventHandler OnSubLayoutDeleteClick;

        public INVR NVR;
        public Dictionary<String, String> Localization;

        public UInt16 Id;
        private ISubLayout _subLayout;
        public ISubLayout SubLayout
        {
            get { return _subLayout; }
            set
            {
                _subLayout = value;
                ParseSubLayout();
            }
        }
        public IDeviceLayout DeviceLayout;

        private Boolean _isTitle;
        public Boolean IsTitle
        {
            get { return _isTitle; }
            set
            {
                _isTitle = value;
                if (_isTitle)
                {
                    Controls.Remove(_nameTextBox);
                    Controls.Remove(_setButton);
                    Controls.Remove(_doneButton);
                    Controls.Remove(_deleteButton);
                }
            }
        }
        private readonly TextBox _nameTextBox;
        private readonly Button _setButton;
        private readonly Button _doneButton;
        private readonly Button _deleteButton;

        private static Image _set = Resources.GetResources(Properties.Resources.set, Properties.Resources.IMGSet);
        private static Image _done = Resources.GetResources(Properties.Resources.done, Properties.Resources.IMGDone);
        private static Image _delete = Resources.GetResources(Properties.Resources.delete, Properties.Resources.IMGDelete);

        public SubLayoutPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"DeviceLayoutPanel_ID", "ID"},
                                   {"DeviceLayoutPanel_Name", "Name"},
                                   {"DeviceLayoutPanel_X", "X"},
                                   {"DeviceLayoutPanel_Y", "Y"},
                                   {"DeviceLayoutPanel_Width", "Width"},
                                   {"DeviceLayoutPanel_Height", "Height"},
                                   {"DeviceLayoutPanel_Set", "Set"},
                                   {"DeviceLayoutPanel_Done", "Done"},
                                   {"DeviceLayoutPanel_Delete", "Delete"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Default;
            Size = new Size(300, 40);

            BackColor = Color.Transparent;

            _nameTextBox = new PanelBase.HotKeyTextBox
            {
                MaxLength = 50,
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Location = new Point(44, 10),
                Size = new Size(200, 22),
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0)
            };

            _setButton = new Button
            {
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Location = new Point(260, 7),
                Size = new Size(50, 25),
                Cursor = Cursors.Hand,
                //Text = Localization["DeviceLayoutPanel_Set"],
                BackgroundImageLayout = ImageLayout.Center,
                BackgroundImage = _set
            };
            SharedToolTips.SharedToolTip.SetToolTip(_setButton, Localization["DeviceLayoutPanel_Set"]);

            _doneButton = new Button
            {
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Location = new Point(350, 7),
                Size = new Size(50, 25),
                Cursor = Cursors.Hand,
                //Text = Localization["DeviceLayoutPanel_Done"],
                BackgroundImageLayout = ImageLayout.Center,
                BackgroundImage = _done
            };
            SharedToolTips.SharedToolTip.SetToolTip(_doneButton, Localization["DeviceLayoutPanel_Done"]);


            _deleteButton = new Button
            {
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Location = new Point(440, 7),
                Size = new Size(50, 25),
                Cursor = Cursors.Hand,
                //Text = Localization["DeviceLayoutPanel_Delete"],
                BackgroundImageLayout = ImageLayout.Center,
                BackgroundImage = _delete
            };
            SharedToolTips.SharedToolTip.SetToolTip(_deleteButton, Localization["DeviceLayoutPanel_Delete"]);

            Controls.Add(_nameTextBox);
            Controls.Add(_setButton);
            Controls.Add(_doneButton);
            Controls.Add(_deleteButton);

            _nameTextBox.TextChanged += NameTextBoxTextChanged;
            _setButton.MouseClick += SetButtonMouseClick;
            _doneButton.MouseClick += DoneButtonMouseClick;
            _deleteButton.MouseClick += DeleteButtonMouseClick;

            Paint += SubLayoutPanelPaint;
        }

        private Boolean _isEdit;
        private void ParseSubLayout()
        {
            _isEdit = false;
            _setButton.Enabled = true;
            _doneButton.Enabled = false;
            if (SubLayout != null)
            {
                _nameTextBox.Text = SubLayout.Name;
                _nameTextBox.Enabled = true;
                _deleteButton.Enabled = true;
            }
            else
            {
                _nameTextBox.Text = null;
                _nameTextBox.Enabled = false;
                _deleteButton.Enabled = false;
            }

            Invalidate();

            _isEdit = true;
        }

        private void SetButtonMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnSubLayoutSetClick != null)
                OnSubLayoutSetClick(this, null);

            _doneButton.Enabled = true;
        }

        private void DoneButtonMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnSubLayoutDoneClick != null)
                OnSubLayoutDoneClick(this, null);

            _doneButton.Enabled = false;
        }

        private void DeleteButtonMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnSubLayoutDeleteClick != null)
                OnSubLayoutDeleteClick(this, null);
        }

        private void NameTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!_isEdit || SubLayout == null || SubLayout.Name == _nameTextBox.Text) return;

            SubLayout.Name = _nameTextBox.Text;

            NVR.SubLayoutModify(SubLayout);
        }

        private void PaintTitle(Graphics g)
        {
            if (Width <= 300) return;
            Manager.PaintTitleText(g, Localization["DeviceLayoutPanel_Name"]);
            
            if (Width <= 390) return;
            g.DrawString(Localization["DeviceLayoutPanel_Set"], Manager.Font, Manager.TitleTextColor, 260, 13);
            
            if (Width <= 480) return;
            g.DrawString(Localization["DeviceLayoutPanel_Done"], Manager.Font, Manager.TitleTextColor, 350, 13);
            
            if (Width <= 570) return;
            g.DrawString(Localization["DeviceLayoutPanel_Delete"], Manager.Font, Manager.TitleTextColor, 440, 13);
            
            if (Width <= 590) return;
            g.DrawString(Localization["DeviceLayoutPanel_X"], Manager.Font, Manager.TitleTextColor, 540, 13);
            
            if (Width <= 640) return;
            g.DrawString(Localization["DeviceLayoutPanel_Y"], Manager.Font, Manager.TitleTextColor, 590, 13);
            
            if (Width <= 700) return;
            g.DrawString(Localization["DeviceLayoutPanel_Width"], Manager.Font, Manager.TitleTextColor, 650, 13);
            
            if (Width <= 760) return;
            g.DrawString(Localization["DeviceLayoutPanel_Height"], Manager.Font, Manager.TitleTextColor, 710, 13);
        }

        private void SubLayoutPanelPaint(Object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;

            if (Parent == null) return;

            Graphics g = e.Graphics;

            if (IsTitle)
            {
                Manager.PaintTitleTopInput(g, this);
                PaintTitle(g);
                return;
            }

            Manager.Paint(g, control);

            if (SubLayout == null) return;

            //if (Width <= 300) return;
            //Manager.PaintText(g, SubLayout.Name);

            if (Width <= 590) return;
            g.DrawString(SubLayout.X.ToString(), Manager.Font, Brushes.Black, 540, 13);

            if (Width <= 660) return;
            g.DrawString(SubLayout.Y.ToString(), Manager.Font, Brushes.Black, 590, 13);

            if (Width <= 700) return;
            g.DrawString(SubLayout.Width.ToString(), Manager.Font, Brushes.Black, 650, 13);

            if (Width <= 760) return;
            g.DrawString(SubLayout.Height.ToString(), Manager.Font, Brushes.Black, 710, 13);
        }
    }
}
