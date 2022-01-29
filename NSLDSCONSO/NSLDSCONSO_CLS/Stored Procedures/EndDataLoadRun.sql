CREATE PROCEDURE [nsldsconso].[EndDataLoadRun]
	@DataLoadRunId INT
AS
	
	UPDATE
		nsldsconso.DataLoadRuns
	SET
		EndedOn = GETDATE()
	WHERE
		DataLoadRunId = @DataLoadRunId

RETURN 0
