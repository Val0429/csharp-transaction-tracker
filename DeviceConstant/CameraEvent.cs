using System;
using System.Collections.Generic;
using System.Globalization;
using Constant;

namespace DeviceConstant
{
    public class CameraEvent
    {
        public EventType Type { get; set; }
        public DateTime DateTime { get; set; }
        public UInt64 Timecode { get; set; }
        /// <summary>
        /// Event ID
        /// </summary>
        public UInt16 Id { get; set; }
        public UInt16 NVRId { get; set; }
        public Boolean Value { get; set; }
        public String Status { get; set; }

        public CameraEvent()
        {
            Id = 1;
            Value = true;
        }

        public string ToLocalizationString()
        {
            return Type.ToLocalizationString(Value, Id);
        }

        public override string ToString()
        {
            try
            {
                return Type.ToString(Value, Id);
            }
            catch (Exception ex)
            {
                return base.ToString();
            }
        }
    }
}