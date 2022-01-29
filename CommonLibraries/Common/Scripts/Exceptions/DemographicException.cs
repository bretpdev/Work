using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.Scripts
{
	public class DemographicException :Exception
	{
		public DemographicException()
		{
		}

		public DemographicException(string message)
			: base(message)
		{
		}

		public DemographicException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
