USE [CDW]
GO

/****** Object:  View [FsaInvMet].[Monthly_Military_XXXX]    Script Date: XX/XX/XXXX XX:XX:XX AM ******/

SELECT distinct 
	LNXX.BF_SSN,
--	LNXX.LN_SEQ, 
	X AS ActiveMilitaryIndicator
FROM
	CDW..LNXX_LON LNXX
	INNER JOIN CDW..DWXX_DW_CLC_CLU DWXX
		ON DWXX.BF_SSN = LNXX.BF_SSN
		AND DWXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN 
	(
		SELECT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LR_ITR,
			LNXX.LC_INT_RDC_PGM,
			LNXX.LC_INT_RDC_PGM_TYP,
			ROW_NUMBER() OVER (PARTITION BY LNXX.BF_SSN, LNXX.LN_SEQ ORDER BY LNXX.LD_STA_LONXX DESC) AS CurrentRow
		FROM
			CDW..LNXX_INT_RTE_HST LNXX
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) BETWEEN LNXX.LD_ITR_EFF_BEG AND LNXX.LD_ITR_EFF_END /*last day prev month*/
	) LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
		AND LNXX.CurrentRow = X
	LEFT JOIN
	(
		SELECT DISTINCT
			DWXX.BF_SSN,
			DWXX.LN_SEQ
		FROM
			CDW..DWXX_DW_CLC_CLU DWXX
			INNER JOIN CDW..LNXX_BR_DFR_APV LNXX
				ON DWXX.BF_SSN = LNXX.BF_SSN
				AND DWXX.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LC_STA_LONXX = 'A'
				AND CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) BETWEEN LNXX.LD_DFR_BEG AND LNXX.LD_DFR_END /*last day prev month*/
			INNER JOIN CDW..DFXX_BR_DFR_REQ DFXX
				ON DWXX.BF_SSN = DFXX.BF_SSN
				AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
				AND DFXX.LC_DFR_STA = 'A'
				AND DFXX.LC_STA_DFRXX = 'A'
				AND DFXX.LC_DFR_TYP IN('XX','XX')
				AND LNXX.LC_DFR_RSP != 'XXX'
		WHERE 
			DWXX.WC_DW_LON_STA = 'XX'
	) DefermentDate 
		ON DefermentDate.BF_SSN = LNXX.BF_SSN
		AND DefermentDate.LN_SEQ = LNXX.LN_SEQ
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
				GROUP BY
					BorrSsn
			) MaxADR
				ON MaxADR.CreatedAt = ADR.CreatedAt
				AND MaxADR.BorrSSN = ADR.BorrSSN
		WHERE
			CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) BETWEEN ADR.TXCXBegin AND ADR.TXCXEnd
		GROUP BY
			ADR.BorrSSN
	) MilitaryDatabase
		ON MilitaryDatabase.BorrSSN = LNXX.BF_SSN
		AND CAST(LNXX.LD_LON_X_DSB AS DATE) <= MilitaryDatabase.BeginDate
WHERE
	LNXX.LC_STA_LONXX = 'R'
	AND LNXX.LA_CUR_PRI > X
	AND
	(
		(LNXX.LR_ITR = X AND (LNXX.LC_INT_RDC_PGM = 'S' OR (LNXX.LC_INT_RDC_PGM = 'M' AND LNXX.LC_INT_RDC_PGM_TYP = 'H'))) --danger zone borrower
		OR (LNXX.LR_ITR <= X AND LNXX.LC_INT_RDC_PGM = 'M') 
		OR DefermentDate.BF_SSN IS NOT NULL --has military deferment
		OR MilitaryDatabase.BorrSSN IS NOT NULL --Active in new database
	)




