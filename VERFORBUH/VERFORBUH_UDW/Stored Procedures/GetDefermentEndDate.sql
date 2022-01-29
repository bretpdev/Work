﻿CREATE PROCEDURE [verforbuh].[GetDefermentEndDate]
	@Ssn CHAR(9)
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SELECT
		MAX(CAST(LN50.LD_DFR_END AS DATETIME))
	FROM
		[dbo].DF10_BR_DFR_REQ DF10
		INNER JOIN [dbo].LN50_BR_DFR_APV LN50 ON LN50.BF_SSN = DF10.BF_SSN AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM 
	WHERE 
		DF10.BF_SSN = @Ssn
		AND
		DF10.LC_DFR_STA = 'A'
		AND
		LN50.LC_STA_LON50 = 'A'

RETURN 0
