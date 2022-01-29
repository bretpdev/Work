CREATE PROCEDURE [subscrprt].[SetProcessed]
	@PrintDataId INT
AS
	UPDATE
		subscrprt.PrintData
	SET
		ProcessedAt = GETDATE()
	WHERE
		PrintDataId = @PrintDataId
RETURN 0