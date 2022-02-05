using System.Collections.Generic;

namespace DocIdCornerStone
{
	class DocumentSource
	{
		public string Code { get; set; }
		public string Description { get; set; }

		private DocumentSource(string code, string description)
		{
			Code = code;
			Description = description;
		}

		public static IEnumerable<DocumentSource> GetList()
		{
			List<DocumentSource> sources = new List<DocumentSource>();
			sources.Add(new DocumentSource("PO", "Post Office"));
			sources.Add(new DocumentSource("IH", "In-House"));
			sources.Add(new DocumentSource("FA", "Fax"));
			sources.Add(new DocumentSource("OT", "Other"));
			return sources;
		}

		public override string ToString()
		{
			return Description;
		}
	}//class
}//namespace
