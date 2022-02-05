using System;

namespace AACLOCKFED
{
	class Batches
	{
		public LockAndUnlock.Action Action { get; set; }

		public string NewlineSeparatedValues { get; set; }

		public string[] ToArray() { return NewlineSeparatedValues.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries); }
	}//class
}//namespace
