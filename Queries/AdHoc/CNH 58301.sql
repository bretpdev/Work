SELECT * FROM OPENQUERY(DUSTER,
'
	SELECT
		RSXX.*
	FROM 
		OLWHRMX.RSXX_IBR_RPS RSXX
		INNER JOIN OLWHRMX.PDXX_PRS_NME PDXX
			ON PDXX.DF_PRS_ID = RSXX.BF_SSN
	WHERE
		PDXX.DF_SPE_ACC_ID = ''XXXXXXXXXX''
')

SELECT * FROM OPENQUERY(DUSTER,
'
	SELECT
		RSXX.*
	FROM 
		OLWHRMX.RSXX_BR_RPD RSXX
		INNER JOIN OLWHRMX.PDXX_PRS_NME PDXX
			ON PDXX.DF_PRS_ID = RSXX.BF_SSN
	WHERE
		PDXX.DF_SPE_ACC_ID = ''XXXXXXXXXX''
')


SELECT * FROM OPENQUERY(DUSTER,
'
	SELECT
		LNXX.*
	FROM 
		OLWHRMX.LNXX_LON_RPS LNXX
		INNER JOIN OLWHRMX.PDXX_PRS_NME PDXX
			ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	WHERE
		PDXX.DF_SPE_ACC_ID = ''XXXXXXXXXX''
')