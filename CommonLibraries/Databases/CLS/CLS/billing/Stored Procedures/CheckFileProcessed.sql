CREATE PROCEDURE [billing].[CheckFileProcessed]
(
	@SourceFile VARCHAR(100)
)
AS
BEGIN
	SELECT 
		COUNT(*)
	FROM
		billing.PrintProcessing
	WHERE
		SourceFile = @SourceFile
END;