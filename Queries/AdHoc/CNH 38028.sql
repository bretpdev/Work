USE CDW
GO

DECLARE @StartDate DATE = 'XX/XX/XXXX'
DECLARE @EndDate DATE = 'XX/XX/XXXX'

SELECT DISTINCT
	AYXX.BF_SSN,
	AYXX.PF_REQ_ACT,
	CAST(AYXX.LD_ATY_REQ_RCV AS DATE) AS LD_ATY_REQ_RCV
FROM
	CDW..AYXX_BR_LON_ATY AYXX
WHERE 
	AYXX.PF_REQ_ACT IN ('BXPOC','BPOCX','BPOCR')
	AND 
	AYXX.LD_ATY_REQ_RCV BETWEEN @StartDate AND @EndDate
	AND
	AYXX.LC_STA_ACTYXX = 'A'