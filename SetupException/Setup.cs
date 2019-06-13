using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupException
{
	public sealed partial class Setup : UserControl, IControl, IServerUse, IBlockPanelUse, IMinimize
	{
		public event EventHandler OnMinimizeChange;
		public event EventHandler<EventArgs<String>> OnSelectionChange;

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

		public Button Icon { get; private set; }
		private static readonly Image _icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
		private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);

		public UInt16 MinimizeHeight
		{
			get { return 0; }
		}
		public Boolean IsMinimize { get; private set; }

		private ListPanel _listPanel;
		private EditPanel _editPanel;
		private ThresholdPanel _thresholdPanel;
		private ExceptionColorPanel _exceptionColorPanel;

		private Control _focusControl;
		public String TitleName { get; set; }
		
		public Setup()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_Exception", "Exception"},

								   {"MessageBox_Information", "Information"},
								   {"MessageBox_Error", "Error"},
								   {"MessageBox_Confirm", "Confirm"},
								   
								   {"SetupException_NewException", "New Exception"},
								   {"SetupException_DeleteException", "Delete Exception"},
								   {"POS_Threshold", "Threshold"},
								   {"POS_ExceptionColor", "Exception Color"},
								   {"SetupPOS_MaximumLicense", "Reached maximum license limit \"%1\""},
							   };
			Localizations.Update(Localization);

			Name = "Exception";
			TitleName = Localization["Control_Exception"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			BackgroundImage = Manager.Background;
			//---------------------------
			Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_Exception"] };
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
				PTS = PTS
			};
			_listPanel.Initialize();

			_editPanel = new EditPanel
			{
				PTS = PTS
			};
			_editPanel.Initialize();

			_thresholdPanel = new ThresholdPanel
			{
				PTS = PTS
			};
			_thresholdPanel.Initialize();

			_exceptionColorPanel = new ExceptionColorPanel
			{
				PTS = PTS
			};
			_exceptionColorPanel.Initialize();

			_listPanel.OnExceptionEdit += ListPanelOnExceptionEdit;
			_listPanel.OnExceptionAdd += ListPanelOnExceptionAdd;
			_listPanel.OnThresholdEdit += ListPanelOnThresholdEdit;
			_listPanel.OnExceptionColorEdit += ListPanelOnExceptionColorEdit;

			contentPanel.Controls.Contains(_listPanel);
		}

		public void Activate()
		{
		}

		public void Deactivate()
		{
		}

		public void ShowContent(Object sender, EventArgs<String> e)
		{
			BlockPanel.ShowThisControlPanel(this);

			ShowExceptionList();
		}

		public void SelectionChange(Object sender, EventArgs<String> e)
		{
			String item;
			if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
				return;

			switch (item)
			{
				case "Confirm":
					_listPanel.RemoveSelectedException();

					ShowExceptionList();
					break;

				case "Delete":
					DeleteException();
					break;

				default:
					if (item == TitleName || item == "Back")
					{
						Manager.ReplaceControl(_focusControl, _listPanel, contentPanel, ShowExceptionList);
					}
					break;
			}
		}

		private void DeleteException()
		{
			_listPanel.SelectedColor = Manager.DeleteTextColor;
			_listPanel.ShowCheckBox();

			var text = TitleName + "  /  " + Localization["SetupException_DeleteException"];

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "Confirm")));
		}

		private void ShowExceptionList()
		{
			_focusControl = _listPanel;

			_listPanel.Enabled = true;
			if (!contentPanel.Controls.Contains(_listPanel))
			{
				contentPanel.Controls.Clear();
				contentPanel.Controls.Add(_listPanel);
			}

			_listPanel.GenerateViewModel();

			if (OnSelectionChange != null)
			{
				String buttons = "";

				if (PTS != null && PTS.POS.Exceptions.Count > 0)
					buttons = "Delete";

				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName,
					"", buttons)));
			}
		}

		private void ListPanelOnExceptionEdit(Object sender, EventArgs<POS_Exception> e)
		{
			EditException(e.Value);
		}

		private void ListPanelOnThresholdEdit(Object sender, EventArgs e)
		{
			_focusControl = _thresholdPanel;

			Manager.ReplaceControl(_listPanel, _thresholdPanel, contentPanel, ManagerMoveToThresholdComplete);
		}

		private void ListPanelOnExceptionColorEdit(Object sender, EventArgs e)
		{
			_focusControl = _exceptionColorPanel;

			Manager.ReplaceControl(_listPanel, _exceptionColorPanel, contentPanel, ManagerMoveToExceptionColorPanelComplete);
		}

		private void ListPanelOnExceptionAdd(Object sender, EventArgs e)
		{
			POS_Exception exception = null;
			if (PTS != null)
			{
				exception = new POS_Exception
				{
					Id = PTS.POS.GetNewExceptionId(),
					Manufacture = POS_Exception.Manufactures[0]
				};
				POS_Exception.SetDefaultExceptions(exception);
				POS_Exception.SetDefaultSegments(exception);
				POS_Exception.SetDefaultTags(exception);
			}

			if (exception == null) return;

			if (exception.Id == 0)
			{
				TopMostMessageBox.Show(Localization["SetupPOS_MaximumLicense"].Replace("%1", Server.License.Amount.ToString()),
					Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			exception.Name = Localization["SetupException_NewException"] + @" " + exception.Id;

			if (!PTS.POS.Exceptions.ContainsKey(exception.Id))
			{
				PTS.POS.Exceptions.Add(exception.Id, exception);
			}

			EditException(exception);
		}

		private void EditException(POS_Exception exception)
		{
			_focusControl = _editPanel;
			_editPanel.POSException = exception;

			Manager.ReplaceControl(_listPanel, _editPanel, contentPanel, ManagerMoveToEditComplete);
		}

		private void ManagerMoveToEditComplete()
		{
			_editPanel.ParsePOSException();
			var text = TitleName + "  /  " + _editPanel.POSException;

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "")));
		}

		private void ManagerMoveToThresholdComplete()
		{
			_thresholdPanel.ScrollTop();

			var text = TitleName + "  /  " + Localization["POS_Threshold"];

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "")));
		}

		private void ManagerMoveToExceptionColorPanelComplete()
		{
			_exceptionColorPanel.RefreshColor();
			//_exceptionColorPanel.ScrollTop();

			var text = TitleName + "  /  " + Localization["POS_ExceptionColor"];

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "")));
		}
		private void DockIconClick(Object sender, EventArgs e)
		{
			if (IsMinimize)
				Maximize();
			else //dont hide self to keep at last selection panel on screen
				ShowExceptionList();
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
	}
}
