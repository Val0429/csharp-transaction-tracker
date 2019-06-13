using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupDevice
{
    public partial class PtzCommandControl : UserControl
    {
        public Dictionary<String, String> Localization;
        public EditPanel EditPanel;
        public String HttpTooltip;
        public PtzCommandControl()
        {

            Localization = new Dictionary<String, String>
            {
                {"DevicePanel_PtzCommand", "PTZ Command"},
            };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Top;

            Paint += PtzCommandControlPaint;
        }

        private void PtzCommandControlPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawString(Localization["DevicePanel_PtzCommand"], Manager.Font, Brushes.DimGray, 8, 10);
        }

        private UriCommandPanel _upUriCommandPanel;
        private UriCommandPanel _downUriCommandPanel;
        private UriCommandPanel _leftUriCommandPanel;
        private UriCommandPanel _rightUriCommandPanel;
        private UriCommandPanel _upLeftUriCommandPanel;
        private UriCommandPanel _downLeftUriCommandPanel;
        private UriCommandPanel _upRightUriCommandPanel;
        private UriCommandPanel _downRightUriCommandPanel;
        private UriCommandPanel _stopUriCommandPanel;
        private UriCommandPanel _zoomInPanel;
        private UriCommandPanel _zoomOutPanel;
        private UriCommandPanel _zoomStopPanel;
        private UriCommandPanel _focusInPanel;
        private UriCommandPanel _focusOutPanel;
        private UriCommandPanel _focusStopPanel;

        private Dictionary<UInt16, UriCommandPanel> _presetPointPanels;
        private Dictionary<UInt16, UriCommandPanel> _gotoPresetPointPanels;
        private Dictionary<UInt16, UriCommandPanel> _deletePresetPointPanels;

        public void Initialize()
        {
            _upUriCommandPanel = new UriCommandPanel {EditPanel = EditPanel, HttpTooltip = HttpTooltip };
            _downUriCommandPanel = new UriCommandPanel { EditPanel = EditPanel, HttpTooltip = HttpTooltip };
            _leftUriCommandPanel = new UriCommandPanel { EditPanel = EditPanel, HttpTooltip = HttpTooltip };
            _rightUriCommandPanel = new UriCommandPanel { EditPanel = EditPanel, HttpTooltip = HttpTooltip };
            _upLeftUriCommandPanel = new UriCommandPanel { EditPanel = EditPanel, HttpTooltip = HttpTooltip };
            _downLeftUriCommandPanel = new UriCommandPanel { EditPanel = EditPanel, HttpTooltip = HttpTooltip };
            _upRightUriCommandPanel = new UriCommandPanel { EditPanel = EditPanel, HttpTooltip = HttpTooltip };
            _downRightUriCommandPanel = new UriCommandPanel { EditPanel = EditPanel, HttpTooltip = HttpTooltip };
            _stopUriCommandPanel = new UriCommandPanel { EditPanel = EditPanel, HttpTooltip = HttpTooltip };

            _zoomInPanel = new UriCommandPanel { EditPanel = EditPanel, HttpTooltip = HttpTooltip };
            _zoomOutPanel = new UriCommandPanel { EditPanel = EditPanel, HttpTooltip = HttpTooltip };
            _zoomStopPanel = new UriCommandPanel { EditPanel = EditPanel, HttpTooltip = HttpTooltip };
            _focusInPanel = new UriCommandPanel { EditPanel = EditPanel, HttpTooltip = HttpTooltip };
            _focusOutPanel = new UriCommandPanel { EditPanel = EditPanel, HttpTooltip = HttpTooltip };
            _focusStopPanel = new UriCommandPanel { EditPanel = EditPanel, HttpTooltip = HttpTooltip };

            //-----------------------------------------------
            _deletePresetPointPanels = new Dictionary<ushort, UriCommandPanel>();
            for (int i = 10; i >= 1; i--)
            {
                var newPanel = new UriCommandPanel { EditPanel = EditPanel, HttpTooltip = HttpTooltip };
                _deletePresetPointPanels.Add((ushort)i, newPanel);
                Controls.Add(newPanel);
            }

            var titleDeletePresetPointPanel = new UriCommandPanel { IsTitle = true, TitleType = "DeletePresetPoint" };
            Controls.Add(titleDeletePresetPointPanel);

            var titleDeletePresetPointLabel = new Panel {Height = 10, Dock = DockStyle.Top};
            Controls.Add(titleDeletePresetPointLabel);
            //-----------------------------------------------
            _gotoPresetPointPanels = new Dictionary<ushort, UriCommandPanel>();
            for (int i = 10; i >= 1; i--)
            {
                var newPanel = new UriCommandPanel { EditPanel = EditPanel, HttpTooltip = HttpTooltip };
                _gotoPresetPointPanels.Add((ushort)i, newPanel);
                Controls.Add(newPanel);
            }

            var titleGotoPresetPointPanel = new UriCommandPanel { IsTitle = true, TitleType = "GotoPresetPoint" };
            Controls.Add(titleGotoPresetPointPanel);
            var titleGotoPresetPointLabel = new Panel { Height = 10, Dock = DockStyle.Top };
            Controls.Add(titleGotoPresetPointLabel);
            //-----------------------------------------------
            _presetPointPanels = new Dictionary<ushort, UriCommandPanel>();
            for (int i = 10; i >= 1; i--)
            {
                var newPanel = new UriCommandPanel { EditPanel = EditPanel, HttpTooltip = HttpTooltip };
                _presetPointPanels.Add((ushort)i, newPanel);
                Controls.Add(newPanel);
            }

            var titlePresetPointPanel = new UriCommandPanel { IsTitle = true, TitleType = "PresetPoint"};
            Controls.Add(titlePresetPointPanel);
            var titlePresetPointLabel = new Panel { Height = 10, Dock = DockStyle.Top };
            Controls.Add(titlePresetPointLabel);
            //-----------------------------------------------
            var titleActionPanel = new UriCommandPanel{IsTitle =  true, TitleType = "Action"};

            Controls.Add(_focusStopPanel);
            Controls.Add(_focusOutPanel);
            Controls.Add(_focusInPanel);
            Controls.Add(_zoomStopPanel);
            Controls.Add(_zoomOutPanel);
            Controls.Add(_zoomInPanel);

            Controls.Add(_stopUriCommandPanel);
            Controls.Add(_downRightUriCommandPanel);
            Controls.Add(_upRightUriCommandPanel);
            Controls.Add(_downLeftUriCommandPanel);
            Controls.Add(_upLeftUriCommandPanel);
            Controls.Add(_rightUriCommandPanel);
            Controls.Add(_leftUriCommandPanel);
            Controls.Add(_downUriCommandPanel);
            Controls.Add(_upUriCommandPanel);
            Controls.Add(titleActionPanel);
        }

        public void ParseDevice()
        {
            if (EditPanel.Camera == null) return;

            _upUriCommandPanel.PtzCommandCgi = EditPanel.Camera.Profile.PtzCommand.Up;
            _downUriCommandPanel.PtzCommandCgi = EditPanel.Camera.Profile.PtzCommand.Down;
            _leftUriCommandPanel.PtzCommandCgi = EditPanel.Camera.Profile.PtzCommand.Left;
            _rightUriCommandPanel.PtzCommandCgi = EditPanel.Camera.Profile.PtzCommand.Right;
            _upLeftUriCommandPanel.PtzCommandCgi = EditPanel.Camera.Profile.PtzCommand.UpLeft;
            _downLeftUriCommandPanel.PtzCommandCgi = EditPanel.Camera.Profile.PtzCommand.DownLeft;
            _upRightUriCommandPanel.PtzCommandCgi = EditPanel.Camera.Profile.PtzCommand.UpRight;
            _downRightUriCommandPanel.PtzCommandCgi = EditPanel.Camera.Profile.PtzCommand.DownRight;
            _stopUriCommandPanel.PtzCommandCgi = EditPanel.Camera.Profile.PtzCommand.Stop;

            _zoomInPanel.PtzCommandCgi = EditPanel.Camera.Profile.PtzCommand.ZoomIn;
            _zoomOutPanel.PtzCommandCgi = EditPanel.Camera.Profile.PtzCommand.ZoomOut;
            _zoomStopPanel.PtzCommandCgi = EditPanel.Camera.Profile.PtzCommand.ZoomStop;
            _focusInPanel.PtzCommandCgi = EditPanel.Camera.Profile.PtzCommand.FocusIn;
            _focusOutPanel.PtzCommandCgi = EditPanel.Camera.Profile.PtzCommand.FocusOut;
            _focusStopPanel.PtzCommandCgi = EditPanel.Camera.Profile.PtzCommand.FocusStop;

            foreach (KeyValuePair<ushort, PtzCommandCgi> point in EditPanel.Camera.Profile.PtzCommand.PresetPoints)
            {
                _presetPointPanels[point.Key].PtzCommandCgi = point.Value;
            }

            foreach (KeyValuePair<ushort, PtzCommandCgi> point in EditPanel.Camera.Profile.PtzCommand.GotoPresetPoints)
            {
                _gotoPresetPointPanels[point.Key].PtzCommandCgi = point.Value;
            }

            foreach (KeyValuePair<ushort, PtzCommandCgi> point in EditPanel.Camera.Profile.PtzCommand.DeletePresetPoints)
            {
                _deletePresetPointPanels[point.Key].PtzCommandCgi = point.Value;
            }
        }
    }

    public sealed class UriCommandPanel: Panel
    {
        public EditPanel EditPanel;
        public String HttpTooltip;
        public Dictionary<String, String> Localization;
        private Boolean _isTitle;
        public Boolean IsTitle
        {
            get { return _isTitle; }
            set
            {
                _isTitle = value;
                if (value)
                {
                    Controls.Clear();
                }
            }
        }

        public String TitleType;
     
        private PtzCommandCgi _ptzCommandCgi;
        public PtzCommandCgi PtzCommandCgi
        {
            get { return _ptzCommandCgi; }
            set
            {
                _ptzCommandCgi = value;
                if (_ptzCommandCgi == null) return;

                _isEditing = false;

                _cgiTextBox.Text = _ptzCommandCgi.Cgi;
                _parameterTextBox.Text = _ptzCommandCgi.Parameter;
                _methodComboBox.SelectedValue = _ptzCommandCgi.Method;

                _parameterTextBox.Enabled = (PtzCommandCgi.Method == PtzCommandMethod.Post);

                _isEditing = true;
            }
        }

        private readonly TextBox _cgiTextBox;
        private readonly TextBox _parameterTextBox;
        private readonly ComboBox _methodComboBox;
        public UriCommandPanel()
        {
            Localization = new Dictionary<String, String>
            {
                {"DevicePanel_UriCommandPanel_Action", "Action"},
                {"DevicePanel_UriCommandPanel_ActionUp", "Up"},
                {"DevicePanel_UriCommandPanel_ActionDown", "Down"},
                {"DevicePanel_UriCommandPanel_ActionLeft", "Left"},
                {"DevicePanel_UriCommandPanel_ActionRight", "Right"},
                {"DevicePanel_UriCommandPanel_ActionUpLeft", "Up Left"},
                {"DevicePanel_UriCommandPanel_ActionDownLeft", "Down Left"},
                {"DevicePanel_UriCommandPanel_ActionUpRight", "Up Right"},
                {"DevicePanel_UriCommandPanel_ActionDownRight", "Down Right"},
                {"DevicePanel_UriCommandPanel_ActionStop", "Stop"},
                {"DevicePanel_UriCommandPanel_ActionZoomIn", "Zoom In"},
                {"DevicePanel_UriCommandPanel_ActionZoomOut", "Zoom Out"},
                {"DevicePanel_UriCommandPanel_ActionZoomStop", "Zoom Stop"},
                {"DevicePanel_UriCommandPanel_ActionFocusIn", "Focus In"},
                {"DevicePanel_UriCommandPanel_ActionFocusOut", "Focus Out"},
                {"DevicePanel_UriCommandPanel_ActionFocusStop", "Focus Stop"},

                {"DevicePanel_UriCommandPanel_AddPresetPoint", "Add Preset Point"},
                {"DevicePanel_UriCommandPanel_GotoPresetPoint", "Goto Preset Point"},
                {"DevicePanel_UriCommandPanel_DeletePresetPoint", "Delete Preset Point"},

                {"DevicePanel_UriCommandPanel_Method", "Method"},
                {"DevicePanel_UriCommandPanel_ActionCommand", "Action Command"},
                {"DevicePanel_UriCommandPanel_SetPresetPointCommand", "Set Preset Point Command"},
                {"DevicePanel_UriCommandPanel_GotoPresetPointCommand", "Goto Preset Point Command"},
                {"DevicePanel_UriCommandPanel_DeletePresetPointCommand", "Delete Preset Point Command"},
                {"DevicePanel_UriCommandPanel_PostParameter", "Parameter for Post  (ex: direction=up&speed=2)"}
            };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Default;
            Height = 40;
            //Padding = new Padding(3,3,3,3);

            BackColor = Color.Transparent;

            var methods = new DataTable();
            methods.Columns.Add("Value", typeof(PtzCommandMethod));
            methods.Columns.Add("Display", typeof(String));
            methods.Rows.Add(PtzCommandMethod.Get, "Get");
            methods.Rows.Add(PtzCommandMethod.Post, "Post");
            _methodComboBox = new ComboBox
                                  {
                                      Size = new Size(50, 22),
                                      Dock = DockStyle.None,
                                      Location = new Point(200, 8),
                                      Padding = new Padding(0, 0, 0, 0),
                                      Margin = new Padding(0),
                                      DropDownStyle = ComboBoxStyle.DropDownList,
                                      DataSource = methods,
                                      DisplayMember = "Display",
                                      ValueMember = "Value",
                                  };
            _methodComboBox.SelectedIndexChanged += MethodComboBoxSelectedChanged;
            Controls.Add(_methodComboBox);

            _cgiTextBox = new TextBox
                              {
                                  Size = new Size(310, 22),
                                  Dock = DockStyle.None,
                                  Location = new Point(380, 8),
                                  Padding = new Padding(0, 0, 0, 0),
                                  Margin = new Padding(0)
                              };
            _cgiTextBox.TextChanged += CgiTextBoxTextChanged;
            Controls.Add(_cgiTextBox);

            _parameterTextBox = new TextBox
            {
                Size = new Size(300, 22),
                Dock = DockStyle.None,
                Location = new Point(750, 8),
                Padding = new Padding(0, 0, 0, 0),
                Margin = new Padding(0)
            };
            _parameterTextBox.TextChanged += ParameterTextBoxTextChanged;
            Controls.Add(_parameterTextBox);

            Paint += UriCommandPanelPaint;
        }

        private Boolean _isEditing;
        private void CgiTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!_isEditing || PtzCommandCgi == null) return;

            PtzCommandCgi.Cgi = _cgiTextBox.Text;
            EditPanel.CameraIsModify();
        }

        private void ParameterTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!_isEditing || PtzCommandCgi == null) return;

            PtzCommandCgi.Parameter = _parameterTextBox.Text;
            EditPanel.CameraIsModify();
        }

        private void MethodComboBoxSelectedChanged(Object sender, EventArgs e)
        {
            if (!_isEditing || PtzCommandCgi == null) return;

            PtzCommandCgi.Method = (PtzCommandMethod) _methodComboBox.SelectedValue;
            _parameterTextBox.Enabled = PtzCommandCgi.Method == PtzCommandMethod.Post;
            EditPanel.CameraIsModify();
        }

        private void UriCommandPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;

            if (IsTitle)
            {
                Manager.PaintTitleTopInput(g, this);
                //Manager.Paint(g, this);
                PaintTitle(g);
                return;
            }
            Manager.Paint(g, this);

            if (PtzCommandCgi == null) return;

            if (Width <= 200) return;
            Manager.PaintText(g, Localization.ContainsKey("DevicePanel_UriCommandPanel_Action" + PtzCommandCgi.Name) ? Localization["DevicePanel_UriCommandPanel_Action" + PtzCommandCgi.Name] : PtzCommandCgi.Name);

            if (Width <= 300) return;
            g.DrawString(@"http://address:port/", Manager.Font, Brushes.Black, 270, 13);
        }

        private void PaintTitle(Graphics g)
        {
            if (Width <= 200) return;
            
            switch (TitleType)
            {
                case "Action":
                    Manager.PaintTitleText(g, Localization["DevicePanel_UriCommandPanel_Action"]);
                    break;

                case "PresetPoint":
                    Manager.PaintTitleText(g, Localization["DevicePanel_UriCommandPanel_AddPresetPoint"]);
                    break;

                case "GotoPresetPoint":
                    Manager.PaintTitleText(g, Localization["DevicePanel_UriCommandPanel_GotoPresetPoint"]);
                    break;

                case "DeletePresetPoint":
                    Manager.PaintTitleText(g, Localization["DevicePanel_UriCommandPanel_DeletePresetPoint"]);
                    break;
            }
            
            if (Width <= 300) return;
            g.DrawString(Localization["DevicePanel_UriCommandPanel_Method"], Manager.Font, Manager.TitleTextColor, 200, 13);

            if (Width <= 400) return;
            switch (TitleType)
            {
                case "Action":
                    g.DrawString(Localization["DevicePanel_UriCommandPanel_ActionCommand"], Manager.Font, Manager.TitleTextColor, 270, 13);
                    break;

                case "PresetPoint":
                    g.DrawString(Localization["DevicePanel_UriCommandPanel_SetPresetPointCommand"], Manager.Font, Manager.TitleTextColor, 270, 13);
                    break;

                case "GotoPresetPoint":
                    g.DrawString(Localization["DevicePanel_UriCommandPanel_GotoPresetPointCommand"], Manager.Font, Manager.TitleTextColor, 270, 13);
                    break;

                case "DeletePresetPoint":
                    g.DrawString(Localization["DevicePanel_UriCommandPanel_DeletePresetPointCommand"], Manager.Font, Manager.TitleTextColor, 270, 13);
                    break;
            }

            if (Width <= 850) return;
            g.DrawString(Localization["DevicePanel_UriCommandPanel_PostParameter"], Manager.Font, Manager.TitleTextColor, 750, 13);
        }
    }
}
