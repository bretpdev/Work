CREATE PROCEDURE [fafsadblod].[AddSchools]
AS
BEGIN

INSERT INTO compfafsa.SchoolVariations(SchoolIdFromFile, SchoolName, AdjustedSchoolId, AddedAt, AddedBy, AdjustedAt, AdjustedBy, DeletedAt, DeletedBy)
SELECT DISTINCT
	IFD.HighSchoolCode AS SchoolIdFromFile,
	UPPER(RTRIM(IFD.HighSchoolName)) AS SchoolName,
	NULL AS AdjustedSchoolId,
	GETDATE() AS AddedAt,
	SUSER_SNAME() AS AddedBy,
	NULL AS AdjustedAt,
	NULL AS AdjustedBy,
	NULL AS DeletedAt,
	NULL AS DeletedBy
FROM 
	fafsadblod.IsirFileData IFD
	LEFT JOIN compfafsa.SchoolVariations SV
		ON SV.SchoolName = RTRIM(IFD.HighSchoolName)
		AND SV.DeletedAt IS NULL
	LEFT JOIN compfafsa.Schools SVMSL
		ON SVMSL.SchoolId = SV.AdjustedSchoolId
		AND SVMSL.DeletedAt IS NULL
	LEFT JOIN compfafsa.Schools MSL
		ON MSL.SchoolName = RTRIM(IFD.HighSchoolName)
		AND MSL.DeletedAt IS NULL
	LEFT JOIN compfafsa.SchoolVariations PendingExisting
		ON PendingExisting.SchoolIdFromFile = IFD.HighSchoolCode
		AND PendingExisting.SchoolName = RTRIM(IFD.HighSchoolName)
WHERE
	IFD.HighSchoolState = 'UT'
	AND MSL.SchoolId IS NULL
	AND SVMSL.SchoolId IS NULL
	AND PendingExisting.SchoolName IS NULL
END