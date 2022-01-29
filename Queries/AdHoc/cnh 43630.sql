USE CDW
GO

SELECT 
	CNH.*
FROM
	CDW..[CNH 43630] CNH
	INNER JOIN CDW..CS_Transfer2 T1
		ON T1.BF_SSN = CNH.SSN
	INNER JOIN 
	(
		SELECT DISTINCT
			LN90.BF_SSN
		FROM
			CDW..LN90_FIN_ATY LN90
			inner join CDW..[CNH 43630] CNH
				on cnh.ssn = ln90.bf_ssn	
			INNER JOIN CDW..CS_Transfer2Loans TL
                ON TL.BF_SSN = LN90.BF_SSN
                AND TL.LN_SEQ = LN90.LN_SEQ
		WHERE
			LN90.LC_STA_LON90 = 'A'
			AND COALESCE(LN90.LC_FAT_REV_REA,'') = ''
			AND LN90.LD_FAT_EFF >= '2020-11-11'
			AND LN90.PC_FAT_TYP = '04'
			AND LN90.PC_FAT_SUB_TYP = '96'
	) Trans
		ON Trans.BF_SSN = T1.BF_SSN