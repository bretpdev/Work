
CREATE PROCEDURE [scra].[SetErroredAt]
	@ScriptProcessingId INT
AS

UPDATE
	[scra].ScriptProcessing
SET
	ErroredAt = GETDATE()
WHERE
	ScriptProcessingId = @ScriptProcessingId
	AND ErroredAt IS NULL