LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
RSUBMIT LEGEND;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
CREATE TABLE NHXXXX AS
	SELECT DISTINCT	LNXX.BF_SSN,LNXX.LF_DOE_SCL_ORG
	FROM PKUB.LNXX_LON LNXX
	WHERE LNXX.LF_DOE_SCL_ORG IN ('XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX');
QUIT;
ENDRSUBMIT;
