﻿CREATE PROCEDURE [finalbfed].[UpdatePrinted]
	@RM_APL_PGM_PRC VARCHAR(8),
	@RT_RUN_SRT_DTS_PRC DATETIME,
	@RN_SEQ_LTR_CRT_PRC INT,
	@RN_SEQ_REC_PRC INT,
	@RM_DSC_LTR_PRC VARCHAR(10),
	@DF_SPE_ACC_ID CHAR(10),
	@IsCoborrower BIT = 0
AS 
BEGIN
IF @IsCoborrower = 0
	BEGIN
		UPDATE
			LT20_LTR_REQ_PRC
		SET
			PrintedAt = GETDATE()
		WHERE
			RM_APL_PGM_PRC = @RM_APL_PGM_PRC
			AND RT_RUN_SRT_DTS_PRC = @RT_RUN_SRT_DTS_PRC
			AND RN_SEQ_LTR_CRT_PRC = @RN_SEQ_LTR_CRT_PRC
			AND RN_SEQ_REC_PRC = @RN_SEQ_REC_PRC
			AND RM_DSC_LTR_PRC = @RM_DSC_LTR_PRC
			AND DF_SPE_ACC_ID = @DF_SPE_ACC_ID
	END

ELSE
	BEGIN
		UPDATE
			LT20_LTR_REQ_PRC_Coborrower
		SET
			PrintedAt = GETDATE()
		WHERE
			RM_APL_PGM_PRC = @RM_APL_PGM_PRC
			AND RT_RUN_SRT_DTS_PRC = @RT_RUN_SRT_DTS_PRC
			AND RN_SEQ_LTR_CRT_PRC = @RN_SEQ_LTR_CRT_PRC
			AND RN_SEQ_REC_PRC = @RN_SEQ_REC_PRC
			AND RM_DSC_LTR_PRC = @RM_DSC_LTR_PRC
			AND DF_SPE_ACC_ID = @DF_SPE_ACC_ID
	END
END
