using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;
using Enumerable = System.Linq.Enumerable;

namespace PTSReports.Base
{
	public sealed partial class ExceptionPanel : UserControl
	{
		public event EventHandler OnSelectChange;

		public IPTS PTS;
		public Dictionary<String, String> Localization;
		public POS_Exception.SearchCriteria SearchCriteria;
		public Boolean ShowExceptionColor;

		public ExceptionPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_Manufacture", "Manufacture"},
								   {"PTSReports_AllException", "All Exception"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.None;
			Name = "Exception";

            BackgroundImage = Manager.BackgroundNoBorder;

			filterComboBox.Items.Add(Localization["PTSReports_AllException"]);
			foreach (var manufacture in POS_Exception.Manufactures)
			{
				filterComboBox.Items.Add(POS_Exception.ToDisplay(manufacture));
			}
			filterComboBox.SelectedIndex = 0;
			filterComboBox.SelectedIndexChanged += FilterComboBoxSelectedIndexChanged;
			filterPanel.Paint += FilterPanelPaint;
		}

		private void FilterComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			_exceptionSelectionList = GetExceptionList();

			containerPanel.Visible = false;

			FilterExceptionList();

			containerPanel.Visible = true;
		}

		private void FilterExceptionList()
		{

			var selectAll = true;
			foreach (StringPanel stringPanel in containerPanel.Controls)
			{
				if (stringPanel.IsTitle) continue;

			    StringPanel panel = stringPanel;
			    if (Enumerable.Any(_exceptionSelectionList, item => item.Key == panel.Exception.Key && item.Value == panel.Exception.Value))
				{
					stringPanel.Visible = true;
					if (selectAll && !stringPanel.Checked)
						selectAll = false;
				}
				else
				{
					stringPanel.Visible = false;
				}
			}

			var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as StringPanel;
			if (title != null && title.IsTitle && title.Checked != selectAll)
			{
				title.OnSelectAll -= StringPanelOnSelectAll;
				title.OnSelectNone -= StringPanelOnSelectNone;

				title.Checked = selectAll;

				title.OnSelectAll += StringPanelOnSelectAll;
				title.OnSelectNone += StringPanelOnSelectNone;
			}
		}

		public void FilterExceptionByManufacture(String manufacture)
		{
			_exceptionSelectionList = GetExceptionList(manufacture);
			FilterExceptionList();
		}

