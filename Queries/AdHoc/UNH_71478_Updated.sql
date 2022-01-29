USE ODW
GO

DROP TABLE IF EXISTS #BASE_POP

SELECT DISTINCT
	pd01.DF_PRS_ID,
	PD01.DF_SPE_ACC_ID AS [Account Number],
	ISNULL(DC11.LD_TRX_EFF, RM10.LX_RMT_TRX_EFF_DTE) AS [Effective Date],
	ISNULL(DC11.LA_TRX, RM10.LX_RMT_TRX_AMT) AS [Transaction Amount],
	DC11.LA_APL_PRI AS [Principal Applied],
	DC11.LA_APL_INT AS [Interest Applied],
	DC11.LA_APL_COL_CST AS [Collection Costs Applied]
	--DC11.LA_PRI_AT_PST AS [Principal When Posted],
	--DC11.LA_INT_ACR_THS_PRD AS [Interest Accrued Through Period],
	--DC11.LA_COL_CST AS [Amount Of Collection Costs]
INTO #BASE_POP
FROM
	PD01_PDM_INF PD01
	INNER JOIN DC01_LON_CLM_INF DC01
		ON DC01.BF_SSN = PD01.DF_PRS_ID
	LEFT JOIN DC11_LON_FAT DC11
		ON DC11.BF_SSN = PD01.DF_PRS_ID
		AND DC11.LC_TRX_TYP IN ('RH')
		AND DC11.LD_TRX_EFF >= '03/13/2020' 
		AND 
		(
			DC11.LD_TRX_ADJ IS NULL
			OR (DC11.LD_TRX_ADJ IS NOT NULL AND DC11.LC_REV_IND_TYP != '')
		)
	LEFT JOIN 
	(
		SELECT
			*
		FROM
			OPENQUERY (DUSTER,
			'
				SELECT
					LF_PRS_ID,
					LC_RMT_TRX_TYP,
					LX_RMT_TRX_EFF_DTE,
					LX_RMT_TRX_AMT
				FROM
					OLWHRM1.RM10_RMT_SUS_DTL
				WHERE
					LC_RMT_TRX_TYP IN (''RH'')
					AND LX_RMT_TRX_EFF_DTE >= ''03/13/2020''
					AND LC_RMT_STA = ''S''
			')
	) RM10
		ON RM10.LF_PRS_ID = PD01.DF_PRS_ID
WHERE
	(
		DC11.BF_SSN IS NOT NULL
		OR RM10.LF_PRS_ID IS NOT NULL
	)
	AND DC01.LF_CLM_ID != ''


ORDER BY
	PD01.DF_SPE_ACC_ID ASC


SELECT
	BP.*
FROM
	#BASE_POP BP
	--INNER JOIN 
	--(
	--	SELECT DISTINCT
	--		pd01.DF_PRS_ID,
	--		PD01.DF_SPE_ACC_ID AS [Account Number]
	--	FROM
	--		PD01_PDM_INF PD01
	--		INNER JOIN DC01_LON_CLM_INF DC01
	--			ON DC01.BF_SSN = PD01.DF_PRS_ID
	--		LEFT JOIN DC11_LON_FAT DC11
	--			ON DC11.BF_SSN = PD01.DF_PRS_ID
	--			AND DC11.LC_TRX_TYP IN ('SO','FO','GP')
	--			AND DC11.LD_TRX_EFF >= '03/13/2020' 
	--			AND 
	--			(
	--				DC11.LD_TRX_ADJ IS NULL
	--				OR (DC11.LD_TRX_ADJ IS NOT NULL AND DC11.LC_REV_IND_TYP != '')
	--			)
	--		LEFT JOIN 
	--		(
	--			SELECT
	--				*
	--			FROM
	--				OPENQUERY (DUSTER,
	--				'
	--					SELECT
	--						LF_PRS_ID,
	--						LC_RMT_TRX_TYP,
	--						LX_RMT_TRX_EFF_DTE,
	--						LX_RMT_TRX_AMT
	--					FROM
	--						OLWHRM1.RM10_RMT_SUS_DTL
	--					WHERE
	--						LC_RMT_TRX_TYP IN (''SO'',''FO'',''GP'')
	--						AND LX_RMT_TRX_EFF_DTE >= ''03/13/2020''
	--						AND LC_RMT_STA = ''S''
	--				')
	--		) RM10
	--			ON RM10.LF_PRS_ID = PD01.DF_PRS_ID
	--	WHERE
	--		(
	--			DC11.BF_SSN IS NOT NULL
	--			OR RM10.LF_PRS_ID IS NOT NULL
	--		)
	--		AND DC01.LF_CLM_ID != ''
	--) OP
	--	ON BP.DF_PRS_ID = OP.DF_PRS_ID