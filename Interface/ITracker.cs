using System;
using System.ComponentModel;
using System.Windows.Forms;
using Constant;
using DeviceConstant;

namespace Interface
{
	public interface ITracker
	{
		ICamera Camera { get; set; }

		Boolean IsBusy { get; }
		Boolean Using { get; set; }
		Control Parent { get; set; }
		void Invalidate();

		Record GetFocusRecord();
		void ShowRecord();

		void AddBookmark();
		void EraserBookmark();

		void SearchEventAdd(EventType type);
		void SearchEventRemove(EventType type);

		void GetPart(Object sender, DoWorkEventArgs e);
		void GetPartAddFront(Object sender, DoWorkEventArgs e);
		void GetPartAddBack(Object sender, DoWorkEventArgs e);
	}
}
