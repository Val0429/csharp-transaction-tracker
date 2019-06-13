using System;
using System.Windows.Forms;
using Constant;

namespace TimeTrack
{
    public partial class TimeTrack
    {
        private EventType _removeEventType = EventType.VideoRecord;
        private EventType _addEventType = EventType.VideoRecord;
        private void SearchEvent()
        {
            //no event search condition change
            if (_addEventType == EventType.VideoRecord && _removeEventType == EventType.VideoRecord) return;

            if (_addEventType != EventType.VideoRecord)
            {
                TrackerContainer.SearchEventAdd(_addEventType);
            }

            if (_removeEventType != EventType.VideoRecord)
            {
                TrackerContainer.SearchEventRemove(_removeEventType);
            }

            _addEventType = _removeEventType = EventType.VideoRecord;
        }

        private void EventButtonMouseClick(Object sender, MouseEventArgs e)
        {
            var button = sender as EventButton;
            if (button == null) return;

            if (TrackerContainer.SearchEventType.Contains(button.EventType))
            {
                _removeEventType = button.EventType;
                TrackerContainer.SearchEventType.Remove(button.EventType);
                button.BackgroundImage = button.Image;
            }
            else
            {
                _addEventType = button.EventType;
                TrackerContainer.SearchEventType.Add(button.EventType);
                button.BackgroundImage = button.ActiveImage;
            }
            SearchEvent();
        }
    }
}
