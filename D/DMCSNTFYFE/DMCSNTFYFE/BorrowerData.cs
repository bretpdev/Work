using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common;

namespace DMCSNTFYFE
{
    public class BorrowerData
    {
        [Order(2)]
        public string AccountNumber { get; set; }
        [Order(1)]
        public string KeyLine { get; set; }

        [Ignore()]
        public List<int> LoanSeq { get; set; }
        [Order(3)]
        public string FirstName { get; set; }
        [Order(4)]
        public string LastName { get; set; }
        [Order(5)]
        public string Address1 { get; set; }
        [Order(6)]
        public string Address2 { get; set; }
        [Order(7)]
        public string City { get; set; }
        [Order(8)]
        public string State { get; set; }
        [Order(9)]
        public string Zip { get; set; }
        [Order(10)]
        public string Country { get; set; }


        public BorrowerData()
        {
            LoanSeq = new List<int>();
        }


        public static BorrowerData Populate(List<string> line)
        {
            if (!CheckData(line))
                return null;

            BorrowerData bor = new  BorrowerData()
            {
                    AccountNumber = line[0],
                    KeyLine = line[1],
                    FirstName = line[2],
                    LastName = line[3],
                    Address1 = line[4],
                    Address2 = line[5],
                    City = line[6],
                    State = line[7],
                    Zip = line[8],
                    Country = line[9]

            };

            bor.LoanSeq.Add(line[10].ToInt());

            return bor;
        }

        private static bool CheckData(List<string> line)
        {
            if (line[0].IsNullOrEmpty())
                return false;
            else if (line[1].IsNullOrEmpty())
                return false;
            else if (line[2].IsNullOrEmpty())
                return false;
            else if (line[3].IsNullOrEmpty())
                return false;
            else if (line[4].IsNullOrEmpty())
                return false;
            else if (line[6].IsNullOrEmpty())
                return false;
            else if (line[8].IsNullOrEmpty())
                return false;
            else
                return true;
        }
    }
}
