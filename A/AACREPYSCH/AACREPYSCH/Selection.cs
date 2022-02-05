namespace AACREPYSCH
{
	/// <summary>
	/// Represents a row on a page (on a screen such as TS0N) that can be selected.
	/// </summary>
	class Selection
	{
		public int Page { get; private set; }
		public int Row { get; private set; }

		public Selection(int page, int row)
		{
			Page = page;
			Row = row;
		}
	}//class
}//namespace
