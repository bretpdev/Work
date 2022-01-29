DECLARE @ROLE INT = (SELECT MAX(ROLE) FROM [CSYS].[pmtcancl].[Roles]);

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

INSERT INTO [CSYS].[pmtcancl].[Roles]([Role],[Description],[UheaaAccess],[FedAccess])
VALUES 
	(@ROLE+1,'ROLE - Operations Supervisor WO Fed',1,0),
	(@ROLE+2,'ROLE - Operations - Team Lead WO Fed',1,0)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 2 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END