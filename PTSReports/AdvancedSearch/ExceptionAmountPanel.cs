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
	public sealed partial class ExceptionAmountPanel : UserControl
	{
		public IPTS PTS;
		public List<POS_Exception.ExceptionAmountCriteria> ExceptionAmountCriterias;

		public Dictionary<String, String> Localization;

		public ExceptionAmountPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_Manufacture", "Manufacture"},
								   {"PTSReports_AllException", "All Exception"},
								   {"PTSReports_SearchConditionsAssociated", "Search conditions associated"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.None;
			Name = "ExceptionAmount";
            BackgroundImage = Manager.BackgroundNoBorder;

			conditionalComboBox.Items.Add("AND");
			conditionalComboBox.Items.Add("OR");
			conditionalComboBox.SelectedIndex = 0;
			conditionalComboBox.SelectedIndexChanged += ConditionalComboBoxSelectedIndexChanged;

			conditionalDoubleBufferPanel.Paint += ConditionalDoubleBufferPanelPaint;

			filterComboBox.Items.Add(Localization["PTSReports_AllException"]);
			foreach (var manufacture in POS_Exception.Manufactures)
			{
				filterComboBox.Items.Add(POS_Exception.ToDisplay(manufacture));
			}
			filterPanel.Paint += FilterPanelPaint;
		}

		private void FilterComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			containerPanel.Visible = false;
			//ParseSetting();

			_exceptionSelectionList = GetExceptionList();

			var selectAll = true;
			foreach (ExceptionAmountSelectPanel exceptionAmountPanel in containerPanel.Controls)
			{
				if (exceptionAmountPanel.IsTitle) continue;

			    ExceptionAmountSelectPanel panel = exceptionAmountPanel;
			    if (_exceptionSelectionList.Any(item => item.Key == panel.Exception.Key && item.Value == panel.Exception.Value))
				{
					exceptionAmountPanel.Visible = true;
					if (selectAll && !exceptionAmountPanel.Checked)
						selectAll = false;
				}
				else
				{
					exceptionAmountPanel.Visible = false;
				}
                exceptionAmountPanel.Visible = (_exceptionSelectionList.Any(item => item.Key == panel.Exception.Key && item.Value == panel.Exception.Value));
			}

			var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as ExceptionAmountSelectPanel;
			if (title != null && title.IsTitle && title.Checked != selectAll)
			{
				title.OnSelectAll -= ExceptionAmountSelectPanelOnSelectAll;
				title.OnSelectNone -= ExceptionAmountSelectPanelOnSelectNone;

				title.Checked = selectAll;

				title.OnSelectAll += ExceptionAmountSelectPanelOnSelectAll;
				title.OnSelectNone += ExceptionAmountSelectPanelOnSelectNone;
			}

			containerPanel.Visible = true;
		}

		private List<POS_Exception.Exception> _exceptionSelectionList;
        private List<POS_Exception.Exception> GetExceptionList()
		{
            var list = new List<POS_Exception.Exception>();

			//all exception
			String[] manufactures;

			if (filterComboBox.SelectedIndex == 0)
			{
				manufactures = new String[POS_Exception.Manufactures.Length];
				Array.Copy(POS_Exception.Manufactures, manufactures, POS_Exception.Manufactures.Length);
			}
			else
			{
				manufactures = new[] { POS_Exception.ToIndex(filterComboBox.SelectedItem.ToString()) };
			}

			foreach (var manufacture in manufactures)
			{
				var newException = new POS_Exception { Manufacture = manufacture };
				POS_Exception.SetDefaultExceptions(newException);

				foreach (var obj in newException.Exceptions)
				{
					if (!list.Contains(obj))
						list.Add(obj);
				}

                if(manufacture == "Generic")
                {
                    foreach (KeyValuePair<UInt16, POS_Exception> posException in PTS.POS.Exceptions)
                    {
                        if(posException.Value.Manufacture != manufacture) continue;

                        foreach (POS_Exception.Exception exception in posException.Value.Exceptions)
                        {
                            if (!list.Contains(exception))
                                list.Add(exception);
                        }
                    }
                }
			}
			list.Sort((x, y) => (y.Value.CompareTo(x.Value)));

			return list;
		}

		private readonly Stack<ExceptionAmountSelectPanel> _exceptionAmountSelectPanel = new Stack<ExceptionAmountSelectPanel>();
		
		public void Initialize()
		{
			filterComboBox.SelectedIndexChanged -= FilterComboBoxSelectedIndexChanged;
			filterComboBox.SelectedIndex = 0;
			filterComboBox.SelectedIndexChanged += FilterComboBoxSelectedIndexChanged;

			conditionalComboBox.SelectedIndex = 0; //AND
			
			containerPanel.Controls.Clear();
			_exceptionAmountSelectPanel.Clear();

			_exceptionSelectionList = GetExceptionList();

			foreach (POS_Exception.Exception exception in _exceptionSelectionList)
			{
                var exist = false;
                foreach (ExceptionAmountSelectPanel control in containerPanel.Controls)
                {
                    if (control.IsTitle) continue;
                    if (control.Exception.Key == exception.Key && control.Exception.Value == exception.Value)
                        exist = true;
                }

                if (exist) continue;
				var exceptionSelectPanel = GetExceptionAmountSelectPanel();
			    exceptionSelectPanel.Exception = exception;
				exceptionSelectPanel.ExceptionAmountCriteria = new POS_Exception.ExceptionAmountCriteria
				{
					Exception = exception.Key,
					Amount = "0",
					Action = POS_Exception.Do.None,
					Condition = POS_Exception.Logic.AND,
					Equation = POS_Exception.Comparative.Greater,
					Keyword = "",
					KeywordEquation = POS_Exception.Comparative.Like,
				};
				containerPanel.Controls.Add(exceptionSelectPanel);
				_exceptionAmountSelectPanel.Push(exceptionSelectPanel);
			}

			var exceptionAmountSelectTitlePanel = GetExceptionAmountSelectPanel();
			exceptionAmountSelectTitlePanel.IsTitle = true;
			exceptionAmountSelectTitlePanel.Cursor = Cursors.Default;
			exceptionAmountSelectTitlePanel.Checked = false;
			exceptionAmountSelectTitlePanel.OnSelectAll += ExceptionAmountSelectPanelOnSelectAll;
			exceptionAmountSelectTitlePanel.OnSelectNone += ExceptionAmountSelectPanelOnSelectNone;
			containerPanel.Controls.Add(exceptionAmountSelectTitlePanel);
		}

		public void ParseSetting()
		{
			var isOr = false;
			foreach (var exceptionAmountCriteria in ExceptionAmountCriterias)
			{
				if (_exceptionAmountSelectPanel.Count == 0) break;

				var exceptionAmountSelectPanel = _exceptionAmountSelectPanel.Pop();
				exceptionAmountSelectPanel.ExceptionAmountCriteria = exceptionAmountCriteria;

				exceptionAmountSelectPanel.OnSelectChange -= ExceptionAmountSelectPanelOnSelectChange;
				exceptionAmountSelectPanel.Checked = true;
				exceptionAmountSelectPanel.OnSelectChange += ExceptionAmountSelectPanelOnSelectChange;

				if (!isOr)
					isOr = (exceptionAmountCriteria.Condition == POS_Exception.Logic.OR);
			}

			conditionalComboBox.SelectedIndexChanged += ConditionalComboBoxSelectedIndexChanged;
			conditionalComboBox.SelectedItem = POS_Exception.Logics.ToString(isOr ? POS_Exception.Logic.OR : POS_Exception.Logic.AND);
			conditionalComboBox.SelectedIndexChanged -= ConditionalComboBoxSelectedIndexChanged;

			if (_exceptionAmountSelectPanel.Count == 0 && containerPanel.Controls.Count > 0)
			{
				var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as ExceptionAmountSelectPanel;
				if (title != null && title.IsTitle)
				{
					title.OnSelectAll -= ExceptionAmountSelectPanelOnSelectAll;
					title.OnSelectNone -= ExceptionAmountSelectPanelOnSelectNone;

					title.Checked = true;

					title.OnSelectAll += ExceptionAmountSelectPanelOnSelectAll;
					title.OnSelectNone += ExceptionAmountSelectPanelOnSelectNone;
				}
			}
		}

		public void ScrollTop()
		{
			containerPanel.Select();
			containerPanel.AutoScrollPosition = new Point(0, 0);
		}

		private ExceptionAmountSelectPanel GetExceptionAmountSelectPanel()
		{
			var exceptionAmountSelectPanel = new ExceptionAmountSelectPanel
			{
				SelectionVisible = true,
			};

			exceptionAmountSelectPanel.OnSelectChange += ExceptionAmountSelectPanelOnSelectChange;

			return exceptionAmountSelectPanel;
		}

		private void ConditionalComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			var condition = POS_Exception.Logics.ToIndex(conditionalComboBox.SelectedItem.ToString());
			foreach (var exceptionAmountCriteria in ExceptionAmountCriterias)
			{
				exceptionAmountCriteria.Condition = condition;
			}
		}

		private void FilterPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintTop(g, filterPanel);

			if (filterPanel.Width <= 150) return;

			Manager.PaintText(g, Localization["PTSReports_Manufacture"]);
		}

		private void ConditionalDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			var g = e.Graphics;

			Manager.PaintBottom(g, conditionalDoubleBufferPanel);
			//Manager.PaintSingleInput(g, conditionalDoubleBufferPanel);
			if (conditionalDoubleBufferPanel.Width <= 150) return;

			Manager.PaintText(g, Localization["PTSReports_SearchConditionsAssociated"]);
		}

		private void ExceptionAmountSelectPanelOnSelectChange(Object sender, EventArgs e)
		{
			var panel = sender as ExceptionAmountSelectPanel;
			if (panel == null) return;
			if (panel.IsTitle) return;
			var condition = POS_Exception.Logics.ToIndex(conditionalComboBox.SelectedItem.ToString());

			var selectAll = false;
			if (panel.Checked)
			{
				panel.ExceptionAmountCriteria.Condition = condition;
				if (!ExceptionAmountCriterias.Contains(panel.ExceptionAmountCriteria))
					ExceptionAmountCriterias.Add(panel.ExceptionAmountCriteria);

				selectAll = true;
				foreach (ExceptionAmountSelectPanel control in containerPanel.Controls)
				{
					if (control.IsTitle) continue;
                    if (!control.Checked && control.Enabled && Enumerable.Any(_exceptionSelectionList, item => item.Key == panel.Exception.Key && item.Value == panel.Exception.Value))
					{
						selectAll = false;
						break;
					}
				}
			}
			else
			{
				ExceptionAmountCriterias.Remove(panel.ExceptionAmountCriteria);
			}

			var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as ExceptionAmountSelectPanel;
			if (title != null && title.IsTitle && title.Checked != selectAll)
			{
				title.OnSelectAll -= ExceptionAmountSelectPanelOnSelectAll;
				title.OnSelectNone -= ExceptionAmountSelectPanelOnSelectNone;

				title.Checked = selectAll;

				title.OnSelectAll += ExceptionAmountSelectPanelOnSelectAll;
				title.OnSelectNone += ExceptionAmountSelectPanelOnSelectNone;
			}
		}

		private void ExceptionAmountSelectPanelOnSelectAll(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (ExceptionAmountSelectPanel control in containerPanel.Controls)
			{
				if (control.ExceptionAmountCriteria != null)
				{
				    ExceptionAmountSelectPanel control1 = control;
				    if (!_exceptionSelectionList.Any(item => item.Key == control1.Exception.Key && item.Value == control1.Exception.Value)) continue;
				}

			    control.Checked = true;
			}
			containerPanel.AutoScroll = true;
		}

		private void ExceptionAmountSelectPanelOnSelectNone(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (ExceptionAmountSelectPanel control in containerPanel.Controls)
			{
				if (control.ExceptionAmountCriteria != null)
				{
				    ExceptionAmountSelectPanel control1 = control;
				    if (!_exceptionSelectionList.Any(item => item.Key == control1.Exception.Key && item.Value == control1.Exception.Value)) continue;
				}

			    control.Checked = false;
			}
			containerPanel.AutoScroll = true;
		}
	}

	public sealed class ExceptionAmountSelectPanel : Panel
	{
		public event EventHandler OnSelectAll;
		public event EventHandler OnSelectNone;
		public event EventHandler OnSelectChange;
	    public POS_Exception.Exception Exception;

		public Dictionary<String, String> Localization;

		private readonly CheckBox _checkBox;
		private readonly TextBox _exceptionTextBox;
		private readonly TextBox _amountTextBox;
		private readonly ComboBox _exceptionEquationComboBox;
		private readonly ComboBox _calculateComboBox;
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
				_amountTextBox.Visible = _calculateComboBox.Visible =
				_conditionComboBox.Visible = _equationComboBox.Visible =
					_keywordTextBox.Visible = _keywordEquationComboBox.Visible =
					_exceptionTextBox.Visible = _exceptionEquationComboBox.Visible = !value;
			}
		}
		private POS_Exception.ExceptionAmountCriteria _exceptionAmountCriteria;
		public POS_Exception.ExceptionAmountCriteria ExceptionAmountCriteria
		{
			get { return _exceptionAmountCriteria; }
			set
			{
				_exceptionAmountCriteria = value;
				if (_exceptionAmountCriteria != null)
				{
					_isEditing = false;
					_amountTextBox.Text = _exceptionAmountCriteria.Amount;

					switch (_exceptionAmountCriteria.Equation)
					{
						case POS_Exception.Comparative.Exists:
							_exceptionEquationComboBox.SelectedItem = "exists";
							break;

						case POS_Exception.Comparative.NotExists:
							_exceptionEquationComboBox.SelectedItem = "not exists";
							break;
					}

					_calculateComboBox.SelectedItem = POS_Exception.Dos.ToString(_exceptionAmountCriteria.Action);
					_exceptionTextBox.Text = Exception.Value;
					_conditionComboBox.SelectedItem = POS_Exception.Logics.ToString(_exceptionAmountCriteria.Condition);
					_equationComboBox.SelectedItem = POS_Exception.Comparatives.ToString(_exceptionAmountCriteria.Equation);
					_keywordTextBox.Text = _exceptionAmountCriteria.Keyword;

					switch (_exceptionAmountCriteria.KeywordEquation)
					{
						case POS_Exception.Comparative.To:
							_keywordEquationComboBox.SelectedItem = "to";
							break;

						case POS_Exception.Comparative.Include:
							_keywordEquationComboBox.SelectedItem = "include";
							break;

						case POS_Exception.Comparative.Exclude:
							_keywordEquationComboBox.SelectedItem = "exclude";
							break;
					}

					if (_exceptionAmountCriteria.Action == POS_Exception.Do.None)
					{
						_equationComboBox.Enabled =
						_amountTextBox.Enabled = false;
					}
					else
					{
						if (_exceptionAmountCriteria.ExceptionEquation == POS_Exception.Comparative.NotExists)
						{
							_calculateComboBox.SelectedIndex = 0;//when select <>, default disable compare exception amount
						}
						else
						{
							//if (_calculateComboBox.SelectedIndex == 0)
							//    _calculateComboBox.SelectedIndex = 1;
						}

						_equationComboBox.Enabled =
						_amountTextBox.Enabled = true;
					}

					_isEditing = true;
				}
			}
		}

		public ExceptionAmountSelectPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"POS_Exception", "Exception"},
								   {"PTSReports_Amount", "Amount"},
								   {"PTSReports_Calculate", "Calculate"},
								   {"PTSReports_Keyword", "Keyword"},
								   {"PTSReports_Conditional", "Conditional"},
								   {"PTSReports_Equation", "Equation"},
								   {"PTSReports_Comparison", "Comparison"},
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

			_exceptionEquationComboBox = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				Location = new Point(44, 6),
				Size = new Size(75, 23)
			};
			_exceptionEquationComboBox.Items.Add("exists");//=
			_exceptionEquationComboBox.Items.Add("not exists");//<>
			_exceptionEquationComboBox.SelectedIndex = 0;
			_exceptionEquationComboBox.SelectedIndexChanged += ExceptionEquationComboBoxSelectedIndexChanged;

			_exceptionTextBox = new PanelBase.HotKeyTextBox
			{
				Location = new Point(135, 6),
				Size = new Size(125, 23),
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0)
			};
			_exceptionTextBox.TextChanged += ExceptionTextBoxTextChanged;

			_calculateComboBox = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				Location = new Point(280, 6),
				Size = new Size(70, 23)
			};
			_calculateComboBox.Items.Add("");
			_calculateComboBox.Items.Add("SUM");
			_calculateComboBox.Items.Add("COUNT");
			_calculateComboBox.SelectedIndex = 0;
			_calculateComboBox.SelectedIndexChanged += CalculateComboBoxSelectedIndexChanged;

			_equationComboBox = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				Location = new Point(370, 6),
				Size = new Size(50, 23)
			};
			_equationComboBox.Items.Add("<");
			_equationComboBox.Items.Add("<=");
			_equationComboBox.Items.Add("=");
			_equationComboBox.Items.Add(">");
			_equationComboBox.Items.Add(">=");
			_equationComboBox.Items.Add("<>");
			_equationComboBox.SelectedIndex = 3;
			_equationComboBox.SelectedIndexChanged += EquationComboBoxSelectedIndexChanged;
			_equationComboBox.Enabled = false;

			_amountTextBox = new PanelBase.HotKeyTextBox
			{
				Location = new Point(440, 6),
				Size = new Size(75, 23),
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0)
			};
			_amountTextBox.KeyPress += NumberValidation;
			_amountTextBox.TextChanged += AmountTextBoxTextChanged;
			_amountTextBox.Enabled = false;

			_keywordEquationComboBox = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				Location = new Point(530, 6),
				Size = new Size(66, 23)
			};
			//_keywordEquationComboBox.Items.Add("to");
			_keywordEquationComboBox.Items.Add("include");
			_keywordEquationComboBox.Items.Add("exclude");
			_keywordEquationComboBox.SelectedIndex = 0;
			_keywordEquationComboBox.SelectedIndexChanged += KeywordEquationComboBoxSelectedIndexChanged;

			_keywordTextBox = new PanelBase.HotKeyTextBox
			{
				Location = new Point(615, 6),
				Size = new Size(90, 23),
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0)
			};
			_keywordTextBox.TextChanged += KeywordTextBoxTextChanged;

			_conditionComboBox = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				Location = new Point(710, 6),
				Size = new Size(50, 23)
			};
			_conditionComboBox.Items.Add("AND");
			_conditionComboBox.Items.Add("OR");
			_conditionComboBox.SelectedIndex = 1;
			_conditionComboBox.SelectedIndexChanged += ConditionComboBoxSelectedIndexChanged;

			Controls.Add(_checkBox);
			Controls.Add(_exceptionEquationComboBox);
			Controls.Add(_exceptionTextBox);
			Controls.Add(_calculateComboBox);
			Controls.Add(_equationComboBox);
			Controls.Add(_amountTextBox);
			Controls.Add(_keywordEquationComboBox);
			Controls.Add(_keywordTextBox);
			//Controls.Add(_conditionComboBox);

			_checkBox.CheckedChanged += CheckBoxCheckedChanged;

			MouseClick += ExceptionSelectPanelMouseClick;
			Paint += ExceptionAmountSelectPanelPaint;
		}

		private static void NumberValidation(object sender, KeyPressEventArgs e)
		{
			//                                                                                dot                       minus  -              backspace
			if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 46 || e.KeyChar == 45 || e.KeyChar == 8)
			{
				e.Handled = false;
			}
			else
			{
				e.Handled = true;
			}
		}

		private Boolean _isEditing;

		private void ExceptionTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _exceptionAmountCriteria == null) return;

			_exceptionAmountCriteria.Exception = _exceptionTextBox.Text;
		}

		private void AmountTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _exceptionAmountCriteria == null) return;

			_exceptionAmountCriteria.Amount = _amountTextBox.Text;
		}

		private void ExceptionEquationComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _exceptionAmountCriteria == null) return;

			switch (_exceptionEquationComboBox.SelectedItem.ToString())
			{
				case "exists":
					_exceptionAmountCriteria.ExceptionEquation = POS_Exception.Comparative.Exists;
					break;

				case "not exists":
					_exceptionAmountCriteria.ExceptionEquation = POS_Exception.Comparative.NotExists;
					break;
			}

			//dont change user's setting

			//if (_exceptionAmountCriteria.ExceptionEquation == POS_Exception.Comparative.NotExists)
			//{
			//    _calculateComboBox.SelectedIndex = 0;//when select <>, default disable compare exception amount
			//}
			//else
			//{
				//if (_calculateComboBox.SelectedIndex == 0)
				//    _calculateComboBox.SelectedIndex = 1;
			//}
		}

		private void CalculateComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _exceptionAmountCriteria == null) return;
			
			_exceptionAmountCriteria.Action = POS_Exception.Dos.ToIndex(_calculateComboBox.SelectedItem.ToString());

			if (_exceptionAmountCriteria.Action == POS_Exception.Do.None)
			{
				_equationComboBox.Enabled = 
				_amountTextBox.Enabled = false;
			}
			else
			{
				_equationComboBox.Enabled =
				_amountTextBox.Enabled = true;
			}
		}

		private void ConditionComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _exceptionAmountCriteria == null) return;

			_exceptionAmountCriteria.Condition = POS_Exception.Logics.ToIndex(_conditionComboBox.SelectedItem.ToString());
		}

		private void EquationComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _exceptionAmountCriteria == null) return;

			_exceptionAmountCriteria.Equation = POS_Exception.Comparatives.ToIndex(_equationComboBox.SelectedItem.ToString());
		}

		private void KeywordTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _exceptionAmountCriteria == null) return;

			_exceptionAmountCriteria.Keyword = _keywordTextBox.Text;
		}

		private void KeywordEquationComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _exceptionAmountCriteria == null) return;
			
			switch (_keywordEquationComboBox.SelectedItem.ToString())
			{
				case "to":
					_exceptionAmountCriteria.KeywordEquation = POS_Exception.Comparative.To;
					break;

				case "include":
					_exceptionAmountCriteria.KeywordEquation = POS_Exception.Comparative.Include;
					break;

				case "exclude":
					_exceptionAmountCriteria.KeywordEquation = POS_Exception.Comparative.Exclude;
					break;
			}
		}

		private void PaintTitle(Graphics g)
		{
			if (Width <= 250) return;
			Manager.PaintTitleText(g, Localization["PTSReports_Equation"]);

			g.DrawString(Localization["POS_Exception"], Manager.Font, Manager.TitleTextColor, 135, 13);

			if (Width <= 350) return;
			g.DrawString(Localization["PTSReports_Calculate"], Manager.Font, Manager.TitleTextColor, 280, 13);

			if (Width <= 450) return;
			g.DrawString(Localization["PTSReports_Comparison"], Manager.Font, Manager.TitleTextColor, 370, 13);

			if (Width <= 500) return;
			g.DrawString(Localization["PTSReports_Amount"], Manager.Font, Manager.TitleTextColor, 440, 13);

			if (Width <= 600) return;
			g.DrawString(Localization["PTSReports_Equation"], Manager.Font, Manager.TitleTextColor, 530, 13);

			if (Width <= 700) return;
			g.DrawString(Localization["PTSReports_Keyword"], Manager.Font, Manager.TitleTextColor, 615, 13);

			//if (Width <= 700) return;
			//g.DrawString(Localization["PTSReports_Conditional"], Manager.Font, Manager.TitleTextColor, 630, 13);
		}

		private void ExceptionAmountSelectPanelPaint(Object sender, PaintEventArgs e)
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
