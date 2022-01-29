CREATE PROCEDURE [nsldsconso].[RecoveryMigration]
	@OldDataLoadRunId INT,
	@NewDataLoadRunID INT
AS
	UPDATE
		nsldsconso.DataLoadRuns
	SET
		RecoveryMigratedTo = @NewDataLoadRunID
	WHERE
		DataLoadRunId = @OldDataLoadRunId

	UPDATE
		nsldsconso.Borrowers
	SET
		DataLoadRunId = @NewDataLoadRunID
	WHERE
		DataLoadRunId = @OldDataLoadRunId

	SELECT
		B.SSN
	FROM
		nsldsconso.Borrowers B
	WHERE
		B.DataLoadRunId = @NewDataLoadRunID

RETURN 0
