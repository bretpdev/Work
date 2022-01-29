SELECT
	DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	CONVERT(VARCHAR(XX), LD_ATY_REQ_RCV, XXX) AS DATE_ARC_REQUESTED
FROM
	CDW..AYXX_BR_LON_ATY AYXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = AYXX.BF_SSN
WHERE
	PF_REQ_ACT = 'CTDFB'