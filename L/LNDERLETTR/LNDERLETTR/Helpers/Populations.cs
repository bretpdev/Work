﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNDERLETTR.Helpers
{
    public class Populations
    {
        public Dictionary<string, int> InBana { get; set; }
        public Dictionary<string, int> InUheaa { get; set; }
        public Dictionary<string, int> InOpen { get; set; }
        public Dictionary<string, int> InClosed { get; set; }

        public Populations()
        {
            InBana = new Dictionary<string, int>
            {
                { "814817", 814817 }, { "818334", 818334 }, { "824421", 824421 }, { "826079", 826079 }, { "831495", 831495 }, { "832733", 832733 },
                { "801871", 801871 }, { "802176", 802176 }, { "805317", 805317 }, { "806746", 806746 }, { "807674", 807674 }, { "811735", 811735 }

            };

            InUheaa = new Dictionary<string, int>
            {
                { "834529", 834529 }, { "829306", 829306 }, { "826717", 826717 }, { "830248", 830248 }, { "828476", 828476 },
                { "834437", 834437 }, { "834493", 834493 }, { "829769", 829769 }, { "834396", 834396 }, { "999775", 999775 },
                { "83449301", 83449301 }, { "82847601", 82847601 }
            };

            InOpen = new Dictionary<string, int>
            {
                //{"817476", 817476 },
                {"814817", 814817 }, {"818334", 818334 }, {"824421", 824421 }, {"826079", 826079 }, {"831495", 831495 }, {"832733", 832733 },
                {"834529", 834529 }, {"829306", 829306 }, {"826717", 826717 }, {"830248", 830248 }, {"828476", 828476 }, {"834437", 834437 },
                {"834493", 834493 }, {"829769", 829769 }, {"834396", 834396 }, {"999775", 999775 }, {"83449301", 83449301 }, {"82847601", 82847601 }
            };

            InClosed = new Dictionary<string, int>
            {
                {"814817", 814817 }, {"818334", 818334 }, {"824421", 824421 }, {"826079", 826079 }, {"831495", 831495 }, {"832733", 832733 },
                {"834529", 834529 }, {"829306", 829306 }, {"826717", 826717 }, {"830248", 830248 }, {"828476", 828476 }, {"834437", 834437 },
                {"834493", 834493 }, {"829769", 829769 }, {"834396", 834396 }, {"999775", 999775 }, {"83449301", 83449301 }, {"82847601", 82847601 }
            };
        }
    }
}