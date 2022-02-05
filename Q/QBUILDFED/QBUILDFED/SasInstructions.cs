namespace QBUILDFED
{
	public class SasInstructions
	{
		public string FileName { get; set; }
		public bool EmptyFileIsOk { get; set; }
		public bool MissingFileIsOk { get; set; }
		public bool ProcessMultipleFiles { get; set; }
	}
}