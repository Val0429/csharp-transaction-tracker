using System;
using System.Collections.Generic;
using System.Linq;

namespace Constant
{
    public class Schedule : List<ScheduleData>
    {
        public List<ScheduleData> CustomSchedule;

        public ScheduleMode Description = ScheduleMode.CustomSchedule;

        public void SetDefaultSchedule(ScheduleType type)
        {
            ClearSchedule();
            Add(new ScheduleData
            {
                StartTime = 0,
                EndTime = 86400 * 7,
                Type = type,
            });

            //if (CustomSchedule == null)
            //{
            //    CustomSchedule = new List<ScheduleData>();
            //    ScheduleModes.Clone(CustomSchedule, this);
            //}
        }

        public void RemoveSchedule(ScheduleData data)
        {
            if (data.StartTime == data.EndTime) return;

            if(Count == 0) return;

            Boolean isOverlap = false;
            foreach (ScheduleData scheduleData in this)
            {
                if (scheduleData.EndTime >= data.StartTime && scheduleData.StartTime <= data.EndTime)
                {
                    isOverlap = true;
                    break;
                }
            }
            
            if (!isOverlap) return;

            var removeContainsData = new List<ScheduleData>();
            foreach (ScheduleData scheduleData in this)
            {
                if (scheduleData.EndTime <= data.EndTime && scheduleData.StartTime >= data.StartTime)
                    removeContainsData.Add(scheduleData);
            }

            foreach (ScheduleData scheduleData in removeContainsData)
            {
                Remove(scheduleData);
            }

            removeContainsData.Clear();

            foreach (ScheduleData scheduleData in this)
            {
                if (scheduleData.EndTime <= data.EndTime && scheduleData.EndTime >= data.StartTime)
                {
                    scheduleData.EndTime = data.StartTime;
                    break;
                }
            }

            foreach (ScheduleData scheduleData in this)
            {
                if (scheduleData.StartTime <= data.EndTime && scheduleData.StartTime >= data.StartTime)
                {
                    scheduleData.StartTime = data.EndTime;
                    break;
                }
            }

            foreach (ScheduleData scheduleData in this)
            {
                if (scheduleData.EndTime >= data.EndTime && scheduleData.StartTime <= data.StartTime)
                {
                    Add(new ScheduleData
                    {
                        StartTime = data.EndTime,
                        EndTime = scheduleData.EndTime,
                        Type = scheduleData.Type,
                    });

                    scheduleData.EndTime = data.StartTime;
                    break;
                }
            }

            Sort((x, y) => x.StartTime.CompareTo(y.StartTime));
        }

        public void AddSchedule(ScheduleData data)
        {
            if(data.StartTime == data.EndTime) return;

            if(Count == 0){
                Add(data);
                return;
            }

            Boolean isOverlap = false;
            foreach (ScheduleData scheduleData in this)
            {
                if (scheduleData.EndTime >= data.StartTime && scheduleData.StartTime <= data.EndTime)
                {
                    isOverlap = true;
                    break;
                }
            }

            if (!isOverlap)
            {
                Add(data);
                Sort((x, y) => x.StartTime.CompareTo(y.StartTime));
                return;
            }

            foreach (ScheduleData scheduleData in this)
            {
                if (scheduleData.EndTime >= data.EndTime && scheduleData.StartTime <= data.StartTime)
                {
                    if (scheduleData.Type == data.Type)
                        return;

                    Add(new ScheduleData
                    {
                        StartTime = data.EndTime,
                        EndTime = scheduleData.EndTime,
                        Type = scheduleData.Type,
                    });

                    scheduleData.EndTime = data.StartTime;
                    break;
                }
            }

            List<ScheduleData> removeContainsData = new List<ScheduleData>();
            foreach (ScheduleData scheduleData in this)
            {
                if (scheduleData.EndTime <= data.EndTime && scheduleData.StartTime >= data.StartTime)
                    removeContainsData.Add(scheduleData);
            }

            foreach (ScheduleData scheduleData in removeContainsData)
            {
                Remove(scheduleData);
            }

            removeContainsData.Clear();

            ScheduleData mergeStart = null;
            ScheduleData mergeEnd = null;

            foreach (ScheduleData scheduleData in this)
            {
                if (scheduleData.EndTime <= data.EndTime && scheduleData.EndTime >= data.StartTime)
                {
                    mergeStart = scheduleData;
                    break;
                }
            }

            foreach (ScheduleData scheduleData in this)
            {
                if (scheduleData.StartTime <= data.EndTime && scheduleData.StartTime >= data.StartTime)
                {
                    mergeEnd = scheduleData;
                    break;
                }
            }

            if (mergeStart != null)
            {
                if (mergeStart.Type == data.Type)
                {
                    Remove(mergeStart);
                    data.StartTime = mergeStart.StartTime;
                }
                else
                {
                    mergeStart.EndTime = data.StartTime;
                }
            }

            if (mergeEnd != null)
            {
                if (mergeEnd.Type == data.Type)
                {
                    Remove(mergeEnd);
                    data.EndTime = mergeEnd.EndTime;
                }
                else
                {
                    mergeEnd.StartTime = data.EndTime;
                }
            }

            Add(data);

            Sort((x, y) => x.StartTime.CompareTo(y.StartTime));
        }

