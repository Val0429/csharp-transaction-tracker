using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Constant;
using Constant.Utility;
using PanelBase;

namespace App
{
	public static class ApplicationForms
	{
		private static readonly TransparentPanel TransparentPanel;
		private static readonly DoubleBufferPanel SavingProgressPanel;
		private static readonly DoubleBufferPanel SavingProgressBgPanel;
		private static readonly PictureBox LoadingIcon;
		private static readonly DoubleBufferLabel DotLabel;
		private static readonly System.Timers.Timer AnimTimer;
		private static readonly System.Timers.Timer DotTimer;

		static ApplicationForms()
		{
			TransparentPanel = new TransparentPanel();

			SavingProgressPanel = new DoubleBufferPanel
			{
				BackgroundImage = Resources.GetResources(Properties.Resources.progressBar, Properties.Resources.IMGProgressBar),
				Location = new Point(0, 0),
				Size = new Size(0, 26)
			};

			SavingProgressBgPanel = new DoubleBufferPanel
			{
				BackColor = Color.White,
				BackgroundImage = Resources.GetResources(Properties.Resources.progressBarBg, Properties.Resources.IMGProgressBarBg),
				Location = new Point(45, 47),
				Margin = new Padding(0),
				Size = new Size(416, 26)
			};
			SavingProgressBgPanel.Controls.Add(SavingProgressPanel);

			LoadingIcon = new PictureBox
			{
				Dock = DockStyle.None,
				Size = new Size(100, 100),
				SizeMode = PictureBoxSizeMode.CenterImage,
				BackColor = Color.Transparent,
				Image = Resources.GetResources(Properties.Resources.loadingClock, Properties.Resources.IMGLoadingClock),
			};
			LoadingIcon.Size = new Size(Math.Max(LoadingIcon.Image.Width, LoadingIcon.Width), Math.Max(LoadingIcon.Image.Height, LoadingIcon.Height));

			DotLabel = new DoubleBufferLabel
			{
				Dock = DockStyle.None,
				Size = new Size(100, 40),
				BackColor = Color.Transparent,
			};
			DotLabel.Paint += DotLabelPaint;

			AnimTimer = new System.Timers.Timer(30);
			AnimTimer.Elapsed += AnimTimerElapsed;

			DotTimer = new System.Timers.Timer(250);
			DotTimer.Elapsed += DotTimerElapsed;
		}

		private static String _dotStr = "";
		private static readonly Font DotFont = new Font("Arial", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
		public static readonly Font StatusFont = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
		public static readonly Brush StatusFontColor = Brushes.LightGray;
		public static readonly Brush DotBrush = new SolidBrush(Color.FromArgb(59, 133, 185));
		private static void DotLabelPaint(Object sender, PaintEventArgs e)
		{
			try
			{
				var g = e.Graphics;
				g.DrawString(_dotStr, DotFont, DotBrush, 3, 5);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception);
			}
		}

		private static Process _proc;
		private static Image _snapshot;
		private static Form _form;
		public static void ShowLoadingIcon(Form form)
		{
			AnimTimer.SynchronizingObject = form;
			DotTimer.SynchronizingObject = form;

			_form = form;

			if (_form == null) return;

			_proc = Process.GetCurrentProcess();

			_form.TopMost = true;
			_snapshot = ScreenShot.GetSnapshot(_proc);
			_form.TopMost = false;

			 Application.Idle -= ShowLoading;
			 Application.Idle += ShowLoading;
		}

