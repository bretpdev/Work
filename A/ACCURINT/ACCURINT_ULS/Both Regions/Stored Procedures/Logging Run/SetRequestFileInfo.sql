CREATE PROCEDURE [accurint].[SetRequestFileInfo]
	@RunId INT,
	@RecordsSent INT
AS
	UPDATE
		accurint.RunLogger
	SET
		RequestFileCreatedAt = GETDATE(),
		RecordsSent = @RecordsSent
	WHERE
		RunId = @RunId
		
RETURN 0
