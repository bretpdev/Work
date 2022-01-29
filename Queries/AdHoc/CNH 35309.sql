USE CDW

GO

SELECT DISTINCT
 --SSN = LNXX.BF_SSN
	LNXX.BF_SSN AS SSN
 --Loan Seq = LNXX.LN_SEQ
	,LNXX.LN_SEQ AS [Loan Seq]
 --Alleged Date = if PDXX.DC_DTH_STA = XX then used DD_DTH_STA; if PDXX.DC_DTH_STA = XX or XX use PDXX.DD_DTH_NTF
	,CASE
		WHEN PDXX.DC_DTH_STA = 'XX' tHEN CAST(PDXX.DD_DTH_STA AS DATE)
		WHEN PDXX.DC_DTH_STA IN ('XX', 'XX') THEN CAST(PDXX.DD_DTH_NTF AS DATE)
		ELSE NULL
	END AS [Alleged Date]
	,FORB.MONS_FORB AS [Months in Forb]
 --ARC PXXXA Dates = AYXX.LD_ATY_REQ_RCV where AYXX.PF_REQ_ACT = PXXXA
	,CAST(PXXXA.LD_ATY_REQ_RCV AS DATE) AS [ARC PXXXA Dates]
 --ARC ADDTH Dates = AYXX.LD_ATY_REQ_RCV where AYXX.PF_REQ_ACT = ADDTH
	,CAST(ADDTH.LD_ATY_REQ_RCV AS DATE) AS [ARC ADDTH Dates]
 --Date Alleged Status Removed = If DC_DTH_STA = XX or XX then DD_DTH_STA, otherwise leave blank
	,CASE
		WHEN PDXX.DC_DTH_STA IN ('XX', 'XX') THEN CAST(PDXX.DD_DTH_STA AS DATE)
		ELSE NULL
	END  AS [Date Alleged Status Removed]
 --Borrower Default = If LNXX.LD_STA_LONXX = D and LNXX.LC_SST_LONXX = XX or XX then Y; otherwise N
	,CASE
		WHEN LNXX.LC_STA_LONXX = 'D' AND LNXX.LC_SST_LONXX IN ('X', 'X') THEN 'Y'
		ELSE 'N'
	END AS [Borrower Default]
	,LNXX.LA_CUR_PRI AS [Principal Balance]
FROM
	LNXX_LON LNXX
	INNER JOIN PDXX_GTR_DTH PDXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	LEFT JOIN LNXX_LON_ATY LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
	LEFT JOIN AYXX_BR_LON_ATY PXXXA
		ON LNXX.BF_SSN = PXXXA.BF_SSN
		AND LNXX.LN_ATY_SEQ = PXXXA.LN_ATY_SEQ
		AND PXXXA.PF_REQ_ACT = 'PXXXA'
LEFT JOIN AYXX_BR_LON_ATY ADDTH
		ON LNXX.BF_SSN = ADDTH.BF_SSN
		AND LNXX.LN_ATY_SEQ = ADDTH.LN_ATY_SEQ
		AND ADDTH.PF_REQ_ACT = 'ADDTH'
--claculate loan level months in forbearance
	LEFT JOIN (
			SELECT DISTINCT
				LNXX.BF_SSN
				,LNXX.LN_SEQ
				,SUM(
			 ---# months in forb type XX or XX after notification = Where FBXX.LC_FOR_TYP = XX or XX AND the LNXX.LD_FOR_BEG >= PDXX.DD_DTH_NTF calculate the number of dates between LNXX.LD_FOR_BEG and LD_FOR_END and divide by XX
				CASE
					WHEN CAST(LNXX.LD_FOR_BEG AS DATE) >= CAST(PDXX.DD_DTH_NTF AS DATE) THEN cast((DATEDIFF(day, LNXX.LD_FOR_BEG, LNXX.LD_FOR_END)) AS decimal)/XX
					ELSE X.XX
				END
	
				) AS MONS_FORB
	
			FROM
				LNXX_LON LNXX
				INNER JOIN PDXX_GTR_DTH PDXX
					ON LNXX.BF_SSN = PDXX.DF_PRS_ID
				INNER JOIN  FBXX_BR_FOR_REQ FBXX
					ON LNXX.BF_SSN = FBXX.BF_SSN
					AND FBXX.LC_FOR_TYP IN ('XX', 'XX')
				INNER JOIN LNXX_BR_FOR_APV LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
			GROUP BY
				LNXX.BF_SSN
				,LNXX.LN_SEQ
		) FORB
			ON LNXX.BF_SSN = FORB.BF_SSN
			AND LNXX.LN_SEQ = FORB.LN_SEQ

ORDER BY
	LNXX.BF_SSN
	,LNXX.LN_SEQ