		private static void ShowLoading(Object sender, EventArgs e)
		{
			Application.Idle -= ShowLoading;

			if (_form == null) return;

			Size borderSize = SystemInformation.FrameBorderSize;
			int captionHeight = SystemInformation.CaptionHeight;

			_form.TopMost = true;
			var snapshot = ScreenShot.GetSnapshot(_proc);
			if (snapshot != null && snapshot.Width == (_form.Width - (borderSize.Width * 2)) && snapshot.Height == (_form.Height - captionHeight - (borderSize.Height * 2)))
				TransparentPanel.BackgroundImage = snapshot;
			else if (_snapshot != null && _snapshot.Width == (_form.Width - (borderSize.Width * 2)) && _snapshot.Height == (_form.Height - captionHeight - (borderSize.Height * 2)))
				TransparentPanel.BackgroundImage = _snapshot;
			else
				TransparentPanel.BackgroundImage = null;
			_form.TopMost = false;

			_form.Controls.Add(TransparentPanel);
			TransparentPanel.Size = new Size(_form.ClientSize.Width, _form.ClientSize.Height);
			TransparentPanel.BringToFront();

			TransparentPanel.Controls.Add(LoadingIcon);
			LoadingIcon.Location = new Point((TransparentPanel.Width - LoadingIcon.Width) / 2, (TransparentPanel.Height - LoadingIcon.Height) / 2);

			TransparentPanel.Controls.Add(DotLabel);
			DotLabel.Location = new Point((TransparentPanel.Width - DotLabel.Width) / 2, LoadingIcon.Location.Y + LoadingIcon.Height);

			_dotStr = "";
			DotTimer.Enabled = true;
			_form.Enabled = false;
		}

		private delegate void HideLoadingIconDelegate();
		public static void HideLoadingIcon()
		{
			if(_form == null) return;

			if (_form.InvokeRequired)
			{
				try
				{
					_form.Invoke(new HideLoadingIconDelegate(HideLoadingIcon));
				}
				catch (Exception)
				{
				}
				return;
			}

			AnimTimer.SynchronizingObject = _form;
			DotTimer.SynchronizingObject = _form;

			if (_form == null) return;

			Application.Idle -= ShowLoading;
			Application.Idle -= HideLoading;
			Application.Idle += HideLoading;
		}

		private static void HideLoading(Object sender, EventArgs e)
		{
			Application.Idle -= HideLoading;

			DotTimer.Enabled = false;
			_form.Controls.Remove(TransparentPanel);
			TransparentPanel.Controls.Remove(LoadingIcon);
			TransparentPanel.Controls.Remove(DotLabel);
			_form.Enabled = true;
		}

		private delegate void ShowProgressBarDelegate(Form form);
		public static void ShowProgressBar(Form form)
		{
			if (form.InvokeRequired)
			{
				try
				{
					form.Invoke(new ShowProgressBarDelegate(ShowProgressBar), form);
				}
				catch (Exception)
				{
				}
				return;
			}
			
			AnimTimer.SynchronizingObject = form;
			DotTimer.SynchronizingObject = form;

			_form = form;

			if (_form == null) return;

			_proc = Process.GetCurrentProcess();

			_form.TopMost = true;
			_snapshot = ScreenShot.GetSnapshot(_proc);
			_form.TopMost = false;

			Application.Idle -= ShowProgress;
			Application.Idle += ShowProgress;
		}

		private static void ShowProgress(Object sender, EventArgs e)
		{
			Application.Idle -= ShowProgress;

			if (_form == null) return;
			
			Size borderSize = SystemInformation.FrameBorderSize;
			int captionHeight = SystemInformation.CaptionHeight;

			_form.TopMost = true;
			var snapshot = ScreenShot.GetSnapshot(_proc);
			if (snapshot != null && snapshot.Width == (_form.Width - (borderSize.Width * 2)) && snapshot.Height == (_form.Height - captionHeight - (borderSize.Height * 2)))
				TransparentPanel.BackgroundImage = snapshot;
			else if (_snapshot != null && _snapshot.Width == (_form.Width - (borderSize.Width * 2)) && _snapshot.Height == (_form.Height - captionHeight - (borderSize.Height * 2)))
				TransparentPanel.BackgroundImage = _snapshot;
			else
				TransparentPanel.BackgroundImage = null;
			_form.TopMost = false;

			_form.Controls.Add(TransparentPanel);
			TransparentPanel.Size = new Size(_form.ClientSize.Width, _form.ClientSize.Height);
			TransparentPanel.BringToFront();

			TransparentPanel.Controls.Add(SavingProgressBgPanel);
			SavingProgressBgPanel.Location = new Point((TransparentPanel.Width - SavingProgressBgPanel.Width) / 2, (TransparentPanel.Height - SavingProgressBgPanel.Height) / 2);

			_form.Enabled = false;
		}

