using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Device;
using Interface;
using PanelBase;

namespace SetupDeviceGroup
{
    public sealed partial class SubLayoutEditPanel : UserControl
    {
        public event EventHandler<EventArgs<IDeviceLayout>> OnSubDeviceLayoutEdit;
        
        public IApp App;
        public IServer Server;
        private INVR _nvr;
        public IDeviceLayout DeviceLayout;
        public Dictionary<String, String> Localization;
        private VideoMonitor.VideoMonitor _monitor;
        public SubLayoutEditPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"DeviceLayoutPanel_Layout", "Layout"},
                                   {"DeviceLayoutPanel_Crop", "Crop"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Manager.BackgroundNoBorder;

            layoutLabel.Text = Localization["DeviceLayoutPanel_Layout"];
        }

        private SubLayoutPanel _subLayoutPanel1;
        private SubLayoutPanel _subLayoutPanel2;
        private SubLayoutPanel _subLayoutPanel3;
        private SubLayoutPanel _subLayoutPanel4;
        private SubLayoutPanel _subLayoutPanel5;
        public void Initialize()
        {
            _nvr = Server as INVR;
            var subLayoutTitlePanel = new SubLayoutPanel { NVR = _nvr, IsTitle = true };

            _subLayoutPanel1 = new SubLayoutPanel { NVR = _nvr, Id = 1 };
            _subLayoutPanel2 = new SubLayoutPanel { NVR = _nvr, Id = 2 };
            _subLayoutPanel3 = new SubLayoutPanel { NVR = _nvr, Id = 3 };
            _subLayoutPanel4 = new SubLayoutPanel { NVR = _nvr, Id = 4 };
            _subLayoutPanel5 = new SubLayoutPanel { NVR = _nvr, Id = 5 };

            _subLayoutPanel1.OnSubLayoutSetClick += SubLayoutPanelOnSubLayoutSetClick;
            _subLayoutPanel2.OnSubLayoutSetClick += SubLayoutPanelOnSubLayoutSetClick;
            _subLayoutPanel3.OnSubLayoutSetClick += SubLayoutPanelOnSubLayoutSetClick;
            _subLayoutPanel4.OnSubLayoutSetClick += SubLayoutPanelOnSubLayoutSetClick;
            _subLayoutPanel5.OnSubLayoutSetClick += SubLayoutPanelOnSubLayoutSetClick;

            _subLayoutPanel1.OnSubLayoutDoneClick += SubLayoutPanelOnSubLayoutDoneClick;
            _subLayoutPanel2.OnSubLayoutDoneClick += SubLayoutPanelOnSubLayoutDoneClick;
            _subLayoutPanel3.OnSubLayoutDoneClick += SubLayoutPanelOnSubLayoutDoneClick;
            _subLayoutPanel4.OnSubLayoutDoneClick += SubLayoutPanelOnSubLayoutDoneClick;
            _subLayoutPanel5.OnSubLayoutDoneClick += SubLayoutPanelOnSubLayoutDoneClick;

            _subLayoutPanel1.OnSubLayoutDeleteClick += SubLayoutPanelOnSubLayoutDeleteClick;
            _subLayoutPanel2.OnSubLayoutDeleteClick += SubLayoutPanelOnSubLayoutDeleteClick;
            _subLayoutPanel3.OnSubLayoutDeleteClick += SubLayoutPanelOnSubLayoutDeleteClick;
            _subLayoutPanel4.OnSubLayoutDeleteClick += SubLayoutPanelOnSubLayoutDeleteClick;
            _subLayoutPanel5.OnSubLayoutDeleteClick += SubLayoutPanelOnSubLayoutDeleteClick;

            _monitor = new VideoMonitor.VideoMonitor { App = App, Server = Server };
            _monitor.Initialize();
            _monitor.SetDeviceLayoutEditProperty();

            subDoubleBufferPanel.Controls.Add(_subLayoutPanel5);
            subDoubleBufferPanel.Controls.Add(_subLayoutPanel4);
            subDoubleBufferPanel.Controls.Add(_subLayoutPanel3);
            subDoubleBufferPanel.Controls.Add(_subLayoutPanel2);
            subDoubleBufferPanel.Controls.Add(_subLayoutPanel1);
            subDoubleBufferPanel.Controls.Add(subLayoutTitlePanel);
            containerPanel.Controls.Add(_monitor);
        }

