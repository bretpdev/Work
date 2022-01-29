
BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	--XXX
	UPDATE
		PN
	SET
		PN.ResolvedAt = GETDATE(),
		PN.ResolvedBy = 'DCR XXXXX'
	--SELECT *
	FROM
		ProcessLogs..ProcessNotifications PN
		INNER JOIN
		(
			SELECT
				PLM.ProcessNotificationId
			FROM
				ULS..ArcAddProcessing AAP
				INNER JOIN ULS..ArcAddProcessLoggerMapping MAP ON MAP.ArcAddProcessingId = AAP.ArcAddProcessingId
				INNER JOIN ProcessLogs..ProcessNotifications PN ON PN.ProcessLogId = MAP.ProcessLogId AND PN.ProcessNotificationId = MAP.ProcessNotificationId
				INNER JOIN ULS.[log].ProcessLogMessages PLM ON PLM.ProcessNotificationId = PN.ProcessNotificationId
			WHERE
				AAP.ProcessOn < GETDATE()
				AND
				AAP.ProcessingAttempts >= X
				AND
				PN.NotificationSeverityTypeId != X
				AND
				PN.ResolvedBy IS NULL
				AND
				AAP.LN_ATY_SEQ IS NULL
		) N ON N.ProcessNotificationId = PN.ProcessNotificationId


	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	--XXXX
	UPDATE
		PN
	SET
		PN.ResolvedAt = GETDATE(),
		PN.ResolvedBy = 'DCR '
	--SELECT *
	FROM
		ProcessLogs..ProcessNotifications PN
		INNER JOIN
		(
			SELECT
				PLM.ProcessNotificationId
			FROM
				CLS..ArcAddProcessing AAP
				INNER JOIN CLS..ArcAddProcessLoggerMapping MAP ON MAP.ArcAddProcessingId = AAP.ArcAddProcessingId
				INNER JOIN ProcessLogs..ProcessNotifications PN ON PN.ProcessLogId = MAP.ProcessLogId AND PN.ProcessNotificationId = MAP.ProcessNotificationId
				INNER JOIN CLS.[log].ProcessLogMessages PLM ON PLM.ProcessNotificationId = PN.ProcessNotificationId
			WHERE
				AAP.ProcessOn < GETDATE()
				AND
				AAP.ProcessingAttempts >= X
				AND
				PN.NotificationSeverityTypeId != X
				AND
				PN.ResolvedBy IS NULL
				AND
				AAP.LN_ATY_SEQ IS NULL
		) N ON N.ProcessNotificationId = PN.ProcessNotificationId

	-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

-- XXX
--XXXX
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
