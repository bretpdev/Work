USE BSYS

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

--1 Add SCKR_REF_StatusPR Complete status record
	INSERT INTO
		[BSYS].[dbo].[SCKR_REF_StatusPIR] ([Request],[Class],[Status],[Begin],[End],[Court])
	SELECT
		DISTINCT
		STA.Request AS Request,
		STA.Class AS [Class],
		'Complete' AS [Status],
		GETDATE() AS [Begin],
		NULL AS [End],
		'' AS [Court]
	FROM 
		[BSYS].[dbo].[SCKR_DAT_PIR] PIR
		INNER JOIN [BSYS].[dbo].[SCKR_REF_StatusPIR] STA
			ON PIR.Request = STA.Request
			AND PIR.Class = STA.Class
	WHERE
		PIR.CurrentStatus NOT IN ('Complete', 'Closed', 'Returned')
		AND DATEDIFF(MONTH,PIR.CurrentStatusDate,GETDATE()) > 6
	-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR



IF @ROWCOUNT = 2461 AND @ERROR = 0
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

