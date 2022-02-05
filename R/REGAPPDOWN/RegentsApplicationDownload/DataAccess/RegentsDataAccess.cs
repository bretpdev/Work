using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using RegentsScholarshipBackEnd;

namespace RegentsApplicationDownload.DataAccess
{
	public class RegentsDataAccess
	{
		private static DataContext _regentsDataContext;
		protected static DataContext RegentsDataContext
		{
			get
			{
				if (_regentsDataContext == null)
				{
					_regentsDataContext = new DataContext(RegentsScholarshipBackEnd.DataAccess.ConnectionString);
				}
				return _regentsDataContext;
			}
		}

		/// <summary>
		/// Selected information on students that already exist in the Regents' Scholarship database.
		/// </summary>
		public static List<ExistingStudent> ExistingStudents
		{
			get
			{
				string query = "SELECT StateStudentId, FirstName, LastName, DateOfBirth FROM Student";
				return RegentsDataContext.ExecuteQuery<ExistingStudent>(query).ToList();
			}
		}

		public static void SetHowHearAbout(string stateStudentId, string howHearAbout)
		{
			//Get the matching HowTheyHearAboutUsCode, if there is a match.
			string query = string.Format("SELECT Code FROM HearAboutSourceLookup WHERE Description = '{0}'", howHearAbout.Trim());
			int howHearAboutCode = RegentsDataContext.ExecuteQuery<int>(query).SingleOrDefault();
			//If there is no match, get the code for "Other"
			//and set the HowTheyHearAboutUsText as well;
			//otherwise, just set the code.
			if (howHearAboutCode == 0)
			{
				query = "SELECT Code FROM HearAboutSourceLookup WHERE Description LIKE '%Other%'";
				howHearAboutCode = RegentsDataContext.ExecuteQuery<int>(query).Single();
				query = string.Format("UPDATE ScholarshipApplication SET HowTheyHearAboutUsCode = {0}, HowTheyHearAboutUsText = '{1}' WHERE StateStudentId = '{2}'", howHearAboutCode, howHearAbout, stateStudentId);
				if (RegentsDataContext.ExecuteCommand(query) == 0)
				{
					throw new RegentsInvalidDataException("Error updating HowTheyHearAboutUsCode and HowTheyHearAboutUsText.");
				}
			}
			else
			{
				query = string.Format("UPDATE ScholarshipApplication SET HowTheyHearAboutUsCode = {0} WHERE StateStudentId = '{1}'", howHearAboutCode, stateStudentId);
				if (RegentsDataContext.ExecuteCommand(query) == 0)
				{
					throw new RegentsInvalidDataException("Error updating HowTheyHearAboutUsCode.");
				}
			}
		}//SetHowHearAbout()

		public static void SetPlannedCollege(string stateStudentId, string collegeName)
		{
			//See if the college is already in the lookup table.
			string collegeCodeQuery = string.Format("SELECT Code FROM CollegeLookup WHERE Description = '{0}'", collegeName);
			Nullable<int> collegeCode = RegentsDataContext.ExecuteQuery<Nullable<int>>(collegeCodeQuery).DefaultIfEmpty(null).SingleOrDefault();
			//Make a new lookup record if needed.
			if (collegeCode == null)
			{
				string collegeLookupCommand = string.Format("INSERT INTO CollegeLookup (Description, IsInDefaultList) VALUES ('{0}', 0)", collegeName);
				RegentsDataContext.ExecuteCommand(collegeLookupCommand);
				collegeCode = RegentsDataContext.ExecuteQuery<int>(collegeCodeQuery).SingleOrDefault();
			}

			//Update the ScholarshipApplication table with the planned college code.
			string updateCommand = string.Format("UPDATE ScholarshipApplication SET PlannedCollegeToAttendCode = {0} WHERE StateStudentId = '{1}'", collegeCode, stateStudentId);
			RegentsDataContext.ExecuteCommand(updateCommand);
		}//SetPlannedCollege()
	}//class
}//namespace
