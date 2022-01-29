SELECT
	DEF.*
FROM
	CDW..CS_TransferX TX
	INNER JOIN 
	(
		SELECT DISTINCT	
			DFXX.BF_SSN,
			FSXX.LF_FED_AWD,
			FSXX.LN_FED_AWD_SEQ,
			(FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(XX)), X)) AS COMBINED_AWARDID_SEQ,
			'DEFERMENT '  +
			DFXX.LC_DFR_TYP AS [TYPE],
			LNXX.LD_DFR_BEG BEGIN_DATE,
			LNXX.LD_DFR_END END_DATE
		FROM
			CDW..DFXX_BR_DFR_REQ DFXX
			INNER JOIN CDW..LNXX_BR_DFR_APV LNXX
				ON LNXX.BF_SSN = DFXX.BF_SSN
				AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
			INNER JOIN CDW..FSXX_DL_LON FSXX
				ON FSXX.BF_SSN = LNXX.BF_SSN
				AND FSXX.LN_SEQ = LNXX.LN_SEQ
			INNER JOIN 
			(
				SELECT DISTINCT
					LNXX.BF_SSN,
					LNXX.LN_SEQ
				FROM
					CDW..LNXX_FIN_ATY LNXX
					INNER JOIN CDW..CS_TransferX TX
						ON LNXX.BF_SSN = TX.BF_SSN
				WHERE
					LNXX.LC_STA_LONXX = 'A'
					AND COALESCE(LNXX.LC_FAT_REV_REA,'') = ''
					AND LNXX.PC_FAT_TYP = 'XX'
					AND LNXX.PC_FAT_SUB_TYP = 'XX'
					AND LNXX.LD_FAT_EFF > 'XX/XX/XXXX'
					AND TX.DidTransfer = X
			) LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND DFXX.LC_STA_DFRXX = 'A'
			AND DFXX.LC_DFR_STA = 'A' --denied records cant have this active
			AND LNXX.LC_DFR_RSP != 'XXX'
			AND LNXX.LD_DFR_BEG >= 'XX/XX/XXXX'
	) DEF
		ON DEF.BF_SSN = TX.BF_SSN
WHERE
	TX.DidTransfer = X
UNION ALL
SELECT
	FORB.*
FROM
	CDW..CS_TransferX TX
	INNER JOIN 
	(
		SELECT DISTINCT	
			FBXX.BF_SSN,
			FSXX.LF_FED_AWD,
			FSXX.LN_FED_AWD_SEQ,
			(FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(XX)), X)) AS COMBINED_AWARDID_SEQ,
			'FORBEARANCE ' +
			FBXX.LC_FOR_TYP AS [TYPE],
			LNXX.LD_FOR_BEG BEGIN_DATE,
			LNXX.LD_FOR_END END_DATE
		FROM
			CDW..FBXX_BR_FOR_REQ FBXX
			INNER JOIN CDW..LNXX_BR_FOR_APV LNXX
				ON LNXX.BF_SSN = FBXX.BF_SSN
				AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_DFR_CTL_NUM
			INNER JOIN CDW..FSXX_DL_LON FSXX
				ON FSXX.BF_SSN = LNXX.BF_SSN
				AND FSXX.LN_SEQ = LNXX.LN_SEQ
			INNER JOIN 
			(
				SELECT DISTINCT
					LNXX.BF_SSN,
					LNXX.LN_SEQ
				FROM
					CDW..LNXX_FIN_ATY LNXX
					INNER JOIN CDW..CS_TransferX TX
						ON LNXX.BF_SSN = TX.BF_SSN
				WHERE
					LNXX.LC_STA_LONXX = 'A'
					AND COALESCE(LNXX.LC_FAT_REV_REA,'') = ''
					AND LNXX.PC_FAT_TYP = 'XX'
					AND LNXX.PC_FAT_SUB_TYP = 'XX'
					AND LNXX.LD_FAT_EFF > 'XX/XX/XXXX'
					AND TX.DidTransfer = X
			) LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND FBXX.LC_STA_FORXX = 'A'
			AND FBXX.LC_FOR_STA = 'A' --denied records cant have this active
			AND LNXX.LC_FOR_RSP != 'XXX'
			AND LNXX.LD_FOR_BEG >= 'XX/XX/XXXX'
	) FORB
		ON FORB.BF_SSN = TX.BF_SSN
WHERE
	TX.DidTransfer = X
ORDER BY
	BF_SSN,BEGIN_DATE
