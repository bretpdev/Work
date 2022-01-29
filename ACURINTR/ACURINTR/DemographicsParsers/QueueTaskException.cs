using System;

namespace ACURINTR.DemographicsParsers
{
	class QueueTaskException : ParseException
	{
		public QueueTaskException(string message, string accountNumber, string demographicsSource, string systemSource, string demographicsText)
			: base(message, accountNumber, demographicsSource, systemSource, demographicsText) { }
	}
}
