using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports.AdvancedSearch
{
	public sealed partial class TagPanel : UserControl
	{
		public IPTS PTS;
		public List<POS_Exception.TagCriteria> TagCriterias;

		public Dictionary<String, String> Localization;

		public TagPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_Manufacture", "Manufacture"},
								   {"PTSReports_AllTag", "All Tag"},
								   {"PTSReports_SearchConditionsAssociated", "Search conditions associated"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.None;
			Name = "Tag";
            BackgroundImage = Manager.BackgroundNoBorder;

			conditionalComboBox.Items.Add("AND");
			conditionalComboBox.Items.Add("OR");
			conditionalComboBox.SelectedIndex = 0;
			conditionalComboBox.SelectedIndexChanged += ConditionalComboBoxSelectedIndexChanged;

			conditionalDoubleBufferPanel.Paint += ConditionalDoubleBufferPanelPaint;

			filterComboBox.Items.Add(Localization["PTSReports_AllTag"]);
			foreach (var manufacture in POS_Exception.Manufactures)
			{
				filterComboBox.Items.Add(POS_Exception.ToDisplay(manufacture));
			}
			filterPanel.Paint += FilterPanelPaint;
		}

        private Boolean _tagSelectionListContains(String tag)
        {
            foreach (ComboxItem item in _tagSelectionList)
            {
                if(item.Value == tag)
                {
                    return true;
                }
            }

            return false;
        }

		private void FilterComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			containerPanel.Visible = false;
			//ParseSetting();

			_tagSelectionList = GetTagList();

			var selectAll = true;
			foreach (TagSelectPanel tagSelectPanel in containerPanel.Controls)
			{
				if (tagSelectPanel.IsTitle) continue;

                if (_tagSelectionListContains(tagSelectPanel.TagCriteria.TagName))
				{
					tagSelectPanel.Visible = true;
					if (selectAll && !tagSelectPanel.Checked)
						selectAll = false;
				}
				else
				{
					tagSelectPanel.Visible = false;
				}
                tagSelectPanel.Visible = (_tagSelectionListContains(tagSelectPanel.TagCriteria.TagName));
			}

			var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as TagSelectPanel;
			if (title != null && title.IsTitle && title.Checked != selectAll)
			{
				title.OnSelectAll -= TagSelectPanelOnSelectAll;
				title.OnSelectNone -= TagSelectPanelOnSelectNone;

				title.Checked = selectAll;

				title.OnSelectAll += TagSelectPanelOnSelectAll;
				title.OnSelectNone += TagSelectPanelOnSelectNone;
			}

			containerPanel.Visible = true;
		}

        private List<ComboxItem> _tagSelectionList;
		private List<ComboxItem> GetTagList()
		{
            var list = new Dictionary<String, ComboxItem>();

			//all tag
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
				POS_Exception.SetDefaultTags(newException);

				foreach (var obj in newException.Tags)
				{
					if (!list.ContainsKey(obj.Value))
                        list.Add(obj.Value, new ComboxItem(manufacture, obj.Value));
				}

                if (manufacture == "Generic")
                {
                    foreach (KeyValuePair<UInt16, POS_Exception> posException in PTS.POS.Exceptions)
                    {
                        if (posException.Value.Manufacture != manufacture) continue;

                        foreach (POS_Exception.Tag tag in posException.Value.Tags)
                        {
                            if (!list.ContainsKey(tag.Value))
                                list.Add(tag.Value, new ComboxItem(manufacture, tag.Value));
                        }
                    }
                }
			}

		    var result = new List<ComboxItem>(list.Values);
            result.Sort((x, y) => (y.Value.CompareTo(x.Value)));

            return result;
		}

		private readonly Stack<TagSelectPanel> _tagSelectPanel = new Stack<TagSelectPanel>();
		public void Initialize()
		{
			containerPanel.Controls.Clear();

			filterComboBox.SelectedIndexChanged -= FilterComboBoxSelectedIndexChanged;
			filterComboBox.SelectedIndex = 0;
			filterComboBox.SelectedIndexChanged += FilterComboBoxSelectedIndexChanged;

			conditionalComboBox.SelectedIndex = 0; //AND

			_tagSelectionList = GetTagList();

			foreach (ComboxItem item in _tagSelectionList)
			{
				var tagSelectPanel = GetTagSelectPanel();
				tagSelectPanel.TagCriteria = new POS_Exception.TagCriteria
				{
                    Manufacture = item.Text,
					TagName = item.Value,
					Value = "",
					Action = POS_Exception.Do.None,
					Condition = POS_Exception.Logic.AND,
					Equation = POS_Exception.Comparative.Equal,
				};
				containerPanel.Controls.Add(tagSelectPanel);
				_tagSelectPanel.Push(tagSelectPanel);
			}

			var tagSelectTitlePanel = GetTagSelectPanel();
			tagSelectTitlePanel.IsTitle = true;
			tagSelectTitlePanel.Cursor = Cursors.Default;
			tagSelectTitlePanel.Checked = false;
			tagSelectTitlePanel.OnSelectAll += TagSelectPanelOnSelectAll;
			tagSelectTitlePanel.OnSelectNone += TagSelectPanelOnSelectNone;
			
			containerPanel.Controls.Add(tagSelectTitlePanel);
		}

		public void ParseSetting()
		{
			var isOr = false;
			foreach (var tagCriteria in TagCriterias)
			{
				if (_tagSelectPanel.Count == 0) break;

				var tagSelectPanel = _tagSelectPanel.Pop();
				tagSelectPanel.TagCriteria = tagCriteria;

				tagSelectPanel.OnSelectChange -= TagSelectPanelOnSelectChange;
				tagSelectPanel.Checked = true;
				tagSelectPanel.OnSelectChange += TagSelectPanelOnSelectChange;

				if (!isOr)
					isOr = (tagCriteria.Condition == POS_Exception.Logic.OR);
			}

			conditionalComboBox.SelectedIndexChanged += ConditionalComboBoxSelectedIndexChanged;
			conditionalComboBox.SelectedItem = POS_Exception.Logics.ToString(isOr ? POS_Exception.Logic.OR : POS_Exception.Logic.AND);
			conditionalComboBox.SelectedIndexChanged -= ConditionalComboBoxSelectedIndexChanged;

			if (_tagSelectPanel.Count == 0 && containerPanel.Controls.Count > 0)
			{
				var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as TagSelectPanel;
				if (title != null && title.IsTitle)
				{
					title.OnSelectAll -= TagSelectPanelOnSelectAll;
					title.OnSelectNone -= TagSelectPanelOnSelectNone;

					title.Checked = true;

					title.OnSelectAll += TagSelectPanelOnSelectAll;
					title.OnSelectNone += TagSelectPanelOnSelectNone;
				}
			}
		}

		public void ScrollTop()
		{
			containerPanel.Select();
			containerPanel.AutoScrollPosition = new Point(0, 0);
		}

		private TagSelectPanel GetTagSelectPanel()
		{
			var tagSelectPanel = new TagSelectPanel
			{
				SelectionVisible = true,
			};

			tagSelectPanel.OnSelectChange += TagSelectPanelOnSelectChange;

			return tagSelectPanel;
		}

		private void ConditionalComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			foreach (var tagCriteria in TagCriterias)
			{
				tagCriteria.Condition = POS_Exception.Logics.ToIndex(conditionalComboBox.SelectedItem.ToString());
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
			if (conditionalDoubleBufferPanel.Width <= 150) return;

			Manager.PaintText(g, Localization["PTSReports_SearchConditionsAssociated"]);
		}

		private void TagSelectPanelOnSelectChange(Object sender, EventArgs e)
		{
			var panel = sender as TagSelectPanel;
			if (panel == null) return;
			if (panel.IsTitle) return;
			var condition = POS_Exception.Logics.ToIndex(conditionalComboBox.SelectedItem.ToString());

			var selectAll = false;
			if (panel.Checked)
			{
				if (!TagCriterias.Contains(panel.TagCriteria))
				{
					panel.TagCriteria.Condition = condition;
					TagCriterias.Add(panel.TagCriteria);
				}

				selectAll = true;
				foreach (TagSelectPanel control in containerPanel.Controls)
				{
					if (control.IsTitle) continue;
                    if (!control.Checked && control.Enabled && _tagSelectionListContains(control.TagCriteria.TagName))
					{
						selectAll = false;
						break;
					}
				}
			}
			else
			{
				TagCriterias.Remove(panel.TagCriteria);
			}

			var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as TagSelectPanel;
			if (title != null && title.IsTitle && title.Checked != selectAll)
			{
				title.OnSelectAll -= TagSelectPanelOnSelectAll;
				title.OnSelectNone -= TagSelectPanelOnSelectNone;

				title.Checked = selectAll;

				title.OnSelectAll += TagSelectPanelOnSelectAll;
				title.OnSelectNone += TagSelectPanelOnSelectNone;
			}
		}

		private void TagSelectPanelOnSelectAll(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (TagSelectPanel control in containerPanel.Controls)
			{
				if (control.TagCriteria != null)
				{
                    if (!_tagSelectionListContains(control.TagCriteria.TagName)) continue;
				}

				control.Checked = true;
			}
			containerPanel.AutoScroll = true;
		}

		private void TagSelectPanelOnSelectNone(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (TagSelectPanel control in containerPanel.Controls)
			{
				if (control.TagCriteria != null)
				{
                    if (!_tagSelectionListContains(control.TagCriteria.TagName)) continue;
				}

				control.Checked = false;
			}
			containerPanel.AutoScroll = true;
		}
	}

	public sealed class TagSelectPanel : Panel
	{
		public event EventHandler OnSelectAll;
		public event EventHandler OnSelectNone;
		public event EventHandler OnSelectChange;

		public Dictionary<String, String> Localization;

		private readonly CheckBox _checkBox;
		private readonly TextBox _tagTextBox;
		private readonly TextBox _valueTextBox;
		private readonly ComboBox _calculateComboBox;
		private readonly ComboBox _conditionComboBox;
		private readonly ComboBox _equationComboBox;

		private Boolean _isTitle;
		public Boolean IsTitle
		{
			get { return _isTitle; }
			set
			{
				_isTitle = value;
				_valueTextBox.Visible = _calculateComboBox.Visible =
				_conditionComboBox.Visible = _equationComboBox.Visible =
					_tagTextBox.Visible = !value;
			}
		}
		private POS_Exception.TagCriteria _tagCriteria;
		public POS_Exception.TagCriteria TagCriteria
		{
			get { return _tagCriteria; }
			set
			{
				_tagCriteria = value;
				if (_tagCriteria != null)
				{
					_isEditing = false;
					_tagTextBox.Text = _tagCriteria.TagName;
					_valueTextBox.Text = _tagCriteria.Value;
					_calculateComboBox.SelectedItem = POS_Exception.Dos.ToString(_tagCriteria.Action);
					_conditionComboBox.SelectedItem = POS_Exception.Logics.ToString(_tagCriteria.Condition);
					_equationComboBox.SelectedItem = POS_Exception.Comparatives.ToString(_tagCriteria.Equation);
					_isEditing = true;
				}
			}
		}

		public TagSelectPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_Tag", "Tag"},
								   {"PTSReports_Value", "Value"},
								   {"PTSReports_Calculate", "Calculate"},
								   {"PTSReports_Conditional", "Conditional"},
								   {"PTSReports_Comparison", "Comparison"},
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

			_tagTextBox = new PanelBase.HotKeyTextBox
			{
				Location = new Point(44, 6),
				Size = new Size(125, 23),
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0)
			};
			_tagTextBox.TextChanged += TagTextBoxTextChanged;

			_calculateComboBox = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				Location = new Point(190, 6),
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
				Location = new Point(280, 6),
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

			_valueTextBox = new PanelBase.HotKeyTextBox
			{
				Location = new Point(360, 6),
				Size = new Size(75, 23),
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0)
			};
			_valueTextBox.TextChanged += ValueTextBoxTextChanged;

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
			Controls.Add(_tagTextBox);
			Controls.Add(_calculateComboBox);
			Controls.Add(_equationComboBox);
			Controls.Add(_valueTextBox);
			//Controls.Add(_conditionComboBox);

			_checkBox.CheckedChanged += CheckBoxCheckedChanged;

			MouseClick += TagSelectPanelMouseClick;
			Paint += TagSelectPanelPaint;
		}

		private Boolean _isEditing;

		private void TagTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _tagCriteria == null) return;

			_tagCriteria.TagName = _tagTextBox.Text;
		}

		private void ValueTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _tagCriteria == null) return;

			_tagCriteria.Value = _valueTextBox.Text;
		}

		private void CalculateComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _tagCriteria == null) return;

			_tagCriteria.Action = POS_Exception.Dos.ToIndex(_calculateComboBox.SelectedItem.ToString());
		}

		private void ConditionComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _tagCriteria == null) return;

			_tagCriteria.Condition = POS_Exception.Logics.ToIndex(_conditionComboBox.SelectedItem.ToString());
		}

		private void EquationComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!_isEditing || _tagCriteria == null) return;

			_tagCriteria.Equation = POS_Exception.Comparatives.ToIndex(_equationComboBox.SelectedItem.ToString());
		}

		private void PaintTitle(Graphics g)
		{
			if (Width <= 200) return;
			Manager.PaintTitleText(g, Localization["PTSReports_Tag"]);

			if (Width <= 300) return;
			g.DrawString(Localization["PTSReports_Calculate"], Manager.Font, Manager.TitleTextColor, 190, 13);

			if (Width <= 400) return;
			g.DrawString(Localization["PTSReports_Comparison"], Manager.Font, Manager.TitleTextColor, 280, 13);

			if (Width <= 510) return;
			g.DrawString(Localization["PTSReports_Value"], Manager.Font, Manager.TitleTextColor, 360, 13);

			//if (Width <= 600) return;
			//g.DrawString(Localization["PTSReports_Conditional"], Manager.Font, Manager.TitleTextColor, 450, 13);
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

		private void TagSelectPanelMouseClick(Object sender, MouseEventArgs e)
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
