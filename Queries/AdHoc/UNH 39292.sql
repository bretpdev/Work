USE Reporting
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE [dbo].[TimeTracking]
	SET EndTime = '2016-09-01 12:56:11.083'
	WHERE EndTime IS NULL
		AND StartTime = '2016-09-01 12:55:11.083'
		AND SqlUserID = 1633 --Hope Gibson
		AND TicketID = 28769
		AND TimeTrackingId = 57313
		AND Region = 'uheaa'
	;
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR


	UPDATE [dbo].[TimeTracking]
	SET EndTime = '2016-02-03 09:18:29.540'
	WHERE EndTime = '2016-10-11 10:34:54.510'
		AND SqlUserID = 1558 --Peter Busche
		AND TicketID = 25213
		AND TimeTrackingId = 46790
		AND Region = 'uheaa'
	;
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


	UPDATE [dbo].[TimeTracking]
	SET EndTime = '2016-07-28 13:07:49.833'
	WHERE EndTime IS NULL
		AND StartTime = '2016-07-28 11:07:49.833'
		AND SqlUserID = 1448 --Alisia Wixom
		AND TicketID = 28141
		AND TimeTrackingId = 55331
		AND Region = 'uheaa'
	;
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	
	UPDATE [dbo].[TimeTracking]
	SET EndTime = '2016-09-19 06:27:56.783'
	WHERE EndTime IS NULL
		AND StartTime = '2016-09-19 06:25:56.783'
		AND SqlUserID = 1161 --Debbie Phillips
		AND TicketID = 28147
		AND TimeTrackingId = 58057
		AND Region = 'uheaa'
	;
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


	UPDATE [dbo].[TimeTracking]
	SET EndTime = '2016-09-19 12:58:31.170'
	WHERE EndTime IS NULL
		AND StartTime = '2016-09-19 12:56:31.170'
		AND SqlUserID = 1161 --Debbie Phillips
		AND TicketID = 29032
		AND TimeTrackingId = 58091
		AND Region = 'uheaa'
	;
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


	UPDATE [dbo].[TimeTracking]
	SET EndTime = '2016-09-23 07:30:37.987'
	WHERE EndTime IS NULL
		AND StartTime = '2016-09-23 07:28:37.987'
		AND SqlUserID = 1161 --Debbie Phillips
		AND TicketID = 10091
		AND TimeTrackingId = 58416
		AND Region = 'cornerstone'
	;
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


IF @ROWCOUNT = 6 AND @ERROR = 0
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
