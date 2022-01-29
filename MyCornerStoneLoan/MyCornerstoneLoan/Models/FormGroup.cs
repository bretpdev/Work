using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCornerstoneLoan.Models
{
	public class FormGroup
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public List<Form> Forms { get; set; }

		public FormGroup(string id, string groupName, params string[] groupsOfNameEnglishSpanishExplanation)
		{
			this.Id = id;
			this.Name = groupName;
			this.Forms = new List<Form>();
			for (int i = 0; i < groupsOfNameEnglishSpanishExplanation.Length; i += 4)
			{
				var form = new Form()
				{
					Name = groupsOfNameEnglishSpanishExplanation[i],
					EnglishLink = groupsOfNameEnglishSpanishExplanation[i + 1],
					SpanishLink = groupsOfNameEnglishSpanishExplanation[i + 2],
					ExplanationLink = groupsOfNameEnglishSpanishExplanation[i + 3]
				};
				this.Forms.Add(form);
			}
		}

		public void Order()
		{
			this.Forms = this.Forms.OrderBy(o => o.Name).ToList();
		}

		public FormGroup Append(Form form)
		{
			this.Forms.Add(form);
			return this;
		}
	}
}