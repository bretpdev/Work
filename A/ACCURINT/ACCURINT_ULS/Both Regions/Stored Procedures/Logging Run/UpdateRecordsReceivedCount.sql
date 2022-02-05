CREATE PROCEDURE [accurint].[UpdateRecordsReceivedCount]
		@RunId INT,
		@RecordsReceived INT
AS
	
	UPDATE
		accurint.RunLogger
	SET
		RecordsReceived = ISNULL(RecordsReceived, 0) + @RecordsReceived
	WHERE
		RunId = @RunId

RETURN 0
