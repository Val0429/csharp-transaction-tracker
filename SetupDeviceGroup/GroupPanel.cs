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
	public sealed class GroupPanel : Panel
	{
		public event EventHandler OnGroupEditClick;

		public Dictionary<String, String> Localization;
		
		private readonly  CheckBox _checkBox = new CheckBox();

		public IServer Server;

		private IDeviceGroup _group;
		public IDeviceGroup Group
		{
			get { return _group; }
			set {
				_group = value;
				_modifyed.Clear();
			}
		}

		private readonly TextBox _nameTextBox;

		private List<String> _modifyed = new List<string>(); 
		public GroupPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"GroupPanel_Name", "Name"},
								   {"GroupPanel_NumDevice", "(%1 Device)"},
								   {"GroupPanel_NumDevices", "(%1 Devices)"},
							   };
			Localizations.Update(Localization);

			DoubleBuffered = true;
			Dock = DockStyle.Top;
			Cursor = Cursors.Hand;
			Size = new Size(300, 40);

			BackColor = Color.Transparent;

			_nameTextBox = new PanelBase.HotKeyTextBox
			{
				MaxLength = 50,
				Visible = false,
				Anchor = AnchorStyles.Top | AnchorStyles.Right,
				Location = new Point(60, 10),
				Size = new Size(220, 22),
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0)
			};

			_checkBox.Padding = new Padding(10, 0, 0, 0);
			_checkBox.Dock = DockStyle.Left;
			_checkBox.Width = 25;

			Controls.Add(_checkBox);
			Controls.Add(_nameTextBox);

			_nameTextBox.TextChanged += NameTextBoxTextChanged;
			_checkBox.CheckedChanged += CheckBoxCheckedChanged;
			_nameTextBox.LostFocus += NameTextBoxLostFocus;
			MouseClick += GroupPanelMouseClick;
			Paint += GroupPanelPaint;
		}

		void NameTextBoxLostFocus(object sender, EventArgs e)
		{
			if ( _modifyed.Contains("NAME"))
				Server.WriteOperationLog("Modify Group Information %1 to %2".Replace("%1", Localization["GroupPanel_Name"]).Replace("%2", Group.Name));
		}

		private void NameTextBoxTextChanged(Object sender, EventArgs e)
		{

			if (Group == null || Server == null || Group.Name == _nameTextBox.Text) return;

			_modifyed.Add("NAME");

			Group.Name = _nameTextBox.Text;

			Server.GroupModify(Group);
		}

		public String GroupName
		{
			get { return  _nameTextBox.Text; }
		}

		public Boolean TextEditorVisible
		{
			set
			{
				_nameTextBox.Text = (Group != null) ? Group.Name : "";
				_nameTextBox.Visible = value;
			}
		}

		public Brush SelectedColor = Manager.DeleteTextColor;

		private static RectangleF _nameRectangleF = new RectangleF(44, 13, 236, 17);

		private void GroupPanelPaint(Object sender, PaintEventArgs e)
		{
			var control = sender as Control;
			if (control == null) return;

			if (Parent == null || Group == null) return;

			Graphics g = e.Graphics;

			Brush fontBrush = Brushes.Black;

			if (_editVisible || _checkBox.Visible)
			{
				if (_editVisible)
				{
					Manager.Paint(g, control, true);
					//Manager.PaintEdit(g, this);
				}
				else
					Manager.Paint(g, control);
			}
			else
			{
				Manager.PaintSingleInput(g, control);
			}

			Manager.PaintStatus(g, Group.ReadyState);

			if (!_editVisible && !_checkBox.Visible)
				g.DrawString(Localization["GroupPanel_Name"], Manager.Font, fontBrush, _nameRectangleF);

			if (Width <= 300) return;

			if (_checkBox.Visible && Checked)
				fontBrush = SelectedColor;
			
			if (!_nameTextBox.Visible)
			{
				if (Group.Items.Count == 0)
					g.DrawString(Group.ToString(), Manager.Font, fontBrush, _nameRectangleF);
				else
				{
					var name = Group + "   " + (((Group.Items.Count <= 1)
						? Localization["GroupPanel_NumDevice"]
						: Localization["GroupPanel_NumDevices"]).Replace("%1", Group.Items.Count.ToString()));

						g.DrawString(name, Manager.Font, fontBrush, _nameRectangleF);
				}
			}
		}

		private void GroupPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (_checkBox.Visible)
			{
				_checkBox.Checked = !_checkBox.Checked;
				return;
			}

			if (OnGroupEditClick != null)
				OnGroupEditClick(this, e);

		}

		private void CheckBoxCheckedChanged(Object sender, EventArgs e)
		{
			Invalidate();
		}

		public Boolean Checked
		{
			get
			{
				return _checkBox.Checked;
			}
			set
			{
				_checkBox.Checked = value;
			}
		}

		public Boolean SelectionVisible
		{
			get { return _checkBox.Visible; }
			set
			{
				if (value)
				{
					if (Group == null || Group.Id == 0)
						_checkBox.Visible = false;
					else
						_checkBox.Visible = true;
				}
				else
				{
					_checkBox.Visible = false;
				}
			}
		}

		private Boolean _editVisible;
		public Boolean EditVisible { 
			set
			{
				if (Group != null && Group.Id == 0)
				{
					_editVisible = false;
					Cursor = Cursors.Default;
				}
				else
				{
					_editVisible = value;
				}
				Invalidate();
			}
		}
	}
}
