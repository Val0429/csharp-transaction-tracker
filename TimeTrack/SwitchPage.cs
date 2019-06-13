
using System;
using System.Windows.Forms;
using Interface;

namespace TimeTrack
{
    public partial class TimeTrack
    {
        public UInt16 PageIndex = 1;
        public UInt16 TrackerNumPerPage = 4;
        protected void UpButtonMouseClick(Object sender, MouseEventArgs e)
        {
            PageIndex--;
            if (PageIndex < 1)
                PageIndex = 1;

            UpdateContent();
            UpdateButtonStatus();
        }

        protected void DownButtonMouseClick(Object sender, MouseEventArgs e)
        {
            PageIndex++;
            if (PageIndex > (MaxConnection / TrackerNumPerPage))
                PageIndex = Convert.ToUInt16(MaxConnection / TrackerNumPerPage);

            var hasDeviceAfter = false;
            for (int i = ((PageIndex - 1) * TrackerNumPerPage); i < UsingDevices.Length; i++)
            {
                if(UsingDevices[i] != null)
                {
                    hasDeviceAfter = true;
                    break;
                }
            }

            if(!hasDeviceAfter)
            {
                PageIndex--;
            }
            //if (PageIndex > (ushort)Convert.ToInt16(Math.Ceiling(Convert.ToDouble(MaxConnection / TrackerNumPerPage))))
            //    PageIndex = (ushort)Convert.ToInt16(Math.Ceiling(Convert.ToDouble(MaxConnection / TrackerNumPerPage)));


            UpdateContent();
            UpdateButtonStatus();
        }

        private void UpdateContent()
        {
            //refresh trackerContainer's tracker 
            DeviceContainer.PageIndex = PageIndex;
            TrackerContainer.PageIndex = PageIndex;

            DeviceContainer.UpdateContent(UsingDevices);
            TrackerContainer.UpdateContent(UsingDevices);

            TrackerContainer.RefreshTracker = false;
            if (TrackerContainer.Count == 0) return;

            TrackerContainer.RefreshTracker = true;
            TrackerContainer.ShowRecord();
        }

        private void UpdateButtonStatus()
        {
            if (PageIndex > 1)
            {
                upButton.BackgroundImage = _upOn;
            }
            else
            {
                upButton.BackgroundImage = _up;
            }

            if (PageIndex < (MaxConnection / TrackerNumPerPage))
            {
                downButton.BackgroundImage = _downOn;
            }
            else
            {
                downButton.BackgroundImage = _down;
            }
        }
    }
}
