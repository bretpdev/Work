USE PLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	DELETE FROM PLS.crpqassign.Users
	WHERE UserName IN ('P617900','P617943','P619699','P618179','P621386','P610746','P610568','P619013','P618369','P619088','P619697','P621115')

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	INSERT INTO PLS.crpqassign.Users(UserName, AgentName)
	VALUES('P624614','Josh Cook')
	,('P623182','Chase Hansen')
	,('P622753','Megan Ostlie')

	SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

IF @ROWCOUNT = 15 AND @ERROR = 0
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