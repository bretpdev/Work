SELECT
	*
FROM
	OPENQUERY(LEGEND,'
	
	SELECT
		LNXX.BF_SSN,
		LNXX.LN_SEQ,
		LNXX.LD_TRM_BEG,
		LNXX.LF_DOE_SCL_ORG,
		SDXX.LF_DOE_SCL_ENR_CUR,
		CASE WHEN
			WQXX.BF_SSN IS NULL THEN X ELSE X
		END HASAPPLICATION
	FROM
		PKUB.LNXX_LON LNXX
		INNER JOIN PKUB.SDXX_STU_SPR SDXX ON SDXX.LF_STU_SSN = LNXX.BF_SSN
		LEFT JOIN PKUB.WQXX_TSK_QUE WQXX
			ON LNXX.BF_SSN = WQXX.BF_SSN
			AND WQXX.PF_REQ_ACT = ''DIFCR''
	WHERE
		SDXX.LF_DOE_SCL_ENR_CUR = ''XXXXXXXX''
		AND
		DAYS(LNXX.LD_TRM_BEG) BETWEEN DAYS(''X/X/XXXX'') AND DAYS(''XX/XX/XXXX'')

')