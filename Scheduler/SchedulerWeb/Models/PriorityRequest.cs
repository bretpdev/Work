using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uheaa.Common.DataAccess;

namespace SchedulerWeb
{
    public class PriorityRequest
    {
        public int RequestPriorityId { get; set; }
        public int? ParentId { get; set; }
        public int RequestTypeId { get; set; }
        public int RequestId { get; set; }
        public int PriorityLevel { get; set; }
        public string RequestType { get; set; }
        public string Name { get; set; }
        public DateTime? Requested { get; set; }
        public string CurrentCourt { get; set; }
        public int Priority { get; set; }
        public string Status { get; set; }
        public string DevBeginString { get { return Format(DevBegin); } }
        public DateTime? DevBegin { get; set; }
        public string DevEndString { get { return Format(DevEnd); } }
        public DateTime? DevEnd { get; set; }
        public string TestBeginString { get { return Format(TestBegin); } }
        public DateTime? TestBegin { get; set; }
        public string TestEndString { get { return Format(TestEnd); } }
        public DateTime? TestEnd { get; set; }

        private string Format(DateTime? date)
        {
            if (date == null)
                return "";
            return date.Value.ToShortDateString();
        }

        public static List<PriorityRequest> GetAll()
        {
            return DataAccessHelper.ExecuteList<PriorityRequest>("GetRequestPriorityList", DataAccessHelper.Database.Scheduler);
        }
    }
}