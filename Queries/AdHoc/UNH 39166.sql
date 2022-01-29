
BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	INSERT INTO DocIDDeterminationCrit(RecNum, ResultOrder, DocID, DocumentType, LoanStatus, ReasonCode, Servicer)
	VALUES(55, 1, 'PCBKP', 'BANKRUPTCY DOCUMENT/CORRESPONDENCE', null, null, null)
	,(56, 1, 'PCMOC', 'BANKRUPTCY MEETING OF CREDITORS', null, null, null)
	,(57, 1, 'PCCON', 'BANKRUPTCY DISCHARGE/DISMISSAL DOCUMENTS', null, null, null)
	,(58, 1, 'RCSUC', 'CLAIMS SUPPLEMENTAL CLAIM', null, null, null)
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	
	INSERT INTO DocIDDocTypeLst(DocID, DocumentType, CompassOnlyArc)
	VALUES('PCBKP', 'BANKRUPTCY DOCUMENT/CORRESPONDENCE', null)
	,('PCMOC', 'BANKRUPTCY MEETING OF CREDITORS', null)
	,('PCCON', 'BANKRUPTCY DISCHARGE/DISMISSAL DOCUMENTS', null)
	,('RCSUC','CLAIMS SUPPLEMENTAL CLAIM', null)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	INSERT INTO DocTypeFunctionalityChecks(DocType, DoLSandSChecks, DoBankruptcyCheck, DoGeneralCorr, DoLVC, DocIDTranslation)
	VALUES('BANKRUPTCY DOCUMENT/CORRESPONDENCE', 'false', 'false', 'false', 'false', 'PCBKP')
	,('BANKRUPTCY MEETING OF CREDITORS', 'false', 'false', 'false', 'false', 'PCMOC')
	,('BANKRUPTCY DISCHARGE/DISMISSAL DOCUMENTS', 'false', 'false', 'false', 'false', 'PCCON')
	,('CLAIMS SUPPLEMENTAL CLAIM', 'false', 'false', 'false', 'false', 'RCSUC')
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	
IF @ROWCOUNT = 12 AND @ERROR = 0
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
