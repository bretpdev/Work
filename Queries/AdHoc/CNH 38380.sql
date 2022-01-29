--payment due dates summary
SELECT
	COUNT(LNXX.BF_SSN) [Total],
	SUM(CASE WHEN DAY(RSXX.LD_RPS_X_PAY_DU) BETWEEN X AND XX THEN X ELSE X END) [DueDateBetweenXandXX],
    SUM(CASE WHEN DAY(RSXX.LD_RPS_X_PAY_DU) BETWEEN XX AND XX THEN X ELSE X END) [DueDateBetweenXXandXX],
	SUM(CASE WHEN DAY(RSXX.LD_RPS_X_PAY_DU) BETWEEN XX AND XX THEN X ELSE X END) [DueDateBetweenXXandXX]
FROM
    CDW..LNXX_LON LNXX
    INNER JOIN CDW..LNXX_LON_RPS LNXX ON LNXX.BF_SSN = LNXX.BF_SSN AND LNXX.LN_SEQ = LNXX.LN_SEQ AND LNXX.LC_STA_LONXX = 'A'
    INNER JOIN CDW..RSXX_BR_RPD RSXX ON LNXX.BF_SSN = RSXX.BF_SSN AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ AND RSXX.LC_STA_RPSTXX = 'A'
    LEFT JOIN
    (
		SELECT
			LNXX.BF_SSN,
			LNXX.LN_SEQ
		FROM
			CDW..DFXX_BR_DFR_REQ DFXX
			JOIN CDW..LNXX_BR_DFR_APV LNXX ON DFXX.BF_SSN = LNXX.BF_SSN	AND DFXX.LF_DFR_CTL_NUM = LNXX.LF_DFR_CTL_NUM
		WHERE
			DFXX.LC_DFR_STA = 'A'
			AND DFXX.LC_STA_DFRXX = 'A'
			AND LNXX.LC_STA_LONXX = 'A'
			AND GETDATE() BETWEEN LNXX.LD_DFR_BEG AND LNXX.LD_DFR_END
    ) DEFR ON LNXX.BF_SSN = DEFR.BF_SSN AND LNXX.LN_SEQ = DEFR.LN_SEQ
    LEFT JOIN
    (
        SELECT
            LNXX.BF_SSN,
            LNXX.LN_SEQ
        FROM
            CDW..FBXX_BR_FOR_REQ FBXX
            JOIN CDW..LNXX_BR_FOR_APV LNXX ON FBXX.BF_SSN = LNXX.BF_SSN AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM
        WHERE
			FBXX.LC_FOR_STA = 'A'
			AND FBXX.LC_STA_FORXX = 'A'
			AND LNXX.LC_STA_LONXX = 'A'
			AND GETDATE() BETWEEN LNXX.LD_FOR_BEG AND LNXX.LD_FOR_END
	) FORB ON LNXX.BF_SSN = FORB.BF_SSN AND LNXX.LN_SEQ = FORB.LN_SEQ
WHERE
    LNXX.LA_CUR_PRI > X
    AND LNXX.LC_STA_LONXX = 'R'
    AND DEFR.BF_SSN IS NULL
    AND FORB.BF_SSN IS NULL