		private static Int32 _progressValue;

		private delegate void SetProgressBarValueDelegate(Int32 value);
		private static void SetProgressBarValue(Int32 value)
		{
			if (SavingProgressPanel.InvokeRequired)
			{
				try
				{
					SavingProgressPanel.Invoke(new SetProgressBarValueDelegate(SetProgressBarValue), value);
				}
				catch (Exception)
				{
				}
				return;
			}
			_progressValue = value;
			if (value >= 100)
			{
				SavingProgressPanel.Width = SavingProgressBgPanel.Width;
				AnimTimer.Enabled = false;
				return;
			}
			AnimTimer.Enabled = true;
		}

		public static Int32 ProgressBarValue
		{
			set
			{
				SetProgressBarValue(value);
			}
			get
			{
				return _progressValue;
			}
		}

		private static void DotTimerElapsed(Object sender, EventArgs e)
		{
			if (_dotStr.Length < 13)
				_dotStr += ".";
			else
				_dotStr = "";

			DotLabel.Invalidate();
		}

		private static void AnimTimerElapsed(Object sender, EventArgs e)
		{
			if (AnimTimer.Enabled && SavingProgressPanel.Width * 100.0 / SavingProgressBgPanel.Width < _progressValue)
			{
				SavingProgressPanel.Width += Convert.ToInt32((SavingProgressBgPanel.Width * (_progressValue / 100.0)) - SavingProgressPanel.Width) / 10;
			}
			//_savingProgressPanel.Width = Convert.ToInt32((value / 100.0) * _savingProgressBgPanel.Width);
		}

		private delegate void HideProgressBarDelegate();
		public static void HideProgressBar()
		{
			if (_form == null) return;

			if (_form.InvokeRequired)
			{
				try
				{
					_form.Invoke(new HideProgressBarDelegate(HideProgressBar));
				}
				catch (Exception)
				{
				}
				return;
			}

			AnimTimer.SynchronizingObject = _form;
			DotTimer.SynchronizingObject = _form;

			if (_form == null) return;

			Application.Idle -= HideProgress;
			Application.Idle += HideProgress;
		}

		private static void HideProgress(Object sender, EventArgs e)
		{
			Application.Idle -= HideProgress;

			if (_form == null) return;

			_form.Controls.Remove(TransparentPanel);
			TransparentPanel.Controls.Remove(SavingProgressBgPanel);

			_form.Enabled = true;
			AnimTimer.Enabled = false;

			SavingProgressPanel.Width = 0;
		}

		private const UInt16 PaddingLeft = 10;
		public static Panel MainPanel()
		{
			return new DoubleBufferPanel
			{
				Dock = DockStyle.Fill,
				BackColor = Color.FromArgb(202, 202, 202),
				Padding = new Padding(PaddingLeft, 0, 0, 0),
			};
		}

		public static Panel MainPanelUI2()
		{
			return new DoubleBufferPanel
			{
				Dock = DockStyle.Fill,
				Padding = new Padding(0, 0, 0, 0),
			};
		}

		public static Panel WorkPanel()
		{
			return new DoubleBufferPanel
			{
				Dock = DockStyle.Fill,
				Padding = new Padding(0, 0, 10, 0),
			};
		}

		public static Panel WorkPanelUI2()
		{
			return new DoubleBufferPanel
			{
				Dock = DockStyle.Fill,
				Padding = new Padding(0, 0, 0, 0),
			};
		}
		
		public static Panel ToolPanel()
		{
			return new DoubleBufferPanel
			{
				Dock = DockStyle.Right,
				AutoScroll = false,
				Width = 60,
				//MinimumSize = new Size(75, 800),
				//BackColor = Color.Gray,
				BackgroundImage = Resources.GetResources(Properties.Resources.toolBg, Properties.Resources.IMGToolBg),
				BackgroundImageLayout = ImageLayout.Stretch,
				Padding = new Padding(0, 5, 0, 0),
			};
		}

