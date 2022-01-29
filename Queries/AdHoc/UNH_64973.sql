USE [CSYS]
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

INSERT INTO [pmtcancl].[Roles]
           ([Role]
           ,[Description]
           ,[UheaaAccess]
           ,[FedAccess])
     VALUES
           (132,'ROLE - Operations - Team Lead  Email-Skip',1,1),
		   (126,'ROLE - Operations - Supervisor Email-SKIP',1,1),
		   (140,'ROLE - Operations - Supervisor w Titanium',1,1)

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 3 AND @ERROR = 0
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

GO


