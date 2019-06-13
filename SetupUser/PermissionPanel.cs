using System;
using System.Drawing;
using System.Windows.Forms;
using SetupBase;

namespace SetupUser
{
    public class PermissionPanel : Panel
    {
        public String PermissionName { get; set; }
        public PermissionPanel()
        {
            DoubleBuffered = true;
            Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            BackColor = Color.Transparent;
            Dock = DockStyle.Top;
            Height = 40;
            Cursor = Cursors.Default;

            Paint += PagePermissionPanelPaint;
        }

        protected virtual void PagePermissionPanelPaint(Object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;

            if (Parent == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, control);

            if (Width <= 250) return;

            if (_hasPermission)
            {
                Manager.PaintSelected(g);
                Manager.PaintText(g, PermissionName, Manager.SelectedTextColor);
            }
            else
                Manager.PaintText(g, PermissionName);
        }

        private Boolean _hasPermission;
        public Boolean HasPermission
        {
            get { return _hasPermission; }
            set
            {
                _hasPermission = value;
				
                Invalidate();
            }
        }
    }
}