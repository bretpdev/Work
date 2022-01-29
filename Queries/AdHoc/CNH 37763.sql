USE [CDW]
GO
/****** Object:  StoredProcedure [dbo].[GetLTXXRecordSequences]    Script Date: X/XX/XXXX X:XX:XX AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetLTXXRecordSequences]
	@DF_SPE_ACC_ID CHAR(XX),
	@RM_APL_PGM_PRC VARCHAR(X),
	@RT_RUN_SRT_DTS_PRC DATETIME,
	@RN_SEQ_LTR_CRT_PRC INT,
	@RM_DSC_LTR_PRC VARCHAR(XX),
	@IsCoborrower BIT = X
AS
BEGIN
	IF @IsCoborrower = X
	BEGIN
		SELECT
			LTXX.RN_SEQ_REC_PRC
		FROM
			LTXX_LTR_REQ_PRC LTXX
			LEFT JOIN CLS.dbo.SpecialLetterRecipients SR
				ON LTXX.RM_DSC_LTR_PRC = SR.LetterId
		WHERE
			LTXX.DF_SPE_ACC_ID = @DF_SPE_ACC_ID
			AND LTXX.RM_APL_PGM_PRC = @RM_APL_PGM_PRC
			AND LTXX.RT_RUN_SRT_DTS_PRC = @RT_RUN_SRT_DTS_PRC
			AND LTXX.RN_SEQ_LTR_CRT_PRC = @RN_SEQ_LTR_CRT_PRC
			AND LTXX.RM_DSC_LTR_PRC = @RM_DSC_LTR_PRC
			AND
			(
				(LTXX.PrintedAt IS NULL AND LTXX.OnEcorr = X)
				OR
				(LTXX.EcorrDocumentCreatedAt IS NULL)
				OR
				(LTXX.PrintedAt IS NULL AND SR.LetterId IS NOT NULL)
			)
			AND InactivatedAt IS NULL
	END

	ELSE
	BEGIN
		SELECT
			LTXX.RN_SEQ_REC_PRC
		FROM
			LTXX_LTR_REQ_PRC_Coborrower LTXX
			LEFT JOIN CLS.dbo.SpecialLetterRecipients SR
				ON LTXX.RM_DSC_LTR_PRC = SR.LetterId
		WHERE
			LTXX.DF_SPE_ACC_ID = @DF_SPE_ACC_ID
			AND LTXX.RM_APL_PGM_PRC = @RM_APL_PGM_PRC
			AND LTXX.RT_RUN_SRT_DTS_PRC = @RT_RUN_SRT_DTS_PRC
			AND LTXX.RN_SEQ_LTR_CRT_PRC = @RN_SEQ_LTR_CRT_PRC
			AND LTXX.RM_DSC_LTR_PRC = @RM_DSC_LTR_PRC
			AND
			(
				(LTXX.PrintedAt IS NULL AND LTXX.OnEcorr = X)
				OR
				(LTXX.EcorrDocumentCreatedAt IS NULL)
				OR
				(LTXX.PrintedAt IS NULL AND SR.LetterId IS NOT NULL)
			)
			AND InactivatedAt IS NULL
	END
END
GO

USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X


DELETE FROM CLS..SpecialLetterRecipients
WHERE LetterId = 'TSXXSKPBOR'

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = X AND @ERROR = X
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