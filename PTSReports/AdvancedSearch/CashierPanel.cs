using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports.AdvancedSearch
{
	public sealed partial class CashierPanel : UserControl
	{
		public IPTS PTS;
		public List<POS_Exception.CashierCriteria> CashierCriterias;

		public Dictionary<String, String> Localization;

		public CashierPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_SearchConditionsAssociated", "Search conditions associated"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.None;
			Name = "Cashier";
            BackgroundImage = Manager.BackgroundNoBorder;

			conditionalComboBox.Items.Add("AND");
			conditionalComboBox.Items.Add("OR");
			conditionalComboBox.SelectedIndex = 1;
			conditionalComboBox.SelectedIndexChanged += ConditionalComboBoxSelectedIndexChanged;

			conditionalDoubleBufferPanel.Paint += ConditionalDoubleBufferPanelPaint;
		}

		private readonly Stack<CashierSelectPanel> _cashierSelectPanel = new Stack<CashierSelectPanel>();
		public void Initialize()
		{
			containerPanel.Controls.Clear();

			//default 10 empty cashiers
			var cashierCriterias = new List<POS_Exception.CashierCriteria>();
			for (var i = 0; i < 10; i++)
			{
				cashierCriterias.Add(new POS_Exception.CashierCriteria
				{
					Cashier = "",
					Equation = POS_Exception.Comparative.Like,
				});
			}
			cashierCriterias.Reverse();

			foreach (var cashierCriteria in cashierCriterias)
			{
				var cashierSelectPanel = GetCashierSelectPanel();
				cashierSelectPanel.CashierCriteria = cashierCriteria;
				containerPanel.Controls.Add(cashierSelectPanel);
				_cashierSelectPanel.Push(cashierSelectPanel);
			}

			var cashierSelectTitlePanel = GetCashierSelectPanel();
			cashierSelectTitlePanel.IsTitle = true;
			cashierSelectTitlePanel.Cursor = Cursors.Default;
			cashierSelectTitlePanel.Checked = false;
			cashierSelectTitlePanel.OnSelectAll += CashierSelectPanelOnSelectAll;
			cashierSelectTitlePanel.OnSelectNone += CashierSelectPanelOnSelectNone;

			containerPanel.Controls.Add(cashierSelectTitlePanel);
		}

		public void ParseSetting()
		{
			var isOr = true;
			foreach (var cashierCriteria in CashierCriterias)
			{
				if(_cashierSelectPanel.Count == 0) break;

				var cashierSelectPanel = _cashierSelectPanel.Pop();
				cashierSelectPanel.CashierCriteria = cashierCriteria;

				cashierSelectPanel.OnSelectChange -= CashierSelectPanelOnSelectChange;
				cashierSelectPanel.Checked = true;
				cashierSelectPanel.OnSelectChange += CashierSelectPanelOnSelectChange;

				if (isOr)
					isOr = (cashierCriteria.Condition == POS_Exception.Logic.OR);
			}

			conditionalComboBox.SelectedIndexChanged += ConditionalComboBoxSelectedIndexChanged;
			conditionalComboBox.SelectedItem = POS_Exception.Logics.ToString(isOr ? POS_Exception.Logic.OR : POS_Exception.Logic.AND);
			conditionalComboBox.SelectedIndexChanged -= ConditionalComboBoxSelectedIndexChanged;

			if (_cashierSelectPanel.Count == 0 && containerPanel.Controls.Count > 0)
			{
				var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as CashierSelectPanel;
				if (title != null && title.IsTitle)
				{
					title.OnSelectAll -= CashierSelectPanelOnSelectAll;
					title.OnSelectNone -= CashierSelectPanelOnSelectNone;

					title.Checked = true;

					title.OnSelectAll += CashierSelectPanelOnSelectAll;
					title.OnSelectNone += CashierSelectPanelOnSelectNone;
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
			foreach (var cashierCriteria in CashierCriterias)
			{
				cashierCriteria.Condition = condition;
			}
		}

		private void ConditionalDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			var g = e.Graphics;

			Manager.PaintSingleInput(g, conditionalDoubleBufferPanel);
			if (conditionalDoubleBufferPanel.Width <= 150) return;

			Manager.PaintText(g, Localization["PTSReports_SearchConditionsAssociated"]);
		}

		private CashierSelectPanel GetCashierSelectPanel()
		{
			var cashierSelectPanel = new CashierSelectPanel
			{
				SelectionVisible = true,
			};

			cashierSelectPanel.OnSelectChange += CashierSelectPanelOnSelectChange;

			return cashierSelectPanel;
		}

		private void CashierSelectPanelOnSelectChange(Object sender, EventArgs e)
		{
			var panel = sender as CashierSelectPanel;
			if (panel == null) return;
			if (panel.IsTitle) return;
			var condition = POS_Exception.Logics.ToIndex(conditionalComboBox.SelectedItem.ToString());

			var selectAll = false;
			if (panel.Checked)
			{
				panel.CashierCriteria.Condition = condition;
				if (!CashierCriterias.Contains(panel.CashierCriteria))
					CashierCriterias.Add(panel.CashierCriteria);

				selectAll = true;
				foreach (CashierSelectPanel control in containerPanel.Controls)
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
				CashierCriterias.Remove(panel.CashierCriteria);
			}

			var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as CashierSelectPanel;
			if (title != null && title.IsTitle && title.Checked != selectAll)
			{
				title.OnSelectAll -= CashierSelectPanelOnSelectAll;
				title.OnSelectNone -= CashierSelectPanelOnSelectNone;

				title.Checked = selectAll;

				title.OnSelectAll += CashierSelectPanelOnSelectAll;
				title.OnSelectNone += CashierSelectPanelOnSelectNone;
			}
		}

		private void CashierSelectPanelOnSelectAll(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (CashierSelectPanel control in containerPanel.Controls)
			{
				control.Checked = true;
			}
			containerPanel.AutoScroll = true;
		}

		private void CashierSelectPanelOnSelectNone(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (CashierSelectPanel control in containerPanel.Controls)
			{
				control.Checked = false;
			}
			containerPanel.AutoScroll = true;
		}
	}

	public sealed class CashierSelectPanel: Panel
	{
		public event EventHandler OnSelectAll;
		public event EventHandler OnSelectNone;
		public event EventHandler OnSelectChange;

		public Dictionary<String, String> Localization;

		private readonly CheckBox _checkBox;
		private readonly TextBox _cashierTextBox;
		private readonly ComboBox _conditionComboBox;
		private readonly ComboBox _equationComboBox;

		private Boolean _isTitle;
		public Boolean IsTitle
		{
			get { return _isTitle; }
			set
			{
				_isTitle = value;
				_cashierTextBox.Visible = _conditionComboBox.Visible = _equationComboBox.Visible = !value;
			}
		}
		private POS_Exception.CashierCriteria _cashierCriteria;
		public POS_Exception.CashierCriteria CashierCriteria
		{
			get { return _cashierCriteria; }
			set
			{
				_cashierCriteria = value;
				if (_cashierCriteria != null)
				{
					_isEditing = false;
					_cashierTextBox.Text = _cashierCriteria.Cashier;
					_conditionComboBox.SelectedItem = POS_Exception.Logics.ToString(_cashierCriteria.Condition);
					switch (_cashierCriteria.Equation)
					{
						case POS_Exception.Comparative.Include:
							_equationComboBox.SelectedItem = "include";
							break;

						case POS_Exception.Comparative.Exclude:
							_equationComboBox.SelectedItem = "exclude";
							break;
					}
					_isEditing = true;
				}
			}
		}

		public CashierSelectPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_Cashier", "Cashier"},
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
				Size = new Size(65, 23)
			};
			_equationComboBox.Items.Add("include");
			_equationComboBox.Items.Add("exclude");
			_equationComboBox.SelectedIndex = 0;
			_equationComboBox.SelectedIndexChanged += EquationComboBoxSelectedIndexChanged;

			_cashierTextBox = new PanelBase.HotKeyTextBox
			{
				Location = new Point(125, 6),
				Size = new Size(90, 23),
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0)
			};
			_cashierTextBox.TextChanged += CashierTextBoxTextChanged;

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
			Controls.Add(_cashierTextBox);
			//Controls.Add(_conditionComboBox);

			_checkBox.CheckedChanged += CheckBoxCheckedChanged;

			MouseClick += CashierSelectPanelMouseClick;

			Paint += CashierSelectPanelPaint;
		}

		private Boolean _isEditing;

		private void CashierTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _cashierCriteria == null) return;

			_cashierCriteria.Cashier = _cashierTextBox.Text;
		}

		private void ConditionComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _cashierCriteria == null) return;

			_cashierCriteria.Condition = POS_Exception.Logics.ToIndex(_conditionComboBox.SelectedItem.ToString());
		}

		private void EquationComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _cashierCriteria == null) return;

			switch (_equationComboBox.SelectedItem.ToString())
			{
				case "include":
					_cashierCriteria.Equation = POS_Exception.Comparative.Include;
					break;

				case "exclude":
					_cashierCriteria.Equation = POS_Exception.Comparative.Exclude;
					break;
			}
		}
		
		private void PaintTitle(Graphics g)
		{
			if (Width <= 200) return;
			Manager.PaintTitleText(g, Localization["PTSReports_Equation"]);

			g.DrawString(Localization["PTSReports_Cashier"], Manager.Font, Manager.TitleTextColor, 125, 13);

			//if (Width <= 300) return;
			//g.DrawString(Localization["PTSReports_Conditional"], Manager.Font, Manager.TitleTextColor, 230, 13);
		}

		private void CashierSelectPanelPaint(Object sender, PaintEventArgs e)
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

		private void CashierSelectPanelMouseClick(Object sender, MouseEventArgs e)
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
