USE CDW
GO

SELECT 
	CNH.*
FROM
	CDW..[CNHXXXXX] CNH
	INNER JOIN CDW..CS_TransferX TX
		ON TX.BF_SSN = CNH.SSN
	INNER JOIN 
	(
		SELECT DISTINCT
			LNXX.BF_SSN
		FROM
			CDW..LNXX_FIN_ATY LNXX
			inner join CDW..[CNHXXXXX] CNH
				on cnh.ssn = lnXX.bf_ssn	
			INNER JOIN CDW..CS_TransferXLoans TL
                ON TL.BF_SSN = LNXX.BF_SSN
                AND TL.LN_SEQ = LNXX.LN_SEQ
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND COALESCE(LNXX.LC_FAT_REV_REA,'') = ''
			AND LNXX.LD_FAT_EFF >= 'XX/XX/XXXX'
			AND LNXX.PC_FAT_TYP = 'XX'
			AND LNXX.PC_FAT_SUB_TYP = 'XX'
	) Trans
		ON Trans.BF_SSN = TX.BF_SSN