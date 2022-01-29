USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	INSERT INTO ArcAddProcessing(AccountNumber, ARC, ArcTypeId, Comment, ScriptId, ProcessOn, CreatedAt, CreatedBy)
	VALUES('591671927', 'DDPHN', 1, 'NobleCallHistoryId:1789778 Agent:     No Answer', 'DIACTCMTS', GETDATE(), GETDATE(), USER_NAME()),
	('592582123', 'DDPHN', 1, 'NobleCallHistoryId:1789790 Agent:     Virtual Message', 'DIACTCMTS', GETDATE(), GETDATE(), USER_NAME()),
	('103549216', 'DDPHN', 1, 'NobleCallHistoryId:1789795 Agent:     No Answer', 'DIACTCMTS', GETDATE(), GETDATE(), USER_NAME())

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 3 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
