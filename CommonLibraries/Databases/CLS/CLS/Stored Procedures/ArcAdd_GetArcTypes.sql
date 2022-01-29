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

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_GetArcTypes] TO [db_executor]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_GetArcTypes] TO [UHEAA\SystemAnalysts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_GetArcTypes] TO [UHEAA\CornerStoneUsers]
    AS [dbo];