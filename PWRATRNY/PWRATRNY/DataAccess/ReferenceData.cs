using Uheaa.Common.Scripts;

namespace PWRATRNY
{
    public class ReferenceData : SystemBorrowerDemographics
	{
		public Relationship RelationshipToBorrower { get; set; }

		public ReferenceData(string firstName, string lastName)
			: base()
		{
			RelationshipToBorrower = new Relationship();
			FirstName = firstName.ToUpper();
			LastName = lastName.ToUpper();
			MiddleIntial = "";
		}
	}
}