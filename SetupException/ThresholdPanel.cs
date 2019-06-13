using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace SetupException
{
	public sealed partial class ThresholdPanel : UserControl
	{
		public IPTS PTS;

		public Dictionary<String, String> Localization;

		public ThresholdPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_Manufacture", "Manufacture"},
								   {"PTSReports_AllException", "All Exception"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;

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

		private void FilterPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintSingleInput(g, filterPanel);

			if (filterPanel.Width <= 100) return;

			Manager.PaintText(g, Localization["PTSReports_Manufacture"]);
		}

		private void FilterComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			containerPanel.Visible = false;

			_exceptionSelectionList = GetExceptionList();

			foreach (ThresholdValueEditPanel thresholdValueEditPanel in containerPanel.Controls)
			{
				if (thresholdValueEditPanel.IsTitle) continue;

				if (_exceptionSelectionList.Contains(thresholdValueEditPanel.Exception))
				{
					thresholdValueEditPanel.Visible = true;
				}
				else
				{
					thresholdValueEditPanel.Visible = false;
				}
			}

			containerPanel.Visible = true;
		}

		private List<String> _exceptionSelectionList;
		private List<String> GetExceptionList()
		{
			var list = new List<String>();

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
					if (!list.Contains(obj.Key))
						list.Add(obj.Key);
				}
			}
			list.Sort((x, y) => (y.CompareTo(x)));

			return list;
		}

		public void Initialize()
		{
			var keys = new List<String>(PTS.POS.ExceptionThreshold.Keys.ToList());
			keys.Sort((x, y) => (y.CompareTo(x)));

			foreach (var key in keys)
			{
				var thresholdValueEditPanel = new ThresholdValueEditPanel
												  {
													  Exception = key,
													  ExceptionThreshold = PTS.POS.ExceptionThreshold[key]
												  };
				thresholdValueEditPanel.Initialize();
				containerPanel.Controls.Add(thresholdValueEditPanel);
			}

			containerPanel.Controls.Add(new ThresholdValueEditPanel { IsTitle = true });

			containerPanel.Select();
			containerPanel.AutoScrollPosition = new Point(0, 0);
		}

		public void ScrollTop()
		{
			//containerPanel.AutoScroll = false;
			containerPanel.Select();
			containerPanel.AutoScrollPosition = new Point(0, 0);
			//containerPanel.AutoScroll = true;
		}
	}

	public sealed class ThresholdValueEditPanel: Panel
	{
		public Dictionary<String, String> Localization;

		public String Exception;
		public POS_Exception.ExceptionThreshold ExceptionThreshold;
		private readonly TextBox _threshodeTextBox1 = new PanelBase.HotKeyTextBox();
		private readonly TextBox _threshodeTextBox2 = new PanelBase.HotKeyTextBox();

		public Boolean IsTitle;
		public ThresholdValueEditPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"POS_Exception", "Exception"},
								   {"POS_Threshold", "Threshold"},
							   };
			Localizations.Update(Localization);

			DoubleBuffered = true;
			Dock = DockStyle.Top;
			Cursor = Cursors.Default;
			Height = 40;

			BackColor = Color.Transparent;

			_threshodeTextBox1.Padding = new Padding(0, 0, 0, 0);
			_threshodeTextBox1.Margin = new Padding(0, 0, 0, 0);
			_threshodeTextBox1.Location = new Point(250, 8);
			_threshodeTextBox1.Dock = DockStyle.None;
			_threshodeTextBox1.Size = new Size(40, 21);
			_threshodeTextBox1.MaxLength = 4;
			_threshodeTextBox1.ImeMode = ImeMode.Disable;
			
			_threshodeTextBox2.Padding = new Padding(0, 0, 0, 0);
			_threshodeTextBox2.Margin = new Padding(0, 0, 0, 0);
			_threshodeTextBox2.Location = new Point(340, 8);
			_threshodeTextBox2.Dock = DockStyle.None;
			_threshodeTextBox2.Size = new Size(40, 21);
			_threshodeTextBox2.MaxLength = 4;
			_threshodeTextBox2.ImeMode = ImeMode.Disable;

			_threshodeTextBox1.KeyPress += KeyAccept.AcceptNumberOnly;
			_threshodeTextBox2.KeyPress += KeyAccept.AcceptNumberOnly;

			Paint += ThresholdValueEditPanelPaint;
		}

		private void ThresholdValueEditPanelPaint(Object sender, PaintEventArgs e)
		{
			if (Parent == null) return;

			Graphics g = e.Graphics;

			if (IsTitle)
			{
				Manager.PaintTitleTopInput(g, this);
				PaintTitle(g);
				return;
			}

			Manager.Paint(g, this);

			if (Width <= 250) return;
            Manager.PaintText(g, POS_Exception.FindExceptionValueByKey(Exception));
		}
		
		private void PaintTitle(Graphics g)
		{
			if (Width <= 250) return;
			Manager.PaintTitleText(g, Localization["POS_Exception"]);
			
			if (Width <= 310) return;
			g.DrawString(Localization["POS_Threshold"] + " 1", Manager.Font, Manager.TitleTextColor, 250, 13);

			if (Width <= 400) return;
			g.DrawString(Localization["POS_Threshold"] + " 2", Manager.Font, Manager.TitleTextColor, 340, 13);
		}

		public void Initialize()
		{
			if (IsTitle) return;

			Controls.Add(_threshodeTextBox1);
			Controls.Add(_threshodeTextBox2);

			_threshodeTextBox1.Text = ExceptionThreshold.ThresholdValue1.ToString();
			_threshodeTextBox2.Text = ExceptionThreshold.ThresholdValue2.ToString();

			_threshodeTextBox1.TextChanged += ThreshodeTextBox1TextChanged;
			_threshodeTextBox2.TextChanged += ThreshodeTextBox2TextChanged;
			_threshodeTextBox1.LostFocus += ThreshodeTextBox1LostFocus;
			_threshodeTextBox2.LostFocus += ThreshodeTextBox2LostFocus;
		}

		private void ThreshodeTextBox1TextChanged(Object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(_threshodeTextBox1.Text)) return;

			ExceptionThreshold.ThresholdValue1 = Convert.ToUInt16(Math.Min(Math.Max(Convert.ToInt32(_threshodeTextBox1.Text), 1), 9999));
		}

		private void ThreshodeTextBox2TextChanged(Object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(_threshodeTextBox2.Text)) return;

			ExceptionThreshold.ThresholdValue2 = Convert.ToUInt16(Math.Min(Math.Max(Convert.ToInt32(_threshodeTextBox2.Text), 1), 9999));
		}

		private void ThreshodeTextBox1LostFocus(Object sender, EventArgs e)
		{
			if (_threshodeTextBox1.Text == ExceptionThreshold.ThresholdValue1.ToString()) return;

			_threshodeTextBox1.TextChanged -= ThreshodeTextBox1TextChanged;
			
			_threshodeTextBox1.Text = ExceptionThreshold.ThresholdValue1.ToString();

			_threshodeTextBox1.TextChanged += ThreshodeTextBox1TextChanged;
		}

		private void ThreshodeTextBox2LostFocus(Object sender, EventArgs e)
		{
			if (_threshodeTextBox2.Text == ExceptionThreshold.ThresholdValue2.ToString()) return;

			_threshodeTextBox2.TextChanged -= ThreshodeTextBox2TextChanged;

			_threshodeTextBox2.Text = ExceptionThreshold.ThresholdValue2.ToString();

			_threshodeTextBox2.TextChanged += ThreshodeTextBox2TextChanged;
		}
	}
}