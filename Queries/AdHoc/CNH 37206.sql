--Accounts with active open loans:
--LNXX.LC-STA-LONXX = R & LNXX.LA-CUR-PRI > X

--QUERY X Requirements:  X. Accounts that are currently on an IDR plan. (all idr plan types - LNXX. LC_TYP_SCH_DIS = CA, CP, CQ, CX, CX, CX, IB, IL, IP, IS, IX, IX, IA (Eliezer said to add IA))
--QUERY X Requirements:  X. Currently receiving the interest subsidy. Finacial Transaction Type: XXXXA. (clarified A is a transaction automatically added by the system, can be disregarded)
--COMBINE QUERY X AND X INTO ONE QUERY (per Eliezer)
--QUERY X Requirements:  X. No longer receiving the interest subsidy (Josh recommended using FSXX and Eli agreed)
--QUERY X Requirements: X. Accounts that have never been on an idr plan. (no schedules exist with any of the types from query X)
--QUERY X Requirements: active IDR schedule and borrower has both DLCON and NCDRV ARCs

--COMBINED QUERY X AND QUERY X - currently on IDR and receiving the interest subsidy
SELECT DISTINCT
	LNXX.BF_SSN,
	LNXX.LC_TYP_SCH_DIS AS PLAN_TYPE
FROM
	CDW..LNXX_LON LNXX
	INNER JOIN CDW..LNXX_LON_RPS LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN CDW..LNXX_FIN_ATY LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
WHERE
	LNXX.LC_STA_LONXX = 'R'
	AND LNXX.LA_CUR_PRI > X
	AND LNXX.LC_TYP_SCH_DIS IN ('CA', 'CP', 'CQ', 'CX', 'CX', 'CX', 'IB', 'IL', 'IP', 'IS', 'IX', 'IX', 'IA')
	AND LNXX.LC_STA_LONXX = 'A'
	AND LNXX.LC_TLX_IBR_ELG IN ('B', 'C', 'P','R', 'X')
	AND LNXX.PC_FAT_TYP = 'XX'
	AND LNXX.PC_FAT_SUB_TYP = 'XX'
	


--QUERY X - No longer receiving the interest subsidy
SELECT DISTINCT
	LNXX.BF_SSN,
	LNXX.LC_TYP_SCH_DIS AS PLAN_TYPE
FROM
	CDW..LNXX_LON LNXX
	INNER JOIN CDW..LNXX_LON_RPS LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN
		(
			SELECT
				BF_SSN,
				LN_SEQ,
				MAX(LD_STA_FSXX) AS LD_STA_FSXX
			FROM
				CDW..FSXX_SUB_LOS_RNS
			GROUP BY
				BF_SSN,
				LN_SEQ
		) MAX_FSXX -- most recent subsidy record for the loan
			ON LNXX.BF_SSN = MAX_FSXX.BF_SSN
			AND LNXX.LN_SEQ = MAX_FSXX.LN_SEQ
	INNER JOIN CDW..FSXX_SUB_LOS_RNS FSXX -- join to most recent subsidy record to get subsidy status
		ON MAX_FSXX.BF_SSN = FSXX.BF_SSN
		AND MAX_FSXX.LN_SEQ = FSXX.LN_SEQ
		AND MAX_FSXX.LD_STA_FSXX = FSXX.LD_STA_FSXX
WHERE
	LNXX.LC_STA_LONXX = 'R'
	AND LNXX.LA_CUR_PRI > X
	AND LNXX.LC_TYP_SCH_DIS IN ('CA', 'CP', 'CQ', 'CX', 'CX', 'CX', 'IB', 'IL', 'IP', 'IS', 'IX', 'IX', 'IA')
	AND LNXX.LC_STA_LONXX = 'A'
	AND LNXX.LC_TLX_IBR_ELG IN ('B', 'C', 'P','R', 'X')
	AND FSXX.LC_INC_SUB_STA = 'L' -- status of most recent subsity record is "lost"

--QUERY X - accounts that have never been on an idr plan
SELECT DISTINCT
	LNXX.BF_SSN
FROM
	CDW..LNXX_LON LNXX
	LEFT JOIN CDW..LNXX_LON_RPS LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
		AND LNXX.LC_TYP_SCH_DIS IN ('CA', 'CP', 'CQ', 'CX', 'CX', 'CX', 'IB', 'IL', 'IP', 'IS', 'IX', 'IX', 'IA')
WHERE
	LNXX.LC_STA_LONXX = 'R'
	AND LNXX.LA_CUR_PRI > X
	AND LNXX.LC_TLX_IBR_ELG IN ('B', 'C', 'P','R', 'X')
	AND LNXX.BF_SSN IS NULL -- no IDR schedules

--QUERY X - active IDR schedule and borrower has both DLCON and NCDRV ARCs
SELECT DISTINCT
	LNXX.BF_SSN,
	LNXX.LC_TYP_SCH_DIS AS PLAN_TYPE
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..LNXX_LON LNXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN CDW..LNXX_LON_RPS LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN CDW..AYXX_BR_LON_ATY DLCON
		ON LNXX.BF_SSN = DLCON.BF_SSN
			AND DLCON.PF_REQ_ACT = 'DLCON'
	INNER JOIN CDW..AYXX_BR_LON_ATY NCDRV
			ON LNXX.BF_SSN = DLCON.BF_SSN
			AND NCDRV.PF_REQ_ACT = 'NCDRV'
WHERE
	LNXX.LC_TYP_SCH_DIS IN ('CA', 'CP', 'CQ', 'CX', 'CX', 'CX', 'IB', 'IL', 'IP', 'IS', 'IX', 'IX', 'IA')
	AND LNXX.LC_STA_LONXX = 'A'



--QUERY X - LNXX dump for example account
SELECT
	LNXX.IC_LON_PGM,
	LNXX.LC_STA_LONXX,
	LNXX.LA_CUR_PRI,
	LNXX.*,
	DLCON.PF_REQ_ACT AS DLCON,
	NCDRV.PF_REQ_ACT AS NCDRV
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..LNXX_LON LNXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN CDW..LNXX_LON_RPS LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN CDW..AYXX_BR_LON_ATY DLCON
		ON LNXX.BF_SSN = DLCON.BF_SSN
			AND DLCON.PF_REQ_ACT = 'DLCON'
	INNER JOIN CDW..AYXX_BR_LON_ATY NCDRV
			ON LNXX.BF_SSN = DLCON.BF_SSN
			AND NCDRV.PF_REQ_ACT = 'NCDRV'
WHERE
	PDXX.DF_SPE_ACC_ID = 'XXXXXXXXXX'

	

