using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports.Base
{
    public sealed partial class POSPanel : UserControl
    {
        public event EventHandler OnSelectChange;

        public IPTS PTS;
        public POS_Exception.SearchCriteria SearchCriteria;

        public POSPanel()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "POS";
            BackgroundImage = Manager.BackgroundNoBorder;
        }

        private Boolean _isEditing;
        public void ParseSetting()
        {
            _isEditing = false;

            ClearViewModel();

            var sortResult = new List<IPOS>(PTS.POS.POSServer);

            if (sortResult.Count == 0) return;

            sortResult.Sort((x, y) => (x.Id.CompareTo(y.Id)));
            
            var selectAll = true;
            var count = 0;
            //containerPanel.Visible = false;
            foreach (IPOS pos in sortResult)
            {
                SetupPOS.POSPanel posPanel = GetPOSPanel();

                posPanel.POS = pos;

                if (SearchCriteria.POS.Contains(pos.Id))
                {
                    count++;
                    posPanel.Checked = true;
                }
                else
                    selectAll = false;

                containerPanel.Controls.Add(posPanel);
            }
            if (count == 0 && selectAll)
                selectAll = false;

            SetupPOS.POSPanel posTitlePanel = GetPOSPanel();
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

        private readonly Queue<SetupPOS.POSPanel> _recyclePOS = new Queue<SetupPOS.POSPanel>();
        private SetupPOS.POSPanel GetPOSPanel()
        {
            if (_recyclePOS.Count > 0)
            {
                return _recyclePOS.Dequeue();
            }

            var posPanel = new SetupPOS.POSPanel
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

            var panel = sender as SetupPOS.POSPanel;
            if (panel == null) return;
            if (panel.IsTitle) return;

            var selectAll = false;
            if (panel.Checked)
            {
                if (!SearchCriteria.POS.Contains(panel.POS.Id))
                {
                    SearchCriteria.POS.Add(panel.POS.Id);
                    SearchCriteria.POS.Sort((x, y) => (x.CompareTo(y)));
                }

                selectAll = true;
                foreach (SetupPOS.POSPanel control in containerPanel.Controls)
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
                SearchCriteria.POS.Remove(panel.POS.Id);
            }

            if (OnSelectChange != null)
                OnSelectChange(null, null);

            var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as SetupPOS.POSPanel;
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
            foreach (SetupPOS.POSPanel posPanel in containerPanel.Controls)
            {
                posPanel.SelectionVisible = false;

                posPanel.Checked = false;
                posPanel.POS = null;
                posPanel.Cursor = Cursors.Hand;
                posPanel.EditVisible = false;
                posPanel.SelectionVisible = true;

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
            foreach (SetupPOS.POSPanel control in containerPanel.Controls)
            {
                control.Checked = true;
            }
            containerPanel.AutoScroll = true;
        }

        private void POSPanelOnSelectNone(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (SetupPOS.POSPanel control in containerPanel.Controls)
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
