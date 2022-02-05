CREATE PROCEDURE [dbo].[EndBusinessUnit]
	@ScriptId nvarchar(10),
	@BusinessUnitId int
AS
	UPDATE
		ProcessBusinessUnitMapping
	SET
		EndedAt = GETDATE()
	WHERE
		ScriptId = @ScriptId
		AND BusinessUnitId = @BusinessUnitId
RETURN 0
