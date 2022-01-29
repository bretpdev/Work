USE Reporting
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE [dbo].[TimeTracking]
	SET EndTime = '2016-07-21 17:00:00.000'
	WHERE EndTime = '2016-07-22 18:19:46.553'
		AND StartTime = '2016-07-21 14:59:39.387'
		AND SqlUserID = 1449 --J. Scott Briggs
		AND TicketID = 9750
		AND TimeTrackingId = 55050
		AND Region = 'cornerstone'
	;
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

		
	UPDATE [dbo].[TimeTracking]
	SET EndTime = '2016-07-06 17:00:00.000'
	WHERE EndTime = '2016-07-07 11:21:36.230'
		AND StartTime = '2016-07-06 10:19:46.663'
		AND SqlUserID = 1449 --J. Scott Briggs
		AND TicketID = 9642
		AND TimeTrackingId = 54222
		AND Region = 'cornerstone'
	;		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	
	UPDATE [dbo].[TimeTracking]
	SET EndTime = '2016-07-20 17:00:00.000'
	WHERE EndTime = '2016-07-21 13:16:30.703'
		AND StartTime = '2016-07-20 15:44:06.457'
		AND SqlUserID = 1449 --J. Scott Briggs
		AND TicketID = 9750
		AND TimeTrackingId = 54987
		AND Region = 'cornerstone'
	;		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	
	
	UPDATE [dbo].[TimeTracking]
	SET EndTime = '2016-07-07 17:00:00.000'
	WHERE EndTime = '2016-07-08 09:53:33.987'
		AND StartTime = '2016-07-07 16:08:09.250'
		AND SqlUserID = 1449 --J. Scott Briggs
		AND TicketID = 9575
		AND TimeTrackingId = 54309
		AND Region = 'cornerstone'
	;		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	
IF @ROWCOUNT = 4 AND @ERROR = 0
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
