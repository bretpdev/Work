CREATE PROCEDURE [dbo].[DeleteSackerCache]
	@SackerCacheId INT
AS

	DELETE
	FROM
		SackerCache
	WHERE
		SackerCacheId = @SackerCacheId

RETURN 0
