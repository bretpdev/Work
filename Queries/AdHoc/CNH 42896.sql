USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	DROP TABLE IF EXISTS #BwrPop

	SELECT
		PP.PrintProcessingId,
		PP.AccountNumber,
		DENSE_RANK() OVER (PARTITION BY PP.AccountNumber ORDER BY PP.AddedAt) AS RNK --First ltr request = X
	INTO #BwrPop
	FROM
		(
			SELECT
				AccountNumber,
				ScriptFileId,
				COUNT(X) C
			FROM
				CLS.billing.PrintProcessing PP
			WHERE
				CAST(AddedAt AS DATE) > 'X/XX/XXXX'
				AND DeletedAt IS NULL
			GROUP BY
				AccountNumber,
				ScriptFileId
			HAVING
				COUNT(*) >= X
		) BorDupePop
		INNER JOIN CLS.billing.PrintProcessing PP
			ON PP.AccountNumber = BorDupePop.AccountNumber
			AND PP.ScriptFileId = BorDupePop.ScriptFileId
			AND PP.DeletedAt IS NULL
			AND ((PP.PrintedAt IS NULL AND PP.OnEcorr = X) OR EcorrDocumentCreatedAt IS NULL)


	UPDATE
		CLS.billing.PrintProcessing
	SET
		DeletedAt = GETDATE(), DeletedBy = 'CNH XXXXX'
	WHERE
		PrintProcessingId IN 
			(
				SELECT
					PrintProcessingId
				FROM
					#BwrPop
				WHERE
					RNK != X
			)
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = XXX AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END