USE ProcessLogs
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		DD
	SET
		DD.Printed = NULL
	--SELECT *
	FROM
		[ECorrFed].[dbo].[DocumentDetails] DD
		INNER JOIN
		(
			SELECT
				DD.DocumentDetailsId,
				LTRIM(RTRIM(SUBSTRING(DD.[Path], XX, PATINDEX('%.pdf%',DD.[Path]) + X))) [FName]
			FROM
				[ECorrFed].[dbo].[DocumentDetails] DD
			WHERE
				DD.CreateDate > DATEADD(MONTH, -X, GETDATE())
		) X ON X.DocumentDetailsId = DD.DocumentDetailsId
		INNER JOIN 
		(
			SELECT
				PLM.LogMessage,
				LTRIM(RTRIM(SUBSTRING(PLM.LogMessage, XX, PATINDEX('%.pdf%', PLM.LogMessage) + X - XX))) [FName]
			FROM
				ProcessLogs PL
				INNER JOIN ProcessNotifications PN on PN.ProcessLogId = PL.ProcessLogId
				INNER JOIN CLS.[log].ProcessLogMessages PLM on PLM.ProcessNotificationId = PN.ProcessNotificationId
			WHERE
				PL.ScriptId LIKE 'E-Corr XML'
				AND
				PL.ProcessLogId IN 
				(
					'XXXXXX',
					'XXXXXX',
					'XXXXXX'
				)
		) Y on Y.FName = X.FName

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = XXXXX AND @ERROR = X
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