		public static Panel ToolPanelUI2()
		{
			return new DoubleBufferPanel
			{
				Dock = DockStyle.Left,//default, can change at xml
				AutoScroll = false,
				Width = 70, //default, can change at xml
				BackgroundImage = Resources.GetResources(Properties.Resources.toolBg2, Properties.Resources.IMGToolBg2),
				BackgroundImageLayout = ImageLayout.Tile,
				//BackColor = Color.Black,
				Padding = new Padding(0, 0, 0, 0),
			};
		}
		
		public static Panel SwitchPagePanel()
		{
			return new DoubleBufferPanel
			{
				Dock = DockStyle.Top,
				AutoSize = true,
				//MinimumSize = new Size(50, 150),
				BackColor = Color.Transparent,
			};
		}

		public static Panel SwitchPagePanelUI2()
		{
			return new DoubleBufferPanel
			{
				Dock = DockStyle.None,
				Padding = new Padding(0),
				AutoSize = false,
				Height = 40,
				//Anchor = AnchorStyles.Left | AnchorStyles.Right,
				BackColor = Color.Transparent,
			};
		}

		public static Panel PageDockIconPanelUI2()
		{
			return new DoubleBufferPanel
			{
				Dock = DockStyle.Fill,
				BackColor = Color.Transparent,
			};
		}
		
		public static Panel FunctionPanel()
		{
			return new DoubleBufferPanel
			{
				Height = 360,
				//AutoSizeMode = AutoSizeMode.GrowAndShrink,
				//AutoSize = true,
				BackColor = Color.Transparent,
				BackgroundImage = Resources.GetResources(Properties.Resources.functionBg, Properties.Resources.IMGFunctionBg),
				BackgroundImageLayout = ImageLayout.Stretch,
				Dock = DockStyle.Bottom,
			};
		}

		public static Panel FunctionPanelUI2()
		{
			return new DoubleBufferPanel
			{
				Height = 360,//change at xml
				BackColor = Color.Transparent,
				Dock = DockStyle.Bottom,
				Padding = new Padding(0, 5, 0, 5)
			};
		}
		
		public static Panel MiniFunctionPanel()
		{
			return new DoubleBufferPanel
			{
				Cursor = Cursors.Hand,
				Size = new Size(60, 20),
				BackColor = Color.Transparent,
				Dock = DockStyle.Bottom,
				BackgroundImage = Resources.GetResources(Properties.Resources.collapse, Properties.Resources.IMGCollapse),
				BackgroundImageLayout = ImageLayout.Center,
			};
		}

		public static Panel StatusPanel()
		{
			return new DoubleBufferPanel
			{
				Dock = DockStyle.Bottom,
				Height = 20,
				BackColor = ColorTranslator.FromHtml("#CACACA"),
			};
		}

		public static Panel StatusPanelUI2()
		{
			return new DoubleBufferPanel
			{
				Dock = DockStyle.Bottom,
				Height = 35,
				BackgroundImage = Resources.GetResources(Properties.Resources.statusBg, Properties.Resources.IMGStatusBg),
				BackgroundImageLayout = ImageLayout.Tile,
			};
		}

		public static Panel StoragePanel()
		{
			var icon = new DoubleBufferPanel
			{
				Dock = DockStyle.Left,
				Width = 20,
				BackgroundImage = Resources.GetResources(Properties.Resources.storageIcon, Properties.Resources.IMGStorageIcon),
				BackgroundImageLayout = ImageLayout.Center,
			};

			var storagePanel =  new DoubleBufferPanel
			{
				Dock = DockStyle.Left,
				Name = "storagePanel",
				Size = new Size(520, 20),
				BackColor = Color.Transparent
			};

			storagePanel.Controls.Add(icon);

			return storagePanel;
		}

		public static Panel TotalBitratePanel()
		{
			var icon = new DoubleBufferPanel
			{
				Dock = DockStyle.Left,
				Width = 20,
				BackgroundImage = Resources.GetResources(Properties.Resources.bitrateIcon, Properties.Resources.IMGBitrateIcon),
				BackgroundImageLayout = ImageLayout.Center,
				Visible = false
			};

			var bitratePanel = new DoubleBufferPanel
			{
				Dock = DockStyle.Left,
				Name = "totalBitratePanel",
				Size = new Size(150, 20),
				BackColor = Color.Transparent
			};

			bitratePanel.Controls.Add(icon);
			
			return bitratePanel;
		}

