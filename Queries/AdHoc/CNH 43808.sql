declare @data table(bf_ssn char(X))
insert into @data values
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX'),
('XXXXXXXXX')


SELECT DISTINCT
	D.BF_SSN,
	PDXX.DM_PRS_X + ' '  + PDXX.DM_PRS_LST AS [NAME],
	LNXX.LF_FED_CLC_RSK AS [CRC CODE],
	FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(XX)),X) AS [AWARD ID],
	LNXX.LD_FAT_EFF AS [TRANSFER DATE],
	LNXX.LA_FAT_CUR_PRI AS [PRINCIPAL TRANSFERED],
	LNXX.LA_FAT_NSI AS [INTEREST TRANSFERED]
FROM 
	@DATA d 
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = D.BF_SSN
	INNER JOIN 
	(
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LD_FAT_EFF,
			SUM(COALESCE(LA_FAT_CUR_PRI,X.XX)) AS LA_FAT_CUR_PRI,
			SUM(COALESCE(LA_FAT_NSI,X.XX)) AS LA_FAT_NSI
		FROM
			CDW..LNXX_FIN_ATY LNXX
			INNER JOIN @DATA D
				ON D.BF_SSN = LNXX.BF_SSN
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND COALESCE(rtrim(LNXX.LC_FAT_REV_REA),'') = ''
			AND LNXX.PC_FAT_TYP = 'XX'
			AND LNXX.PC_FAT_SUB_TYP = 'XX'
			AND LNXX.LD_FAT_EFF >= 'XX/XX/XXXX'
		GROUP BY
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LD_FAT_EFF
	) LNXX
		ON LNXX.BF_SSN = D.BF_SSN
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN CDW..FSXX_DL_LON FSXX
		ON FSXX.BF_SSN = LNXX.BF_SSN
		AND FSXX.LN_SEQ = LNXX.LN_SEQ