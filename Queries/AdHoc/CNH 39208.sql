USE CDW
GO

SELECT DISTINCT
	AYXX.BF_SSN,
	LTRIM(RTRIM(PDXX.DM_PRS_X)) + ' ' + LTRIM(RTRIM(PDXX.DM_PRS_LST)) AS [NAME],
	AYXX.PF_REQ_ACT AS ARC,
	CONVERT(VARCHAR(XX), AYXX.LD_ATY_REQ_RCV, XXX) AS ARC_DATE
FROM
	CDW..AYXX_BR_LON_ATY AYXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = AYXX.BF_SSN
WHERE
	PF_REQ_ACT = 'DISPC'
	AND CAST(LD_ATY_REQ_RCV AS DATE) >= 'XX/XX/XXXX'
	AND AYXX.LC_STA_ACTYXX = 'A'