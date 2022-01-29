CREATE PROCEDURE [print].CheckFileProcessed
(
	@SourceFile VARCHAR(100)
)
AS
BEGIN
	SELECT 
		COUNT(*)
	FROM
		[print].PrintProcessing
	WHERE
		SourceFile = @SourceFile
END;