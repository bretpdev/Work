
SELECT DISTINCT
	D.*
	--EX.*
FROM
	CDW..[CNH 43351] D
	INNER JOIN CDW..CS_Transfer1 T
		ON T.BF_SSN = D.SSN
	--LEFT JOIN CDW..CS_Transfer_Exclusions EX
	--	ON EX.BF_SSN = D.BF_SSN

