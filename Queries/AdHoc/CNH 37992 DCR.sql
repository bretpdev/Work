USE CLS

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedCount INT = XX


DECLARE @Temp TABLE(AccountNumber VARCHAR(XX), PaymentAmount DECIMAL(XX,X), EffectiveDate DATE)
INSERT INTO @Temp
VALUES('XXXXXXXXXX',XX.XX,'XX/XX/XX'),
('XXXXXXXXXX',XXX.XX,'XX/XX/XX'),
('XXXXXXXXXX',XXX.XX,'XX/XX/XX'),
('XXXXXXXXXX',XX.XX,'XX/XX/XX'),
('XXXXXXXXXX',XX.XX,'XX/XX/XX'),
('XXXXXXXXXX',XXX.XX,'XX/XX/XX'),
('XXXXXXXXXX',XX.XX,'XX/XX/XX'),
('XXXXXXXXXX',XX.XX,'XX/XX/XX'),
('XXXXXXXXXX',XX.XX,'XX/XX/XX'),
('XXXXXXXXXX',XXX.XX,'XX/XX/XX'),
('XXXXXXXXXX',XX.XX,'XX/XX/XX'),
('XXXXXXXXXX',XXX.XX,'XX/XX/XX'),
('XXXXXXXXXX',XXX.XX,'XX/XX/XX'),
('XXXXXXXXXX',XXX.XX,'XX/XX/XX'),
('XXXXXXXXXX',XXX.XX,'XX/XX/XX'),
('XXXXXXXXXX',XX.XX,'XX/XX/XX'),
('XXXXXXXXXX',XXX.XX,'XX/XX/XX')

UPDATE
	CBP
SET
	CBP.ProcessedDate = NULL,
	CBP.[FileName] = NULL
FROM
	CLS..CheckByPhone CBP
	INNER JOIN @Temp T
		ON T.AccountNumber = CBP.AccountNumber
		AND T.PaymentAmount = CBP.PaymentAmount
		AND T.EffectiveDate = CBP.EffectiveDate

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedCount AND @ERROR = X
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





  
  
 
  
