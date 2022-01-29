using System;


namespace CCCLOSURES
{
    public class ScheduleSelctionData
    {
        [DataGridViewLabel("ID", 0)]
        public int StatusScheduleId { get; set; }
        [DataGridViewHidden(1)]
        public int RegionId { get; set; }
        [DataGridViewLabel("Region", 2)]
        public string RegionName { get; set; }
        [DataGridViewHidden(3)]
        public int StatusCodeId { get; set; }
        [DataGridViewLabel("Status",4)]
        public string StatusCodeName { get; set; }
        [DataGridViewLabel("Start", 5)]
        public DateTime StartAt { get; set; }
        [DataGridViewLabel("End", 6)]
        public DateTime EndAt { get; set; }
    }
}