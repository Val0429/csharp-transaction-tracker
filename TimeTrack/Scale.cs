using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;

namespace TimeTrack
{
    public partial class TimeTrack
    {
        private void ScaleButtonMouseDown(Object sender, MouseEventArgs e)
        {
            scaleButton.BackgroundImage = _scalePointselect;

            scalePanel.MouseMove -= ScalePanelMouseMove;
            scalePanel.MouseMove += ScalePanelMouseMove;

            scalePanel.MouseUp -= ScalePanelMouseUp;
            scalePanel.MouseUp += ScalePanelMouseUp;

            scalePanel.Capture = true;
        }

        private Int32 _lastMovePosition;
        private const UInt16 PointWidth= 11;
        private const UInt16 ScaleDiff = 13;
        private void ScalePanelMouseMove(Object sender, MouseEventArgs e)
        {
            if (_lastMovePosition == e.X) return;

            Int32 newX = Math.Min(Math.Max(ScalePosition1, e.X - PointWidth), ScalePosition6);

            if (newX <= ScalePosition1 + ScaleDiff)
                scaleButton.Location = new Point(ScalePosition1, scaleButton.Location.Y);
            else if (newX > ScalePosition2 - ScaleDiff && newX <= ScalePosition2 + ScaleDiff)
                scaleButton.Location = new Point(ScalePosition2, scaleButton.Location.Y);
            else if (newX > ScalePosition3 - ScaleDiff && newX <= ScalePosition3 + ScaleDiff)
                scaleButton.Location = new Point(ScalePosition3, scaleButton.Location.Y);
            else if (newX > ScalePosition4 - ScaleDiff && newX <= ScalePosition4 + ScaleDiff)
                scaleButton.Location = new Point(ScalePosition4, scaleButton.Location.Y);
            else if (newX > ScalePosition5 - ScaleDiff && newX <= ScalePosition5 + ScaleDiff)
                scaleButton.Location = new Point(ScalePosition5, scaleButton.Location.Y);
            else if (newX > ScalePosition6 - ScaleDiff)
                scaleButton.Location = new Point(ScalePosition6, scaleButton.Location.Y);

            _lastMovePosition = e.X;
        }

        protected void PlusButtonMouseClick(Object sender, MouseEventArgs e)
        {
            if (ScaleList.IndexOf(_currentlyScale) + 1 < ScaleList.Count)
                CurrentlyScale = ScaleList[ScaleList.IndexOf(_currentlyScale) + 1];
        }

        protected void MinusButtonMouseClick(Object sender, MouseEventArgs e)
        {
            if (ScaleList.IndexOf(_currentlyScale) > 0)
                CurrentlyScale = ScaleList[ScaleList.IndexOf(_currentlyScale) - 1];
        }

        private static readonly Image _scalePoint = Resources.GetResources(Properties.Resources.scalePoint, Properties.Resources.IMGScalePoint);
        private static readonly Image _scalePointselect = Resources.GetResources(Properties.Resources.scalePoint_select, Properties.Resources.IMGScalePoint_select);
        private void ScalePanelMouseUp(Object sender, MouseEventArgs e)
        {
            scalePanel.MouseMove -= ScalePanelMouseMove;
            scalePanel.MouseUp -= ScalePanelMouseUp;
            scaleButton.BackgroundImage = _scalePoint;

            switch (scaleButton.Location.X)
            {
                case ScalePosition1:
                    CurrentlyScale = TimeScale.Scale1Second;
                    break;

                case ScalePosition2:
                    CurrentlyScale = TimeScale.Scale10Seconds;
                    break;

                case ScalePosition3:
                    CurrentlyScale = TimeScale.Scale1Minute;
                    break;

                case ScalePosition4:
                    CurrentlyScale = TimeScale.Scale10Minutes;
                    break;

                case ScalePosition5:
                    CurrentlyScale = TimeScale.Scale1Hour;
                    break;

                //case ScalePosition6:
                //    CurrentlyScale = TimeScale.Scale4Hours;
                //    break;

                case ScalePosition6:
                    CurrentlyScale = TimeScale.Scale1Day;
                    break;
            }
        }

        protected void ScaleButtonLocationChanged(Object sender, EventArgs e)
        {
            switch (scaleButton.Location.X)
            {
                case ScalePosition1:
                    scaleLabel.Text = 1 + @" " + Localization["Common_Sec"];
                    break;

                case ScalePosition2:
                    scaleLabel.Text = 10 + @" " + Localization["Common_Sec"];
                    break;

                case ScalePosition3:
                    scaleLabel.Text = 1 + @" " + Localization["Common_Min"];
                    break;

                case ScalePosition4:
                    scaleLabel.Text = 10 + @" " + Localization["Common_Min"];
                    break;

                case ScalePosition5:
                    scaleLabel.Text = 1 + @" " + Localization["Common_Hr"];
                    break;

                case ScalePosition6:
                    scaleLabel.Text = 1 + @" " + Localization["Common_Day"];
                    break;
            }
        }
    }
}
