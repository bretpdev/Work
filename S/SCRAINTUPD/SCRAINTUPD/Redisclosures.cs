using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace SCRAINTUPD
{
    public partial class ScraProcess
    {
        public List<ScraRecord> RediscloseProcessing(List<ScraRecord> recordsByBorrower)
        {
            recordsByBorrower = Redisclosure(recordsByBorrower);
            if (recordsByBorrower.Any(p => !p.TS0NUpdatedAt.HasValue))
            {
                foreach (ScraRecord record in recordsByBorrower)
                    DA.SetProcessedTS0N(record, false);
            }
            return recordsByBorrower;
        }

        public List<ScraRecord> Redisclosure(List<ScraRecord> records)
        {
            if (records.Any(p => !p.TS0NUpdatedAt.HasValue))
            {
                bool redisclose = false;
                if (records.All(p => p.ExemptSchedule))
                    AddArc(records, "PSCRA", "Borrower has an exempt schedule. ", true);
                else if (records.All(p => p.Deconverted))
                    AddArc(records, "PSCRA", "Borrower is deconverted. ", true);
                else if (records.All(p => p.ExemptLoanStatus))
                    AddArc(records, "PSCRA", "Borrower is in an exempt loan status. ", true);
                else if (records.All(p => p.ExemptLitigation))
                    AddArc(records, "MSCRA", "SCRA unable to redisclose, borrower is in lititgation. ");
                else if (records.Any(p => p.ExemptForbType))
                    AddArc(records, "PSCRA", "Borrower has an exempt forbearance type on a loan. ", true);
                else if (records.Any(p => p.ExemptDifferingSchedules))
                    AddArc(records, "MSCRA", "SCRA unable to redisclose, borrower is on mixed exempt and non exempt schedule types. ");
                else if (records.Any(p => p.ExemptInactiveSchedule))
                    AddArc(records, "MSCRA", "Borrower has loans on inactive schedules. ");
                else if (records.Any(p => p.ExemptFixedAlternateSchedule))
                    AddArc(records, "MSCRA", "SCRA unable to redisclose, borrower is on alternative fixed schedule. ");
                else
                    redisclose = true;

                if (redisclose)
                {
                    foreach (ScraRecord record in records)
                    {
                        if (record.ScriptAction.IsIn("E", "U") && !record.TS0NUpdatedAt.HasValue)
                        {
                            record.TS0NUpdatedSuccessfully = ExtendTerms(record);
                            if (record.TS0NUpdatedSuccessfully)
                                record.TS0NUpdatedAt = DateTime.Now;
                        }
                    }
                    if (records.Any(p => !p.TS0NUpdatedAt.HasValue))
                        ErrorProcessing(records, "MSCRA");
                }
            }
            else
            {
                foreach (ScraRecord record in records)
                {
                    DA.SetProcessedTS0N(record, false);
                    record.CalcSchedules = true; //dont calc schedules if we didnt update ts0n
                }
            }

            return records;
        }

        public bool ExtendTerms(ScraRecord record)
        {
            if (record.BorrBalance >= 30000.00m)
            {
                RI.FastPath(string.Format("TX3Z/CTS7C{0}", record.BorrowerSSN));
                if (RI.CheckForText(1, 72, "TSX3S"))
                    return ExtendMultiple(record);
                else if (RI.CheckForText(1, 72, "TSX7D"))
                    return ExtendSingle(record);
            }

            return true;
        }

        public bool ExtendSingle(ScraRecord record)
        {
            RI.PutText(14, 48, "Y", Key.Enter, true);
            if (RI.MessageCode == "40054")
                RI.PutText(18, 19, "0", Key.Enter, true);
            if (!RI.MessageCode.IsIn("01005", "01003"))
            {
                record.SessionErrorMessage = RI.Message;
                return false;
            }
            //record.LoanOwner = RI.GetText(6, 7, 8);
            return true;
        }

        public bool ExtendMultiple(ScraRecord record)
        {
            int timesToHitF8 = 0;
            for (int row = 7; RI.MessageCode != "90007"; row++)
            {
                RI.FastPath(string.Format("TX3Z/CTS7C{0}", record.BorrowerSSN));

                for (int count = timesToHitF8; count > 0; count--)
                {
                    RI.Hit(Key.F8);
                    if (RI.MessageCode == "90007")
                        break;
                }

                if (RI.CheckForText(row, 3, "  ") || row > 21)
                {
                    row = 7;
                    timesToHitF8++;
                    continue;
                }

                string loanSequence = RI.GetText(row, 20, 4);
                if (loanSequence.ToIntNullable() == record.LoanSequence)
                {
                    RI.PutText(22, 19, RI.GetText(row, 3, 2), Key.Enter);
                    return ExtendSingle(record);
                }
            }
            return true;
        }
    }
}
