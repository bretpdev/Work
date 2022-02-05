
CREATE PROCEDURE [scra].[SetTS0NIndicators]
	@Success BIT,
	@ScriptProcessingId INT
AS

UPDATE
	[scra].ScriptProcessing
SET
	TS0NIndicator = @Success,
	TS0NUpdated = GETDATE()
WHERE
	ScriptProcessingId = @ScriptProcessingId
	AND TS0NIndicator IS NULL