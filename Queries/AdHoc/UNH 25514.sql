
BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		[Reporting].[dbo].[TimeTracking]
	SET
		EndTime = '2015-11-04 17:54:51.393'
		--SELECT * 
		--FROM [Reporting].[dbo].[TimeTracking]
	WHERE
		TimeTrackingId = 44131

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	UPDATE
		[Reporting].[dbo].[TimeTracking]
	SET
		EndTime = '2015-11-05 17:57:09.533'
		--SELECT * 
		--FROM [Reporting].[dbo].[TimeTracking]		
	WHERE
		TimeTrackingId = 44160

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	
	UPDATE
		[Reporting].[dbo].[TimeTracking]
	SET
		EndTime = '2015-10-28 17:25:43.780'
		--SELECT * 
		--FROM [Reporting].[dbo].[TimeTracking]		
	WHERE
		TimeTrackingId = 43787

	-- Save/Set the row count and error number (if any) from the previously executed statement
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
