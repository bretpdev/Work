﻿CREATE PROCEDURE [verforbfed].[GetTotalForbDaysUsed]
	@Ssn CHAR(9)
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT
	MAX(M.MONTHS_USED)
FROM
	(
		SELECT
			LN60.BF_SSN,
			LN60.LF_FOR_CTL_NUM,
			LN60.LN_SEQ,
			LN60.LD_FOR_BEG,
			LN60.LD_FOR_END,
			ROUND(SUM((DATEDIFF(DAY,LN60.LD_FOR_BEG, LN60.LD_FOR_END + 1)/365.00)*12) OVER (PARTITION BY LN60.BF_SSN, LN60.LN_SEQ), 1) AS MONTHS_USED
		FROM
			FB10_BR_FOR_REQ FB10
			INNER JOIN LN60_BR_FOR_APV LN60
				ON FB10.BF_SSN = LN60.BF_SSN
				AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
				AND LN60.LC_STA_LON60 = 'A'
			INNER JOIN LN10_LON LN10
				ON LN60.BF_SSN = LN10.BF_SSN
				AND LN60.LN_SEQ = LN10.LN_SEQ
		WHERE
			FB10.LC_FOR_TYP = '05'
			AND FB10.LC_FOR_STA = 'A'
			AND FB10.LC_STA_FOR10 = 'A'
			AND LN60.LC_FOR_RSP != '003'
			AND FB10.BF_SSN = @Ssn
			AND LN10.LA_CUR_PRI > 0
		GROUP BY
			LN60.BF_SSN,
			LN60.LN_SEQ,
			LN60.LF_FOR_CTL_NUM,
			LN60.LD_FOR_BEG,
			LN60.LD_FOR_END
	) M