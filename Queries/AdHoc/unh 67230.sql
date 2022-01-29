SELECT *   FROM OPENQUERY(DUSTER,
'
SELECT DISTINCT
	RTRIM(PD10.DM_PRS_1) || '' '' || RTRIM(PD10.DM_PRS_LST) AS NAME,
	BF_SSN AS SSN,
	LD_ATY_REQ_RCV AS BEGIN_DATE
FROM
	OLWHRM1.AY10_BR_LON_ATY AY10
	INNER JOIN OLWHRM1.PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = AY10.BF_SSN
	
WHERE PF_REQ_ACT = ''TLPLG ''
and LC_STA_ACTY10 = ''A''
ORDER BY LD_ATY_REQ_RCV
')
