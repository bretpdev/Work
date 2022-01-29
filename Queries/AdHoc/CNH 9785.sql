SELECT * FROM OPENQUERY (LEGEND, '
	SELECT DISTINCT
		 PDXX.DF_PRS_ID
		--,PDXX.DC_DOM_ST
		--,PDXX.DI_VLD_ADR
	FROM
		PKUB.PDXX_PRS_ADR PDXX
		INNER JOIN PKUB.LNXX_LON LNXX
			ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	WHERE
		PDXX.DC_DOM_ST = ''CA''
		AND PDXX.DI_VLD_ADR = ''Y''
		AND LNXX.LC_STA_LONXX = ''R''
		AND LNXX.LA_CUR_PRI > ''X''


');

SELECT * FROM OPENQUERY (LEGEND, '
	SELECT
		COUNT(DISTINCT PDXX.DF_PRS_ID) AS CA_Unique_Borrowers
	FROM
		PKUB.PDXX_PRS_ADR PDXX
		INNER JOIN PKUB.LNXX_LON LNXX
			ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	WHERE
		PDXX.DC_DOM_ST = ''CA''
		AND PDXX.DI_VLD_ADR = ''Y''
		AND LNXX.LC_STA_LONXX = ''R''
		AND LNXX.LA_CUR_PRI > ''X''
');