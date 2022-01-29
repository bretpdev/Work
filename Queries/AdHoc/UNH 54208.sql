USE ServicerInventoryMetrics
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	DECLARE @FirstUser INT = (SELECT AllowedUserId FROM AllowedUsers WHERE AllowedUser = 'tvig') 
	
	UPDATE 
		AllowedUserAccessGroupMapping
	SET
		AccessGroupId = 18
	WHERE
		AllowedUserId = @FirstUser

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	DECLARE @SecondUser INT
	INSERT INTO AllowedUsers(AllowedUser, IsAdmin)
	VALUES('pbusche', 1)

	SET @SecondUser = @@IDENTITY

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	INSERT INTO AllowedUserAccessGroupMapping(AccessGroupId, AllowedUserId)
	VALUES(18, @SecondUser)

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

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
