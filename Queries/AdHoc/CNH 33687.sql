USE CSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = X--X records to be updated.
  
UPDATE CSYS..GENR_DAT_EnterpriseFileSystem SET [Path] = REPLACE([Path], '\\uheaa-fs', '\\fsuheaaxyz') WHERE [Key] = 'EcorrArchive'

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR --X



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
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
