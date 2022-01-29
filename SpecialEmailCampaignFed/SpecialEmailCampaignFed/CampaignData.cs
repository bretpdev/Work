namespace SpecialEmailCampaignFed
{
	public class CampaignData
	{
		public long CampID { get; set; }
		public bool CornerStone { get; set; }
        public bool IncludeAccountNumber { get; set; }
		public string EmailSubjectLine { get; set; }
		public string Arc { get; set; }
		public string CommentText { get; set; }
		public string DataFile { get; set; }
		public string HTMLFile { get; set; }
		public string EmailFrom { get; set; }
	}
}
