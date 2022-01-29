using System;

namespace TIMETRAKUP
{
    class UserTime
    {
        public int TimeTrackingId { get; set; }
        public string Region { get; set; }
        public int TicketID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan? Elapsed
        {
            get
            {
                if (EndTime != null)
                {
                    try
                    {
                        TimeSpan span = new TimeSpan((EndTime.Value - StartTime).Ticks);
                        //return new TimeSpan(span.Days, span.Hours, span.Minutes, span.Seconds);
                        return span.Subtract(new TimeSpan(0, 0, 0, 0, span.Milliseconds));
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                else
                    return null;
            }
        }
    }
}
