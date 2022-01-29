CREATE PROCEDURE [finalrev].[GetRecoveryValues]
AS
	SELECT
		BorrowerRecordID,
		Ssn,
		Step
	FROM
		finalrev.BorrowerRecord
	WHERE
		ProcessedAt IS NULL
		AND DeletedAt IS NULL
RETURN 0