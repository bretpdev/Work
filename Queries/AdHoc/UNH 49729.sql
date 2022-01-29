USE TLP
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	DELETE FROM
		SL
	FROM
		[TLP].[dbo].[SchoolSlot] SL
		INNER JOIN SchoolInformation SI ON SI.ID = SL.SchoolInfoID
		INNER JOIN ParticipatingSchoolsList PSL ON PSL.SchoolCode = SI.SchoolCode
		LEFT JOIN LoanDat LD ON LD.SchoolCode = SI.SchoolCode AND LD.SSN = SL.SSN AND LD.LoanStatus != 'Rejected' AND CAST(LD.DisbDate AS DATE) BETWEEN '07/01/2016' AND '06/30/2017'
	WHERE
		SL.SlotStatus = 'A'
		AND
		CAST(SL.SlotStartDt AS DATE) BETWEEN '07/01/2016' AND '06/30/2017'
		AND
		SI.SchoolCode IN ('00367000', '00368000')
		AND
		LD.SSN IS NULL

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 20 AND @ERROR = 0
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
