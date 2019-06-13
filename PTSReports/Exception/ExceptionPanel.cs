using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports.Exception
{
    public sealed partial class ExceptionPanel : UserControl
    {
        public IPTS PTS;
        public Exception ExceptionReport;

        public ExceptionPanel()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "Exception";
            BackgroundImage = Resources.GetResources(Properties.Resources.bg_noborder, Properties.Resources.IMGBgNoborder);
        }

        private Boolean _isEditing;
        public void ParseSetting()
        {
            _isEditing = false;

            ClearViewModel();

            var selectAll = true;
            //containerPanel.Visible = false;

            var list = new  List<String>();
            var exceptions = PTS.POS.Exceptions;
         
            foreach (var posException in exceptions)
            {
                foreach (var exception in posException.Value.Exceptions)
                {
                    if (!list.Contains(exception.Value))
                        list.Add(exception.Key);
                }
            }
            list.Sort((x, y) => (y.CompareTo(x)));
            
            //String[] list = POS_Exception.ExceptionList.Clone() as String[];);

            foreach (String exception in list)
            {
                StringPanel stringPanel = GetStringPanel();

                stringPanel.Exception = exception;

                if (ExceptionReport.Exceptions.Contains(exception))
                    stringPanel.Checked = true;
                else
                    selectAll = false;

                containerPanel.Controls.Add(stringPanel);
            }

            StringPanel stringTitlePanel = GetStringPanel();
            stringTitlePanel.IsTitle = true;
            stringTitlePanel.Cursor = Cursors.Default;
            stringTitlePanel.Checked = selectAll;
            stringTitlePanel.OnSelectAll += StringPanelOnSelectAll;
            stringTitlePanel.OnSelectNone += StringPanelOnSelectNone;
            containerPanel.Controls.Add(stringTitlePanel);

            _isEditing = true;
        }

        public void ScrollTop()
        {
            containerPanel.Select();
            containerPanel.AutoScrollPosition = new Point(0, 0);
        }

        private readonly Queue<StringPanel> _recycleString = new Queue<StringPanel>();
        private StringPanel GetStringPanel()
        {
            if (_recycleString.Count > 0)
            {
                return _recycleString.Dequeue();
            }

            var stringPanel = new StringPanel
            {
                SelectionVisible = true,
            };

            stringPanel.OnSelectChange += StringControlOnSelectChange;

            return stringPanel;
        }

        private void StringControlOnSelectChange(Object sender, EventArgs e)
        {
            if(!_isEditing) return;

            var panel = sender as StringPanel;
            if (panel == null) return;
            if (panel.IsTitle) return;

            var selectAll = false;
            if (panel.Checked)
            {
                if (!ExceptionReport.Exceptions.Contains(panel.Exception))
                {
                    ExceptionReport.Exceptions.Add(panel.Exception);
                }

                selectAll = true;
                foreach (StringPanel control in containerPanel.Controls)
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
                ExceptionReport.Exceptions.Remove(panel.Exception);
            }

            var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as StringPanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= StringPanelOnSelectAll;
                title.OnSelectNone -= StringPanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += StringPanelOnSelectAll;
                title.OnSelectNone += StringPanelOnSelectNone;
            }
        }

        private void ClearViewModel()
        {
            foreach (StringPanel stringPanel in containerPanel.Controls)
            {
                stringPanel.SelectionVisible = false;

                stringPanel.Checked = false;
                stringPanel.Exception = "";
                stringPanel.Cursor = Cursors.Hand;
                stringPanel.SelectionVisible = true;

                if (stringPanel.IsTitle)
                {
                    stringPanel.OnSelectAll -= StringPanelOnSelectAll;
                    stringPanel.OnSelectNone -= StringPanelOnSelectNone;
                    stringPanel.IsTitle = false;
                }

                if (!_recycleString.Contains(stringPanel))
                {
                    _recycleString.Enqueue(stringPanel);
                }
            }
            containerPanel.Controls.Clear();
        }

        private void StringPanelOnSelectAll(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (StringPanel control in containerPanel.Controls)
            {
                control.Checked = true;
            }
            containerPanel.AutoScroll = true;
        }

        private void StringPanelOnSelectNone(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (StringPanel control in containerPanel.Controls)
            {
                control.Checked = false;
            }
            containerPanel.AutoScroll = true;
        }
    }

    public class StringPanel: Panel
    {
        public event EventHandler OnSelectAll;
        public event EventHandler OnSelectNone;
        public event EventHandler OnSelectChange;

        public Dictionary<String, String> Localization;

        private readonly CheckBox _checkBox = new CheckBox();

        public Boolean IsTitle;
        public String Exception;

        public StringPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"POS_Exception", "Exception"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Hand;
            Height = 40;

            BackColor = Color.Transparent;

            _checkBox.Padding = new Padding(10, 0, 0, 0);
            _checkBox.Dock = DockStyle.Left;
            _checkBox.Width = 25;

            Controls.Add(_checkBox);

            _checkBox.CheckedChanged += CheckBoxCheckedChanged;

            MouseClick += StringPanelMouseClick;
            Paint += StringPanelPaint;
        }
        
        private void PaintTitle(Graphics g)
        {
            if (Width <= 300) return;
            Manager.PaintText(g, Localization["POS_Exception"]);
        }

        private void StringPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, (Control)sender);
           
            if (IsTitle)
            {
                PaintTitle(g);
                return;
            }

            Brush fontBrush = Brushes.Black;

            if (_checkBox.Visible && Checked)
            {
                fontBrush = SelectedColor;
            }

            if (Width <= 300) return;

            Manager.PaintText(g, Exception, fontBrush);
        }

        private void StringPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (IsTitle)
            {
                if (_checkBox.Visible)
                {
                    _checkBox.Checked = !_checkBox.Checked;
                    return;
                }
            }
            else
            {
                if (_checkBox.Visible)
                {
                    _checkBox.Checked = !_checkBox.Checked;
                    return;
                }
            }
        }

        private void CheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            Invalidate();

            if (IsTitle)
            {
                if (Checked && OnSelectAll != null)
                    OnSelectAll(this, null);
                else if (!Checked && OnSelectNone != null)
                    OnSelectNone(this, null);

                return;
            }

            _checkBox.Focus();
            if (OnSelectChange != null)
                OnSelectChange(this, null);
        }

        public Brush SelectedColor = Brushes.RoyalBlue;

        public Boolean Checked
        {
            get
            {
                return _checkBox.Checked;
            }
            set
            {
                _checkBox.Checked = value;
            }
        }

        public Boolean SelectionVisible
        {
            set { _checkBox.Visible = value; }
        }
    }
}
