
BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		[Reporting].[dbo].[TimeTracking]
	SET
		EndTime = '2015-02-12 17:10:01.363'
		--SELECT * 
		--FROM [Reporting].[dbo].[TimeTracking]
	WHERE
		TimeTrackingId = 35047

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	UPDATE
		[Reporting].[dbo].[TimeTracking]
	SET
		EndTime = '2015-03-03 12:06:44.373'
		--SELECT * 
		--FROM [Reporting].[dbo].[TimeTracking]		
	WHERE
		TimeTrackingId = 35592

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	
	UPDATE
		[Reporting].[dbo].[TimeTracking]
	SET
		EndTime = '2015-02-23 16:56:02.327'
		--SELECT * 
		--FROM [Reporting].[dbo].[TimeTracking]		
	WHERE
		TimeTrackingId = 35272

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
