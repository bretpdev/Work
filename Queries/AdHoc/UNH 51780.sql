USE UDW
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

UPDATE
	UDW..LT20_LTR_REQ_PRC 
SET
	EcorrDocumentCreatedAt = NULL
WHERE 
	DF_SPE_ACC_ID IN ('5110720920','5514028860','5819532755') 
	AND RM_DSC_LTR_PRC = 'US09BFDLP'
	AND DATEDIFF(DAY,EcorrDocumentCreatedAt,'2017-05-15') = 0

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

UPDATE
	EcorrUheaa..DocumentDetails
SET
	Active = 0
WHERE
	DocumentDetailsId IN (617503,617504,617505)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	
IF @ROWCOUNT = 6 AND @ERROR = 0
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
