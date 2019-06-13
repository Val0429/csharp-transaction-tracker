using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using App;
using Constant;
using Device;
using Interface;
using PanelBase;

namespace Interrogation
{
    public partial class EventSearch : Investigation.EventSearch.EventSearch
    {
        public new SearchPanel SearchPanel;

        private static readonly Image _icon = Resources.GetResources(Properties.Resources.smartSearchIcon, Properties.Resources .IMGSmartSearchIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.smartSearchIcon_activate, Properties.Resources.IMGSmartSearchIconActivate);

        public EventSearch()
        {
            Localization.Add("Control_InterrogationSearch", "Interrogation Search");
            Localizations.Update(Localization);

            TitleName = Localization["Control_InterrogationSearch"];

            InitializeComponent();

            Icon = new ControlIconButton { Image = _iconActivate, BackgroundImage = ControlIconButton.IconBgActivate };
        }

        public override void Initialize()
        {
            SearchCriteria.Event.Add(EventType.UserDefine);
            CriteriaPanel = new CriteriaPanel
            {
                NVR = _nvr,
                SearchCriteria = SearchCriteria
            };

            SearchPanel = new SearchPanel
            {
                NVR = _nvr,
                App = App,
                SearchCriteria = SearchCriteria
            };
            SearchPanel.OnPlayback += SearchPanelOnPlayback;

            base.Initialize();

        }

        protected override void SearchPanelOnPlayback(Object sender, EventArgs<List<CameraEvents>, DateTime, UInt64, UInt64> e)
        {
            var a = 1;

            var list = e.Value1 as List<CameraEvents>;
            var camera = list.FirstOrDefault().Device;
            var timecode = DateTimes.ToUtc(e.Value2, Server.Server.TimeZone);

            App.SwitchPage("Playback", new PlaybackParameter
            {
                Device = camera,
                Timecode = timecode,
                TimeUnit = TimeUnit.Unit1Senond
            });
        }

        public override void SelectionChange(Object sender, EventArgs<string> e)
        {
            String item;
			if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
				return;

            switch (item)
            {
                case "ClearConditional":
                    (CriteriaPanel as CriteriaPanel).ClearCriteria();

                    SetDefaultCondition();
                    SearchCriteria.Event.Add(EventType.UserDefine);

					CriteriaPanel.Invalidate();
                    if(Server is ICMS)
                        NVRListPanel.Invalidate();
					break;

                default :
                    base.SelectionChange(sender, e);
                    break;
            }
        }

        protected override void SearchEvent()
        {
            _isSearching = true;
            _focusControl = SearchPanel;

            SearchPanel.SearchName = ((CriteriaPanel) CriteriaPanel).CriteriaName;
            SearchPanel.NumberofRecord = ((CriteriaPanel)CriteriaPanel).CriteriaNumberofRecord;

            SearchPanel.ClearResult();

            Manager.ReplaceControl(CriteriaPanel, SearchPanel, contentPanel, ManagerMoveToSearchComplete);
        }

        protected override void ManagerMoveToSearchComplete()
        {
            SearchPanel.Focus();

            SearchPanel.SearchEvent();
            var text = TitleName + "  /  " + Localization["Investigation_SearchResult"];

            RaiseOnSelectionChange(Manager.SelectionChangedXml(TitleName, text, "Back", ""));
        }

        public override void SaveReport()
        {
            if (_focusControl != SearchPanel) return;

            SearchPanel.SaveReport();
        }
        
        
        public override void Minimize()
        {
            base.Minimize();

            Icon.Image = _icon;
            Icon.BackgroundImage = null;
        }

        public override void Maximize()
        {
            base.Maximize();

            Icon.Image = _iconActivate;
            Icon.BackgroundImage = ControlIconButton.IconBgActivate;
        }
    }
}
