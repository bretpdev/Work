SELECT
	*
FROM
	OPENQUERY
	(
		LEGEND,
		'
                SELECT
                        LNXX.BF_SSN,
						FSXX.LF_FED_AWD || FSXX.LN_FED_AWD_SEQ AS AWARD_ID,
                        LNXX.IF_LON_SRV_DFL_LON
                FROM
                        PKUB.LNXX_LON LNXX
                        JOIN PKUB.FSXX_DL_LON FSXX
                                ON LNXX.BF_SSN = FSXX.BF_SSN
                                AND LNXX.LN_SEQ = FSXX.LN_SEQ
                        JOIN PKUB.LNXX_RPD_PIO_CVN LNXX
                                ON LNXX.BF_SSN = LNXX.BF_SSN
                                AND LNXX.LN_SEQ = LNXX.LN_SEQ
                WHERE
                        LNXX.LC_STA_LONXX = ''L''
		'
	)
