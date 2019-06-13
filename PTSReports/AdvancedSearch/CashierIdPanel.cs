using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports.AdvancedSearch
{
	public sealed partial class CashierIdPanel : UserControl
	{
		public IPTS PTS;
		public List<POS_Exception.CashierIdCriteria> CashierIdCriterias;

		public Dictionary<String, String> Localization;

		public CashierIdPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_SearchConditionsAssociated", "Search conditions associated"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.None;
			Name = "CashierId";
            BackgroundImage = Manager.BackgroundNoBorder;

			conditionalComboBox.Items.Add("AND");
			conditionalComboBox.Items.Add("OR");
			conditionalComboBox.SelectedIndex = 1;
			conditionalComboBox.SelectedIndexChanged += ConditionalComboBoxSelectedIndexChanged;

			conditionalDoubleBufferPanel.Paint += ConditionalDoubleBufferPanelPaint;
		}

		private readonly Stack<CashierIdSelectPanel> _cashierIdSelectPanel = new Stack<CashierIdSelectPanel>();
		public void Initialize()
		{
			containerPanel.Controls.Clear();

			//default 10 empty cashierIds
			var cashierIdCriterias = new List<POS_Exception.CashierIdCriteria>();
			for (var i = 0; i < 10; i++)
			{
				cashierIdCriterias.Add(new POS_Exception.CashierIdCriteria
				{
					CashierId = "",
					Equation = POS_Exception.Comparative.Equal,
				});
			}
			cashierIdCriterias.Reverse();

			foreach (var cashierIdCriteria in cashierIdCriterias)
			{
				var cashierIdSelectPanel = GetCashierIdSelectPanel();
				cashierIdSelectPanel.CashierIdCriteria = cashierIdCriteria;
				containerPanel.Controls.Add(cashierIdSelectPanel);
				_cashierIdSelectPanel.Push(cashierIdSelectPanel);
			}

			var cashierIdSelectTitlePanel = GetCashierIdSelectPanel();
			cashierIdSelectTitlePanel.IsTitle = true;
			cashierIdSelectTitlePanel.Cursor = Cursors.Default;
			cashierIdSelectTitlePanel.Checked = false;
			cashierIdSelectTitlePanel.OnSelectAll += CashierIdSelectPanelOnSelectAll;
			cashierIdSelectTitlePanel.OnSelectNone += CashierIdSelectPanelOnSelectNone;

			containerPanel.Controls.Add(cashierIdSelectTitlePanel);
		}

		public void ParseSetting()
		{
			var isOr = true;
			foreach (var cashierIdCriteria in CashierIdCriterias)
			{
				if (_cashierIdSelectPanel.Count == 0) break;

				var cashierIdSelectPanel = _cashierIdSelectPanel.Pop();
				cashierIdSelectPanel.CashierIdCriteria = cashierIdCriteria;

				cashierIdSelectPanel.OnSelectChange -= CashierIdSelectPanelOnSelectChange;
				cashierIdSelectPanel.Checked = true;
				cashierIdSelectPanel.OnSelectChange += CashierIdSelectPanelOnSelectChange;

				if (isOr)
					isOr = (cashierIdCriteria.Condition == POS_Exception.Logic.OR);
			}

			conditionalComboBox.SelectedIndexChanged += ConditionalComboBoxSelectedIndexChanged;
			conditionalComboBox.SelectedItem = POS_Exception.Logics.ToString(isOr ? POS_Exception.Logic.OR : POS_Exception.Logic.AND);
			conditionalComboBox.SelectedIndexChanged -= ConditionalComboBoxSelectedIndexChanged;

			if (_cashierIdSelectPanel.Count == 0 && containerPanel.Controls.Count > 0)
			{
				var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as CashierIdSelectPanel;
				if (title != null && title.IsTitle)
				{
					title.OnSelectAll -= CashierIdSelectPanelOnSelectAll;
					title.OnSelectNone -= CashierIdSelectPanelOnSelectNone;

					title.Checked = true;

					title.OnSelectAll += CashierIdSelectPanelOnSelectAll;
					title.OnSelectNone += CashierIdSelectPanelOnSelectNone;
				}
			}
		}

		public void ScrollTop()
		{
			containerPanel.Select();
			containerPanel.AutoScrollPosition = new Point(0, 0);
		}

		private void ConditionalComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			var condition = POS_Exception.Logics.ToIndex(conditionalComboBox.SelectedItem.ToString());
			foreach (var cashierIdCriteria in CashierIdCriterias)
			{
				cashierIdCriteria.Condition = condition;
			}
		}

		private void ConditionalDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			var g = e.Graphics;

			Manager.PaintSingleInput(g, conditionalDoubleBufferPanel);
			if (conditionalDoubleBufferPanel.Width <= 150) return;

			Manager.PaintText(g, Localization["PTSReports_SearchConditionsAssociated"]);
		}

		private CashierIdSelectPanel GetCashierIdSelectPanel()
		{
			var cashierIdSelectPanel = new CashierIdSelectPanel
			{
				SelectionVisible = true,
			};

			cashierIdSelectPanel.OnSelectChange += CashierIdSelectPanelOnSelectChange;

			return cashierIdSelectPanel;
		}

		private void CashierIdSelectPanelOnSelectChange(Object sender, EventArgs e)
		{
			var panel = sender as CashierIdSelectPanel;
			if (panel == null) return;
			if (panel.IsTitle) return;
			var condition = POS_Exception.Logics.ToIndex(conditionalComboBox.SelectedItem.ToString());

			var selectAll = false;
			if (panel.Checked)
			{
				panel.CashierIdCriteria.Condition = condition;
				if (!CashierIdCriterias.Contains(panel.CashierIdCriteria))
					CashierIdCriterias.Add(panel.CashierIdCriteria);

				selectAll = true;
				foreach (CashierIdSelectPanel control in containerPanel.Controls)
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
				CashierIdCriterias.Remove(panel.CashierIdCriteria);
			}

			var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as CashierIdSelectPanel;
			if (title != null && title.IsTitle && title.Checked != selectAll)
			{
				title.OnSelectAll -= CashierIdSelectPanelOnSelectAll;
				title.OnSelectNone -= CashierIdSelectPanelOnSelectNone;

				title.Checked = selectAll;

				title.OnSelectAll += CashierIdSelectPanelOnSelectAll;
				title.OnSelectNone += CashierIdSelectPanelOnSelectNone;
			}
		}

		private void CashierIdSelectPanelOnSelectAll(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (CashierIdSelectPanel control in containerPanel.Controls)
			{
				control.Checked = true;
			}
			containerPanel.AutoScroll = true;
		}

		private void CashierIdSelectPanelOnSelectNone(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (CashierIdSelectPanel control in containerPanel.Controls)
			{
				control.Checked = false;
			}
			containerPanel.AutoScroll = true;
		}
	}

	public sealed class CashierIdSelectPanel : Panel
	{
		public event EventHandler OnSelectAll;
		public event EventHandler OnSelectNone;
		public event EventHandler OnSelectChange;

		public Dictionary<String, String> Localization;

		private readonly CheckBox _checkBox;
		private readonly TextBox _cashierIdTextBox;
		private readonly ComboBox _conditionComboBox;
		private readonly ComboBox _equationComboBox;

		private Boolean _isTitle;
		public Boolean IsTitle
		{
			get { return _isTitle; }
			set
			{
				_isTitle = value;
				_conditionComboBox.Visible = _equationComboBox.Visible =
					_cashierIdTextBox.Visible = !value;
			}
		}
		private POS_Exception.CashierIdCriteria _cashierIdCriteria;
		public POS_Exception.CashierIdCriteria CashierIdCriteria
		{
			get { return _cashierIdCriteria; }
			set
			{
				_cashierIdCriteria = value;
				if (_cashierIdCriteria != null)
				{
					_isEditing = false;
					_cashierIdTextBox.Text = _cashierIdCriteria.CashierId;
					_conditionComboBox.SelectedItem = POS_Exception.Logics.ToString(_cashierIdCriteria.Condition);
					switch (_cashierIdCriteria.Equation)
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

		public CashierIdSelectPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_CashierId", "Cashier Id"},
								   {"PTSReports_Conditional", "Conditional"},
								   {"PTSReports_Equation", "Equation"},
							   };
			Localizations.Update(Localization);

			DoubleBuffered = true;
			Dock = DockStyle.Top;
			Cursor = Cursors.Default;
			Height = 40;

			BackColor = Color.Transparent;

			_checkBox = new CheckBox
			{
				Padding = new Padding(10, 0, 0, 0),
				Dock = DockStyle.Left,
				Width = 25
			};

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

			_cashierIdTextBox = new PanelBase.HotKeyTextBox
			{
				Location = new Point(130, 6),
				Size = new Size(90, 23),
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0)
			};
			_cashierIdTextBox.TextChanged += CashierIdTextBoxTextChanged;

			_conditionComboBox = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				Location = new Point(230, 6),
				Size = new Size(50, 23)
			};
			_conditionComboBox.Items.Add("AND");
			_conditionComboBox.Items.Add("OR");
			_conditionComboBox.SelectedIndex = 1;
			_conditionComboBox.SelectedIndexChanged += ConditionComboBoxSelectedIndexChanged;

			Controls.Add(_checkBox);
			Controls.Add(_equationComboBox);
			Controls.Add(_cashierIdTextBox);
			//Controls.Add(_conditionComboBox);

			_checkBox.CheckedChanged += CheckBoxCheckedChanged;

			MouseClick += CashierIdSelectPanelMouseClick;

			Paint += CashierIdSelectPanelPaint;
		}

		private Boolean _isEditing;

		private void CashierIdTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _cashierIdCriteria == null) return;

			_cashierIdCriteria.CashierId = _cashierIdTextBox.Text;
		}

		private void ConditionComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _cashierIdCriteria == null) return;

			_cashierIdCriteria.Condition = POS_Exception.Logics.ToIndex(_conditionComboBox.SelectedItem.ToString());
		}

		private void EquationComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _cashierIdCriteria == null) return;

			switch (_equationComboBox.SelectedItem.ToString())
			{
				case "equal":
					_cashierIdCriteria.Equation = POS_Exception.Comparative.Equal;
					break;

				case "not equal":
					_cashierIdCriteria.Equation = POS_Exception.Comparative.NotEqual;
					break;
			}
		}
		
		private void PaintTitle(Graphics g)
		{
			if (Width <= 200) return;
			Manager.PaintTitleText(g, Localization["PTSReports_Equation"]);

			g.DrawString(Localization["PTSReports_CashierId"], Manager.Font, Manager.TitleTextColor, 130, 13);

			//if (Width <= 300) return;
			//g.DrawString(Localization["PTSReports_Conditional"], Manager.Font, Manager.TitleTextColor, 230, 13);
		}

		private void CashierIdSelectPanelPaint(Object sender, PaintEventArgs e)
		{
			if (Parent == null) return;

			Graphics g = e.Graphics;
		   
			if (IsTitle)
			{
				Manager.PaintTitleTopInput(g, this);
				PaintTitle(g);
				return;
			}

			Manager.Paint(g, (Control)sender);
		}

		private void CashierIdSelectPanelMouseClick(Object sender, MouseEventArgs e)
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
			set { _checkBox.Visible = value; }
		}
	}
}
