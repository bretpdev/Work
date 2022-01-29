LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
LIBNAME WEBFLSX DBX DATABASE=DNFPUTDL OWNER=WEBFLSX;
PROC SQL;
		SELECT DISTINCT
			COUNT(DISTINCT LNXX.BF_SSN)
		FROM
			PKUB.LNXX_LON LNXX
			JOIN WEBFLSX.WBXX_CSM_USR_ACC WBXX
				ON LNXX.BF_SSN = WBXX.DF_USR_SSN
		WHERE
			LNXX.LC_STA_LONXX = 'R'
			AND LNXX.LA_CUR_PRI > X
			AND WBXX.DF_USR_LST_ATH_DTS > 'XXSEPXXXX'D
			AND WBXX.DF_USR_LST_ATH_DTS < 'XXOCTXXXX'D
	;
QUIT;
