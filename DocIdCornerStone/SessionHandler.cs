using Q;

namespace DocIdCornerStone
{
	class SessionHandler : ScriptSessionBase
	{
		public SessionHandler(ReflectionInterface ri) : base(ri)
		{
		}

		public new SystemBorrowerDemographics GetDemographicsFromTX1J(string ssn)
		{
			return base.GetDemographicsFromTX1J(ssn);
		}

		public bool LogCorrespondence(string ssn, string arc, string comment)
		{
            return (ATD22ByBalance(ssn, arc, comment, Program.SCRIPT_ID, false, ssn, TD22.RegardsTo.None, "") == Common.CompassCommentScreenResults.CommentAddedSuccessfully);
		}
	}//class
}//namespace
