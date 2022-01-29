SELECT
	*
FROM OPENQUERY(LEGEND,
'
	SELECT
		*
	FROM
		PKUB.RMXX_RMT_SPS_RFD
	WHERE
		LD_RMT_BCH_INI = ''XXXX-XX-XX''
		AND LC_RMT_BCH_SRC_IPT = ''M''
'
)

SELECT
	*
FROM OPENQUERY(LEGEND,
'
	SELECT
		*
	FROM
		PKUB.RMXX_RMT_SPS_RFD
	WHERE
		LF_RMT_SPS_RFD_TRY = ''NCXXXXXXXXXXXXXX''
'
)