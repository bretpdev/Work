namespace DocIdCornerStone
{
	class DocumentDetail
	{
		public string DocId { get; set; }
		public string Description { get; set; }
		public string Arc { get; set; }

		public override string ToString()
		{
			return string.Format("{0} - {1}", DocId, Description);
		}
	}//class
}//namespace
