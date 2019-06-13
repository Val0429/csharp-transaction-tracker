using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupServer
{
	public sealed partial class DevicePackControl : UserControl
	{
		public IServer Server;
		public Dictionary<String, String> Localization;

		public String FullFileName;
		public String FileName;
		private readonly OpenFileDialog _openDevicePackDialog;

		public DevicePackControl()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"SetupServer_DevicePackFile", "Device Pack File"},
								   {"SetupServer_NoSelectedFile", "No selected file"},
								   {"SetupServer_Browse", "Browse"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.None;
			Name = "DevicePackVersion";

			_openDevicePackDialog = new OpenFileDialog
			{
				Filter = Localization["SetupServer_DevicePackFile"] + @" (.pak)|*.pak"
			};

			filePanel.Paint += FilePanelPaint;

			browserButton.Text = Localization["SetupServer_Browse"];

			BackgroundImage = Manager.BackgroundNoBorder;
		}

		public void Reset()
		{
			FileName = FullFileName = String.Empty;
		}

		private void FilePanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintSingleInput(g, filePanel);
			if (Width <= 100) return;

			String title = Localization["SetupServer_DevicePackFile"];

			title = title + " : ";

			if (String.IsNullOrEmpty(FileName))
			{
				title = title + Localization["SetupServer_NoSelectedFile"];
				Manager.PaintText(g, title, Brushes.Black);
			}
			else
			{
				title = title + FileName;
				
				Manager.PaintText(g, title, Manager.SelectedTextColor);
			}
		}

		private void BrowserButtonClick(Object sender, EventArgs e)
		{
			if (_openDevicePackDialog.ShowDialog() != DialogResult.OK) return;

			var fileInfo = new FileInfo(_openDevicePackDialog.FileName);
			FileName = fileInfo.Name;
			FullFileName = fileInfo.FullName;

			filePanel.Invalidate();
		}
	}
}
