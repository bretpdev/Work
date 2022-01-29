CREATE PROCEDURE [scra].[SetRecordSpecialBypass]
	@ScriptProcessingId INT
AS

	UPDATE
		scra.ScriptProcessing
	SET
		TS06Indicator = 0,
		TS06Updated = GETDATE(),
		TXCXIndicator = 0,
		TXCXUpdated = GETDATE(),
		TS0NIndicator = 0,
		TS0NUpdated = GETDATE()
	WHERE
		ScriptProcessingId = @ScriptProcessingId


RETURN 0
