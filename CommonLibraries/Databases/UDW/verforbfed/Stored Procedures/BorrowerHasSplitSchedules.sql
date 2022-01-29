﻿CREATE PROCEDURE [verforbfed].[BorrowerHasSplitSchedules]
	@Ssn CHAR(9)
AS

	SELECT
		CAST(CASE WHEN (COUNT(*) - IDR.IdrCount) > 0 THEN 1 ELSE 0 END AS BIT) 
	FROM 
		LN65_LON_RPS LN65
		LEFT JOIN 
		(
			SELECT
				LN65.BF_SSN,
				COUNT(*) as IdrCount
			FROM
				LN65_LON_RPS LN65
				INNER JOIN IDRRepaymentSchedules IDR ON IDR.IDRRepaymentSchedule = LN65.LC_TYP_SCH_DIS
			WHERE
				BF_SSN = @Ssn
			GROUP BY LN65.BF_SSN
		
		) IDR
		ON IDR.BF_SSN = LN65.BF_SSN
	WHERE
		LN65.BF_SSN = @Ssn
	GROUP BY 
		IDR.IdrCount

RETURN 0