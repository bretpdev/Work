using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace CPINTRTLPD
{
    public class Processor
    {
        ReflectionInterface ri;
        UserInput input;
        IEnumerable<CsvRow> records;
        Dictionary<DateTime, string> batches;
        Logger log;
        public Processor(ReflectionInterface ri, Logger log, UserInput input)
        {
            this.ri = ri;
            this.log = log;
            this.input = input;
        }

        public bool Process()
        {
            if (!ParseFile())
                return false;

            var dates = records.GroupBy(o => o.PD_EFF_SR_LPD06);

            if (!CreateDateBatches())
                return false;

            if (!CopyRates())
                return false;

            return true;
        }

        private bool ParseFile()
        {
            var results = CsvHelper.ParseTo<CsvRow>(File.ReadAllLines(input.InputFileLocation));
            if (results.InvalidLines.Any())
            {
                log.Error(results.GenerateErrorMessage());
                return false;
            }
            this.records = results.ValidLines.Select(o => o.ParsedEntity);
            return true;
        }

        private bool CreateDateBatches()
        {
            batches = new Dictionary<DateTime, string>();
            foreach (var date in records.Select(o => o.PD_EFF_SR_LPD06).Distinct())
            {
                ri.FastPath("TX3Z/ATX24");
                ri.PutText(5, 24, "LP06 COPY FR 000749 " + date.ToShortDateString() + " TO " + input.Guarantor + " " + input.Owner);
                ri.PutText(13, 15, date.ToString("MMddyy"));
                ri.Hit(ReflectionInterface.Key.Enter);
                if (ri.MessageCode != "01004")
                {
                    log.Error("Batch creation failed for date " + date.ToShortDateString());
                    return false;
                }
                string batchId = ri.GetText(5, 15, 8);
                log.Log("Created Batch ID {0} for date {1}", batchId, date.ToShortDateString());
                batches[date] = batchId;
            }
            return true;
        }

        private bool CopyRates()
        {
            foreach (var record in records)
            {
                if (!UpdateCopyFromScreen(record))
                    return false;
                if (!UpdateCopyToGuarantor(record))
                    return false;
                if (!UpdateCopyToOwner(record))
                    return false;
                if (!UpdateCopyToBond(record))
                    return false;
            }
			if (!PostBatches())
                return false;

            return true;
        }

        private bool UpdateCopyFromScreen(CsvRow record)
        {
            ri.FastPath("TX3Z/ATX2A");
            ri.PutText(7, 29, "A"); //status
            ri.PutText(8, 29, record.IC_LON_PGM);
            ri.PutText(9, 29, record.PF_RGL_CAT);
            ri.PutText(10, 29, record.IF_GTR);
            ri.PutText(11, 29, record.IF_OWN);
            ri.PutText(12, 29, record.IF_BND_ISS);
            ri.PutText(15, 29, record.PD_EFF_SR_LPD06.ToString("MMddyy"));
            ri.PutText(17, 29, "06"); //LPD table
            ri.PutText(20, 29, record.PC_ITR_TYP);
            ri.Hit(ReflectionInterface.Key.Enter);
            if (ri.ScreenCode != "TXX3W")
            {
                log.Error("Error processing COPY FROM screen for record {0}", record);
                return false;
            }
            return true;
        }

        private bool UpdateCopyToGuarantor(CsvRow record)
        {
            ri.PutText(18, 31, input.Guarantor); //guarantor
            ri.PutText(18, 40, "00000000", true); //owner
            ri.PutText(18, 51, "00000000", true); //bond
            ri.PutText(22, 4, batches[record.PD_EFF_SR_LPD06]);
            ri.Hit(ReflectionInterface.Key.Enter);
            if (ri.MessageCode != "01185")
            {
                log.Error("Error processing COPY TO (Guarantor) screen for record {0}", record);
                return false;
            }
            return true;
        }

        private bool UpdateCopyToOwner(CsvRow record)
        {
            ri.PutText(1, 4, "A");
            ri.Hit(ReflectionInterface.Key.Enter);
            ri.PutText(18, 31, input.Guarantor, true); //guarantor
            ri.PutText(18, 40, input.Owner, true); //owner
            ri.PutText(18, 51, "00000000", true); //bond
            ri.PutText(22, 4, batches[record.PD_EFF_SR_LPD06]);
            ri.Hit(ReflectionInterface.Key.Enter);
            if (ri.MessageCode != "01185")
            {
                log.Error("Error processing COPY TO (Owner) screen for record {0}", record);
                return false;
            }
            return true;
        }

        private bool UpdateCopyToBond(CsvRow record)
        {
            ri.PutText(1, 4, "A");
            ri.Hit(ReflectionInterface.Key.Enter);
            ri.PutText(18, 31, input.Guarantor, true); //guarantor
            ri.PutText(18, 40, input.Owner, true); //owner
            ri.PutText(18, 51, input.Bond, true); //bond
            ri.PutText(22, 4, batches[record.PD_EFF_SR_LPD06]);
            if (ri.CheckForText(21, 16, "STOP")) //check to make sure EFFECTIVE STOP DATE is actually displayed on screen
                ri.PutText(22, 16, record.PD_EFF_END_LPD06.ToString("MMddyy"));
            ri.Hit(ReflectionInterface.Key.Enter);
            if (ri.MessageCode != "01185")
            {
                log.Error("Error processing COPY TO (Bond) screen for record {0}", record);
                return false;
            }
            return true;
        }

        private bool PostBatches()
        {
            foreach (var batch in batches)
            {
                ri.FastPath("TX3Z/CTX26" + batch.Value);
                ri.Hit(ReflectionInterface.Key.F6); //post
                bool successful = false;
                if (ri.MessageCode == "01273")
                {
                    log.Log("Successfully posted batch {0}", batch.Value);
                    successful = true;
                }
                else if (new string[] { "01173", "01229", "01230" }.Contains(ri.MessageCode))
                {
                    ri.FastPath("TX3Z/CTX3M" + batch.Value);
                    ri.Hit(ReflectionInterface.Key.F6); //post
                    if (ri.MessageCode == "01273")
                        successful = true;
                }
                if (!successful)
                {
                    log.Error("Error posting batch {0}.  Error: {1}", batch.Value, ri.Message);
                    return false;
                }
            }
            return true;
        }
    }
}
