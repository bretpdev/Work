CREATE PROCEDURE [dbo].[ArcAdd_GetArcTypes]
AS
BEGIN
	SELECT
		ArcTypeId,
		ArcType
	FROM
		ArcType
RETURN 0
END