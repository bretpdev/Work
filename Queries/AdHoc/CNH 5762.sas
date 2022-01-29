LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE AYXX AS
		SELECT
			BF_SSN, 
			LN_ATY_SEQ, 
			LC_STA_ACTYXX, 
			LF_LST_DTS_AYXX, 
			PF_RSP_ACT
		FROM
			PKUB.AYXX_BR_LON_ATY
		WHERE
			LD_ATY_REQ_RCV = 'XXSEPXXXX'D
			AND LF_USR_REQ_ATY = 'TXXGX'
			AND LD_ATY_RSP = 'XXAUGXXXX'D
	;
QUIT;
ENDRSUBMIT;

PROC EXPORT
		DATA=LEGEND.AYXX
		OUTFILE='T:\SAS\AYXX Query.XLSX'
		REPLACE;
QUIT;
