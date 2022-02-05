CREATE PROCEDURE [print].[SetPrintingCompleteWithTable]
	@PrintProcessingId PrintProcessingIds READONLY
AS
	UPDATE PP
	SET 
		PP.PrintedAt = GETDATE()
	FROM 
		PrintProcessing PP
	INNER JOIN @PrintProcessingId IDS
		ON IDS.PrintProcessingId = PP.PrintProcessingId
	WHERE 
		PP.PrintedAt IS NULL
RETURN 0