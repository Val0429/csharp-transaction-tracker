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
    public sealed partial class CopyExceptionReportPanel : UserControl
    {
        public IPTS PTS;
        public Dictionary<String, String> Localization;

        private readonly PageSelector _pageSelector;

        public CopyExceptionReportPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupExceptionReport_CopyFrom", "Copy From"},
                               };
            Localizations.Update(Localization);

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

        private Int32 _page = 1;
        private const UInt16 CountPerPage = 20;

        private void PageSelectorOnSelectionChange(Object sender, EventArgs<Int32> e)
        {
            _page = _pageSelector.SelectPage;

            containerPanel.Visible = false;

            GenerateViewModel();

            containerPanel.Visible = true;
        }

        public void Initialize()
        {
            copyFromPanel.Paint += CopyFromPanelPaint;

            copyFromComboBox.SelectedIndexChanged += CopyFromComboBoxSelectedIndexChanged;
        }

        private Boolean _isEditing;
        private IPOS _copyFromPos;
        private void CopyFromComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if(!containerPanel.Enabled) return;

            if (!(copyFromComboBox.SelectedItem is IPOS)) return;
            _copyFromPos = copyFromComboBox.SelectedItem as IPOS;

            foreach (POSPanel control in containerPanel.Controls)
            {
                control.Checked = false;
            }

            foreach (POSPanel control in containerPanel.Controls)
            {
                if(control.IsTitle) return;

                control.Enabled = (control.POS != _copyFromPos);
            }
        }

        private void CopyFromPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintSingleInput(g, copyFromPanel);

            if (copyFromPanel.Width <= 100) return;

            Manager.PaintText(g, Localization["SetupExceptionReport_CopyFrom"]);
        }

        private readonly Queue<POSPanel> _recyclePOS = new Queue<POSPanel>();

        public void GenerateViewModel()
        {
            _isEditing = false;
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

            containerPanel.Enabled = true;
            containerPanel.Visible = false;
            foreach (IPOS pos in pageResult)
            {
                var posPanel = GetPOSPanel();

                posPanel.POS = pos;
                posPanel.SelectionVisible = true;
                containerPanel.Controls.Add(posPanel);
            }

            var posTitlePanel = GetPOSPanel();
            posTitlePanel.IsTitle = true;
            posTitlePanel.OnSelectAll += POSPanelOnSelectAll;
            posTitlePanel.OnSelectNone += POSPanelOnSelectNone;
            posTitlePanel.Cursor = Cursors.Default;
            posTitlePanel.EditVisible = false;
            containerPanel.Controls.Add(posTitlePanel);
            containerPanel.Visible = true;

            copyFromComboBox.Items.Clear();

            sortResult.Sort((x, y) => (x.Id.CompareTo(y.Id)));
            foreach (IPOS device in sortResult)
            {
                copyFromComboBox.Items.Add(device);
            }

            Manager.DropDownWidth(copyFromComboBox);
            copyFromComboBox.SelectedIndex = 0;

            _isEditing = true;
        }

        private POSPanel GetPOSPanel()
        {
            if (_recyclePOS.Count > 0)
            {
                return _recyclePOS.Dequeue();
            }

            var posPanel = new POSPanel
            {
                EditVisible = false,
                SelectionVisible = true,
                DataType = "ExceptionReport",
                Cursor = Cursors.Hand,
            };
            posPanel.OnSelectChange += POSPanelOnSelectChange;
            return posPanel;
        }

        private void POSPanelOnSelectAll(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (POSPanel control in containerPanel.Controls)
            {
                if (!control.Enabled) continue;

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

        private void POSPanelOnSelectChange(Object sender, EventArgs e)
        {
            if(!_isEditing) return;
            if (_copyFromPos == null) return;
            var panel = sender as POSPanel;
            if (panel == null) return;

            if (panel.POS == null) return;

            var selectAll = false;
            if (panel.Checked)
            {
                CopyExceptionReport(panel.POS);

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

        private void CopyExceptionReport(IPOS pos)
        {
            if (pos == null) return;

            pos.ExceptionReports.Clear();
            foreach (var exceptionReport in _copyFromPos.ExceptionReports)
            {
                var report = new ExceptionReport
                                 {
                                     Exception = exceptionReport.Exception,
                                     Threshold = exceptionReport.Threshold,
                                     Increment = exceptionReport.Increment,
                                     ReportForm = { Format = exceptionReport.ReportForm.Format },
                                 };
                report.ReportForm.POS.Add(pos.Id);
                report.ReportForm.Exceptions.AddRange(exceptionReport.ReportForm.Exceptions);
                report.ReportForm.MailReceiver = exceptionReport.ReportForm.MailReceiver;
                report.ReportForm.Subject = exceptionReport.ReportForm.Subject;
                //report.ReportForm.Body = exceptionReport.ReportForm.Body;

                pos.ExceptionReports.Add(report);
            }

            if (pos.ExceptionReports.ReadyState == ReadyState.Ready)
                pos.ExceptionReports.ReadyState = ReadyState.Modify;

            if (pos.ReadyState == ReadyState.Ready)
                pos.ReadyState = ReadyState.Modify;

            PTS.POSModify(pos);
        }

        private void ClearViewModel()
        {
            foreach (POSPanel posPanel in containerPanel.Controls)
            {
                posPanel.SelectionVisible = true;

                posPanel.OnSelectChange -= POSPanelOnSelectChange;
                posPanel.Enabled = true;
                posPanel.Checked = false;
                posPanel.POS = null;
                posPanel.Cursor = Cursors.Hand;
                posPanel.EditVisible = false;
                posPanel.OnSelectChange += POSPanelOnSelectChange;

                if (posPanel.IsTitle)
                {
                    posPanel.OnSelectAll -= POSPanelOnSelectAll;
                    posPanel.OnSelectNone -= POSPanelOnSelectNone;
                    //deviceControl.OnSortChange -= DeviceControlOnSortChange;
                    posPanel.IsTitle = false;
                }

                if (!_recyclePOS.Contains(posPanel))
                    _recyclePOS.Enqueue(posPanel);
            }
            containerPanel.Controls.Clear();
        }
    } 
}