using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports.Template
{
    public sealed partial class ListPanel : UserControl
    {
        public event EventHandler OnTemplateAdd;
        public event EventHandler<EventArgs<POS_Exception.TemplateConfig>> OnTemplateEdit;
        
        public IPTS PTS;
        public Dictionary<String, String> Localization;

        public ListPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"PTSReports_AddNewTemplate", "Add new template..."},
                                   {"PTSReports_AddedTemplate", "Added template"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Manager.BackgroundNoBorder;
            addedTemplateLabel.Text = Localization["PTSReports_AddedTemplate"];
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

            Manager.PaintText(g, Localization["PTSReports_AddNewTemplate"]);
        }

        private void AddNewDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnTemplateAdd != null)
                OnTemplateAdd(null, null);
        }

        private readonly Queue<TemplatePanel> _recycleTemplate = new Queue<TemplatePanel>();

        private Point _previousScrollPosition; 
        public void GenerateViewModel()
        {
            _previousScrollPosition = containerPanel.AutoScrollPosition;
            _previousScrollPosition.Y *= -1;
            ClearViewModel();
            addNewDoubleBufferPanel.Visible = true;
            addedTemplateLabel.Visible = false;

            if (PTS == null) return;

            //containerPanel.AutoScroll = false;
            var sortResult = new List<POS_Exception.TemplateConfig>(PTS.POS.TemplateConfigs);
            sortResult.Reverse();

            if (sortResult.Count == 0)
            {
                addedTemplateLabel.Visible = false;
                return;
            }

            addedTemplateLabel.Visible = true;
            containerPanel.Visible = false;
            foreach (POS_Exception.TemplateConfig config in sortResult)
            {
                var configPanel = GetTemplatePanel();

                configPanel.Config = config;

                containerPanel.Controls.Add(configPanel);
            }

            //containerPanel.AutoScroll = true;
            containerPanel.Visible = true;
            containerPanel.Height++;
            containerPanel.Focus();
            containerPanel.Invalidate();
            containerPanel.AutoScrollPosition = _previousScrollPosition;
        }

        private TemplatePanel GetTemplatePanel()
        {
            if (_recycleTemplate.Count > 0)
            {
                return _recycleTemplate.Dequeue();
            }

            var templatePanel = new TemplatePanel
            {
                EditVisible = true,
                SelectionVisible = false,
                PTS = PTS,
            };
            templatePanel.OnTemplateEditClick += TemplatePanelOnTemplateEditClick;

            return templatePanel;
        }

        private void TemplatePanelOnTemplateEditClick(Object sender, EventArgs e)
        {
            if (((TemplatePanel)sender).Config != null)
            {
                if (OnTemplateEdit != null)
                    OnTemplateEdit(this, new EventArgs<POS_Exception.TemplateConfig>(((TemplatePanel)sender).Config));
            }
        }

        private void ClearViewModel()
        {
            foreach (TemplatePanel templatePanel in containerPanel.Controls)
            {
                templatePanel.SelectionVisible = false;

                templatePanel.Checked = false;
                templatePanel.Config = null;
                templatePanel.Cursor = Cursors.Hand;
                templatePanel.EditVisible = true;

                if (!_recycleTemplate.Contains(templatePanel))
                    _recycleTemplate.Enqueue(templatePanel);
            }
            containerPanel.Controls.Clear();
        }

        public void RemoveSelectedTemplate()
        {
            foreach (TemplatePanel templatePanel in containerPanel.Controls)
            {
                if (!templatePanel.Checked) continue;

                PTS.POS.TemplateConfigs.Remove(templatePanel.Config);
            }
        }

        public void ShowCheckBox()
        {
            addNewDoubleBufferPanel.Visible = addedTemplateLabel.Visible = false;

            foreach (TemplatePanel control in containerPanel.Controls)
            {
                control.SelectionVisible = true;
                control.EditVisible = false;
            }

            containerPanel.AutoScroll = false;
            containerPanel.Focus();
            containerPanel.AutoScroll = true;
        }
    } 
}