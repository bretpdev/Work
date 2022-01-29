USE CDW
GO



SELECT 
	PDXX.DF_SPE_ACC_ID,
	AYXX.LD_ATY_REQ_RCV,
	AYXX.PF_REQ_ACT,
	AYXX.LX_ATY
FROM
	AYXX_ATY_TXT AYXX
	INNER JOIN AYXX_BR_LON_ATY AYXX
		ON AYXX.BF_SSN = AYXX.BF_SSN
		AND AYXX.LN_ATY_SEQ = AYXX.LN_ATY_SEQ
	INNER JOIN PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = AYXX.BF_SSN
WHERE		
	AYXX.LC_STA_ACTYXX = 'A'
	AND AYXX.LD_ATY_REQ_RCV BETWEEN 'XX/XX/XXXX' AND  'XX/XX/XXXX'
	AND AYXX.LX_ATY LIKE '%HAZARD%'

