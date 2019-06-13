using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace SetupPOSConnection
{
    public sealed partial class ListPanel : UserControl
    {
        public event EventHandler<EventArgs<IPOSConnection>> OnPOSConnectionEdit;
        public event EventHandler OnPOSConnectionAdd;

        public IPTS PTS;

        public Dictionary<String, String> Localization;

        public ListPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupPOSConnection_AddNewPOSConnection", "Add new POS Connection..."},
                                   {"SetupPOSConnection_AddedPOSConnection", "Added POS Connection"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Manager.BackgroundNoBorder;
            addedPOSConnectionLabel.Text = Localization["SetupPOSConnection_AddedPOSConnection"];
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

            Manager.PaintText(g, Localization["SetupPOSConnection_AddNewPOSConnection"]);
        }

        private readonly Queue<POSConnectionPanel> _recyclePOSConnectionPanel = new Queue<POSConnectionPanel>();

        private Point _previousScrollPosition; 
        public void GenerateViewModel()
        {
            _previousScrollPosition = containerPanel.AutoScrollPosition;
            _previousScrollPosition.Y *= -1;
            ClearViewModel();
            addedPOSConnectionLabel.Visible = addNewDoubleBufferPanel.Visible = true;

            List<IPOSConnection> sortResult = null;
            if (PTS != null)
            {
                sortResult = new List<IPOSConnection>(PTS.POS.Connections.Values);
            }

            if (sortResult == null)
            {
                addedPOSConnectionLabel.Visible = false;
                return;
            }

            sortResult.Sort((x, y) => (y.Id - x.Id));

            if (sortResult.Count == 0)
            {
                addedPOSConnectionLabel.Visible = false;
                return;
            }

            addedPOSConnectionLabel.Visible = true;
            containerPanel.Visible = false;
            foreach (IPOSConnection posConnection in sortResult)
            {
                if(posConnection.IsCapture) continue;
                var posConnectionPanel = GetPOSConnectionPanel();

                posConnectionPanel.POSConnection = posConnection;
                posConnectionPanel.EditVisible = true;

                posConnectionPanel.ShowPOS();
                containerPanel.Controls.Add(posConnectionPanel);
            }
            containerPanel.Visible = true;
            containerPanel.Focus();
            containerPanel.AutoScrollPosition = _previousScrollPosition;
        }

        public Boolean SelectionVisible
        {
            set
            {
                foreach (POSConnectionPanel posConnectionPanel in containerPanel.Controls)
                    posConnectionPanel.SelectionVisible = value;
            }
        }

        private POSConnectionPanel GetPOSConnectionPanel()
        {
            if (_recyclePOSConnectionPanel.Count > 0)
            {
                return _recyclePOSConnectionPanel.Dequeue();
            }

            var posConnectionPanel = new POSConnectionPanel
            {
                PTS = PTS,
                EditVisible = true,
            };

            posConnectionPanel.OnPOSConnectionEditClick += POSConnectionPanelPOSConnectionEditClick;

            return posConnectionPanel;
        }

        public void RemoveSelectedPOSConnection()
        {
            foreach (POSConnectionPanel posConnectionPanel in containerPanel.Controls)
            {
                if (!posConnectionPanel.Checked) continue;

                PTS.POS.Connections.Remove(posConnectionPanel.POSConnection.Id);
            }
        }

        private void AddNewDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnPOSConnectionAdd != null)
                OnPOSConnectionAdd(this, e);
        }

        private void POSConnectionPanelPOSConnectionEditClick(Object sender, EventArgs e)
        {
            if (OnPOSConnectionEdit != null)
                OnPOSConnectionEdit(this, new EventArgs<IPOSConnection>(((POSConnectionPanel)sender).POSConnection));
        }

        public void ShowPOSConnection()
        {
            addedPOSConnectionLabel.Visible = addNewDoubleBufferPanel.Visible = false;

            foreach (POSConnectionPanel posConnectionPanel in containerPanel.Controls)
            {
                posConnectionPanel.ShowPOSConnection();
                posConnectionPanel.EditVisible = false;
            }

        }

        private void ClearViewModel()
        {
            foreach (POSConnectionPanel posConnectionPanel in containerPanel.Controls)
            {
                posConnectionPanel.ClearViewModel();
                posConnectionPanel.POSConnection = null;
                if (!_recyclePOSConnectionPanel.Contains(posConnectionPanel))
                    _recyclePOSConnectionPanel.Enqueue(posConnectionPanel);
            }

            containerPanel.Controls.Clear();
        }
    } 
}
