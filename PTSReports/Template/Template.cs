using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using App;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;
using ApplicationForms = PanelBase.ApplicationForms;

namespace PTSReports.Template
{
    public sealed partial class Template : UserControl, IControl, IAppUse, IServerUse, IBlockPanelUse, IMinimize
	{
        public event EventHandler OnMinimizeChange;
		public event EventHandler<EventArgs<String>> OnSelectionChange;

        public Button Icon { get; private set; }
        private static readonly Image _icon = Resources.GetResources(Properties.Resources.templateIcon, Properties.Resources.IMGTemplateIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.templateIcon_activate, Properties.Resources.IMGTemplateIconActivate);
        public UInt16 MinimizeHeight
        {
            get { return 0; }
        }
        public Boolean IsMinimize { get; private set; }

		public IApp App { get; set; }
		public IPTS PTS;
		private IServer _server;
		public IServer Server
		{
			get { return _server; }
			set
			{
				_server = value;
				if (value is IPTS)
					PTS = value as IPTS;
			}
		}
		public IBlockPanel BlockPanel { get; set; }
		public Dictionary<String, String> Localization;

		public String TitleName { get; set; }

		private ListPanel _listPanel;
		private AdvancedSearch.AdvancedSearch _advancedSearch;

		private Control _focusControl;

		private readonly FolderBrowserDialog _folderBrowserDialog;
		private readonly OpenFileDialog _openFileDialog;

		private Dictionary<POS_Exception.TemplateConfig, AdvancedSearch.AdvancedSearch> _configDic = new Dictionary<POS_Exception.TemplateConfig, AdvancedSearch.AdvancedSearch>();
		public Template()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},
								   {"MessageBox_Error", "Error"},
								   {"Application_SaveCompleted", "Save Completed"},

								   {"Control_ReportTemplate", "Template"},
								   {"PTSReports_SearchResult", "Search Result"},
								   
