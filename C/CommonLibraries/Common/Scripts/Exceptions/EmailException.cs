using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.Scripts
{
	class EmailException :Exception
	{
		public EmailException()
			: base()
		{
		}

		public EmailException(string message)
			: base(message)
		{
		}

		public EmailException (string message, Exception innerException)
			:base(message, innerException)
		{
		}
	}
}
