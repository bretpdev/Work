-- UNH XXXXX

USE UDW
GO


SELECT DISTINCT
	LNXX.BF_SSN
FROM
	UDW..LNXX_LON LNXX
WHERE
	LNXX.IC_LON_PGM = 'TILP'
	AND
	LNXX.LD_LON_X_DSB BETWEEN 'XXXX-X-X' AND 'XXXX-XX-XX'
