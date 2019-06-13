using System;
using System.Windows.Forms;
using Interface;

namespace DeviceTree.View
{
	public class ViewBase : IViewBase
	{
		public String Name { get; set; }
		public Panel ViewModelPanel { get; set; }

		public virtual void UpdateView()
		{
		}

		public virtual void UpdateView(String sort)
		{
		}
		
		public virtual void GenerateViewModel()
		{
		}

		public virtual void ClearViewModel()
		{
		}

		public virtual void UpdateToolTips()
		{
		}

		public virtual void UpdateRecordingStatus()
		{
		}

		public virtual void Refresh()
		{
		}

        public virtual void UpdateFailoverSyncTimer(Boolean enable)
        {
        }
	}
}
