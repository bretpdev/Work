USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE	@CategoryId INT


	DELETE FROM
		ArcLoanSequenceSelection
	WHERE
		ArcAddProcessingID IN
	( --14
		SELECT ArcAddProcessingId from ArcAddProcessing where ScriptId = 'UNH_26897'
	)

	DELETE FROM ArcAddProcessing WHERE ScriptId = 'UNH_26897'


	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ERROR = 0
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
	
DBCC CHECKIDENT(ArcAddProcessing, RESEED)
DBCC CHECKIDENT(ArcLoanSequenceSelection, RESEED)