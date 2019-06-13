using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupSchedule
{
    public partial class SchedulePanel : UserControl
    {
        public IServer Server;
        public ICamera Camera;

        public Boolean IsEditing;
        private ScheduleControl _recordSchedule;
        private ScheduleControl _eventSchedule;

        public Dictionary<String, String> Localization;

        public SchedulePanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Schedule_CustomSchedule", "Custom schedule"},
                                   {"Schedule_FullTime", "Full Time"},
                                   {"Schedule_WorkingTime", "Working Hours"},
                                   {"Schedule_WorkingDay", "Working Day"},
                                   {"Schedule_NonWorkingTime", "Non-working Hours"},
                                   {"Schedule_NonWorkingDay", "Non-working Day"},
                                   {"Schedule_NoSchedule", "No schedule"},
                                    
                                   {"SetupSchedule_VideoRecording", "Video Recording"},
                                   {"SetupSchedule_EventHandling", "Event Handling"},
                                   {"SetupSchedule_FillEventRecording", "Fill the blank with event recording"},
                                   {"SetupSchedule_EventRecordingDuration", "Event Recording Duration(Sec)"},
                                   {"SetupSchedule_PreEventRecording", "Pre-event Recording"},
                                   {"SetupSchedule_PostEventRecording", "Post-event Recording"},
                                  
                                   {"SetupSchedule_AddRecordingSchedule", "Add recording schedule"},
                                   {"SetupSchedule_AddEventRecordingSchedule", "Add event recording schedule"},
                                   {"SetupSchedule_RemoveRecordingSchedule", "Remove recording schedule"},
                                   {"SetupSchedule_AddEventHandlingSchedule", "Add event handling schedule"},
                                   {"SetupSchedule_RemoveEventHandlingSchedule", "Remove event handling schedule"},
                                   {"SetupSchedule_PreEventDuration", "(Duration should between %1 sec to %2 secs)"},
                                   {"SetupSchedule_PostEventDuration", "(Duration should between %1 secs to %2 hour)"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            recordLabel.Text = Localization["SetupSchedule_VideoRecording"];
            eventLabel.Text = Localization["SetupSchedule_EventHandling"];
            filledEventCheckBox.Text = Localization["SetupSchedule_FillEventRecording"];
            durationLabel.Text = Localization["SetupSchedule_EventRecordingDuration"];
            previousLabel.Text = Localization["SetupSchedule_PreEventRecording"];
            postLabel.Text = Localization["SetupSchedule_PostEventRecording"];
            label1.Text = Localization["SetupSchedule_PreEventDuration"].Replace("%1", MiniumPre.ToString()).Replace("%2", MaximumPre.ToString());
            label2.Text = Localization["SetupSchedule_PostEventDuration"].Replace("%1", MiniumPost.ToString()).Replace("%2", (MaximumProst / 3600).ToString());

            BackgroundImage = Manager.BackgroundNoBorder;

            eventPictureBox.Image = Resources.GetResources(Properties.Resources.alarm_icon, Properties.Resources.IMGAlarm_icon);
            eraserEventPictureBox.Image = Resources.GetResources(Properties.Resources.eraser_icon, Properties.Resources.IMGEraser_icon);
            eraserPictureBox.Image = Resources.GetResources(Properties.Resources.eraser_icon, Properties.Resources.IMGEraser_icon);
            eventHandlingPictureBox.Image = Resources.GetResources(Properties.Resources.event_icon, Properties.Resources.IMGEvent_icon);
            eventHandlingPictureBox.BackgroundImage = Resources.GetResources(Properties.Resources.record_bg, Properties.Resources.BGRecord_bg);
            recordPictureBox.Image = Resources.GetResources(Properties.Resources.record_icon, Properties.Resources.IMGRecord_icon);
            recordPictureBox.BackgroundImage = Resources.GetResources(Properties.Resources.record_bg, Properties.Resources.BGRecord_bg);

            eventToolPanel.BackgroundImage = Resources.GetResources(Properties.Resources.tool_bg2, Properties.Resources.BGTool_bg2);
            recordToolPanel.BackgroundImage = Resources.GetResources(Properties.Resources.tool_bg, Properties.Resources.BGTool_bg);
        }

        private const UInt16 MiniumPre = 0;
        private const UInt16 MaximumPre = 15;
        private const UInt16 MiniumPost = 30;
        private const UInt16 MaximumProst = 3600;
        public void Initialize()
        {
            _recordSchedule = new ScheduleControl
            {
                Mode = "FullRecord",
                Data = "Record",
                SchedulePanel = this,
            };
            _recordSchedule.Initialize();

            _eventSchedule = new ScheduleControl
            {
                Mode = "EventHandling",
                Data = "Event",
                SchedulePanel = this,
            };
            _eventSchedule.Initialize();

            recordSchedulePanel.Controls.Add(_recordSchedule);
            _recordSchedule.BringToFront();

            eventSchedulePanel.Controls.Add(_eventSchedule);
            _eventSchedule.BringToFront();

            _recordSchedule.OnScheduleEdit += RecordScheduleOnScheduleEdit;
            _eventSchedule.OnScheduleEdit += EventScheduleOnScheduleEdit;

            preRecordTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
            postRecordTextBox.KeyPress += KeyAccept.AcceptNumberOnly;

            SharedToolTips.SharedToolTip.SetToolTip(recordPictureBox, Localization["SetupSchedule_AddRecordingSchedule"]);
            SharedToolTips.SharedToolTip.SetToolTip(eventPictureBox, Localization["SetupSchedule_AddEventRecordingSchedule"]);
            SharedToolTips.SharedToolTip.SetToolTip(eraserPictureBox, Localization["SetupSchedule_RemoveRecordingSchedule"]);

            SharedToolTips.SharedToolTip.SetToolTip(eventHandlingPictureBox, Localization["SetupSchedule_AddEventHandlingSchedule"]);
            SharedToolTips.SharedToolTip.SetToolTip(eraserEventPictureBox, Localization["SetupSchedule_RemoveEventHandlingSchedule"]);

            quickSetRecordComboBox.Items.Add(Localization["Schedule_CustomSchedule"]);
            quickSetRecordComboBox.Items.Add(Localization["Schedule_FullTime"]);
            quickSetRecordComboBox.Items.Add(Localization["Schedule_WorkingTime"]);
            quickSetRecordComboBox.Items.Add(Localization["Schedule_WorkingDay"]);
            quickSetRecordComboBox.Items.Add(Localization["Schedule_NonWorkingTime"]);
            quickSetRecordComboBox.Items.Add(Localization["Schedule_NonWorkingDay"]);
            quickSetRecordComboBox.Items.Add(Localization["Schedule_NoSchedule"]);

            Manager.DropDownWidth(quickSetRecordComboBox);

            quickSetEventComboBox.Items.Add(Localization["Schedule_CustomSchedule"]);
            quickSetEventComboBox.Items.Add(Localization["Schedule_FullTime"]);
            quickSetEventComboBox.Items.Add(Localization["Schedule_WorkingTime"]);
            quickSetEventComboBox.Items.Add(Localization["Schedule_WorkingDay"]);
            quickSetEventComboBox.Items.Add(Localization["Schedule_NonWorkingTime"]);
            quickSetEventComboBox.Items.Add(Localization["Schedule_NonWorkingDay"]);
            quickSetEventComboBox.Items.Add(Localization["Schedule_NoSchedule"]);

            Manager.DropDownWidth(quickSetEventComboBox);

            if (Server is IVAS || Server is ICMS)
            {
                recordSchedulePanel.Visible = durationPanel.Visible = false;
            }
        }

    	protected virtual void PreRecordTextBoxTextChanged(Object sender, EventArgs e)
        {
            UInt32 duration = (preRecordTextBox.Text != "") ? Convert.ToUInt32(preRecordTextBox.Text) : 0;

            Camera.PreRecord = Convert.ToUInt32(Math.Min(Math.Max(duration, MiniumPre), MaximumPre)) * 1000;

            Server.DeviceModify(Camera);
        }

        private void PostRecordTextBoxTextChanged(Object sender, EventArgs e)
        {
            UInt32 duration = (postRecordTextBox.Text != "") ? Convert.ToUInt32(postRecordTextBox.Text) : MiniumPost;

            Camera.PostRecord = Convert.ToUInt32(Math.Min(Math.Max(duration, MiniumPost), MaximumProst)) * 1000;

            Server.DeviceModify(Camera);
        }

        private void RecordScheduleOnScheduleEdit(Object sender, EventArgs e)
        {
            ParseRecordSchedule();
        }

        private void EventScheduleOnScheduleEdit(Object sender, EventArgs e)
        {
            ParseEventSchedule();
        }
        
        private void QuickSetRecordSchedule()
        {
            switch (quickSetRecordComboBox.SelectedIndex)
            {
                case 0:
                    if (Camera.RecordSchedule.CustomSchedule != null)
                    {
                        ScheduleModes.Clone(Camera.RecordSchedule, Camera.RecordSchedule.CustomSchedule);
                    }

                    Camera.RecordSchedule.Description = ScheduleMode.CustomSchedule;
                    break;

                case 1:
                    Camera.RecordSchedule.SetDefaultSchedule(ScheduleType.Continuous);
                    Camera.RecordSchedule.Description = ScheduleMode.FullTimeRecording;
                    break;

                case 2:
                    Camera.RecordSchedule.ClearSchedule();
                    for (int i = 0; i < 5; i++)
                    {
                        Camera.RecordSchedule.AddSchedule(new ScheduleData
                        {
                            StartTime = Convert.ToUInt32(ScheduleModes.StartWorkHour + (86400 * i)),
                            EndTime = Convert.ToUInt32(ScheduleModes.EndWorkHour + (86400 * i)),
                            Type = ScheduleType.Continuous,
                        });
                    }
                    Camera.RecordSchedule.Description = ScheduleMode.WorkingTimeRecording;
                    break;

                case 3:
                    Camera.RecordSchedule.ClearSchedule();
                    Camera.RecordSchedule.AddSchedule(new ScheduleData
                    {
                        StartTime = 0,
                        EndTime = Convert.ToUInt32((86400 * 5)),
                        Type = ScheduleType.Continuous,
                    });

                    Camera.RecordSchedule.Description = ScheduleMode.WorkingDayRecording;
                    break;

                case 4:
                    Camera.RecordSchedule.ClearSchedule();
                    for (int i = 0; i < 5; i++)
                    {
                        Camera.RecordSchedule.AddSchedule(new ScheduleData
                        {
                            StartTime = Convert.ToUInt32((86400 * i)),
                            EndTime = Convert.ToUInt32(ScheduleModes.StartWorkHour + (86400 * i)),
                            Type = ScheduleType.Continuous,
                        });
                        Camera.RecordSchedule.AddSchedule(new ScheduleData
                        {
                            StartTime = Convert.ToUInt32(ScheduleModes.EndWorkHour + (86400 * i)),
                            EndTime = Convert.ToUInt32(86400 + (86400 * i)),
                            Type = ScheduleType.Continuous,
                        });
                    }
                    Camera.RecordSchedule.AddSchedule(new ScheduleData
                    {
                        StartTime = Convert.ToUInt32((86400 * 5)),
                        EndTime = Convert.ToUInt32(86400 + (86400 * 6)),
                        Type = ScheduleType.Continuous,
                    });

                    Camera.RecordSchedule.Description = ScheduleMode.NonWorkingTimeRecording;
                    break;

                case 5:
                    Camera.RecordSchedule.ClearSchedule();
                    Camera.RecordSchedule.AddSchedule(new ScheduleData
                    {
                        StartTime = Convert.ToUInt32((86400 * 5)),
                        EndTime = Convert.ToUInt32(86400 + (86400 * 6)),
                        Type = ScheduleType.Continuous,
                    });

                    Camera.RecordSchedule.Description = ScheduleMode.NonWorkingDayRecording;
                    break;

                case 6:
                    Camera.RecordSchedule.ClearSchedule();

                    Camera.RecordSchedule.Description = ScheduleMode.NoSchedule;
                    break;
            }
        }

        private void FilledEventCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            QuickSetRecordSchedule();

            if (Camera.RecordSchedule.Description == ScheduleMode.CustomSchedule)
                filledEventCheckBox.Enabled = true;

            FilledEventToSchedule();

            Server.DeviceModify(Camera);
            _recordSchedule.Invalidate();
        }

        private void QuickSetRecordComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            QuickSetRecordSchedule();

            if (Camera.RecordSchedule.Description == ScheduleMode.CustomSchedule)
            {
                filledEventCheckBox.Enabled = true;

                filledEventCheckBox.CheckedChanged -= FilledEventCheckBoxCheckedChanged;

                if (ScheduleModes.CheckMode(Camera.RecordSchedule) == ScheduleMode.FullTimeRecording)
                {
                    filledEventCheckBox.Enabled = false;
                    filledEventCheckBox.Checked = false;
                }
                else
                {
                    filledEventCheckBox.Enabled = true;
                    filledEventCheckBox.Checked = !ScheduleModes.HasEmptySchedule(Camera.RecordSchedule);
                }

                filledEventCheckBox.CheckedChanged += FilledEventCheckBoxCheckedChanged;
            }
            else
                FilledEventToSchedule();

            Server.DeviceModify(Camera);
            _recordSchedule.Invalidate();
        }

        private void FilledEventToSchedule()
        {
            Boolean filledWithEvent = filledEventCheckBox.Checked;

            //fill event
            if (filledWithEvent)
            {
                if (Camera.RecordSchedule.Count == 0)
                    Camera.RecordSchedule.SetDefaultSchedule(ScheduleType.EventRecording);
                else
                {
                    var temp = new List<ScheduleData>(Camera.RecordSchedule);
                    for (var i = 0; i < temp.Count; i++)
                    {
                        //first one
                        if (i == 0 && temp[i].StartTime != 0)
                        {
                            Camera.RecordSchedule.AddSchedule(new ScheduleData
                            {
                                StartTime = 0,
                                EndTime = temp[i].StartTime,
                                Type = ScheduleType.EventRecording,
                            });
                        }

                        //last one
                        if (i == (temp.Count - 1))
                        {
                            if (temp[i].EndTime != 86400 * 7)
                            {
                                Camera.RecordSchedule.AddSchedule(new ScheduleData
                                {
                                    StartTime = temp[i].EndTime,
                                    EndTime = 86400 * 7,
                                    Type = ScheduleType.EventRecording,
                                });
                            }
                            break;
                        }

                        //continues record, no matter it's record or event
                        if (temp[i].EndTime == temp[i + 1].StartTime) continue;

                        Camera.RecordSchedule.AddSchedule(new ScheduleData
                        {
                            StartTime = temp[i].EndTime,
                            EndTime = temp[i + 1].StartTime,
                            Type = ScheduleType.EventRecording,
                        });
                    }
                }
            }
            else //clear event schedule
            {
                var temp = new List<ScheduleData>(Camera.RecordSchedule);

                foreach (var scheduleDate in temp)
                {
                    if (scheduleDate.Type == ScheduleType.EventRecording)
                        Camera.RecordSchedule.RemoveSchedule(scheduleDate);
                }
            }

            Camera.RecordSchedule.Description = ScheduleModes.CheckMode(Camera.RecordSchedule);
            ParseRecordSchedule();
        }

        private void QuickSetEventComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            switch (quickSetEventComboBox.SelectedIndex)
            {
                case 0:
                    if (Camera.EventSchedule.CustomSchedule != null)
                    {
                        ScheduleModes.Clone(Camera.EventSchedule, Camera.EventSchedule.CustomSchedule);
                    }

                    Camera.EventSchedule.Description = ScheduleMode.CustomSchedule;
                    break;

                case 1:
                    Camera.EventSchedule.SetDefaultSchedule(ScheduleType.EventHandlering);
                    Camera.EventSchedule.Description = ScheduleMode.FullTimeEventHandling;
                    break;

                case 2:
                    Camera.EventSchedule.ClearSchedule();
                    for (int i = 0; i < 5; i++)
                    {
                        Camera.EventSchedule.AddSchedule(new ScheduleData
                        {
                            StartTime = Convert.ToUInt32(ScheduleModes.StartWorkHour + (86400 * i)),
                            EndTime = Convert.ToUInt32(ScheduleModes.EndWorkHour + (86400 * i)),
                            Type = ScheduleType.EventHandlering,
                        });
                    }
                    Camera.EventSchedule.Description = ScheduleMode.WorkingTimeEventHandling;
                    break;

                case 3:
                    Camera.EventSchedule.ClearSchedule();
                    Camera.EventSchedule.AddSchedule(new ScheduleData
                    {
                        StartTime = 0,
                        EndTime = Convert.ToUInt32((86400 * 5)),
                        Type = ScheduleType.EventHandlering,
                    });
                    Camera.EventSchedule.Description = ScheduleMode.WorkingDayEventHandling;
                    break;
                    
                case 4:
                    Camera.EventSchedule.ClearSchedule();
                    for (int i = 0; i < 5; i++)
                    {
                        Camera.EventSchedule.AddSchedule(new ScheduleData
                        {
                            StartTime = Convert.ToUInt32((86400 * i)),
                            EndTime = Convert.ToUInt32(ScheduleModes.StartWorkHour + (86400 * i)),
                            Type = ScheduleType.EventHandlering,
                        });
                        Camera.EventSchedule.AddSchedule(new ScheduleData
                        {
                            StartTime = Convert.ToUInt32(ScheduleModes.EndWorkHour + (86400 * i)),
                            EndTime = Convert.ToUInt32(86400 + (86400 * i)),
                            Type = ScheduleType.EventHandlering,
                        });
                    }
                    Camera.EventSchedule.AddSchedule(new ScheduleData
                    {
                        StartTime = Convert.ToUInt32((86400 * 5)),
                        EndTime = Convert.ToUInt32(86400 + (86400 * 6)),
                        Type = ScheduleType.EventHandlering,
                    });
                    Camera.EventSchedule.Description = ScheduleMode.NonWorkingTimeEventHandling;
                    break;

                case 5:
                    Camera.EventSchedule.ClearSchedule();
                    Camera.EventSchedule.AddSchedule(new ScheduleData
                    {
                        StartTime = Convert.ToUInt32((86400 * 5)),
                        EndTime = Convert.ToUInt32((86400 * 7)),
                        Type = ScheduleType.EventHandlering,
                    });
                    Camera.EventSchedule.Description = ScheduleMode.NonWorkingDayEventHandling;
                    break;

                case 6:
                    Camera.EventSchedule.ClearSchedule();
                    Camera.EventSchedule.Description = ScheduleMode.NoSchedule;
                    break;
            }

            if (Camera.EventHandling != null)
                Camera.EventHandling.ReadyState = ReadyState.Modify;

            Server.DeviceModify(Camera);
            _eventSchedule.Invalidate();
        }

        public void ParseDevice()
        {
            if (Server is IVAS)
            {
                ParseEventSchedule();
            }
            else
            {
                preRecordTextBox.Text = (Camera.PreRecord / 1000).ToString();
                postRecordTextBox.Text = (Camera.PostRecord / 1000).ToString();
                ParseRecordSchedule();
                ParseEventSchedule();
            }
        }

        private void ParseRecordSchedule()
        {
            if (Camera.RecordSchedule == null) return;

            filledEventCheckBox.CheckedChanged -= FilledEventCheckBoxCheckedChanged;
            quickSetRecordComboBox.SelectedIndexChanged -= QuickSetRecordComboBoxSelectedIndexChanged;
            preRecordTextBox.TextChanged -= PreRecordTextBoxTextChanged;
            postRecordTextBox.TextChanged -= PostRecordTextBoxTextChanged;

            filledEventCheckBox.Enabled = true;
            switch (Camera.RecordSchedule.Description)
            {
                case ScheduleMode.FullTimeRecording:
                    quickSetRecordComboBox.SelectedIndex = 1;
                    filledEventCheckBox.Enabled = false;
                    break;

                case ScheduleMode.WorkingTimeRecording:
                case ScheduleMode.WorkingTimeRecordingWithEventRecording:
                    quickSetRecordComboBox.SelectedIndex = 2;
                    break;

                case ScheduleMode.WorkingDayRecording:
                case ScheduleMode.WorkingDayRecordingWithEventRecording:
                    quickSetRecordComboBox.SelectedIndex = 3;
                    break;

                case ScheduleMode.NonWorkingTimeRecording:
                case ScheduleMode.NonWorkingTimeRecordingWithEventRecording:
                    quickSetRecordComboBox.SelectedIndex = 4;
                    break;

                case ScheduleMode.NonWorkingDayRecording:
                case ScheduleMode.NonWorkingDayRecordingWithEventRecording:
                    quickSetRecordComboBox.SelectedIndex = 5;
                    break;

                case ScheduleMode.NoSchedule:
                case ScheduleMode.FullTimeEventRecording:
                    quickSetRecordComboBox.SelectedIndex = 6;
                    break;

                default:
                    if (Camera.RecordSchedule.CustomSchedule == null)
                    {
                        Camera.RecordSchedule.CustomSchedule = new List<ScheduleData>();
                    }

                    ScheduleModes.Clone(Camera.RecordSchedule.CustomSchedule, Camera.RecordSchedule);
                    quickSetRecordComboBox.SelectedIndex = 0;
                    break;
            }
            filledEventCheckBox.Checked = (!ScheduleModes.HasEmptySchedule(Camera.RecordSchedule) && filledEventCheckBox.Enabled);

            filledEventCheckBox.CheckedChanged += FilledEventCheckBoxCheckedChanged;
            quickSetRecordComboBox.SelectedIndexChanged += QuickSetRecordComboBoxSelectedIndexChanged;
            preRecordTextBox.TextChanged += PreRecordTextBoxTextChanged;
            postRecordTextBox.TextChanged += PostRecordTextBoxTextChanged;
        }

        private void ParseEventSchedule()
        {
            if (Camera.EventSchedule == null) return;

            quickSetEventComboBox.SelectedIndexChanged -= QuickSetEventComboBoxSelectedIndexChanged;

            switch (Camera.EventSchedule.Description)
            {
                case ScheduleMode.FullTimeEventHandling:
                    quickSetEventComboBox.SelectedIndex = 1;
                    break;

                case ScheduleMode.WorkingTimeEventHandling:
                    quickSetEventComboBox.SelectedIndex = 2;
                    break;

                case ScheduleMode.WorkingDayEventHandling:
                    quickSetEventComboBox.SelectedIndex = 3;
                    break;

                case ScheduleMode.NonWorkingTimeEventHandling:
                    quickSetEventComboBox.SelectedIndex = 4;
                    break;

                case ScheduleMode.NonWorkingDayEventHandling:
                    quickSetEventComboBox.SelectedIndex = 5;
                    break;
                case ScheduleMode.NoSchedule:
                    quickSetEventComboBox.SelectedIndex = 6;
                    break;

                default:
                    if (Camera.EventSchedule.CustomSchedule == null)
                    {
                        Camera.EventSchedule.CustomSchedule = new List<ScheduleData>();
                    }

                    ScheduleModes.Clone(Camera.EventSchedule.CustomSchedule, Camera.EventSchedule);
                    quickSetEventComboBox.SelectedIndex = 0;
                    break;
            }
            quickSetEventComboBox.SelectedIndexChanged += QuickSetEventComboBoxSelectedIndexChanged;
        }

        private void RecordPictureBoxMouseClick(Object sender, MouseEventArgs e)
        {
            recordPictureBox.BackgroundImage = _recordbg;
            eventPictureBox.BackgroundImage = null;
            eraserPictureBox.BackgroundImage = null;

            _recordSchedule.Mode = "FullRecord";
        }

        private static readonly Image _eventbg = Resources.GetResources(Properties.Resources.event_bg, Properties.Resources.BGEvent_bg);
        private void EventPictureBoxMouseClick(Object sender, MouseEventArgs e)
        {
            recordPictureBox.BackgroundImage = null;
            eventPictureBox.BackgroundImage = _eventbg;
            eraserPictureBox.BackgroundImage = null;

            _recordSchedule.Mode = "EventRecord";
        }

        private void EraserPictureBoxMouseClick(Object sender, MouseEventArgs e)
        {
            recordPictureBox.BackgroundImage = null;
            eventPictureBox.BackgroundImage = null;
            eraserPictureBox.BackgroundImage = _eraserbg;
            
            _recordSchedule.Mode = "Eraser";
        }

        private static readonly Image _recordbg = Resources.GetResources(Properties.Resources.record_bg, Properties.Resources.BGRecord_bg);
        private void EventHandlingPictureBoxMouseClick(Object sender, MouseEventArgs e)
        {
            eventHandlingPictureBox.BackgroundImage = _recordbg;
            eraserEventPictureBox.BackgroundImage = null;

            _eventSchedule.Mode = "EventHandling";
        }

        private static readonly Image _eraserbg = Resources.GetResources(Properties.Resources.eraser_bg, Properties.Resources.BGEraser_bg);
        private void EraserEventPictureBoxMouseClick(Object sender, MouseEventArgs e)
        {
            eventHandlingPictureBox.BackgroundImage = null;
            eraserEventPictureBox.BackgroundImage = _eraserbg;

            _eventSchedule.Mode = "Eraser";
        }
    }
}
