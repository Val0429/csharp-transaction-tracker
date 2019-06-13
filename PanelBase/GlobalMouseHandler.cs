using System.Windows.Forms;

namespace PanelBase
{
    public delegate void MouseMovedEvent();

    public class GlobalMouseHandler : IMessageFilter
    {
        private const int WmMousemove = 0x0200;

        public event MouseMovedEvent TheMouseMoved;

        /// <summary>
        /// Use PreFilterMessage to filter out a message before it is dispatched to a control or form. 
        /// For example, to stop the Click event of a Button control from being dispatched to the control, 
        /// you implement the PreFilterMessage method and return a true value when the Click message occurs. 
        /// You can also use this method to perform code work that you might need to do before the message is dispatched.
        /// </summary>
        /// <param name="m"></param>
        /// <returns>
        /// true to filter the message and stop it from being dispatched; 
        /// false to allow the message to continue to the next filter or control.
        /// </returns>
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WmMousemove)
            {
                var handler = TheMouseMoved;
                if (handler != null)
                {
                    handler();
                }
            }

            return false;
        }
    }
}
