using System;

namespace TimeTracking
{
    public class UserTime
    {
        public int TimeTrackingId { get; set; }
        public string Region { get; set; }
        public int? TicketID { get; set; }
        public int? SystemTypeId { get; set; }
        public string SystemType { get; set; }
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
        public int? CostCenterId { get; set; }
        public string CostCenter { get; set; }
        public bool BatchProcessing { get; set; }
        public string GenericMeeting { get; set; }
    }
}