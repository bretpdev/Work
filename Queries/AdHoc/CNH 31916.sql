SELECT
	*
FROM
	OPENQUERY
	(
		LEGEND,
		'
			SELECT
				AYXX.BF_SSN,
				AYXX.LN_ATY_SEQ,
				AYXX.LD_ATY_REQ_RCV,
				AYXX.LF_USR_REQ_ATY,
				AYXX.LC_PRD_CAL,
				AYXX.LF_PRF_BY,
				AYXX.LC_STA_ACTYXX,
				AYXX.LD_STA_ACTYXX,
				AYXX.LI_ATY_MKP_GRC,
				AYXX.LC_ATY_RCP,
				AYXX.LF_LST_DTS_AYXX,
				AYXX.LC_ATY_RGD_TO,
				AYXX.LF_ATY_RGD_TO,
				AYXX.PF_REQ_ACT
			FROM
				PKUB.AYXX_BR_LON_ATY AYXX
			WHERE
				AYXX.PF_REQ_ACT IN (''WELCN'', ''WELCO'')
				AND
				CAST(AYXX.LF_LST_DTS_AYXX AS DATE) IN (''XXXX-X-XX'', ''XXXX-X-XX'')
		'
	)


SELECT
	*
FROM
	OPENQUERY
	(
		LEGEND,
		'
			SELECT
				AYXX.BF_SSN,
				AYXX.LN_ATY_SEQ,
				AYXX.LN_ATY_CMT_SEQ,
				AYXX.LF_LST_DTS_ACXX,
				AYXX.LC_ATY_CMT,
				AYXX.LC_STA_AYXX,
				AYXX.LD_STA_AYXX,
				AYXX.LF_CRT_USR_AYXX,
				AYXX.LF_CRT_DTS_AYXX
			FROM
				PKUB.AYXX_ATY_CMT AYXX
				INNER JOIN PKUB.AYXX_BR_LON_ATY AYXX ON AYXX.BF_SSN = AYXX.BF_SSN AND AYXX.LN_ATY_SEQ = AYXX.LN_ATY_SEQ
			WHERE
				AYXX.PF_REQ_ACT IN (''WELCN'', ''WELCO'')
				AND
				CAST(AYXX.LF_LST_DTS_AYXX AS DATE) IN (''XXXX-X-XX'', ''XXXX-X-XX'')
		'
	)


SELECT
	*
FROM
	OPENQUERY
	(
		LEGEND,
		'
			SELECT
				AYXX.BF_SSN,
				AYXX.LN_ATY_SEQ,
				AYXX.LN_ATY_CMT_SEQ,
				AYXX.LN_ATY_TXT_SEQ,
				AYXX.LX_ATY,
				AYXX.LF_LST_DTS_AYXX
			FROM
				PKUB.AYXX_ATY_TXT AYXX
				INNER JOIN PKUB.AYXX_BR_LON_ATY AYXX ON AYXX.BF_SSN = AYXX.BF_SSN AND AYXX.LN_ATY_SEQ = AYXX.LN_ATY_SEQ
			WHERE
				AYXX.PF_REQ_ACT IN (''WELCN'', ''WELCO'')
				AND
				CAST(AYXX.LF_LST_DTS_AYXX AS DATE) IN (''XXXX-X-XX'', ''XXXX-X-XX'')
		'
	)