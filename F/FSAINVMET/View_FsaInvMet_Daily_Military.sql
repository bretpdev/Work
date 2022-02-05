USE CDW
GO

/****** Object:  View [FsaInvMet].[Daily_Military]    Script Date: 11/5/2019 12:57:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW FsaInvMet.Daily_Military

AS

SELECT
	LN10.BF_SSN,
	LN10.LN_SEQ, 
	1 AS ActiveMilitaryIndicator
FROM
	CDW..LN10_LON LN10
	INNER JOIN CDW..DW01_DW_CLC_CLU DW01
		ON DW01.BF_SSN = LN10.BF_SSN
		AND DW01.LN_SEQ = LN10.LN_SEQ
	INNER JOIN 
	(
		SELECT
			LN72.BF_SSN,
			LN72.LN_SEQ,
			LN72.LR_ITR,
			LN72.LC_INT_RDC_PGM,
			LN72.LC_INT_RDC_PGM_TYP,
			ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ ORDER BY LN72.LD_STA_LON72 DESC) AS CurrentRow
		FROM
			CDW..LN72_INT_RTE_HST LN72
		WHERE
			LN72.LC_STA_LON72 = 'A'
			AND CAST(GETDATE() AS DATE) BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END 
	) LN72
		ON LN72.BF_SSN = LN10.BF_SSN
		AND LN72.LN_SEQ = LN10.LN_SEQ
		AND LN72.CurrentRow = 1
	LEFT JOIN
	(
		SELECT DISTINCT
			DW01.BF_SSN,
			DW01.LN_SEQ
		FROM
			CDW..DW01_DW_CLC_CLU DW01
			INNER JOIN CDW..LN50_BR_DFR_APV LN50
				ON DW01.BF_SSN = LN50.BF_SSN
				AND DW01.LN_SEQ = LN50.LN_SEQ
				AND LN50.LC_STA_LON50 = 'A'
				AND CAST(GETDATE() AS DATE) BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END 
			INNER JOIN CDW..DF10_BR_DFR_REQ DF10
				ON DW01.BF_SSN = DF10.BF_SSN
				AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
				AND DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP IN('38','40')
				AND LN50.LC_DFR_RSP != '003'
		WHERE 
			DW01.WC_DW_LON_STA = '04'
	) DefermentDate 
		ON DefermentDate.BF_SSN = LN10.BF_SSN
		AND DefermentDate.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT DISTINCT
			ADR.BorrSSN,
			MIN(ADR.TXCXBegin) AS BeginDate,
			MAX(ADR.TXCXEnd) AS EndDate
		FROM 
			CLS.scra.ActiveDutyReporting ADR
			INNER JOIN
			(
				SELECT
					BorrSSN,
					MAX(CreatedAt) AS CreatedAt
				FROM
					CLS.scra.ActiveDutyReporting
				WHERE
					DeletedAt IS NULL
				GROUP BY
					BorrSSN
			) MaxADR
				ON MaxADR.CreatedAt = ADR.CreatedAt
				AND MaxADR.BorrSSN = ADR.BorrSSN
		WHERE
			CAST(GETDATE() AS DATE) BETWEEN ADR.TXCXBegin AND ADR.TXCXEnd
			AND ADR.DeletedAt IS NULL
		GROUP BY
			ADR.BorrSSN
	) MilitaryDatabase
		ON MilitaryDatabase.BorrSSN = LN10.BF_SSN
		AND CAST(LN10.LD_LON_1_DSB AS DATE) <= MilitaryDatabase.BeginDate
WHERE
	LN10.LC_STA_LON10 = 'R'
	AND
	(
		(
			LN72.LR_ITR = 0 
			AND 
			(
				(LN72.LC_INT_RDC_PGM = 'S' AND ISNULL(LN72.LC_INT_RDC_PGM_TYP,'') = '') --Distinction S blank is military, SV is being used as emergency interest
				OR (LN72.LC_INT_RDC_PGM = 'M' AND LN72.LC_INT_RDC_PGM_TYP = 'H')
			)
		) --danger zone 
		OR (LN72.LR_ITR <= 6 AND LN72.LC_INT_RDC_PGM = 'M') --regular scra
		OR DefermentDate.BF_SSN IS NOT NULL --has military deferment
		OR MilitaryDatabase.BorrSSN IS NOT NULL --Active in new database
	)

GO