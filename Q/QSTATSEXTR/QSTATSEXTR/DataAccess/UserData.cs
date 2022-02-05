using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace QSTATSEXTR
{
    class UserData
    {
        public DateTime RuntimeDate { get; set; }
        public string Queue { get; set; }
        [DbName("StatusCode")]
        public TaskStatus Status { get; set; }
        public string UserId { get; set; }
        [DbIgnore]
        public TimeSpan? TotalTimeWorked { get; set; }
        [DbIgnore]
        public DateTime? LastWorked { get; set; }
        public int CountInStatus { get; set; }

        public string TotalTime
        {
            get
            {
                return TotalTimeWorked?.ToString(TIME_FORMAT) ?? "";
            }
        }
        public string AverageTime
        {
            get
            {
                var span = TotalTimeWorked;
                if (span.HasValue)
                {
                    var totalSeconds = span.Value.TotalSeconds;
                    totalSeconds /= CountInStatus;
                    return new TimeSpan(0, 0, (int)totalSeconds).ToString(TIME_FORMAT);
                }
                return "";
            }
        }

        const string TIME_FORMAT = @"hh\:mm\:ss";
        public enum TaskStatus
        {
            Working = 'W',
            Assigned = 'A',
            Complete = 'C',
            Cancelled = 'X'
        }
    }
}
