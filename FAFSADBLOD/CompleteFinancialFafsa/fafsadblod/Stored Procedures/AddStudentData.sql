CREATE PROCEDURE [fafsadblod].[AddStudentData]
AS
BEGIN

OPEN SYMMETRIC KEY UHEAA_Data_Key DECRYPTION BY CERTIFICATE UHEAA_Encryption_Certificate;
--Invalidate students by age
UPDATE
	compfafsa.StudentData
SET
	InvalidatedAt = GETDATE(),
	InvalidatedBy = 'FAFSADBLOD'
WHERE
	CAST(CONVERT(VARCHAR(10),DECRYPTBYKEY(StudentDateOfBirth)) AS DATE) <= CAST(CAST(YEAR(GETDATE()) - 20 AS VARCHAR(4)) + '-12-01' AS DATE)
	AND InvalidatedAt IS NULL

--Load StudentData.  If school already in master list, populate, otherwise leave blank to be associated later
INSERT INTO compfafsa.StudentData(StudentFirstName, StudentLastName, StudentDateOfBirth, SchoolNameFromFile, SchoolIdFromFile, MasterSchoolListId)
SELECT DISTINCT
    RTRIM(IFD.StudentFirstName),
    RTRIM(IFD.StudentLastName),
    ENCRYPTBYKEY(Key_GUID('UHEAA_Data_Key'), CONVERT(VARCHAR(10),DECRYPTBYKEY(IFD.StudentDateOfBirth))) AS StudentDateOfBirth,
    COALESCE(UPPER(MasterSchool.SchoolName),UPPER(IFD.HighSchoolName)) AS SchoolNameFromFile,
    COALESCE(MasterSchool.SchoolId,IFD.HighSchoolCode) As SchoolId,
    MasterSchool.MasterSchoolListId
FROM 
    fafsadblod.IsirFileData IFD
    INNER JOIN 
	(
		SELECT
			CONVERT(VARCHAR(10), DECRYPTBYKEY(OriginalSsn)) AS SSN,
			MAX(CAST([TransactionNumber] AS INT)) AS [TransactionNumber]
		FROM
			fafsadblod.IsirFileData IFD
		GROUP BY
			CONVERT(VARCHAR(10), DECRYPTBYKEY(OriginalSsn)) 
	) MV
		ON MV.SSN = CONVERT(VARCHAR(10), DECRYPTBYKEY(IFD.OriginalSsn))
		AND MV.[TransactionNumber] = IFD.[TransactionNumber]
    LEFT JOIN
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
        ON MasterSchool.VarSchoolName = RTRIM(IFD.HighSchoolName)
    LEFT JOIN compfafsa.StudentData SD
        ON 
        (
            SD.MasterSchoolListId = MasterSchool.MasterSchoolListId
            OR SD.MasterSchoolListId IS NULL
        )
        AND RTRIM(SD.StudentFirstName) = RTRIM(IFD.StudentFirstName)
        AND RTRIM(SD.StudentLastName) = RTRIM(IFD.StudentLastName)
WHERE
	SD.StudentDataId IS NULL
	AND IFD.HighSchoolState = 'UT'
	AND IFD.PermanentState = 'UT'
	AND CAST(CONVERT(VARCHAR(10),DECRYPTBYKEY(IFD.StudentDateOfBirth)) AS DATE) >= CAST(CAST(YEAR(GETDATE()) - 20 AS VARCHAR(4)) + '-12-01' AS DATE)

--Set masterSchoolListId on StudentData for those schools that have been mapped since initial load
UPDATE
	SD
SET
	SD.MasterSchoolListId = MSL.MasterSchoolListId,
	SD.SchoolNameFromFile = MSL.SchoolName,
	SD.SchoolIdFromFile = MSL.SchoolId
FROM
	compfafsa.StudentData SD
	LEFT JOIN compfafsa.SchoolVariations SV
		ON SV.SchoolName = SD.SchoolNameFromFile
		AND SV.AdjustedSchoolId IS NOT NULL
	LEFT JOIN compfafsa.Schools MSL
		ON MSL.SchoolId = SV.AdjustedSchoolId
WHERE
	SD.MasterSchoolListId IS NULL
	AND MSL.MasterSchoolListId IS NOT NULL

CLOSE SYMMETRIC KEY UHEAA_Data_Key;
END