								   {"PTSReports_EditTemplate", "Edit Template"},
								   {"PTSReports_DeleteTemplate", "Delete Template"},
								   {"PTSReports_TemplateFormatError", "Template file XML format error."},
							   };
			Localizations.Update(Localization);
			
			Name = "ReportTemplate";
			TitleName = Localization["Control_ReportTemplate"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			BackgroundImage = Resources.GetResources(Properties.Resources.bg, Properties.Resources.IMGBg);

			_folderBrowserDialog = new FolderBrowserDialog
			{
				Description = @"Select the directory that you want to save template config.",
				SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
			};
			
			_openFileDialog = new OpenFileDialog
			{
				Filter = @"template files (*.xml)|*.xml",
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
				RestoreDirectory = true
			};

            //---------------------------
            Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_ReportTemplate"] };
            Icon.Click += DockIconClick;

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);
            //---------------------------
		}

		public void Initialize()
		{
			if (Parent is IControlPanel)
				BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

			_listPanel = new ListPanel
			{
				PTS = PTS,
			};
			_listPanel.Initialize();

			//_advancedSearch = new AdvancedSearch.AdvancedSearch
			//{
			//    Server = Server
			//};
			//_advancedSearch.OnSelectionChange += AdvancedSearchOnSelectionChange;
			//_advancedSearch.Initialize();

			_listPanel.OnTemplateAdd += ListPanelOnTemplateAdd;
			_listPanel.OnTemplateEdit += ListPanelOnTemplateEdit;

			contentPanel.Controls.Add(_listPanel);
		}
		
		public void SelectionChange(Object sender, EventArgs<String> e)
		{
			String item;
			if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
				return;

			XmlDocument xmlDoc = Xml.LoadXml(e.Value);

			XmlElement fromNode;
			switch (item)
			{
				case "Confirm":
					_listPanel.RemoveSelectedTemplate();

					var temp = new List<POS_Exception.TemplateConfig>();
					temp.AddRange(_configDic.Keys);
					foreach (var config in temp)
					{
						if(PTS.POS.TemplateConfigs.Contains(config)   ) continue;
						_configDic[config].Dispose();
						_configDic.Remove(config);
					}
					temp.Clear();

					ShowTemplateList();
					break;

				case "Delete":
					DeleteTemplate();
					break;

				case "ExportTemplate":
					ExportTemplate();
					break;

				case "ImportTemplate":
					ImportTemplate();
					break;

				case "Save":
					SaveTemplate();
					break;

				case "SaveReport":
					SaveReport();
					break;
					
				case "SearchException":
				case "ClearConditional":
					if (_advancedSearch != null)
					{
						fromNode = Xml.GetFirstElementByTagName(xmlDoc, "From");
						fromNode.InnerText = _advancedSearch.TitleName;
						_advancedSearch.SelectionChange(sender, new EventArgs<String>(xmlDoc.InnerXml));
					}
					break;

				default:
					if (item == TitleName || item == "Back")
					{
						if (_advancedSearch != null && !_advancedSearch.IsAtIndexPage)
						{
							fromNode = Xml.GetFirstElementByTagName(xmlDoc, "From");
							fromNode.InnerText = _advancedSearch.TitleName;
							_advancedSearch.SelectionChange(sender, new EventArgs<String>(xmlDoc.InnerXml));

							return;
						}

						ShowTemplateList();
					}
					break;
			}
		}

		private void AdvancedSearchOnSelectionChange(Object sender, EventArgs<String> e)
		{
			if (_focusControl != _advancedSearch) return;

			var xmlDoc = Xml.LoadXml(e.Value);
			var fromNode = Xml.GetFirstElementByTagName(xmlDoc, "From");
			var itemNode = Xml.GetFirstElementByTagName(xmlDoc, "Item");
			var backtoRoot = (fromNode.InnerText == itemNode.InnerText);
			fromNode.InnerText = TitleName;
			
			if (backtoRoot)
				itemNode.InnerText = Localization["PTSReports_EditTemplate"];

			var previousNode = Xml.GetFirstElementByTagName(xmlDoc, "Previous");
			previousNode.InnerText = "Back";

			//var buttonsNode = Xml.GetFirstElementByTagName(xmlDoc, "Buttons");
			//if(_advancedSearch.IsAtIndexPage)
			//    buttonsNode.InnerText = "SearchException,ClearConditional";
			//else
			//    buttonsNode.InnerText = "";

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(xmlDoc.InnerXml));
		}

		private void ListPanelOnTemplateAdd(Object sender, EventArgs e)
		{
			var remplateConfig = new POS_Exception.TemplateConfig
			{
				DateTimeSet = DateTimeSet.Today
			};

			foreach (IPOS obj in PTS.POS.POSServer)
			{
				remplateConfig.POSCriterias.Add(new POS_Exception.POSCriteria
				{
					POSId = obj.Id,
					Equation = POS_Exception.Comparative.Equal
				});
			}
			PTS.POS.TemplateConfigs.Add(remplateConfig);
			EditTemplate(remplateConfig);
		}

		private void ListPanelOnTemplateEdit(Object sender, EventArgs<POS_Exception.TemplateConfig> e)
		{
			EditTemplate(e.Value);
		}

		private void EditTemplate(POS_Exception.TemplateConfig config)
		{
			//every time edit, use a NEW advance~
			if (_configDic.ContainsKey(config))
			{
				_advancedSearch = _configDic[config];
			}
			else
			{
				_advancedSearch = new AdvancedSearch.AdvancedSearch
				{
					App = App,
					Server = Server,
					SetDefaultPOSConfig = false,
					SearchCriteria = config
				};

				_advancedSearch.OnSelectionChange += AdvancedSearchOnSelectionChange;
				_advancedSearch.Initialize();
				_advancedSearch.POSPanel.ParseSetting();
				_advancedSearch.CashierIdPanel.ParseSetting();
				_advancedSearch.CashierPanel.ParseSetting();
				_advancedSearch.ExceptionAmountPanel.ParseSetting();
				_advancedSearch.TagPanel.ParseSetting();
				_advancedSearch.KeywordPanel.ParseSetting();
                _advancedSearch.TimeIntervalPanel.ParseSetting();
                _advancedSearch.CountingPanel.ParseSetting();

				_configDic.Add(config, _advancedSearch);
			}

			_focusControl = _advancedSearch;

			Manager.ReplaceControl(_listPanel, _advancedSearch.RootPanel, contentPanel, ManagerMoveToEditComplete);
		}

		private void ShowTemplateList()
		{
			if (_advancedSearch != null)
				_advancedSearch.Deactivate();
			
			_focusControl = _listPanel;
			
			//_listPanel.Enabled = true;
			if (!contentPanel.Controls.Contains(_listPanel))
			{
				contentPanel.Controls.Clear();
				contentPanel.Controls.Add(_listPanel);
			}

			_listPanel.GenerateViewModel();

			var button = "ImportTemplate,Save";
			if (PTS.POS.TemplateConfigs.Count > 0)
				button = "Delete,ExportTemplate,ImportTemplate,Save";

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName,
					"", button)));
		}

		private Boolean _isActivate;
		public void Activate()
		{
			if (!BlockPanel.IsFocusedControl(this)) return;

			_isActivate = true;
		}

		public void Deactivate()
		{
			_isActivate = false;
		}

        private void DockIconClick(Object sender, EventArgs e)
        {
            if (IsMinimize)
                Maximize();
            else //dont hide self to keep at last selection panel on screen
                ShowTemplateList();
        }

        public void Minimize()
        {
            if (BlockPanel.LayoutManager.Page.Version == "2.0" && !IsMinimize)
                BlockPanel.HideThisControlPanel(this);

            Deactivate();
            ((IconUI2)Icon).IsActivate = false;

            IsMinimize = true;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

        public void Maximize()
        {
            ShowContent(this, null);

            ((IconUI2)Icon).IsActivate = true;

            IsMinimize = false;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

		public void ShowContent(Object sender, EventArgs<String> e)
		{
			if (BlockPanel != null)
				BlockPanel.ShowThisControlPanel(this);

			//do nothing stay at search result
			if (_advancedSearch != null && _advancedSearch.IsAtSearchPanel)
			{
				if (OnSelectionChange != null)
					OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
						Localization["PTSReports_SearchResult"], "Back", "SaveReport")));

				return;
			}

			ShowTemplateList();
		}

		private void ManagerMoveToEditComplete()
		{
			_advancedSearch.ShowCriteriaPanel();

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, Localization["PTSReports_EditTemplate"],
					"Back", "SearchException,ClearConditional")));
		}

		public void SaveReport(Object sender, EventArgs e)
		{
			if (!_isActivate) return;

			SaveReport();
		}

		public void SaveReport()
		{
			if (_advancedSearch == null || _focusControl != _advancedSearch) return;

			_advancedSearch.SaveReport();
		}

		private void DeleteTemplate()
		{
			_listPanel.ShowCheckBox();

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
						 Localization["PTSReports_DeleteTemplate"],
						 "Back", "Confirm")));
		}

		private void ExportTemplate()
		{
			var result = _folderBrowserDialog.ShowDialog();
			if (result != DialogResult.OK) return;

			var folderName = _folderBrowserDialog.SelectedPath;

			var fileName = "\\template" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xml";
			PTS.POS.ConvertTemplateToXmlDocument().Save(folderName + fileName);
			Process.Start("explorer.exe", "/select," + folderName + fileName);
		}

		private readonly CultureInfo _enus = new CultureInfo("en-US");
		private void ImportTemplate()
		{
			var result = _openFileDialog.ShowDialog();
			if (result != DialogResult.OK) return;

			var fileName = _openFileDialog.FileName;
			
			var file = new FileInfo(fileName);
			if (!file.Exists || file.Extension.ToUpper(_enus) != ".XML")
			{
				TopMostMessageBox.Show(Localization["PTSReports_TemplateFormatError"], Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}
			
			var count = PTS.POS.TemplateConfigs.Count;

			try
			{
				var xmlDoc = new XmlDocument();
				xmlDoc.Load(fileName);

				PTS.POS.ConvertXmlDocumentToTemplate(xmlDoc);

				if (count != PTS.POS.TemplateConfigs.Count)
				{
					ShowTemplateList();
				}
				else
				{
					TopMostMessageBox.Show(Localization["PTSReports_TemplateFormatError"], Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Stop);
				}
			}
			catch (System.Exception)
			{
				TopMostMessageBox.Show(Localization["PTSReports_TemplateFormatError"], Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		private void SaveTemplate()
		{
			ApplicationForms.ShowLoadingIcon(Server.Form);
			Application.RaiseIdle(null);
			//call CGI save to server
			SaveTemplateDelegate saveTemplateDelegate = PTS.POS.SaveTemplate;
			saveTemplateDelegate.BeginInvoke(SaveTemplateCallback, saveTemplateDelegate);
			
			//PTS.POS.SaveTemplate();
		}

		private delegate void SaveTemplateDelegate();
		private delegate void SaveTemplateCallbackDelegate(IAsyncResult result);
		private void SaveTemplateCallback(IAsyncResult result)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke(new SaveTemplateCallbackDelegate(SaveTemplateCallback), result);
				}
				catch (System.Exception)
				{
				}
				return;
			}

			((SaveTemplateDelegate)result.AsyncState).EndInvoke(result);

			TopMostMessageBox.Show(Localization["Application_SaveCompleted"],
				Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

			ApplicationForms.HideLoadingIcon();
		}
	}
}
