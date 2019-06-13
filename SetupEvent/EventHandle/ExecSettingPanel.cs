using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Constant;
using Interface;
using ServerProfile;

namespace SetupEvent
{
	public sealed class ExecSettingPanel : Panel
	{
		public HandlePanel HandlePanel;
		public Dictionary<String, String> Localization;

		private ExecEventHandle _eventHandle;
		public ExecEventHandle EventHandle
		{
			set
			{
				_eventHandle = value;

				if (_eventHandle == null) return;

				_isEditing = false;

				_fileTextBox.Text = _eventHandle.FileName;

				_isEditing = true;
			}
		}

		private readonly Label _fileLabel;
		private readonly TextBox _fileTextBox;
		private readonly OpenFileDialog _fileDialog;

		public ExecSettingPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Handler_FileName", "File Name"},
								   //{"Handler_Pick_A_File", "Pick a File"},
							   };
			Localizations.Update(Localization);

			DoubleBuffered = true;
			Dock = DockStyle.None;
			AutoSize = true;
			Height = 24;
			Location = new Point(200, 7);
			Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);

			_fileLabel = new Label
			{
				Padding = new Padding(0, 0, 0, 4),
				AutoSize = true,
				MinimumSize = new Size(20, 24),
				Dock = DockStyle.Left,
				Text = Localization["Handler_FileName"],
				TextAlign = ContentAlignment.MiddleLeft,
			};

			_fileTextBox = new PanelBase.HotKeyTextBox
			{
				Width = 400,
				Dock = DockStyle.Left,
			};

			_fileDialog = new OpenFileDialog
			{
				//Title = Localization["Handler_Pick_A_File"],
				//Filter = @"Waveform audio format|*.wav"
			};

			Controls.Add(_fileTextBox);
			Controls.Add(_fileLabel);

			_fileTextBox.Click += FileTextBoxClick;
		}

		private Boolean _isEditing;

		private void FileTextBoxClick(object sender, EventArgs e)
		{
			if (!_isEditing || _eventHandle == null) return;

			if (_fileTextBox.Text != "")
			{
				_fileDialog.InitialDirectory = Path.GetFullPath(_fileTextBox.Text);
				_fileDialog.FileName = Path.GetFileName(_fileTextBox.Text);
			}

			if (_fileDialog.ShowDialog() != DialogResult.Cancel)
			{
				_eventHandle.FileName = _fileTextBox.Text = _fileDialog.FileName;
			}

			if (HandlePanel != null)
				HandlePanel.HandleChange();
		}
	}
}
