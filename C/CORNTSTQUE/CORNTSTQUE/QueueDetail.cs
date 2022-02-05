namespace CORNTSTQUE
{
	class QueueDetail
	{
		public string Queue { get; set; }
		public string SubQueue { get; set; }

		public QueueDetail(string queue, string subQueue)
		{
			Queue = queue;
			SubQueue = subQueue;
		}
	}//class
}//namespace
