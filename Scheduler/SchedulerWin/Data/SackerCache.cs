using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerWeb
{
    public class SackerCache
    {
        public int SackerCacheId { get; set; }
        public int RequestTypeId { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public string Status { get; set; }
        public int? Priority { get; set; }
        public string Court { get; set; }
        public string AssignedProgrammer { get; set; }
        public string AssignedTester { get; set; }
        public decimal DevEstimate { get; set; }
        public decimal TestEstimate { get; set; }

        public bool CacheValuesMatch(SackerCache other)
        {
            return RequestTypeId == other.RequestTypeId && Name == other.Name && Id == other.Id && Status == other.Status && Priority == other.Priority
                && Court == other.Court && AssignedProgrammer == other.AssignedProgrammer && AssignedTester == other.AssignedTester;
        }
    }
}