--due date detail
--Tablix for this removed XX/X/XX per Debbie
SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ,
	DATEPART(DAY, RSXX.LD_RPS_X_PAY_DU) [LD_RPS_X_PAY_DU],
	COALESCE(LNXX.LN_DLQ_MAX,X) AS LN_DLQ_MAX,
	CASE LNXX.LC_TYP_SCH_DIS
		WHEN 'CA' THEN 'PAYE PFH'
		WHEN 'CL' THEN 'CONTINGENT LEVEL'
		WHEN 'CP' THEN 'PAYE PERM STD'
		WHEN 'CQ' THEN 'ICR PERM STD'
		WHEN 'CX' THEN 'ICRX'
		WHEN 'CX' THEN 'ICRX'
		WHEN 'CX' THEN 'ICRX'
		WHEN 'EG' THEN 'EXTENDED GRAD'
		WHEN 'EL' THEN 'EXTENDED LEVEL'
		WHEN 'EX' THEN 'EXT SELECT X'
		WHEN 'EX' THEN 'EXT SELECT X'
		WHEN 'FG' THEN 'FIXED TERM'
		WHEN 'FS' THEN 'FIXED AMOUNT'
		WHEN 'G' THEN 'GRADUATED'
		WHEN 'IA' THEN 'REPAY ALT'
		WHEN 'IB' THEN 'IBR PFH'
		WHEN 'IL' THEN 'IBR PERM STD'
		WHEN 'IP' THEN 'IBRXXXX PERM STD'
		WHEN 'IS' THEN 'INCOME SENSITIVE'
		WHEN 'IX' THEN 'IBRXXXX PFH'
		WHEN 'IX' THEN 'REPAYE'
		WHEN 'L' THEN 'LEVEL'
		WHEN 'PG' THEN 'PREHERA GRAD'
		WHEN 'PL' THEN 'PREHERA LEVEL'
		WHEN 'SX' THEN 'SELECT X'
		WHEN 'SX' THEN 'SELECT X'
		ELSE 'UNKNOWN' 
	END AS RepayType,
	COALESCE(FMT.Label,DWXX.WC_DW_LON_STA) AS LoanStatus,
	CASE WHEN DC_STA_SKP IN(X,X) THEN 'YES' ELSE 'NO' END AS SkipStatus
FROM
    CDW..LNXX_LON LNXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN CDW..DWXX_DW_CLC_CLU DWXX ON DWXX.BF_SSN = LNXX.BF_SSN AND DWXX.LN_SEQ = LNXX.LN_SEQ
    INNER JOIN CDW..LNXX_LON_RPS LNXX ON LNXX.BF_SSN = LNXX.BF_SSN AND LNXX.LN_SEQ = LNXX.LN_SEQ AND LNXX.LC_STA_LONXX = 'A'
    INNER JOIN CDW..RSXX_BR_RPD RSXX ON LNXX.BF_SSN = RSXX.BF_SSN AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ AND RSXX.LC_STA_RPSTXX = 'A'
	LEFT JOIN CDW..FormatTranslation FMT ON FMT.FmtName = '$LONSTA' AND FMT.Start = DWXX.WC_DW_LON_STA
	LEFT JOIN
	(
		SELECT
			BF_SSN,
			LN_SEQ,
			MAX(LN_DLQ_MAX) AS LN_DLQ_MAX
		FROM
			CDW..LNXX_LON_DLQ_HST
		WHERE
			LC_STA_LONXX = 'X'
		GROUP BY
			BF_SSN,
			LN_SEQ
	) LNXX ON LNXX.BF_SSN = LNXX.BF_SSN AND LNXX.LN_SEQ = LNXX.LN_SEQ
	LEFT JOIN
	(
		SELECT	
			BF_SSN,
			LN_SEQ,
			DC_STA_SKP
		FROM OPENQUERY
		(LEGEND,
		'
			SELECT DISTINCT
				BF_SSN,
				LN_SEQ,
				DC_STA_SKP,
				MAX(DD_STA_SKP) AS SkipDate
			FROM
				PKUB.PDXX_PRS_SKP_PRC
			GROUP BY
				BF_SSN,
				LN_SEQ,
				DC_STA_SKP
		'
		) OQ
	) PDXX ON PDXX.BF_SSN = LNXX.BF_SSN AND PDXX.LN_SEQ = LNXX.LN_SEQ
    LEFT JOIN
    (
		SELECT
			LNXX.BF_SSN,
			LNXX.LN_SEQ
		FROM
			CDW..DFXX_BR_DFR_REQ DFXX
			JOIN CDW..LNXX_BR_DFR_APV LNXX ON DFXX.BF_SSN = LNXX.BF_SSN	AND DFXX.LF_DFR_CTL_NUM = LNXX.LF_DFR_CTL_NUM
		WHERE
			DFXX.LC_DFR_STA = 'A'
			AND DFXX.LC_STA_DFRXX = 'A'
			AND LNXX.LC_STA_LONXX = 'A'
			AND GETDATE() BETWEEN LNXX.LD_DFR_BEG AND LNXX.LD_DFR_END
    ) DEFR ON LNXX.BF_SSN = DEFR.BF_SSN AND LNXX.LN_SEQ = DEFR.LN_SEQ
    LEFT JOIN
    (
        SELECT
            LNXX.BF_SSN,
            LNXX.LN_SEQ
        FROM
            CDW..FBXX_BR_FOR_REQ FBXX
            JOIN CDW..LNXX_BR_FOR_APV LNXX ON FBXX.BF_SSN = LNXX.BF_SSN AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM
        WHERE
			FBXX.LC_FOR_STA = 'A'
			AND FBXX.LC_STA_FORXX = 'A'
			AND LNXX.LC_STA_LONXX = 'A'
			AND GETDATE() BETWEEN LNXX.LD_FOR_BEG AND LNXX.LD_FOR_END
	) FORB ON LNXX.BF_SSN = FORB.BF_SSN AND LNXX.LN_SEQ = FORB.LN_SEQ
