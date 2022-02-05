using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using Q;

namespace RefereneceTelephoneActivityContact
{
	public class DataAccess : DataAccessBase
	{
		/// <summary>
		/// Gets a list of ARCs to be used on TD22, given the contact result (and reference result if applicable).
		/// </summary>
		/// <param name="testMode">True if running in test mode.</param>
		/// <param name="contactResult">
		/// Result of contact, from the Result column of the RTAC_REF_ContactResult table in BSYS.
		/// </param>
		/// <param name="referenceResult">
		/// Result of talking to the reference (if applicable), from the Result column of the RTAC_REF_RefContact table in BSYS.
		/// This value is only checked if the contact result is one that has a foreign key relationship to a reference result.
		/// </param>
		public static List<string> GetArcs(bool testMode, string contactResult, string referenceResult)
		{
			List<string> arcs = new List<string>();
			string query = string.Format("SELECT Arc FROM RTAC_REF_ContactResult WHERE Result = '{0}'", contactResult);
			if (GetContactResultsForReferences(testMode).Contains(contactResult))
			{
				//For "Called Ref; Spoke w/Borr", run the above query to get the first ARC. Then run the next query for a second ARC.
				if (contactResult == "Called Ref; Spoke w/Borr")
				{
					string extraArc = BsysDataContext(testMode).ExecuteQuery<string>(query).SingleOrDefault();
					if (!string.IsNullOrEmpty(extraArc)) { arcs.Add(extraArc); }
				}
				//For all reference contacts, get the ARC from the RefContact table.
				query = string.Format("SELECT Arc FROM RTAC_REF_RefContact WHERE Contact = '{0}' AND Result = '{1}'", contactResult, referenceResult);
			}
			string arc = BsysDataContext(testMode).ExecuteQuery<string>(query).SingleOrDefault();
			if (!string.IsNullOrEmpty(arc)) { arcs.Add(arc); }
			return arcs;
		}//GetArc()

		/// <summary>
		/// Gets the pre-defined comment text to be used on TD22, given the contact result (and reference result if applicable).
		/// </summary>
		/// <param name="testMode">True if running in test mode.</param>
		/// <param name="contactResult">
		/// Result of contact, from the Result column of the RTAC_REF_ContactResult table in BSYS.
		/// </param>
		/// <param name="referenceResult">
		/// Result of talking to the reference (if applicable), from the Result column of the RTAC_REF_RefContact table in BSYS.
		/// This value is only checked if the contact result is one that has a foreign key relationship to a reference result.
		/// </param>
		public static string GetCommentText(bool testMode, string contactResult, string referenceResult)
		{
			string query = string.Format("SELECT MessageText FROM RTAC_REF_ContactResult WHERE Result = '{0}'", contactResult);
			string commentText = BsysDataContext(testMode).ExecuteQuery<string>(query).SingleOrDefault() ?? "";
			if (GetContactResultsForReferences(testMode).Contains(contactResult))
			{
				query = string.Format("SELECT MessageText FROM RTAC_REF_RefContact WHERE Contact = '{0}' AND Result = '{1}'", contactResult, referenceResult);
				commentText += BsysDataContext(testMode).ExecuteQuery<string>(query).SingleOrDefault() ?? "";
			}
			return commentText;
		}//GetCommentText()

		/// <summary>
		/// Gets a list of valid contact results from the Result column of the RTAC_REF_ContactResult table in BSYS.
		/// </summary>
		/// <param name="testMode">True if running in test mode.</param>
		public static List<string> GetContactResults(bool testMode)
		{
			string query = "SELECT Result FROM RTAC_REF_ContactResult";
			return BsysDataContext(testMode).ExecuteQuery<string>(query).ToList();
		}//GetContactResults()

		private static List<string> _contactResultsForReferences;
		/// <summary>
		/// Gets the subset of contact results that apply to reference contacts
		/// (i.e., that have a foreign key relationship to a reference result).
		/// </summary>
		/// <param name="testMode">True if running in test mode.</param>
		public static List<string> GetContactResultsForReferences(bool testMode)
		{
			if (_contactResultsForReferences == null)
			{
				StringBuilder queryBuilder = new StringBuilder();
				queryBuilder.Append("SELECT DISTINCT A.Result");
				queryBuilder.Append(" FROM RTAC_REF_ContactResult A");
				queryBuilder.Append(" INNER JOIN RTAC_REF_RefContact B");
				queryBuilder.Append(" ON A.Result = B.Contact");
				_contactResultsForReferences = BsysDataContext(testMode).ExecuteQuery<string>(queryBuilder.ToString()).ToList();
			}
			return _contactResultsForReferences;
		}//GetContactResultsForReferences()

		/// <summary>
		/// Gets a list of applicable reference results from the Result column of the RTAC_REF_RefContact table in BSYS,
		/// given a contact result that has a foreign key relationship to a reference result.
		/// </summary>
		/// <param name="testMode">True if running in test mode.</param>
		/// <param name="contactResult">The contact result that has a foreign key relationship to a reference result.</param>
		public static List<string> GetReferenceResults(bool testMode, string contactResult)
		{
			string query = string.Format("SELECT Result FROM RTAC_REF_RefContact WHERE Contact = '{0}'", contactResult);
			return BsysDataContext(testMode).ExecuteQuery<string>(query).ToList();
		}//GetReferenceResults()
	}//class
}//namespace