		public static Panel CPUUsagePanel()
		{
			var icon = new DoubleBufferPanel
			{
				Dock = DockStyle.Left,
				Width = 20,
				BackgroundImage = Resources.GetResources(Properties.Resources.cpuIcon, Properties.Resources.IMGCPUIcon),
				BackgroundImageLayout = ImageLayout.Center,
			};

			var cpuPanel = new DoubleBufferPanel
			{
				Dock = DockStyle.Left,
				Name = "cpuUsagePanel",
				Size = new Size(120, 20),
				BackColor = Color.Transparent
			};

			cpuPanel.Controls.Add(icon);

			return cpuPanel;
		}

		public static Panel UserPanel()
		{
			var icon = new DoubleBufferPanel
			{
				Dock = DockStyle.Left,
				Width = 20,
				BackgroundImage = Resources.GetResources(Properties.Resources.userIcon, Properties.Resources.IMGUserIcon),
				BackgroundImageLayout = ImageLayout.Center,
			};

			var userPanel = new DoubleBufferPanel
			{
				Dock = DockStyle.Right,
				Name = "userPanel",
				Size = new Size(140, 20),
				BackColor = Color.Transparent
			};

			userPanel.Controls.Add(icon);

			return userPanel;
		}

		public static Panel TimePanel()
		{
			var icon = new DoubleBufferPanel
			{
				Dock = DockStyle.Left,
				Width = 20,
				BackgroundImage = Resources.GetResources(Properties.Resources.timeIcon, Properties.Resources.IMGTimeIcon),
				BackgroundImageLayout = ImageLayout.Center,
			};

            var timeFont = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            var timeZoneSize = TextRenderer.MeasureText("(GMT + 10) ", timeFont);
            var dateTimeStringSize = DateTimeConverter.GetDateTimeStringSize(timeFont);
            var panelSize = new Size(timeZoneSize.Width + dateTimeStringSize.Width + 15, 20);
			var timePanel = new DoubleBufferPanel
			{
				Dock = DockStyle.Right,
                Font = timeFont,
                Size = panelSize,
				BackColor = Color.Transparent
			};

			timePanel.Controls.Add(icon);

			return timePanel;
		}

		public static Panel MenuPanel()
		{
			return new DoubleBufferPanel
			{
				Dock = DockStyle.Top,
				Size = new Size(600, 30),
				BackgroundImage = Resources.GetResources(Properties.Resources.menuBg, Properties.Resources.IMGMenuBg),
				BackgroundImageLayout = ImageLayout.Tile,
			};
		}

		public static Panel MenuPanelUI2()
		{
			return new DoubleBufferPanel
			{
				Dock = DockStyle.Top,
				Size = new Size(600, 60),
				BackgroundImage = Resources.GetResources(Properties.Resources.headerBg, Properties.Resources.IMGHeaderBg),
				BackgroundImageLayout = ImageLayout.Tile,
			};
		}

		public static PictureBox Logo()
		{
			var logo = new PictureBox
			{
				Dock = DockStyle.Left,
				BackColor = Color.Transparent,
				Width = PaddingLeft,
				SizeMode = PictureBoxSizeMode.CenterImage,
			};

			if (File.Exists(Application.StartupPath + "\\images\\logo.png"))
			{
				Image newLogo = Image.FromFile(Application.StartupPath + "\\images\\logo.png");
				if (newLogo != null)
				{
					logo.Width = newLogo.Width + PaddingLeft;// +16;
					logo.Image = newLogo;
					logo.Padding = new Padding(PaddingLeft, 0, 0, 0);
				}
			}
			
			return logo;
		}

		public static Label VersionLabel()
		{
			return new DoubleBufferLabel
			{
				Width = 150,
				Dock = DockStyle.Right,
				Padding = new Padding(0, 0, 10, 0),
				TextAlign = ContentAlignment.MiddleRight,
				BackColor = Color.Transparent,
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
			};
		}
	}
}
