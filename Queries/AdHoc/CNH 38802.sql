SELECT
	BASE.*
FROM
	CDW..RSXX_IBR_RPS BASE
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = BASE.BF_SSN
WHERE
	PDXX.DF_SPE_ACC_ID IN ('XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX')
ORDER BY 
	BASE.BF_SSN

SELECT
	BASE.*
FROM
	CDW..RSXX_BR_RPD BASE
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = BASE.BF_SSN
WHERE
	PDXX.DF_SPE_ACC_ID IN ('XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX')
ORDER BY 
	BASE.BF_SSN

SELECT
	BASE.*
FROM
	CDW..RSXX_IBR_IRL_LON BASE
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = BASE.BF_SSN
WHERE
	PDXX.DF_SPE_ACC_ID IN ('XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX')
ORDER BY 
	BASE.BF_SSN

SELECT
	BASE.*
FROM
	CDW..LNXX_LON_RPS BASE
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = BASE.BF_SSN
WHERE
	PDXX.DF_SPE_ACC_ID IN ('XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX')
ORDER BY 
	BASE.BF_SSN