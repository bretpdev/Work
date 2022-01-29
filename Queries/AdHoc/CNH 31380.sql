USE CDW
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		LTXX
	SET
		PrintedAt = NULL
	FROM
		[CDW].[dbo].[LTXX_LetterRequests] LTXX
		LEFT JOIN [CDW].[dbo].[PHXX_ContactEmail] PHXX ON LTXX.DF_SPE_ACC_ID = PHXX.DF_SPE_ACC_ID
	WHERE
		LTXX.RM_DSC_LTR_PRC = 'TSXXBQRTLY'
		AND LTXX.InactivatedAt IS NULL
		AND LTXX.RT_RUN_SRT_DTS_PRC > 'X/X/XX'
		AND LTXX.PrintedAt IS NOT NULL
		AND (PHXX.DI_CNC_ELT_OPI = X OR (PHXX.DI_CNC_ELT_OPI = X	AND PHXX.DI_VLD_CNC_EML_ADR = X))
		AND CAST(LTXX.PrintedAt AS DATE) = 'X/X/XX'

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = XXXX AND @ERROR = X
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