CREATE PROCEDURE [scra].[SetTSX0TIndicators]
	@Success BIT,
	@ScriptProcessingId INT
AS

UPDATE
	[scra].ScriptProcessing
SET
	TSX0TIndicator = @Success,
	TSX0TUpdated = GETDATE()
WHERE
	ScriptProcessingId = @ScriptProcessingId
	AND TSX0TIndicator IS NULL