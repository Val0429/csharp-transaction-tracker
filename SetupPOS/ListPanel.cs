using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using Manager = SetupBase.Manager;

namespace SetupPOS
{
    public sealed partial class ListPanel : UserControl
    {
        public event EventHandler<EventArgs<IPOS>> OnPOSEdit;
        public event EventHandler OnPOSAdd;
        
        public IPTS PTS;

        public Dictionary<String, String> Localization;

        private readonly PageSelector _pageSelector;

        public ListPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"MessageBox_Information", "Information"},
                                   
                                   {"SetupPOS_AddedPOS", "Added POS"},
                                   {"SetupPOS_AddNewPOS", "Add new POS..."},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            addedPOSLabel.Text = Localization["SetupPOS_AddedPOS"];

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

            Manager.PaintText(g, Localization["SetupPOS_AddNewPOS"]);
        }

        public void GenerateViewModel()
        {
            _ShowCheckBox = false;

            ClearViewModel();
            addNewDoubleBufferPanel.Visible = true;

            List<IPOS> sortResult = null;
            if (PTS != null)
            {
                sortResult = new List<IPOS>(PTS.POS.POSServer);
            }
            
            if (sortResult == null)
            {
                addedPOSLabel.Visible = false;
                return;
            }

            sortResult.Sort((x, y) => (x.Id.CompareTo(y.Id)));

            if (sortResult.Count == 0)
            {
                addedPOSLabel.Visible = false;
                return;
            }

            _pageSelector.SelectPage = _page;
            _pageSelector.Pages = Convert.ToInt32(Math.Ceiling(sortResult.Count / (CountPerPage * 1.0)));
            _pageSelector.ShowPages();


            List<IPOS> pageResult = new List<IPOS>();
            var start = (_page - 1) * CountPerPage;
            var end = start - 1 + CountPerPage;
            if (end > sortResult.Count) end = sortResult.Count - 1;

            var idx = -1;
            foreach (IPOS pos in sortResult)
            {
                idx++;
                if (idx < start) continue;
                if (idx > end) break;

                pageResult.Insert(0, pos);
            }

            addedPOSLabel.Visible = true;
            containerPanel.Visible = false;
            foreach (IPOS pos in pageResult)
            {
                if(pos.IsCapture) continue;
                POSPanel posPanel = GetPOSPanel();

                posPanel.POS = pos;

                containerPanel.Controls.Add(posPanel);
            }

            POSPanel posTitlePanel = GetPOSPanel();
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

        private readonly Queue<POSPanel> _recyclePOS = new Queue<POSPanel>();
        private POSPanel GetPOSPanel()
        {
            if (_recyclePOS.Count > 0)
            {
                return _recyclePOS.Dequeue();
            }

            var posPanel = new POSPanel
            {
                SelectedColor = Manager.DeleteTextColor,
                EditVisible = true,
                SelectionVisible = false,
            };

             posPanel.OnPOSEditClick += POSPanelOnPOSEditClick;
             posPanel.OnSelectChange += POSPanelOnSelectChange;

             return posPanel;
        }

        public Brush SelectedColor{
            set
            {
                foreach (POSPanel control in containerPanel.Controls)
                    control.SelectedColor = value;
            }
        }

        private Boolean _ShowCheckBox = false;
        public void ShowCheckBox()
        {
            _ShowCheckBox = true;

            addNewDoubleBufferPanel.Visible = addedPOSLabel.Visible = false;

            foreach (POSPanel control in containerPanel.Controls)
            {
                control.SelectionVisible = true;
                control.EditVisible = false;
            }

            containerPanel.AutoScroll = false;
            containerPanel.Focus();
            containerPanel.AutoScroll = true;
        }

        public void RemoveSelectedPOS()
        {
            foreach (POSPanel control in containerPanel.Controls)
            {
                if (!control.Checked || control.POS == null) continue;

                if (PTS != null)
                    PTS.POS.POSServer.Remove(control.POS);

                control.POS.ReadyState = ReadyState.Delete;

                if (PTS != null)
                    PTS.POSModify(control.POS);
            }
        }

        private void AddNewDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnPOSAdd != null)
                OnPOSAdd(this, e);
        }
        
        private void POSPanelOnPOSEditClick(Object sender, EventArgs e)
        {
            if (((POSPanel)sender).POS != null)
            {
                if (OnPOSEdit != null)
                    OnPOSEdit(this, new EventArgs<IPOS>(((POSPanel)sender).POS));
            }
        }

        private void POSPanelOnSelectChange(Object sender, EventArgs e)
        {
            var panel = sender as POSPanel;
            if (panel == null) return;

            var selectAll = false;
            if (panel.Checked)
            {
                selectAll = true;
                foreach (POSPanel control in containerPanel.Controls)
                {
                    if (control.IsTitle) continue;
                    if (!control.Checked && control.Enabled)
                    {
                        selectAll = false;
                        break;
                    }
                }
            }

            var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as POSPanel;
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
            foreach (POSPanel posPanel in containerPanel.Controls)
            {
                posPanel.SelectionVisible = false;

                posPanel.Checked = false;
                posPanel.POS = null;
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
            foreach (POSPanel control in containerPanel.Controls)
            {
                control.Checked = true;
            }
            containerPanel.AutoScroll = true;
        }

        private void POSPanelOnSelectNone(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (POSPanel control in containerPanel.Controls)
            {
                control.Checked = false;
            }
            containerPanel.AutoScroll = true;
        }
    } 
}