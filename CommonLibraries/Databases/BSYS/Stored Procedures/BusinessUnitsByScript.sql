CREATE PROCEDURE BusinessUnitsByScript
	@ScriptId nvarchar(10)
AS
	SELECT
		unitid.ID,
		unitid.Name
	FROM
		BSYS.dbo.SCKR_DAT_Scripts script
		JOIN BSYS.dbo.SCKR_REF_Unit unit ON script.Script = unit.Program
		JOIN CSYS.dbo.GENR_LST_BusinessUnits unitId ON unit.Unit = unitId.Name
	WHERE
		script.Id = @ScriptId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BusinessUnitsByScript] TO [db_executor]
    AS [dbo];

