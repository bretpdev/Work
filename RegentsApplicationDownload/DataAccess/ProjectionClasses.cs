using System;

namespace RegentsApplicationDownload.DataAccess
{
    public class Act
    {
        public string Type { get; set; }
        public double Score { get; set; }
    }//Act

    public class Applicant
    {
        public string Username { get; set; }
        public DateTime SubmitDate { get; set; }
    }//Applicant

    public class ClassList
    {
        public string ClassType { get; set; }
        public int SequenceNo { get; set; }
        public string ClassTitle { get; set; }
        public string GradeLevel { get; set; }
        public string Weight { get; set; }
        public double Credits { get; set; }
		public string ConcurrentEnrollmentCollege { get; set; }
    }//ClassList

    public class Grade
    {
        public string ClassType { get; set; }
        public int ClassSequenceNo { get; set; }
        public int Term { get; set; }
        public string LetterGrade { get; set; }
    }//Grade

    public class ScholarshipApplication
    {
        public bool HighSchoolIsInUtah { get; set; }
        public string HighSchoolCeebCode { get; set; }
        public double HighSchoolCumulativeGpa { get; set; }
        public string College { get; set; }
        public string JuniorHighCeebCode { get; set; }
        public bool HaveAttendedOther { get; set; }
		public int GradeLevel { get; set; }
    }//ScholarshipApplication

    public class Student
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Ethnicity { get; set; }
        public string StateStudentId { get; set; }
        public bool HasCriminalRecord { get; set; }
        public bool IsUsCitizen { get; set; }
        public bool IsEligibleForFederalAid { get; set; }
        public bool IntendsToApplyForFederalAid { get; set; }
        public string HowHearAbout { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
		public bool UESP { get; set; }
    }//Student

    public class StudentAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public bool IsValid { get; set; }
    }//StudentAddress

    //Info for existing students in the Regents database
    public class ExistingStudent
    {
        public string StateStudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }//ExistingStudent
}//namespace
