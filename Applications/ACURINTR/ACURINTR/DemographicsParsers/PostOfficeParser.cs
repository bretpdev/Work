using System;
using System.Linq;
using System.Text;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace ACURINTR.DemographicsParsers
{
	class PostOfficeParser : IDemographicsParser
	{
		private ReflectionInterface RI { get; set; }
		public PostOfficeParser(ReflectionInterface ri)
		{
			RI = ri;
		}

		/// <summary>
		/// Creates a AccurintRDemographics object and gathers other associated details from a POST OFFICE queue task.
		/// The screen MUST be on LP5FC for an open PDEM task before calling this method.
		/// </summary>
		public QueueTask Parse()
		{
			string accountNumber = "";
			string ssn = "";
			if (RI.CheckForText(1, 71, "QUEUE"))
			{
				ssn = RI.GetText(17, 70, 9); //Use the borrower's SSN.
				accountNumber = RI.GetText(17, 52, 12).Replace(" ", "");
			}
			else //1, 67, "WORKGROUP"
				ssn = RI.GetText(5, 70, 9); //Use the target SSN.
			QueueTask task = new QueueTask(RejectAction.Sources.POST_OFFICE, SystemCode.Sources.POST_OFFICE);
			task.OriginalDemographicsText = (RI.GetText(12, 11, 58) + RI.GetText(13, 11, 58) + RI.GetText(14, 11, 58) + RI.GetText(15, 11, 26)).TrimEnd('_').TrimEnd(' ');//Get the comment text to determine how to proceed.
			if (task.OriginalDemographicsText.Contains("TEMPORARILY AWAY")) //Skip the task if the new address is "TEMPORARILY AWAY".
			{
				string message = RejectAction.Sources.POST_OFFICE + " demographics information is invalid.";
				throw new QueueTaskException(message, ssn, RejectAction.Sources.POST_OFFICE, SystemCode.Sources.POST_OFFICE, task.OriginalDemographicsText);
			}

			string postOfficeComment = task.OriginalDemographicsText.Substring(0, task.OriginalDemographicsText.IndexOf(':') + 1); //Parse based on the post office comment.
			if (postOfficeComment.Contains("NO MATCH ON OLD ADDRESS:") || postOfficeComment.Contains("DATA ERROR"))
			{
				//The new address is the part after the first comma.
				string newAddress = task.OriginalDemographicsText.Substring(task.OriginalDemographicsText.IndexOf(',') + 1).Trim();
				try
				{
					task.Demographics = ParseAddressOnly(newAddress);
				}
				catch (IndexOutOfRangeException)
				{
					throw new ParseException(task.OriginalDemographicsText, ssn, RejectAction.Sources.POST_OFFICE, SystemCode.Sources.POST_OFFICE, newAddress);
				}
			}
			else if (postOfficeComment.Contains("NO MATCH ON LAST NAME:"))
			{
				//The name and address are the part after the post office comment.
				string nameAndAddress = task.OriginalDemographicsText.Substring(task.OriginalDemographicsText.IndexOf(':') + 1);
				try
				{
					task.Demographics = ParseNameAndAddress(nameAndAddress);
				}
				catch (IndexOutOfRangeException)
				{
					throw new ParseException(task.OriginalDemographicsText, ssn, RejectAction.Sources.POST_OFFICE, SystemCode.Sources.POST_OFFICE, nameAndAddress);
				}
			}

			task.Demographics.AccountNumber = accountNumber;
			task.Demographics.Ssn = ssn;
			return task;
		}

		/// <summary>
		/// Gets an address from a comment in the session.
		/// </summary>
		/// <param name="newAddress"></param>
		/// <returns>Address in an AccurintRDemographics object</returns>
		private AccurintRDemographics ParseAddressOnly(string newAddress)
		{
			AccurintRDemographics taskDemographics = new AccurintRDemographics();
			int commentIndex = General.FindEndOfCommentText(newAddress); //Pick out the address elements from the comment text, working backwards from the end.
			StringBuilder elementBuilder = new StringBuilder();
			for (; newAddress[commentIndex] != ' '; commentIndex--) //Zip code--read back until there's a space.
			{
				elementBuilder.Insert(0, newAddress[commentIndex]);
			}
			taskDemographics.ZipCode = elementBuilder.ToString().Substring(0, 5);
			while (newAddress[commentIndex] == ' ')
			{
				commentIndex--;
			}

			elementBuilder = new StringBuilder();
			for (; newAddress[commentIndex] != ' '; commentIndex--) //State--read back until there's a space.
			{
				elementBuilder.Insert(0, newAddress[commentIndex]);
			}
			taskDemographics.State = elementBuilder.ToString();
			while (newAddress[commentIndex] == ' ') 
			{
				commentIndex--; 
			}

			elementBuilder = new StringBuilder();
			for (; newAddress[commentIndex] != ','; commentIndex--) //City--read back until there's a comma.
			{
				elementBuilder.Insert(0, newAddress[commentIndex]);
			}
			taskDemographics.City = elementBuilder.ToString();
			while (newAddress[commentIndex] == ',' || newAddress[commentIndex] == ' ') 
			{ 
				commentIndex--; 
			}

			elementBuilder = new StringBuilder();
			for (; commentIndex >= 0; commentIndex--) //Street--read back to the beginning.
			{
				elementBuilder.Insert(0, newAddress[commentIndex]);
			}
			taskDemographics.Address1 = elementBuilder.ToString().Trim();
			return taskDemographics;
		}

		/// <summary>
		/// Gets an address and name from a comment in the session.
		/// </summary>
		/// <param name="nameAndAddress"></param>
		/// <returns></returns>
		private AccurintRDemographics ParseNameAndAddress(string nameAndAddress)
		{
			//The address starts after the second comma.
			int firstComma = nameAndAddress.IndexOf(',');
			int secondComma = nameAndAddress.IndexOf(',', firstComma + 1);
			string newAddress = nameAndAddress.Substring(secondComma + 1);
			AccurintRDemographics taskDemographics = ParseAddressOnly(newAddress);

			//Check that the name in the comment matches the name in the task.
			string commentLastName = nameAndAddress.Substring(0, firstComma).Trim();
			string taskFulLastName = RI.GetText(17, 6, 38);
			string taskFirstName = taskFulLastName.Substring(0, taskFulLastName.IndexOf(' '));
			string taskMiddleInitial = taskFulLastName.Substring(taskFirstName.Length + 1, 2).Trim();
			int charactersBetweenFirstAndLastName = (taskMiddleInitial.Length == 1 ? 3 : 1);
			string taskLastName = taskFulLastName.Substring(taskFirstName.Length + charactersBetweenFirstAndLastName);
			if (commentLastName != taskLastName)
			{
				//See if the first name matches on the first three characters, which indicates the last name may have changed.
				string commentFirstName = nameAndAddress.Substring(firstComma + 1, secondComma - firstComma - 1).Trim();
				if (commentFirstName.SafeSubString(0, 3) == taskFirstName.SafeSubString(0, 3))
				{
					taskDemographics.FirstName = commentFirstName;
					taskDemographics.LastName = commentLastName;
				}
				else
				{
					string message = "Forwarding address does not belong to a UHEAA borrower.";
					string ssn = RI.GetText(17, 70, 9);
					throw new QueueTaskException(message, ssn, RejectAction.Sources.POST_OFFICE, SystemCode.Sources.POST_OFFICE, nameAndAddress);
				}
			}

			return taskDemographics;
		}
	}
}
