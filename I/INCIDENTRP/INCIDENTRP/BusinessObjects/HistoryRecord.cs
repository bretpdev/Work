using System;
using System.Text;
using SubSystemShared;

namespace INCIDENTRP
{
	public class HistoryRecord
	{
		public bool IsDirty { get; set; }
		public DateTime UpdateDateTime { get; set; }
		public SqlUser User { get; set; }
		public string Status { get; set; }
		public string StatusChangeDescription { get; set; }
		public string UpdateText { get; set; }

		public override string ToString()
		{
			StringBuilder historyBuilder = new StringBuilder();
			historyBuilder.AppendFormat("{0} {1} - {2:MM/dd/yyyy hh:mm tt} - {3}{4}", User.FirstName, User.LastName, UpdateDateTime, Status, Environment.NewLine);
			if (!string.IsNullOrEmpty(StatusChangeDescription))
                historyBuilder.AppendFormat("{0}{1}", StatusChangeDescription, Environment.NewLine); 
			if (!string.IsNullOrEmpty(UpdateText))
                historyBuilder.AppendFormat("{0}{1}", UpdateText, Environment.NewLine); 
			return historyBuilder.ToString();
		}
	}
}