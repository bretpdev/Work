CREATE PROCEDURE [dbo].[AddBusinessUnit]
	@ID int,
	@ScriptId nvarchar(10)
AS
	INSERT INTO ProcessBusinessUnitMapping(ScriptId, BusinessUnitId, StartedAt)
	VALUES(@ScriptId, @ID, GETDATE())
RETURN 0
