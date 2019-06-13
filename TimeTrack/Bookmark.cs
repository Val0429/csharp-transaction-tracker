
using System;
using System.Windows.Forms;
using PanelBase;

namespace TimeTrack
{
    public partial class TimeTrack
    {
        protected void BookmarkButtonMouseClick(Object sender, MouseEventArgs e)
        {
            TrackerContainer.AddBookmark();
        }

        protected void EraserBookmarkButtonMouseClick(Object sender, MouseEventArgs e)
        {
            DialogResult result = TopMostMessageBox.Show(Localization["TimeTrack_DeleteBookmarkConfirm"], Localization["MessageBox_Confirm"],
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                TrackerContainer.EraserBookmark();
        }

        protected void NextBookmarkButtonMouseDown(Object sender, MouseEventArgs e)
        {
            Int64 nextBookmarkTicks = TrackerContainer.NextBookmark();
            Stop();
            //if (Rate < 0)
            //    Stop();

            if (nextBookmarkTicks != DateTime.MaxValue.Ticks)
                TrackerContainer.DateTime = new DateTime(nextBookmarkTicks);
            else
                TopMostMessageBox.Show(Localization["TimeTrack_NoMoreBookmarks"], Localization["MessageBox_Information"],
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected void PreviousBookmarkButtonMouseDown(Object sender, MouseEventArgs e)
        {
            Int64 previousBookmarkTicks = TrackerContainer.PreviousBookmark();
            Stop();
            //if (Rate > 0)
            //    Stop();

            if (previousBookmarkTicks != DateTime.MinValue.Ticks)
                TrackerContainer.DateTime = new DateTime(previousBookmarkTicks);
            else
                TopMostMessageBox.Show(Localization["TimeTrack_NoMoreBookmarks"], Localization["MessageBox_Information"],
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
