using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace SetupEvent
{
    public sealed partial class EventReportListPanel : UserControl
    {
        public event EventHandler OnExceptionReportAdd;
        public event EventHandler<EventArgs<ExceptionReport>> OnExceptionReportEdit;

        public IPTS PTS;
        public IPOS POS;
        public Dictionary<String, String> Localization;

        public EventReportListPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupEvent_AddNewEventReport", "Add new event report..."},
                                   {"SetupEvent_AddedEventReport", "Added event report"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Resources.GetResources(Properties.Resources.bg_noborder, Properties.Resources.IMGBgNoborder);
            addedEventReportLabel.Text = Localization["SetupEvent_AddedEventReport"];
        }

        public void Initialize()
        {
            addNewDoubleBufferPanel.Paint += InputPanelPaint;
            addNewDoubleBufferPanel.MouseClick += AddNewDoubleBufferPanelMouseClick;
        }

        private void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintHighLightInput(g, addNewDoubleBufferPanel);
            Manager.PaintEdit(g, addNewDoubleBufferPanel);

            Manager.PaintText(g, Localization["SetupEvent_AddNewEventReport"]);
        }

        private readonly Queue<ExceptionReportPanel> _recycleExceptionReport = new Queue<ExceptionReportPanel>();

        private Point _previousScrollPosition; 
        public void GenerateViewModel()
        {
            _previousScrollPosition = containerPanel.AutoScrollPosition;
            _previousScrollPosition.Y *= -1;

            ClearViewModel();
            addedEventReportLabel.Visible = false;

            if (POS.ExceptionReports.Count == 0) return;

            addedEventReportLabel.Visible = true;
            containerPanel.Visible = false;

            var reports = new List<ExceptionReport> ();
            reports.AddRange(POS.ExceptionReports);
            reports.Reverse();
            foreach (var report in reports)
            {
                var exceptionReportPanel = GetExceptionReportPanel();

                exceptionReportPanel.Report = report;

                containerPanel.Controls.Add(exceptionReportPanel);
            }

            var exceptionTitlePanel = GetExceptionReportPanel();
            exceptionTitlePanel.IsTitle = true;
            exceptionTitlePanel.OnSelectAll += ExceptionReportPanelOnSelectAll;
            exceptionTitlePanel.OnSelectNone += ExceptionReportPanelOnSelectNone;
            exceptionTitlePanel.Cursor = Cursors.Default;
            exceptionTitlePanel.EditVisible = false;
            containerPanel.Controls.Add(exceptionTitlePanel);

            containerPanel.Visible = true;
            containerPanel.Focus();
            containerPanel.AutoScrollPosition = _previousScrollPosition;
        }

        public Boolean SelectionVisible
        {
            set
            {
                foreach (ExceptionReportPanel exceptionReportPanel in containerPanel.Controls)
                    exceptionReportPanel.SelectionVisible = value;
            }
        }

        private ExceptionReportPanel GetExceptionReportPanel()
        {
            if (_recycleExceptionReport.Count > 0)
            {
                return _recycleExceptionReport.Dequeue();
            }

            var exceptionReportPanel = new ExceptionReportPanel
            {
                EditVisible = true,
                SelectionVisible = false,
                PTS = PTS,
            };
            exceptionReportPanel.OnSelectChange += ExceptionReportPanelOnSelectChange;
            exceptionReportPanel.OnReportEditClick += ExceptionReportPanelOnReportEditClick;

            return exceptionReportPanel;
        }

        private void ExceptionReportPanelOnReportEditClick(Object sender, EventArgs e)
        {
            if (((ExceptionReportPanel)sender).Report != null)
            {
                if (OnExceptionReportEdit != null)
                    OnExceptionReportEdit(this, new EventArgs<ExceptionReport>(((ExceptionReportPanel)sender).Report));
            }
        }
        private void ExceptionReportPanelOnSelectChange(Object sender, EventArgs e)
        {
            var panel = sender as ExceptionReportPanel;
            if (panel == null) return;

            var selectAll = false;
            if (panel.Checked)
            {
                selectAll = true;
                foreach (ExceptionReportPanel control in containerPanel.Controls)
                {
                    if (control.IsTitle) continue;
                    if (!control.Checked && control.Enabled)
                    {
                        selectAll = false;
                        break;
                    }
                }
            }

            var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as ExceptionReportPanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= ExceptionReportPanelOnSelectAll;
                title.OnSelectNone -= ExceptionReportPanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += ExceptionReportPanelOnSelectAll;
                title.OnSelectNone += ExceptionReportPanelOnSelectNone;
            }
        }

        private void ExceptionReportPanelOnSelectAll(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (ExceptionReportPanel control in containerPanel.Controls)
            {
                control.Checked = true;
            }
            containerPanel.AutoScroll = true;
        }

        private void ExceptionReportPanelOnSelectNone(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (ExceptionReportPanel control in containerPanel.Controls)
            {
                control.Checked = false;
            }
            containerPanel.AutoScroll = true;
        }

        public void RemoveSelectedGroups()
        {
            foreach (ExceptionReportPanel exceptionReportPanel in containerPanel.Controls)
            {
                if (!exceptionReportPanel.Checked) continue;

                POS.ExceptionReports.Remove(exceptionReportPanel.Report);
            }
        }

        private void AddNewDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnExceptionReportAdd != null)
                OnExceptionReportAdd(this, e);
        }

        private void ClearViewModel()
        {
            foreach (ExceptionReportPanel exceptionReportPanel in containerPanel.Controls)
            {
                exceptionReportPanel.SelectionVisible = false;
                exceptionReportPanel.Checked = false;
                exceptionReportPanel.Cursor = Cursors.Hand;
                exceptionReportPanel.EditVisible = true;
                if (exceptionReportPanel.IsTitle)
                {
                    exceptionReportPanel.IsTitle = false;

                    exceptionReportPanel.OnSelectAll -= ExceptionReportPanelOnSelectAll;
                    exceptionReportPanel.OnSelectNone -= ExceptionReportPanelOnSelectNone;
                }
                exceptionReportPanel.Report = null;

                if (!_recycleExceptionReport.Contains(exceptionReportPanel))
                    _recycleExceptionReport.Enqueue(exceptionReportPanel);
            }

            containerPanel.Controls.Clear();
        }
    } 
}
