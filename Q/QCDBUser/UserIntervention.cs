using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QCDBUser
{
    class UserIntervention
    {
        public Int64 ID { get; set; }
        public string ReportName { get; set; }
        public string UserID { get; set; }
        public DateTime ActivityDate { get; set; }
        public string Description { get; set; }
        public int RequiredDays { get; set; }
        public string BusinessUnit { get; set; }
        public string PriorityCategory { get; set; }
        public string PriorityUrgency { get; set; }
        public Nullable<DateTime> SavedDate { get; set; }
    }
}
