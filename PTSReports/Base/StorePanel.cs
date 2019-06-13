using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using Manager = SetupBase.Manager;

namespace PTSReports.Base
{
    public sealed partial class StorePanel : UserControl
    {
        public event EventHandler OnSelectChange;

        public IPTS PTS;
        public POS_Exception.SearchCriteria SearchCriteria;

        private readonly PageSelector _pageSelector;

        public StorePanel()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "Store";
            BackgroundImage = Manager.BackgroundNoBorder;

            _pageSelector = new PageSelector
            {
                PagesDisplay = 10,
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                BackColor = Color.Transparent
            };
            _pageSelector.OnSelectionChange += PageSelectorOnSelectionChange;
            pageSelectorPanel.Controls.Add(_pageSelector);
            _pageSelector.BringToFront();

        }

        private Int32 _page = 1;
        private const UInt16 CountPerPage = 20;

        private void PageSelectorOnSelectionChange(Object sender, EventArgs<Int32> e)
        {
            _page = _pageSelector.SelectPage;

            containerPanel.SuspendLayout();

            ParseSetting();

            containerPanel.ResumeLayout();
        }

        private Boolean _isEditing;
        public void ParseSetting()
        {
            _isEditing = false;

            ClearViewModel();

            var sortResult = new List<IStore>(PTS.POS.StoreManager.Values);

            if (sortResult.Count == 0) return;

            sortResult.Sort((x, y) => (x.Id - y.Id));

            _pageSelector.SelectPage = _page;
            _pageSelector.Pages = Convert.ToInt32(Math.Ceiling(sortResult.Count / (CountPerPage * 1.0)));
            _pageSelector.ShowPages();


            List<IStore> pageResult = new List<IStore>();
            var start = (_page - 1) * CountPerPage;
            var end = start - 1 + CountPerPage;
            if (end > sortResult.Count) end = sortResult.Count - 1;

            var idx = -1;
            foreach (IStore store in sortResult)
            {
                idx++;
                if (idx < start) continue;
                if (idx > end) break;

                pageResult.Insert(0, store);
            }

            var selectAll = true;
            var count = 0;
            //containerPanel.Visible = false;
            foreach (IStore store in pageResult)
            {
                SetupStore.ItemPanel storePanel = GetStorePanel();

                storePanel.Store = store;
                storePanel.EditVisible = false;
                storePanel.SelectionVisible = true;

                if (SearchCriteria.Store.Contains(store.Id))
                {
                    count++;
                    storePanel.Checked = true;
                }
                else
                    selectAll = false;

                containerPanel.Controls.Add(storePanel);
            }
            if (count == 0 && selectAll)
                selectAll = false;

            SetupStore.ItemPanel posTitlePanel = GetStorePanel();
            posTitlePanel.IsTitle = true;
            posTitlePanel.Cursor = Cursors.Default;
            posTitlePanel.EditVisible = false;
            posTitlePanel.Checked = selectAll;
            //deviceTitleControl.OnSortChange += DeviceControlOnSortChange;
            posTitlePanel.OnSelectAll += POSPanelOnSelectAll;
            posTitlePanel.OnSelectNone += POSPanelOnSelectNone;
            containerPanel.Controls.Add(posTitlePanel);
            //containerPanel.Visible = true;

            _isEditing = true;
        }

        //public void ScrollTop()
        //{
        //    //containerPanel.AutoScroll = false;
        //    containerPanel.Select();
        //    containerPanel.AutoScrollPosition = new Point(0, 0);
        //    //containerPanel.AutoScroll = true;
        //}

        private readonly Queue<SetupStore.ItemPanel> _recyclePOS = new Queue<SetupStore.ItemPanel>();
        private SetupStore.ItemPanel GetStorePanel()
        {
            if (_recyclePOS.Count > 0)
            {
                return _recyclePOS.Dequeue();
            }

            var posPanel = new SetupStore.ItemPanel
            {
                EditVisible = false,
                SelectionVisible = true,
            };

            posPanel.OnSelectChange += POSControlOnSelectChange;

            return posPanel;
        }

        private void POSControlOnSelectChange(Object sender, EventArgs e)
        {
            if (!_isEditing) return;

            var panel = sender as SetupStore.ItemPanel;
            if (panel == null) return;
            if (panel.IsTitle) return;

            var selectAll = false;
            if (panel.Checked)
            {
                if (!SearchCriteria.Store.Contains(panel.Store.Id))
                {
                    SearchCriteria.Store.Add(panel.Store.Id);
                    SearchCriteria.Store.Sort((x, y) => x - y);
                }

                selectAll = true;
                foreach (SetupStore.ItemPanel control in containerPanel.Controls)
                {
                    if (control.IsTitle) continue;
                    if (!control.Checked && control.Enabled)
                    {
                        selectAll = false;
                        break;
                    }
                }
            }
            else
            {
                SearchCriteria.Store.Remove(panel.Store.Id);
            }

            if (OnSelectChange != null)
                OnSelectChange(null, null);

            var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as SetupStore.ItemPanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= POSPanelOnSelectAll;
                title.OnSelectNone -= POSPanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += POSPanelOnSelectAll;
                title.OnSelectNone += POSPanelOnSelectNone;
            }
        }

        private void ClearViewModel()
        {
            foreach (SetupStore.ItemPanel itemPanel in containerPanel.Controls)
            {
                itemPanel.SelectionVisible = false;

                itemPanel.Checked = false;
                itemPanel.Store = null;
                itemPanel.Cursor = Cursors.Hand;
                itemPanel.EditVisible = false;
                itemPanel.SelectionVisible = true;

                if (itemPanel.IsTitle)
                {
                    itemPanel.OnSelectAll -= POSPanelOnSelectAll;
                    itemPanel.OnSelectNone -= POSPanelOnSelectNone;
                    itemPanel.IsTitle = false;
                }

                if (!_recyclePOS.Contains(itemPanel))
                {
                    _recyclePOS.Enqueue(itemPanel);
                }
            }
            containerPanel.Controls.Clear();
        }

        private void POSPanelOnSelectAll(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (SetupStore.ItemPanel control in containerPanel.Controls)
            {
                control.Checked = true;
            }
            containerPanel.AutoScroll = true;
        }

        private void POSPanelOnSelectNone(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (SetupStore.ItemPanel control in containerPanel.Controls)
            {
                control.Checked = false;
            }
            containerPanel.AutoScroll = true;
        }

        public void ScrollTop()
        {
            containerPanel.Select();
            containerPanel.AutoScrollPosition = new Point(0, 0);
        }

        public Panel ContainerPanel
        {
            get { return containerPanel; }
        }
    }
}
