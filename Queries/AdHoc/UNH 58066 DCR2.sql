USE BSYS

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

--2 Set SCKR_REF_StatusPIR end date to current date
	UPDATE
		BSYS..[SCKR_REF_StatusPIR]
	SET
		[End] = GETDATE()
		
	WHERE
		[Sequence] IN (
						SELECT
							STA.Sequence
						FROM
							( 
								SELECT DISTINCT
									PIR.Request,
									PIR.Class
								FROM 
									[BSYS].[dbo].[SCKR_DAT_PIR] PIR
								WHERE
									PIR.CurrentStatus NOT IN ('Complete', 'Closed', 'Returned')
									AND DATEDIFF(MONTH,PIR.CurrentStatusDate,GETDATE()) > 6
							) PIR
							INNER JOIN [BSYS].[dbo].[SCKR_REF_StatusPIR] STA
								ON PIR.Request = STA.Request
								AND PIR.Class = STA.Class
						WHERE
							STA.[Status] NOT IN ('Complete', 'Closed', 'Returned')
							AND STA.[End] IS NULL
						)

	-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR






IF @ROWCOUNT = 2468 AND @ERROR = 0
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