USE UDW
GO

DECLARE @FYBEGIN DATE = '07/01/2017';
DECLARE @FYEND DATE = '06/30/2018';

SELECT DISTINCT
	PD10.DF_SPE_ACC_ID AS AwardedAccount
FROM
	LN10_LON LN10
	INNER JOIN LN15_DSB LN15
		ON LN10.BF_SSN = LN15.BF_SSN
		AND LN10.LN_SEQ = LN15.LN_SEQ
	INNER JOIN PD10_PRS_NME PD10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	LEFT JOIN SC10_SCH_DMO SC10
		ON LN10.LF_DOE_SCL_ORG = SC10.IF_DOE_SCL
WHERE
	LN10.IC_LON_PGM = 'TILP'
	AND	(LN15.LA_DSB - COALESCE(LN15.LA_DSB_CAN,0)) != 0
	AND LN15.LD_DSB BETWEEN @FYBEGIN AND @FYEND
;

----NH 53191:
--SELECT
--	*
--FROM
--	OPENQUERY(DUSTER,
--		'
--			SELECT DISTINCT
--				PD10.DF_SPE_ACC_ID AS AwardedAccount
--			FROM
--				OLWHRM1.LN10_LON LN10
--				INNER JOIN OLWHRM1.LN15_DSB LN15
--					ON LN10.BF_SSN = LN15.BF_SSN
--					AND LN10.LN_SEQ = LN15.LN_SEQ
--					AND LN15.LD_DSB BETWEEN ''07/01/2016'' AND ''06/30/2017''
--				LEFT JOIN OLWHRM1.SC10_SCH_DMO SC10
--					ON LN10.LF_DOE_SCL_ORG = SC10.IF_DOE_SCL
--				INNER JOIN OLWHRM1.PD10_PRS_NME PD10
--					ON LN10.BF_SSN = PD10.DF_PRS_ID
--			WHERE
--				LN10.IC_LON_PGM = ''TILP''
--				AND
--				(LN15.LA_DSB - COALESCE(LN15.LA_DSB_CAN,0)) != 0
--		'
--	)
