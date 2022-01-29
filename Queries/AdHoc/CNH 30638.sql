USE CLS

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		ArcAddProcessing
	SET
		ProcessedAt = NULL
	WHERE
		ArcAddProcessingId IN
		(
			SELECT
				SUBSTRING(PLM.LogMessage, PATINDEX('%ArcAddId: %', PLM.LogMessage) + XX, X) [ArcAddProcessingId]
			FROM
				ProcessLogs..ProcessLogs PL
				INNER JOIN ProcessLogs..ProcessNotifications PN ON PN.ProcessLogId = PL.ProcessLogId
				INNER JOIN cls.[log].ProcessLogMessages PLM ON PLM.ProcessNotificationId = PN.ProcessNotificationId
			WHERE
				PLM.LogMessage LIKE 'Error adding % arc to borrower:%'
				AND
				PLM.LogMessage NOT LIKE '%RECORD ALREADY EXISTS%'
				AND
				CAST(PL.StartedOn AS DATE) = 'XXXX-XX-XX'
		)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = XXXX AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END
