CREATE PROCEDURE [compfafsa].[GetUserStudentData]
	@User VARCHAR(200)
AS
	OPEN SYMMETRIC KEY UHEAA_Data_Key DECRYPTION BY CERTIFICATE
	UHEAA_Encryption_Certificate;

	SELECT DISTINCT
		SD.StudentFirstName AS FirstName,
		SD.StudentLastName AS LastName,
		CONVERT(DATE, CONVERT(VARCHAR(10), DECRYPTBYKEY(SD.StudentDateOfBirth))) AS DOB,
		S.SchoolName AS School,
		S.DistrictId AS DistrictId,
		CAST(IFD.CreatedAt AS DATE) AS AddedAt
	FROM
		compfafsa.StudentData SD
		INNER JOIN compfafsa.UserSchools US
			ON US.SchoolId = SD.MasterSchoolListId
		INNER JOIN compfafsa.Schools S
			ON S.MasterSchoolListId = US.SchoolId
		INNER JOIN compfafsa.Users U
			ON U.UserId = US.UserId
		INNER JOIN 
		(
			SELECT DISTINCT
				MSL.SchoolName AS SchoolName,
				SV.SchoolName AS VarSchoolName,
				SV.AdjustedSchoolId AS SchoolId,
				MSL.MasterSchoolListId AS MasterSchoolListId
			FROM
				compfafsa.SchoolVariations SV
				INNER JOIN compfafsa.Schools MSL
					ON MSL.SchoolId = SV.AdjustedSchoolId
			WHERE
				MSL.DeletedAt IS NULL
				AND SV.DeletedAt IS NULL
				AND SV.AdjustedSchoolId IS NOT NULL
		) MasterSchool 
			ON 
			(
				MasterSchool.MasterSchoolListId = SD.MasterSchoolListId
				OR SD.MasterSchoolListId IS NULL
			)
		INNER JOIN fafsadblod.IsirFileData IFD
			ON RTRIM(IFD.StudentFirstName) = RTRIM(SD.StudentFirstName)
			AND RTRIM(IFD.StudentLastName) = RTRIM(SD.StudentLastName)
	WHERE
		U.EmailAddress = @User
		AND U.DeletedAt IS NULL
		AND U.DeletedBy IS NULL
		AND S.DeletedAt IS NULL
		AND S.DeletedBy IS NULL
		AND SD.InvalidatedAt IS NULL
		AND SD.InvalidatedBy IS NULL
		AND IFD.HighSchoolState = 'UT'
		AND IFD.PermanentState = 'UT'

	CLOSE SYMMETRIC KEY UHEAA_Data_Key
RETURN 0
