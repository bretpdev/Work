CREATE PROCEDURE [dbo].[GetNextDueDateChangeRecord]

AS
BEGIN
	SET NOCOUNT ON;
BEGIN TRANSACTION

	DECLARE @DueDateChangeId BIGINT
	DECLARE @ERROR INT

	DECLARE @MyTableVar 
		TABLE
		(
			DueDateChangeId int NOT NULL
		);

	SELECT @ERROR = @@ERROR

	UPDATE TOP (1)
		DueDateChange
	SET
		ProcessedAt = GETDATE()
	OUTPUT inserted.DueDateChangeId
	INTO @MyTableVar
	WHERE
		ProcessedAt IS NULL
	
	SELECT @ERROR = @ERROR + @@ERROR

	SELECT
		DDC.DueDateChangeId,
		Ssn,
		AccountNumber,
		DueDate,
		Arc,
		Comment
	FROM 
		DueDateChange DDC
	INNER JOIN @MyTableVar MT
		ON MT.DueDateChangeId = DDC.DueDateChangeId

	
	SELECT @ERROR = @ERROR + @@ERROR

	IF @ERROR = 0
		BEGIN
			COMMIT TRANSACTION
		END
	ELSE
		BEGIN
			RAISERROR ('Failed to retrieve dbo.ArcAddProcessing record for processing.', -- error message
				   16, -- Severity.
				   1 -- State.
				   );
			ROLLBACK TRANSACTION
		END
	END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNextDueDateChangeRecord] TO [db_executor]
    AS [dbo];
