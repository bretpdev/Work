
SELECT DISTINCT
	RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [NAME],
	LN10.BF_SSN AS SSN,
	LN10.LD_LON_EFF_ADD AS LOAN_ADD_DATE,
	PD22.DD_DSA AS TPD_BEGIN_DATE,
	LN90.LD_FAT_EFF AS CLAIM_PAID_DATE
FROM
	UDW..LN90_FIN_ATY LN90
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = LN90.BF_SSN
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = LN90.BF_SSN
		AND LN10.LN_SEQ = LN90.LN_SEQ
	INNER JOIN UDW..CL10_CLM_PCL CL10
		ON CL10.BF_SSN = LN90.BF_SSN
		AND LC_REA_CLM_PCL = '02'
		AND LF_CLM_BCH IS NOT NULL
	INNER JOIN UDW..PD22_PRS_DSA PD22
		ON PD22.DF_PRS_ID = LN90.BF_SSN
WHERE
	PC_FAT_TYP = '10'
	AND PC_FAT_SUB_TYP = '30'
	AND LC_STA_LON90 = 'A'
	AND ISNULL(RTRIM(LC_FAT_REV_REA),'') = ''
	AND LD_FAT_EFF BETWEEN '01/01/2021' AND '11/15/2021'
	AND PD22.DD_DSA < LN10.LD_LON_EFF_ADD