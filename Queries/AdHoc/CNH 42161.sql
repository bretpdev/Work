USE CDW
GO

SELECT
	--PDXX.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	--AYXX.PF_REQ_ACT AS ARC,
	--CONVERT(VARCHAR, AYXX.LD_ATY_REQ_RCV, XXX) AS ARC_DATE
	COUNT(DISTINCT AYXX.BF_SSN) AS ARC_COUNT
FROM
	CDW..AYXX_BR_LON_ATY AYXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = AYXX.BF_SSN
	
WHERE
	PF_REQ_ACT = 'FCVFA'
	AND LC_STA_ACTYXX = 'A'
	AND LD_ATY_REQ_RCV BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'

SELECT
	count(distinct fbXX.bf_ssn) AS FORB_COUNT
FROM
	CDW..FBXX_BR_FOR_REQ FBXX
	INNER JOIN CDW..LNXX_BR_FOR_APV LNXX
		ON LNXX.BF_SSN = FBXX.BF_SSN
		AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
		AND LNXX.LA_CUR_PRI > X
		AND LNXX.LC_STA_LONXX = 'R'
WHERE
	LNXX.LC_STA_LONXX = 'A'
	AND FBXX.LC_STA_FORXX = 'A'
	AND FBXX.LC_FOR_STA = 'A' --denied records cant have this active
	sAND LNXX.LC_FOR_RSP != 'XXX'
	 and FBXX.LC_FOR_TYP = 'XX'
	 and LD_CRT_REQ_FOR BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
	