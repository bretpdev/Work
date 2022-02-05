using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace AddDateForFSAReports
{
    class FileData
    {
        public string Counter { get; set; }
        public string Servicer { get; set; }
        public string Ssn { get; set; }
        public string Type { get; set; }
        public decimal Balance { get; set; }
        public decimal OutStandingInterest { get; set; }

        public string ToString(string date)
        {
            return string.Format("{0} {1} {2} {3} {4} {5} {6}", Counter, Servicer, Ssn, Type, Balance.ToString("0.00").PadLeft(10, '0'), OutStandingInterest.ToString("0.00").PadLeft(10, '0'), date);
        }

        public static FileData Parse(string line)
        {
            return new FileData()
            {
                Counter = line.Substring(0,8),
                Servicer = line.Substring(9,6),
                Ssn = line.Substring(16,9),
                Type = line.Substring(26,2),
                Balance = line.Substring(29,10).ToDecimal(),
                OutStandingInterest = line.Substring(40).ToDecimal()
            };
        }
    }
}
