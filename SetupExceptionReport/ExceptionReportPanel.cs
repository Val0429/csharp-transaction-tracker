using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace SetupExceptionReport
{
	public sealed class ExceptionReportPanel : Panel
	{
		public event EventHandler OnReportEditClick;
		public event EventHandler OnSelectAll;
		public event EventHandler OnSelectNone;
		public event EventHandler OnSelectChange;

		public Dictionary<String, String> Localization;

		private readonly CheckBox _checkBox;

		public ExceptionReport Report;
		public Boolean IsTitle;
		public IPTS PTS;

		public ExceptionReportPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"ExceptionReportPanel_Format", "Format"},
								   {"ExceptionReportPanel_Exception", "Exception"},
								   {"ExceptionReportPanel_Threshold", "Threshold"},
								   {"ExceptionReportPanel_Increment", "Increment"},
								   {"ExceptionReportPanel_Recipient", "Recipient"},
							   };
			Localizations.Update(Localization);

			DoubleBuffered = true;
			Dock = DockStyle.Top;
			Cursor = Cursors.Hand;
			Height = 40;

			_checkBox = new CheckBox
			{
				Location = new Point(10, 8),
				Dock = DockStyle.None,
				Width = 25,
			};

			Controls.Add(_checkBox);

			_checkBox.CheckedChanged += CheckBoxCheckedChanged;

			MouseClick += ExceptionReportPanelMouseClick;
			Paint += ExceptionReportPanelPaint;
		}

		private void ExceptionReportPanelMouseClick(Object sender, MouseEventArgs e)
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
				if (OnReportEditClick != null)
					OnReportEditClick(this, e);
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

		//public Brush SelectedColor = Manager.SelectedTextColor;

		private static RectangleF _nameRectangleF = new RectangleF(44, 13, 150, 17);
		private void ExceptionReportPanelPaint(Object sender, PaintEventArgs e)
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

			if (_editVisible)
				Manager.PaintEdit(g, this);

			if (Report == null) return;

			Manager.PaintStatus(g, Report.ReadyState);

			if (Width < 200) return;

			Brush fontBrush = Brushes.Black;
			if (_checkBox.Visible && Checked)
			{
				fontBrush = Manager.DeleteTextColor;
			}

            g.DrawString(POS_Exception.FindExceptionValueByKey(Report.Exception), Manager.Font, fontBrush, _nameRectangleF);

			if (Width < 300) return;
			g.DrawString(ReportFormats.ToString(Report.ReportForm.Format), Manager.Font, fontBrush, 200, 13);

			if (Width < 400) return;
			g.DrawString(Report.Threshold.ToString(), Manager.Font, fontBrush, 300, 13);

			if (Width < 500) return;
			g.DrawString(Report.Increment.ToString(), Manager.Font, fontBrush, 400, 13);

			if (Width < 600) return;
			g.DrawString(Report.ReportForm.MailReceiver, Manager.Font, Brushes.Black, 500, 13);
		}

		private void PaintTitle(Graphics g)
		{
			if (Width < 200) return;
			Manager.PaintTitleText(g, Localization["ExceptionReportPanel_Exception"]);

			if (Width < 300) return;
			g.DrawString(Localization["ExceptionReportPanel_Format"], Manager.Font, Manager.TitleTextColor, 200, 13);

			if (Width < 400) return;
			g.DrawString(Localization["ExceptionReportPanel_Threshold"], Manager.Font, Manager.TitleTextColor, 300, 13);

			if (Width < 500) return;
			g.DrawString(Localization["ExceptionReportPanel_Increment"], Manager.Font, Manager.TitleTextColor, 400, 13);

			if (Width < 600) return;
			g.DrawString(Localization["ExceptionReportPanel_Recipient"], Manager.Font, Manager.TitleTextColor, 500, 13);
		}

		public Boolean SelectionVisible
		{
			set { _checkBox.Visible = value; }
		}

		public Boolean Checked
		{
			set { _checkBox.Checked = value; }
			get { return _checkBox.Checked; }
		}

		private Boolean _editVisible;
		public Boolean EditVisible
		{
			set
			{
				_editVisible = value;
				Invalidate();
			}
		}
	}
}
