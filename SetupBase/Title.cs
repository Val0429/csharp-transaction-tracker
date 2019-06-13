using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;
using PanelBase;

namespace SetupBase
{
	public sealed partial class Title : UserControl, IControl
	{
		public event EventHandler<EventArgs<String>> OnSelectionChange;

		public String TitleName
		{
			get { return _item; }
			set { _item = value; }
		}
		public Dictionary<String, String> Localization;

		private static readonly Image _leftBack = Resources.GetResources(Properties.Resources.back_left, Properties.Resources.IMGBackLeft);
		private static readonly Image _rightBack = Resources.GetResources(Properties.Resources.back_right, Properties.Resources.IMGBackRight);
		private static readonly Image _bg = Resources.GetResources(Properties.Resources.back, Properties.Resources.IMGBack);
		public Title()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_IconTitle", "Setup"},
								   {"Control_ReportTitle", "Report"},
								   
								   {"Common_Back", "Back"},

								   {"SetupTitle_RestoreSetting", "Restore Setting"},
								   {"SetupTitle_UpgradeFirmware", "Upgrade Firmware"},
								   {"SetupTitle_UpdateEthernet", "Update"},
								   {"SetupTitle_Format", "Format"},
								   {"SetupTitle_EditSetup", "Edit Setup"},
								   {"SetupTitle_Sync", "Sync"},
								   {"SetupTitle_Refresh", "Refresh"},
								   {"SetupTitle_Search", "Search Device"},
								   {"SetupTitle_Delete", "Delete"},
								   {"SetupTitle_Clone", "Clone"},
								   {"SetupTitle_CopySetting", "Copy Setting"},
								   {"SetupTitle_Confirm", "Confirm"},
								   {"SetupTitle_Duplicate", "Duplicate"},
								   {"SetupTitle_CopyEventHandling", "Copy Event Handling"},
								   {"SetupTitle_CopySchedule", "Copy Schedule"},
								   {"SetupTitle_CopyExceptionReport", "Copy Exception Report"},
								   {"SetupTitle_OnlineRegistration", "Online Registration"},
								   {"SetupTitle_OfflineRegistration", "Offline Registration"},
								   {"SetupTitle_SaveAs", "Save as..."},
								   {"SetupTitle_SearchException", "Search Exception"},
								   {"SetupTitle_SendTestEmail", "Send test email"},
								   {"SetupTitle_ClearConditional", "Clear conditional"},
								   
								   {"SetupTitle_Save", "Save"},
								   {"SetupTitle_SaveSetting", "Save Setting"},
								   {"SetupTitle_ExportTemplate", "Export template"},
								   {"SetupTitle_ImportTemplate", "Import template"},
								   
								   {"SetupTitle_SaveReport", "Save Report"},

								   {"SetupTitle_OpenWebPage", "Open web page"},

								   {"SetupTitle_SearchEvent", "Search Event"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			Dock = DockStyle.Top;

			BackgroundImage = Resources.GetResources(Properties.Resources.controllerBG, Properties.Resources.IMGControllerBG);
		}

		public void Initialize()
		{
			TitleName = Localization["Control_IconTitle"];

			flowLayoutPanel.Visible = false;
			Paint += TitlePaint;
			backButton.Paint += BackButtonPaint;
		}

		public void ChangeDisplayTitleToReport()
		{
			TitleName = Localization["Control_ReportTitle"];
			Invalidate();
		}

		private void TitlePaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			SizeF fSize = g.MeasureString(_item, Manager.Font);
			g.DrawString(_item, Manager.Font, Brushes.White, (Width - fSize.Width) / 2, 7);
		}

		private void BackButtonPaint(Object sender, PaintEventArgs e)
		{
			if(_previous == "") return;

			Graphics g = e.Graphics;

			g.DrawImage(_bg, _leftBack.Width - 2, 0, backButton.Width - _leftBack.Width - _rightBack.Width + 4, backButton.Height);

			g.DrawImage(_leftBack, 0, 0);

			g.DrawImage(_rightBack, backButton.Width - _rightBack.Width, 0);

			g.DrawString(backButton.Text, Manager.Font, Brushes.White, 11, 7);
		}

		public void Activate()
		{
		}

		public void Deactivate()
		{
		}

		private String _from = "";
		private String _item = "Setup";
		private String _previous = "";

		public void DisplayTitle(Object sender, EventArgs<String> e)
		{
			if (flowLayoutPanel.Controls.Count > 0)
			{
				foreach (Button button in flowLayoutPanel.Controls)
				{
					button.MouseClick -= ButtonMouseClick;
					Manager.UnregistButton(button);
				}
				flowLayoutPanel.Controls.Clear();
			}

			XmlDocument xmlDoc = Xml.LoadXml(e.Value);

			_item = Xml.GetFirstElementValueByTagName(xmlDoc, "Item");
			String previous = Xml.GetFirstElementValueByTagName(xmlDoc, "Previous");
			_from = Xml.GetFirstElementValueByTagName(xmlDoc, "From");
			String buttons = Xml.GetFirstElementValueByTagName(xmlDoc, "Buttons");

			if (buttons != "")
			{
				String[] array = buttons.Split(',');
				flowLayoutPanel.Visible = true;

				foreach (String name in array)
				{
					Button button = Manager.RegistButton();
					button.MouseClick += ButtonMouseClick;
					button.Name = name;
					button.Text = Localization.ContainsKey("SetupTitle_" + name)
									  ? Localization["SetupTitle_" + name]
									  : name;
					
					flowLayoutPanel.Controls.Add(button);
				}
			}

			_previous = previous;
			if (previous == "Back")
			{
				backButton.Text = Localization["Common_Back"]; //for auto size
			}
			else
			{
				backButton.Text = Localization.ContainsKey("SetupTitle_" + previous)
								  ? Localization["SetupTitle_" + previous]
								  : previous;
			}
			backButton.Visible = (_previous != "");

			Invalidate();
		}

		private void ButtonMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(SelectionChangedXml(_from, ((Control)sender).Name)));
		}

		private static String SelectionChangedXml(String from, String item)
		{
			var xmlDoc = new XmlDocument();

			XmlElement xmlRoot = xmlDoc.CreateElement("XML");
			xmlDoc.AppendChild(xmlRoot);

			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "From", from));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Item", item));

			return xmlDoc.InnerXml;
		}

		private void BackButtonMouseClick(Object sender, MouseEventArgs e)
		{
			if (flowLayoutPanel.Controls.Count > 0)
			{
				foreach (Button button in flowLayoutPanel.Controls)
				{
					button.MouseClick -= ButtonMouseClick;
					Manager.UnregistButton(button);
				}
				 flowLayoutPanel.Controls.Clear();
			}
			backButton.Visible = false;
			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(SelectionChangedXml(_from, _previous)));
		}
	}
}
