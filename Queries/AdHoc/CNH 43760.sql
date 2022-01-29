SELECT distinct
	TX.BF_SSN AS [CLAIMANT SSN],
	RTRIM(PDXX.DM_PRS_X) + ' ' + RTRIM(PDXX.DM_PRS_LST) AS [CLAIMANT NAME],
	'BORROWER' AS [BORROWER OR ENDORSER?],
	LNXX.LN_SEQ AS [LOAN SEQ#],
	'' AS [DATE NOTE SIGNED],
	LNXX.LD_LON_X_DSB AS [DISBURSE� DATE],
	lnXX.BALANCE as [DISBURSE AMOUNT],
	ABS(Trans.la_fat_cur_pri) as PRINCIPAL,
	ABS(Trans.LA_FAT_NSI) as INTEREST,
	ayXX.ld_aty_req_rcv

FROM
	CDW..CS_TRANSFERX TX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = TX.BF_SSN
	INNER JOIN CDW..AYXX_BR_LON_ATY AYXX
		ON AYXX.BF_SSN = TX.BF_SSN
	INNER JOIN 
	(
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			lnXX.LA_FAT_CUR_PRI,
			lnXX.LA_FAT_NSI
		FROM
			CDW..LNXX_FIN_ATY LNXX	
			INNER JOIN CDW..CS_TRANSFERXLoans TL
                ON TL.BF_SSN = LNXX.BF_SSN
                AND TL.LN_SEQ = LNXX.LN_SEQ
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND COALESCE(LNXX.LC_FAT_REV_REA,'') = ''
			AND LNXX.LD_FAT_EFF >= 'XXXX-XX-XX'
			AND LNXX.PC_FAT_TYP = 'XX'
			AND LNXX.PC_FAT_SUB_TYP = 'XX'
	) Trans
		on trans.bf_ssn = TX.bf_ssn
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = TX.BF_SSN
		AND LNXX.LN_SEQ = TRANS.LN_SEQ

	LEFT JOIN
	(
		SELECT 
			lnXX.BF_SSN,
			lnXX.LN_SEQ,
			SUM((LA_DSB - isnull(LA_DSB_CAN,X))) AS BALANCE
		FROM
			CDW..LNXX_DSB lnXX
			INNER JOIN CDW..CS_TRANSFERXLoans TL
                ON TL.BF_SSN = lnXX.BF_SSN
                AND TL.LN_SEQ = lnXX.LN_SEQ
		WHERE
			LC_STA_LONXX = 'X'
		GROUP BY
			lnXX.BF_SSN,
			lnXX.LN_SEQ
	) LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ

WHERE
	AYXX.PF_REQ_ACT IN ( 'IDTFN', 'DIFRD')
	AND AYXX.LC_STA_ACTYXX = 'A'

