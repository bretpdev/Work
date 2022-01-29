CREATE PROCEDURE [print].[SetEcorrDocumentCreatedAtWithTable]
	@PrintProcessingId PrintProcessIdsTable READONLY
AS
	UPDATE PP
	SET 
		PP.EcorrDocumentCreatedAt = GETDATE()
	FROM 
		PrintProcessing PP
	INNER JOIN @PrintProcessingId IDS
		ON IDS.PrintProcessingId = PP.PrintProcessingId
	WHERE 
		PP.EcorrDocumentCreatedAt IS NULL
RETURN 0