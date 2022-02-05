CREATE PROCEDURE [finalrev].[GetSchoolLetterData]
AS
	SELECT
		BorrowerRecordId,
		Ssn
	FROM
		finalrev.BorrowerRecord
	WHERE
		DeletedAt IS NULL
		AND SchoolLetterNeeded = 1
		AND SchoolLetterSent IS NULL