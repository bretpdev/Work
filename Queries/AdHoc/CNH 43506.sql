USE CDW

GO

SELECT
	fsXX.*
FROM
	CDW..CS_TransferX TX
	INNER JOIN CDW..cs_TransferXLoans TL
		on TL.BF_SSN = TX.BF_SSN
	INNER JOIN 
	(
		SELECT DISTINCT 
			LNXX.BF_SSN,
			LNXX.LN_SEQ
		FROM
			CDW..LNXX_FIN_ATY LNXX
			INNER JOIN CDW..LNXX_LON LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LA_CUR_PRI >= X.XX
				AND LNXX.LC_FED_PGM_YR = 'LNC'
				AND LNXX.LD_PIF_RPT IS NULL
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND COALESCE(LNXX.LC_FAT_REV_REA,'') = ''
			AND LNXX.PC_FAT_TYP = 'XX'
			AND LNXX.PC_FAT_SUB_TYP = 'XX'
			AND LNXX.LD_FAT_EFF > ='XX/XX/XXXX'
		GROUP BY
			LNXX.BF_SSN,
			LNXX.LN_SEQ
	) Trans
		ON Trans.BF_SSN = TX.BF_SSN
		AND Trans.LN_SEQ = TL.LN_SEQ
	INNER JOIN 
	(
		select fsXX.BF_SSN, fsXX.LN_SEQ, dup.award_id from cdw..FSXX_DL_LON fsXX
		inner join 
		(
		select
			bf_ssn,
			FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(X)),X) award_id,
			count(*) as c
		from
			cdw..FSXX_DL_LON fsXX
		group by
			bf_ssn,
			FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(X)),X)
		having count(*) > X
	) dup
		on dup.BF_SSN = fsXX.BF_SSN
		and dup.award_id = FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(X)),X)
	) fsXX
		 on fsXX.BF_SSN = trans.BF_SSN
		 and fsXX.ln_seq = trans.ln_seq
order by fsXX.bf_ssn,
fsXX.ln_seq