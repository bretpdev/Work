USE CDW
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	DECLARE @Borrowers TABLE
	(
		AccountNumber CHAR(XX)
	)

	INSERT INTO @Borrowers
	(
		AccountNumber
	)
	VALUES
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX'),
		('XXXXXXXXXX')


		UPDATE
			LTXX
		SET
			PrintedAt = NULL
		--SELECT *
		FROM
			CDW..LTXX_LetterRequests LTXX
			INNER JOIN @Borrowers B on B.AccountNumber = LTXX.DF_SPE_ACC_ID
		WHERE
			CAST(LTXX.PrintedAt as DATE) = 'XXXX/XX/XX'
	

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
