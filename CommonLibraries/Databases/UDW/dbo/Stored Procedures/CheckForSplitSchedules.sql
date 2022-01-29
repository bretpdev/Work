
CREATE PROCEDURE [dbo].[CheckForSplitSchedules]
	@AccountNumber char(10)
AS
	SELECT
		COUNT(*) - i.idrcount
	FROM 
		LN65_RepaymentSched l
		LEFT JOIN 
		(
			SELECT
				l.DF_SPE_ACC_ID,
				count(*) as idrcount
			FROM
				LN65_RepaymentSched l
				INNER JOIN IDRRepaymentSchedules i
					ON i.IDRRepaymentSchedule = l.TYP_SCH_DIS
			WHERE
				DF_SPE_ACC_ID = @AccountNumber
			GROUP BY l.DF_SPE_ACC_ID
		
		)i
		ON i.DF_SPE_ACC_ID = l.DF_SPE_ACC_ID
	WHERE
		l.DF_SPE_ACC_ID = @AccountNumber
	GROUP BY 
		i.idrcount

RETURN 0