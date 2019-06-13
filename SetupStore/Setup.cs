using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;

using Manager = SetupBase.Manager;

namespace SetupStore
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
		
		private Control _focusControl;
		public String TitleName { get; set; }
		
		public Setup()
		{
		    Localization = new Dictionary<String, String>
		    {
		        {"Control_Store", "Store"},
		        {"SetupStore_NewStore", "New Store"},

		        {"SetupStore_DeleteStore", "Delete Store"}
		    };

            Localizations.Update(Localization);

			Name = "Store";
			TitleName = Localization["Control_Store"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;

			BackgroundImage = Manager.Background;
			//---------------------------
			Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_Store"] };
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

			_listPanel.OnDivisionEdit += ListPanelOnDivisionEdit;
			_listPanel.OnDivisionAdd += ListPanelOnDivisionAdd;
			
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

            ShowItemList();
		}

		public void SelectionChange(Object sender, EventArgs<String> e)
		{
			String item;
			if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
				return;

			switch (item)
			{
				case "Confirm":
					_listPanel.RemoveSelectedItem();

					ShowItemList();
					break;

				case "Delete":
					DeleteItem();
					break;

				default:
					if (item == TitleName || item == "Back")
					{
						Manager.ReplaceControl(_focusControl, _listPanel, contentPanel, ShowItemList);
					}
					break;
			}
		}

		private void DeleteItem()
		{
			_listPanel.SelectedColor = Manager.DeleteTextColor;
			_listPanel.ShowCheckBox();

			var text = TitleName + "  /  " + Localization["SetupStore_DeleteStore"];

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text,
					"Back", "Confirm")));
		}

		private void ShowItemList()
		{
			_focusControl = _listPanel;

			_listPanel.Enabled = true;
			if (!contentPanel.Controls.Contains(_listPanel))
			{
				contentPanel.Controls.Clear();
				contentPanel.Controls.Add(_listPanel);
			}

			_listPanel.GenerateViewModel();

			if (OnSelectionChange == null) return;
			String buttons = "";

			if (PTS != null && PTS.POS.StoreManager.Count > 0)
				buttons = "Delete";
            
			OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "", buttons)));
		}

		private void ListPanelOnDivisionEdit(Object sender, EventArgs<IStore> e)
		{
            EditItem(e.Value);
		}

		private void ListPanelOnDivisionAdd(Object sender, EventArgs e)
		{
            IStore store = new PTSStore
                {
                    Pos = new Dictionary<string, IPOS>()
                };

            store.Id = PTS.POS.GetNewStoreId();
            store.ReadyState = ReadyState.New;

            store.Name = Localization["SetupStore_NewStore"] + @" " + store.Id;

            PTS.POS.StoreManager[store.Id] = store;

            EditItem(store);
		}

		private void EditItem(IStore item)
		{
            _focusControl = _editPanel;
            _editPanel.Store = item;

            Manager.ReplaceControl(_listPanel, _editPanel, contentPanel, ManagerMoveToEditComplete);
        }

        private void ManagerMoveToEditComplete()
		{
			_editPanel.ParseDivision();

			var text = TitleName + "  /  " + _editPanel.Store;

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "")));
		}

		private void DockIconClick(Object sender, EventArgs e)
		{
			if (IsMinimize)
				Maximize();
			else //dont hide self to keep at last selection panel on screen
                ShowItemList();
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
