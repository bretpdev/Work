USE PLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	DECLARE @Count INT = (SELECT COUNT(UserName) FROM crpqassign.Users WHERE UserName IN ('P616050','P616049','P610746','P610563','P612795','P613093','P613397','P614420','P616366','P616564','P618018'))

	DELETE
		users
	FROM
		crpqassign.Users users
	WHERE
		users.UserName IN ('P616050','P616049','P610746','P610563','P612795','P613093','P613397','P614420','P616366','P616564','P618018')

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	INSERT INTO crpqassign.Users(UserName, AgentName)
	VALUES('P617898','Krystel Porter')
	,('P618175','Ursula Davies')

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = @Count + 2 AND @ERROR = 0
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