using System;
using System.Windows.Forms;

namespace Interface
{
	public interface IViewBase
	{
		String Name { get; set; }
		Panel ViewModelPanel { get; set; }

		void UpdateView();
	    void UpdateToolTips();
		void UpdateView(String sort);
	    void UpdateRecordingStatus();
	    void Refresh();
        void UpdateFailoverSyncTimer(Boolean enable);
	}
}
