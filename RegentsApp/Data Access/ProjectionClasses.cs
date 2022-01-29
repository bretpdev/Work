using System;

namespace RegentsApp
{
    public class StudentData
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }
        public int GenderCode { get; set; }
        public int EthnicityCode { get; set; }
        public string StateStudentId { get; set; }
        public Nullable<bool> HasCriminalRecord { get; set; }
        public Nullable<bool> IsUsCitizen { get; set; }
        public Nullable<bool> IsEligibleForFederalAid { get; set; }
        public Nullable<bool> IntendsToApplyForFederalAid { get; set; }
        public int HowHearAboutCode { get; set; }
        public Nullable<bool> UESP { get; set; }
    }

    public class StudentAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public int StateCode { get; set; }
        public string Zip { get; set; }
    }

    public class UserLogin
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }

    public class CheckUserName
    {
        public string UserName { get; set; }
    }

    public class CheckUserNameForgot
    {
        public string UserName { get; set; }
        public int SecurityCode1 { get; set; }
        public int SecurityCode2 { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
    }

    public class CheckEmail
    {
        public string Email { get; set; }
    }

    public class CheckUserNameByEmail
    {
        public string UserName { get; set; }
        public int SecurityCode1 { get; set; }
        public int SecurityCode2 { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
    }

    public class GetQuestions
    {
        public string Description { get; set; }
    }

    public class HasBeenSubmitted
    {
        public string UserName { get; set; }
        public DateTime SubmitDate { get; set; }
    }

    public class SchoolInfo
    {
        public string UserName { get; set; }
        public Nullable<bool> HighSchoolIsInUtah { get; set; }
        public string HighSchoolCeebCode { get; set; }
        public Nullable<bool> GraduationYear { get; set; }
        public int GradeLevel { get; set; }
        public double HighSchoolCumulativeGPA { get; set; }
        public int CollegeCode { get; set; }
        public string JuniorHighCeebCode { get; set; }
        public Nullable<bool> HaveAttendedOther { get; set; }
    }

    public class SchoolNames
    {
        public string Name { get; set; }
        public string CeebCode { get; set; }
    }

    public class CompositeACT
    {
        public int Code { get; set; }
        public string Description { get; set; }
    }

    public class EnglishACT
    {
        public int Code { get; set; }
        public string Description { get; set; }
    }

    public class MathACT
    {
        public int Code { get; set; }
        public string Description { get; set; }
    }

    public class ScienceACT
    {
        public int Code { get; set; }
        public string Description { get; set; }
    }

    public class ReadingACT
    {
        public int Code { get; set; }
        public string Description { get; set; }
    }

    public class ClassData
    {
        public string Username { get; set; }
        public int ClassTypeCode { get; set; }
        public int CollegeCode { get; set; }
        public int SequenceNo { get; set; }
        public int ClassTitleCode { get; set; }
        public string GradeLevel { get; set; }
        public string WeightCode { get; set; }
        public double Credits { get; set; }
        public string AcademicYear { get; set; }
    }

    public class GradeData
    {
        public string Username { get; set; }
        public int ClassTypeCode { get; set; }
        public int ClassSequenceNo { get; set; }
        public int Term { get; set; }
        public int GradeCode { get; set; }
    }

    public class ClassList
    {
        public int Code { get; set; }
        public string Description { get; set; }
    }



    /// <summary>
    /// Contains a flattened projection of an applicant, sliced at the class level.
    /// </summary>
    public class PdfRecord
    {
        public string StateStudentId { get; set; }
        public DateTime ApplicationSubmitDate { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Ethnicity { get; set; }
        public bool HasCriminalRecord { get; set; }
        public bool IsUsCitizen { get; set; }
        public bool IntendsToApplyForFederalAid { get; set; }
        public string HowHearAbout { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string EmailAddress { get; set; }
        public bool HighSchoolIsInUtah { get; set; }
        public string HighSchoolName { get; set; }
        public string HighSchoolDistrict { get; set; }
        public string HighSchoolCity { get; set; }
        public string HighSchoolState { get; set; }
        public string HighSchoolZip { get; set; }
        public bool HighSchoolGraduationYear { get; set; }
        public double HighSchoolCumulativeGpa { get; set; }
        public string College { get; set; }
        public string JuniorHighName { get; set; }
        public string JuniorHighDistrict { get; set; }
        public string JuniorHighCity { get; set; }
        public string JuniorHighState { get; set; }
        public string JuniorHighZip { get; set; }
        public bool HasAttendedOtherHighSchool { get; set; }
        public string ActCompositeScore { get; set; }
        public string ActEnglishScore { get; set; }
        public string ActMathScore { get; set; }
        public string ActScienceScore { get; set; }
        public string ActReadingScore { get; set; }
        public string ClassType { get; set; }
        public int ClassNumber { get; set; }
        public string ClassName { get; set; }
        public string GradeLevel { get; set; }
        public string ClassWeight { get; set; }
        public double Credits { get; set; }
        public string Grade1 { get; set; }
        public string Grade2 { get; set; }
        public string Grade3 { get; set; }
        public string Grade4 { get; set; }
        public string Grade5 { get; set; }
        public string Grade6 { get; set; }
    }


    /// <summary>
    /// Contains a flattened projection of an applicant, sliced at the class level.
    /// </summary>
    public class AllDataRecords
    {
        public string StateStudentId { get; set; }
        public DateTime ApplicationSubmitDate { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Ethnicity { get; set; }
        public bool HasCriminalRecord { get; set; }
        public bool IsUsCitizen { get; set; }
        public bool IntendsToApplyForFederalAid { get; set; }
        public string HowHearAbout { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string EmailAddress { get; set; }
        public bool HighSchoolIsInUtah { get; set; }
        public string HighSchoolName { get; set; }
        public string HighSchoolDistrict { get; set; }
        public string HighSchoolCity { get; set; }
        public string HighSchoolState { get; set; }
        public string HighSchoolZip { get; set; }
        public bool HighSchoolGraduationYear { get; set; }
        public double HighSchoolCumulativeGpa { get; set; }
        public string College { get; set; }
        public string JuniorHighName { get; set; }
        public string JuniorHighDistrict { get; set; }
        public string JuniorHighCity { get; set; }
        public string JuniorHighState { get; set; }
        public string JuniorHighZip { get; set; }
        public bool HasAttendedOtherHighSchool { get; set; }
        public string ActCompositeScore { get; set; }
        public string ActEnglishScore { get; set; }
        public string ActMathScore { get; set; }
        public string ActScienceScore { get; set; }
        public string ActReadingScore { get; set; }
        public string ClassType { get; set; }
        public int ClassNumber { get; set; }
        public string ClassName { get; set; }
        public string GradeLevel { get; set; }
        public string ClassWeight { get; set; }
        public double Credits { get; set; }
        public string Grade1 { get; set; }
        public string Grade2 { get; set; }
        public string Grade3 { get; set; }
        public string Grade4 { get; set; }
        public string Grade5 { get; set; }
        public string Grade6 { get; set; }
    }
}