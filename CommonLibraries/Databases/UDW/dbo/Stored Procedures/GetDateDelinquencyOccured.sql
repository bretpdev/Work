﻿CREATE PROCEDURE [dbo].[GetDateDelinquencyOccured]
(
	@AccountNumber VARCHAR(10)
)
AS
BEGIN

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

    SELECT
		MIN(CAST(LN16.[LD_DLQ_OCC] AS DATETIME)) AS PASTDUE_DATE
	FROM
		PD10_PRS_NME PD10 
		INNER JOIN LN16_LON_DLQ_HST LN16 ON LN16.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN LN10_Loan LN10 ON LN10.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID AND LN10.LN_SEQ = LN16.LN_SEQ
	WHERE
		PD10.DF_SPE_ACC_ID = @AccountNumber
		AND 
		LN16.LC_STA_LON16 = '1'
		AND
		LN10.LA_CUR_PRI > 0
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDateDelinquencyOccured] TO [db_executor]
    AS [dbo];
