﻿CREATE PROCEDURE [verforbfed].[GetBorrowerSpecialStatuses]
	@AccountNumber CHAR(10)
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SELECT
		CASE
			WHEN ISNULL(LN60.LI_FOR_VRB_DFL_RUL, 'N') != 'Y' THEN 1 ELSE 0
		END AS LI_FOR_VRB_DFL_RUL 
	FROM
		PD10_PRS_NME PD10 
		INNER JOIN LN16_LON_DLQ_HST LN16 ON LN16.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN LN10_Lon LN10 ON LN10.BF_SSN = PD10.DF_PRS_ID AND LN10.LN_SEQ = LN16.LN_SEQ
		LEFT JOIN LN60_BR_FOR_APV LN60 ON LN60.BF_SSN = LN16.BF_SSN
	WHERE
		PD10.DF_SPE_ACC_ID = @AccountNumber
		AND
		LN16.LN_DLQ_MAX >= 270
		AND 
		LN16.LC_STA_LON16 = '1'
		AND 
		LN10.LA_CUR_PRI > 0
		AND
		LN10.LC_STA_LON10 = 'R'

RETURN 0