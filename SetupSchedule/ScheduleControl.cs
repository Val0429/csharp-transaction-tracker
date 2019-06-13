using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using PanelBase;

namespace SetupSchedule
{
    public sealed partial class ScheduleControl : UserControl
    {
        public event EventHandler OnScheduleEdit;

        public String Data;
        public String Mode;

        private ScheduleDayControl _monScheduleDayControl;
        private ScheduleDayControl _tueScheduleDayControl;
        private ScheduleDayControl _wedScheduleDayControl;
        private ScheduleDayControl _thuScheduleDayControl;
        private ScheduleDayControl _friScheduleDayControl;
        private ScheduleDayControl _satScheduleDayControl;
        private ScheduleDayControl _sunScheduleDayControl;

        public Dictionary<String, String> Localization;
        private readonly List<ScheduleDayControl> _dayLists = new List<ScheduleDayControl>();
        public SchedulePanel SchedulePanel;
        public ScheduleControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Schedule_Mon", "Mon"},
                                   {"Schedule_Tue", "Tue"},
                                   {"Schedule_Wed", "Wed"},
                                   {"Schedule_Thu", "Thu"},
                                   {"Schedule_Fri", "Fri"},
                                   {"Schedule_Sat", "Sat"},
                                   {"Schedule_Sun", "Sun"},
                               };
            Localizations.Update(Localization);

