using System;
using System.Security.Permissions;
using System.Windows.Forms;
using Interface;

namespace ViewerAir
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]

    public partial class VideoProvider : UserControl
    {
        public IDeviceLayout DeviceLayout;
        public VideoProvider()
        {
            InitializeComponent();
        }

        public String Version
        {
            get
            {
                return "";
            }
        }
    }
}
