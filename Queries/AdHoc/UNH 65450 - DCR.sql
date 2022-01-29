GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @Expected INT = 3

DELETE FROM 
	ULS.taskprocessing.UheaaQueues 
WHERE 
	UheaaQueueId IN
	(
		SELECT MAX(UQ.UheaaQueueId) FROM 
			ULS.taskprocessing.UheaaQueues UQ 
			INNER JOIN ULS.taskprocessing.UheaaQueues UQ2 
				ON UQ.BusinessUnit = UQ2.BusinessUnit 
				AND UQ.Queue = UQ2.Queue 
				AND UQ.ComplianceDays = UQ2.ComplianceDays
				AND UQ.UheaaQueueId != UQ2.UheaaQueueId
		GROUP BY
			UQ.BusinessUnit,
			UQ.Queue,
			UQ.ComplianceDays
	)

-- Save/Set the row count and error number (if any) from the previously executed statement
SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @Expected AND @ERROR = 0
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