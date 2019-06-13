using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace SetupScheduleReport
{
    public sealed partial class ScheduleReportListPanel : UserControl
    {
        public event EventHandler OnScheduleReportAdd;
        public event EventHandler<EventArgs<ScheduleReport>> OnScheduleReportEdit;

        public IPTS PTS;
        public Dictionary<String, String> Localization;

        public ScheduleReportListPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupSchedule_AddNewScheduleReport", "Add new schedule report..."},
                                   {"SetupSchedule_AddedScheduleReport", "Added schedule report"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Resources.GetResources(Properties.Resources.bg_noborder, Properties.Resources.IMGBgNoborder);
            addedScheduleReportLabel.Text = Localization["SetupSchedule_AddedScheduleReport"];
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

            Manager.PaintText(g, Localization["SetupSchedule_AddNewScheduleReport"]);
        }

        private readonly Queue<ScheduleReportPanel> _recycleScheduleReport = new Queue<ScheduleReportPanel>();

        private Point _previousScrollPosition; 
        public void GenerateViewModel()
        {
            _previousScrollPosition = containerPanel.AutoScrollPosition;
            _previousScrollPosition.Y *= -1;

            ClearViewModel();
            addNewDoubleBufferPanel.Visible = true;
            addedScheduleReportLabel.Visible = false;

            if (PTS.POS.ScheduleReports.Count == 0) return;

            addedScheduleReportLabel.Visible = true;
            containerPanel.Visible = false;

            var reports = new List<ScheduleReport> ();
            reports.AddRange(PTS.POS.ScheduleReports);
            reports.Reverse();
            foreach (var report in reports)
            {
                var scheduleReportPanel = GetScheduleReportPanel();

                scheduleReportPanel.Report = report;

                containerPanel.Controls.Add(scheduleReportPanel);
            }

            var scheduleTitlePanel = GetScheduleReportPanel();
            scheduleTitlePanel.IsTitle = true;
            scheduleTitlePanel.OnSelectAll += ScheduleReportPanelOnSelectAll;
            scheduleTitlePanel.OnSelectNone += ScheduleReportPanelOnSelectNone;
            scheduleTitlePanel.Cursor = Cursors.Default;
            scheduleTitlePanel.EditVisible = false;
            containerPanel.Controls.Add(scheduleTitlePanel);

            containerPanel.Visible = true;
            containerPanel.Focus();
            containerPanel.AutoScrollPosition = _previousScrollPosition;
        }

        public Boolean SelectionVisible
        {
            set
            {
                foreach (ScheduleReportPanel scheduleReportPanel in containerPanel.Controls)
                    scheduleReportPanel.SelectionVisible = value;
            }
        }

        private ScheduleReportPanel GetScheduleReportPanel()
        {
            if (_recycleScheduleReport.Count > 0)
            {
                return _recycleScheduleReport.Dequeue();
            }

            var scheduleReportPanel = new ScheduleReportPanel
            {
                EditVisible = true,
                SelectionVisible = false,
                PTS = PTS,
            };
            scheduleReportPanel.OnSelectChange += ScheduleReportPanelOnSelectChange;
            scheduleReportPanel.OnReportEditClick += ScheduleReportPanelOnReportEditClick;

            return scheduleReportPanel;
        }

        private void ScheduleReportPanelOnReportEditClick(Object sender, EventArgs e)
        {
            if (((ScheduleReportPanel)sender).Report != null)
            {
                if (OnScheduleReportEdit != null)
                    OnScheduleReportEdit(this, new EventArgs<ScheduleReport>(((ScheduleReportPanel)sender).Report));
            }
        }

        private void ScheduleReportPanelOnSelectChange(Object sender, EventArgs e)
        {
            var panel = sender as ScheduleReportPanel;
            if (panel == null) return;

            var selectAll = false;
            if (panel.Checked)
            {
                selectAll = true;
                foreach (ScheduleReportPanel control in containerPanel.Controls)
                {
                    if (control.IsTitle) continue;
                    if (!control.Checked && control.Enabled)
                    {
                        selectAll = false;
                        break;
                    }
                }
            }

            var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as ScheduleReportPanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= ScheduleReportPanelOnSelectAll;
                title.OnSelectNone -= ScheduleReportPanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += ScheduleReportPanelOnSelectAll;
                title.OnSelectNone += ScheduleReportPanelOnSelectNone;
            }
        }

        private void ScheduleReportPanelOnSelectAll(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (ScheduleReportPanel control in containerPanel.Controls)
            {
                control.Checked = true;
            }
            containerPanel.AutoScroll = true;
        }

        private void ScheduleReportPanelOnSelectNone(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (ScheduleReportPanel control in containerPanel.Controls)
            {
                control.Checked = false;
            }
            containerPanel.AutoScroll = true;
        }

        public void RemoveSelectedReports()
        {
            foreach (ScheduleReportPanel scheduleReportPanel in containerPanel.Controls)
            {
                if (!scheduleReportPanel.Checked) continue;

                PTS.POS.ScheduleReports.Remove(scheduleReportPanel.Report);
            }
        }

        public void ShowCheckBox()
        {
            addNewDoubleBufferPanel.Visible = addedScheduleReportLabel.Visible = false;

            foreach (ScheduleReportPanel control in containerPanel.Controls)
            {
                control.SelectionVisible = true;
                control.EditVisible = false;
            }

            containerPanel.AutoScroll = false;
            containerPanel.Focus();
            containerPanel.AutoScroll = true;
        }

        private void AddNewDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnScheduleReportAdd != null)
                OnScheduleReportAdd(null, null);
        }

        private void ClearViewModel()
        {
            foreach (ScheduleReportPanel scheduleReportPanel in containerPanel.Controls)
            {
                scheduleReportPanel.SelectionVisible = false;
                scheduleReportPanel.Checked = false;
                scheduleReportPanel.Cursor = Cursors.Hand;
                scheduleReportPanel.EditVisible = true;
                if (scheduleReportPanel.IsTitle)
                {
                    scheduleReportPanel.IsTitle = false;

                    scheduleReportPanel.OnSelectAll -= ScheduleReportPanelOnSelectAll;
                    scheduleReportPanel.OnSelectNone -= ScheduleReportPanelOnSelectNone;
                }
                scheduleReportPanel.Report = null;

                if (!_recycleScheduleReport.Contains(scheduleReportPanel))
                    _recycleScheduleReport.Enqueue(scheduleReportPanel);
            }

            containerPanel.Controls.Clear();
        }
    } 
}
