using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace UNEXDASCRB
{
    public class Scrubber
    {
        public ProcessLogRun PLR { get; set; }
        private DataAccess DA { get; set; }
        public Scrubber(ProcessLogRun plr)
        {
            PLR = plr;
            DA = new DataAccess(new LogDataAccess(DataAccessHelper.CurrentMode, PLR.ProcessLogId, false, true));
        }

        public int ScrubData()
        {
            Dictionary<string, List<string>> foundData = new Dictionary<string, List<string>>();
            foreach (var columnName in DA.GetScrubbableColumns())
            {
                foreach (var callId in DA.GetScrubbableRows(columnName))
                {
                    if (!foundData.ContainsKey(callId))
                        foundData[callId] = new List<string>();
                    foundData[callId].Add(columnName);
                }
            }
            foreach (var callId in foundData.Keys)
            {
                Console.WriteLine($"Found potential PII for CallId {callId} in column(s) {string.Join(", ", foundData[callId])}.");
                foreach (var column in foundData[callId])
                {
                    DA.ScrubRow(callId, column);
                }
                Console.WriteLine($"Potential PII scrubbed from CallId {callId}.");
            }

            return 0;
        }
    }
}
