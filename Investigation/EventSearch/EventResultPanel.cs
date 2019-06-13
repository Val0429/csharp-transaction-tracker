using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using Constant;
using Constant.Utility;
using Device;
using Interface;
using PanelBase;

namespace Investigation.EventSearch
{
    public class EventResultPanel : Panel
    {
        public event EventHandler OnPlayback;

        public IApp App;
        public INVR NVR;
        public SearchPanel SearchPanel;

        public CameraEvents CameraEvent;
        public Dictionary<String, String> Localization;
        public Int32 Id;

        private readonly PictureBox _snapshot;

        public Boolean IsTitle;

        public EventResultPanel()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"EventResultPanell_ID", "ID"},
								   {"EventResultPanel_Device", "Device"},
								   {"EventResultPanel_DateTime", "DateTime"},
								   {"EventResultPanel_Event", "Event"},
							   };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.PanSouth;// Cursors.Hand;
            Size = new Size(640, 40);

            _snapshot = new PictureBox
            {
                Location = new Point(160, 43),
                BackgroundImageLayout = ImageLayout.Stretch,
                SizeMode = PictureBoxSizeMode.CenterImage,
                Size = new Size(320, 234),
                Image = _snapshotBg,
                Cursor = Cursors.Hand,
            };

            Paint += EventResultPanelPaint;
            _snapshot.Paint += SnapshotPaint;

            _snapshot.MouseClick += SnapshotMouseClick;

            Controls.Add(_snapshot);

            MouseClick += EventResultPanelMouseClick;
        }

        private Boolean _isReset;
        public Boolean IsLoadingImage;
        public Boolean IsLoad;

        private static readonly Image _noImage = Resources.GetResources(Properties.Resources.no_image, Properties.Resources.IMGNo_image);
        private static readonly Image _snapshotBg = Resources.GetResources(Properties.Resources.image, Properties.Resources.IMGImage);
        public void LoadSnapshot()
        {
            if (CameraEvent.Device == null || CameraEvent.Timecode == 0)
            {
                _snapshot.Image = _noImage;
                _snapshot.Tag = "";
                return;
            }

            var camera = CameraEvent.Device as ICamera;
            if (camera == null)
            {
                _snapshot.Image = _noImage;
                _snapshot.Tag = "";
                return;
            }

            _isReset = false;
            IsLoadingImage = true;


            var bitmap = camera.GetSnapshot(CameraEvent.Timecode, new Size(320, 240)) as Bitmap;
            //var bitmap = _camera.GetSnapshot(new Size(320, 240)) as Bitmap;
            UInt32 retry = 1;
            while (bitmap == null && retry > 0 && !_isReset)
            {
                retry--;
                bitmap = camera.GetSnapshot(CameraEvent.Timecode, new Size(320, 240)) as Bitmap;
                //bitmap = _camera.GetSnapshot(new Size(320, 240)) as Bitmap;
            }

            IsLoadingImage = false;
            if (_isReset)
                return;

            if (bitmap == null)
            {
                _snapshot.Image = _noImage;
                _snapshot.Tag = "";
            }
            else
            {
                _snapshot.Image = null;
                _snapshot.BackgroundImage = bitmap;
                _snapshot.Tag = bitmap.Tag;
            }
        }

        public void Reset()
        {
            CameraEvent = null;
            IsTitle = _isExpand = IsLoad = false;
            _snapshot.Image = _snapshotBg;
            _snapshot.BackgroundImage = null;
            Height = 40;
            Cursor = Cursors.PanSouth;

            _isReset = true;
        }

        private readonly Point _pointA = new Point(145, 102);
        private readonly Point _pointB = new Point(145, 132);
        private readonly Point _pointC = new Point(175, 117);

        private readonly Point _point1 = new Point(125, 89);
        private readonly Point _point2 = new Point(195, 89);
        private readonly Point _point3 = new Point(200, 94);
        private readonly Point _point4 = new Point(200, 140);
        private readonly Point _point5 = new Point(195, 145);
        private readonly Point _point6 = new Point(125, 145);
        private readonly Point _point7 = new Point(120, 140);
        private readonly Point _point8 = new Point(120, 94);
        private void SnapshotPaint(Object sender, PaintEventArgs e)
        {
            if (_snapshot.BackgroundImage == null) return;

            Graphics g = e.Graphics;

            Point[] rectPoints = {
				_point1, 
				_point2, 
				_point3,
				_point4,
				_point5,
				_point6,
				_point7,
				_point8,
			};
            var brush = new SolidBrush(Color.FromArgb(170, 0, 0, 0));
            g.FillPolygon(brush, rectPoints, FillMode.Alternate);

            Point[] curvePoints = {
				_pointA, 
				_pointB, 
				_pointC
			};
            g.FillPolygon(new SolidBrush(Color.White), curvePoints, FillMode.Alternate);
        }

        private static RectangleF _nameRectangleF = new RectangleF(90, 13, 126, 17);

        protected virtual void EventResultPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;


            if (IsTitle)
            {
                Manager.PaintTitleTopInput(g, this);

                if (Width <= 200) return;
                Manager.PaintTitleText(g, Localization["EventResultPanell_ID"]);
                g.DrawString(Localization["EventResultPanel_Device"], Manager.Font, Manager.TitleTextColor, 90, 13);

                if (Width <= 500) return;
                g.DrawString(Localization["EventResultPanel_DateTime"], Manager.Font, Manager.TitleTextColor, 330, 13);

                if (Width <= 610) return;
                g.DrawString(Localization["EventResultPanel_Event"], Manager.Font, Manager.TitleTextColor, 505, 13);
                return;
            }

            Manager.Paint(g, this);
            if (CameraEvent == null) return;

            if(NVR != null)
            {
                if (!NVR.Server.CheckProductNoToSupport("snapshot"))
                    Manager.PaintEdit(g, this);
                else
                {
                    if (Height == 40)
                        Manager.PaintExpand(g, this);
                    else
                        Manager.PaintCollapse(g, this);
                }
            }
            else
            {
                if (Height == 40)
                    Manager.PaintExpand(g, this);
                else
                    Manager.PaintCollapse(g, this);
            }

            if (Width <= 200) return;

            Manager.PaintText(g, Id.ToString());
            if(NVR is ICMS)
            {
                g.DrawString(String.Format("{0} ({1})", CameraEvent.Device.ToString(), CameraEvent.Device.Server.ToString()), Manager.Font, Brushes.Black, _nameRectangleF);
            }
            else
            {
                g.DrawString(CameraEvent.Device.ToString(), Manager.Font, Brushes.Black, _nameRectangleF);
            }

            if (Width <= 500) return;
            g.DrawString(CameraEvent.DateTime.ToDateTimeString(), Manager.Font, Brushes.Black, 330, 13);

            if (Width <= 610) return;
            g.DrawString(CameraEvent.ToLocalizationString(), Manager.Font, Brushes.Black, 505, 13);
        }

        private Boolean _isExpand;
        private void EventResultPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (IsTitle || CameraEvent == null) return;

            if(NVR != null)
            {
                if(!NVR.Server.CheckProductNoToSupport("snapshot"))
                {
                    if (OnPlayback != null)
                        OnPlayback(this, null);

                    return;
                }
            }

            _isExpand = !_isExpand;
            if (_isExpand)
            {
                Height = 300;
                Cursor = Cursors.PanNorth;
                if (!IsLoad)
                {
                    IsLoad = true;
                    if (CameraEvent.Device != null)
                        SearchPanel.QueueLoadSnapshot(this);
                    else
                    {
                        IsLoadingImage = false;
                        _snapshot.Image = _noImage;
                    }
                    _snapshot.Focus();
                    Invalidate();
                }
            }
            else
            {
                Height = 40;
                Cursor = Cursors.PanSouth;
                Invalidate();

                //refresh scroll bar
                var panel = Parent as Panel;
                if (panel != null)
                {
                    panel.Visible = false;

                    var scrollPosition = panel.AutoScrollPosition;
                    scrollPosition.Y *= -1;
                    panel.AutoScroll = false;
                    panel.AutoScroll = true;

                    panel.AutoScrollPosition = scrollPosition;
                    panel.Visible = true;
                    panel.Invalidate();
                }
            }
        }

        private void SnapshotMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnPlayback != null)
                OnPlayback(this, null);
        }
    }
}
