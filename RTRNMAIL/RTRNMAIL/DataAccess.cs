using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using Q;

namespace RTRNMAIL
{
	class DataAccess : DataAccessBase
	{
		private static Dictionary<char, char> MYLAUGHTER;

		static DataAccess()
		{
			MYLAUGHTER = new Dictionary<char, char>();
			MYLAUGHTER.Add('M', '0');
			MYLAUGHTER.Add('Y', '9');
			MYLAUGHTER.Add('L', '8');
			MYLAUGHTER.Add('A', '7');
			MYLAUGHTER.Add('U', '6');
			MYLAUGHTER.Add('G', '5');
			MYLAUGHTER.Add('H', '4');
			MYLAUGHTER.Add('T', '3');
			MYLAUGHTER.Add('E', '2');
			MYLAUGHTER.Add('R', '1');
		}

		public static List<BarcodeInfo> GetBarcodeInfos(bool testMode, string recipientId)
		{
			StringBuilder queryBuilder = new StringBuilder();
			queryBuilder.Append("SELECT RecipientId, LetterId, CreateDate");
			queryBuilder.Append(" FROM RTML_DAT_BarcodeData");
			queryBuilder.AppendFormat(" WHERE RecipientId = '{0}'", recipientId);
			queryBuilder.Append(" AND AddressInvalidatedDate IS NOT NULL");
			queryBuilder.Append(" AND ForwardingAddressUpdatedDate IS NULL");
			return BsysDataContext(testMode).ExecuteQuery<BarcodeInfo>(queryBuilder.ToString()).ToList();
		}//GetBarcodeInfos()

		public static string GetBusinessUnitForResendLetter(string letterId)
		{
			StringBuilder queryBuilder = new StringBuilder();
			queryBuilder.Append("SELECT TOP 1 DD.Unit");
			queryBuilder.Append(" FROM LTDB_DAT_DocDetail DD");
			queryBuilder.Append(" INNER JOIN LTDB_DAT_CentralPrintingDocData CP");
			queryBuilder.Append(" ON DD.DocSeqNo = CP.DocSeqNo");
			queryBuilder.AppendFormat(" WHERE DD.ID = '{0}'", letterId);
			queryBuilder.Append(" AND DD.Status = 'Active'");
			queryBuilder.Append(" AND CP.ResendMail = 1");
			queryBuilder.Append(" ORDER BY DD.DocSeqNo");
			return BsysDataContext(false).ExecuteQuery<string>(queryBuilder.ToString()).SingleOrDefault();
		}//GetBusinessUnitForResendLetter()

		public static string GetEmailRecipients(bool testMode, string typeKey)
		{
			string query = string.Format("SELECT WinUName + '@utahsbr.edu' FROM GENR_REF_MiscEmailNotif WHERE TypeKey = '{0}'", typeKey);
			IEnumerable<string> userNames = BsysDataContext(testMode).ExecuteQuery<string>(query);
			return string.Join(";", userNames.ToArray());
		}//GetEmailRecipients()

		private static Dictionary<string, string> _secondaryUnits;
		/// <summary>
		/// Street address secondary units and their abbreviations (e.g., "Apartment" and "APT").
		/// Key = spelled-out unit.
		/// Value = abbreviation.
		/// </summary>
		public static Dictionary<string, string> GetSecondaryUnits(bool testMode)
		{
			if (_secondaryUnits == null)
			{
				_secondaryUnits = new Dictionary<string, string>();
				string query = "SELECT Abbreviation, FullText FROM GENR_LST_StandardAddressAbbreviations WHERE TypeKey = 'Secondary Unit' ORDER BY FullText";
				foreach (AddressTerm term in BsysDataContext(testMode).ExecuteQuery<AddressTerm>(query))
				{
					_secondaryUnits.Add(term.Abbreviation, term.FullText);
				}
			}
			return _secondaryUnits;
		}//GetSecondaryUnits()

		private static List<string> _stateCodes;
		public static List<string> GetStateCodes()
		{
			if (_stateCodes == null)
			{
				string query = "SELECT Code FROM GENR_LST_States WHERE Domestic = 'Y'";
				_stateCodes = BsysDataContext(false).ExecuteQuery<string>(query).OrderBy(p => p).ToList();
			}
			return _stateCodes;
		}//GetStateCodes()

		private static Dictionary<string, string> _streetSuffixes;
		/// <summary>
		/// Street suffixes and their abbreviations (e.g., "Avenue" and "AVE").
		/// Key = spelled-out street suffix.
		/// Value = abbreviation.
		/// </summary>
		public static Dictionary<string, string> GetStreetSuffixes(bool testMode)
		{
			if (_streetSuffixes == null)
			{
				_streetSuffixes = new Dictionary<string, string>();
				string query = "SELECT Abbreviation, FullText FROM GENR_LST_StandardAddressAbbreviations WHERE TypeKey = 'Street Suffix' ORDER BY FullText";
				foreach (AddressTerm term in BsysDataContext(testMode).ExecuteQuery<AddressTerm>(query))
				{
					_streetSuffixes.Add(term.Abbreviation, term.FullText);
				}
			}
			return _streetSuffixes;
		}//GetStreetSuffixes()

		public static void MarkBarcodeRecordCompleted(bool testMode, BarcodeInfo barcodeInfo)
		{
			StringBuilder commandBuilder = new StringBuilder();
			commandBuilder.Append("UPDATE RTML_DAT_BarcodeData");
			commandBuilder.Append(" SET ForwardingAddressUpdatedDate = GETDATE()");
			commandBuilder.AppendFormat(" WHERE RecipientId = '{0}'", barcodeInfo.RecipientId);
			commandBuilder.AppendFormat(" AND LetterId = '{0}'", barcodeInfo.LetterId);
			commandBuilder.AppendFormat(" AND DATEDIFF(d, CreateDate, '{0}') = 0", barcodeInfo.CreateDate.ToString("yyyy-MM-dd"));
			BsysDataContext(testMode).ExecuteCommand(commandBuilder.ToString());
		}//MarkBarcodeRecordCompleted()

		public static string StopLaughing(string myLaughter)
		{
			StringBuilder cryBuilder = new StringBuilder();
			foreach (char letter in myLaughter)
			{
				if (MYLAUGHTER.ContainsKey(letter))
				{
					cryBuilder.Append(MYLAUGHTER[letter]);
				}
				else
				{
					cryBuilder.Append(letter);
				}
			}
			return cryBuilder.ToString();
		}//StopLaughing()

		private class AddressTerm
		{
			public string Abbreviation { get; set; }
			public string FullText { get; set; }
		}//AddressTerm
	}//class
}//namespace
