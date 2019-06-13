using System.Windows.Forms;

namespace PanelBase
{
    public sealed class HotKeyTextBox : TextBox
    {
        public HotKeyTextBox()
        {
            //if disable ShortcutsEnabled, ALSO the hotkey like CTRL+A, CTRL+V will be disable, you CANT get from key down event;
            ShortcutsEnabled = false; //disabe context menu to paste widechar

            //KeyDown += HotKeyTextBoxKeyDown;
        }

        //private void HotKeyTextBoxKeyDown(object sender, KeyEventArgs e)
        //{
        //    if (!e.Control) return;

        //    switch (e.KeyData)
        //    {
        //        case (Keys.Control | Keys.C):
        //            Copy();
        //            break;

        //        case (Keys.Control | Keys.X):
        //            Cut();
        //            break;

        //        case (Keys.Control | Keys.A):
        //            SelectAll();
        //            break;
        //    }
        //}
    }
}
