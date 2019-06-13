using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Interface;
using SetupPOS;

namespace SetupPOSConnection
{
    public sealed class POSConnectionPanel : Panel
    {
        public event EventHandler OnPOSSelectionChange;
        public event EventHandler OnPOSConnectionEditClick;

        public IPTS PTS;
        private readonly ConnectionPanel _connectionPanel;
        private readonly Queue<POSPanel> _recyclePOS = new Queue<POSPanel>();
        private readonly POSListPanel _posListPanel = new POSListPanel();

        public IPOSConnection POSConnection
        {
            get
            {
                return _connectionPanel.POSConnection;
            }
            set
            {
                _connectionPanel.POSConnection = value;

                _connectionPanel.Cursor = Cursors.Hand;
            }
        }

        public POSConnectionPanel()
        {
            Dock = DockStyle.Top;
            Padding = new Padding(0, 0, 0, 15);
            DoubleBuffered = true;
            AutoSize = true;
            BackColor = Color.Transparent;
            //MinimumSize = new Size(0, 15);

            _connectionPanel = new ConnectionPanel();
            _connectionPanel.OnConnectionEditClick += EditButtonMouseClick;
        }

        public void ShowPOS()
        {
            _isEditing = false;
            ClearPOSListControls();

            if (POSConnection.POS.Count > 0)
            {
                _posListPanel.Padding = new Padding(0, 0, 0, 0);
                _posListPanel.Show();

                var list = new List<IPOS>(POSConnection.POS);

                //reverse
                list.Sort((x, y) => (y.Id.CompareTo(x.Id)));

                foreach (IPOS pos in list)
                {
                    if (pos == null) continue;

                    POSPanel posPanel = GetPOSPanel();
                    posPanel.POS = pos;

                    _posListPanel.Controls.Add(posPanel);
                }

                var posTitlePanel = GetPOSPanel();
                posTitlePanel.IsTitle = posTitlePanel.IsMiddleTitle = true;
                posTitlePanel.Cursor = Cursors.Default;
                posTitlePanel.EditVisible = false;
                _posListPanel.Controls.Add(posTitlePanel);

                var invasable = GetPOSPanel();
                invasable.IsTitle = posTitlePanel.IsMiddleTitle = true;
                invasable.Height = 0;
                _posListPanel.Controls.Add(invasable);

                Controls.Add(_posListPanel);
            }

            Controls.Add(_connectionPanel);
        }

        public void ShowPOSWithSelection()
        {
            _isEditing = false;
            ClearPOSListControls();

            AppendPOSListPanel(_posListPanel);

            _isEditing = true;
            _connectionPanel.Cursor = Cursors.Default;

            Controls.Remove(_connectionPanel);
        }

        private void AppendPOSListPanel(Control listPanel)
        {
            var selectAll = true;
            var poss = new List<IPOS>(PTS.POS.POSServer);
            //reverse
            var count = 0;
            poss.Sort((x, y) => (y.Id.CompareTo(x.Id)));

            if (poss.Count > 0)
            {
                foreach (IPOS pos in poss)
                {
                    var posPanel = GetPOSPanel();
                    posPanel.SelectionVisible = true;
                    posPanel.POS = pos;

                    if (pos.Manufacture == POSConnection.Manufacture)
                    {
                        posPanel.Enabled = true;
                        if (POSConnection.POS.Contains(pos))
                        {
                            count++;
                            posPanel.Checked = true;
                        }
                        else
                            selectAll = false;
                    }
                    else
                    {
                        posPanel.Enabled = false;
                    }

                    listPanel.Controls.Add(posPanel);
                }
                if (count == 0 && selectAll)
                    selectAll = false;

                POSPanel posTitlePanel = GetPOSPanel();
                posTitlePanel.IsTitle = posTitlePanel.IsMiddleTitle = true;
                posTitlePanel.Cursor = Cursors.Default;
                posTitlePanel.EditVisible = false;
                posTitlePanel.SelectionVisible = true;
                posTitlePanel.Checked = selectAll;
                posTitlePanel.OnSelectAll += POSPanelOnSelectAll;
                posTitlePanel.OnSelectNone += POSPanelOnSelectNone;
                listPanel.Controls.Add(posTitlePanel);
            }

            Controls.Add(listPanel);
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
                SelectionVisible = false,
                Cursor = Cursors.Default
            };
            posPanel.OnSelectChange += POSPanelOnSelectChange;

            return posPanel;
        }

        public Boolean Checked
        {
            get
            {
                return _connectionPanel.Checked;
            }
            set
            {
                _connectionPanel.Checked = value;
            }
        }

