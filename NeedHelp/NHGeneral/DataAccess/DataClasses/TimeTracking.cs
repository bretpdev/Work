using System;

namespace NHGeneral
{
    public class TimeTracking
    {
        public int TimeTrackingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Region { get; set; }
    }
}