using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace SetupDevice
{
    public sealed partial class PresetTourPanel : UserControl
    {
        public IServer Server;
        public ICamera Camera;
        public PresetTour PresetTour;

        private readonly Queue<PointSelectPanel> _recyclePoints = new Queue<PointSelectPanel>();

        public PresetTourPanel()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = SetupBase.Manager.BackgroundNoBorder;
        }

        private Boolean _isEditing;
        public void Initialize()
        {
        }

        public void ParsePresetTour()
        {
            ClearViewModel();

            if (Camera == null) return;

            //add new tour as default
            if (Camera.PresetTours.Count > 0)
                PresetTour = Camera.PresetTours.Values.First();
            else
            {
                PresetTour = new PresetTour();

                for (UInt16 id = 1; id <= Camera.PresetTours.Count + 1; id++)
                {
                    if (Camera.PresetTours.ContainsKey(id)) continue;
                    PresetTour.Id = id;
                    break;
                }

                //PresetTour.Name = PresetTour.Id.ToString();
                Camera.PresetTours.Add(PresetTour.Id, PresetTour);
            }

            ShowPointsWithSelection();

            containerPanel.Focus();
        }

        public void ShowPointsWithSelection()
        {
            _isEditing = false;

            var selectAll = true;
            var tourPoints = new List<TourPoint>(PresetTour.Tour);

            var points = Camera.PresetPoints.Values.OrderBy(g => g.Id);
            
            //var presetPoints = new List<PresetPoint>(Camera.PresetPoints.Values););)

            if (points.ToArray().Length == 0)
            {
                _isEditing = true;
                return;
            }

            foreach (var point in points)
            {
                var pointSelectPanel = GetPointSelectPanel();
                pointSelectPanel.Camera = Camera;
                if (tourPoints.Count > 0)
                {

                    pointSelectPanel.TourPoint = tourPoints[0];
                    tourPoints.Remove(pointSelectPanel.TourPoint);
                    pointSelectPanel.Checked = true;
                }
                else
                {
                    pointSelectPanel.TourPoint = new TourPoint
                    {
                        Id = point.Id,
                        Duration = 5
                    };
                    pointSelectPanel.Checked = false;
                    selectAll = false;
                }

                containerPanel.Controls.Add(pointSelectPanel);
                pointSelectPanel.BringToFront();
            }

            var deviceTitlePanel = GetPointSelectPanel();
            deviceTitlePanel.IsTitle = true;
            deviceTitlePanel.Cursor = Cursors.Default;
            deviceTitlePanel.Checked = selectAll;
            deviceTitlePanel.OnSelectAll += PointSelectPanelOnSelectAll;
            deviceTitlePanel.OnSelectNone += PointSelectPanelOnSelectNone;
            containerPanel.Controls.Add(deviceTitlePanel);

            _isEditing = true;
        }

        private void ClearViewModel()
        {
            _isEditing = false;
            foreach (PointSelectPanel control in containerPanel.Controls)
            {
                control.Checked = false;
                control.TourPoint = null;

                if (control.IsTitle)
                {
                    control.OnSelectAll -= PointSelectPanelOnSelectAll;
                    control.OnSelectNone -= PointSelectPanelOnSelectNone;
                }

                control.IsTitle = false;

                if (!_recyclePoints.Contains(control))
                    _recyclePoints.Enqueue(control);
            }

            containerPanel.Controls.Clear();
            _isEditing = true;
        }

        private PointSelectPanel GetPointSelectPanel()
        {
            if (_recyclePoints.Count > 0)
            {
                return _recyclePoints.Dequeue();
            }

            var pointSelectPanel = new PointSelectPanel
            {
                Cursor = Cursors.Default,
                Server = Server,
            };
            pointSelectPanel.OnSelectChange += PointSelectPanelOnSelectChange;

            return pointSelectPanel;
        }

        private IEnumerable<TourPoint> TourSelection()
        {
            var points = new List<TourPoint>();
            foreach (PointSelectPanel control in containerPanel.Controls)
            {
                if (!control.Checked || control.TourPoint == null) continue;

                points.Add(control.TourPoint);
            }
            points.Reverse();

            return points;
        }

        private void PointSelectPanelOnSelectChange(Object sender, EventArgs e)
        {
            if (!_isEditing) return;

            var panel = sender as PointSelectPanel;
            if (panel == null) return;

            var selectAll = false;
            if (panel.Checked)
            {
                selectAll = true;
                foreach (PointSelectPanel control in containerPanel.Controls)
                {
                    if (control.IsTitle) continue;
                    if (!control.Checked)
                    {
                        selectAll = false;
                        break;
                    }
                }
            }

            var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as PointSelectPanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= PointSelectPanelOnSelectAll;
                title.OnSelectNone -= PointSelectPanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += PointSelectPanelOnSelectAll;
                title.OnSelectNone += PointSelectPanelOnSelectNone;
            }

            var presetTour = Camera.PresetTours.Values.First();
            presetTour.Tour.Clear();
            presetTour.Tour.AddRange(TourSelection());

            //dont save device setting just because preset is change
            Camera.PresetTours.IsModify = true;
            Camera.ReadyState = ReadyState.Modify;
            Server.DeviceModify(Camera);
        }

        private void PointSelectPanelOnSelectAll(Object sender, EventArgs e)
        {
            var pointSelectPanel = sender as PointSelectPanel;
            if (pointSelectPanel == null || pointSelectPanel.Parent == null) return;

            Panel panel = null;
            try
            {
                panel = ((Panel)Parent);
            }
            catch (Exception)
            {
            }

            var scrollTop = 0;

            if (panel != null)
            {
                scrollTop = panel.AutoScrollPosition.Y * -1;
                panel.AutoScroll = false;
            }

            foreach (PointSelectPanel control in pointSelectPanel.Parent.Controls)
            {
                control.Checked = true;
            }

            if (panel != null)
            {
                panel.AutoScroll = true;
                panel.AutoScrollPosition = new Point(0, scrollTop);
            }
        }

        private void PointSelectPanelOnSelectNone(Object sender, EventArgs e)
        {
            var pointSelectPanel = sender as PointSelectPanel;
            if (pointSelectPanel == null || pointSelectPanel.Parent == null) return;

            Panel panel = null;
            try
            {
                panel = ((Panel)Parent);
            }
            catch (Exception)
            {
            }

            var scrollTop = 0;

            if (panel != null)
            {
                scrollTop = panel.AutoScrollPosition.Y * -1;
                panel.AutoScroll = false;
            }

            foreach (PointSelectPanel control in pointSelectPanel.Parent.Controls)
            {
                control.Checked = false;
            }

            if (panel != null)
            {
                panel.AutoScroll = true;
                panel.AutoScrollPosition = new Point(0, scrollTop);
            }
        }
    } 
}
