using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;

namespace QBUILDFED
{
    public class QueueBuilderRecord
    {
        public string SSN { get; private set; }
        public string ARC { get; private set; }
        public DateTime? DateFrom { get; private set; }
        public DateTime? DateTo { get; private set; }
        public DateTime? NeededByDate { get; private set; }
        public string RecipientId { get; private set; }
        public string RegardsToCode { get; private set; }
        public string RegardsToId { get; private set; }
        public List<int> LoanSequences { get; private set; }
        public string Comment { get; private set; }

        /// <summary>
        /// Populates the QueueBuilderRecord from a given line in the file.
        /// </summary>
        public QueueBuilderRecord Populate(string recordText, DataAccess da)
        {
            QueueBuilderRecord record = new QueueBuilderRecord();
            List<string> fields = recordText.SplitAndRemoveQuotes(",");
            record.SSN = fields[0];
            record.ARC = fields[1];
            record.DateFrom = fields[2].ToDateNullable();
            record.DateTo = fields[3].ToDateNullable();
            record.NeededByDate = fields[4].ToDateNullable();
            record.RecipientId = fields[5];

            if (record.RecipientId.Length == 10)
                record.RecipientId = da.GetSsnFromAccountNumber(record.RecipientId);

            record.RegardsToCode = fields[6];
            record.RegardsToId = fields[7];

            if (record.RegardsToId.Length == 10)
                record.RegardsToId = da.GetSsnFromAccountNumber(record.RegardsToId);

            if (fields[8] != "ALL")
                record.LoanSequences = fields[8].Split(',').Select(p => int.Parse(p)).ToList();

            record.Comment = fields[9].SafeSubString(0, 152);
            return record;
        }
    }
}