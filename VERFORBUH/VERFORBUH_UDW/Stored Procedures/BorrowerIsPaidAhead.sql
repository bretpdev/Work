﻿CREATE PROCEDURE [verforbuh].[BorrowerIsPaidAhead]
	@AccountNumber CHAR(10)
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SELECT
		CAST(
			CASE WHEN DATEDIFF(DAY, GETDATE(), MAX(LN80.LD_BIL_DU_LON)) > 30 THEN 1 ELSE 0 END 
		AS BIT)
	FROM
		LN80_LON_BIL_CRF LN80
		INNER JOIN PD10_PRS_NME PD10 ON PD10.DF_PRS_ID = BF_SSN
		INNER JOIN LN10_Lon LN10 ON LN10.BF_SSN = PD10.DF_PRS_ID AND LN10.LN_SEQ = LN80.LN_SEQ          
		INNER JOIN DW01_DW_CLC_CLU DW01 ON DW01.BF_SSN = LN80.BF_SSN
	WHERE  
		PD10.DF_SPE_ACC_ID = @AccountNumber
		AND 
		LN80.LC_STA_LON80 = 'A'
		AND
		LN10.LC_STA_LON10 = 'R' 
		AND 
		DATEDIFF(DAY, GETDATE(), LD_BIL_DU_LON) > 30
		AND
		LN10.LA_CUR_PRI > 0
		AND
		DW01.WC_DW_LON_STA = '03'


RETURN 0
