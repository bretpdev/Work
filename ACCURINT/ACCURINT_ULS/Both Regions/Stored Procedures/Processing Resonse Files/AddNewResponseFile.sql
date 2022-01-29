CREATE PROCEDURE [accurint].[AddNewResponseFile]
	@RunId INT,
	@ResponseFileName VARCHAR(260)
AS

	IF NOT EXISTS (SELECT RunId, ResponseFileName FROM accurint.ResponseFileProcessingQueue WHERE DeletedAt IS NULL AND ResponseFileName = @ResponseFileName AND RunId = @RunId)
		BEGIN
			INSERT INTO accurint.ResponseFileProcessingQueue (RunId, ResponseFileName)
			VALUES (@RunId,	@ResponseFileName)
		END
		
RETURN 0
