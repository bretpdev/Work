USE UDW

SELECT DISTINCT TOP 100
	BF_SSN
	,LN_SEQ
	,CONCAT(LN10.BF_SSN, FORMAT (LN_SEQ, '00')) AS LN_ID
FROM
	LN10_LON LN10