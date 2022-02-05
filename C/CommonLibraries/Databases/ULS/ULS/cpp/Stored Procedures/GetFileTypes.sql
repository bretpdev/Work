CREATE PROCEDURE [cpp].[GetFileTypes]
AS
	SELECT
		FileTypeId,
		FileType
	FROM
		FileTypes

RETURN 0

GRANT EXECUTE ON [cpp].[GetFileTypes] TO db_executor