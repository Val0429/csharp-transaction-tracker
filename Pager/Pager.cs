using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;

namespace Pager
{
	public sealed partial class Pager : UserControl, IControl
	{
		public event EventHandler<EventArgs<List<WindowLayout>>> OnLayoutChange;

		public String TitleName { get; set; }
        private IServer _server;
        private INVR _nvr;
        public IServer Server {
            get { return _server; } 
            set
            {
                _server = value;
				if (value is INVR)
				{
                    _nvr = value as INVR;
				}
            } 
        }

		public Pager()
		{
			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Top;
		}

		public void Initialize()
		{
		}

		public void LayoutChange(Object sender, EventArgs<List<WindowLayout>> e)
		{
			if(e.Value == null) return;

			foreach (LayoutButton layoutButton in _buttons)
			{
				layoutButton.Active = (WindowLayouts.LayoutCompare(layoutButton.WindowLayout, e.Value));
			}
		}

		public UInt16 ButtonWidth;
		public UInt16 MiniButtonCount;
		public UInt16 TotalButtonCount;
		private readonly List<LayoutButton> _buttons = new List<LayoutButton>();
		public void SetLiveProperty()
		{
            if(_nvr != null)
            {
                if (_nvr.Server.CheckProductNoToSupportNumber("liveChannel") == 16)
                {
                    SetLayoutButtonsFor16();
                    return;
                }
            }

			ButtonWidth = 21;
			MiniButtonCount = 7;
			//TotalButtonCount = 18;

			Padding = new Padding(5, 8, 15, 8);
			_buttons.Add(new LayoutButton { LayoutFormat = "********,********,********,********,********,********,********,********" });//64
			_buttons.Add(new LayoutButton { LayoutFormat = "******,******,******,******,******,******" });//36
			_buttons.Add(new LayoutButton { LayoutFormat = "*****,*****,*****,*****,*****" });//25
			_buttons.Add(new LayoutButton { LayoutFormat = "****,****,****,****" });//16
			_buttons.Add(new LayoutButton { LayoutFormat = "***,***,***" });//9
			_buttons.Add(new LayoutButton { LayoutFormat = "**,**", Active = true, });//4
			_buttons.Add(new LayoutButton { LayoutFormat = "*" });
			_buttons.Add(new LayoutButton { LayoutFormat = "*******,*******,*******,*******,*******,*******,*******" });//49
			_buttons.Add(new LayoutButton { LayoutFormat = "****,****" });//8
			_buttons.Add(new LayoutButton { LayoutFormat = "1111,1111,****,****" });//9 B
			_buttons.Add(new LayoutButton { LayoutFormat = "11**,11**,11**,11**" });//9 A
			_buttons.Add(new LayoutButton { LayoutFormat = "11**,11**,22**,22**" });//10
			_buttons.Add(new LayoutButton { LayoutFormat = "1*,1*,2*,2*" });//6
			_buttons.Add(new LayoutButton { LayoutFormat = "111*,111*,111*,****" });//8 A
			_buttons.Add(new LayoutButton { LayoutFormat = "11*,11*,***" });//6 A
			_buttons.Add(new LayoutButton { LayoutFormat = "11**,11**" });//5
			_buttons.Add(new LayoutButton { LayoutFormat = "1*,1*" });//3A
			_buttons.Add(new LayoutButton { LayoutFormat = "11,**" });//3
			_buttons.Add(new LayoutButton { LayoutFormat = "*,*" });//2 A
			_buttons.Add(new LayoutButton { LayoutFormat = "**"});//2
			TotalButtonCount = (UInt16)_buttons.Count;

			var size = new Size(15, 15);
			flowLayoutPanel.FlowDirection = FlowDirection.RightToLeft;
			flowLayoutPanel.WrapContents = false;
			foreach (LayoutButton layoutButton in _buttons)
			{
				layoutButton.Size = size;
				layoutButton.Margin = new Padding(3, 0, 3, 0);
				//layoutButton.Dock = DockStyle.Right;
				layoutButton.OnLayoutMouseDown += LayoutControlOnLayoutMouseDown;
				flowLayoutPanel.Controls.Add(layoutButton);
			}

			//Height = Convert.ToInt32(Math.Ceiling(_buttons.Count / 2.0) * 25) + Padding.Top + Padding.Bottom;
		}

		public void SetLayoutProperty()
		{
			_buttons.Add(new LayoutButton { LayoutFormat = "*"});
			_buttons.Add(new LayoutButton { LayoutFormat = "**,**", Active = true});//4
			_buttons.Add(new LayoutButton { LayoutFormat = "***,***,***"});//9
			_buttons.Add(new LayoutButton { LayoutFormat = "11*,11*,***"});//6 A
			_buttons.Add(new LayoutButton { LayoutFormat = "111*,111*,111*,****"});//8 A
			_buttons.Add(new LayoutButton { LayoutFormat = "**"});//2
			_buttons.Add(new LayoutButton { LayoutFormat = "*,*"});//2A
			_buttons.Add(new LayoutButton { LayoutFormat = "11,**"});//3
			_buttons.Add(new LayoutButton { LayoutFormat = "11**,11**,11**,11**"});//9 A
			_buttons.Add(new LayoutButton { LayoutFormat = "1111,1111,****,****"});//9B
			_buttons.Add(new LayoutButton { LayoutFormat = "1*,1*,2*,2*"});//6
			_buttons.Add(new LayoutButton { LayoutFormat = "11**,11**,22**,22**"});//10
			_buttons.Add(new LayoutButton { LayoutFormat = "****,****"});//8
			_buttons.Add(new LayoutButton { LayoutFormat = "****,****,****,****"});//16
			_buttons.Add(new LayoutButton { LayoutFormat = "*****,*****,*****,*****,*****"});//25
			_buttons.Add(new LayoutButton { LayoutFormat = "******,******,******,******,******,******"});//36
			_buttons.Add(new LayoutButton { LayoutFormat = "*******,*******,*******,*******,*******,*******,*******"});//49
			_buttons.Add(new LayoutButton { LayoutFormat = "********,********,********,********,********,********,********,********"});//64

			var size = new Size(30, 30);
			foreach (LayoutButton layoutButton in _buttons)
			{
				layoutButton.Size = size;
				layoutButton.OnLayoutMouseDown += LayoutControlOnLayoutMouseDown;
				flowLayoutPanel.Controls.Add(layoutButton);
			}
			BackgroundImageLayout = ImageLayout.Stretch;
			BackgroundImage = Resources.GetResources(Properties.Resources.banner, Properties.Resources.IMGBanner);
		}

