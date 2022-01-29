
CREATE PROCEDURE [fp].[GetAndUpdateUnprocessedRecord]
AS
BEGIN

SET NOCOUNT ON;
BEGIN TRANSACTION

	DECLARE @FileProcessingId BIGINT
	DECLARE @Error INT

	DECLARE @MyTableVar
		TABLE
		(
			FileProcessingId int NOT NULL
		);

	SELECT @Error = @@ERROR

	UPDATE TOP (1)
		fp.FileProcessing
	SET
		ProcessedAt = GETDATE()
	OUTPUT inserted.FileProcessingId
	INTO @MyTableVar
	WHERE
		ProcessedAt IS NULL

	SELECT @Error = @Error + @@ERROR
	
	SELECT
		FP.FileProcessingId,
		FP.GroupKey,
		FP.SourceFile,
		FP.ProcessedAt
	FROM
		fp.FileProcessing FP
	INNER JOIN @MyTableVar MT
		ON MT.FileProcessingId = FP.FileProcessingId

	SELECT @Error = @Error + @@ERROR

	IF @Error = 0
		BEGIN
			COMMIT TRANSACTION
		END
	ELSE
		BEGIN
			RAISERROR ('Failed to retrieve fp.FileProcessingId record for processing.', -- error message
				   16, -- Severity.
				   1 -- State.
				   );
			ROLLBACK TRANSACTION
		END
	END
GO
GRANT EXECUTE
    ON OBJECT::[fp].[GetAndUpdateUnprocessedRecord] TO [db_executor]
    AS [dbo];

