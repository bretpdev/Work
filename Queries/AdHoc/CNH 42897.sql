USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	
	DROP TABLE IF EXISTS #BwrPop

	SELECT
		PP.PrintProcessingId,
		PP.AccountNumber,
		DENSE_RANK() OVER (PARTITION BY PP.AccountNumber, PP.ScriptDataId ORDER BY PP.AddedAt) AS RNK --First ltr request = X
	INTO #BwrPop
	FROM
		(
			SELECT
				AccountNumber,
				ScriptDataId,
				COUNT(X) C
			FROM
				CLS.[print].PrintProcessing
			WHERE
				DeletedAt IS NULL
				AND 
					(
						(PrintedAt IS NULL AND OnEcorr = X)
					OR
						(EcorrDocumentCreatedAt IS NULL)
					)
			GROUP BY
				AccountNumber, 
				ScriptDataId
			HAVING 
				COUNT(X) > X
		) BorDupePop
	INNER JOIN CLS.[print].PrintProcessing PP
		ON PP.AccountNumber = BorDupePop.AccountNumber
		AND PP.ScriptDataId = BorDupePop.ScriptDataId
		AND PP.DeletedAt IS NULL
		AND ((PP.PrintedAt IS NULL AND PP.OnEcorr = X) OR EcorrDocumentCreatedAt IS NULL)


	UPDATE
		CLS.[print].PrintProcessing
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

	UPDATE
		CLS.[print].PrintProcessing
	SET
		DeletedAt = GETDATE(), DeletedBy = 'CNH XXXXX'
	WHERE
		PrintProcessingId = XXXXXXX

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

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
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END