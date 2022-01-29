--run on NOCHOUSE
BEGIN TRANSACTION

	DECLARE @ERROR INT = 0,
			@ROWCOUNT INT = 0;

	UPDATE	
		BSYS..LTDB_DAT_CentralPrintingDocData
	SET
		[PATH] = 'X:\PADD\AccountServices\AUInSchoolDef.docx'
	WHERE
		[PATH] = 'X:\PADD\AccountServices\AUInSchoolDef.doc'
		AND ID = 'SADEFER'
	;
	--1
	
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR --1

IF @ROWCOUNT = 1 AND @ERROR = 0
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
