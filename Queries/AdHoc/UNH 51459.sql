BEGIN TRANSACTION
DECLARE @ERROR INT = 0
DECLARE @ROWCOUNT INT = 0
DECLARE @ExpectedRowCount INT = 1

--Query Body

UPDATE
	Norad.dbo.CKPH_DAT_OPSCheckByPhone
SET
	EncryptedBankAccountNumber = 0x003E84E2C15DFD458BB7F39FB5B4844E010000009E83050EBDA0527682438FFDCC4F7AB93DABC7C573E6EFD25C55A80D4F5EF8EF441D8C79928FB355CE8FF4D41DAE26BC
WHERE
	ID = 71165

--Query Body

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END
