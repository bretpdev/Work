
CREATE PROCEDURE [fp].[GetFileHeader]
	@FileHeaderId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		FileHeader
	FROM
		[fp].FileHeaders
	WHERE
		FileHeaderId = @FileHeaderId

END
GO
GRANT EXECUTE
    ON OBJECT::[fp].[GetFileHeader] TO [db_executor]
    AS [dbo];