WHERE
    LNXX.LA_CUR_PRI > X
    AND LNXX.LC_STA_LONXX = 'R'
    AND DEFR.BF_SSN IS NULL
    AND FORB.BF_SSN IS NULL
ORDER BY
	DATEPART(DAY, RSXX.LD_RPS_X_PAY_DU),
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ
--dialy breakout
SELECT
	DAY(RSXX.LD_RPS_X_PAY_DU) [Day],
	COUNT(RSXX.LD_RPS_X_PAY_DU) AS [Total]
FROM
    CDW..LNXX_LON LNXX
    INNER JOIN CDW..LNXX_LON_RPS LNXX ON LNXX.BF_SSN = LNXX.BF_SSN AND LNXX.LN_SEQ = LNXX.LN_SEQ AND LNXX.LC_STA_LONXX = 'A'
    INNER JOIN CDW..RSXX_BR_RPD RSXX ON LNXX.BF_SSN = RSXX.BF_SSN AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ AND RSXX.LC_STA_RPSTXX = 'A'
    LEFT JOIN
    (
		SELECT
			LNXX.BF_SSN,
			LNXX.LN_SEQ
		FROM
			CDW..DFXX_BR_DFR_REQ DFXX
			JOIN CDW..LNXX_BR_DFR_APV LNXX ON DFXX.BF_SSN = LNXX.BF_SSN	AND DFXX.LF_DFR_CTL_NUM = LNXX.LF_DFR_CTL_NUM
		WHERE
			DFXX.LC_DFR_STA = 'A'
			AND DFXX.LC_STA_DFRXX = 'A'
			AND LNXX.LC_STA_LONXX = 'A'
			AND GETDATE() BETWEEN LNXX.LD_DFR_BEG AND LNXX.LD_DFR_END
    ) DEFR ON LNXX.BF_SSN = DEFR.BF_SSN AND LNXX.LN_SEQ = DEFR.LN_SEQ
    LEFT JOIN
    (
        SELECT
            LNXX.BF_SSN,
            LNXX.LN_SEQ
        FROM
            CDW..FBXX_BR_FOR_REQ FBXX
            JOIN CDW..LNXX_BR_FOR_APV LNXX ON FBXX.BF_SSN = LNXX.BF_SSN AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM
        WHERE
			FBXX.LC_FOR_STA = 'A'
			AND FBXX.LC_STA_FORXX = 'A'
			AND LNXX.LC_STA_LONXX = 'A'
			AND GETDATE() BETWEEN LNXX.LD_FOR_BEG AND LNXX.LD_FOR_END
	) FORB ON LNXX.BF_SSN = FORB.BF_SSN AND LNXX.LN_SEQ = FORB.LN_SEQ
WHERE
    LNXX.LA_CUR_PRI > X
    AND LNXX.LC_STA_LONXX = 'R'
    AND DEFR.BF_SSN IS NULL
    AND FORB.BF_SSN IS NULL
GROUP BY
	DAY(RSXX.LD_RPS_X_PAY_DU)