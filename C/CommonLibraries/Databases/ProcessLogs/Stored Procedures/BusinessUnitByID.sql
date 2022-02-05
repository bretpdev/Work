CREATE PROCEDURE [dbo].[BusinessUnitByID]
	@BusinessUnitId int
AS
	SELECT
		BusinessUnitId,
		BusinessUnitName
	FROM
		BusinessUnits
	WHERE
		BusinessUnitId = @BusinessUnitId
RETURN 0