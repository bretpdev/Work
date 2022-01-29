

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

INSERT INTO
	CSYS..COST_DAT_TimeTracking
		(
			TaskDate,
			[Hours],
			Sr,
			Sasr,
			Lts,
			Pmd,
			Project,
			GenericMeetings,
			BatchScripts,
			FsaCr,
			BillingScript,
			ConversionActivities,
			CostCenter,
			Agent,
			CostCenterId,
			SqlUserId
		)
	SELECT
		TaskDate,
		[Hours],
		Sr,
		Sasr,
		Lts,
		Pmd,
		Project,
		GenericMeetings,
		BatchScripts,
		FsaCr,
		BillingScript,
		ConversionActivities,
		CostCenter,
		Agent,
		CostCenterId,
		SqlUserId
	FROM
		CSYS..COST_DAT_TimeTracking_Trial_Run

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR



IF @ROWCOUNT = 2721 AND @ERROR = 0
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