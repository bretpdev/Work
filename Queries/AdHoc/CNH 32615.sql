SELECT 
	BF_SSN,
	PF_REQ_ACT,
	CONVERT(VARCHAR,LD_ATY_REQ_RCV,XXX) AS LD_ATY_REC_RCV
FROM 
	CDW..AYXX_BR_LON_ATY 
WHERE 
	PF_REQ_ACT IN('IBAPV','ICAPV','REAPV','PEAPV','IBDNY','ICDNY','REDNY','PEDNY','IBPND', 'ICPND', 'REPND')
	AND CAST(LD_ATY_REQ_RCV AS DATE) BETWEEN 'X/X/XXXX' AND 'X/XX/XXXX'