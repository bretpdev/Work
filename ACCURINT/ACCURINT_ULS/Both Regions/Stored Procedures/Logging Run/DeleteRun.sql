CREATE PROCEDURE [accurint].[DeleteRun]
	@RunId INT
AS

BEGIN TRANSACTION
	
	DECLARE 
		@ERROR INT = 0,
		@ROWCOUNT INT = 0,
		@Now DATETIME = GETDATE(),
		@User VARCHAR(256) = SUSER_NAME(),
		@EXPECTEDCOUNT INT = 1 + (SELECT COUNT(1) FROM accurint.DemosProcessingQueue_UH WHERE RunId = @RunId) + (SELECT COUNT(1) FROM accurint.DemosProcessingQueue_OL WHERE RunId = @RunId) + (SELECT COUNT(1) FROM accurint.ResponseFileProcessingQueue WHERE RunId = @RunId);

	UPDATE --May want to update so that it sets the DeletedAt not only in RunLogger, but in the processing tables as well (thus negating needing for OL_/UH_SetDeleted sprocs)
		accurint.RunLogger
	SET
		DeletedAt = @Now,
		DeletedBy = @User
	WHERE
		RunId = @RunId

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR;
		
	UPDATE
		accurint.DemosProcessingQueue_UH
	SET
		DeletedAt = @Now,
		DeletedBy = @User
	WHERE
		RunId = @RunId

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR;

	UPDATE 
		accurint.DemosProcessingQueue_OL
	SET
		DeletedAt = @Now,
		DeletedBy = @User
	WHERE
		RunId = @RunId

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR;

	UPDATE 
		accurint.ResponseFileProcessingQueue
	SET
		DeletedAt = @Now,
		DeletedBy = @User
	WHERE
		RunId = @RunId

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR;

IF @ROWCOUNT = @EXPECTEDCOUNT AND @ERROR = 0
	BEGIN
		COMMIT TRANSACTION
		SELECT CAST(1 AS BIT) AS WasSuccessful; --Succeeded
	END
ELSE
	BEGIN
		ROLLBACK TRANSACTION
		SELECT CAST(0 AS BIT) AS WasSuccessful; --Failed
	END

RETURN 0;
