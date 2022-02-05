using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Uheaa.Common;

namespace DEMUPDTFED
{
	class Demographics
	{
		public enum Type { Address, Phone }

		public string Source { get; set; }
		public string Street1 { get; set; }
		public string Street2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string ForeignCountry { get; set; }
		public string HomePhone { get; set; }
		public string OtherPhone { get; set; }
		public string Email { get; set; }
        public string CommaDelimitedLine { get; set; }

		public bool HasAddress { get { return !string.IsNullOrEmpty(Street1) || !string.IsNullOrEmpty(City) || !string.IsNullOrEmpty(State) || !string.IsNullOrEmpty(Zip) || !string.IsNullOrEmpty(ForeignCountry); } }
		public bool HasHomePhone { get { return !string.IsNullOrEmpty(HomePhone); } }
		public bool HasOtherPhone { get { return !string.IsNullOrEmpty(OtherPhone); } }
		public bool HasEmail { get { return !string.IsNullOrEmpty(Email); } }

		/// <summary>
		/// Creates a blank Demographics object.
		/// </summary>
		public Demographics()
		{
		}

		/// <summary>
		/// Creates a Demographics object from a queue task comment, using the Comma Delimited parse method.
		/// </summary>
		/// <param name="queueTaskComment">
		/// A comma-delimited list of address fields, in the following order:
		/// Demographics source,Street1,Street2,City,State,Zip,Foreign country,Home phone,Other phone,Email address
		/// </param>
		/// <remarks>
		/// All 10 fields are required. Any fields beyond the 10th will be ignored.
		/// The ZIP code will be truncated to five characters for domestic addresses.
		/// Both phone numbers will have non-numeric characters stripped out.
		/// </remarks>
		public Demographics(string queueTaskComment)
		{
            CommaDelimitedLine = queueTaskComment;
			string[] fields = queueTaskComment.Split(',');
			if (fields.Length < 10) { throw new Exception("parse error"); }
			Source = fields[0];
			Street1 = fields[1];
			Street2 = fields[2];
			City = fields[3];
			State = fields[4];
			Zip = fields[5];
			ForeignCountry = fields[6];
			HomePhone = fields[7];
			OtherPhone = fields[8];
			Email = fields[9];
			if (string.IsNullOrEmpty(ForeignCountry)) { Zip = Zip.SafeSubString(0, 5); }
			HomePhone = System.Text.RegularExpressions.Regex.Replace(HomePhone, @"[^\d]", "", RegexOptions.Compiled);
            OtherPhone = System.Text.RegularExpressions.Regex.Replace(OtherPhone, @"[^\d]", "", RegexOptions.Compiled);
		}

		public bool AddressEquals(Demographics other)
		{
			if (State != other.State) { return false; }
			if (Zip != other.Zip.SafeSubString(0, 5)) { return false; }
			IEnumerable<string> street1Numbers = GetNumericGroups(Street1);
			IEnumerable<string> otherStreet1Numbers = GetNumericGroups(other.Street1);
			if (street1Numbers.Intersect(otherStreet1Numbers).Count() != street1Numbers.Count()) { return false; }
			IEnumerable<string> street2Numbers = GetNumericGroups(Street2);
			IEnumerable<string> otherStreet2Numbers = GetNumericGroups(other.Street2);
			if (street2Numbers.Intersect(otherStreet2Numbers).Count() != street2Numbers.Count()) { return false; }
			return true;
		}

		private List<string> GetNumericGroups(string text)
		{
			List<string> numericGroups = new List<string>();
			if (string.IsNullOrEmpty(text)) { return numericGroups; }
			
			StringBuilder numberBuilder = new StringBuilder();
			for (int i = 0; i < text.Length; i++)
			{
				if (System.Text.RegularExpressions.Regex.IsMatch(text[i].ToString(), @"\d"))
				{
					numberBuilder.Append(text[i]);
				}
				else if (numberBuilder.Length > 0)
				{
					numericGroups.Add(numberBuilder.ToString());
					numberBuilder = new StringBuilder();
				}
			}
			if (numberBuilder.Length > 0)
			{
				numericGroups.Add(numberBuilder.ToString());
			}
			
			return numericGroups;
		}
	}
}