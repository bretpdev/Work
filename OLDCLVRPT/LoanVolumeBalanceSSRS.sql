USE [CentralData]
GO
/****** Object:  StoredProcedure [oldclvrpt].[LoanVolumeBalanceSSRS]    Script Date: 9/23/2021 9:38:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [oldclvrpt].[LoanVolumeBalanceSSRS]
AS
SELECT DISTINCT
	PD01.DF_SPE_ACC_ID AS [Account Number],
	MAX(GA10.Loans) AS [Amount of Loans],
	MAX(GA10.Balance) AS [Outstanding Principal],
	SUM(ISNULL(DC11_PRI.LA_PRI_AT_PST,0.00) + ISNULL(DC11_INT.LA_APL_INT,0.00) + ISNULL(CAST(DC11_PRI.LA_APL_COL_CST AS DECIMAL(18,2)),0.00)) AS [Principal Interest]
FROM
	ODW..PD01_PDM_INF PD01
	INNER JOIN ODW..DC01_LON_CLM_INF DC01
		ON DC01.BF_SSN = PD01.DF_PRS_ID
		AND DC01.LC_AUX_STA = '12'
		AND DC01.LF_CLM_ID != ''
	INNER JOIN 
	(
		SELECT
			DC01.BF_SSN,
			DC01.AF_APL_ID,
			DC01.AF_APL_ID_SFX,
			MAX(DC01.LF_CRT_DTS_DC10) AS LF_CRT_DTS_DC10
		FROM
			ODW..DC01_LON_CLM_INF DC01
		WHERE
			DC01.LC_AUX_STA = '12'
			AND DC01.LF_CLM_ID != ''
		GROUP BY
			DC01.BF_SSN,
			DC01.AF_APL_ID,
			DC01.AF_APL_ID_SFX
	) MaxDC01
		ON MaxDC01.BF_SSN = DC01.BF_SSN
		AND MaxDC01.AF_APL_ID = DC01.AF_APL_ID
		AND MaxDC01.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
		AND MaxDC01.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10
	LEFT JOIN ODW..DC11_LON_FAT DC11_PRI
		ON DC11_PRI.BF_SSN = DC01.BF_SSN
		AND DC11_PRI.AF_APL_ID = DC01.AF_APL_ID
		AND DC11_PRI.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
		AND DC11_PRI.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10
		AND DC11_PRI.LC_TRX_TYP IN ('DC')
		AND DC11_PRI.LD_TRX_EFF >= '03/13/2020'
		AND DC11_PRI.LC_REV_IND_TYP = ''
	    AND (DC11_PRI.LD_TRX_ADJ IS NULL OR DC11_PRI.LD_TRX_ADJ = '')
	LEFT JOIN ODW..DC11_LON_FAT DC11_INT
		ON DC11_INT.BF_SSN = DC01.BF_SSN
		AND DC11_INT.AF_APL_ID = DC01.AF_APL_ID
		AND DC11_INT.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
		AND DC11_INT.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10
		AND DC11_INT.LC_TRX_TYP IN ('AC')
		AND DC11_INT.LD_TRX_EFF >= '03/13/2020'
		AND DC11_INT.LC_REV_IND_TYP = ''
	    AND (DC11_INT.LD_TRX_ADJ IS NULL OR DC11_INT.LD_TRX_ADJ = '')
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
					LC_RMT_TRX_TYP IN (''DC'')
					AND LX_RMT_TRX_EFF_DTE >= ''03/13/2020''
					AND LC_RMT_STA = ''S''
			')
	) RM10
		ON RM10.LF_PRS_ID = PD01.DF_PRS_ID
	LEFT JOIN
	(
		SELECT DISTINCT
			DF_PRS_ID,
			COUNT(GA14.AF_APL_ID_SFX) AS Loans,
			SUM(GA10.AA_CUR_PRI) AS Balance
		FROM
			ODW..PD01_PDM_INF PD01
			INNER JOIN ODW..GA01_APP GA01
				ON GA01.DF_PRS_ID_BR = PD01.DF_PRS_ID
			INNER JOIN ODW..GA10_LON_APP GA10
				ON GA10.AF_APL_ID = GA01.AF_APL_ID
			INNER JOIN ODW..GA14_LON_STA GA14
				ON GA14.AF_APL_ID = GA10.AF_APL_ID
				AND GA14.AF_APL_ID_SFX = GA10.AF_APL_ID_SFX
				AND GA14.AC_LON_STA_TYP = 'DN' --Defaulted loans only
				AND GA14.AC_STA_GA14 = 'A'
			INNER JOIN
			(
				SELECT
					GA14.AF_APL_ID,
					GA14.AF_APL_ID_SFX,
					MAX(GA14.AF_CRT_DTS_GA14) AS AF_CRT_DTS_GA14
				FROM
					ODW..GA14_LON_STA GA14
				WHERE
					GA14.AC_LON_STA_TYP = 'DN'
					AND GA14.AC_STA_GA14 = 'A'
				GROUP BY
					GA14.AF_APL_ID,
					GA14.AF_APL_ID_SFX
			) GA14Max
				ON GA14Max.AF_APL_ID = GA14.AF_APL_ID
				AND GA14Max.AF_APL_ID_SFX = GA14.AF_APL_ID_SFX
				AND GA14Max.AF_CRT_DTS_GA14 = GA14.AF_CRT_DTS_GA14
			INNER JOIN ODW..DC01_LON_CLM_INF DC01
				ON DC01.AF_APL_ID = GA10.AF_APL_ID
				AND DC01.AF_APL_ID_SFX = GA10.AF_APL_ID_SFX
				AND DC01.LC_AUX_STA = '12'
				AND DC01.LF_CLM_ID != ''
		GROUP BY
			DF_PRS_ID
	) GA10
		ON GA10.DF_PRS_ID = PD01.DF_PRS_ID
WHERE
	(
		DC11_PRI.BF_SSN IS NOT NULL
		OR RM10.LF_PRS_ID IS NOT NULL
	)
GROUP BY
	PD01.DF_SPE_ACC_ID
ORDER BY
	PD01.DF_SPE_ACC_ID ASC