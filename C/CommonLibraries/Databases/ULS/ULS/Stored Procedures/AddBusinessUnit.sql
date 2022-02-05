CREATE PROCEDURE [dbo].[AddBusinessUnit]
	@ID int,
	@Name varchar(50),
	@ScriptId nvarchar(10)
AS
	INSERT INTO BusinessUnits(BusinessUnitId, BusinessUnitName)
	VALUES(@ID, @Name)

	INSERT INTO ProcessBusinessUnitMapping(ScriptId, BusinessUnitId, StartedAt)
	VALUES(@ScriptId, @ID, GETDATE())
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddBusinessUnit] TO [db_executor]
    AS [dbo];

