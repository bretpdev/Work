BEGIN TRANSACTION
	DECLARE @ERROR INT = X;
	DECLARE @ROWCOUNT INT = X;
	DECLARE @ExpectedRowCount INT = XX; --Change This Value
	DECLARE @CNHTIX VARCHAR(XX) = 'CNH XXXXX'; --Change This Value

	INSERT INTO CLS..ArcAddProcessing(ArcTypeId, AccountNumber, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, RegardsTo, LN_ATY_SEQ, ProcessingAttempts, CreatedAt, CreatedBy, ProcessedAt)
	SELECT DISTINCT
		X,
		PDXX.DF_SPE_ACC_ID,
		'BRTXT',
		@CNHTIX,
		GETDATE(),
		'Text Message sent.',
		X,
		X,
		'',
		X,
		X,
		GETDATE(),
		@CNHTIX,
		NULL
	FROM
		CDW..PDXX_PRS_NME PDXX
	WHERE --Change This List
		PDXX.DF_SPE_ACC_ID IN 
		(
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX'
		)

	;

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR 

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END
;
