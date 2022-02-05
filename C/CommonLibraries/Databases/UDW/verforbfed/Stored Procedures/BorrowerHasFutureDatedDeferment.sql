﻿CREATE PROCEDURE [verforbfed].[BorrowerHasFutureDatedDeferment]
	@Ssn CHAR(9)
AS

	SELECT 
		CAST(CASE WHEN COUNT(*) > 1 THEN 1 ELSE 0 END AS BIT)
	FROM
		[dbo].DF10_BR_DFR_REQ DF10
		INNER JOIN [dbo].LN50_BR_DFR_APV LN50 ON LN50.BF_SSN = DF10.BF_SSN AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
	WHERE 
		DF10.BF_SSN = @Ssn
		AND 
        DATEDIFF(DAY, LN50.LD_DFR_BEG, GETDATE()) BETWEEN -90 AND 0 		
		AND
		LN50.LC_STA_LON50 = 'A'

RETURN 0