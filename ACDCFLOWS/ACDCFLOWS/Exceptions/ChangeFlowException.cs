using System;
using Q;

namespace ACDCFlows
{
	class ChangeFlowException : Exception
	{
		public ChangeFlowException(string message, Exception exception)
			: base(message, exception)
		{
		}

		public ChangeFlowException(string message)
			: base(message)
		{
		}

		public ChangeFlowException()
			: base()
		{
		}
	}
}