		public void SetPlaybackProperty()
		{
			if(_nvr != null)
			{
                if (_nvr.Server.CheckProductNoToSupportNumber("playbackChannel") == 16)
                {
                    SetLayoutButtonsFor16();
                }
                else
                {
                    SetLayoutButtonsFor4();
                }
			}
            else
			{
                SetLayoutButtonsFor16();
			}
		}

        private void SetLayoutButtonsFor16()
        {
            ButtonWidth = 21;
            MiniButtonCount = 5;
            //TotalButtonCount = 14;

            Padding = new Padding(5, 8, 15, 8);
            _buttons.Add(new LayoutButton { LayoutFormat = "**,**", Active = true, });//4
            _buttons.Add(new LayoutButton { LayoutFormat = "11,**" });//3
            _buttons.Add(new LayoutButton { LayoutFormat = "*,*" });//2 A
            _buttons.Add(new LayoutButton { LayoutFormat = "**" });//2
            _buttons.Add(new LayoutButton { LayoutFormat = "*" });
            _buttons.Add(new LayoutButton { LayoutFormat = "****,****,****,****" });//16
            _buttons.Add(new LayoutButton { LayoutFormat = "***,***,***" });//9
            _buttons.Add(new LayoutButton { LayoutFormat = "****,****" });//8
            _buttons.Add(new LayoutButton { LayoutFormat = "1111,1111,****,****" });//9 B
            _buttons.Add(new LayoutButton { LayoutFormat = "11**,11**,11**,11**" });//9 A
            _buttons.Add(new LayoutButton { LayoutFormat = "11**,11**,22**,22**" });//10
            _buttons.Add(new LayoutButton { LayoutFormat = "1*,1*,2*,2*" });//6
            _buttons.Add(new LayoutButton { LayoutFormat = "111*,111*,111*,****" });//8 A
            _buttons.Add(new LayoutButton { LayoutFormat = "11*,11*,***" });//6 A
            _buttons.Add(new LayoutButton { LayoutFormat = "11**,11**" });//5
            _buttons.Add(new LayoutButton { LayoutFormat = "1*,1*" });//3A

            TotalButtonCount = (UInt16)_buttons.Count;

            var size = new Size(15, 15);
            flowLayoutPanel.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel.WrapContents = false;
            foreach (LayoutButton layoutButton in _buttons)
            {
                layoutButton.Size = size;
                layoutButton.Margin = new Padding(3, 0, 3, 0);
                //layoutButton.Dock = DockStyle.Right;
                layoutButton.OnLayoutMouseDown += LayoutControlOnLayoutMouseDown;
                flowLayoutPanel.Controls.Add(layoutButton);
            }

            //Height = Convert.ToInt32(Math.Ceiling(_buttons.Count / 2.0) * 25) + Padding.Top + Padding.Bottom;
        }

        private void SetLayoutButtonsFor4()
        {
            ButtonWidth = 21;
            MiniButtonCount = 5;
            //TotalButtonCount = 14;

            Padding = new Padding(5, 8, 15, 8);
            _buttons.Add(new LayoutButton { LayoutFormat = "**,**", Active = true, });//4
            _buttons.Add(new LayoutButton { LayoutFormat = "11,**" });//3
            _buttons.Add(new LayoutButton { LayoutFormat = "*,*" });//2 A
            _buttons.Add(new LayoutButton { LayoutFormat = "**" });//2
            _buttons.Add(new LayoutButton { LayoutFormat = "*" });
            _buttons.Add(new LayoutButton { LayoutFormat = "1*,1*" });//3A

            TotalButtonCount = (UInt16)_buttons.Count;

            var size = new Size(15, 15);
            flowLayoutPanel.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel.WrapContents = false;
            foreach (LayoutButton layoutButton in _buttons)
            {
                layoutButton.Size = size;
                layoutButton.Margin = new Padding(3, 0, 3, 0);
                //layoutButton.Dock = DockStyle.Right;
                layoutButton.OnLayoutMouseDown += LayoutControlOnLayoutMouseDown;
                flowLayoutPanel.Controls.Add(layoutButton);
            }

            //Height = Convert.ToInt32(Math.Ceiling(_buttons.Count / 2.0) * 25) + Padding.Top + Padding.Bottom;
        }

		private void LayoutControlOnLayoutMouseDown(Object sender, MouseEventArgs e)
		{
			if (OnLayoutChange != null)
				OnLayoutChange(this, new EventArgs<List<WindowLayout>>(((LayoutButton)sender).WindowLayout));
			else
			{
				var button = sender as LayoutButton;

				if (button != null)
				{
					foreach (LayoutButton layoutButton in _buttons)
					{
						layoutButton.Active = (layoutButton == button);
					}
				}
			}
		}

		public void Activate()
		{
		}

		public void Deactivate()
		{
		}
	}
}
