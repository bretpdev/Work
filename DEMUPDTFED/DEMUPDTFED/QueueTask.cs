using System;

namespace DEMUPDTFED
{
	class QueueTask
	{
		public enum Relationship { Borrower, Endorser, Reference }

		public string Queue { get; private set; }
		public string SSN { get; private set; }
		public string AccountNumber { get; private set; }
		public string RecipientId { get; private set; }
		public Relationship RecipientRelationship { get; private set; }
		public string Comment { get; private set; }
		public Demographics Demographics { get; private set; }

		public QueueTask(string queue, string ssn, string accountNumber, string recipientId, string recipientRelationshipIndicator, string comment)
		{
			Queue = queue;
			SSN = ssn;
			AccountNumber = accountNumber;
			RecipientId = recipientId;
			switch (recipientRelationshipIndicator)
			{
				case "E":
					RecipientRelationship = Relationship.Endorser;
					break;
				case "R":
					RecipientRelationship = Relationship.Reference;
					break;
				default:
					RecipientRelationship = Relationship.Borrower;
					break;
			}
			Comment = comment;
			try
			{
				Demographics = new Demographics(comment);
			}
			catch (Exception)
			{
				Demographics = null;
			}
		}
	}
}