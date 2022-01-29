CREATE PROCEDURE [emailbatch].[CheckFileProcessed]
	@SourceFile VARCHAR(300)
AS
	SELECT 
		COUNT(*)
	FROM
		[emailbatch].EmailProcessing
	WHERE
		ActualFile = @SourceFile
RETURN 0
