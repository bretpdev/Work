SELECT
	CASE MONTH(LD_RPS_X_PAY_DU)
		WHEN X THEN 'JUNE'
		WHEN X THEN 'JULY'
		WHEN X THEN 'AUGUST'
	END AS MONTHS,
	COUNT(DISTINCT LNXX.BF_SSN) AS BORROWER_COUNT
FROM 
	CDW..LNXX_LON LNXX
	INNER JOIN CDW..RSXX_BR_RPD RSXX
		ON RSXX.BF_SSN = LNXX.BF_SSN
WHERE
	LA_CUR_PRI > X.XX
	AND LNXX.LC_STA_LONXX = 'R'
	AND RSXX.LC_STA_RPSTXX = 'A'
	AND RSXX.LD_RPS_X_PAY_DU BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
GROUP BY
	CASE MONTH(LD_RPS_X_PAY_DU)
		WHEN X THEN 'JUNE'
		WHEN X THEN 'JULY'
		WHEN X THEN 'AUGUST'
	END
