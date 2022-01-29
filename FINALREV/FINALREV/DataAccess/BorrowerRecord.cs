using System;
using System.Collections.Generic;
using Uheaa.Common.Scripts;

namespace FINALREV
{
    public class BorrowerRecord
    {
        public int BorrowerRecordId { get; set; }
        public string Ssn { get; set; }
        public FinalReview.RecoveryStep Step { get; set; }
        public DateTime StartDate { get; set; }
        public FinalReview.SkipSystem SkipSystem { get; set; } = FinalReview.SkipSystem.None;
        public string SkipType { get; set; }
        public List<ReferenceData> ReferenceIds { get; set; }
        public SystemBorrowerDemographics Demos { get; set; }
        public bool IsEndorser { get; set; }
        public bool TaskAdded { get; set; }
        public bool OLTaskNeeded { get; set; }

        public BorrowerRecord()
        {
            ReferenceIds = new List<ReferenceData>();
        }

        public void UpdateStep(DataAccess da, FinalReview.RecoveryStep step)
        {
            Step = step;
            da.UpdateStep(BorrowerRecordId, step);
        }
    }
}