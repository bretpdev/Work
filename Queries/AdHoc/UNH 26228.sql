USE DocID
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	INSERT INTO DocIDDeterminationCrit(ResultOrder, DocID, DocumentType)
	VALUES(1, 'AMDTH', 'UHEAA Death Certificates'),
	(1, 'AMDAR', 'Non-UHEAA Manual DAAR forms'),
	(1, 'AMPCA', 'Non-UHEAA pre-claim Trnsmittals'),
	(1, 'AMTNS', 'Claim transmittals received from guarantors'),
	(1, 'AMFIX', 'Claim Corrections'),
	(1, 'AMSUB', 'Non-UHEAA Claims Submitted'),
	(1, 'AMRTN', 'Return/Recalled Claims from Gurantors'),
	(1, 'AMCOR', 'General or Misc Asset Management Documents')

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	INSERT INTO DocIDDocTypeLst(DocId, DocumentType)
	VALUES('AMDTH', 'UHEAA Death Certificates'),
	('AMDAR', 'Non-UHEAA Manual DAAR forms'),
	('AMPCA', 'Non-UHEAA pre-claim Trnsmittals'),
	('AMTNS', 'Claim transmittals received from guarantors'),
	('AMFIX', 'Claim Corrections'),
	('AMSUB', 'Non-UHEAA Claims Submitted'),
	('AMRTN', 'Return/Recalled Claims from Gurantors'),
	('AMCOR', 'General or Misc Asset Management Documents')

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR	

	INSERT INTO DocTypeFunctionalityChecks(DocType, DoLSandSChecks, DoBankruptcyCheck, DoGeneralCorr, DoLVC, DocIDTranslation)
	VALUES('UHEAA Death Certificates', 'False', 'False', 'False', 'False', 'AMDTH'),
	('Non-UHEAA Manual DAAR forms', 'False', 'False', 'False', 'False', 'AMDAR'),
	('Non-UHEAA pre-claim Trnsmittals', 'False', 'False', 'False', 'False', 'AMPCA'),
	('Claim transmittals received from guarantors', 'False', 'False', 'False', 'False', 'AMTNS'),
	('Claim Corrections', 'False', 'False', 'False', 'False', 'AMFIX'),
	('Non-UHEAA Claims Submitted', 'False', 'False', 'False', 'False', 'AMSUB'),
	('Return/Recalled Claims from Gurantors', 'False', 'False', 'False', 'False', 'AMRTN'),
	('General or Misc Asset Management Documents', 'False', 'False', 'False', 'False', 'AMCOR')

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR	

IF @ROWCOUNT = 24 AND @ERROR = 0
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
