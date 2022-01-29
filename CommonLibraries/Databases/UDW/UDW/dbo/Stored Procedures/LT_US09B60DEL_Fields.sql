﻿CREATE PROCEDURE LT_US09B60DEL_Fields
	@AccountNumber char(10)
AS
BEGIN

	SELECT
		MIN(DATEADD(DAY, 270, LN16.LD_DLQ_OCC))
	FROM
		UDW.[dbo].[LN16_LON_DLQ_HST] LN16
		JOIN [dbo].[PD10_PRS_NME] PD10 ON LN16.BF_SSN = PD10.DF_PRS_ID
	WHERE
		PD10.DF_SPE_ACC_ID = @AccountNumber
		AND LC_STA_LON16 = 1

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_US09B60DEL_Fields] TO [db_executor]
    AS [dbo];

