using System;

namespace ACHSETUPFD
{
	class LetterSelection
	{
		[Flags]
		public enum Letter
		{
			None = 0,
			Approved = 1,
			Denied = 2
		}

		public Letter Selected { get; set; }
	}
}
