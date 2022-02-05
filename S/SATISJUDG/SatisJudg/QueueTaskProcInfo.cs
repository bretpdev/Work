using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;

namespace SatisJudg
{
    public class QueueTaskProcInfo
    {

        public string SSN { get; set; }
        public string Name { get; set; }
        public string AKA { get; set; }
        public SystemBorrowerDemographics BrwDemos { get; set; }
        public string SatisDate { get; set; }
        public string SatisCd { get; set; }
        public double Balance { get; set; }
        public string SatisRea { get; set; }
        public string InactRea { get; set; }
        public string Judge { get; set; }
        public string DockNo { get; set; }
        public string FiledDate { get; set; }
        public string County { get; set; }
        public string Court { get; set; }
        public string ComplDate { get; set; }
        public double ComplAmt { get; set; }
        public string AbstractNo { get; set; }
        public double FeeAmt { get; set; }

        public QueueTaskProcInfo(string ssn, string name)
        {
            SSN = ssn;
            Name = name;
            AKA = string.Empty;
            SatisDate = string.Empty;
            SatisCd = string.Empty;
            Balance = 0;
            SatisRea = string.Empty;
            InactRea = string.Empty;
            Judge = string.Empty;
            DockNo = string.Empty;
            FiledDate = string.Empty;
            County = string.Empty;
            Court = string.Empty;
            ComplDate = string.Empty;
            ComplAmt = 0;
            AbstractNo = string.Empty;
            FeeAmt = 0;
        }
    }
}
