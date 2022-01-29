USE CLS
GO


BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X


	-- Add the files to the EcorrCategories
	INSERT INTO [CLS].[print].EcorrCategories(ScriptFileId, EcorrFieldName)
	VALUES(X, '[DI_CNC_EBL_OPI ]'),
	(X, '[DI_CNC_EBL_OPI ]'),
	(X, '[DI_CNC_EBL_OPI ]'),
	(XX, '[DI_CNC_EBL_OPI ]'),
	(XX, '[DI_CNC_EBL_OPI ]')

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	-- Update all the Ecorr fields for the records from the RXX file on X/X/XX 
	DECLARE @Sql varchar(XXXX)
	SET @Sql = 
		N'
			UPDATE
				PP
			SET
				PP.OnEcorr = 
					(
						CASE
							WHEN ISNULL(PHXX.[DI_CNC_EBL_OPI ], X) = X AND ISNULL(PHXX.DI_VLD_CNC_EML_ADR, X) = X
							THEN X
							ELSE X
						END
					)
			FROM
				[CLS].[print].PrintProcessing PP
				LEFT JOIN [CDW].[dbo].PHXX_ContactEmail PHXX ON PP.AccountNumber = PHXX.DF_SPE_ACC_ID
			WHERE
				CAST(PP.AddedAt AS DATE) = ''X/X/XX''
				AND PP.ScriptFileId = XX
		'	
	EXEC sp_sqlexec @Sql

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


	-- Set the PrintedAt field to null for all records in the RXX file on X/X/XX that are not on Ecorr
	UPDATE
		[print].PrintProcessing
	SET
		PrintedAt = NULL
	WHERE
		CAST(AddedAt AS DATE) = 'X/X/XX'
		AND ScriptFileId = XX
		AND OnEcorr = X
		
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	-- Update the DocumentDetails CorrMethod to EmailNotify for all records that were updated to OnEcorr = X in PrintProcess for the RXX file on X/X/XX
	UPDATE
		DD
	SET
		DD.CorrMethod = 'Email Notify'
	FROM
		[ECorrFed]..DocumentDetails DD
		JOIN [CLS].[print].PrintProcessing PP ON DD.ADDR_ACCT_NUM = PP.AccountNumber
	WHERE
		DD.DocDate = 'X/X/XX'
		AND DD.LetterId = XXXX
		AND PP.ScriptFileId = XX
		AND CAST(PP.AddedAt AS DATE) = 'X/X/XX'
		AND PP.OnEcorr = X
		
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = XXXXXX AND @ERROR = X
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


---------- Test Queries

-- Number of records affected: XXXXXX
--SELECT
--	*
--FROM
--	[CLS].[print].PrintProcessing
--WHERE
--	CAST(AddedAt AS DATE) = 'X/X/XX'
--	AND ScriptFileId = XX


-- Number of records being updated to OnEcorr = X: XXXXXX
--SELECT
--	COUNT(
--		CASE
--			WHEN ISNULL(PHXX.[DI_CNC_EBL_OPI ], X) = X AND ISNULL(PHXX.DI_VLD_CNC_EML_ADR, X) = X
--			THEN X
--			ELSE NULL
--		END
--	)
--FROM
--	[CLS].[print].PrintProcessing PP
--	LEFT JOIN [CDW].[dbo].PHXX_ContactEmail PHXX ON PP.AccountNumber = PHXX.DF_SPE_ACC_ID
--WHERE
--	CAST(PP.AddedAt AS DATE) = 'X/X/XX'
--	AND ScriptFileId = XX

-- Number of records havig PrintedAt set to null: XXXXX
--SELECT
--	COUNT(
--		CASE
--			WHEN ISNULL(PHXX.[DI_CNC_EBL_OPI ], X) = X AND ISNULL(PHXX.DI_VLD_CNC_EML_ADR, X) = X
--			THEN NULL
--			ELSE X
--		END
--	)
--FROM
--	[CLS].[print].PrintProcessing PP
--	LEFT JOIN [CDW].[dbo].PHXX_ContactEmail PHXX ON PP.AccountNumber = PHXX.DF_SPE_ACC_ID
--WHERE
--	CAST(PP.AddedAt AS DATE) = 'X/X/XX'
--	AND ScriptFileId = XX

-- Number of records being updated to EmailNotify: XXXXXX
--SELECT
--	DD.DocumentDetailsId,
--	DD.ADDR_ACCT_NUM,
--	PP.AccountNumber,
--	PP.PrintProcessingId,
--	PP.SourceFile,
--	PP.AddedAt
--FROM
--	[ECorrFed]..DocumentDetails DD
--	INNER JOIN [CLS].[print].PrintProcessing PP ON DD.ADDR_ACCT_NUM = PP.AccountNumber
--WHERE
--	DD.DocDate = 'X/X/XX'
--	AND DD.LetterId = XXXX
--	AND PP.ScriptFileId = XX
--	AND CAST(PP.AddedAt AS DATE) = 'X/X/XX'
--GROUP BY
--	DD.DocumentDetailsId,
--	DD.ADDR_ACCT_NUM,
--	PP.AccountNumber,
--	PP.PrintProcessingId,
--	PP.SourceFile,
--	PP.AddedAt