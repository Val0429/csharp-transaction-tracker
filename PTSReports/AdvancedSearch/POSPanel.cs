using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports.AdvancedSearch
{
	public sealed partial class POSPanel : UserControl
	{
		public IPTS PTS;
		public List<POS_Exception.POSCriteria> POSCriterias;

		public Dictionary<String, String> Localization;

		public POSPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_SearchConditionsAssociated", "Search conditions associated"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.None;
			Name = "POS";
            BackgroundImage = Manager.BackgroundNoBorder;

			conditionalComboBox.Items.Add("AND");
			conditionalComboBox.Items.Add("OR");
			conditionalComboBox.SelectedIndex = 1;
			conditionalComboBox.SelectedIndexChanged += ConditionalComboBoxSelectedIndexChanged;

			conditionalDoubleBufferPanel.Paint += ConditionalDoubleBufferPanelPaint;
		}

		private Boolean _isEditing;
		public void ParseSetting()
		{
			_isEditing = false;

			ClearViewModel();

			var sortResult = new List<IPOS>(PTS.POS.POSServer);

			if (sortResult.Count == 0) return;

			var isOr = true;
            sortResult.Sort((x, y) => (x.Id.CompareTo(y.Id)));
            
			var selectAll = true;
			var count = 0;
			//containerPanel.Visible = false;
			foreach (IPOS pos in sortResult)
			{
				var posPanel = GetPOSPanel();

				posPanel.POS = pos;

				var hasCriteria = false;
				foreach (var posCriteria in POSCriterias)
				{
					if (posCriteria.POSId == pos.Id)
					{
						hasCriteria = true;
						count++;
						posPanel.Checked = true;
						posPanel.POSCriteria = posCriteria;

						if (isOr)
							isOr = (posCriteria.Condition == POS_Exception.Logic.OR);
					}
				}

				if (!hasCriteria)
				{
					posPanel.POSCriteria = new POS_Exception.POSCriteria
					{
						POSId = pos.Id,
						Equation = POS_Exception.Comparative.Equal,
					};
					selectAll = false;
				}

				containerPanel.Controls.Add(posPanel);
			}

			conditionalComboBox.SelectedIndexChanged += ConditionalComboBoxSelectedIndexChanged;
			conditionalComboBox.SelectedItem = POS_Exception.Logics.ToString(isOr ? POS_Exception.Logic.OR : POS_Exception.Logic.AND);
			conditionalComboBox.SelectedIndexChanged -= ConditionalComboBoxSelectedIndexChanged;

			if (count == 0 && selectAll)
				selectAll = false;

			var posTitlePanel = GetPOSPanel();
			posTitlePanel.IsTitle = true;
			posTitlePanel.Cursor = Cursors.Default;
			posTitlePanel.Checked = selectAll;
			//deviceTitleControl.OnSortChange += DeviceControlOnSortChange;
			posTitlePanel.OnSelectAll += POSPanelOnSelectAll;
			posTitlePanel.OnSelectNone += POSPanelOnSelectNone;
			containerPanel.Controls.Add(posTitlePanel);
			//containerPanel.Visible = true;

			_isEditing = true;

		}

		//public void ScrollTop()
		//{
		//    //containerPanel.AutoScroll = false;
		//    containerPanel.Select();
		//    containerPanel.AutoScrollPosition = new Point(0, 0);
		//    //containerPanel.AutoScroll = true;
		//}

		private void ConditionalComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			var condition = POS_Exception.Logics.ToIndex(conditionalComboBox.SelectedItem.ToString());
			foreach (var posCriteria in POSCriterias)
			{
				posCriteria.Condition = condition;
			}
		}

		private void ConditionalDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			var g = e.Graphics;

			Manager.PaintSingleInput(g, conditionalDoubleBufferPanel);
			if (conditionalDoubleBufferPanel.Width <= 150) return;

			Manager.PaintText(g, Localization["PTSReports_SearchConditionsAssociated"]);
		}

		private readonly Queue<POSSelectPanel> _recyclePOS = new Queue<POSSelectPanel>();
		private POSSelectPanel GetPOSPanel()
		{
			if (_recyclePOS.Count > 0)
			{
				return _recyclePOS.Dequeue();
			}

			var posPanel = new POSSelectPanel
			{
				SelectionVisible = true,
			};

			posPanel.OnSelectChange += POSSelectPanelOnSelectChange;

			return posPanel;
		}

		private void POSSelectPanelOnSelectChange(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			var panel = sender as POSSelectPanel;
			if (panel == null) return;
			if (panel.IsTitle) return;
			var condition = POS_Exception.Logics.ToIndex(conditionalComboBox.SelectedItem.ToString());

			var selectAll = false;
			if (panel.Checked)
			{
				panel.POSCriteria.Condition = condition;
				if (!POSCriterias.Contains(panel.POSCriteria))
				{
					POSCriterias.Add(panel.POSCriteria);
                    POSCriterias.Sort((x, y) => (x.POSId.CompareTo(y.POSId)));
				}

				selectAll = true;
				foreach (POSSelectPanel control in containerPanel.Controls)
				{
					if (control.IsTitle) continue;
					if (!control.Checked && control.Enabled)
					{
						selectAll = false;
						break;
					}
				}
			}
			else
			{
				POSCriterias.Remove(panel.POSCriteria);
			}

			var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as POSSelectPanel;
			if (title != null && title.IsTitle && title.Checked != selectAll)
			{
				title.OnSelectAll -= POSPanelOnSelectAll;
				title.OnSelectNone -= POSPanelOnSelectNone;

				title.Checked = selectAll;

				title.OnSelectAll += POSPanelOnSelectAll;
				title.OnSelectNone += POSPanelOnSelectNone;
			}
		}

		private void ClearViewModel()
		{
			foreach (POSSelectPanel posPanel in containerPanel.Controls)
			{
				posPanel.SelectionVisible = false;

				posPanel.Checked = false;
				posPanel.POS = null;
				posPanel.POSCriteria = null;
				posPanel.Cursor = Cursors.Hand;
				posPanel.SelectionVisible = true;

				if (posPanel.IsTitle)
				{
					posPanel.OnSelectAll -= POSPanelOnSelectAll;
					posPanel.OnSelectNone -= POSPanelOnSelectNone;
					posPanel.IsTitle = false;
				}

				if (!_recyclePOS.Contains(posPanel))
				{
					_recyclePOS.Enqueue(posPanel);
				}
			}
			containerPanel.Controls.Clear();
		}

		private void POSPanelOnSelectAll(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (POSSelectPanel control in containerPanel.Controls)
			{
				control.Checked = true;
			}
			containerPanel.AutoScroll = true;
		}

		private void POSPanelOnSelectNone(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (POSSelectPanel control in containerPanel.Controls)
			{
				control.Checked = false;
			}
			containerPanel.AutoScroll = true;
		}

		public void ScrollTop()
		{
			containerPanel.Select();
			containerPanel.AutoScrollPosition = new Point(0, 0);
		}
	}

	public sealed class POSSelectPanel : Panel
	{
		public event EventHandler OnSelectAll;
		public event EventHandler OnSelectNone;
		public event EventHandler OnSelectChange;

		public Dictionary<String, String> Localization;

		private readonly CheckBox _checkBox;
		private readonly ComboBox _conditionComboBox;
		private readonly ComboBox _equationComboBox;

		private Boolean _isTitle;
		public Boolean IsTitle
		{
			get { return _isTitle; }
			set
			{
				_isTitle = value;
				_conditionComboBox.Visible = _equationComboBox.Visible = !value;
			}
		}
		public IPOS POS;
		private POS_Exception.POSCriteria _posCriteria;
		public POS_Exception.POSCriteria POSCriteria
		{
			get { return _posCriteria; }
			set
			{
				_posCriteria = value;
				if (_posCriteria != null)
				{
					_isEditing = false;
					_conditionComboBox.SelectedItem = POS_Exception.Logics.ToString(_posCriteria.Condition);
					switch (_posCriteria.Equation)
					{
						case POS_Exception.Comparative.Equal:
							_equationComboBox.SelectedItem = "equal";
							break;

						case POS_Exception.Comparative.NotEqual:
							_equationComboBox.SelectedItem = "not equal";
							break;
					}
					_isEditing = true;
				}
			}
		}

		public POSSelectPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"POS_ID", "ID"},
								   {"POS_Name", "Name"},
								   {"POS_Manufacture", "Manufacture"},
								   {"POS_RegisterId", "Register Id"},

								   {"PTSReports_Conditional", "Conditional"},
								   {"PTSReports_Equation", "Equation"},
							   };
			Localizations.Update(Localization);

			DoubleBuffered = true;
			Dock = DockStyle.Top;
			Cursor = Cursors.Hand;
			Height = 40;

			BackColor = Color.Transparent;
			_checkBox = new CheckBox
			{
				Padding = new Padding(10, 0, 0, 0),
				Dock = DockStyle.Left,
				Width = 25
			};
			_checkBox.CheckedChanged += CheckBoxCheckedChanged;

			_equationComboBox = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				Location = new Point(44, 6),
				Size = new Size(75, 23)
			};
			_equationComboBox.Items.Add("equal");//=
			_equationComboBox.Items.Add("not equal");//<>
			_equationComboBox.SelectedIndex = 0;
			_equationComboBox.SelectedIndexChanged += EquationComboBoxSelectedIndexChanged;

			_conditionComboBox = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				Location = new Point(380, 6),
				Size = new Size(50, 23)
			};
			_conditionComboBox.Items.Add("AND");
			_conditionComboBox.Items.Add("OR");
			_conditionComboBox.SelectedIndex = 1;
			_conditionComboBox.SelectedIndexChanged += ConditionComboBoxSelectedIndexChanged;

			Controls.Add(_checkBox);
			Controls.Add(_equationComboBox);
			//Controls.Add(_conditionComboBox);

			MouseClick += POSPanelMouseClick;
			Paint += POSPanelPaint;
		}

		private Boolean _isEditing;
		private void ConditionComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _posCriteria == null) return;

			_posCriteria.Condition = POS_Exception.Logics.ToIndex(_conditionComboBox.SelectedItem.ToString());
		}

		private void EquationComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _posCriteria == null) return;

			switch (_equationComboBox.SelectedItem.ToString())
			{
				case "equal":
					_posCriteria.Equation = POS_Exception.Comparative.Equal;
					break;

				case "not equal":
					_posCriteria.Equation = POS_Exception.Comparative.NotEqual;
					break;
			}
		}

		private readonly RectangleF _nameRectangleF = new RectangleF(200, 13, 126, 15);

		private void PaintTitle(Graphics g)
		{
			if (Width <= 200) return;
			Manager.PaintText(g, Localization["PTSReports_Equation"]);

			if (Width <= 300) return;
			g.DrawString(Localization["POS_RegisterId"], Manager.Font, Brushes.Black, 130, 13);
			g.DrawString(Localization["POS_Name"], Manager.Font, Brushes.Black, 200, 13);

			if (Width <= 490) return;
			g.DrawString(Localization["POS_Manufacture"], Manager.Font, Brushes.Black, 330, 13);

			//if (Width <= 550) return;
			//g.DrawString(Localization["PTSReports_Conditional"], Manager.Font, Brushes.Black, 380, 13);
		}

		private void POSPanelPaint(Object sender, PaintEventArgs e)
		{
			if (Parent == null) return;

			Graphics g = e.Graphics;

			Manager.Paint(g, (Control)sender);

			if (IsTitle)
			{
				PaintTitle(g);
				return;
			}

			Brush fontBrush = Brushes.Black;

			Manager.PaintStatus(g, POS.ReadyState);

			if (_checkBox.Visible && Checked)
			{
				fontBrush = SelectedColor;
			}

			if (Width <= 300) return;
			g.DrawString(POS.Id.ToString(), Manager.Font, fontBrush, 130, 13);

			g.DrawString(POS.Name, Manager.Font, fontBrush, _nameRectangleF);

			if (Width <= 490) return;
			g.DrawString(POS.Manufacture, Manager.Font, fontBrush, 330, 13);
		}

		private void POSPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (IsTitle)
			{
				if (_checkBox.Visible)
				{
					_checkBox.Checked = !_checkBox.Checked;
				}
			}
			else
			{
				if (_checkBox.Visible)
				{
					_checkBox.Checked = !_checkBox.Checked;
				}
			}
		}

		private void CheckBoxCheckedChanged(Object sender, EventArgs e)
		{
			Invalidate();

			if (IsTitle)
			{
				if (Checked && OnSelectAll != null)
					OnSelectAll(this, null);
				else if (!Checked && OnSelectNone != null)
					OnSelectNone(this, null);

				return;
			}

			_checkBox.Focus();
			if (OnSelectChange != null)
				OnSelectChange(this, null);
		}

		public Brush SelectedColor = Brushes.RoyalBlue;

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
			set
			{
				_checkBox.Visible = value;
			}
		}
	}
}
