--run on NOCHOUSE
BEGIN TRY
	BEGIN TRANSACTION
		--SELECT *, DATEADD(MINUTE,15,StartTime)
		--FROM [Reporting].[dbo].[TimeTracking]
		--WHERE 
		--	TimeTrackingId = 130895
		--	AND SqlUserID = 1682 --John Hyde
		--	AND TicketID = 61027
		--	AND EndTime IS NULL; --still running

		UPDATE
			[Reporting].[dbo].[TimeTracking]
		SET 
			EndTime = DATEADD(MINUTE,15,StartTime)
		WHERE 
			TimeTrackingId = 130895
			AND SqlUserID = 1682 --John Hyde
			AND TicketID = 61027
			AND EndTime IS NULL; --still running			

	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	PRINT 'UNH 62139.sql transaction NOT committed.';
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;