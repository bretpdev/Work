using System.Collections.Generic;

namespace CORNTSTQUE
{
	class ProcessingDetails
	{
		public enum Action
		{
			Add,
			Remove
		}

		public Action SelectedAction { get; set; }
		public List<QueueDetail> Queues { get; set; }
		public List<UserDetail> Users { get; set; }

		public ProcessingDetails()
		{
			Queues = new List<QueueDetail>();
			Users = new List<UserDetail>();
		}
	}//class
}//namespace
