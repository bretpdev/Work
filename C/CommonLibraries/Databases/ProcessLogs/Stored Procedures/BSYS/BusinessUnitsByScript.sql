USE [BSYS]
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('BusinessUnitsByScript') AND TYPE IN ('P', 'PC'))
DROP PROCEDURE BusinessUnitsByScript
GO
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
GRANT EXECUTE ON BusinessUnitsByScript TO db_executor
GO