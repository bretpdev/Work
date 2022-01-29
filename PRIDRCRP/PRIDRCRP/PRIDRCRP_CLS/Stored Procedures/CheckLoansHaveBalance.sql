﻿CREATE PROCEDURE [pridrcrp].[CheckLoansHaveBalance]
(
	@AccountNumber VARCHAR(10)
)
AS
	SELECT 
		CAST(MAX(CASE WHEN LN10.LA_CUR_PRI > 0.00 AND LN10.LC_STA_LON10 = 'R' THEN 1 ELSE 0 END) AS BIT) AS HasBalance
	FROM
		CDW..LN10_LON LN10
		INNER JOIN CDW..PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
	WHERE
		PD10.DF_SPE_ACC_ID = @AccountNumber

RETURN 0