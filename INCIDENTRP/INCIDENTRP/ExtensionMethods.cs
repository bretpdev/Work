using System;
using System.Collections.Generic;
using System.Linq;

namespace INCIDENTRP
{
    static class ExtensionMethods
	{
        public enum Separator
        {
            None,
            Dash,
            Dot,
            Space,
            SameAsOriginal
        }

		public static string AsString(this List<HistoryRecord> history)
		{
			List<HistoryRecord> sortedRecords = history.OrderByDescending(p => p.UpdateDateTime).ToList();
			List<string> stringizedRecords = sortedRecords.Select(p => p.ToString()).ToList();
			string joinedRecords = string.Join(Environment.NewLine, stringizedRecords.ToArray());
			return joinedRecords;
		}
	}
}