        public Boolean CheckSchedule(UInt32 time)
        {
            return this.Any(scheduleData => scheduleData.StartTime <= time && scheduleData.EndTime >= time);
        }

        public void ClearSchedule()
        {
            Clear();
        }
    }

    public class ScheduleData
    {
        public ScheduleData()
        {
            
        }


        public UInt32 StartTime { get; set; } //Second
        public UInt32 EndTime { get; set; } //Second
        public ScheduleType Type = ScheduleType.Continuous;
    }

    public enum ScheduleType : ushort
    {
        Continuous = 1,
        EventRecording = 2,
        EventHandlering = 3,
    }

    public enum ScheduleMode : ushort
    {
        CustomSchedule = 0,

        FullTimeEventHandling = 1,
        WorkingTimeEventHandling = 2,
        WorkingDayEventHandling = 3,
        NonWorkingTimeEventHandling = 4,
        NonWorkingDayEventHandling = 5,
        NoSchedule = 6,

        FullTimeRecording = 7,
        WorkingTimeRecording = 8,
        WorkingDayRecording = 9,
        NonWorkingTimeRecording = 10,
        NonWorkingDayRecording = 11,

        FullTimeEventRecording = 12,
        WorkingTimeRecordingWithEventRecording = 13,
        WorkingDayRecordingWithEventRecording = 14,
        NonWorkingTimeRecordingWithEventRecording = 15,
        NonWorkingDayRecordingWithEventRecording = 16,
    }

    public static class ScheduleModes
    {
        public static UInt32 StartWorkHour = 8 * 3600;
        public static UInt32 EndWorkHour = 18 * 3600;

        public static ScheduleMode CheckMode(List<ScheduleData> schedules)
        {
            foreach (Schedule schedule in Schedules)
            {
                if(schedules.Count != schedule.Count) continue;
                Boolean theSame = true;
                foreach (ScheduleData scheduleData in schedules)
                {
                    Boolean getIt = false;
                    foreach (ScheduleData data in schedule)
                    {
                        if(data.StartTime == scheduleData.StartTime && data.EndTime == scheduleData.EndTime && data.Type == scheduleData.Type)
                        {
                            getIt = true;
                            break;
                        }
                    }
                    if (!getIt)
                    {
                        theSame = false;
                        break;
                    }
                }

                if (theSame)
                {
                    return schedule.Description;
                }
            }
            return ScheduleMode.CustomSchedule;
        }

        private static readonly List<Schedule> Schedules = new List<Schedule>();

