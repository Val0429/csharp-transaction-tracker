using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace DownloadCaseForm
{
	public sealed class DownloadCasePanel : Panel
	{
		public event EventHandler OnDownloadCaseEditClick;
		public event EventHandler OnSelectAll;
		public event EventHandler OnSelectNone;
		public event EventHandler OnSelectChange;

		public Dictionary<String, String> Localization;

		private readonly CheckBox _checkBox;

		public DownloadCaseConfig Config;
		public Boolean IsTitle;
		public DownloadCasePanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"ExportVideo_ID", "ID"},
								   {"ExportVideo_Start", "Start"},
								   {"ExportVideo_End", "End"},
								   {"ExportVideo_AudioIn", "Audio In"},
								   {"ExportVideo_AudioOut", "Audio Out"},
								   {"ExportVideo_Encode", "Encode"},
								   {"ExportVideo_Overlay", "Overlay OSD"},
								   {"ExportVideo_Path", "Path"},
								   {"DownloadCase_Comment", "Comment"},
								   {"DownloadCase_Status", "Status"},
								   
								   {"SetupGeneral_Desktop", "Desktop"},
								   {"SetupGeneral_Document", "My Documents"},
								   {"SetupGeneral_Picture", "My Pictures"},

								   {"DownloadCase_Downloading", "Downloading"},
								   {"DownloadCase_Completed", "Completed"},
								   {"DownloadCase_Failed", "Failed"},
								   {"DownloadCase_Queue", "Queue"},
								   {"DownloadCase_Stop", "Stop"},
							   };
			Localizations.Update(Localization);

			DoubleBuffered = true;
			Dock = DockStyle.Top;
			Cursor = Cursors.Default;
			Height = 40;

			_checkBox = new CheckBox
			{
				Location = new Point(10, 8),
				Dock = DockStyle.None,
				Width = 25,
				Visible = false,
			};

			Controls.Add(_checkBox);

			_checkBox.CheckedChanged += CheckBoxCheckedChanged;

			MouseClick += DownloadCasePanelMouseClick;
			Paint += DownloadCasePanelPaint;
		}

		private void DownloadCasePanelMouseClick(Object sender, MouseEventArgs e)
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
				if (OnDownloadCaseEditClick != null)
					OnDownloadCaseEditClick(this, e);
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

		//private readonly String _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		//private readonly String _documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		//private readonly String _picturePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
		//private static RectangleF _pathRectangleF = new RectangleF(700, 13, 95, 17);
		private static RectangleF _commentRectangleF = new RectangleF(500, 13, 145, 17);
		private void DownloadCasePanelPaint(Object sender, PaintEventArgs e)
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

			if (Config == null) return;

			if (Width < 200) return;

			Brush fontBrush = Brushes.Black;
			if (_checkBox.Visible && Checked)
			{
				fontBrush = Manager.DeleteTextColor;
			}

			Manager.PaintText(g, Config.Id.ToString());
			String status;
			switch (Config.Status)
			{
				case QueueStatus.Downloading:
					status = Localization["DownloadCase_Downloading"];
					break;
					
				case QueueStatus.Completed:
					status = Localization["DownloadCase_Completed"];
					break;

				case QueueStatus.Failed:
					status = Localization["DownloadCase_Failed"];
					break;
					
				case QueueStatus.Queue:
					status = Localization["DownloadCase_Queue"];
					break;

				default:
					status = Localization["DownloadCase_Stop"];
					break;
			}
			g.DrawString(status, Manager.Font, Brushes.Black, 100, 13);

			if (Width < 350) return;
			g.DrawString(Config.StartDateTime.ToString("yyyy-MM-dd-HH-mm-ss"), Manager.Font, fontBrush, 200, 13);

			if (Width < 500) return;
			g.DrawString(Config.EndDateTime.ToString("yyyy-MM-dd-HH-mm-ss"), Manager.Font, fontBrush, 350, 13);

			//if (Width < 600) return;
			//if(Config.AudioIn)
			//    g.DrawImage(_enableIcon, 500, 8);

			//if (Width < 700) return;
			//if (Config.AudioOut)
			//    g.DrawImage(_enableIcon, 600, 8);

			//if (Width < 800) return;
			//if (Config.EncodeAVI)
			//    g.DrawImage(_enableIcon,700, 8);

			//if (Width < 900) return;
			//if (Config.OverlayOSD)
			//    g.DrawImage(_enableIcon, 800, 8);

			//if (Width < 800) return;

			//String path;
			//if(Config.DownloadPath == _desktopPath)
			//    path = Localization["SetupGeneral_Desktop"];
			//else if(Config.DownloadPath == _documentPath)
			//    path = Localization["SetupGeneral_Document"];
			//else if(Config.DownloadPath == _picturePath)
			//        path = Localization["SetupGeneral_Picture"];
			//else
			//        path  = Config.DownloadPath;

			//g.DrawString(path, Manager.Font, fontBrush, _pathRectangleF);

			if (Width < 600) return;
			g.DrawString(Config.Comments, Manager.Font, fontBrush, _commentRectangleF);
		}

		private void PaintTitle(Graphics g)
		{
			if (Width < 200) return;
			Manager.PaintTitleText(g, Localization["ExportVideo_ID"]);

			g.DrawString(Localization["DownloadCase_Status"], Manager.Font, Manager.TitleTextColor, 100, 13);

			if (Width < 350) return;
			g.DrawString(Localization["ExportVideo_Start"], Manager.Font, Manager.TitleTextColor, 200, 13);

			if (Width < 500) return;
			g.DrawString(Localization["ExportVideo_End"], Manager.Font, Manager.TitleTextColor, 350, 13);

			//if (Width < 600) return;
			//g.DrawString(Localization["ExportVideo_AudioIn"], Manager.Font, Manager.TitleTextColor, 500, 13);

			//if (Width < 700) return;
			//g.DrawString(Localization["ExportVideo_AudioOut"], Manager.Font, Manager.TitleTextColor, 600, 13);

			//if (Width < 800) return;
			//g.DrawString(Localization["ExportVideo_Encode"], Manager.Font, Manager.TitleTextColor, 700, 13);

			//if (Width < 900) return;
			//g.DrawString(Localization["ExportVideo_Overlay"], Manager.Font, Manager.TitleTextColor, 800, 13);

			//if (Width < 800) return;
			//g.DrawString(Localization["ExportVideo_Path"], Manager.Font, Manager.TitleTextColor, 700, 13);

			if (Width < 600) return;
			g.DrawString(Localization["DownloadCase_Comment"], Manager.Font, Manager.TitleTextColor, 500, 13);
		}

		public Boolean Checked
		{
			set { _checkBox.Checked = value; }
			get { return _checkBox.Checked; }
		}

		public Boolean SelectionVisible
		{
			set
			{
				_checkBox.Visible = value;
				Cursor = (value)
							 ? Cursors.Hand
							 : Cursors.Default;
			}
		}
	}
}