            MinimumSize = new Size(120, 30);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Top;
        }

        public void Initialize()
        {
            _monScheduleDayControl = new ScheduleDayControl
            {
                Day = Localization["Schedule_Mon"],
                StartTime = 0,
                EndTime = 86400,
                ScheduleControl = this,
            };

            _tueScheduleDayControl = new ScheduleDayControl
            {
                Day = Localization["Schedule_Tue"],
                StartTime = 86400,
                EndTime = 172800,
                ScheduleControl = this,
            };

            _wedScheduleDayControl = new ScheduleDayControl
            {
                Day = Localization["Schedule_Wed"],
                StartTime = 172800,
                EndTime = 259200,
                ScheduleControl = this,
            };

            _thuScheduleDayControl = new ScheduleDayControl
            {
                Day = Localization["Schedule_Thu"],
                StartTime = 259200,
                EndTime = 345600,
                ScheduleControl = this,
            };

            _friScheduleDayControl = new ScheduleDayControl
            {
                Day = Localization["Schedule_Fri"],
                StartTime = 345600,
                EndTime = 432000,
                ScheduleControl = this,
            };

            _satScheduleDayControl = new ScheduleDayControl
            {
                Day = Localization["Schedule_Sat"],
                StartTime = 432000,
                EndTime = 518400,
                ScheduleControl = this,
            };

            _sunScheduleDayControl = new ScheduleDayControl
            {
                Day = Localization["Schedule_Sun"],
                StartTime = 518400,
                EndTime = 604800,
                ScheduleControl = this,
            };

            _dayLists.Add(_monScheduleDayControl);
            _dayLists.Add(_tueScheduleDayControl);
            _dayLists.Add(_wedScheduleDayControl);
            _dayLists.Add(_thuScheduleDayControl);
            _dayLists.Add(_friScheduleDayControl);
            _dayLists.Add(_satScheduleDayControl);
            _dayLists.Add(_sunScheduleDayControl);

            Controls.Add(_sunScheduleDayControl);
            Controls.Add(_satScheduleDayControl);
            Controls.Add(_friScheduleDayControl);
            Controls.Add(_thuScheduleDayControl);
            Controls.Add(_wedScheduleDayControl);
            Controls.Add(_tueScheduleDayControl);
            Controls.Add(_monScheduleDayControl);
            Controls.Add(titlePanel);

            Paint += ScheduleControlPaint;
            titlePanel.Paint += TitlePanelPaint;

            MouseMove += ScheduleControlMouseMove;
            MouseUp += ScheduleControlMouseUp;
        }

        public ScheduleDayControl SelectDay;
        public UInt32 SelectStartTime;
        public UInt16 SelectStartX;
        public UInt16 SelectStartY;
        private void ScheduleControlMouseUp(Object sender, MouseEventArgs e)
        {
            Boolean isChange = false;
            Schedule schedule = (Data == "Record")
                ? SchedulePanel.Camera.RecordSchedule
                : SchedulePanel.Camera.EventSchedule;

            foreach (ScheduleDayControl scheduleDayControl in _dayLists)
            {
                if (!scheduleDayControl.IsSelected) continue;

                switch (Mode)
                {
                    case "FullRecord":
                        schedule.AddSchedule(new ScheduleData
                        {
                            StartTime = scheduleDayControl.SelectStart,
                            EndTime = scheduleDayControl.SelectEnd,
                            Type = ScheduleType.Continuous,
                        });
                        break;

                    case "EventRecord":
                        schedule.AddSchedule(new ScheduleData
                        {
                            StartTime = scheduleDayControl.SelectStart,
                            EndTime = scheduleDayControl.SelectEnd,
                            Type = ScheduleType.EventRecording,
                        });
                        break;

                    case "EventHandling":
                        schedule.AddSchedule(new ScheduleData
                        {
                            StartTime = scheduleDayControl.SelectStart,
                            EndTime = scheduleDayControl.SelectEnd,
                            Type = ScheduleType.EventHandlering,
                        });
                        break;

                    case "Eraser":
                        schedule.RemoveSchedule(new ScheduleData
                        {
                            StartTime = scheduleDayControl.SelectStart,
                            EndTime = scheduleDayControl.SelectEnd,
                        });
                        break;
                }

                //Console.WriteLine(scheduleDayControl.SelectStart + " " + scheduleDayControl.SelectEnd);

                isChange = true;
                scheduleDayControl.IsSelected = false;
                scheduleDayControl.Invalidate();
            }

            if (isChange && schedule != null)
            {
                if (Data == "Record")
                {
                    SchedulePanel.Camera.RecordSchedule.Description = ScheduleModes.CheckMode(schedule);

                    if (SchedulePanel.Camera.RecordSchedule.CustomSchedule == null)
                    {
                        SchedulePanel.Camera.RecordSchedule.CustomSchedule = new List<ScheduleData>();
                    }
                    ScheduleModes.Clone(SchedulePanel.Camera.RecordSchedule.CustomSchedule, SchedulePanel.Camera.RecordSchedule);
                }
                else
                {
                    SchedulePanel.Camera.EventSchedule.Description = ScheduleModes.CheckMode(schedule);

                    if (SchedulePanel.Camera.EventSchedule.CustomSchedule == null)
                    {
                        SchedulePanel.Camera.EventSchedule.CustomSchedule = new List<ScheduleData>();
                    }
                    ScheduleModes.Clone(SchedulePanel.Camera.EventSchedule.CustomSchedule, SchedulePanel.Camera.EventSchedule);

                    if (SchedulePanel.Camera.EventHandling != null)
                        SchedulePanel.Camera.EventHandling.ReadyState = ReadyState.Modify;
                }

                SchedulePanel.Server.DeviceModify(SchedulePanel.Camera);

                if (OnScheduleEdit != null)
                    OnScheduleEdit(this, null);
            }

            SelectDay.Cursor = Cursors.Default;
            Capture = false;
        }

        private const UInt16 DayHeight = 30;
        private void ScheduleControlMouseMove(Object sender, MouseEventArgs e)
        {
            if (SelectDay == null) return;

            UInt32 endTime = (e.X > SelectStartX)
                                 ? SelectDay.AsEnd(SelectDay.ScheduleTime(e.X))
                                 : SelectDay.AsStart(SelectDay.ScheduleTime(e.X));

            if (e.Y > SelectStartY)
            {
                Boolean start = false;
                Boolean done = false;
                foreach (ScheduleDayControl scheduleDayControl in _dayLists)
                {
                    if (!start && scheduleDayControl == SelectDay) start = true;
                    if (!start)
                    {
                        scheduleDayControl.IsSelected = false;
                        scheduleDayControl.Invalidate();
                        continue;
                    }

                    if (!done && SelectStartY <= scheduleDayControl.Location.Y + DayHeight && e.Y >= scheduleDayControl.Location.Y)
                    {
                        scheduleDayControl.IsSelected = true;
                        scheduleDayControl.SelectionRange(SelectStartTime, endTime);
                    }
                    else
                    {
                        done = true;
                        scheduleDayControl.IsSelected = false;
                        scheduleDayControl.Invalidate();
                    }
                }
            }
            else
            {
                Boolean done = false;
                foreach (ScheduleDayControl scheduleDayControl in _dayLists)
                {
                    if (done)
                    {
                        scheduleDayControl.IsSelected = false;
                        scheduleDayControl.Invalidate();
                        continue;
                    }
                    if (SelectStartY >= scheduleDayControl.Location.Y && e.Y <= scheduleDayControl.Location.Y + DayHeight)
                    {
                        scheduleDayControl.IsSelected = true;
                        scheduleDayControl.SelectionRange(SelectStartTime, endTime);
                    }
                    else
                    {
                        scheduleDayControl.IsSelected = false;
                        scheduleDayControl.Invalidate();
                    }
                    if (scheduleDayControl == SelectDay)
                    {
                        done = true;
                    }
                }
            }

        }

        public const UInt16 StartPosition = 80;

        private static readonly Image _topLeft = Resources.GetResources(Properties.Resources.sh_top_left, Properties.Resources.BGSh_top_left);
        private static readonly Image _topRight = Resources.GetResources(Properties.Resources.sh_top_right, Properties.Resources.BGSh_top_right);
        private static readonly Image _bgInput = Resources.GetResources(Properties.Resources.inputBG, Properties.Resources.BGInputBG);

        public UInt16 Diff;
        public UInt32 ScheduleWidth;
        private void ScheduleControlPaint(Object sender, PaintEventArgs e)
        {
            Int32 diff = (Width - StartPosition - 15) / 24;

            Diff = Convert.ToUInt16(diff / 3 * 3);

            ScheduleWidth = Convert.ToUInt32(StartPosition + Diff * 24 + 15);
        }

        private void TitlePanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawImage(_bgInput, _topLeft.Width - 2, 0, ScheduleWidth - _topLeft.Width - _topRight.Width + 4, titlePanel.Height);

            g.DrawImage(_topLeft, 0, 0, _topLeft.Width, titlePanel.Height);

            g.DrawImage(_topRight, ScheduleWidth - _topRight.Width, 0, _topRight.Width, titlePanel.Height);

            if (ScheduleWidth < 400) return;

            Int32 start = StartPosition;
            for (UInt16 hour = 0; hour <= 24; hour++)
            {
                SizeF fSize = g.MeasureString((hour % 24).ToString(), Manager.Font);
                g.DrawString((hour % 24).ToString(), Manager.Font, Brushes.Black, start - (fSize.Width) / 2, 12);

                start += Diff;
            }
        }
    }
}
