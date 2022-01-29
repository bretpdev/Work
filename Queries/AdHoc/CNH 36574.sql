--Please provide a data dubmp of RSXX, RSXX, RSXX and LNXX for the borrowers listed below. A DCR Ticket will be completed to deactive the repayment schedules. 

--Madison G Johnson - XXXXXXXXXX
--Cyrus A Nazarian - XXXXXXXXXX
--Sameera G Mabrey - XXXXXXXXXX


--RSXX
SELECT
	PDXX.DF_SPE_ACC_ID,
	RSXX.*
FROM
	CDW..PDXX_PRS_NME PDXX
	LEFT JOIN CDW..RSXX_IBR_RPS RSXX
		ON PDXX.DF_PRS_ID = RSXX.BF_SSN
WHERE
	PDXX.DF_SPE_ACC_ID IN ('XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX')

ORDER BY
	PDXX.DF_SPE_ACC_ID,
	RSXX.BD_CRT_RSXX,
	RSXX.BN_IBR_SEQ


--RSXX
SELECT
	PDXX.DF_SPE_ACC_ID,
	RSXX.*
FROM
	CDW..PDXX_PRS_NME PDXX
	LEFT JOIN CDW..RSXX_BR_RPD RSXX
		ON PDXX.DF_PRS_ID = RSXX.BF_SSN
WHERE
	PDXX.DF_SPE_ACC_ID IN ('XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX')

ORDER BY
	PDXX.DF_SPE_ACC_ID,
	RSXX.LN_RPS_SEQ

--RSXX
--SELECT *
--FROM OPENQUERY
--	(LEGEND,
--		'
--			SELECT
--				PDXX.DF_SPE_ACC_ID,
--				RSXX.*
--			FROM
--				PKUB.PDXX_PRS_NME PDXX
--				LEFT JOIN PKUB.RSXX_IBR_IRL_LON RSXX
--					ON PDXX.DF_PRS_ID = RSXX.BF_SSN
--			WHERE
--				PDXX.DF_SPE_ACC_ID IN (''XXXXXXXXXX'',''XXXXXXXXXX'',''XXXXXXXXXX'')
--			ORDER BY
--				PDXX.DF_SPE_ACC_ID,
--				RSXX.BD_CRT_RSXX,
--				RSXX.BN_IBR_SEQ,
--				RSXX.LN_SEQ
--		'
--	)


--LNXX
SELECT
	PDXX.DF_SPE_ACC_ID,
	LNXX.*
FROM
	CDW..PDXX_PRS_NME PDXX
	LEFT JOIN CDW..[LNXX_LON_RPS] LNXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
WHERE
	PDXX.DF_SPE_ACC_ID IN ('XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX')

ORDER BY
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ,
	LNXX.LN_RPS_SEQ