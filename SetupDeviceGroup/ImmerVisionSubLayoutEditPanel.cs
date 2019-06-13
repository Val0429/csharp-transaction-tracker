using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;
using PanelBase;

namespace SetupDeviceGroup
{
    public sealed partial class ImmerVisionSubLayoutEditPanel : UserControl
    {
        public event EventHandler<EventArgs<IDeviceLayout>> OnSubDeviceLayoutEdit;
        
        public IApp App;
        public IServer Server;
        private INVR _nvr;
        public IDeviceLayout DeviceLayout;
	    //public IDevice Camera;
        public Dictionary<String, String> Localization;

        private VideoMonitor.VideoMonitor _monitor;
		public ImmerVisionSubLayoutEditPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"DeviceLayoutPanel_Layout", "Layout"},
                                   {"DeviceLayoutPanel_SubLayout", "Sub-Layout"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Manager.Background;

            layoutLabel.Text = Localization["DeviceLayoutPanel_Layout"];
        }

        private ImmerVisionSubLayoutPanel _subLayoutPanel1;
		private ImmerVisionSubLayoutPanel _subLayoutPanel2;
		private ImmerVisionSubLayoutPanel _subLayoutPanel3;
		private ImmerVisionSubLayoutPanel _subLayoutPanel4;

        public void Initialize()
        {
            _nvr = Server as INVR;
			var subLayoutTitlePanel = new ImmerVisionSubLayoutPanel { NVR = _nvr, IsTitle = true };

			_subLayoutPanel1 = new ImmerVisionSubLayoutPanel { NVR = _nvr, Id = 1, IsTitle = false };
			_subLayoutPanel2 = new ImmerVisionSubLayoutPanel { NVR = _nvr, Id = 2, IsTitle = false };
			_subLayoutPanel3 = new ImmerVisionSubLayoutPanel { NVR = _nvr, Id = 3, IsTitle = false };
			_subLayoutPanel4 = new ImmerVisionSubLayoutPanel { NVR = _nvr, Id = 4, IsTitle = false };

			subLayoutTitlePanel.OnSubLayoutDoneClick += subLayoutTitlePanel_OnSubLayoutDoneClick;

            _monitor = new VideoMonitor.VideoMonitor { App = App, Server = Server };
            _monitor.Initialize();
            _monitor.SetDeviceLayoutEditProperty();

            subDoubleBufferPanel.Controls.Add(_subLayoutPanel4);
            subDoubleBufferPanel.Controls.Add(_subLayoutPanel3);
            subDoubleBufferPanel.Controls.Add(_subLayoutPanel2);
            subDoubleBufferPanel.Controls.Add(_subLayoutPanel1);
            subDoubleBufferPanel.Controls.Add(subLayoutTitlePanel);
            containerPanel.Controls.Add(_monitor);
        }

		void subLayoutTitlePanel_OnSubLayoutDoneClick(object sender, EventArgs e)
		{
			var control = sender as ImmerVisionSubLayoutPanel;
			if (control == null || DeviceLayout == null) return;

			var xml = _monitor.UpdateSubLayoutRegion();
			//<Regions><Area>35,22,282,206</Area><Area>94,79,274,221</Area><Area>168,138,294,214</Area><Area>245,191,250,177</Area></Regions>
			control.Invalidate();

			var _doc = Xml.LoadXml(xml);

			if ( _doc == null) return;

			var areas = _doc.GetElementsByTagName("Area");

			UInt16 idx = 1;
			foreach (XmlNode area in areas)
			{
				var txt = area.InnerText;
				//0,0,529,182

				var val = txt.Split(',');

				DeviceLayout.SubLayouts[idx].X = Math.Max(Convert.ToInt32(val[0]), 0);
				DeviceLayout.SubLayouts[idx].Y = Math.Max(Convert.ToInt32(val[1]), 0);
				DeviceLayout.SubLayouts[idx].Width = Math.Max(Convert.ToInt32(val[2]), 0);
				DeviceLayout.SubLayouts[idx].Height = Math.Max(Convert.ToInt32(val[3]), 0);

				idx++;
			}

			subDoubleBufferPanel.Invalidate();

			//_nvr.SubLayoutModify(control.SubLayout);
			//_nvr.DeviceLayoutModify(DeviceLayout);
		}

