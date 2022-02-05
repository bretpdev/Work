
CREATE PROCEDURE [scra].[SetArcAddId]
	@ArcAddProcessingId BIGINT,
	@ScriptProcessingId INT
AS

UPDATE
	[scra].ScriptProcessing
SET
	AAPUpdated = @ArcAddProcessingId
WHERE
	ScriptProcessingId = @ScriptProcessingId
	AND ErroredAt IS NULL