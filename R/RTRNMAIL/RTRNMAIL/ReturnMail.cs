using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Q;
using AesSystem = Q.Common.AesSystem;
using Key = Q.ReflectionInterface.Key;

namespace RTRNMAIL
{
	public class ReturnMail : ScriptBase
	{
		private SessionAccess _sessionAccess;

		public ReturnMail(ReflectionInterface ri)
			: base(ri, "RTRNMAIL")
		{
			_sessionAccess = new SessionAccess(ri, ScriptID);
		}

		public override void Main()
		{
			//Loop through as many letters as the user cares to process.
			for (List<BarcodeInfo> barcodeInfos = GetBarcodeInfos(); barcodeInfos != null; barcodeInfos = GetBarcodeInfos())
			{
				BarcodeInfo firstBarcode = barcodeInfos.First();
				//Get personal info.
				IdentifyingInfo recipientInfo = _sessionAccess.GetRecipientInfo(firstBarcode.RecipientId);
				if (recipientInfo == null)
				{
					string message = "The ACS Encryption Code / SSN / Account Number / Reference ID provided could not be found.";
					MessageBox.Show(message, ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					continue;
				}
				List<string> personTypes = _sessionAccess.GetPersonTypes(firstBarcode.RecipientId);

				//Prompt the user for the forwarding address.
				MailingAddress forwardingAddress = null;
				frmDemographics demographicsForm = new frmDemographics(RI.TestMode, recipientInfo, out forwardingAddress);
				if (demographicsForm.ShowDialog() != DialogResult.OK) { break; }

				//Create a queue task for the Automate Demographics script.
				string queueComment = string.Join(",", new string[] { forwardingAddress.Address1, forwardingAddress.Address2, forwardingAddress.City, forwardingAddress.State, forwardingAddress.Zip, forwardingAddress.Country, "", "", "" });
				AddQueueTaskInLP9O(recipientInfo.SSN, "DFTDEMOS", queueComment);

				//Do per-letter processing.
				AesSystem applicableSystems = _sessionAccess.DetermineApplicableSystems(firstBarcode.RecipientId);
				bool oneLinkIsApplicable = ((applicableSystems & AesSystem.OneLink) == AesSystem.OneLink);
				bool compassIsApplicable = ((applicableSystems & AesSystem.Compass) == AesSystem.Compass);
				foreach (BarcodeInfo info in barcodeInfos)
				{
					//Create a queue task to re-send the letter if needed.
					string businessUnit = DataAccess.GetBusinessUnitForResendLetter(info.LetterId);
					if (!string.IsNullOrEmpty(businessUnit))
					{
						bool businessUnitIsOneLink = (businessUnit == "Loan Management" || businessUnit == "Postclaim Services");
						bool businessUnitIsCompass = (businessUnit == "Account Services" || businessUnit == "Borrower Services");
						string dateDue = DateTime.Now.AddDays(1).ToString("MM/dd/yyyy");
						string resendComment = string.Format("Letter {0} needs to be resent.", info.LetterId);
						if (oneLinkIsApplicable && businessUnitIsOneLink)
						{
							AddQueueTaskInLP9O(recipientInfo.SSN, "COLRTMAL", dateDue, resendComment);
						}
						else if (compassIsApplicable && businessUnitIsCompass)
						{
							Common.CompassCommentScreenResults result = ATD22AllLoansBackedUpWithATD37FirstApp(recipientInfo.SSN, "RMLS", DateTime.Now.AddDays(1), resendComment, false);
							if (result != Common.CompassCommentScreenResults.CommentAddedSuccessfully && result != Common.CompassCommentScreenResults.SSNNotFoundOnSystem)
							{
								string failMessage = string.Format("A queue task could not be created to resend the letter. Please let {0} know that the letter needs to be re-sent tomorrow.", businessUnit);
								MessageBox.Show(failMessage, ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Error);
								break;
							}//if
						}//if/else
					}//if

					//Mark the database record as complete.
					DataAccess.MarkBarcodeRecordCompleted(RI.TestMode, info);
				}//foreach
			}//for
		}//Main()

		private List<BarcodeInfo> GetBarcodeInfos()
		{
			string recipientId = GetNextRecipient();
			if (recipientId == null) { return null; }
			List<BarcodeInfo> infos = DataAccess.GetBarcodeInfos(RI.TestMode, recipientId);
			while (infos.Count == 0)
			{
				string message = string.Format("{0} was not found in the database of returned mail. Either the barcode was not scanned and invalidated, or the address has already been updated.", recipientId);
				MessageBox.Show(message, ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Information);
				recipientId = GetNextRecipient();
				if (recipientId == null) { return null; }
				infos = DataAccess.GetBarcodeInfos(RI.TestMode, recipientId);
			}
			return infos;
		}//GetBarcodeInfos()

		private string GetNextRecipient()
		{
			//Prompt the user for a recipient ID.
			string recipientId = null;
			using (frmReturnMail rm = new frmReturnMail(RI))
			{
				if (rm.ShowDialog() == DialogResult.OK) { recipientId = rm.RecipientId; }
			}
			if (recipientId != null)
			{
				//De-obfuscate the recipient ID if it's not a reference and it's not numeric.
				bool idIsReference = (recipientId.StartsWith("RF@") || recipientId.StartsWith("P"));
				bool idIsObfuscated = (!idIsReference && !recipientId.IsNumeric());
				if (idIsObfuscated)
				{
					recipientId = DataAccess.StopLaughing(recipientId);
				}
			}
			return recipientId;
		}//GetNextRecipient()
	}//class
}//namespace
