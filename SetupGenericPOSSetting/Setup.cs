using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Device;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupGenericPOSSetting
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
        private EditTransactionPanel _editTransactionPanel;
		private Control _focusControl;
		public String TitleName { get; set; }
		
		public Setup()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_GenericPOSSetting", "Generic POS Setting"},

								   {"MessageBox_Information", "Information"},
								   {"MessageBox_Error", "Error"},
								   {"MessageBox_Confirm", "Confirm"},
								   {"SetupPOSConnection_NewPOSConnection", "New POS Connection"},
								   {"SetupException_NewException", "New Exception"},
								   {"SetupException_DeleteException", "Delete Exception"},
								   {"POS_Threshold", "Threshold"},
								   {"POS_ExceptionColor", "Exception Color"},
								   {"SetupPOS_MaximumLicense", "Reached maximum license limit \"%1\""}
							   };
			Localizations.Update(Localization);

            Name = "GenericPOSSetting";
            TitleName = Localization["Control_GenericPOSSetting"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			BackgroundImage = Manager.Background;
			//---------------------------
            Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_GenericPOSSetting"] };
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

		    _editTransactionPanel = new EditTransactionPanel
		    {
		        PTS = PTS
		    };
            _editTransactionPanel.Initialize();

			//_listPanel.OnExceptionEdit += ListPanelOnExceptionEdit;
			_listPanel.OnPOSSettingAdd += ListPanelOnExceptionAdd;
            _listPanel.OnPOSSettingEdit += ListPanelOnPOSSettingEdit;
			//_listPanel.OnExceptionColorEdit += ListPanelOnExceptionColorEdit;

			contentPanel.Controls.Contains(_listPanel);

            PTS.Form.FormClosing += FormFormClosing;
		}

		public void Activate()
		{
		}

		public void Deactivate()
		{
            _editTransactionPanel.Stop();
		}

        private void FormFormClosing(object sender, FormClosingEventArgs e)
        {
            _editTransactionPanel.Stop();
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

                case "SaveSetting":
                    SaveSetting();
                    break;

				default:
					if (item == TitleName || item == "Back")
					{
                        //if(_focusControl == _editPanel)
                        //    _editPanel.Stop();
						Manager.ReplaceControl(_focusControl, _listPanel, contentPanel, ShowExceptionList);
					}
					break;
			}
		}

        private void SaveSetting()
        {
            _editTransactionPanel.SaveSetting();
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
		    ListPanelOnPOSSettingEdit(this, null);
		    return;

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

		private void ListPanelOnExceptionAdd(Object sender, EventArgs e)
		{
            if (PTS == null) return;

		    var posConnection = CreatePOSSetting();
            if (posConnection == null) return;
            EditPOSConnection(posConnection);
		}

        private IPOSConnection CreatePOSSetting()
        {
            if (PTS == null)
            {
                _editTransactionPanel.EnableButtons(false);
                return null;
            }
            //----------------Exception--------------------
            POS_Exception exception = null;
            foreach (KeyValuePair<UInt16, POS_Exception> posException in PTS.POS.Exceptions)
            {
                if (posException.Value.IsCapture)
                {
                    exception = posException.Value;
                    exception.TransactionType = 3;
                    break;
                }
            }

            if (exception == null)
            {
                exception = new POS_Exception
                {
                    Id = PTS.POS.GetNewExceptionId(true),
                    Manufacture = "Generic",
                    TransactionType = 3,
                    IsCapture = true
                };
            }

            if (exception.Id == 0)
            {
                _editTransactionPanel.EnableButtons(false);
                TopMostMessageBox.Show(Localization["SetupPOS_MaximumLicense"].Replace("%1", Server.License.Amount.ToString()),
                    Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            exception.Name = Localization["SetupException_NewException"] + @" " + exception.Id;

            //if (!PTS.POS.Exceptions.ContainsKey(exception.Id))
            //{
            //    PTS.POS.Exceptions.Add(exception.Id, exception);
            //}
            _editPanel.POSException = exception;
            _editTransactionPanel.POSSettingException = exception;

            //-----------------Connection------------------------------------------------
            IPOSConnection posConnection = null;
            foreach (KeyValuePair<ushort, IPOSConnection> connection in PTS.POS.Connections)
            {
                if (connection.Value.IsCapture)
                {
                    posConnection = connection.Value;
                    break;
                }
            }

            if (posConnection == null)
            {
                posConnection = new POSConnection
                {
                    Id = PTS.POS.GetNewConnectionId(true),
                    Protocol = "UDP",
                    IsCapture = true,
                    Manufacture = "Generic"
                };
                posConnection.SetDefaultAuthentication();
            }

            if (posConnection.Id == 0)
            {
                _editTransactionPanel.EnableButtons(false);
                TopMostMessageBox.Show(Localization["SetupPOS_MaximumLicense"].Replace("%1", Server.License.Amount.ToString()),
                    Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            posConnection.ReadyState = ReadyState.New;
            posConnection.Name = Localization["SetupPOSConnection_NewPOSConnection"] + @" " + posConnection.Id;

            //if (!PTS.POS.Connections.ContainsKey(posConnection.Id))
            //{
            //    PTS.POS.Connections.Add(posConnection.Id, posConnection);
            //}
            //----------------POS----------------------------
            POS pos = null;
            if (PTS != null)
            {
                pos = new POS
                {
                    LicenseId = PTS.POS.GetNewPOSLicenseId(),
                    Manufacture = "Generic",
                    IsCapture = true
                };
            }

            pos.Id = "PTSDemo";

            //AddDefaultExceptionIfThereIsNone(pos);

            //AddDefaultConnectionIfThereIsNone(pos);

            pos.ReadyState = ReadyState.New;
            pos.Exception = exception.Id;
            pos.Name = @"POS " + pos.Id;

            if (posConnection.POS.Count == 0)
            {
                posConnection.POS.Add(pos);
            }

            _editTransactionPanel.EnableButtons(true);
            return posConnection;
        }

        private void EditPOSConnection(IPOSConnection genericPosSetting)
        {
            _focusControl = _editPanel;
            _editPanel.GenericPosSetting = genericPosSetting;
            
            Manager.ReplaceControl(_listPanel, _editPanel, contentPanel, ManagerMoveToEditComplete);
        }

        private void ManagerMoveToEditComplete()
        {
            _editPanel.ParsePOSConnection();
            var text = TitleName + "  /  " + _editPanel.GenericPosSetting;

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "")));
        }

        private void ListPanelOnPOSSettingEdit(object sender, EventArgs e)
        {
            var posConnection = CreatePOSSetting();
            if (posConnection == null)
            {
                _focusControl = _editTransactionPanel;

                Manager.ReplaceControl(_listPanel, _editTransactionPanel, contentPanel, ManagerMoveToTransactionEditComplete);
                return;
            }
            if (_editTransactionPanel.GenericPosSetting == null)
                _editTransactionPanel.GenericPosSetting = posConnection;

            _focusControl = _editTransactionPanel;

            Manager.ReplaceControl(_listPanel, _editTransactionPanel, contentPanel, ManagerMoveToTransactionEditComplete);
        }

        private void ManagerMoveToTransactionEditComplete()
        {
            //foreach (POS_Exception posException in PTS.POS.Exceptions.Values)
            //{
            //    if(posException.Manufacture == "Generic")
            //    {
            //        _editTransactionPanel.POSException = posException;
            //        break;
            //    }
            //}
            

            _editTransactionPanel.ParsePOSException(true);
            var text = TitleName;

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "", "SaveSetting")));
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
