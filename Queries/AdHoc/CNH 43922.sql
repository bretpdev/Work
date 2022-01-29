USE CDW
GO

SELECT 
	BF_SSN,
	RMXX.LA_BR_RMT_PST,
	RMXX.LD_RMT_PAY_EFF_PST
FROM
	CDW..RMXX_BR_RMT_PST RMXX
WHERE
	RMXX.LC_RMT_STA_PST = 'N'
AND RMXX.LF_RMT_PST_OBD_NUM = 'XXXXXXXX'