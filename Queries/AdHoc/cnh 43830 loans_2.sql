DECLARE @DATA TABLE(BF_SSN CHAR(9))
INSERT INTO @DATA VALUES

('180629438'),
('180629438'),
('487159344'),
('271048582'),
('288064056'),
('416375253'),
('416375253')

INSERT INTO CDW..CS_TransferPIFLoans(BF_SSN, LN_SEQ, LoanProgram, LoanSaleId)
SELECT 
	LN10.BF_SSN,
	LN10.LN_SEQ,
	CASE WHEN LN10.LC_FED_PGM_YR = 'LNC' THEN 'LNC'
         WHEN LN10.LC_FED_PGM_YR = 'DLO' THEN 'DLO'
	ELSE '' END AS LoanProgram,
	NULL AS LoanSaleId
FROM
	CDW..CS_TRANSFERPIF PIF
	inner join @DATA D
		ON D.BF_SSN = PIF.BF_SSN
	INNER JOIN CDW..LN10_LON LN10
		ON LN10.BF_SSN = PIF.BF_SSN
	LEFT JOIN
	(
		SELECT DISTINCT
				LN90.BF_SSN,
				LN90.LN_SEQ,
				LN90.LD_FAT_EFF
			FROM
				CDW..LN90_FIN_ATY LN90
			WHERE
				LN90.LC_STA_LON90 = 'A'
				AND COALESCE(rtrim(LN90.LC_FAT_REV_REA),'') = ''
				AND LN90.PC_FAT_TYP = '04'
				AND LN90.PC_FAT_SUB_TYP = '96'
				AND LN90.LD_FAT_EFF >= '11/09/2020'
	) LN90
		ON LN90.BF_SSN = LN10.BF_SSN
		AND LN90.LN_SEQ = LN10.LN_SEQ
where ln90.bf_ssn is null
order by pif.bf_ssn, ln10.ln_seq