
CREATE PROCEDURE [scra].[SetTS06Indicators]
	@Success BIT,
	@ScriptProcessingId INT
AS

UPDATE
	[scra].ScriptProcessing
SET
	TS06Indicator = @Success,
	TS06Updated = GETDATE()
WHERE
	ScriptProcessingId = @ScriptProcessingId
	AND TS06Indicator IS NULL