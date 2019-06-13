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
	public sealed partial class ExceptionColorPanel : UserControl
	{
		public IPTS PTS;

		public Dictionary<String, String> Localization;

		public ExceptionColorPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"POS_ExceptionColor", "Exception Color"},
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

			foreach (ExceptionColorEditPanel exceptionColorEditPanel in containerPanel.Controls)
			{
				if (exceptionColorEditPanel.IsTitle) continue;

				if (_exceptionSelectionList.Contains(exceptionColorEditPanel.Exception))
				{
					exceptionColorEditPanel.Visible = true;
				}
				else
				{
					exceptionColorEditPanel.Visible = false;
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
				var thresholdValueEditPanel = new ExceptionColorEditPanel
				{
					Exception = key,
					ExceptionThreshold = PTS.POS.ExceptionThreshold[key]
				};
				thresholdValueEditPanel.Initialize();
				containerPanel.Controls.Add(thresholdValueEditPanel);
			}

			containerPanel.Controls.Add(new ExceptionColorEditPanel { IsTitle = true });

			containerPanel.Select();
			containerPanel.AutoScrollPosition = new Point(0, 0);
		}

		public void RefreshColor()
		{
			foreach (ExceptionColorEditPanel colorEditPanel in containerPanel.Controls)
			{
				colorEditPanel.RefreshColor();
			}
		}
	}

	public sealed class ExceptionColorEditPanel : Panel
	{
		public Dictionary<String, String> Localization;

		public String Exception;
		public POS_Exception.ExceptionThreshold ExceptionThreshold;
		private readonly Panel _colorPanel = new Panel();

		public Boolean IsTitle;
		public ExceptionColorEditPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"POS_Exception", "Exception"},
								   {"POS_ExceptionColor", "Exception Color"},
							   };
			Localizations.Update(Localization);

			DoubleBuffered = true;
			Dock = DockStyle.Top;
			Cursor = Cursors.Default;
			Height = 40;

			BackColor = Color.Transparent;

			_colorPanel.Padding = new Padding(0, 0, 0, 0);
			_colorPanel.Margin = new Padding(0, 0, 0, 0);
			_colorPanel.Location = new Point(250, 8);
			_colorPanel.Dock = DockStyle.None;
			_colorPanel.Size = new Size(40, 21);
			_colorPanel.Cursor = Cursors.Hand;

			Paint += ExceptionColorEditPanelPaint;
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

		private void ExceptionColorEditPanelPaint(Object sender, PaintEventArgs e)
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

			if (Width <= 300) return;
			g.DrawString(Localization["POS_ExceptionColor"], Manager.Font, Manager.TitleTextColor, 250, 13);
		}

		public void Initialize()
		{
			if (!IsTitle)
			{
				Controls.Add(_colorPanel);

				_colorPanel.BackColor = ExceptionThreshold.Color;

				_colorPanel.MouseClick += ColorPanelMouseClick;
			}
		}
	}
}