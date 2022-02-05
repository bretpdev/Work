
CREATE PROCEDURE [print].[SetLastProcessed]
	@ScriptDataId int
AS
	UPDATE
		ScriptData
	SET
		LastProcessed = GETDATE()
	WHERE
		ScriptDataId = @ScriptDataId
RETURN 0