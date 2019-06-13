using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace TimeTrack
{
    public sealed class DeviceContainer : Panel
    {
        public event EventHandler OnSizeChange;
        private readonly Dictionary<UInt16, DeviceLabel> _labels = new Dictionary<UInt16, DeviceLabel>();
        private readonly DoubleBufferPanel _container;
        private readonly DoubleBufferPanel _arrowPanel;
        private static readonly Image _right = Resources.GetResources(Properties.Resources.expand, Properties.Resources.IMGExpand);
        private static readonly Image _left = Resources.GetResources(Properties.Resources.left, Properties.Resources.IMGLeft);
        private static readonly Image _bg = Resources.GetResources(Properties.Resources.deviceContainerBG, Properties.Resources.BGDeviceContainerBG);

        private const Int32 MiniWidth = 35;
        private const Int32 FullWidth = 120;
        public DeviceContainer()
        {
            //DoubleBuffered = true;
            BackgroundImage = _bg;
            BackgroundImageLayout = ImageLayout.Stretch;
            //BackColor = Color.FromArgb(120, 120, 120);
            Dock = DockStyle.Left;
            Location = new Point(0, 0);
            Size = new Size(MiniWidth, 129);
            Padding = new Padding(0, 46, 0, 0);

            _container = new DoubleBufferPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Visible = false,
            };
            
            _arrowPanel = new DoubleBufferPanel
            {
                Dock = DockStyle.None,
                Size = new Size(25, 15),
                Location = new Point(5, 29),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackgroundImage = _right,
                BackgroundImageLayout = ImageLayout.Center,
                Cursor = Cursors.Hand
            };
            
            _arrowPanel.MouseClick += ArrowPanelMouseClick;

            _container.Paint += ContainerPaint;
            Paint += ContainerPaint;
            Controls.Add(_container);
            Controls.Add(_arrowPanel);
        }

        public UInt16 PageIndex = 1;
        public UInt16 TrackerNumPerPage = 4;
        public UInt16 MaxConnection = 64;//16;//TimeTrackSwitch

        private void ContainerPaint(Object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.DrawLine(Pens.Black, Width - 1, Padding.Top - 1, Width - 1, Height);
        }

        public void UpdateContent(IDevice[] devices)
        {
            if (_labels.Count == 0)
            {
                for (UInt16 index = 0; index < MaxConnection; index++)
                {
                    var label = new DeviceLabel();
                    _container.Controls.Add(label);
                    var position = index % TrackerNumPerPage;
                    label.Visible = false;
                    label.Location = new Point(0, position * label.Height);

                    //label.BringToFront();
                    _labels.Add(index, label);
                }
            }

            if (_labels.Count == 0) return;

            var start = Convert.ToUInt16((PageIndex - 1) * TrackerNumPerPage);
            for (UInt16 index = 0; index < _labels.Count; index++)
            {
                var label = _labels[index];
                //not in this page, skip
                if (index < start || index >= start + TrackerNumPerPage)
                {
                    label.Visible = false;
                    continue;
                }

                if (index < devices.Length)
                {
                    label.Visible = true;
                    label.Device = devices[index];
                    label.Invalidate();
                }
            }
            
            _container.Visible = true;
            _arrowPanel.BackgroundImage = _left;
            Width = FullWidth;

            if (OnSizeChange != null)
                OnSizeChange(this, null);
        }

        public void RemoveAll()
        {
            foreach (KeyValuePair<UInt16, DeviceLabel> obj in _labels)
            {
                obj.Value.Device = null;
                obj.Value.Invalidate();
            }

            _container.Visible = false;
            Width = MiniWidth;
            _arrowPanel.BackgroundImage = _right;

            if (OnSizeChange != null)
                OnSizeChange(this, null);
        }

        private void ArrowPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (Width == MiniWidth)
            {
                _container.Visible = true;
                Width = FullWidth;
                _arrowPanel.BackgroundImage = _left;
            }
            else
            {
                _container.Visible = false;
                Width = MiniWidth;
                _arrowPanel.BackgroundImage = _right;
            }
            if (OnSizeChange != null)
                OnSizeChange(this, null);
        }
    }

    public sealed class DeviceLabel : Label
    {
        public IDevice Device;

        public DeviceLabel()
        {
            //Dock = DockStyle.Top;
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            AutoSize = false;
            Width = 120;
            Height = 21;
            BackColor = Color.Transparent;
            ForeColor = Color.White;

            Paint += DeviceLabelPaint;
        }

        private void DeviceLabelPaint(Object sender, PaintEventArgs e)
        {
            if (Device == null) return;

            var maxWidth = Width - 16; //left,right space 8

            var text = Device.ToString();
            Graphics g = e.Graphics;
            SizeF fSize = g.MeasureString(text, Font);

            //trim text if too long
            while (fSize.Width > maxWidth)
            {
                if (text.Length <= 1) break;

                text = text.Substring(0, text.Length - 1);
                fSize = g.MeasureString(text, Font);
            }
            if (text != Device.ToString())
            {
                //trim extra 2 word reaplce to ...
                if (text.Length > 2)
                    text = text.Substring(0, text.Length - 2);

                text += "...";
                fSize = g.MeasureString(text, Font);
            }

            g.DrawString(text, Font, Brushes.White, Width - fSize.Width - 8, 5);
        }
    }
}
