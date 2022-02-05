CREATE PROCEDURE [fp].[CheckFileProcessed]
	@SourceFile varchar(100)
AS
	SELECT
		COUNT(*)
	FROM
		fp.FileProcessing
	WHERE
		SourceFile = @SourceFile
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[fp].[CheckFileProcessed] TO [db_executor]
    AS [dbo];

