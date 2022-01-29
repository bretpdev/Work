LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

PROC SQL;
		SELECT DISTINCT
			COUNT(DISTINCT LNXX.BF_SSN)
		FROM
			PKUB.LNXX_LON LNXX
		WHERE
			LNXX.LC_STA_LONXX = 'R'
			AND LNXX.LA_CUR_PRI > X
	;
QUIT;