CREATE PROCEDURE [i1i2schltr].[MarkPrintDataProcessed]
	@PrintDataId INT
AS
	UPDATE
		i1i2schltr.PrintData
	SET
		ProcessedAt = GETDATE()
	WHERE
		PrintDataId = @PrintDataId
		AND DeletedAt IS NULL
		AND DeletedBy IS NULL
RETURN 0
