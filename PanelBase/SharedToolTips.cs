
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PanelBase
{
	public static class SharedToolTips
	{

		public static class SharedToolTip
		{
			//private static ToolTip _toolTip;
			private static Dictionary<Control, string> _controlDictionary;
			private static Dictionary<Control, ToolTip> _tipDictionary;
			public static void SetToolTip(Control control, string caption)
			{
				if (_controlDictionary == null)
					_controlDictionary = new Dictionary<Control, string>();

				if (_tipDictionary == null)
					_tipDictionary = new Dictionary<Control, ToolTip>();

				if (_controlDictionary.ContainsKey(control))
					_controlDictionary[control] = caption;
				else
				{
					_controlDictionary.Add(control, caption);
					control.MouseEnter += ControlMouseEnter;
				}
			}

            public static void ReleaseToolTip(Control control)
            {
                if (_controlDictionary.ContainsKey(control))
                {
                    control.MouseEnter -= ControlMouseEnter;
                    if (_tipDictionary.ContainsKey(control))
                    {
                        _tipDictionary[control].SetToolTip(control, null);
                    }
                    _controlDictionary.Remove(control);
                    
                } 
            }

			private static void ControlMouseEnter(Object sender, EventArgs e)
			{
				var control = (Control) sender;

				if (!_controlDictionary.ContainsKey(control)) return;

				if (control.Parent == null)//impossible!!
					return;

				//共用tooltips會有無法顯示的問題(當其他共用的control為不可視的狀態 \就會影響到可視的control的tooltips顯示)
				control.MouseEnter -= ControlMouseEnter;
				if (_tipDictionary.ContainsKey(control.Parent))
				{
					_tipDictionary[control.Parent].SetToolTip(control, _controlDictionary[control]);
				}
				else
				{
					var toolTip = new ToolTip { ShowAlways = true };
					toolTip.SetToolTip(control, _controlDictionary[control]);
					_tipDictionary.Add(control.Parent, toolTip);
				}

				_controlDictionary.Remove(control);
			}
		}
	}
}
