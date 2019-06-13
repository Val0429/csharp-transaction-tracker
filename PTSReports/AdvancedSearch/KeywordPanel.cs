using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports.AdvancedSearch
{
	public sealed partial class KeywordPanel : UserControl
	{
		public IPTS PTS;
		public List<POS_Exception.KeywordCriteria> KeywordCriterias;

		public Dictionary<String, String> Localization;

		public KeywordPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_SearchConditionsAssociated", "Search conditions associated"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.None;
			Name = "Keyword";
            BackgroundImage = Manager.BackgroundNoBorder;

			conditionalComboBox.Items.Add("AND");
			conditionalComboBox.Items.Add("OR");
			conditionalComboBox.SelectedIndex = 0;
			conditionalComboBox.SelectedIndexChanged += ConditionalComboBoxSelectedIndexChanged;

			conditionalDoubleBufferPanel.Paint += ConditionalDoubleBufferPanelPaint;
		}

		private readonly Stack<KeywordSelectPanel> _keywordSelectPanel = new Stack<KeywordSelectPanel>();
		public void Initialize()
		{
			containerPanel.Controls.Clear();

			conditionalComboBox.SelectedIndex = 0; //AND

			//default 10 empty keyword
			var keywordCriterias = new List<POS_Exception.KeywordCriteria>();
			for (var i = 0; i < 10; i++)
			{
				keywordCriterias.Add(new POS_Exception.KeywordCriteria
				{
					Keyword = "",
					Condition = POS_Exception.Logic.AND,
					Equation = POS_Exception.Comparative.Like,
				});
			}
			keywordCriterias.Reverse();

			foreach (var keywordCriteria in keywordCriterias)
			{
				var keywordSelectPanel = GetKeywordSelectPanel();
				keywordSelectPanel.KeywordCriteria = keywordCriteria;
				containerPanel.Controls.Add(keywordSelectPanel);
				_keywordSelectPanel.Push(keywordSelectPanel);
			}

			var keywordSelectTitlePanel = GetKeywordSelectPanel();
			keywordSelectTitlePanel.IsTitle = true;
			keywordSelectTitlePanel.Cursor = Cursors.Default;
			keywordSelectTitlePanel.Checked = false;
			keywordSelectTitlePanel.OnSelectAll += KeywordSelectPanelOnSelectAll;
			keywordSelectTitlePanel.OnSelectNone += KeywordSelectPanelOnSelectNone;

			containerPanel.Controls.Add(keywordSelectTitlePanel);
		}

		public void ParseSetting()
		{
			var isOr = false;
			foreach (var keywordCriteria in KeywordCriterias)
			{
				if (_keywordSelectPanel.Count == 0) break;

				var keywordSelectPanel = _keywordSelectPanel.Pop();
				keywordSelectPanel.KeywordCriteria = keywordCriteria;

				keywordSelectPanel.OnSelectChange -= KeywordSelectPanelOnSelectChange;
				keywordSelectPanel.Checked = true;
				keywordSelectPanel.OnSelectChange += KeywordSelectPanelOnSelectChange;

				if (!isOr)
					isOr = (keywordCriteria.Condition == POS_Exception.Logic.OR);
			}

			conditionalComboBox.SelectedIndexChanged += ConditionalComboBoxSelectedIndexChanged;
			conditionalComboBox.SelectedItem = POS_Exception.Logics.ToString(isOr ? POS_Exception.Logic.OR : POS_Exception.Logic.AND);
			conditionalComboBox.SelectedIndexChanged -= ConditionalComboBoxSelectedIndexChanged;

			if (_keywordSelectPanel.Count == 0 && containerPanel.Controls.Count > 0)
			{
				var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as KeywordSelectPanel;
				if (title != null && title.IsTitle)
				{
					title.OnSelectAll -= KeywordSelectPanelOnSelectAll;
					title.OnSelectNone -= KeywordSelectPanelOnSelectNone;

					title.Checked = true;

					title.OnSelectAll += KeywordSelectPanelOnSelectAll;
					title.OnSelectNone += KeywordSelectPanelOnSelectNone;
				}
			}
		}

		public void ScrollTop()
		{
			containerPanel.Select();
			containerPanel.AutoScrollPosition = new Point(0, 0);
		}

		private KeywordSelectPanel GetKeywordSelectPanel()
		{
			var keywordSelectPanel = new KeywordSelectPanel
			{
				SelectionVisible = true,
			};

			keywordSelectPanel.OnSelectChange += KeywordSelectPanelOnSelectChange;

			return keywordSelectPanel;
		}

		private void ConditionalComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			foreach (var keywordCriteria in KeywordCriterias)
			{
				keywordCriteria.Condition = POS_Exception.Logics.ToIndex(conditionalComboBox.SelectedItem.ToString());
			}
		}

		private void ConditionalDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			var g = e.Graphics;

			Manager.PaintSingleInput(g, conditionalDoubleBufferPanel);
			if (conditionalDoubleBufferPanel.Width <= 150) return;

			Manager.PaintText(g, Localization["PTSReports_SearchConditionsAssociated"]);
		}

		private void KeywordSelectPanelOnSelectChange(Object sender, EventArgs e)
		{
			var panel = sender as KeywordSelectPanel;
			if (panel == null) return;
			if (panel.IsTitle) return;
			var condition = POS_Exception.Logics.ToIndex(conditionalComboBox.SelectedItem.ToString());

			var selectAll = false;
			if (panel.Checked)
			{
				if (!KeywordCriterias.Contains(panel.KeywordCriteria))
				{
					panel.KeywordCriteria.Condition = condition;
					KeywordCriterias.Add(panel.KeywordCriteria);
				}

				selectAll = true;
				foreach (KeywordSelectPanel control in containerPanel.Controls)
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
				KeywordCriterias.Remove(panel.KeywordCriteria);
			}

			var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as KeywordSelectPanel;
			if (title != null && title.IsTitle && title.Checked != selectAll)
			{
				title.OnSelectAll -= KeywordSelectPanelOnSelectAll;
				title.OnSelectNone -= KeywordSelectPanelOnSelectNone;

				title.Checked = selectAll;

				title.OnSelectAll += KeywordSelectPanelOnSelectAll;
				title.OnSelectNone += KeywordSelectPanelOnSelectNone;
			}
		}

		private void KeywordSelectPanelOnSelectAll(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (KeywordSelectPanel control in containerPanel.Controls)
			{
				control.Checked = true;
			}
			containerPanel.AutoScroll = true;
		}

		private void KeywordSelectPanelOnSelectNone(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (KeywordSelectPanel control in containerPanel.Controls)
			{
				control.Checked = false;
			}
			containerPanel.AutoScroll = true;
		}
	}

	public sealed class KeywordSelectPanel : Panel
	{
		public event EventHandler OnSelectAll;
		public event EventHandler OnSelectNone;
		public event EventHandler OnSelectChange;

		public Dictionary<String, String> Localization;

		private readonly CheckBox _checkBox;
		private readonly TextBox _keywordTextBox;
		private readonly ComboBox _conditionComboBox;
		private readonly ComboBox _equationComboBox;

		private Boolean _isTitle;
		public Boolean IsTitle
		{
			get { return _isTitle; }
			set
			{
				_isTitle = value;
				_keywordTextBox.Visible = 
				_conditionComboBox.Visible = _equationComboBox.Visible = !value;
			}
		}
		private POS_Exception.KeywordCriteria _keywordCriteria;
		public POS_Exception.KeywordCriteria KeywordCriteria
		{
			get { return _keywordCriteria; }
			set
			{
				_keywordCriteria = value;
				if (_keywordCriteria != null)
				{
					_isEditing = false;
					_keywordTextBox.Text = _keywordCriteria.Keyword;
					_conditionComboBox.SelectedItem = POS_Exception.Logics.ToString(_keywordCriteria.Condition);

					switch (_keywordCriteria.Equation)
					{
						case POS_Exception.Comparative.Equal:
							_equationComboBox.SelectedItem = "equal";
							break;

						case POS_Exception.Comparative.NotEqual:
							_equationComboBox.SelectedItem = "not equal";
							break;

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

		public KeywordSelectPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_Keyword", "Keyword"},
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
			_equationComboBox.Items.Add("include");//like
			_equationComboBox.Items.Add("exclude");//unlike
			_equationComboBox.Items.Add("equal");//=
			_equationComboBox.Items.Add("not equal");//<>
			_equationComboBox.SelectedIndex = 0;
			_equationComboBox.SelectedIndexChanged += EquationComboBoxSelectedIndexChanged;

			_keywordTextBox = new TextBox()
			{
				Location = new Point(130, 6),
				Size = new Size(125, 23),
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0)
			};
			_keywordTextBox.TextChanged += KeywordTextBoxChanged;

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
			Controls.Add(_keywordTextBox);
			//Controls.Add(_conditionComboBox);

			_checkBox.CheckedChanged += CheckBoxCheckedChanged;

			MouseClick += ExceptionSelectPanelMouseClick;
			Paint += TagSelectPanelPaint;
		}

		private Boolean _isEditing;

		private void KeywordTextBoxChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _keywordCriteria == null) return;

			_keywordCriteria.Keyword = _keywordTextBox.Text;
		}

		private void ConditionComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _keywordCriteria == null) return;

			_keywordCriteria.Condition = POS_Exception.Logics.ToIndex(_conditionComboBox.SelectedItem.ToString());
		}

		private void EquationComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _keywordCriteria == null) return;

			switch (_equationComboBox.SelectedItem.ToString())
			{
				case "include":
					_keywordCriteria.Equation = POS_Exception.Comparative.Include;
					break;

				case "exclude":
					_keywordCriteria.Equation = POS_Exception.Comparative.Exclude;
					break;

				case "equal":
					_keywordCriteria.Equation = POS_Exception.Comparative.Equal;
					break;

				case "not equal":
					_keywordCriteria.Equation = POS_Exception.Comparative.NotEqual;
					break;
			}
		}

		private void PaintTitle(Graphics g)
		{
			if (Width <= 200) return;
			Manager.PaintTitleText(g, Localization["PTSReports_Equation"]);

			g.DrawString(Localization["PTSReports_Keyword"], Manager.Font, Manager.TitleTextColor, 130, 13);

			//if (Width <= 300) return;
			//g.DrawString(Localization["PTSReports_Conditional"], Manager.Font, Manager.TitleTextColor, 230, 13);
		}

		private void TagSelectPanelPaint(Object sender, PaintEventArgs e)
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

		private void ExceptionSelectPanelMouseClick(Object sender, MouseEventArgs e)
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
