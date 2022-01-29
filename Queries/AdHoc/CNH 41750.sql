USE CDW
GO
--summary
SELECT
	PDXX.DC_DOM_ST AS [STATE],
	COUNT(DISTINCT PDXX.DF_PRS_ID) AS BORROWER_COUNT
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..PDXX_PRS_ADR PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	PDXX.DI_VLD_ADR = 'Y'
	AND PDXX.DC_DOM_ST IN ('WA','CT','CO','NJ','VA','IL', 'NY')
	AND PDXX.DC_ADR = 'L'
	AND LNXX.LA_CUR_PRI > X.XX
	AND LNXX.LC_STA_LONXX = 'R'
GROUP BY
	PDXX.DC_DOM_ST


SELECT
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ,
	PDXX.DC_DOM_ST
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..PDXX_PRS_ADR PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	PDXX.DI_VLD_ADR = 'Y'
	AND PDXX.DC_DOM_ST = 'WA' --,'CT','CO','NJ','VA','IL')
	AND PDXX.DC_ADR = 'L'
	AND LNXX.LA_CUR_PRI > X.XX
	AND LNXX.LC_STA_LONXX = 'R'

SELECT
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ,
	PDXX.DC_DOM_ST
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..PDXX_PRS_ADR PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	PDXX.DI_VLD_ADR = 'Y'
	AND PDXX.DC_DOM_ST = 'CT' --,'CT','CO','NJ','VA','IL')
	AND PDXX.DC_ADR = 'L'
	AND LNXX.LA_CUR_PRI > X.XX
	AND LNXX.LC_STA_LONXX = 'R'

SELECT
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ,
	PDXX.DC_DOM_ST
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..PDXX_PRS_ADR PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	PDXX.DI_VLD_ADR = 'Y'
	AND PDXX.DC_DOM_ST = 'CO' --,'CT','CO','NJ','VA','IL')
	AND PDXX.DC_ADR = 'L'
	AND LNXX.LA_CUR_PRI > X.XX
	AND LNXX.LC_STA_LONXX = 'R'

SELECT
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ,
	PDXX.DC_DOM_ST
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..PDXX_PRS_ADR PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	PDXX.DI_VLD_ADR = 'Y'
	AND PDXX.DC_DOM_ST = 'NJ' --,'CT','CO','NJ','VA','IL')
	AND PDXX.DC_ADR = 'L'
	AND LNXX.LA_CUR_PRI > X.XX
	AND LNXX.LC_STA_LONXX = 'R'

SELECT
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ,
	PDXX.DC_DOM_ST
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..PDXX_PRS_ADR PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	PDXX.DI_VLD_ADR = 'Y'
	AND PDXX.DC_DOM_ST = 'VA' --,'CT','CO','NJ','VA','IL')
	AND PDXX.DC_ADR = 'L'
	AND LNXX.LA_CUR_PRI > X.XX
	AND LNXX.LC_STA_LONXX = 'R'

SELECT
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ,
	PDXX.DC_DOM_ST
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..PDXX_PRS_ADR PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	PDXX.DI_VLD_ADR = 'Y'
	AND PDXX.DC_DOM_ST = 'IL' --,'CT','CO','NJ','VA','IL')
	AND PDXX.DC_ADR = 'L'
	AND LNXX.LA_CUR_PRI > X.XX
	AND LNXX.LC_STA_LONXX = 'R'

SELECT
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ,
	PDXX.DC_DOM_ST
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..PDXX_PRS_ADR PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	PDXX.DI_VLD_ADR = 'Y'
	AND PDXX.DC_DOM_ST = 'NY' --,'CT','CO','NJ','VA','IL')
	AND PDXX.DC_ADR = 'L'
	AND LNXX.LA_CUR_PRI > X.XX
	AND LNXX.LC_STA_LONXX = 'R'