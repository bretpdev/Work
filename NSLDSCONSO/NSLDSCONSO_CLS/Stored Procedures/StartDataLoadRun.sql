CREATE PROCEDURE [nsldsconso].[StartDataLoadRun]
	@BorrowerCount INT,
	@Filename VARCHAR(256)
AS
	
	INSERT INTO
		nsldsconso.DataLoadRuns 
		(BorrowerCount, [Filename])
	VALUES
		(@BorrowerCount, @Filename)

	SELECT
		CAST(SCOPE_IDENTITY() AS INT) DataLoadRunId

RETURN 0
