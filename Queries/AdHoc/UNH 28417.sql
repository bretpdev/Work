USE BSYS
GO

BEGIN TRANSACTION
DECLARE @ERROR INT = 0
DECLARE @ROWCOUNT INT = 0
DECLARE @ExpectedRowCount INT = 41

--Query Body

DECLARE @OldId nvarchar(max) = 'dlino'
DECLARE @NewId nvarchar(max) = 'dhalaliku'

UPDATE SYSA_LST_UserIDInfo SET WindowsUserName = @NewId WHERE WindowsUserName = @NewId
SET @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT
UPDATE MABC_SSN_User_XRef SET WindowsUserName = @NewId WHERE WindowsUserName = @NewId
SET @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT
UPDATE SystemAccessDB_MasterMimic SET WindowsUserName = @NewId WHERE WindowsUserName = @NewId
SET @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT
UPDATE SYSA_REF_User_AppAndMod SET WindowsUserName = @NewId WHERE WindowsUserName = @NewId
SET @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT
UPDATE SYSA_REF_User_Systems SET WindowsUserName = @NewId WHERE WindowsUserName = @NewId
SET @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT
UPDATE TRAN_DAT_TrainingHistory SET WindowsUserName = @NewId WHERE WindowsUserName = @NewId
SET @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT
UPDATE SYSA_DAT_AppAndTrainCoordErrors SET WindowsUserName = @NewId WHERE WindowsUserName = @NewId
SET @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT
UPDATE SYSA_LST_Users SET WindowsUserName = @NewId WHERE WindowsUserName = @NewId
SET @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT
UPDATE GENR_REF_BU_Agent_Xref SET WindowsUserId = @NewId WHERE WindowsUserId = @NewId
SET @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT


SELECT @ERROR = @@ERROR

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

  