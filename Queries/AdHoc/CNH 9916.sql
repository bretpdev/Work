BEGIN TRANSACTION
DECLARE @ERROR INT = X
DECLARE @ROWCOUNT INT = X
DECLARE @ExpectedRowCount INT = XXX

--Query Body

UPDATE
	[ECorrFed].[dbo].[DocumentDetails]
SET
	Printed = NULL,
	ZipFileName = NULL
WHERE
	 ZipFileName = 'KU_IN_ZIP_XXXXXXXXXXXX_jXIXJaMBgk-CUAXTFivnsA.zip' AND CreateDate >= 'X/XX/XXXX'
--Query Body

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END
