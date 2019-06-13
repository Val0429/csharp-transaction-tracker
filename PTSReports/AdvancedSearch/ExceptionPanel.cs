using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports.AdvancedSearch
{
	public sealed partial class ExceptionPanel : UserControl
	{
		public IPTS PTS;
		public List<POS_Exception.ExceptionCriteria> ExceptionCriterias;

		public Dictionary<String, String> Localization;

		public ExceptionPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_SearchConditionsAssociated", "Search conditions associated"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.None;
			Name = "Exception";
            BackgroundImage = Manager.BackgroundNoBorder;

			conditionalComboBox.Items.Add("AND");
			conditionalComboBox.Items.Add("OR");
			conditionalComboBox.SelectedIndex = 0;
			conditionalComboBox.SelectedIndexChanged += ConditionalComboBoxSelectedIndexChanged;

			conditionalDoubleBufferPanel.Paint += ConditionalDoubleBufferPanelPaint;
		}

		public void Initialize()
		{
			containerPanel.Controls.Clear();

			conditionalComboBox.SelectedIndex = 0; //AND

			var list = new List<String>();


			foreach (String exception in list)
			{
				var exceptionSelectPanel = GetExceptionSelectPanel();
				exceptionSelectPanel.ExceptionCriteria = new POS_Exception.ExceptionCriteria
				{
					Exception = exception,
					Condition = POS_Exception.Logic.AND,
					Equation = POS_Exception.Comparative.Equal,
					Keyword = "",
					KeywordEquation = POS_Exception.Comparative.Like,
				};
				containerPanel.Controls.Add(exceptionSelectPanel);
			}

			var exceptionSelectTitlePanel = GetExceptionSelectPanel();
			exceptionSelectTitlePanel.IsTitle = true;
			exceptionSelectTitlePanel.Cursor = Cursors.Default;
			exceptionSelectTitlePanel.Checked = false;
			exceptionSelectTitlePanel.OnSelectAll += ExceptionSelectPanelOnSelectAll;
			exceptionSelectTitlePanel.OnSelectNone += ExceptionSelectPanelOnSelectNone;
			containerPanel.Controls.Add(exceptionSelectTitlePanel);
		}

		public void ScrollTop()
		{
			containerPanel.Select();
			containerPanel.AutoScrollPosition = new Point(0, 0);
		}

		private ExceptionSelectPanel GetExceptionSelectPanel()
		{
			var exceptionSelectPanel = new ExceptionSelectPanel
			{
				SelectionVisible = true,
			};

			exceptionSelectPanel.OnSelectChange += ExceptionSelectPanelOnSelectChange;

			return exceptionSelectPanel;
		}

		private void ConditionalComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			foreach (var exceptionCriteria in ExceptionCriterias)
			{
				exceptionCriteria.Condition = POS_Exception.Logics.ToIndex(conditionalComboBox.SelectedItem.ToString());
			}
		}

		private void ConditionalDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			var g = e.Graphics;

			Manager.PaintSingleInput(g, conditionalDoubleBufferPanel);
			if (conditionalDoubleBufferPanel.Width <= 150) return;

			Manager.PaintText(g, Localization["PTSReports_SearchConditionsAssociated"]);
		}

		private void ExceptionSelectPanelOnSelectChange(Object sender, EventArgs e)
		{
			var panel = sender as ExceptionSelectPanel;
			if (panel == null) return;
			if (panel.IsTitle) return;
			var condition = POS_Exception.Logics.ToIndex(conditionalComboBox.SelectedItem.ToString());

			var selectAll = false;
			if (panel.Checked)
			{
				if (!ExceptionCriterias.Contains(panel.ExceptionCriteria))
				{
					panel.ExceptionCriteria.Condition = condition;
					ExceptionCriterias.Add(panel.ExceptionCriteria);
				}

				selectAll = true;
				foreach (ExceptionSelectPanel control in containerPanel.Controls)
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
				ExceptionCriterias.Remove(panel.ExceptionCriteria);
			}

			var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as ExceptionSelectPanel;
			if (title != null && title.IsTitle && title.Checked != selectAll)
			{
				title.OnSelectAll -= ExceptionSelectPanelOnSelectAll;
				title.OnSelectNone -= ExceptionSelectPanelOnSelectNone;

				title.Checked = selectAll;

				title.OnSelectAll += ExceptionSelectPanelOnSelectAll;
				title.OnSelectNone += ExceptionSelectPanelOnSelectNone;
			}
		}

		private void ExceptionSelectPanelOnSelectAll(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (ExceptionSelectPanel control in containerPanel.Controls)
			{
				control.Checked = true;
			}
			containerPanel.AutoScroll = true;
		}

		private void ExceptionSelectPanelOnSelectNone(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (ExceptionSelectPanel control in containerPanel.Controls)
			{
				control.Checked = false;
			}
			containerPanel.AutoScroll = true;
		}
	}

	public sealed class ExceptionSelectPanel: Panel
	{
		public event EventHandler OnSelectAll;
		public event EventHandler OnSelectNone;
		public event EventHandler OnSelectChange;

		public Dictionary<String, String> Localization;

		private readonly CheckBox _checkBox;
		private readonly TextBox _exceptionTextBox;
		private readonly ComboBox _conditionComboBox;
		private readonly ComboBox _equationComboBox;
		private readonly TextBox _keywordTextBox;
		private readonly ComboBox _keywordEquationComboBox;

		private Boolean _isTitle;
		public Boolean IsTitle
		{
			get { return _isTitle; }
			set
			{
				_isTitle = value;
				_conditionComboBox.Visible = _equationComboBox.Visible = 
					_keywordTextBox.Visible = _keywordEquationComboBox.Visible =
					_exceptionTextBox.Visible = !value;
			}
		}
		private POS_Exception.ExceptionCriteria _exceptionCriteria;
		public POS_Exception.ExceptionCriteria ExceptionCriteria
		{
			get { return _exceptionCriteria; }
			set
			{
				_exceptionCriteria = value;
				if (_exceptionCriteria != null)
				{
					_isEditing = false;
					_exceptionTextBox.Text = _exceptionCriteria.Exception;
					_conditionComboBox.SelectedItem = POS_Exception.Logics.ToString(_exceptionCriteria.Condition);

					switch (_exceptionCriteria.Equation)
					{
						case POS_Exception.Comparative.Exists:
							_equationComboBox.SelectedItem = "exists";
							break;

						case POS_Exception.Comparative.NotExists:
							_equationComboBox.SelectedItem = "not exists";
							break;
					}

					_keywordTextBox.Text = _exceptionCriteria.Keyword;

					switch (_exceptionCriteria.KeywordEquation)
					{
						case POS_Exception.Comparative.Include:
							_keywordEquationComboBox.SelectedItem = "include";
							break;

						case POS_Exception.Comparative.Exclude:
							_keywordEquationComboBox.SelectedItem = "exclude";
							break;
					}
					_isEditing = true;
				}
			}
		}

		public ExceptionSelectPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"POS_Exception", "Exception"},
								   {"PTSReports_Keyword", "Keyword"},
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

			_equationComboBox = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				Location = new Point(44, 6),
				Size = new Size(70, 23)
			};
			_equationComboBox.Items.Add("exists");//=
			_equationComboBox.Items.Add("not exists");//<>
			_equationComboBox.SelectedIndex = 0;
			_equationComboBox.SelectedIndexChanged += EquationComboBoxSelectedIndexChanged;

			_exceptionTextBox = new PanelBase.HotKeyTextBox
			{
				Location = new Point(120, 6),
				Size = new Size(125, 23),
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0)
			};
			_exceptionTextBox.TextChanged += ExceptionTextBoxTextChanged;

			_keywordEquationComboBox = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				Location = new Point(270, 6),
				Size = new Size(60, 23)
			};
			_keywordEquationComboBox.Items.Add("include");
			_keywordEquationComboBox.Items.Add("exclude");
			_keywordEquationComboBox.SelectedIndex = 0;
			_keywordEquationComboBox.SelectedIndexChanged += KeywordEquationComboBoxSelectedIndexChanged;

			_keywordTextBox = new PanelBase.HotKeyTextBox
			{
				Location = new Point(340, 6),
				Size = new Size(90, 23),
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0)
			};
			_keywordTextBox.TextChanged += KeywordTextBoxTextChanged;

			_conditionComboBox = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				Location = new Point(450, 6),
				Size = new Size(50, 23)
			};
			_conditionComboBox.Items.Add("AND");
			_conditionComboBox.Items.Add("OR");
			_conditionComboBox.SelectedIndex = 1;
			_conditionComboBox.SelectedIndexChanged += ConditionComboBoxSelectedIndexChanged;

			Controls.Add(_checkBox);
			Controls.Add(_equationComboBox);
			Controls.Add(_exceptionTextBox);
			Controls.Add(_keywordEquationComboBox);
			Controls.Add(_keywordTextBox);
			//Controls.Add(_conditionComboBox);

			_checkBox.CheckedChanged += CheckBoxCheckedChanged;

			MouseClick += ExceptionSelectPanelMouseClick;
			Paint += ExceptionSelectPanelPaint;
		}
		
		private Boolean _isEditing;

		private void ExceptionTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _exceptionCriteria == null) return;

			_exceptionCriteria.Exception = _exceptionTextBox.Text;
		}

		private void ConditionComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _exceptionCriteria == null) return;

			_exceptionCriteria.Condition = POS_Exception.Logics.ToIndex(_conditionComboBox.SelectedItem.ToString());
		}

		private void EquationComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _exceptionCriteria == null) return;

			switch (_equationComboBox.SelectedItem.ToString())
			{
				case "exists":
					_exceptionCriteria.Equation = POS_Exception.Comparative.Exists;
					break;

				case "not exists":
					_exceptionCriteria.Equation = POS_Exception.Comparative.NotExists;
					break;
			}
		}

		private void KeywordTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _exceptionCriteria == null) return;

			_exceptionCriteria.Keyword = _keywordTextBox.Text;
		}

		private void KeywordEquationComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _exceptionCriteria == null) return;

			switch (_keywordEquationComboBox.SelectedItem.ToString())
			{
				case "include":
					_exceptionCriteria.KeywordEquation = POS_Exception.Comparative.Include;
					break;

				case "exclude":
					_exceptionCriteria.KeywordEquation = POS_Exception.Comparative.Exclude;
					break;
			}
		}
		
		private void PaintTitle(Graphics g)
		{
			if (Width <= 200) return;
			Manager.PaintTitleText(g, Localization["PTSReports_Equation"]);

			g.DrawString(Localization["POS_Exception"], Manager.Font, Manager.TitleTextColor, 120, 13);

			if (Width <= 300) return;
			g.DrawString(Localization["PTSReports_Equation"], Manager.Font, Manager.TitleTextColor, 270, 13);

			if (Width <= 400) return;
			g.DrawString(Localization["PTSReports_Keyword"], Manager.Font, Manager.TitleTextColor, 340, 13);

			//if (Width <= 500) return;
			//g.DrawString(Localization["PTSReports_Conditional"], Manager.Font, Manager.TitleTextColor, 450, 13);
		}

		private void ExceptionSelectPanelPaint(Object sender, PaintEventArgs e)
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
