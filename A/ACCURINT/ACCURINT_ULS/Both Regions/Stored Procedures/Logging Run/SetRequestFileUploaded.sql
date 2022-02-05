CREATE PROCEDURE [accurint].[SetRequestFileUploaded]
	@RunId INT,
	@RequestFileName VARCHAR(260)
AS
	UPDATE
		accurint.RunLogger
	SET
		RequestFileUploadedAt = GETDATE(),
		RequestFileName = @RequestFileName
	WHERE
		RunId = @RunId
		
RETURN 0
