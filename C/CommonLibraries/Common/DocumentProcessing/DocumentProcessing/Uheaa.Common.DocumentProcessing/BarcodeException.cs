using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.DocumentProcessing
{
	class BarcodeException : Exception
	{
		public BarcodeException()
		{
		}

		public BarcodeException(string message)
			: base(message)
		{
		}

		public BarcodeException(string message, Exception innerException)
			:base(message,innerException)
		{
		}
	}
}