		private List<POS_Exception.Exception> _exceptionSelectionList;
        private List<POS_Exception.Exception> GetExceptionList(String brand = null)
		{
            var list = new List<POS_Exception.Exception>();

			//all exception
			String[] manufactures;

			if (brand == null)
			{
				if (filterComboBox.SelectedIndex == 0)
				{
					manufactures = new String[POS_Exception.Manufactures.Length];
					Array.Copy(POS_Exception.Manufactures, manufactures, POS_Exception.Manufactures.Length);
				}
				else
				{
					manufactures = new[] { POS_Exception.ToIndex(filterComboBox.SelectedItem.ToString()) };
				}
			}
			else
			{
				if (brand == "")
				{
					manufactures = new String[POS_Exception.Manufactures.Length];
					Array.Copy(POS_Exception.Manufactures, manufactures, POS_Exception.Manufactures.Length);
				}
				else
				{
					manufactures = new[] { POS_Exception.ToIndex(brand) };
				}
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

                if (manufacture == "Generic")
                {
                    foreach (KeyValuePair<UInt16, POS_Exception> posException in PTS.POS.Exceptions)
                    {
                        if (posException.Value.Manufacture != manufacture) continue;

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

        private IEnumerable<POS_Exception.Exception> GetAllExceptionList()
		{
            var list = new List<POS_Exception.Exception>();

			var manufactures = new String[POS_Exception.Manufactures.Length];
			Array.Copy(POS_Exception.Manufactures, manufactures, POS_Exception.Manufactures.Length);

			foreach (var manufacture in manufactures)
			{
				var newException = new POS_Exception { Manufacture = manufacture };
				POS_Exception.SetDefaultExceptions(newException);

				foreach (var obj in newException.Exceptions)
				{
                    if (!list.Contains(obj))
                        list.Add(obj);
				}

                if (manufacture == "Generic")
                {
                    foreach (KeyValuePair<UInt16, POS_Exception> posException in PTS.POS.Exceptions)
                    {
                        if (posException.Value.Manufacture != manufacture) continue;

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

		public void ResetManufacture()
		{
			filterComboBox.SelectedIndexChanged -= FilterComboBoxSelectedIndexChanged;
			filterComboBox.SelectedIndex = 0;
			filterComboBox.SelectedIndexChanged += FilterComboBoxSelectedIndexChanged;
		}

		private Boolean _isEditing;
		public void ParseSetting()
		{
			_isEditing = false;

			ClearViewModel();

			var selectAll = true;
			var count = 0;
			//containerPanel.Visible = false;

			_exceptionSelectionList = GetExceptionList();

			var allException = GetAllExceptionList();
			foreach (POS_Exception.Exception exception in allException)
			{
			    var exist = false;
                foreach (StringPanel control in containerPanel.Controls)
                {
                    if (control.IsTitle) continue;
                    if (control.Exception.Key == exception.Key && control.Exception.Value == exception.Value)
                        exist = true;
                }

                if(exist) continue;

				var stringPanel = GetStringPanel();

				stringPanel.Exception = exception;
                stringPanel.Visible = ((Enumerable.Any(_exceptionSelectionList, item => item.Key == stringPanel.Exception.Key && item.Value == stringPanel.Exception.Value)));

				if (ShowExceptionColor)
				{
					stringPanel.ShowExceptionColor = true;
                    stringPanel.ExceptionThreshold = PTS.POS.ExceptionThreshold.ContainsKey(exception.Key)
                                                         ? PTS.POS.ExceptionThreshold[exception.Key]
														 : null;
					stringPanel.RefreshColor();
				}
				if (SearchCriteria.Exceptions.Contains(exception.Key))
				{
					count++;
					stringPanel.Checked = true;
				}
                else
				{
				    POS_Exception.Exception exception1 = exception;
				    if (Enumerable.Any(_exceptionSelectionList, item => item.Key == exception1.Key && item.Value == exception1.Value))
				        selectAll = false;
				}

                
			    containerPanel.Controls.Add(stringPanel);
			}
			if (count == 0 && selectAll)
				selectAll = false;
			
			var stringTitlePanel = GetStringPanel();
			stringTitlePanel.IsTitle = true;
			stringTitlePanel.ShowExceptionColor = ShowExceptionColor;
			stringTitlePanel.Cursor = Cursors.Default;
			stringTitlePanel.Checked = selectAll;
			stringTitlePanel.OnSelectAll += StringPanelOnSelectAll;
			stringTitlePanel.OnSelectNone += StringPanelOnSelectNone;
			containerPanel.Controls.Add(stringTitlePanel);

			containerPanel.Select();
			containerPanel.AutoScrollPosition = new Point(0, 0);

			_isEditing = true;
		}

		private void FilterPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintSingleInput(g, filterPanel);

			if (filterPanel.Width <= 100) return;

			Manager.PaintText(g, Localization["PTSReports_Manufacture"]);
		}

		private readonly Queue<StringPanel> _recycleString = new Queue<StringPanel>();
		private StringPanel GetStringPanel()
		{
			if (_recycleString.Count > 0)
			{
				return _recycleString.Dequeue();
			}

			var stringPanel = new StringPanel
			{
				SelectionVisible = true,
			};

			stringPanel.OnSelectChange += StringPanelOnSelectChange;

			return stringPanel;
		}

		private void StringPanelOnSelectChange(Object sender, EventArgs e)
		{
			if(!_isEditing) return;

			var panel = sender as StringPanel;
			if (panel == null) return;
			if (panel.IsTitle) return;

			var selectAll = false;
			if (panel.Checked)
			{
				if (!SearchCriteria.Exceptions.Contains(panel.Exception.Key))
				{
					if (!String.IsNullOrEmpty(panel.Exception.Key))
						SearchCriteria.Exceptions.Add(panel.Exception.Key);
				}

				selectAll = true;
				foreach (StringPanel control in containerPanel.Controls)
				{
					if (control.IsTitle) continue;
				    StringPanel control1 = control;
				    if (!control.Checked && control.Enabled && (Enumerable.Any(_exceptionSelectionList, item => item.Key == control1.Exception.Key && item.Value == control1.Exception.Value)))
					{
						selectAll = false;
						break;
					}
				}
			}
			else
			{
				SearchCriteria.Exceptions.Remove(panel.Exception.Key);
			}

			if (OnSelectChange != null)
				OnSelectChange(null, null);

			var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as StringPanel;
			if (title != null && title.IsTitle && title.Checked != selectAll)
			{
				title.OnSelectAll -= StringPanelOnSelectAll;
				title.OnSelectNone -= StringPanelOnSelectNone;

				title.Checked = selectAll;

				title.OnSelectAll += StringPanelOnSelectAll;
				title.OnSelectNone += StringPanelOnSelectNone;
			}
		}

		private void ClearViewModel()
		{
			foreach (StringPanel stringPanel in containerPanel.Controls)
			{
				stringPanel.SelectionVisible = false;
				stringPanel.Visible = true;

				stringPanel.Checked = false;
				stringPanel.Exception = null;
				stringPanel.Cursor = Cursors.Hand;
				stringPanel.SelectionVisible = true;

				if (stringPanel.IsTitle)
				{
					stringPanel.OnSelectAll -= StringPanelOnSelectAll;
					stringPanel.OnSelectNone -= StringPanelOnSelectNone;
					stringPanel.IsTitle = false;
				}

				if (!_recycleString.Contains(stringPanel))
				{
					_recycleString.Enqueue(stringPanel);
				}
			}
			containerPanel.Controls.Clear();
		}

		private void StringPanelOnSelectAll(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (StringPanel control in containerPanel.Controls)
			{
				if (!control.IsTitle)
				{
				    StringPanel control1 = control;
                    if (control1.Exception == null) continue; 
				    if (!Enumerable.Any(_exceptionSelectionList, item => item.Key == control1.Exception.Key && item.Value == control1.Exception.Value)) continue;
				}

			    control.Checked = true;
			}
			containerPanel.AutoScroll = true;
		}

		private void StringPanelOnSelectNone(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (StringPanel control in containerPanel.Controls)
			{
				if (!control.IsTitle)
				{
                    StringPanel control1 = control;
                    if (control1.Exception==null) continue; 
                    if (!Enumerable.Any(_exceptionSelectionList, item => item.Key == control1.Exception.Key && item.Value == control1.Exception.Value)) continue;
				}

				control.Checked = false;
			}
			containerPanel.AutoScroll = true;
		}

		public void ScrollTop()
		{
			containerPanel.Select();
			containerPanel.AutoScrollPosition = new Point(0, 0);
		}

		public Panel ContainerPanel
		{
			get { return containerPanel; }
		}
	}

	public sealed class StringPanel: Panel
	{
		public event EventHandler OnSelectAll;
		public event EventHandler OnSelectNone;
		public event EventHandler OnSelectChange;

		public Dictionary<String, String> Localization;

		private readonly CheckBox _checkBox = new CheckBox();
		private readonly Panel _colorPanel = new Panel();

		public Boolean IsTitle;
		public POS_Exception.Exception Exception;
	    public String Manufacture;
		public POS_Exception.ExceptionThreshold ExceptionThreshold;
		private Boolean _showExceptionColor;
		public Boolean ShowExceptionColor
		{
			get { return _showExceptionColor; }
			set
			{
				_showExceptionColor = value;
				if (value && !IsTitle)
				{
					Controls.Add(_colorPanel);
				}
				else
				{
					Controls.Remove(_colorPanel);
				}
			}
		}
		public StringPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"POS_Exception", "Exception"},
								   {"POS_ExceptionColor", "Exception Color"},
							   };
			Localizations.Update(Localization);

			DoubleBuffered = true;
			Dock = DockStyle.Top;
			Cursor = Cursors.Hand;
			Height = 40;

			BackColor = Color.Transparent;

			_checkBox.Padding = new Padding(10, 0, 0, 0);
			_checkBox.Dock = DockStyle.Left;
			_checkBox.Width = 25;

			_colorPanel.Padding = new Padding(0, 0, 0, 0);
			_colorPanel.Margin = new Padding(0, 0, 0, 0);
			_colorPanel.Location = new Point(250, 8);
			_colorPanel.Dock = DockStyle.None;
			_colorPanel.Size = new Size(40, 21);
			_colorPanel.Cursor = Cursors.Hand;
			_colorPanel.MouseClick += ColorPanelMouseClick;

			Controls.Add(_checkBox);

			_checkBox.CheckedChanged += CheckBoxCheckedChanged;

			MouseClick += StringPanelMouseClick;
			Paint += StringPanelPaint;
		}

		public void RefreshColor()
		{
            if (ExceptionThreshold == null) return;
			_colorPanel.BackColor = ExceptionThreshold.Color;
		}

		private static readonly ColorDialog ColorDialog = new ColorDialog();
		private void ColorPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (ExceptionThreshold == null) return;

			if (ColorDialog.ShowDialog() != DialogResult.Cancel)
			{
				ExceptionThreshold.Color = ColorDialog.Color;
				RefreshColor();
			}
		}
		
		private void PaintTitle(Graphics g)
		{
			if (Width <= 250) return;
			Manager.PaintTitleText(g, Localization["POS_Exception"]);

			if (Width <= 300 || !ShowExceptionColor) return;
			g.DrawString(Localization["POS_ExceptionColor"], Manager.Font, Manager.TitleTextColor, 250, 13);
		}

		private void StringPanelPaint(Object sender, PaintEventArgs e)
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

			Brush fontBrush = Brushes.Black;

			if (_checkBox.Visible && Checked)
			{
				fontBrush = SelectedColor;
			}

			if (Width <= 250) return;

			Manager.PaintText(g, Exception.Value, fontBrush);
		}

		private void StringPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (IsTitle)
			{
				if (_checkBox.Visible)
				{
					_checkBox.Checked = !_checkBox.Checked;
					return;
				}
			}
			else
			{
				if (_checkBox.Visible)
				{
					_checkBox.Checked = !_checkBox.Checked;
					return;
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

		public Brush SelectedColor = Manager.SelectedTextColor;

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
