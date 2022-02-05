CREATE PROCEDURE [print].[SetImagedAtWithTable]
	@PrintProcessingId PrintProcessIdsTable READONLY
AS
	UPDATE PP
	SET 
		PP.ImagedAt = GETDATE()
	FROM 
		PrintProcessing PP
	INNER JOIN @PrintProcessingId IDS
		ON IDS.PrintProcessingId = PP.PrintProcessingId
	WHERE 
		PP.ImagedAt IS NULL
RETURN 0