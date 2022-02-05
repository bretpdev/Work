using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace ACURINTR
{
	public class QueueTask
	{
		public enum DemographicType { Address, Phone, Email }

		private List<RejectAction> RejectActions { get; set; }

		#region Properties
		public string ActivityType { get; set; }
		public string AdditionalInfo { get; set; }
		public string CompassSourceCode { get; set; }
		public string ContactType { get; set; }
		public string DemographicsSource { get; set; }
		public bool IsClosed { get; set; }
		public string LocateType { get; set; }
		public string OneLinkSourceCode { get; set; }
		public string OriginalDemographicsText { get; set; }
		public int PageNumber { get; set; }
		public string PdemSource { get; set; }
		public DateTime PdemVerificationDate { get; set; }
		public string SystemSource { get; set; }

		public bool HasAddress
		{
			get
			{
				if (Demographics.Address1.IsPopulated() || Demographics.Address2.IsPopulated() || Demographics.City.IsPopulated()
				   || Demographics.State.IsPopulated() || Demographics.Country.IsPopulated() || Demographics.ZipCode.IsPopulated())
					return true;
				return false;
			}
		}

		public bool HasAltPhone
		{
			get
			{
				return Demographics.AlternatePhone.IsPopulated();
			}
		}

		public bool HasEmail
		{
			get
			{
				return Demographics.EmailAddress.IsPopulated();
			}
		}

		public bool HasHomePhone
		{
			get
			{
				return Demographics.PrimaryPhone.IsPopulated();
			}
		}

		private AccurintRDemographics _demographics;
		public AccurintRDemographics Demographics
		{
			get { return _demographics; }
			set
			{
				const string VALID_CHARS = "[A-Z0-9/ ]";
				_demographics = value;
				if (_demographics.Address1 != null)
				{
					//Strip special characters out of the address fields.
					StringBuilder addressBuilder = new StringBuilder();
					foreach (char addressCharacter in _demographics.Address1)
					{
						if (System.Text.RegularExpressions.Regex.IsMatch(addressCharacter.ToString(), VALID_CHARS, RegexOptions.IgnoreCase))
							addressBuilder.Append(addressCharacter.ToString());
					}
					_demographics.Address1 = addressBuilder.ToString();
				}
				if (_demographics.Address2 != null)
				{
					StringBuilder addressBuilder = new StringBuilder();
					foreach (char addressCharacter in _demographics.Address2)
					{
						if (System.Text.RegularExpressions.Regex.IsMatch(addressCharacter.ToString(), VALID_CHARS, RegexOptions.IgnoreCase))
							addressBuilder.Append(addressCharacter.ToString());
					}
					_demographics.Address2 = addressBuilder.ToString();
				}
			}
		}
		#endregion Properties

		/// <summary>
		/// Creates a QueueTask object with ActivityType, ContactType, LocateType, CompassSourceCode, and OneLinkSourceCode
		/// filled in from the database, with other queue task-specific properties to be set by queue task parsers.
		/// </summary>
		/// <param name="demographicsSource">One of the string constants from the RejectAction.Sources class.</param>
		/// <param name="systemSource">One of the string constants from the SystemCode.Sources class.</param>
		public QueueTask(string demographicsSource, string systemSource, DataAccess da)
		{
			//The Borrower process may close a task in one processing path and then send the same task
			//into another processing path, where attempts to close the task will come up again.
			//Set a flag on the task that can be flipped once the task is closed.
			IsClosed = false;

			//Set the list of reject actions/codes from the database.
			DemographicsSource = demographicsSource;
			RejectActions = da.GetRejectActions(demographicsSource);
			if (!(RejectActions.Count > 0))
			{
				RejectAction actionObj = new RejectAction();
				actionObj.RejectReason = ACURINTR.RejectAction.RejectReasons.DEMOGRAPHICS_IS_INVALID;
				RejectActions.Add(actionObj);
			}
			//Set the system codes from the database.
			SystemSource = systemSource;
			SystemCode codes = da.SystemCodes().Where(p => p.Source == systemSource).SingleOrDefault();
			
			ActivityType = codes.ActivityType;
			CompassSourceCode = codes.CompassSourceCode;
			ContactType = codes.ContactType;
			LocateType = codes.LocateType;
			OneLinkSourceCode = codes.OneLinkSourceCode;

			//PdemVerificationDate and PageNumber must be set so that tasks can be stored in the recovery table,
			//so give them default values. PDEM parsers will update them as appropriate.
			PdemVerificationDate = DateTime.Now;
			PageNumber = 1;
		}

        public override bool Equals(object obj)
        {
            var other = obj as QueueTask;
            if (other == null)
                return false;
            return GetHashCode() == obj.GetHashCode();
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Join(";", ActivityType, AdditionalInfo, CompassSourceCode, ContactType, DemographicsSource, IsClosed, LocateType, OneLinkSourceCode, OriginalDemographicsText, PageNumber, PdemSource, PdemVerificationDate, SystemSource);
        }

        /// <summary>
        /// Gets the ARC to use in system comments for a given reject reason and demographic type.
        /// </summary>
        /// <param name="reason">One of the string constants from the RejectAction.RejectReasons class.</param>
        /// <param name="demographicType">The type of demographic information currently being processed.</param>
        /// <returns></returns>
        public string GetRejectActionCode(string reason, DemographicType demographicType)
		{
			RejectAction action = RejectActions.Where(p => p.RejectReason == reason).SingleOrDefault();
			switch (demographicType)
			{
				case DemographicType.Address:
					return action.ActionCodeAddress;
				case DemographicType.Email:
					return action.ActionCodeEmail;
				case DemographicType.Phone:
					return action.ActionCodePhone;
				default: //To keep the compiler happy...
					return null;
			}
		}
	}
}
