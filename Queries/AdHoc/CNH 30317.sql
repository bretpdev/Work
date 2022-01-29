USE NobleCalls
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		NCH
	SET
		NCH.Deleted = X,
		NCH.DeletedAt = GETDATE(),
		NCH.DeletedBy = 'UNH XXXXX'
	--SELECT *
	FROM
		NobleCallHistory NCH
	WHERE
		NCH.AccountIdentifier = 'XXXXXXXXXX'
		AND
		CAST(NCH.CreatedAt AS DATE) = CAST(DATEADD(DAY, -X, GETDATE()) AS DATE)
		AND
		NCH.IsReconciled = X


	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = XX AND @ERROR = X
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
