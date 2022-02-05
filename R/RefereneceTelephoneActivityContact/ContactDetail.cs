using System.Collections.Generic;

namespace RefereneceTelephoneActivityContact
{
	public class ContactDetail
	{
		private readonly bool _testMode;

		private readonly string _borrowerId;
		public string BorrowerId { get { return _borrowerId; } }

		private readonly string _referenceId;
		public string ReferenceId { get { return _referenceId; } }

		public List<string> Arcs { get; set; }

		public string Comment { get; set; }

		private string _contact;
		public string Contact
		{
			get { return _contact ?? ""; }
			set
			{
				_contact = value;
				//Set Arc and Comment from DataAccess.
				Arcs = DataAccess.GetArcs(_testMode, value ?? "", Result);
				Comment = DataAccess.GetCommentText(_testMode, value ?? "", Result);
			}
		}

		private string _result;
		public string Result
		{
			get { return _result ?? ""; }
			set
			{
				_result = value;
				//Set Arc and Comment from DataAccess.
				Arcs = DataAccess.GetArcs(_testMode, Contact, value ?? "");
				Comment = DataAccess.GetCommentText(_testMode, Contact, value ?? "");
			}
		}

		public ContactDetail(bool testMode, string borrowerId, string referenceId)
		{
			_testMode = testMode;
			_borrowerId = borrowerId;
			_referenceId = referenceId;
		}
	}//class
}//namespace
