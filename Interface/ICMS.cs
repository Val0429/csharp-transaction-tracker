using System;

namespace Interface
{
	public interface ICMS : INVR
	{
		event EventHandler<EventArgs<INVR>> OnNVRModify;
        event EventHandler<EventArgs<INVR>> OnNVRStatusReceive;

		INVRManager NVRManager { get; }

		void ListenNVREvent(INVR nvr);

		void NVRModify(INVR nvr);

	}
}