		//private void SubLayoutPanelOnSubLayoutDoneClick(Object sender, EventArgs e)
		//{
		//    var control = sender as SubLayoutPanel;
		//    if (control == null || DeviceLayout == null) return;

		//    if (!DeviceLayout.SubLayouts.ContainsKey(control.Id)) return;

            
		//    control.Invalidate();

		//    _nvr.SubLayoutModify(control.SubLayout);
		//    _nvr.DeviceLayoutModify(DeviceLayout);
		//}

        private Boolean _isEdit;
        public void ParseDeviceLayout()
        {
            if (DeviceLayout == null) return;

            _isEdit = false;

			var subCount = DeviceLayout.SubLayouts.Count;
	        if (DeviceLayout.isIncludeDevice) subCount--;

			switch (subCount)
			{
				case 1:
					if (!subDoubleBufferPanel.Controls.Contains(_subLayoutPanel1))
					{
						subDoubleBufferPanel.Controls.Add(_subLayoutPanel1);
						subDoubleBufferPanel.Controls.SetChildIndex(_subLayoutPanel1, 0);
					}

					subDoubleBufferPanel.Controls.Remove(_subLayoutPanel2);
					subDoubleBufferPanel.Controls.Remove(_subLayoutPanel3);
					subDoubleBufferPanel.Controls.Remove(_subLayoutPanel4);
					break;
				case 2:
					if (!subDoubleBufferPanel.Controls.Contains(_subLayoutPanel1))
					{
						subDoubleBufferPanel.Controls.Add(_subLayoutPanel1);
						subDoubleBufferPanel.Controls.SetChildIndex(_subLayoutPanel1, 0);
					}

					if (!subDoubleBufferPanel.Controls.Contains(_subLayoutPanel2))
					{
						subDoubleBufferPanel.Controls.Add(_subLayoutPanel2);
						subDoubleBufferPanel.Controls.SetChildIndex(_subLayoutPanel2, 0);
					}

					subDoubleBufferPanel.Controls.Remove(_subLayoutPanel3);
					subDoubleBufferPanel.Controls.Remove(_subLayoutPanel4);
					break;
				case 4:
					if (!subDoubleBufferPanel.Controls.Contains(_subLayoutPanel1))
					{
						subDoubleBufferPanel.Controls.Add(_subLayoutPanel1);
						subDoubleBufferPanel.Controls.SetChildIndex(_subLayoutPanel1, 0);
					}

					if (!subDoubleBufferPanel.Controls.Contains(_subLayoutPanel2))
					{
						subDoubleBufferPanel.Controls.Add(_subLayoutPanel2);
						subDoubleBufferPanel.Controls.SetChildIndex(_subLayoutPanel2, 0);
					}

					if (!subDoubleBufferPanel.Controls.Contains(_subLayoutPanel3))
					{
						subDoubleBufferPanel.Controls.Add(_subLayoutPanel3);
						subDoubleBufferPanel.Controls.SetChildIndex(_subLayoutPanel3, 0);
					}

					if (!subDoubleBufferPanel.Controls.Contains(_subLayoutPanel4))
					{
						subDoubleBufferPanel.Controls.Add(_subLayoutPanel4);
						subDoubleBufferPanel.Controls.SetChildIndex(_subLayoutPanel4, 0);
					}

					break;
			}


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
			var str = _monitor.UpdateSubLayoutRegion();
            _monitor.ClearAll();
        }

        private void ShowVideo()
        {
			if (DeviceLayout == null) return;

            //App.SetAirProviderLiveProperty(DeviceLayout);

            _monitor.ClearAll();
            _monitor.SetLayout(WindowLayouts.LayoutGenerate(1));

			_monitor.VideoWindows[0].Viewer.OnPlay -= Viewer_OnPlay;
			_monitor.VideoWindows[0].Viewer.OnPlay += Viewer_OnPlay;

			_monitor.AppendDevice(DeviceLayout);
        }

		void Viewer_OnPlay(object sender, EventArgs<int> e)
		{
			var subLayouts = new List<ISubLayout>();

			foreach (var layout in DeviceLayout.SubLayouts)
			{
				if (layout.Key == 99) continue;
				subLayouts.Add(layout.Value);
			}

			_monitor.SetSubLayoutRegion(subLayouts);
		}
    } 
}
