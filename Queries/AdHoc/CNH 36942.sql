select * from openquery(legend,
'
SELECT
	*
FROM
	PKUB.RMXX_RMT_SPS_RFD
WHERE
	LC_RMT_BCH_SRC_IPT = ''M''
	AND LC_STA_REMTXX = ''Z''
	AND LF_ZIP_CDE_SRF = ''XXXXXXXXX''
')