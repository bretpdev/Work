USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	--SELECT 
	--	* 
	--FROM 
	--	ULS.[dbo].[ArcAddProcessing]
	--WHERE
	--	ARC = 'ACCNT'
	--	AND	ScriptId = 'PRESZQUE'
	--	AND CreatedAt < '2016-03-01'
	--167

	UPDATE
		ArcAddProcessing
	SET
		ProcessedAt = NULL
	WHERE
		ARC = 'ACCNT'
		AND	ScriptId = 'PRESZQUE'
		AND CreatedAt < '2016-03-01'

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 167 AND @ERROR = 0
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
