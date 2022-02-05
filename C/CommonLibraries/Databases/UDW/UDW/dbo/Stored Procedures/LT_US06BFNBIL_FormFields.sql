﻿
CREATE PROCEDURE [dbo].[LT_US06BFNBIL_FormFields]
	@BF_SSN  CHAR(9)
AS

SELECT
	FB10.LA_REQ_RDC_PAY AS [PrepaidAmount]
FROM
	FB10_BR_FOR_REQ FB10
	INNER JOIN PD10_PRS_NME PD10 ON FB10.BF_SSN = PD10.DF_PRS_ID
WHERE
	PD10.DF_PRS_ID = @BF_SSN

RETURN 0