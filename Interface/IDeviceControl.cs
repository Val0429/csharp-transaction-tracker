using System.Windows.Forms;

namespace Interface
{
	public interface IDeviceControl
	{
		event MouseEventHandler OnDeviceMouseDrag;
		event MouseEventHandler OnDeviceMouseDoubleClick;

		IDevice Device { get; set; }
	}
}