        private void SubLayoutPanelOnSubLayoutSetClick(Object sender, EventArgs e)
        {
            var control = sender as SubLayoutPanel;
            if(control == null || DeviceLayout == null) return;

            if (!DeviceLayout.SubLayouts.ContainsKey(control.Id)) return;

            ISubLayout subLayout = DeviceLayout.SubLayouts[control.Id];
            if (subLayout == null)
            {
                subLayout = new SubLayout
                {
                    Id = control.Id,
                    DeviceLayout = DeviceLayout,
                    ReadyState = ReadyState.New,
                    Name = Localization["DeviceLayoutPanel_Crop"] + @" " + control.Id,
                    Server = Server,
                    Width = DeviceLayout.WindowWidth,
                    Height = DeviceLayout.WindowHeight,
                };
                DeviceLayout.SubLayouts[control.Id] = subLayout;
            }

            control.SubLayout = subLayout;

            _monitor.SetSubLayoutRegion(subLayout);
            _nvr.DeviceLayoutModify(DeviceLayout);
        }

        private void SubLayoutPanelOnSubLayoutDoneClick(Object sender, EventArgs e)
        {
            var control = sender as SubLayoutPanel;
            if (control == null || DeviceLayout == null) return;

            if (!DeviceLayout.SubLayouts.ContainsKey(control.Id)) return;

            _monitor.UpdateSubLayoutRegion(control.SubLayout);
            control.Invalidate();

            _nvr.SubLayoutModify(control.SubLayout);
            _nvr.DeviceLayoutModify(DeviceLayout);
        }

        private void SubLayoutPanelOnSubLayoutDeleteClick(Object sender, EventArgs e)
        {
            var control = sender as SubLayoutPanel;
            if (control == null || DeviceLayout == null) return;

            if (!DeviceLayout.SubLayouts.ContainsKey(control.Id)) return;

            control.SubLayout.ReadyState = ReadyState.Delete;
            _nvr.SubLayoutModify(control.SubLayout);

            control.SubLayout = null;
            DeviceLayout.SubLayouts[control.Id] = null;

            _monitor.SetSubLayoutRegion(null as ISubLayout);
            _nvr.DeviceLayoutModify(DeviceLayout);
        }

        private Boolean _isEdit;
        public void ParseDeviceLayout()
        {
            if (DeviceLayout == null) return;

            _isEdit = false;

            foreach (var layout in DeviceLayout.SubLayouts)
            {
                if (layout.Key == 1)
                    _subLayoutPanel1.SubLayout = layout.Value;

                if (layout.Key == 2)
                    _subLayoutPanel2.SubLayout = layout.Value;

                if (layout.Key == 3)
                    _subLayoutPanel3.SubLayout = layout.Value;

                if (layout.Key == 4)
                    _subLayoutPanel4.SubLayout = layout.Value;

                if (layout.Key == 5)
                    _subLayoutPanel5.SubLayout = layout.Value;
            }

            ShowVideo();

            _isEdit = true;

            containerPanel.Focus();
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
            _monitor.ClearAll();
        }

        private void ShowVideo()
        {
            if (DeviceLayout == null) return;

            //App.SetAirProviderLiveProperty(DeviceLayout);

            _monitor.ClearAll();
            _monitor.SetLayout(WindowLayouts.LayoutGenerate(1));
            _monitor.AppendDevice(DeviceLayout);
        }
    } 
}