        public static void CreateDefaultScheduleSet()
        {
            if (Schedules.Count == 0)
            {
                Schedule schedule = new Schedule();

                schedule.SetDefaultSchedule(ScheduleType.EventHandlering);
                schedule.Description = ScheduleMode.FullTimeEventHandling;
                Schedules.Add(schedule);
                //-------------------------------------------------------------------------
                schedule = new Schedule();
                for (int i = 0; i < 5; i++)
                {
                    schedule.AddSchedule(new ScheduleData
                    {
                        StartTime = Convert.ToUInt32(StartWorkHour + (86400 * i)),
                        EndTime = Convert.ToUInt32(EndWorkHour + (86400 * i)),
                        Type = ScheduleType.EventHandlering,
                    });
                }
                schedule.Description = ScheduleMode.WorkingTimeEventHandling;
                Schedules.Add(schedule);
                //-------------------------------------------------------------------------
                schedule = new Schedule();
                schedule.AddSchedule(new ScheduleData
                {
                    StartTime = 0,
                    EndTime = Convert.ToUInt32((86400 * 5)),
                    Type = ScheduleType.EventHandlering,
                });
                schedule.Description = ScheduleMode.WorkingDayEventHandling;
                Schedules.Add(schedule);
                //-------------------------------------------------------------------------
                schedule = new Schedule();
                for (int i = 0; i < 5; i++)
                {
                    schedule.AddSchedule(new ScheduleData
                    {
                        StartTime = Convert.ToUInt32((86400 * i)),
                        EndTime = Convert.ToUInt32(StartWorkHour + (86400 * i)),
                        Type = ScheduleType.EventHandlering,
                    });
                    schedule.AddSchedule(new ScheduleData
                    {
                        StartTime = Convert.ToUInt32(EndWorkHour + (86400 * i)),
                        EndTime = Convert.ToUInt32(86400 + (86400 * i)),
                        Type = ScheduleType.EventHandlering,
                    });
                }
                schedule.AddSchedule(new ScheduleData
                {
                    StartTime = Convert.ToUInt32((86400 * 5)),
                    EndTime = Convert.ToUInt32(86400 + (86400 * 6)),
                    Type = ScheduleType.EventHandlering,
                });
                schedule.Description = ScheduleMode.NonWorkingTimeEventHandling;
                Schedules.Add(schedule);
                //-------------------------------------------------------------------------
                schedule = new Schedule();
                schedule.AddSchedule(new ScheduleData
                {
                    StartTime = Convert.ToUInt32((86400 * 5)),
                    EndTime = Convert.ToUInt32((86400 * 7)),
                    Type = ScheduleType.EventHandlering,
                });
                schedule.Description = ScheduleMode.NonWorkingDayEventHandling;
                Schedules.Add(schedule);
                //-------------------------------------------------------------------------
                schedule = new Schedule();
                schedule.Description = ScheduleMode.NoSchedule;
                Schedules.Add(schedule);
                //-------------------------------------------------------------------------
                schedule = new Schedule();
                schedule.SetDefaultSchedule(ScheduleType.Continuous);
                schedule.Description = ScheduleMode.FullTimeRecording;
                Schedules.Add(schedule);
                //-------------------------------------------------------------------------
                schedule = new Schedule();
                
                for (int i = 0; i < 5; i++)
                {
                    schedule.AddSchedule(new ScheduleData
                    {
                        StartTime = Convert.ToUInt32(StartWorkHour + (86400 * i)),
                        EndTime = Convert.ToUInt32(EndWorkHour + (86400 * i)),
                        Type = ScheduleType.Continuous,
                    });
                }
                schedule.Description = ScheduleMode.WorkingTimeRecording;
                Schedules.Add(schedule);
                //-------------------------------------------------------------------------
                schedule = new Schedule();
                
                schedule.AddSchedule(new ScheduleData
                {
                    StartTime = 0,
                    EndTime = Convert.ToUInt32((86400 * 5)),
                    Type = ScheduleType.Continuous,
                });
                schedule.Description = ScheduleMode.WorkingDayRecording;
                Schedules.Add(schedule);
                //-------------------------------------------------------------------------
                schedule = new Schedule();
            
                for (int i = 0; i < 5; i++)
                {
                    schedule.AddSchedule(new ScheduleData
                    {
                        StartTime = Convert.ToUInt32((86400 * i)),
                        EndTime = Convert.ToUInt32(StartWorkHour + (86400 * i)),
                        Type = ScheduleType.Continuous,
                    });
                    schedule.AddSchedule(new ScheduleData
                    {
                        StartTime = Convert.ToUInt32(EndWorkHour + (86400 * i)),
                        EndTime = Convert.ToUInt32(86400 + (86400 * i)),
                        Type = ScheduleType.Continuous,
                    });
                }
                schedule.AddSchedule(new ScheduleData
                {
                    StartTime = Convert.ToUInt32((86400 * 5)),
                    EndTime = Convert.ToUInt32(86400 + (86400 * 6)),
                    Type = ScheduleType.Continuous,
                });

                schedule.Description = ScheduleMode.NonWorkingTimeRecording;
                Schedules.Add(schedule);
                //-------------------------------------------------------------------------
                schedule = new Schedule();
                
                schedule.AddSchedule(new ScheduleData
                {
                    StartTime = Convert.ToUInt32((86400 * 5)),
                    EndTime = Convert.ToUInt32(86400 + (86400 * 6)),
                    Type = ScheduleType.Continuous,
                });
                schedule.Description = ScheduleMode.NonWorkingDayRecording;
                Schedules.Add(schedule);
                //-------------------------------------------------------------------------
                schedule = new Schedule();

                schedule.SetDefaultSchedule(ScheduleType.EventRecording);
                schedule.Description = ScheduleMode.FullTimeEventRecording;
                Schedules.Add(schedule);
                //-------------------------------------------------------------------------
                schedule = new Schedule();

                for (int i = 0; i < 5; i++)
                {
                    schedule.AddSchedule(new ScheduleData
                    {
                        StartTime = Convert.ToUInt32(StartWorkHour + (86400 * i)),
                        EndTime = Convert.ToUInt32(EndWorkHour + (86400 * i)),
                        Type = ScheduleType.Continuous,
                    });
                }

                for (int i = 0; i < 5; i++)
                {
                    schedule.AddSchedule(new ScheduleData
                    {
                        StartTime = Convert.ToUInt32((86400 * i)),
                        EndTime = Convert.ToUInt32(StartWorkHour + (86400 * i)),
                        Type = ScheduleType.EventRecording,
                    });
                    schedule.AddSchedule(new ScheduleData
                    {
                        StartTime = Convert.ToUInt32(EndWorkHour + (86400 * i)),
                        EndTime = Convert.ToUInt32(86400 + (86400 * i)),
                        Type = ScheduleType.EventRecording,
                    });
                }
                schedule.AddSchedule(new ScheduleData
                {
                    StartTime = Convert.ToUInt32((86400 * 5)),
                    EndTime = Convert.ToUInt32(86400 + (86400 * 6)),
                    Type = ScheduleType.EventRecording,
                });

                schedule.Description = ScheduleMode.WorkingTimeRecordingWithEventRecording;
                Schedules.Add(schedule);
                //-------------------------------------------------------------------------
                schedule = new Schedule();

                schedule.AddSchedule(new ScheduleData
                {
                    StartTime = 0,
                    EndTime = Convert.ToUInt32((86400 * 5)),
                    Type = ScheduleType.Continuous,
                });
                schedule.AddSchedule(new ScheduleData
                {
                    StartTime = Convert.ToUInt32((86400 * 5)),
                    EndTime = Convert.ToUInt32(86400 + (86400 * 6)),
                    Type = ScheduleType.EventRecording,
                });
                schedule.Description = ScheduleMode.WorkingDayRecordingWithEventRecording;
                Schedules.Add(schedule);
                //-------------------------------------------------------------------------
                schedule = new Schedule();

                for (int i = 0; i < 5; i++)
                {
                    schedule.AddSchedule(new ScheduleData
                    {
                        StartTime = Convert.ToUInt32((86400 * i)),
                        EndTime = Convert.ToUInt32(StartWorkHour + (86400 * i)),
                        Type = ScheduleType.Continuous,
                    });
                    schedule.AddSchedule(new ScheduleData
                    {
                        StartTime = Convert.ToUInt32(EndWorkHour + (86400 * i)),
                        EndTime = Convert.ToUInt32(86400 + (86400 * i)),
                        Type = ScheduleType.Continuous,
                    });
                }
                schedule.AddSchedule(new ScheduleData
                {
                    StartTime = Convert.ToUInt32((86400 * 5)),
                    EndTime = Convert.ToUInt32(86400 + (86400 * 6)),
                    Type = ScheduleType.Continuous,
                });

                for (int i = 0; i < 5; i++)
                {
                    schedule.AddSchedule(new ScheduleData
                    {
                        StartTime = Convert.ToUInt32(StartWorkHour + (86400 * i)),
                        EndTime = Convert.ToUInt32(EndWorkHour + (86400 * i)),
                        Type = ScheduleType.EventRecording,
                    });
                }
                schedule.Description = ScheduleMode.NonWorkingTimeRecordingWithEventRecording;
                Schedules.Add(schedule);
                //-------------------------------------------------------------------------
                schedule = new Schedule();

                schedule.AddSchedule(new ScheduleData
                {
                    StartTime = Convert.ToUInt32((86400 * 5)),
                    EndTime = Convert.ToUInt32(86400 + (86400 * 6)),
                    Type = ScheduleType.Continuous,
                });

                schedule.AddSchedule(new ScheduleData
                {
                    StartTime = 0,
                    EndTime = Convert.ToUInt32((86400 * 5)),
                    Type = ScheduleType.EventRecording,
                });
                schedule.Description = ScheduleMode.NonWorkingDayRecordingWithEventRecording;
                Schedules.Add(schedule);
                //-------------------------------------------------------------------------
            }
        }

        public static Boolean HasEmptySchedule(List<ScheduleData> schedules)
        {
            if (schedules.Count == 0) return true;
            if (schedules.Count == 1)
                return (schedules[0].StartTime != 0 || schedules[0].EndTime != 86400*7);

            for (var i = 0; i < schedules.Count; i++)
            {
                //first one
                if (i == 0 && schedules[i].StartTime != 0)
                    return true;

                //last one
                if (i == (schedules.Count - 1))
                    return (schedules[i].EndTime != 86400*7);

                //continues record, no matter it's record or event
                if (schedules[i].EndTime != schedules[i + 1].StartTime)
                    return true;
            }

            return false;
        }

        public static void Clone(List<ScheduleData> schedule, List<ScheduleData> copyFrom)
        {
            if (schedule == null) return;
            schedule.Clear();
            foreach (var scheduleData in copyFrom)
            {
                schedule.Add(new ScheduleData
                {
                    StartTime = scheduleData.StartTime,
                    EndTime = scheduleData.EndTime,
                    Type = scheduleData.Type
                });
            }
        }
    }
}
