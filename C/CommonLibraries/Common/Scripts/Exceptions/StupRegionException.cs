using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.Scripts
{
	public class StupRegionException :Exception
	{
		public StupRegionException()
			: base()
		{
		}

		public StupRegionException(string message)
			: base(message)
		{
		}

		public StupRegionException(string message, Exception innerException)
			:base(message,innerException)
		{
		}
	}
}
