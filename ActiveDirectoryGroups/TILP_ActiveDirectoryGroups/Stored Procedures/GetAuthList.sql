CREATE PROCEDURE [dbo].[GetAuthList]
AS
	SELECT
		AuthLevel,
		LevelDesc
	FROM
		AuthList
RETURN 0