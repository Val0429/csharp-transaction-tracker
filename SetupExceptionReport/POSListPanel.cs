using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupPOS;
using Manager = SetupBase.Manager;

namespace SetupExceptionReport
{
    public partial class POSListPanel : UserControl
    {
        public event EventHandler<EventArgs<IPOS>> OnPOSEdit;

        public IPTS PTS;

        private readonly PageSelector _pageSelector;

        public POSListPanel()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
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


        public void Initialize()
        {
            //InitializeComponent();
            //DoubleBuffered = true;
            //Dock = DockStyle.Fill;
            //BackgroundImage = Manager.BackgroundNoBorder;
        }

        private Int32 _page = 1;
        private const UInt16 CountPerPage = 20;

        private void PageSelectorOnSelectionChange(Object sender, EventArgs<Int32> e)
        {
            _page = _pageSelector.SelectPage;

            containerPanel.Visible = false;

            GenerateViewModel();

            containerPanel.Visible = true;
        }

        protected Queue<POSPanel> RecyclePOS = new Queue<POSPanel>();

        private Point _previousScrollPosition; 
        public void GenerateViewModel()
        {
            _previousScrollPosition = containerPanel.AutoScrollPosition;
            _previousScrollPosition.Y *= -1;
            ClearViewModel();

            if (PTS == null) return;

            var sortResult = new List<IPOS>(PTS.POS.POSServer);
            sortResult.Sort((x, y) => (x.Id.CompareTo(y.Id)));

            if (sortResult.Count == 0) return;

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

            containerPanel.Visible = false;
            foreach (var pos in pageResult)
            {
                var posPanel = GetPOSPanel();

                posPanel.POS = pos;

                containerPanel.Controls.Add(posPanel);
            }

            var posTitlePanel = GetPOSPanel();
            posTitlePanel.IsTitle = true;
            posTitlePanel.Cursor = Cursors.Default;
            posTitlePanel.EditVisible = false;
            containerPanel.Controls.Add(posTitlePanel);
            containerPanel.Visible = true;
            containerPanel.Focus();
            containerPanel.AutoScrollPosition = _previousScrollPosition;
        }

        private POSPanel GetPOSPanel()
        {
            if (RecyclePOS.Count > 0)
            {
                return RecyclePOS.Dequeue();
            }
            var posPanel = new POSPanel
            {
                SelectedColor = Manager.DeleteTextColor,
                EditVisible = true,
                SelectionVisible = false,
                DataType = "ExceptionReport"
            };
            posPanel.OnPOSEditClick += POSPanelOnPOSEditClick;

            return posPanel;
        }

        private void POSPanelOnPOSEditClick(Object sender, EventArgs e)
        {
            if (((POSPanel)sender).POS == null) return;

            if (OnPOSEdit != null)
                OnPOSEdit(this, new EventArgs<IPOS>(((POSPanel)sender).POS));
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
                    //deviceControl.OnSortChange -= DeviceControlOnSortChange;
                    posPanel.IsTitle = false;
                }

                if (!RecyclePOS.Contains(posPanel))
                    RecyclePOS.Enqueue(posPanel);
            }
            containerPanel.Controls.Clear();
        }
    } 
}