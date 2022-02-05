﻿CREATE PROCEDURE [trdprtyres].[HasOpenLoans]
	@AccountIdentifier VARCHAR(10)
AS
	SELECT
		SUM(LN10.LA_CUR_PRI) AS Balance
	FROM
		UDW..PD10_PRS_NME PD10
		INNER JOIN UDW..LN10_LON LN10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (PD10.DF_PRS_ID, PD10.DF_SPE_ACC_ID)