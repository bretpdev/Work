CREATE PROCEDURE [verforbfed].[GetMaxForbMonthsUsed]
	@AccountNumber CHAR(10)
AS

	SELECT
		ISNULL(MAX(SUM_MONTHS.MONTHS), 0)
	FROM 
		[dbo].[FB10_Forbearance] FORB
		INNER JOIN
		(
			SELECT
				DF_SPE_ACC_ID,
				SUM([MONTHS_USED]) AS MONTHS
			FROM
				[dbo].[FB10_Forbearance]
			WHERE 
				DF_SPE_ACC_ID = @AccountNumber
				AND LC_FOR_TYP = '05'
			GROUP BY
				DF_SPE_ACC_ID,
				LN_SEQ
		)SUM_MONTHS
		ON SUM_MONTHS.DF_SPE_ACC_ID = FORB.DF_SPE_ACC_ID
	WHERE 
		FORB.DF_SPE_ACC_ID = @AccountNumber
		AND FORB.LC_FOR_TYP = '05'

RETURN 0