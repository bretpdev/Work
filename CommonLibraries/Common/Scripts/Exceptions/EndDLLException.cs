using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.Scripts
{
	public class EndDLLException : Exception
	{
		public EndDLLException()
			:base()
		{
		}

		public EndDLLException(string message)
			: base(message)
		{
		}

		public EndDLLException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
