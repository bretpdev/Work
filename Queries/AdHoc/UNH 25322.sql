USE BSYS
GO


BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

UPDATE [BSYS].[dbo].[SYSA_LST_Users] SET WindowsUserName = 'janderson1' WHERE FirstName = 'JULIE' and LastName = 'ANDERSON'
SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR --1
UPDATE [BSYS].[dbo].[SYSA_LST_Users] SET WindowsUserName = 'janderson' WHERE WindowsUserName = 'janderson2'
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR --1
UPDATE [BSYS].[dbo].[SYSA_LST_UserIDInfo] SET WindowsUserName = 'janderson1' where UserID ='UT00231'
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR --1
UPDATE [BSYS].[dbo].[SYSA_LST_UserIDInfo] SET WindowsUserName = 'janderson' WHERE UserID IN ('UT01545','UT01546')
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR --2
INSERT INTO [BSYS].[dbo].[SYSA_LST_UserLogonInfo](WindowsUserID, FavoriteScreen, LogonRegion) VALUES('janderson','LP22','Live')
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR --1
UPDATE [BSYS].[dbo].[SYSA_REF_User_AppAndMod] SET WindowsUserName = 'janderson' where WindowsUserName = 'janderson2'
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR --8

IF @ROWCOUNT = 14 AND @ERROR = 0
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