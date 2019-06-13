using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using Manager = SetupBase.Manager;

namespace SetupRegion
{
    public sealed partial class ListPanel : UserControl
    {
        public event EventHandler<EventArgs<IRegion>> OnDivisionEdit;
        public event EventHandler OnDivisionAdd;
        
        public IPTS PTS;

        public Dictionary<String, String> Localization;

        private readonly PageSelector _pageSelector;
        public ListPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"MessageBox_Information", "Information"},
                                   
                                   {"SetupCity_AddedCity", "Added City"},
                                   {"SetupCity_AddNewCity", "Add new City..."},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            addedItemLabel.Text = Localization["SetupCity_AddedCity"];

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

            containerPanel.Visible = false;

            var _showCheckBox = _ShowCheckBox;

            GenerateViewModel();

            if (_showCheckBox) ShowCheckBox();

            containerPanel.Visible = true;
        }

        public void Initialize()
        {
            addNewDoubleBufferPanel.Paint += AddNewPanelPaint;

            addNewDoubleBufferPanel.MouseClick += AddNewDoubleBufferPanelMouseClick;
        }

        private void AddNewPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintHighLightInput(g, addNewDoubleBufferPanel);
            Manager.PaintEdit(g, addNewDoubleBufferPanel);

            Manager.PaintText(g, Localization["SetupCity_AddNewCity"]);
        }

        public void GenerateViewModel()
        {
            _ShowCheckBox = false;

            ClearViewModel();
            addNewDoubleBufferPanel.Visible = true;


            List<IRegion> sortResult = null;
            if (PTS != null)
            {
                sortResult = new List<IRegion>(PTS.POS.RegionManager.Values);
            }
            
            if (sortResult == null)
            {
                addedItemLabel.Visible = false;
                return;
            }

            sortResult.Sort((x, y) => (x.Id - y.Id));

            if (sortResult.Count == 0)
            {
                addedItemLabel.Visible = false;
                return;
            }

            _pageSelector.SelectPage = _page;
            _pageSelector.Pages = Convert.ToInt32(Math.Ceiling(sortResult.Count / (CountPerPage * 1.0)));
            _pageSelector.ShowPages();


            List<IRegion> pageResult = new List<IRegion>();
            var start = (_page - 1) * CountPerPage;
            var end = start - 1 + CountPerPage;
            if (end > sortResult.Count) end = sortResult.Count - 1;

            var idx = -1;
            foreach (IRegion region in sortResult)
            {
                idx++;
                if (idx < start) continue;
                if (idx > end) break;

                pageResult.Insert(0, region);
            }

            addedItemLabel.Visible = true;
            containerPanel.Visible = false;
            foreach (IRegion region in pageResult)
            {
                ItemPanel posPanel = GetPOSPanel();

                posPanel.Region = region;

                containerPanel.Controls.Add(posPanel);
            }

            ItemPanel posTitlePanel = GetPOSPanel();
            posTitlePanel.IsTitle = true;
            posTitlePanel.Cursor = Cursors.Default;
            posTitlePanel.EditVisible = false;
            //deviceTitleControl.OnSortChange += DeviceControlOnSortChange;
            posTitlePanel.OnSelectAll += POSPanelOnSelectAll;
            posTitlePanel.OnSelectNone += POSPanelOnSelectNone;
            containerPanel.Controls.Add(posTitlePanel);
            containerPanel.Visible = true;

            containerPanel.AutoScroll = false;
            containerPanel.Focus();
            containerPanel.AutoScroll = true;
        }

        private readonly Queue<ItemPanel> _recyclePOS = new Queue<ItemPanel>();
        private ItemPanel GetPOSPanel()
        {
            if (_recyclePOS.Count > 0)
            {
                return _recyclePOS.Dequeue();
            }

            var posPanel = new ItemPanel
            {
                SelectedColor = Manager.DeleteTextColor,
                EditVisible = true,
                SelectionVisible = false,
            };

             posPanel.OnItemEditClick += POSPanelOnItemEditClick;
             posPanel.OnSelectChange += POSPanelOnSelectChange;

             return posPanel;
        }

        public Brush SelectedColor{
            set
            {
                foreach (ItemPanel control in containerPanel.Controls)
                    control.SelectedColor = value;
            }
        }

        private Boolean _ShowCheckBox = false;

        public void ShowCheckBox()
        {
            _ShowCheckBox = true;

            addNewDoubleBufferPanel.Visible = addedItemLabel.Visible = false;

            foreach (ItemPanel control in containerPanel.Controls)
            {
                control.SelectionVisible = true;
                control.EditVisible = false;
            }

            containerPanel.AutoScroll = false;
            containerPanel.Focus();
            containerPanel.AutoScroll = true;
        }

        public void RemoveSelectedItem()
        {
            foreach (ItemPanel control in containerPanel.Controls)
            {
                if (!control.Checked || control.Region == null) continue;

                if (PTS != null)
                    PTS.POS.RegionManager.Remove(control.Region.Id);

                control.Region.ReadyState = ReadyState.Delete;

                //if (PTS != null)
                //    PTS.POSModify(control.Division);
            }
        }

        private void AddNewDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnDivisionAdd != null)
                OnDivisionAdd(this, e);
        }
        
        private void POSPanelOnItemEditClick(Object sender, EventArgs e)
        {
            if (((ItemPanel)sender).Region != null)
            {
                if (OnDivisionEdit != null)
                    OnDivisionEdit(this, new EventArgs<IRegion>(((ItemPanel)sender).Region));
            }
        }

        private void POSPanelOnSelectChange(Object sender, EventArgs e)
        {
            var panel = sender as ItemPanel;
            if (panel == null) return;

            var selectAll = false;
            if (panel.Checked)
            {
                selectAll = true;
                foreach (ItemPanel control in containerPanel.Controls)
                {
                    if (control.IsTitle) continue;
                    if (!control.Checked && control.Enabled)
                    {
                        selectAll = false;
                        break;
                    }
                }
            }

            var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as ItemPanel;
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
            foreach (ItemPanel posPanel in containerPanel.Controls)
            {
                posPanel.SelectionVisible = false;

                posPanel.Checked = false;
                posPanel.Region = null;
                posPanel.Cursor = Cursors.Hand;
                posPanel.EditVisible = true;

                if (posPanel.IsTitle)
                {
                    posPanel.OnSelectAll -= POSPanelOnSelectAll;
                    posPanel.OnSelectNone -= POSPanelOnSelectNone;
                    posPanel.IsTitle = false;
                }

                if (!_recyclePOS.Contains(posPanel))
                {
                    _recyclePOS.Enqueue(posPanel);
                }
            }
            containerPanel.Controls.Clear();
        }

        private void POSPanelOnSelectAll(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (ItemPanel control in containerPanel.Controls)
            {
                control.Checked = true;
            }
            containerPanel.AutoScroll = true;
        }

        private void POSPanelOnSelectNone(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (ItemPanel control in containerPanel.Controls)
            {
                control.Checked = false;
            }
            containerPanel.AutoScroll = true;
        }
    } 
}