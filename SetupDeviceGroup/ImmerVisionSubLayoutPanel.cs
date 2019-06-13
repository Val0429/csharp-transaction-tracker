using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupDeviceGroup
{
    public sealed class ImmerVisionSubLayoutPanel : Panel
    {
        //public event EventHandler OnSubLayoutSetClick;
        public event EventHandler OnSubLayoutDoneClick;
        //public event EventHandler OnSubLayoutDeleteClick;

		//public event EventHandler OnSubLayoutFlipClick;
		//public event EventHandler OnSubLayoutMirrorClick;
		//public event EventHandler OnSubLayoutRotateRightClick;
		//public event EventHandler OnSubLayoutRotateLeftClick;

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
	                //Controls.Remove(_doneButton);
					Controls.Remove(_originalRadioButton);
	                Controls.Remove(_flipRadioButton);
	                Controls.Remove(_mirrorRadioButton);
					Controls.Remove(_flipmirrorRadioButton);
	                Controls.Remove(_rotateRightRadioButton);
	                Controls.Remove(_rotateLeftRadioButton);
                }
                else
                {
					Controls.Remove(_doneButton);
                }
            }
        }
        private readonly TextBox _nameTextBox;
        //private readonly Button _setButton;
        private readonly Button _doneButton;
        //private readonly Button _deleteButton;

		private readonly RadioButton _originalRadioButton;
		private readonly RadioButton _flipRadioButton;
		private readonly RadioButton _mirrorRadioButton;
		private readonly RadioButton _flipmirrorRadioButton;
		private readonly RadioButton _rotateRightRadioButton;
		private readonly RadioButton _rotateLeftRadioButton;

        private readonly ToolTip _toolTip = new ToolTip();
		public ImmerVisionSubLayoutPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"DeviceLayoutPanel_ID", "ID"},
                                   {"DeviceLayoutPanel_Name", "Name"},
                                   {"DeviceLayoutPanel_X", "X"},
                                   {"DeviceLayoutPanel_Y", "Y"},
                                   {"DeviceLayoutPanel_Width", "Width"},
                                   {"DeviceLayoutPanel_Height", "Height"},
                                   //{"DeviceLayoutPanel_Set", "Set"},
                                   {"DeviceLayoutPanel_Done", "Done"},
                                   //{"DeviceLayoutPanel_Delete", "Delete"},
								   {"DeviceLayoutPanel_Original", "Original"},
								   {"DeviceLayoutPanel_Flip", "Flip"},
								   {"DeviceLayoutPanel_Mirror", "Mirror"},
								   {"DeviceLayoutPanel_FlipAndMirror", "Flip And Mirror"},
								   {"DeviceLayoutPanel_RotateRight", "Rotate Right"},
								   {"DeviceLayoutPanel_RotateLeft", "Rotate Left"},
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

            _doneButton = new Button
            {
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Location = new Point(200, 7),
                Size = new Size(50, 25),
                Cursor = Cursors.Hand,
                //Text = Localization["DeviceLayoutPanel_Done"],
                BackgroundImageLayout = ImageLayout.Center,
                BackgroundImage = Resources.GetResources(Properties.Resources.done, Properties.Resources.IMGDone)
            };
            _toolTip.SetToolTip(_doneButton, Localization["DeviceLayoutPanel_Done"]);

			_originalRadioButton = new RadioButton
			{
				Anchor = AnchorStyles.Top | AnchorStyles.Left,
				Location = new Point(305, 7),
				Size = new Size(25, 25),
			};
			_toolTip.SetToolTip(_originalRadioButton, Localization["DeviceLayoutPanel_Original"]);

			_flipRadioButton = new RadioButton
			{
				Anchor = AnchorStyles.Top | AnchorStyles.Left,
				Location = new Point(385, 7),
				Size = new Size(25, 25),
			};
			_toolTip.SetToolTip(_flipRadioButton, Localization["DeviceLayoutPanel_Flip"]);

			_mirrorRadioButton = new RadioButton
			{
				Anchor = AnchorStyles.Top | AnchorStyles.Left,
				Location = new Point(460, 7),
				Size = new Size(25, 25),
			};
			_toolTip.SetToolTip(_mirrorRadioButton, Localization["DeviceLayoutPanel_Mirror"]);

			_flipmirrorRadioButton = new RadioButton
			{
				Anchor = AnchorStyles.Top | AnchorStyles.Left,
				Location = new Point(540, 7),
				Size = new Size(25, 25),
			};
			_toolTip.SetToolTip(_flipmirrorRadioButton, Localization["DeviceLayoutPanel_FlipAndMirror"]);

			_rotateRightRadioButton = new RadioButton
			{
				Anchor = AnchorStyles.Top | AnchorStyles.Left,
				Location = new Point(655, 7),
				Size = new Size(25, 25),
			};
			_toolTip.SetToolTip(_rotateRightRadioButton, Localization["DeviceLayoutPanel_RotateRight"]);

			_rotateLeftRadioButton = new RadioButton
			{
				Anchor = AnchorStyles.Top | AnchorStyles.Left,
				Location = new Point(755, 7),
				Size = new Size(25, 25),
			};
			_toolTip.SetToolTip(_rotateLeftRadioButton, Localization["DeviceLayoutPanel_RotateLeft"]);


            Controls.Add(_nameTextBox);
            Controls.Add(_doneButton);
			Controls.Add(_originalRadioButton);
			Controls.Add(_flipRadioButton);
			Controls.Add(_mirrorRadioButton);
			Controls.Add(_flipmirrorRadioButton);
			Controls.Add(_rotateRightRadioButton);
			Controls.Add(_rotateLeftRadioButton);

			_nameTextBox.KeyPress += _nameTextBox_KeyPress;
            _nameTextBox.TextChanged += NameTextBoxTextChanged;

			_doneButton.MouseClick -= DoneButtonMouseClick;
            _doneButton.MouseClick += DoneButtonMouseClick;

			_originalRadioButton.Click += OriginalRadioButtonClick;
			_flipRadioButton.Click += FlipRadioButtonClick;
			_mirrorRadioButton.Click += MirrorRadioButtonClick;
			_flipmirrorRadioButton.Click += FlipmirrorRadioButtonClick;
			_rotateRightRadioButton.Click += RotateRightRadioButtonClick;
			_rotateLeftRadioButton.Click += RotateLeftRadioButtonClick;

            Paint += SubLayoutPanelPaint;
        }

		void _nameTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (Char)37) { e.Handled = true; return; } 	//	%
			else if (e.KeyChar == (Char)42) { e.Handled = true; return; }	//	*
			else if (e.KeyChar == (Char)44) { e.Handled = true; return; }	//	,
			else if (e.KeyChar == (Char)47) { e.Handled = true; return; }	//	/ 
			else if (e.KeyChar == (Char)58) { e.Handled = true; return; }	//	:
			else if (e.KeyChar == (Char)92) { e.Handled = true; return; }	//	\
			else if (e.KeyChar == (Char)62) { e.Handled = true; return; }	//	>
			else if (e.KeyChar == (Char)63) { e.Handled = true; return; }	//	?
			else if (e.KeyChar == (Char)34) { e.Handled = true; return; }	//	"
			else if (e.KeyChar == (Char)60) { e.Handled = true; return; }	//	<
			else if (e.KeyChar == (Char)124) { e.Handled = true; return; }	//	|

			e.Handled = false;

		}

		void OriginalRadioButtonClick(object sender, EventArgs e)
		{
			SubLayout.Dewarp = 0;
		}

		void RotateRightRadioButtonClick(object sender, EventArgs e)
		{
			SubLayout.Dewarp = 5;
		}

		void RotateLeftRadioButtonClick(object sender, EventArgs e)
		{
			SubLayout.Dewarp = 4;
		}

		void FlipmirrorRadioButtonClick(object sender, EventArgs e)
		{
			SubLayout.Dewarp = 3;
		}
		void MirrorRadioButtonClick(object sender, EventArgs e)
		{
			SubLayout.Dewarp = 2;
		}

		void FlipRadioButtonClick(object sender, EventArgs e)
		{
			SubLayout.Dewarp = 1;
		}

        private Boolean _isEdit;
        private void ParseSubLayout()
        {
            _isEdit = false;

	        _originalRadioButton.Checked = false;
			_flipRadioButton.Checked = false;
			_mirrorRadioButton.Checked = false;
	        _flipmirrorRadioButton.Checked = false;
			_rotateLeftRadioButton.Checked = false;
			_rotateRightRadioButton.Checked = false;

            if (SubLayout != null)
            {
                _nameTextBox.Text = SubLayout.Name;
                _nameTextBox.Enabled = true;

				switch (SubLayout.Dewarp)
	            {
					case 0: _originalRadioButton.Checked = true; break;
					case 1: _flipRadioButton.Checked = true; break;
					case 2: _mirrorRadioButton.Checked = true; break;
					case 3: _flipmirrorRadioButton.Checked = true; break;
					case 4: _rotateLeftRadioButton.Checked = true; break;
					case 5: _rotateRightRadioButton.Checked = true; break;
	            }
            }
            else
            {
                _nameTextBox.Text = null;
                _nameTextBox.Enabled = false;
            }

            Invalidate();

            _isEdit = true;
        }

        private void DoneButtonMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnSubLayoutDoneClick != null)
                OnSubLayoutDoneClick(this, null);
        }

        private void NameTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!_isEdit || SubLayout == null || SubLayout.Name == _nameTextBox.Text) return;

            SubLayout.Name = _nameTextBox.Text;

            NVR.SubLayoutModify(SubLayout);
        }

        private void PaintTitle(Graphics g)
        {
            if (Width <= 100) return;
            Manager.PaintText(g, Localization["DeviceLayoutPanel_Name"]);

			if (Width <= 320) return;
			g.DrawString(Localization["DeviceLayoutPanel_Original"], Manager.Font, Brushes.Black, 300, 13);

			if (Width <= 400) return;
			g.DrawString(Localization["DeviceLayoutPanel_Flip"], Manager.Font, Brushes.Black, 380, 13);

			if (Width <= 470) return;
			g.DrawString(Localization["DeviceLayoutPanel_Mirror"], Manager.Font, Brushes.Black, 450, 13);

			if (Width <= 550) return;
			g.DrawString(Localization["DeviceLayoutPanel_FlipAndMirror"], Manager.Font, Brushes.Black, 520, 13);

			if (Width <= 650) return;
			g.DrawString(Localization["DeviceLayoutPanel_RotateRight"], Manager.Font, Brushes.Black, 630, 13);

			if (Width <= 750) return;
			g.DrawString(Localization["DeviceLayoutPanel_RotateLeft"], Manager.Font, Brushes.Black, 730, 13);
            
            if (Width <= 850) return;
            g.DrawString(Localization["DeviceLayoutPanel_X"], Manager.Font, Brushes.Black, 830, 13);
            
            if (Width <= 920) return;
            g.DrawString(Localization["DeviceLayoutPanel_Y"], Manager.Font, Brushes.Black, 900, 13);
            
            if (Width <= 990) return;
            g.DrawString(Localization["DeviceLayoutPanel_Width"], Manager.Font, Brushes.Black, 970, 13);
            
            if (Width <= 1080) return;
            g.DrawString(Localization["DeviceLayoutPanel_Height"], Manager.Font, Brushes.Black, 1060, 13);
        }

        private void SubLayoutPanelPaint(Object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;

            if (Parent == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, control);

            if (IsTitle)
            {
                PaintTitle(g);
            }

            if (SubLayout == null) return;

            //if (Width <= 300) return;
            //Manager.PaintText(g, SubLayout.Name);

			if (Width <= 850) return;
			g.DrawString(SubLayout.X.ToString(), Manager.Font, Brushes.Black, 830, 13);

			if (Width <= 920) return;
			g.DrawString(SubLayout.Y.ToString(), Manager.Font, Brushes.Black, 900, 13);

			if (Width <= 990) return;
			g.DrawString(SubLayout.Width.ToString(), Manager.Font, Brushes.Black, 975, 13);

			if (Width <= 1080) return;
			g.DrawString(SubLayout.Height.ToString(), Manager.Font, Brushes.Black, 1065, 13);
        }
    }
}
