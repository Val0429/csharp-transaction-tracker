using System;
using System.Collections.Generic;
using Constant;

namespace ServerProfile
{
    public partial class DeviceManager
    {
        private IEnumerable<ScheduleData> ConvertWeekScheduleToScheduleData(String text, String mode)
        {
            var scheduleDate = new List<ScheduleData>();

            //Shift Time Zone Before Convert
            Int16 shiftBlock = (Int16)(Server.Server.TimeZone / 600);
            if (shiftBlock != 0)
            {
                text = (shiftBlock > 0)
                    ? (text.Substring(text.Length - shiftBlock) + text.Substring(0, text.Length - shiftBlock))
                    : (text.Substring(shiftBlock * -1) + text.Substring(0, shiftBlock * -1));
            }

            String subStr;
            for (Int32 i = 0; i < text.Length; i++)
            {
                Char type = text[i];
                subStr = text.Substring(i + 1);

                switch (type)
                {
                    case '0':
                        i = GetIndex(subStr.IndexOf('1'), subStr.IndexOf('2'), i, text.Length);
                        break;

                    default:
                        ScheduleData sch = new ScheduleData
                        {
                            StartTime = (UInt32)(i * 600),
                        };

                        if (mode == "Record")
                        {
                            sch.Type = (type == '1') ? ScheduleType.Continuous : ScheduleType.EventRecording;
                        }
                        else
                        {
                            sch.Type = ScheduleType.EventHandlering;
                        }

                        i = (type == '1')
                            ? GetIndex(subStr.IndexOf('0'), subStr.IndexOf('2'), i, text.Length)
                            : GetIndex(subStr.IndexOf('0'), subStr.IndexOf('1'), i, text.Length);

                        sch.EndTime = (UInt32)((i + 1) * 600);
                        scheduleDate.Add(sch);
                        break;
                }
            }

            return scheduleDate;
        }

        private String ConvertScheduleDataToWeekSchedule(IEnumerable<ScheduleData> schedules)
        {
            String text = new String('0', 1008);

            foreach (ScheduleData scheduleData in schedules)
            {
                UInt16 start = (UInt16)(scheduleData.StartTime / 600);
                UInt16 end = (UInt16)(scheduleData.EndTime / 600);

                char c = '0';
                switch (scheduleData.Type)
                {
                    case ScheduleType.Continuous:
                    case ScheduleType.EventHandlering:
                        c = '1';
                        break;

                    case ScheduleType.EventRecording:
                        c = '2';
                        break;
                }
                text = text.Substring(0, start) + new String(c, end - start) + text.Substring(end);
            }

            //Shift Time Zone After Convert
            Int16 shiftBlock = (Int16)(Server.Server.TimeZone / 600);
            if (shiftBlock != 0)
            {
                text = (shiftBlock > 0)
                    ? (text.Substring(shiftBlock) + text.Substring(0, shiftBlock))
                    : (text.Substring(text.Length + shiftBlock) + text.Substring(0, text.Length + shiftBlock));
            }

            return text;
        }

        private static Int32 GetIndex(Int32 s1, Int32 s2, Int32 i, Int32 length)
        {
            return (s1 == -1 || s2 == -1)
                ? ((s1 + s2 == -2) ? length - 1 : (s2 + s1 + 1 + i))
                : (i + ((s1 < s2) ? s1 : s2));
        }
    }
}