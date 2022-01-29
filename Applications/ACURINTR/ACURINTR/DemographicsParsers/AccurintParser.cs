using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace ACURINTR.DemographicsParsers
{
	class AccurintParser : IDemographicsParser
	{
		private ReflectionInterface RI { get; set; }

		public AccurintParser(ReflectionInterface ri)
		{
			RI = ri;
		}

		/// <summary>
		/// Creates a AccurintRDemographics object and gathers other associated details from an ACURINTR queue task.
		/// The screen MUST be on LP9AC for an open ACURINTR task before calling this method.
		/// </summary>
		public QueueTask Parse()
		{
			QueueTask task = new QueueTask(RejectAction.Sources.ACCURINT, SystemCode.Sources.ACCURINT);

			//The assumption is that if the second line of comments is blank,
			//this task is a phone number; otherwise, it's an address.
			if (RI.CheckForText(13, 11, "__________"))
			{
				task.OriginalDemographicsText = RI.GetText(12, 11, 58);
				task.Demographics = ParsePhone(task.OriginalDemographicsText);
			}
			else
			{
				task.OriginalDemographicsText = RI.GetText(12, 11, 58) + RI.GetText(13, 11, 58);
				task.Demographics = ParseAddress(task.OriginalDemographicsText);
			}
			if (RI.CheckForText(1, 71, "QUEUE"))
			{
				//Use the borrower's SSN.
				task.Demographics.Ssn = RI.GetText(17, 70, 9);
				task.Demographics.AccountNumber = RI.GetText(17, 52, 12).Replace(" ", "");
			}
			else //1, 67, "WORKGROUP"
			{
				//Use the target SSN.
				task.Demographics.Ssn = RI.GetText(5, 70, 9);
				task.Demographics.AccountNumber = "";
			}

			return task;
		}

		/// <summary>
		/// Takes a Demographic string and parses it into an AccurintRDemographics object
		/// </summary>
		/// <param name="commentText">original address info from the session</param>
		/// <returns>AccurintRDemographics object</returns>
		private AccurintRDemographics ParseAddress(string commentText)
		{
			const string REGEX_ZIP = @"^(\d{5}-\d{4}|\d{5}|\d{9})$"; //Matches 5-4, 5, and 9.
			AccurintRDemographics accurintDemographics = new AccurintRDemographics();
			try
			{
				//See if the comment text contains both the street address and the city, state, zip.
				string commentCityStateZip = string.Empty;
				if (commentText.Contains('_')) //Both halves are there.
				{
					accurintDemographics.Address1 = commentText.Substring(0, commentText.IndexOf('_')).Trim();
					commentCityStateZip = commentText.Substring(commentText.LastIndexOf('_') + 1).Trim();
				}
				else //One half is missing. Check the screen to see if it's the street address.
				{
					if (RI.CheckForText(12, 11, " "))
						commentCityStateZip = commentText.Trim();
					else
						accurintDemographics.Address1 = commentText.Trim();
				}
				
				int commentIndex = General.FindEndOfCommentText(commentCityStateZip);
				while (commentIndex >= 0) //Pick out the zip code, state, and city from the comment text, working backwards from the end.
				{
					StringBuilder addressElementBuilder = new StringBuilder();
					//Back the comment index pointer up past any blank spaces and commas.
					while (commentIndex > 0 && (commentCityStateZip[commentIndex] == ' ' || commentCityStateZip[commentIndex] == ','))
						commentIndex--;
					//Read back to the next space or to the beginning of the string, whichever comes first.
					for (; commentIndex >= 0 && commentCityStateZip[commentIndex] != ' '; commentIndex--)
					{
						addressElementBuilder.Insert(0, commentCityStateZip[commentIndex]);
					}
					string addressElement = addressElementBuilder.ToString();
					//Determine what this element is and assign it to the appropriate AccurintRDemographics property.
					if (Regex.IsMatch(addressElement, REGEX_ZIP))
						accurintDemographics.ZipCode = addressElement.Substring(0, 5);
					else if (addressElement.Length < 2 || addressElement == "FC" || DataAccess.StateCodes().Contains(addressElement))
						accurintDemographics.State = addressElement;
					else
					{
						for (; commentIndex >= 0; commentIndex--)
						{
							addressElementBuilder.Insert(0, commentCityStateZip[commentIndex]);
						}
						accurintDemographics.City = addressElementBuilder.ToString().Replace(",", "");
					}
				}
			}
			catch (IndexOutOfRangeException ex)
			{
				throw new ParseException("Could not decipher demographic information.", ex, accurintDemographics.AccountNumber, RejectAction.Sources.ACCURINT, SystemCode.Sources.ACCURINT, commentText);
			}
			return accurintDemographics;
		}

		/// <summary>
		/// Takes the information from the session and converts it to a phone number.  That is returned in an AccurintRDemographics object.
		/// </summary>
		/// <param name="commentText">session phone number info</param>
		/// <returns>AccurintRDemographics</returns>
		private AccurintRDemographics ParsePhone(string commentText)
		{
			AccurintRDemographics accurintDemographics = new AccurintRDemographics();
			//Get the phone number from the comment text.
			try
			{
				accurintDemographics.PrimaryPhone = commentText.Split(' ')[0];
			}
			catch (IndexOutOfRangeException ex)
			{
				throw new ParseException("Could not decipher demographic information.", ex, accurintDemographics.AccountNumber, RejectAction.Sources.ACCURINT, SystemCode.Sources.ACCURINT, commentText);
			}
			return accurintDemographics;
		}
	}
}