        public List<IPOS> POSSelection
        {
            get
            {
                var poss = new List<IPOS>();
                foreach (POSPanel control in _posListPanel.Controls)
                {
                    if (!control.Checked || control.POS == null) continue;

                    poss.Add(control.POS);
                }

                poss.Sort((x, y) => (x.Id.CompareTo(y.Id)));

                return poss;
            }
        }

        private Boolean _isEditing;
        private void POSPanelOnSelectChange(Object sender, EventArgs e)
        {
            if (!_isEditing) return;

            if (OnPOSSelectionChange != null)
            {
                OnPOSSelectionChange(sender, null);

                var panel = sender as POSPanel;
                if (panel == null) return;

                var selectAll = false;
                if (panel.Checked)
                {
                    selectAll = true;
                    foreach (POSPanel control in panel.Parent.Controls)
                    {
                        if (control.IsTitle) continue;
                        if (!control.Checked)
                        {
                            selectAll = false;
                            break;
                        }
                    }
                }

                var title = panel.Parent.Controls[panel.Parent.Controls.Count - 1] as POSPanel;
                if (title != null && title.IsTitle && title.Checked != selectAll)
                {
                    title.OnSelectAll -= POSPanelOnSelectAll;
                    title.OnSelectNone -= POSPanelOnSelectNone;

                    title.Checked = selectAll;

                    title.OnSelectAll += POSPanelOnSelectAll;
                    title.OnSelectNone += POSPanelOnSelectNone;
                }
            }
        }

        public Boolean SelectionVisible
        {
            get { return _connectionPanel.SelectionVisible; }
            set
            {
                if (value)
                {
                    if (POSConnection == null)
                        Visible = false;
                }
                else
                {
                    Visible = true;
                }
                _connectionPanel.SelectionVisible = value;
            }
        }

        public Boolean EditVisible
        {
            set
            {
                _connectionPanel.EditVisible = value;
            }
        }

        public void ShowPOSConnection()
        {
            foreach (POSPanel control in _posListPanel.Controls)
            {
                control.SelectionVisible = false;
                control.Checked = false;
                control.POS = null;
                control.EditVisible = false;
                control.Cursor = Cursors.Hand;
                control.IsTitle = control.IsMiddleTitle = false;
                if (control.Height == 0)
                    continue;

                if (!_recyclePOS.Contains(control))
                    _recyclePOS.Enqueue(control);
            }
            _posListPanel.Controls.Clear();
            _posListPanel.Hide();

            Controls.Add(_connectionPanel);
        }

        private void EditButtonMouseClick(Object sender, EventArgs e)
        {
            if (POSConnection == null) return;

            if (OnPOSConnectionEditClick != null)
                OnPOSConnectionEditClick(this, e);
        }

        public void ClearViewModel()
        {
            _isEditing = false;
            ClearPOSListControls();
            _connectionPanel.POSConnection = null;

            Controls.Clear();
            Checked = false;
        }

        private void ClearPOSListControls()
        {
            foreach (POSPanel control in _posListPanel.Controls)
            {
                control.SelectionVisible = false;
                control.Checked = false;
                control.Enabled = true;
                control.POS = null;
                control.EditVisible = false;

                if (control.IsTitle)
                {
                    control.OnSelectAll -= POSPanelOnSelectAll;
                    control.OnSelectNone -= POSPanelOnSelectNone;
                    control.IsTitle = control.IsMiddleTitle = false;
                }

                if (control.Height == 0)
                    continue;

                if (!_recyclePOS.Contains(control))
                    _recyclePOS.Enqueue(control);
            }
            _posListPanel.Controls.Clear();
        }

        private void POSPanelOnSelectAll(Object sender, EventArgs e)
        {
            var scrollTop = _posListPanel.AutoScrollPosition.Y * -1;
            _posListPanel.AutoScroll = false;

            foreach (POSPanel posPanel in _posListPanel.Controls)
            {
                posPanel.Checked = true;
            }

            _posListPanel.AutoScroll = true;
            _posListPanel.AutoScrollPosition = new Point(0, scrollTop);
        }

        private void POSPanelOnSelectNone(Object sender, EventArgs e)
        {
            var scrollTop = _posListPanel.AutoScrollPosition.Y * -1;
            _posListPanel.AutoScroll = false;

            foreach (POSPanel posPanel in _posListPanel.Controls)
            {
                posPanel.Checked = false;
            }

            _posListPanel.AutoScroll = true;
            _posListPanel.AutoScrollPosition = new Point(0, scrollTop);
        }
    }

    public sealed class POSListPanel : Panel
    {
        public POSListPanel()
        {
            DoubleBuffered = true;
            Dock = DockStyle.Top;
            AutoSize = true;
        }
    }
}