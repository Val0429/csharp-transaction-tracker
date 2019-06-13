﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;

using Manager = SetupBase.Manager;

namespace SetupDivision
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
		        {"Control_Region", "Region"},
		        {"SetupRegion_NewRegion", "New Region"},

		        {"SetupRegion_DeleteRegion", "Delete Region"}
		    };

            Localizations.Update(Localization);

			Name = "Region";
			TitleName = Localization["Control_Region"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;

			BackgroundImage = Manager.Background;
			//---------------------------
			Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_Region"] };
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

			var text = TitleName + "  /  " + Localization["SetupRegion_DeleteRegion"];

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

			if (PTS != null && PTS.POS.DivisionManager.Count > 0)
				buttons = "Delete";
            
			OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "", buttons)));
		}

		private void ListPanelOnDivisionEdit(Object sender, EventArgs<IDivision> e)
		{
            EditItem(e.Value);
		}

		private void ListPanelOnDivisionAdd(Object sender, EventArgs e)
		{
            IDivision division = new Division
                {
					Regions = new Dictionary<ushort, IRegion>()
				};

            division.Id = PTS.POS.GetNewDivisionId();
            division.ReadyState = ReadyState.New;

            division.Name = Localization["SetupRegion_NewRegion"] + @" " + division.Id;

            PTS.POS.DivisionManager[division.Id] = division;

            EditItem(division);
		}

		private void EditItem(IDivision item)
		{
            _focusControl = _editPanel;
            _editPanel.Division = item;

            Manager.ReplaceControl(_listPanel, _editPanel, contentPanel, ManagerMoveToEditComplete);
        }

        private void ManagerMoveToEditComplete()
		{
			_editPanel.ParseDivision();

			var text = TitleName + "  /  " + _editPanel.Division;

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