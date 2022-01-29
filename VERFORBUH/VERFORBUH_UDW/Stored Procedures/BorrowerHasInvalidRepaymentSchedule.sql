﻿CREATE PROCEDURE [verforbuh].[BorrowerHasInvalidRepaymentSchedule]
	@Ssn CHAR(9)
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
    SELECT
		CAST(CASE WHEN COUNT(DISTINCT LN65.LC_TYP_SCH_DIS) > 0 THEN 1 ELSE 0 END AS BIT)
	FROM
		[dbo].LN65_LON_RPS LN65
		INNER JOIN IDRRepaymentSchedules IDR ON IDR.IDRRepaymentSchedule = LN65.LC_TYP_SCH_DIS
	WHERE 
		BF_SSN = @Ssn
		AND
		LN65.LC_STA_LON65 = 'A'

RETURN 0
