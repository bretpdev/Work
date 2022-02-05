CREATE PROCEDURE [dbo].[BusinessUnitsByScript]
	@ScriptId nvarchar(10)
AS
	SELECT
		units.BusinessUnitId AS [ID],
		units.BusinessUnitName AS [Name]
	FROM
		BusinessUnits units
		JOIN ProcessBusinessUnitMapping buMap
			ON units.BusinessUnitId = buMap.BusinessUnitId
	WHERE
		buMap.ScriptId = @ScriptId
		AND 
		EndedAt IS NULL
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BusinessUnitsByScript] TO [db_executor]
    AS [dbo];

