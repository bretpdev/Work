USE CLS

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

UPDATE CLS.[print].Letters SET PagesPerDocument = X	WHERE letter IN ('ICRDENFED','REQRCVFED','TSXXBDXXX','TSXXBDXXX','TSXXBDDXX','TSXXBDDXXX','TSXXBDDXX','TSXXBDDXX','TSXXBDDXX','TSXXBDDIDR','TSXXBDMP','TSXXBDSCH','TSXXBDUNEM','TSXXBFXXX','TSXXBFXXXC','TSXXBFXXX','TSXXBPYED','TSXXBRSMRY','TSXXBSAPPM','TSXXBTMXLM','TSXXBTPDSS','TSXXBTRTX','TSXXBFDLP','TSXXBISF')
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
UPDATE CLS.[print].Letters SET PagesPerDocument = X, Instructions = 'Economic Hardship Deferment' WHERE letter IN ('TSXXBDHRD')
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
UPDATE CLS.[print].Letters SET PagesPerDocument = X, Instructions = 'FORM:  IBR, ICR and Pay As You Earn Repayment Plan Request' WHERE letter IN ('TSXXBIBR')
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
UPDATE CLS.[print].Letters SET PagesPerDocument = X, Instructions = 'The page number reflects the letter and XX page form.' WHERE letter IN ('TSXXBRPYXP')
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
UPDATE CLS.[print].Letters SET PagesPerDocument = X	WHERE letter IN ('APREQFED', 'TSXXBAPIDR', 'TSXXBIDREC', 'TSXXBQRTLY', 'TSXXBGLBX')
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
UPDATE CLS.[print].Letters SET PagesPerDocument = X	WHERE letter IN ('PLRPYMTFED')
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
UPDATE CLS.[print].Letters SET PagesPerDocument = XX WHERE letter IN ('RPDISCFED')
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
INSERT INTO CLS.[print].Letters(Letter,PagesPerDocument,Instructions) VALUES ('TSXXBCARQT', X, NULL), ('TSXXIDTINB',NULL, NULL)
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	

IF @ROWCOUNT = XX AND @ERROR = X
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





  
  
 
  
