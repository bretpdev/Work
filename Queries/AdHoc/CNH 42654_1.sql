
SELECT 
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ,
	LD_END_GRC_PRD,
	SUM(ABS(LNXX.LA_FAT_NSI)) AS INTEREST,
	GRXX.LC_SUB_STA
FROM
	CDW..LNXX_LON LNXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN  CDW..LNXX_FIN_ATY LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN CDW..FSXX_DL_LON FSXX
		ON FSXX.BF_SSN = LNXX.BF_SSN
		AND FSXX.LN_SEQ = LNXX.LN_SEQ
	LEFT JOIN OPENQUERY(LEGEND,
	'
	select DISTINCT
		GRXX.bf_ssn,
		GRXX.lf_fed_awd,
		GRXX.LN_FED_awd_SEQ,
		GRXX.LC_SUB_STA
	from
		pkub.GRXX_NDS_SUB_NTF grXX
		inner join
		(
			select 
				bf_ssn,
				lf_fed_awd,
				LN_FED_awd_SEQ,
				MAX(lf_crt_dts_grXX) as lf_crt_dts_grXX
			from pkub.GRXX_NDS_SUB_NTF
			group by
				bf_ssn,
				lf_fed_awd,
				LN_FED_awd_SEQ
		) m
			on m.bf_ssn = grXX.bf_ssn
			and m.lf_fed_awd = grXX.lf_fed_awd
			and m.LN_FED_awd_SEQ = grXX.LN_FED_awd_SEQ
			and m.lf_crt_dts_grXX = grXX.lf_crt_dts_grXX
	') GRXX
		ON FSXX.BF_SSN = GRXX.BF_SSN
		AND FSXX.lf_fed_awd = GRXX.lf_fed_awd
		AND FSXX.LN_FED_awd_SEQ = GRXX.LN_FED_awd_SEQ
WHERE
	LNXX.IC_LON_PGM IN 
	(
		'DLSSPL',
		'DLSCNS',
		'DLSTFD'
	)
	AND LNXX.LD_LON_X_DSB > 'XX/XX/XXXX'
	AND LNXX.LD_END_GRC_PRD IS NOT NULL
	AND LNXX.LC_STA_LONXX = 'A'
	AND ISNULL(LNXX.LC_FAT_REV_REA,'') = ''
	AND LNXX.PC_FAT_TYP = 'XX'
	AND LNXX.LD_FAT_EFF BETWEEN DATEADD(MONTH, -X, LD_END_GRC_PRD) AND LD_END_GRC_PRD
	AND LNXX.LA_FAT_NSI IS NOT NULL
GROUP BY
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ,
	LD_END_GRC_PRD,
LC_SUB_STA
