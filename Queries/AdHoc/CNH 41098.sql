--We need to have a query run to identify all borrowers in a current verified bankruptcy status that do not have a forbearance, deferment, inschool, or grace status that covers the bankruptcy period.

--From Riley:  In reviewing, I forgot to include status XX in the �not in� statement . If  XX is added It takes the results down to XX including the borrower you get in the second query.  With the Change acct XXXXXXXXXX is one of the XX borrowers that pull back and needs to be included.  This query spring up from the latest change to the dialer file when I found borrowers that had a verified bankruptcy status but where not eligible for the Bankruptcy forb due to when the loan was disbursed. As a result they were still showing in a repayment status.   Mindy stated accounts with a verified bankruptcy status but not eligible for the bankruptcy forb would still need to be placed into a admin forb.  I believe this is the intent of the ticket 
SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..PDXX_PRS_BKR PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
		AND PDXX.DC_BKR_STA = 'XX'
		AND PDXX.DC_PRS_BKR_RSU_REA = ''
	INNER JOIN CDW..LNXX_LON LNXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
		AND LNXX.LC_STA_LONXX = 'R'
		AND LNXX.LA_CUR_PRI > X.XX
	INNER JOIN CDW..DWXX_DW_CLC_CLU DWXX
		ON LNXX.BF_SSN = DWXX.BF_SSN
		AND LNXX.LN_SEQ = DWXX.LN_SEQ
		AND DWXX.WC_DW_LON_STA NOT IN ('XX', 'XX', 'XX', 'XX', 'XX')
ORDER BY
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ

--original query
SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ
	--for testing
	--,PDXX.DD_BKR_STA
	--,LNXX.LD_END_GRC_PRD
	--,SDXX.LD_SCL_SPR
	--,FORB.LD_FOR_BEG
	--,FORB.LD_FOR_END
	--,DEFR.LD_DFR_BEG
	--,DEFR.LD_DFR_END
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..PDXX_PRS_BKR PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
		AND PDXX.DC_BKR_STA = 'XX' --verified bankruptcy
	INNER JOIN CDW..LNXX_LON LNXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
		AND LNXX.LC_STA_LONXX = 'R'
		AND LNXX.LA_CUR_PRI > X.XX
	INNER JOIN CDW..SDXX_STU_SPR SDXX
		ON LNXX.LF_STU_SSN = SDXX.LF_STU_SSN
		AND SDXX.LC_STA_STUXX = 'A'

--ACTIVE FORBEARANCE
	LEFT JOIN
	(
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			CAST(LNXX.LD_FOR_BEG AS DATE) AS LD_FOR_BEG,
			CAST(LNXX.LD_FOR_END AS DATE) AS LD_FOR_END
		FROM
			CDW..FBXX_BR_FOR_REQ FBXX
		INNER JOIN CDW..LNXX_BR_FOR_APV LNXX
			ON FBXX.BF_SSN = LNXX.BF_SSN
			AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM
		WHERE
			FBXX.LC_FOR_STA = 'A'
			AND FBXX.LC_STA_FORXX = 'A'
			AND LNXX.LC_STA_LONXX = 'A'
	) FORB
		ON LNXX.BF_SSN = FORB.BF_SSN
		AND LNXX.LN_SEQ = FORB.LN_SEQ
		AND PDXX.DD_BKR_STA BETWEEN FORB.LD_FOR_BEG AND FORB.LD_FOR_END
--DFXX/LNXX active, not rejected deferments 
	LEFT JOIN
	(
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LD_DFR_BEG,
			LNXX.LD_DFR_END
		FROM
			CDW..DFXX_BR_DFR_REQ DFXX
			INNER JOIN CDW..LNXX_BR_DFR_APV LNXX
				ON DFXX.BF_SSN = LNXX.BF_SSN
				AND DFXX.LF_DFR_CTL_NUM = LNXX.LF_DFR_CTL_NUM
		WHERE
				DFXX.LC_DFR_STA = 'A'
				AND DFXX.LC_STA_DFRXX = 'A'
				AND LNXX.LC_STA_LONXX = 'A'
				AND LNXX.LC_DFR_RSP != 'XXX' --not denied
	) DEFR
		ON LNXX.BF_SSN = DEFR.BF_SSN
		AND LNXX.LN_SEQ = DEFR.LN_SEQ
		AND PDXX.DD_BKR_STA BETWEEN DEFR.LD_DFR_BEG AND DEFR.LD_DFR_END
WHERE
	ISNULL(LNXX.LD_END_GRC_PRD,'XX/XX/XXXX') < PDXX.DD_BKR_STA --bankruptcy not covered by the grace period
	AND PDXX.DD_BKR_STA > SDXX.LD_SCL_SPR --bankruptcy not covered by in school status
	AND DEFR.BF_SSN IS NULL --bankruptcy not covered by a deferment
	AND FORB.BF_SSN IS NULL --bankruptcy not covered by a forbearance
ORDER BY
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ
